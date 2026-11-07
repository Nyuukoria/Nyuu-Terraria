using System.Collections.Generic;
using Terraria;
using Terraria.ID;
using Terraria.ModLoader;

namespace NyuuTerraria.Content.Backpacks;

public abstract class BackpackItem : ModItem
{
    public abstract int Capacity { get; }
    protected abstract int Material { get; }
    protected virtual int MaterialCount => 12;

    public override void SetDefaults()
    {
        Item.width = 28;
        Item.height = 30;
        Item.accessory = true;
        Item.rare = ItemRarityID.Blue;
        Item.value = Item.sellPrice(silver: 40 + Capacity);
    }

    public override bool CanEquipAccessory(Player player, int slot, bool modded) => modded;

    public override void AddRecipes() => CreateRecipe()
        .AddIngredient(Material, MaterialCount)
        .AddIngredient(ItemID.Leather, 2)
        .AddIngredient(ItemID.Silk, 4)
        .AddTile(TileID.Anvils)
        .Register();
}

public class CopperBackpack : BackpackItem
{
    public override int Capacity => 20;
    protected override int Material => ItemID.CopperBar;
}

public class IronBackpack : BackpackItem
{
    public override int Capacity => 30;
    protected override int Material => ItemID.IronBar;
}

public class GoldBackpack : BackpackItem
{
    public override int Capacity => 40;
    protected override int Material => ItemID.GoldBar;
}

public class EmeraldBackpack : BackpackItem
{
    public override int Capacity => 60;
    protected override int Material => ItemID.Emerald;
    protected override int MaterialCount => 8;
}

public class ObsidianBackpack : BackpackItem
{
    public override int Capacity => 80;
    protected override int Material => ItemID.Obsidian;
    protected override int MaterialCount => 30;
}

public class DemoniteBackpack : BackpackItem
{
    public override int Capacity => 100;
    protected override int Material => ItemID.DemoniteBar;
    protected override int MaterialCount => 20;
}

public class BackpackSlot : ModAccessorySlot
{
    public override string FunctionalTexture => "NyuuTerraria/Content/Backpacks/BackpackSlot";
    public override bool HasEquipmentLoadoutSupport => false;
    public override bool DrawVanitySlot => false;
    public override bool DrawDyeSlot => false;
    public override bool CanAcceptItem(Item item, AccessorySlotType context) =>
        context == AccessorySlotType.FunctionalSlot && item.ModItem is BackpackItem;
    public override void OnMouseHover(AccessorySlotType context) => Main.hoverItemName = "Backpack";


}

public class BackpackRecipes : ModSystem
{
    public override void AddRecipes()
    {
        Recipe.Create(ItemID.Leather).AddIngredient(ItemID.Gel, 8)
            .AddIngredient(ItemID.Silk).AddTile(TileID.WorkBenches).Register();
    }
}
