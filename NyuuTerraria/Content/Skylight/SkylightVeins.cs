using Microsoft.Xna.Framework;
using Terraria;
using Terraria.ID;
using Terraria.ModLoader;

namespace NyuuTerraria.Content.Skylight;

public abstract class VeinTile : ModTile
{
    protected abstract Color MapColor { get; }

    public override void SetStaticDefaults()
    {
        Main.tileSolid[Type] = true;
        Main.tileBlockLight[Type] = true;
        Main.tileSpelunker[Type] = true;
        Main.tileOreFinderPriority[Type] = 240;
        Main.tileMergeDirt[Type] = true;
        TileID.Sets.Ore[Type] = true;
        DustType = DustID.Silver;
        AddMapEntry(MapColor, CreateMapEntryName());
    }
}

public class StormglassOreTile : VeinTile
{
    protected override Color MapColor => new Color(163, 115, 205);
}

public class FrostveilOreTile : VeinTile
{
    protected override Color MapColor => new Color(143, 192, 224);
}

public abstract class VeinOre : ModItem
{
    protected abstract int OreTile { get; }

    public override void SetDefaults()
    {
        Item.DefaultToPlaceableTile(OreTile);
        Item.width = 22;
        Item.height = 22;
        Item.value = Item.sellPrice(copper: 95);
    }
}

public class StormglassOre : VeinOre
{
    protected override int OreTile => ModContent.TileType<StormglassOreTile>();
}

public class FrostveilOre : VeinOre
{
    protected override int OreTile => ModContent.TileType<FrostveilOreTile>();
}

public abstract class VeinBar : ModItem
{
    public override void SetDefaults()
    {
        Item.width = 26;
        Item.height = 18;
        Item.maxStack = Item.CommonMaxStack;
        Item.value = Item.sellPrice(silver: 5);
        Item.rare = ItemRarityID.Blue;
    }
}

public class StormglassBar : VeinBar
{
    public override void AddRecipes()
    {
        CreateRecipe().AddIngredient<StormglassOre>(3).AddTile(TileID.Furnaces).Register();
        CreateRecipe().AddIngredient<SkylightBar>().AddIngredient(ItemID.DemoniteBar)
            .AddIngredient(ItemID.Cloud, 2).AddTile(TileID.Furnaces).Register();
    }
}

public class FrostveilBar : VeinBar
{
    public override void AddRecipes()
    {
        CreateRecipe().AddIngredient<FrostveilOre>(3).AddTile(TileID.Furnaces).Register();
        CreateRecipe().AddIngredient<SkylightBar>().AddIngredient(ItemID.IceBlock, 6)
            .AddTile(TileID.Furnaces).Register();
    }
}

public class StormglassScepter : SkylightStaff
{
    protected override int Damage => 34;
    protected override int Mana => 4;
    protected override int CastTime => 15;
    protected override int Spell => ModContent.ProjectileType<StormglassArc>();

    public override void HoldItem(Player player) => player.manaRegenBonus += 12;

    public override void AddRecipes() => CreateRecipe().AddIngredient<StormglassBar>(8)
        .AddIngredient(ItemID.Cloud, 12).AddIngredient(ItemID.FallenStar, 2)
        .AddTile(TileID.Anvils).Register();
}

public class StormglassArc : SkylightSpell
{
    protected override Color Tint => new Color(198, 143, 247);
    protected override int Hits => 3;

    public override void OnHitNPC(NPC target, NPC.HitInfo hit, int damageDone) =>
        target.AddBuff(BuffID.Frostburn, 120);
}

public class FrostveilBlade : ModItem
{
    public override void SetDefaults()
    {
        Item.width = 44;
        Item.height = 46;
        Item.damage = 38;
        Item.DamageType = DamageClass.Melee;
        Item.useStyle = ItemUseStyleID.Swing;
        Item.useTime = Item.useAnimation = 22;
        Item.knockBack = 5f;
        Item.autoReuse = true;
        Item.UseSound = SoundID.Item1;
        Item.shoot = ModContent.ProjectileType<FrostveilCrescent>();
        Item.shootSpeed = 8.5f;
        Item.rare = ItemRarityID.Blue;
        Item.value = Item.sellPrice(silver: 90);
    }

    public override void ModifyShootStats(Player player, ref Vector2 position, ref Vector2 velocity,
        ref int type, ref int damage, ref float knockback) => damage = damage * 2 / 3;

    public override void AddRecipes() => CreateRecipe().AddIngredient<FrostveilBar>(9)
        .AddIngredient(ItemID.Cloud, 10).AddTile(TileID.Anvils).Register();
}

public class FrostveilCrescent : SkylightSpell
{
    protected override Color Tint => new Color(146, 216, 255);
    protected override int Hits => 2;

    public override void SetDefaults()
    {
        base.SetDefaults();
        Projectile.DamageType = DamageClass.Melee;
        Projectile.timeLeft = 45;
    }

    public override void OnHitNPC(NPC target, NPC.HitInfo hit, int damageDone) =>
        target.AddBuff(BuffID.Frostburn, 150);
}
