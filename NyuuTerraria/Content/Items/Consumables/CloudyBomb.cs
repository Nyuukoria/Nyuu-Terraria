using NyuuTerraria.Content.Projectiles;
using Terraria;
using Terraria.ID;
using Terraria.ModLoader;

namespace NyuuTerraria.Content.Items.Consumables;

public class CloudyBomb : ModItem
{
    public override void SetDefaults()
    {
        Item.CloneDefaults(ItemID.Bomb);
        Item.damage = 100;
        Item.shoot = ModContent.ProjectileType<CloudyBombProjectile>();
    }

    public override void AddRecipes()
    {
        CreateRecipe()
            .AddIngredient(ItemID.Bomb)
            .AddIngredient(ItemID.Cloud, 4)
            .AddIngredient(ItemID.SilverBar, 4)
            .Register();
    }
}
