using Microsoft.Xna.Framework;
using Terraria;
using Terraria.ID;
using Terraria.ModLoader;
using NyuuTerraria.Content.Skylight;

namespace NyuuTerraria.Content.AmberGrotto;

public class VerdigrisOreTile : VeinTile
{
    protected override Color MapColor => new Color(84, 148, 124);
}

public class EmberAmberOreTile : VeinTile
{
    protected override Color MapColor => new Color(196, 133, 64);

    public override void SetStaticDefaults()
    {
        base.SetStaticDefaults();
        Main.tileLighted[Type] = true;
    }

    public override void ModifyLight(int i, int j, ref float r, ref float g, ref float b)
    {
        r = 0.22f;
        g = 0.12f;
        b = 0.025f;
    }
}

public class VerdigrisOre : VeinOre
{
    protected override int OreTile => ModContent.TileType<VerdigrisOreTile>();
}

public class EmberAmberOre : VeinOre
{
    protected override int OreTile => ModContent.TileType<EmberAmberOreTile>();
}

public class VerdigrisBar : VeinBar
{
    public override void AddRecipes()
    {
        CreateRecipe().AddIngredient<VerdigrisOre>(3).AddTile(TileID.Furnaces).Register();
        CreateRecipe().AddRecipeGroup("IronBar").AddIngredient(ItemID.CopperBar)
            .AddIngredient(ItemID.GlowingMushroom, 2).AddTile(TileID.Furnaces).Register();
    }
}

public class EmberAmberBar : VeinBar
{
    public override void AddRecipes()
    {
        CreateRecipe().AddIngredient<EmberAmberOre>(3).AddTile(TileID.Furnaces).Register();
        CreateRecipe().AddIngredient(ItemID.Amber).AddIngredient(ItemID.GoldBar)
            .AddTile(TileID.Furnaces).Register();
        CreateRecipe().AddIngredient(ItemID.Amber).AddIngredient(ItemID.PlatinumBar)
            .AddTile(TileID.Furnaces).Register();
    }
}

public class VerdigrisPickaxe : ModItem
{
    public override void SetDefaults()
    {
        Item.CloneDefaults(ItemID.GoldPickaxe);
        Item.damage = 10;
        Item.pick = 60;
        Item.useTime = 13;
        Item.useAnimation = 19;
        Item.tileBoost = 1;
        Item.rare = ItemRarityID.Blue;
    }

    public override void AddRecipes() => CreateRecipe().AddIngredient<VerdigrisBar>(8)
        .AddRecipeGroup("Wood", 4).AddTile(TileID.Anvils).Register();
}

public class GrottoLantern : ModItem
{
    public override void SetDefaults()
    {
        Item.width = 24;
        Item.height = 26;
        Item.accessory = true;
        Item.rare = ItemRarityID.Blue;
        Item.value = Item.sellPrice(silver: 50);
    }

    public override void UpdateAccessory(Player player, bool hideVisual)
    {
        player.pickSpeed -= 0.12f;
        Lighting.AddLight(player.Center, 0.32f, 0.83f, 0.63f);
    }

    public override void AddRecipes() => CreateRecipe().AddIngredient<VerdigrisBar>(4)
        .AddIngredient(ItemID.Glowstick, 6).AddTile(TileID.Anvils).Register();
}

public class AmberheartPendant : ModItem
{
    public override void SetDefaults()
    {
        Item.width = 22;
        Item.height = 28;
        Item.accessory = true;
        Item.rare = ItemRarityID.Green;
        Item.defense = 2;
        Item.value = Item.sellPrice(silver: 65);
    }

    public override void UpdateAccessory(Player player, bool hideVisual) => player.lifeRegen += 2;

    public override void AddRecipes() => CreateRecipe().AddIngredient<EmberAmberBar>(5)
        .AddIngredient(ItemID.Chain, 2).AddTile(TileID.Anvils).Register();
}

public class DeepheartPendant : AmberheartPendant
{
    public override void SetDefaults()
    {
        base.SetDefaults();
        Item.defense = 5;
        Item.rare = ItemRarityID.LightRed;
        Item.value = Item.sellPrice(gold: 2);
    }

    public override void UpdateAccessory(Player player, bool hideVisual)
    {
        player.lifeRegen += 3;
        player.endurance += 0.05f;
    }

    public override void AddRecipes() => CreateRecipe().AddIngredient<AmberheartPendant>()
        .AddIngredient(ItemID.SoulofNight, 8).AddIngredient(ItemID.SoulofLight, 8)
        .AddIngredient<EmberAmberBar>(5).AddTile(TileID.MythrilAnvil).Register();
}

public class AmberflareStaff : SkylightStaff
{
    protected override int Damage => 29;
    protected override int Mana => 5;
    protected override int CastTime => 20;
    protected override int Spell => ModContent.ProjectileType<AmberflareBolt>();

    public override void SetDefaults()
    {
        base.SetDefaults();
        Item.width = 52;
        Item.height = 54;
    }

    public override void AddRecipes() => CreateRecipe().AddIngredient<EmberAmberBar>(7)
        .AddIngredient<VerdigrisBar>(3).AddIngredient(ItemID.Torch, 8).AddTile(TileID.Anvils).Register();
}

public class AmberflareBolt : SkylightSpell
{
    protected override Color Tint => new Color(255, 164, 72);
    protected override int Hits => 2;

    public override void OnHitNPC(NPC target, NPC.HitInfo hit, int damageDone) =>
        target.AddBuff(BuffID.OnFire, 180);
}
