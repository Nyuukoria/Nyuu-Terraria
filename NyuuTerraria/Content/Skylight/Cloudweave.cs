using Terraria;
using Terraria.ID;
using Terraria.ModLoader;

namespace NyuuTerraria.Content.Skylight;

public class CloudweaveBelt : ModItem
{
    public override void SetDefaults()
    {
        Item.width = 26;
        Item.height = 20;
        Item.accessory = true;
        Item.rare = ItemRarityID.Blue;
    }

    public override void AddRecipes() => CreateRecipe().AddIngredient<SkylightBar>(4)
        .AddIngredient(ItemID.Cloud, 12).AddIngredient(ItemID.Silk, 5)
        .AddTile(TileID.Anvils).Register();
}

public class StarfallPendant : SkyTrinket
{
    public override void UpdateAccessory(Player player, bool hideVisual)
    {
        player.statManaMax2 += 20;
        player.GetCritChance(DamageClass.Magic) += 4f;
    }

    public override void AddRecipes() => CreateRecipe().AddIngredient<StormglassBar>(3)
        .AddIngredient(ItemID.FallenStar, 2).AddTile(TileID.Anvils).Register();
}

public class FrostveilSigil : SkyTrinket
{
    public override void UpdateAccessory(Player player, bool hideVisual)
    {
        player.statDefense += 2;
        player.buffImmune[BuffID.Chilled] = true;
    }

    public override void AddRecipes() => CreateRecipe().AddIngredient<FrostveilBar>(3)
        .AddIngredient(ItemID.SnowBlock, 8).AddTile(TileID.Anvils).Register();
}

public abstract class BeltTrinketSlot : ModAccessorySlot
{
    public override bool IsEnabled()
    {
        for (int i = 3; i < 10; i++)
            if (Player.IsItemSlotUnlockedAndUsable(i) && Player.armor[i].ModItem is CloudweaveBelt)
                return true;

        return false;
    }

    public override bool CanAcceptItem(Item item, AccessorySlotType context) => item.ModItem is SkyTrinket;
}

public class BeltTrinketSlotOne : BeltTrinketSlot { }
public class BeltTrinketSlotTwo : BeltTrinketSlot { }

public class CloudsidePonytail : ModHair
{
    public override bool AvailableDuringCharacterCreation => true;
    public override Gender RandomizedCharacterCreationGender => Gender.Female;
    public override string AltTexture => Texture + "_Alt";
}

public class Moonbuns : ModHair
{
    public override bool AvailableDuringCharacterCreation => true;
    public override Gender RandomizedCharacterCreationGender => Gender.Female;
    public override string AltTexture => Texture + "_Alt";
}

public class LavenderBraid : ModHair
{
    public override bool AvailableDuringCharacterCreation => true;
    public override Gender RandomizedCharacterCreationGender => Gender.Female;
    public override string AltTexture => Texture + "_Alt";
}
