using Terraria;
using Terraria.GameContent.Bestiary;
using Terraria.GameContent.ItemDropRules;
using Terraria.ID;
using Terraria.ModLoader;

namespace NyuuTerraria.Content.AmberGrotto;

public abstract class GrottoEnemy : ModNPC
{
    protected abstract int VanillaAI { get; }
    protected abstract int Health { get; }
    protected abstract int ContactDamage { get; }
    protected abstract int Defense { get; }
    protected abstract float SpawnWeight { get; }

    public override void SetDefaults()
    {
        NPC.CloneDefaults(VanillaAI);
        AIType = VanillaAI;
        NPC.width = 30;
        NPC.height = 24;
        NPC.lifeMax = Health;
        NPC.damage = ContactDamage;
        NPC.defense = Defense;
    }

    public override float SpawnChance(NPC.Spawner info)
    {
        if (!(info.Player.ZoneDirtLayerHeight || info.Player.ZoneRockLayerHeight) ||
            info.SpawnTileType != ModContent.TileType<MossSlateTile>())
            return 0f;

        return SpawnWeight;
    }

    public override void SetBestiary(BestiaryDatabase database, BestiaryEntry entry) =>
        entry.Info.Add(new FlavorTextBestiaryInfoElement(
            "Mods.NyuuTerraria.Bestiary." + Name));
}

public class MossbackBeetle : GrottoEnemy
{
    protected override int VanillaAI => NPCID.CochinealBeetle;
    protected override int Health => 65;
    protected override int ContactDamage => 19;
    protected override int Defense => 7;
    protected override float SpawnWeight => 0.75f;

    public override void ModifyNPCLoot(NPCLoot loot)
    {
        loot.Add(ItemDropRule.Common(ModContent.ItemType<VerdigrisOre>(), 2, 1, 3));
        loot.Add(ItemDropRule.Common(ModContent.ItemType<GlowcapSpore>(), 14));
    }
}

public class LanternMoth : GrottoEnemy
{
    protected override int VanillaAI => NPCID.CaveBat;
    protected override int Health => 48;
    protected override int ContactDamage => 16;
    protected override int Defense => 3;
    protected override float SpawnWeight => 0.60f;

    public override void AI()
    {
        NPC.rotation = NPC.velocity.X * 0.06f;
        Lighting.AddLight(NPC.Center, 0.35f, 0.19f, 0.06f);
    }

    public override void ModifyNPCLoot(NPCLoot loot)
    {
        loot.Add(ItemDropRule.Common(ModContent.ItemType<EmberAmberOre>(), 2, 1, 2));
        loot.Add(ItemDropRule.Common(ModContent.ItemType<AmberWispSeed>(), 14));
    }
}
