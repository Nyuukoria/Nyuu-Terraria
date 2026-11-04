using Terraria.ModLoader;

namespace NyuuTerraria.Content.Projectiles;

public class DemocloudShard : DemocloudComet
{
    public override string Texture => ModContent.GetInstance<DemocloudComet>().Texture;

    public override void SetDefaults()
    {
        base.SetDefaults();
        Projectile.width = 12;
        Projectile.height = 8;
        Projectile.scale = 0.45f;
        Projectile.penetrate = 1;
        Projectile.timeLeft = 40;
    }

    public override void OnHitNPC(Terraria.NPC target, Terraria.NPC.HitInfo hit, int damageDone)
    {
    }
}
