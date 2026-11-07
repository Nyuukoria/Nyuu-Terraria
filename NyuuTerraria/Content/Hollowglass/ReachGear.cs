using System;
using Microsoft.Xna.Framework;
using Terraria;
using Terraria.GameContent.ItemDropRules;
using Terraria.ID;
using Terraria.ModLoader;
using NyuuTerraria.Content.AmberGrotto;
using NyuuTerraria.Content.Skylight;

namespace NyuuTerraria.Content.Hollowglass;

public class GlassCrawler : GrottoEnemy
{
    protected override int VanillaAI => NPCID.CochinealBeetle;
    protected override int Health => 82;
    protected override int ContactDamage => 22;
    protected override int Defense => 8;
    protected override float SpawnWeight => 0.65f;
    public override float SpawnChance(NPC.Spawner info) =>
        info.SpawnTileType == ModContent.TileType<GlassSlateTile>() ? SpawnWeight : 0f;
    public override void ModifyNPCLoot(NPCLoot loot) =>
        loot.Add(ItemDropRule.Common(ModContent.ItemType<HollowglassOre>(), 2, 1, 3));
}

public class GlasswingMoth : GrottoEnemy
{
    protected override int VanillaAI => NPCID.CaveBat;
    protected override int Health => 65;
    protected override int ContactDamage => 20;
    protected override int Defense => 5;
    protected override float SpawnWeight => 0.55f;
    public override float SpawnChance(NPC.Spawner info) =>
        info.SpawnTileType == ModContent.TileType<GlassSlateTile>() ? SpawnWeight : 0f;
    public override void ModifyNPCLoot(NPCLoot loot) =>
        loot.Add(ItemDropRule.Common(ModContent.ItemType<HollowglassOre>(), 2, 1, 2));
}

public class GlassedgeBlade : SkylightSaber
{
    public override void SetDefaults()
    {
        base.SetDefaults();
        Item.damage = 31;
        Item.shoot = ModContent.ProjectileType<GlassedgeBolt>();
        Item.shootSpeed = 9f;
    }

    public override void ModifyShootStats(Player player, ref Vector2 position, ref Vector2 velocity,
        ref int type, ref int damage, ref float knockback) => damage = damage * 2 / 3;

    public override void OnHitNPC(Player player, NPC target, NPC.HitInfo hit, int damageDone) { }

    public override void AddRecipes() => CreateRecipe().AddIngredient<HollowglassBar>(8)
        .AddIngredient(ItemID.Emerald, 2).AddTile(TileID.Anvils).Register();
}

public class GlassedgeBolt : SkylightSpell
{
    protected override Color Tint => new Color(118, 222, 220);
    protected override int Hits => 2;
    public override void SetDefaults()
    {
        base.SetDefaults();
        Projectile.DamageType = DamageClass.Melee;
        Projectile.timeLeft = 50;
    }
}

public class HollowchimeStaff : SkylightStaff
{
    protected override int Damage => 56;
    protected override int Mana => 6;
    protected override int CastTime => 18;
    protected override int Spell => ModContent.ProjectileType<HollowchimeBolt>();
    public override void AddRecipes() => CreateRecipe().AddIngredient<ResonantClapper>(5)
        .AddIngredient<HollowglassBar>(10).AddIngredient(ItemID.FallenStar, 3).AddTile(TileID.Anvils).Register();
}

public class HollowchimeBolt : SkylightSpell
{
    protected override Color Tint => new Color(152, 201, 246);
    protected override int Hits => 2;
    public override void AI()
    {
        base.AI();
        NPC closest = null;
        float distance = 300f;
        foreach (NPC npc in Main.ActiveNPCs)
        {
            if (!npc.CanBeChasedBy(Projectile) || !Collision.CanHitLine(Projectile.position,
                Projectile.width, Projectile.height, npc.position, npc.width, npc.height))
                continue;
            float candidate = Projectile.Distance(npc.Center);
            if (candidate < distance)
            {
                closest = npc;
                distance = candidate;
            }
        }
        if (closest != null)
            Projectile.velocity = Vector2.Lerp(Projectile.velocity,
                (closest.Center - Projectile.Center).SafeNormalize(Vector2.UnitX) * 10f, 0.045f);
    }
}

public class ResonanceCharm : ModItem
{
    public override void SetDefaults()
    {
        Item.width = 26;
        Item.height = 28;
        Item.accessory = true;
        Item.defense = 2;
        Item.rare = ItemRarityID.Green;
    }
    public override void UpdateAccessory(Player player, bool hideVisual)
    {
        player.statManaMax2 += 20;
        player.GetCritChance(DamageClass.Magic) += 4f;
    }
    public override void AddRecipes() => CreateRecipe().AddIngredient<PrismHeart>(4)
        .AddIngredient<HollowglassBar>(4).AddTile(TileID.Anvils).Register();
}

public class PocketChime : GrottoPetItem
{
    protected override int Pet => ModContent.ProjectileType<LittleGlassBell>();
    protected override int Buff => ModContent.BuffType<LittleGlassBellBuff>();
    public override void AddRecipes() => CreateRecipe().AddIngredient<ResonantClapper>(2)
        .AddIngredient<HollowglassBar>(3).AddTile(TileID.Anvils).Register();
}

public class LittleGlassBellBuff : GrottoPetBuff
{
    protected override int Pet => ModContent.ProjectileType<LittleGlassBell>();
    protected override string PetTexture => ModContent.GetInstance<PocketChime>().Texture;
}

public class LittleGlassBell : GrottoPet
{
    protected override int PetBuff => ModContent.BuffType<LittleGlassBellBuff>();
    protected override Vector3 Glow => new Vector3(0.15f, 0.36f, 0.42f);
    protected override float FollowHeight => -42f;
}
