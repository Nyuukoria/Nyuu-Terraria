using System.Collections.Generic;
using System.Linq;
using Terraria;
using Terraria.GameContent;
using Terraria.ID;
using Terraria.Localization;
using Terraria.ModLoader;

namespace NyuuTerraria.Content.Backpacks;

public abstract class BackpackItem : ModItem
{
    public abstract int Capacity { get; }
    protected abstract int Material { get; }
    protected virtual int MaterialCount => 12;
    protected virtual int PreviousBackpack => ItemID.None;

    public override void SetDefaults()
    {
        Item.width = 28;
        Item.height = 30;
        Item.accessory = true;
        Item.rare = ItemRarityID.Blue;
        Item.value = Item.sellPrice(silver: 40 + Capacity);
    }

    public override bool CanEquipAccessory(Player player, int slot, bool modded) => modded;

    public override void AddRecipes()
    {
        Recipe recipe = CreateRecipe().AddIngredient(Material, MaterialCount).AddTile(TileID.Anvils);
        if (PreviousBackpack == ItemID.None)
            recipe.AddRecipeGroup("NyuuTerraria:AnyChest");
        else
            recipe.AddIngredient(PreviousBackpack)
                .AddCondition(Language.GetOrRegister("Mods.NyuuTerraria.Conditions.BackpackEmpty",
                    () => "Backpack storage must be empty"),
                    () => Main.LocalPlayer.GetModPlayer<BackpackStorage>().IsEmpty);
        recipe.Register();
    }
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
    protected override int PreviousBackpack => ModContent.ItemType<CopperBackpack>();
}

public class GoldBackpack : BackpackItem
{
    public override int Capacity => 40;
    protected override int Material => ItemID.GoldBar;
    protected override int PreviousBackpack => ModContent.ItemType<IronBackpack>();
}

public class EmeraldBackpack : BackpackItem
{
    public override int Capacity => 60;
    protected override int Material => ItemID.Emerald;
    protected override int MaterialCount => 8;
    protected override int PreviousBackpack => ModContent.ItemType<GoldBackpack>();
}

public class ObsidianBackpack : BackpackItem
{
    public override int Capacity => 80;
    protected override int Material => ItemID.Obsidian;
    protected override int MaterialCount => 30;
    protected override int PreviousBackpack => ModContent.ItemType<EmeraldBackpack>();
}

public class DemoniteBackpack : BackpackItem
{
    public override int Capacity => 100;
    protected override int Material => ItemID.DemoniteBar;
    protected override int MaterialCount => 20;
    protected override int PreviousBackpack => ModContent.ItemType<ObsidianBackpack>();
}

public class BackpackSlot : ModAccessorySlot
{
    // Preserve the existing saved slot while rendering it in the Equipment tab.
    public override bool IsHidden() => Main.EquipPage != 2;
    public override Microsoft.Xna.Framework.Vector2? CustomLocation =>
        new Microsoft.Xna.Framework.Vector2(Main.screenWidth - 92, AccessorySlotLoader.DrawVerticalAlignment + 5 * 47);
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
    public override void AddRecipeGroups()
    {
        int[] chests = ContentSamples.ItemsByType.Values
            .Where(item => item.createTile >= 0 && TileID.Sets.BasicChest[item.createTile])
            .Select(item => item.type)
            .Append(ItemID.Chest)
            .Distinct()
            .OrderBy(type => type == ItemID.Chest ? 0 : 1)
            .ThenBy(type => type)
            .ToArray();
        RecipeGroup.Register("NyuuTerraria:AnyChest", () => "Any Chest", chests);
    }

    public override void AddRecipes()
    {
        Recipe.Create(ItemID.Leather).AddIngredient(ItemID.Gel, 8)
            .AddIngredient(ItemID.Silk).AddTile(TileID.WorkBenches).Register();
    }
}
