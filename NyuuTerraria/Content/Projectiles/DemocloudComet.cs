using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Graphics;
using Terraria;
using Terraria.GameContent;
using Terraria.ID;
using Terraria.ModLoader;

namespace NyuuTerraria.Content.Projectiles;

public class DemocloudComet : ModProjectile
{
    public override void SetDefaults()
    {
        Projectile.width = 22;
        Projectile.height = 16;
        Projectile.friendly = true;
        Projectile.DamageType = DamageClass.Magic;
        Projectile.penetrate = 4;
        Projectile.usesLocalNPCImmunity = true;
        Projectile.localNPCHitCooldown = -1;
        Projectile.timeLeft = 180;
        Projectile.alpha = 0;
    }

    public override Color? GetAlpha(Color lightColor) => Color.White;

    public override void AI()
    {
        Projectile.rotation = Projectile.velocity.ToRotation();
        if (Main.dedServ)
            return;

        Lighting.AddLight(Projectile.Center, 0.50f, 0.14f, 0.72f);
    }

    public override bool PreDraw(Player player, ref Color lightColor)
    {
        const int segments = 14;
        float scale = Projectile.scale;
        Vector2 direction = Projectile.velocity.SafeNormalize(Vector2.UnitX);
        Vector2 anchor = Projectile.Center - Main.screenPosition - direction * (10f * scale);
        float segmentLength = 5f * scale;
        Texture2D pixel = TextureAssets.MagicPixel.Value;
        Rectangle source = new Rectangle(0, 0, 1, 1);
        Vector2 origin = new Vector2(0f, 0.5f);
        float rotation = (-direction).ToRotation();

        for (int i = segments - 1; i >= 0; i--)
        {
            float fade = 1f - i / (float)segments;
            Vector2 position = anchor - direction * (segmentLength * i);
            Color glow = new Color(162, 87, 242) * (0.68f * fade);
            Color core = new Color(255, 184, 245) * (0.94f * fade);
            Main.EntitySpriteDraw(pixel, position, source, glow, rotation, origin,
                new Vector2(segmentLength + 0.75f, (2f + 4f * fade) * scale), SpriteEffects.None);
            Main.EntitySpriteDraw(pixel, position, source, core, rotation, origin,
                new Vector2(segmentLength + 0.75f, (0.7f + 1.5f * fade) * scale), SpriteEffects.None);
        }

        Texture2D comet = TextureAssets.Projectile[Type].Value;
        Main.EntitySpriteDraw(comet, Projectile.Center - Main.screenPosition, null, Color.White,
            Projectile.rotation, new Vector2(comet.Width, comet.Height) * 0.5f,
            scale, SpriteEffects.None);
        return false;
    }

    public override void OnHitNPC(NPC target, NPC.HitInfo hit, int damageDone)
    {
        if (Projectile.owner != Main.myPlayer || Main.rand.Next(3) != 0)
            return;

        Vector2 direction = Projectile.velocity.SafeNormalize(Vector2.UnitX);
        for (int i = -1; i <= 1; i++)
        {
            Vector2 velocity = direction.RotatedBy(i * 0.35f) * 8f;
            Projectile.NewProjectile(Projectile.GetSource_OnHit(target),
                target.Center, velocity, ModContent.ProjectileType<DemocloudShard>(),
                System.Math.Max(1, Projectile.damage / 3), Projectile.knockBack * 0.35f,
                Projectile.owner);
        }
    }

    public override void OnKill(int timeLeft)
    {
        if (Main.dedServ)
            return;

        for (int i = 0; i < 12; i++)
        {
            Vector2 burst = Vector2.UnitX.RotatedBy(MathHelper.TwoPi * i / 12f) * 2.8f;
            SpawnSpark(Projectile.Center, burst, 1.15f);
        }
    }

    private static void SpawnSpark(Vector2 position, Vector2 velocity, float scale)
    {
        Color tint = Main.rand.NextBool() ? new Color(255, 101, 218) : new Color(186, 153, 255);
        Dust dust = Dust.NewDustPerfect(position, DustID.TintableDustLighted, velocity, 35, tint, scale);
        dust.noGravity = true;
    }
}
