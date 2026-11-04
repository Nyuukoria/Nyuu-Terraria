using System;
using Microsoft.Xna.Framework;
using NyuuTerraria.Content.Items.Consumables;
using Terraria;
using Terraria.Audio;
using Terraria.ID;
using Terraria.ModLoader;

namespace NyuuTerraria.Content.Projectiles;

public class CloudyBombProjectile : ModProjectile
{
    private const int RadiusTiles = 28;
    private const int RadiusPixels = RadiusTiles * 16;

    public override string Texture => ModContent.GetInstance<CloudyBomb>().Texture;

    public override void SetDefaults()
    {
        Projectile.width = 18;
        Projectile.height = 18;
        Projectile.friendly = true;
        Projectile.penetrate = -1;
        Projectile.usesLocalNPCImmunity = true;
        Projectile.localNPCHitCooldown = -1;
        Projectile.timeLeft = 150;
        Projectile.tileCollide = true;
    }

    public override bool? CanDamage() => Projectile.timeLeft <= 1 ? null : false;
    public override bool CanHitPvp(Player target) => false;
    public override bool CanHitPlayer(Player target) => false;
    public override bool? CanHitNPC(NPC target) => target.friendly ? false : null;

    public override void AI()
    {
        if (Projectile.ai[0] == 0f)
            Projectile.velocity.Y = Math.Min(12f, Projectile.velocity.Y + 0.22f);
        Projectile.rotation += Projectile.velocity.X * 0.07f;

        if (Projectile.timeLeft != 1)
            return;

        Projectile.Resize(RadiusPixels * 2, RadiusPixels * 2);
        Projectile.tileCollide = false;
        Projectile.velocity = Vector2.Zero;
        Projectile.hide = true;
    }

    public override bool? Colliding(Rectangle projectileHitbox, Rectangle targetHitbox)
    {
        Vector2 nearest = Vector2.Clamp(Projectile.Center,
            new Vector2(targetHitbox.Left, targetHitbox.Top),
            new Vector2(targetHitbox.Right, targetHitbox.Bottom));
        return Vector2.DistanceSquared(nearest, Projectile.Center) <= RadiusPixels * RadiusPixels;
    }

    public override bool OnTileCollide(Vector2 oldVelocity)
    {
        if (Projectile.velocity.X != oldVelocity.X)
            Projectile.velocity.X = -oldVelocity.X * 0.55f;

        if (Projectile.velocity.Y != oldVelocity.Y)
            Projectile.velocity.Y = Math.Abs(oldVelocity.Y) > 1.5f ? -oldVelocity.Y * 0.45f : 0f;

        Projectile.velocity.X *= 0.85f;
        return false;
    }

    public override void OnKill(int timeLeft)
    {
        Vector2 center = Projectile.Center;
        if (Main.netMode != NetmodeID.MultiplayerClient)
        {
            Vector2 tileCenter = center / 16f;
            int left = Math.Max(1, (int)tileCenter.X - RadiusTiles);
            int right = Math.Min(Main.maxTilesX - 2, (int)tileCenter.X + RadiusTiles + 1);
            int top = Math.Max(1, (int)tileCenter.Y - RadiusTiles);
            int bottom = Math.Min(Main.maxTilesY - 2, (int)tileCenter.Y + RadiusTiles + 1);
            for (int x = left; x < right; x++)
            for (int y = top; y < bottom; y++)
            {
                Vector2 tilePosition = new Vector2(x + 0.5f, y + 0.5f);
                if (Vector2.DistanceSquared(tilePosition, tileCenter) >= RadiusTiles * RadiusTiles)
                    continue;

                Tile tile = Main.tile[x, y];
                if (tile.HasTile)
                {
                    WorldGen.KillTile(x, y);
                    // Protected terrain can reject the normal break path.
                    // Keep furniture and storage on Terraria's normal removal path.
                    if (tile.HasTile && Main.tileSolid[tile.TileType] &&
                        !Main.tileFrameImportant[tile.TileType])
                    {
                        tile.HasTile = false;
                        WorldGen.SquareTileFrame(x, y);
                    }
                }

                if (tile.WallType != 0)
                {
                    WorldGen.KillWall(x, y);
                    if (tile.WallType != 0)
                    {
                        tile.WallType = 0;
                        WorldGen.SquareWallFrame(x, y);
                    }
                }
            }

            if (Main.netMode == NetmodeID.Server)
                NetMessage.SendTileSquare(-1, left, top, right - left, bottom - top);

        }

        if (Main.dedServ)
            return;

        SoundEngine.PlaySound(SoundID.Item14, center);
        for (int i = 0; i < 96; i++)
        {
            Vector2 outward = Vector2.UnitX.RotatedBy(MathHelper.TwoPi * i / 96f);
            Vector2 position = center + outward * Main.rand.NextFloat(0f, RadiusPixels);
            Dust cloud = Dust.NewDustPerfect(position, DustID.Smoke, outward * 3f, 85,
                new Color(203, 194, 244), 1.65f);
            cloud.noGravity = true;
        }
    }
}
