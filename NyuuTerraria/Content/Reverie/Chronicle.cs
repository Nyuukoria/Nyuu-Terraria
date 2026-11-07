using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Text.Json;
using Microsoft.Xna.Framework;
using Terraria;
using Terraria.DataStructures;
using Terraria.GameInput;
using Terraria.ID;
using Terraria.ModLoader;
using Terraria.ModLoader.IO;

namespace NyuuTerraria.Content.Reverie;

public sealed class QuestDefinition
{
    public string Id { get; set; } = "";
    public string Title { get; set; } = "";
    public string Description { get; set; } = "";
    public string Requires { get; set; } = "";
    public string Event { get; set; } = "";
    public int Count { get; set; } = 1;
    public int Item { get; set; }
    public string ModItem { get; set; } = "";
    public int Experience { get; set; } = 100;
    public string Reward { get; set; } = "";
    public int RewardCount { get; set; } = 1;
}

// Content lives in JSON so new authored quests and dialogue do not require changing UI code.
public class ChronicleContent : ModSystem
{
    internal static QuestDefinition[] Quests = Array.Empty<QuestDefinition>();
    internal static string[] LyraDialogue = Array.Empty<string>();
    public override void PostSetupContent()
    {
        Quests = JsonSerializer.Deserialize<QuestDefinition[]>(Encoding.UTF8.GetString(Mod.GetFileBytes("Data/quests.json")));
        LyraDialogue = JsonSerializer.Deserialize<string[]>(Encoding.UTF8.GetString(Mod.GetFileBytes("Data/lyra-dialogue.json")));
        foreach (var quest in Quests)
        {
            if (string.IsNullOrWhiteSpace(quest.Id) || quest.Count < 1 || quest.RewardCount < 1 || quest.Experience < 0 ||
                !Mod.TryFind<ModItem>(quest.Reward, out _) ||
                (quest.ModItem.Length > 0 && !Mod.TryFind<ModItem>(quest.ModItem, out _)) ||
                (quest.Requires.Length > 0 && !Quests.Any(q => q.Id == quest.Requires)))
                throw new Exception("Invalid quest definition: " + quest.Id);
        }

        if (Quests.Select(q => q.Id).Distinct().Count() != Quests.Length)
            throw new Exception("Quest IDs must be unique.");
    }
    public override void Unload() { Quests = Array.Empty<QuestDefinition>(); LyraDialogue = Array.Empty<string>(); }
}

