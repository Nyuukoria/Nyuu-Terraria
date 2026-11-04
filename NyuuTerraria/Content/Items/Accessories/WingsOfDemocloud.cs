using NyuuTerraria.Content.Rarities;
using Terraria;
using Terraria.DataStructures;
using Terraria.ID;
using Terraria.ModLoader;

namespace NyuuTerraria.Content.Items.Accessories;

[AutoloadEquip(EquipType.Wings)]
public class WingsOfDemocloud : ModItem
{
    public override void SetStaticDefaults()
    {
        ArmorIDs.Wing.Sets.Stats[Item.wingSlot] = new WingStats(60, 5.5f, 1.15f);
    }

    public override void SetDefaults()
    {
        Item.width = 34;
        Item.height = 30;
        Item.accessory = true;
        Item.rare = ModContent.RarityType<DemocloudRarity>();
        Item.defense = 3;
        Item.value = Item.sellPrice(silver: 80);
    }

    public override void AddRecipes()
    {
        CreateRecipe()
            .AddIngredient(ItemID.Cloud, 20)
            .AddIngredient(ItemID.DemoniteBar, 10)
            .AddTile(TileID.Anvils)
            .Register();
    }
}
