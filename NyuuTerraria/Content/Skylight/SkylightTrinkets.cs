using Microsoft.Xna.Framework;
using Terraria;
using Terraria.ID;
using Terraria.ModLoader;

namespace NyuuTerraria.Content.Skylight;

public abstract class SkyTrinket : ModItem
{
    public override void SetDefaults()
    {
        Item.width = 24;
        Item.height = 24;
        Item.accessory = true;
        Item.rare = ItemRarityID.Blue;
        Item.value = Item.sellPrice(silver: 55);
    }
}

public class GlowglassLens : SkyTrinket
{
    public override void UpdateAccessory(Player player, bool hideVisual)
    {
        player.nightVision = true;
        Lighting.AddLight(player.Center, 0.90f, 0.78f, 0.96f);
    }

    public override void AddRecipes() => CreateRecipe().AddIngredient<SkylightBar>(2)
        .AddIngredient(ItemID.Lens).AddIngredient(ItemID.Glowstick, 3).AddTile(TileID.Anvils).Register();
}

public class PetalBrooch : SkyTrinket
{
    public override void UpdateAccessory(Player player, bool hideVisual)
    {
        player.GetDamage(DamageClass.Magic) += 0.07f;
        player.manaRegenBonus += 15;
    }

    public override void AddRecipes() => CreateRecipe().AddIngredient<SkylightBar>(3)
        .AddIngredient(ItemID.Daybloom, 2).AddTile(TileID.Anvils).Register();
}

public class NimbusCharm : SkyTrinket
{
    public override void UpdateAccessory(Player player, bool hideVisual)
    {
        player.moveSpeed += 0.12f;
        player.jumpSpeedBoost += 0.9f;
    }

    public override void AddRecipes() => CreateRecipe().AddIngredient<WardenCore>(3)
        .AddIngredient(ItemID.Cloud, 8).AddTile(TileID.Anvils).Register();
}

public class UtilityTrinketSlot : ModAccessorySlot
{
    public override bool CanAcceptItem(Item item, AccessorySlotType context) => item.ModItem is GlowglassLens;
}

public class SkyTrinketSlot : ModAccessorySlot
{
    public override bool CanAcceptItem(Item item, AccessorySlotType context) =>
        item.ModItem is PetalBrooch or NimbusCharm;
}
