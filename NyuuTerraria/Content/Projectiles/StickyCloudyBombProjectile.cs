using Microsoft.Xna.Framework;
using NyuuTerraria.Content.Items.Consumables;
using Terraria.ModLoader;

namespace NyuuTerraria.Content.Projectiles;

public class StickyCloudyBombProjectile : CloudyBombProjectile
{
    public override string Texture => ModContent.GetInstance<StickyCloudyBomb>().Texture;

    public override bool OnTileCollide(Vector2 oldVelocity)
    {
        Projectile.ai[0] = 1f;
        Projectile.velocity = Vector2.Zero;
        Projectile.netUpdate = true;
        return false;
    }
}
