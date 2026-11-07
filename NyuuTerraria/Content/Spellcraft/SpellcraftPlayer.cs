using System;
using Microsoft.Xna.Framework;
using Terraria;
using Terraria.GameInput;
using Terraria.ID;
using Terraria.ModLoader;
using Terraria.ModLoader.IO;
using NyuuTerraria.Content.Reverie;

namespace NyuuTerraria.Content.Spellcraft;

public static class Spells
{
    public const int Arcane = 0, Ember = 1, Frost = 2, Tide = 3, Mend = 4, Storm = 5, Ward = 6, Count = 7;
    public const int MaximumExperience = 7500;
    public static readonly string[] Names = { "Arcane Thread", "Emberbloom", "Frostbloom", "Tideglass", "Dawnmend", "Stormglass Fork", "Crescent Ward" };
    public static readonly string[] Icons = { "ArcaneMote", "EmberSeed", "FrostShard", "TideOrb", "DawnPetal", "StormSpark", "WardSigil" };
    public static readonly Color[] Colors = { new(201,145,255), new(255,140,67), new(139,225,255), new(78,205,228), new(255,186,218), new(137,215,255), new(184,155,255) };
    public static readonly int[] Damage = { 36,49,31,40,0,46 };
    public static readonly int[] Mana = { 4,7,5,6,18,6 };
    public static readonly int[] UseTime = { 12,20,15,22,30,16 };
    public static int ExperienceForLevel(int level) => 75 * level * level;
}

public class SpellcraftPlayer : ModPlayer
{
    internal bool[] Learned;
    internal int[] Experience;
    internal int SelectedSpell;
    internal int MendCooldown;
    internal bool EfficientWard;
    private ulong[,] lastHit;

    public override void Initialize()
    {
        Learned = new bool[Spells.Count]; Experience = new int[Spells.Count];
        Learned[Spells.Arcane] = Learned[Spells.Ward] = true;
        lastHit = new ulong[Spells.Count, Main.maxNPCs];
        SelectedSpell = MendCooldown = 0;
    }
    public override void ResetEffects() => EfficientWard = false;
    public int StoredLevel(int spell) => Math.Min(10,(int)Math.Sqrt(Experience[spell]/75d));
    public int LevelCap => Main.hardMode ? 10 : 5;
    public int EffectiveLevel(int spell) => Math.Min(LevelCap,StoredLevel(spell));
    public int WardManaCost => EfficientWard ? 1 : 2;
    internal bool Select(int spell)
    {
        if (spell < 0 || spell >= Spells.Ward || !Learned[spell]) return false;
        SelectedSpell = spell; return true;
    }
    internal bool Learn(int spell)
    {
        if(spell < 0 || spell >= Spells.Count || Learned[spell]) return false;
        Learned[spell] = true; return true;
    }
    internal void GainExperience(int spell, int amount)
    {
        if(spell < 0 || spell >= Spells.Count || !Learned[spell] || amount <= 0) return;
        int cap = Spells.ExperienceForLevel(LevelCap);
        // A character visiting a pre-Hardmode world keeps previously earned Hardmode mastery.
        if(Experience[spell] >= cap) return;
        int before = EffectiveLevel(spell);
        Experience[spell] = Math.Min(cap,Experience[spell]+Math.Min(amount,25));
        if(EffectiveLevel(spell)>before && !Main.dedServ && Player.whoAmI==Main.myPlayer)
            Main.NewText($"{Spells.Names[spell]} mastery {EffectiveLevel(spell)}",Spells.Colors[spell]);
    }
    internal void RecordHit(int spell, NPC target, int damage)
    {
        if(Player.whoAmI!=Main.myPlayer || target.friendly || target.SpawnedFromStatue || target.lifeMax<10 || damage<=0 ||
            spell < 0 || spell >= Spells.Count || target.whoAmI < 0 || target.whoAmI >= Main.maxNPCs) return;
        ulong previous=lastHit[spell,target.whoAmI];
        ulong now=Main.GameUpdateCount+1;
        if(previous>0 && now>=previous && now-previous<15) return;
        lastHit[spell,target.whoAmI]=now;
        GainExperience(spell,Math.Clamp(damage/10,2,8));
    }
    internal bool TryMend()
    {
        if(!Learned[Spells.Mend] || MendCooldown>0 || Player.statLife>=Player.statLifeMax2 || Player.dead) return false;
        int amount=Math.Min(Player.statLifeMax2-Player.statLife,10+EffectiveLevel(Spells.Mend));
        Player.Heal(amount); MendCooldown=480; GainExperience(Spells.Mend,10);
        return true;
    }
    public override void PostUpdate()
    {
        if(MendCooldown>0) MendCooldown--;
    }
    public override void ProcessTriggers(TriggersSet triggersSet)
    {
        if(SpellcraftKeys.Spellbook?.JustPressed!=true || Main.gameMenu || Main.drawingPlayerChat || Main.editSign || Main.editChest) return;
        bool close=ChronicleInterface.Open && ChronicleInterface.Tab==3;
        ChronicleInterface.Tab=3; ChronicleInterface.Open=!close;
    }
    public override void SaveData(TagCompound tag)
    {
        int mask=0;for(int i=0;i<Spells.Count;i++) if(Learned[i]) mask|=1<<i;
        tag["spellcraftKnown"]=mask;tag["spellcraftXP"]=Experience;tag["spellcraftSelected"]=SelectedSpell;
        tag["spellcraftMendCooldown"]=MendCooldown;
    }
    public override void LoadData(TagCompound tag)
    {
        Initialize();int mask=tag.GetInt("spellcraftKnown");int[] xp=tag.GetIntArray("spellcraftXP");
        for(int i=0;i<Spells.Count;i++)
        {
            Learned[i]|=(mask&(1<<i))!=0;
            Experience[i]=i<xp.Length && Learned[i] ? Math.Clamp(xp[i],0,Spells.MaximumExperience) : 0;
        }
        SelectedSpell=Math.Clamp(tag.GetInt("spellcraftSelected"),0,Spells.Ward-1);
        if(!Learned[SelectedSpell]) SelectedSpell=Spells.Arcane;
        MendCooldown=Math.Clamp(tag.GetInt("spellcraftMendCooldown"),0,480);
    }
}

public class SpellcraftKeys : ModSystem
{
    internal static ModKeybind Spellbook;
    public override void Load() { if(!Main.dedServ) Spellbook=KeybindLoader.RegisterKeybind(Mod,"Spellbook","K"); }
    public override void Unload() => Spellbook=null;
}