public class ChroniclePlayer : ModPlayer
{
    internal static readonly string[] Professions = { "Vanguard", "Ranger", "Arcanist", "Summoner", "Explorer" };
    internal int[] Experience = new int[5];
    internal int[] Skills = new int[5];
    internal int Profession = 4;
    internal string Pronouns = "they/them";
    internal HashSet<string> Completed = new();
    internal Dictionary<string, int> Events = new();
    internal List<TagCompound> Looks = new();
    internal Vector2 ReturnPosition;
    internal string ReturnWorld = "";
    private int explorationTicks;
    public int Level(int branch) => Math.Min(50, (int)Math.Sqrt(Experience[branch] / 50.0));
    internal int Available(int branch) => Level(branch) - Skills[branch];
    public void AddExperience(int amount)
    {
        if (amount <= 0) return;
        Experience[Profession] = Math.Clamp(Experience[Profession] + Math.Min(amount, 5000), 0, 125000);
    }
    internal void Record(string name, int amount = 1)
    {
        Events.TryGetValue(name, out int value);
        Events[name] = Math.Min(1000000, value + amount);
    }
    internal int Progress(QuestDefinition quest)
    {
        int itemType = quest.Item;
        if (!string.IsNullOrEmpty(quest.ModItem) && Mod.TryFind<ModItem>(quest.ModItem, out var item)) itemType = item.Type;
        if (itemType > 0)
            return Player.inventory.Where(i => i.type == itemType).Sum(i => i.stack);
        return Events.TryGetValue(quest.Event, out int count) ? count : 0;
    }
    internal bool Claim(QuestDefinition quest)
    {
        if (Completed.Contains(quest.Id) || (!string.IsNullOrEmpty(quest.Requires) && !Completed.Contains(quest.Requires)) || Progress(quest) < quest.Count) return false;
        int type = quest.Item;
        if (!string.IsNullOrEmpty(quest.ModItem) && Mod.TryFind<ModItem>(quest.ModItem, out var ingredient)) type = ingredient.Type;
        if (type > 0)
        {
            int remaining = quest.Count;
            foreach (Item item in Player.inventory)
            {
                if (item.type != type) continue;
                int take = Math.Min(remaining, item.stack);
                item.stack -= take; remaining -= take;
                if (item.stack <= 0) item.TurnToAir();
                if (remaining == 0) break;
            }
        }
        Completed.Add(quest.Id);
        AddExperience(quest.Experience);
        if (Mod.TryFind<ModItem>(quest.Reward, out var reward))
            Player.QuickSpawnItem(Player.GetSource_Misc("ChronicleQuest"), reward.Type, quest.RewardCount);
        if (!Main.dedServ) Recipe.UpdateRecipeList();
        return true;
    }
    public override void ProcessTriggers(TriggersSet triggersSet)
    {
        if (ChronicleInterface.JournalKey?.JustPressed == true && !Main.drawingPlayerChat && !Main.editSign && !Main.editChest)
            ChronicleInterface.Open = !ChronicleInterface.Open;
    }
    public override void PostUpdate()
    {
        if (Player.whoAmI != Main.myPlayer || Player.dead) return;
        if (++explorationTicks % 180 == 0 && RealmWorld.RealmAt(Player.Center) is int realm && realm >= 0)
        {
            string key = realm == 0 ? "visit-reverie" : "visit-hush";
            if (!Events.ContainsKey(key)) { Record(key); AddExperience(80); }
            if (Profession == 4 && Math.Abs(Player.velocity.X) > 1f) AddExperience(2);
        }
    }
    internal void SyncProfession(int toWho = -1, int ignoreClient = -1)
    {
        if (Main.netMode == NetmodeID.SinglePlayer) return;
        var packet = Mod.GetPacket(); packet.Write((byte)2); packet.Write((byte)Player.whoAmI); packet.Write((byte)Profession);
        for (int i = 0; i < 5; i++) packet.Write((byte)Skills[i]);
        packet.Send(toWho, ignoreClient);
    }
    public override void SyncPlayer(int toWho, int fromWho, bool newPlayer) => SyncProfession(toWho, fromWho);
    public override void PostUpdateEquips()
    {
        // Each branch keeps its own investment. Only the selected profession grants bonuses.
        int rank = Skills[Profession];
        switch (Profession)
        {
            case 0: Player.GetDamage(DamageClass.Melee) += rank * .005f; Player.statDefense += rank / 5; break;
            case 1: Player.GetDamage(DamageClass.Ranged) += rank * .005f; break;
            case 2: Player.GetDamage(DamageClass.Magic) += rank * .005f; Player.manaRegenBonus += rank; break;
            case 3: Player.GetDamage(DamageClass.Summon) += rank * .005f; break;
            case 4: Player.moveSpeed += rank * .004f; Player.pickSpeed -= Math.Min(.15f, rank * .003f); break;
        }
    }
    internal void SaveLook(int index)
    {
        while (Looks.Count <= index) Looks.Add(new TagCompound());
        Looks[index] = new TagCompound { ["hair"] = Player.hair, ["skinVariant"] = Player.skinVariant,
            ["hairColor"] = Player.hairColor, ["skinColor"] = Player.skinColor, ["eyeColor"] = Player.eyeColor,
            ["shirtColor"] = Player.shirtColor, ["underShirtColor"] = Player.underShirtColor,
            ["pantsColor"] = Player.pantsColor, ["shoeColor"] = Player.shoeColor };
    }
    internal void RestoreLook(int index)
    {
        if (Looks.Count <= index || !Looks[index].ContainsKey("hair")) return;
        var look = Looks[index];
        Player.hair = Math.Clamp(look.GetInt("hair"), 0, Terraria.GameContent.TextureAssets.PlayerHair.Length - 1);
        Player.skinVariant = look.GetInt("skinVariant");
        Player.hairColor = look.Get<Color>("hairColor"); Player.skinColor = look.Get<Color>("skinColor");
        Player.eyeColor = look.Get<Color>("eyeColor"); Player.shirtColor = look.Get<Color>("shirtColor");
        Player.underShirtColor = look.Get<Color>("underShirtColor"); Player.pantsColor = look.Get<Color>("pantsColor");
        Player.shoeColor = look.Get<Color>("shoeColor");
        if (Main.netMode == NetmodeID.MultiplayerClient) NetMessage.SendData(MessageID.PlayerInfo, number: Player.whoAmI);
    }
    public override void SaveData(TagCompound tag)
    {
        tag["profession"] = Profession; tag["professionXP"] = Experience; tag["professionSkills"] = Skills;
        tag["pronouns"] = Pronouns; tag["questsCompleted"] = Completed.ToList();
        tag["questEvents"] = Events.Select(pair => new TagCompound { ["id"] = pair.Key, ["count"] = pair.Value }).ToList();
        tag["appearancePresets"] = Looks; tag["realmReturn"] = ReturnPosition; tag["realmWorld"] = ReturnWorld;
    }
    public override void LoadData(TagCompound tag)
    {
        Profession = Math.Clamp(tag.GetInt("profession"), 0, 4);
        var xp = tag.GetIntArray("professionXP"); var skill = tag.GetIntArray("professionSkills");
        for (int i = 0; i < 5; i++)
        {
            Experience[i] = i < xp.Length ? Math.Clamp(xp[i], 0, 125000) : 0;
            Skills[i] = i < skill.Length ? Math.Clamp(skill[i], 0, Level(i)) : 0;
        }
        if (!tag.ContainsKey("profession")) Profession = 4;
        Pronouns = tag.ContainsKey("pronouns") ? tag.GetString("pronouns") : "they/them";
        Completed = tag.GetList<string>("questsCompleted").ToHashSet(); Events.Clear();
        foreach (var entry in tag.GetList<TagCompound>("questEvents")) Events[entry.GetString("id")] = Math.Max(0, entry.GetInt("count"));
        Looks = tag.GetList<TagCompound>("appearancePresets").ToList();
        ReturnPosition = tag.Get<Vector2>("realmReturn"); ReturnWorld = tag.GetString("realmWorld");
    }
}

public class ChronicleJournal : ModItem
{
    public override void SetDefaults() { Item.width = 28; Item.height = 32; Item.useStyle = ItemUseStyleID.HoldUp; Item.useTime = Item.useAnimation = 20; Item.rare = ItemRarityID.Blue; }
    public override bool? UseItem(Player player) { if (player.whoAmI == Main.myPlayer) ChronicleInterface.Open = !ChronicleInterface.Open; return true; }
    public override void AddRecipes() => CreateRecipe().AddIngredient(ItemID.Wood, 8).AddIngredient(ItemID.Silk, 2).AddTile(TileID.WorkBenches).Register();
}
