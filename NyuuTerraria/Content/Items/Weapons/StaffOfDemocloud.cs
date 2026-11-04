using NyuuTerraria.Content.Projectiles;
using NyuuTerraria.Content.Rarities;
using Terraria;
using Terraria.ID;
using Terraria.ModLoader;

namespace NyuuTerraria.Content.Items.Weapons;

public class StaffOfDemocloud : ModItem
{
    public override void SetStaticDefaults()
    {
        Item.staff[Type] = true;
    }

    public override void SetDefaults()
    {
        Item.width = 52;
        Item.height = 54;
        Item.damage = 50;
        Item.DamageType = DamageClass.Magic;
        Item.crit = 26; // Added to the player's default 4% magic critical chance.
        Item.mana = 4;
        Item.knockBack = 5f;
        Item.useTime = 12;
        Item.useAnimation = 12;
        Item.useStyle = ItemUseStyleID.Shoot;
        Item.noMelee = true;
        Item.autoReuse = true;
        Item.UseSound = SoundID.Item43;
        Item.shoot = ModContent.ProjectileType<DemocloudComet>();
        Item.shootSpeed = 12f;
        Item.rare = ModContent.RarityType<DemocloudRarity>();
        Item.value = Item.sellPrice(gold: 2);
    }

    public override void HoldItem(Player player)
    {
        player.manaRegenBonus += 30;
    }

    public override void AddRecipes()
    {
        CreateRecipe()
            .AddIngredient(ItemID.DemoniteBar, 25)
            .AddIngredient(ItemID.Cloud, 50)
            .AddIngredient(ItemID.Wood, 15)
            .AddTile(TileID.Anvils)
            .Register();
    }
}
