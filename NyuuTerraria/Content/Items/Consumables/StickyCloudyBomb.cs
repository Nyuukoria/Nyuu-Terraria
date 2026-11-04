using NyuuTerraria.Content.Projectiles;
using Terraria;
using Terraria.ID;
using Terraria.ModLoader;

namespace NyuuTerraria.Content.Items.Consumables;

public class StickyCloudyBomb : CloudyBomb
{
    public override void SetDefaults()
    {
        base.SetDefaults();
        Item.shoot = ModContent.ProjectileType<StickyCloudyBombProjectile>();
    }

    public override void AddRecipes()
    {
        CreateRecipe()
            .AddIngredient<CloudyBomb>()
            .AddIngredient(ItemID.Gel)
            .Register();
    }
}
