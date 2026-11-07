// Nyuu Art Fixes - drop-in code for Nyuu Terraria
//
// Contents of the pack (install.py puts these here for you):
//     NyuuTerraria/Content/NyuuArtFixes.cs
//     NyuuTerraria/Content/creature-anchors.g.cs      (measured ink bounds per creature)
//
// install.py also adds ONE line to each of two PreDraw methods in LivingAnimation.cs so
// the mod calls into this file. Nothing else in the mod is edited, and --revert undoes
// both the copy and the two lines.
//
// What it changes:
//
//  1. Baked frame playback.  CreatureAnimationSetup already forces Main.npcFrameCount = 4
//     for every creature and Main.projFrames = 4 for the pets, but LivingNPCAnimation
//     ignores frames 1-3 and rebuilds the motion at runtime by slicing the sprite into
//     joints - which is why limbs and tails separate from the body. With UseBakedFrames
//     the four poses in the sheet are played instead. Pair this with the regenerated
//     sheets, which contain four distinct poses.
//
//  2. Ink-accurate anchoring.  The art inside an animation frame is smaller than the frame
//     and not centred, and the hitbox is a third independent size:
//         AshBurrower art 36x19 in a 38x38 hitbox, NimbusWarden art 124x120 in 126x88.
//     DrawBaked puts the ink centre on the hitbox centre and the ink bottom on the ground
//     using the bounds measured in creature-anchors.g.cs, so flyers stop drifting inside
//     their own hitbox.
//
//  3. Optional hitbox fitting (FitHitboxToArt, off by default because it changes combat).
//     When on, each creature's hitbox is trimmed to its artwork.
//
// No world writes, no save format changes, no network state.

using System;
using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Graphics;
using Terraria;
using Terraria.GameContent;
using Terraria.ModLoader;
using NyuuTerraria.Content.Frontiers;

namespace NyuuTerraria.Content;

internal readonly record struct Anchor(
    LivingRig rig, int fw, int fh,
    int inkL, int inkR, int inkT, int inkB,
    int dx, int dy, int hitW, int hitH);

internal static partial class NyuuArtFixes
{
    /// <summary>Play the four poses in the PNG instead of the runtime joint slicing.</summary>
    public static bool UseBakedFrames = true;

    /// <summary>Trim hitboxes to the artwork. Changes combat, off by default.</summary>
    public static bool FitHitboxToArt = false;

    /// <summary>Frame height the sheet is currently being drawn with.</summary>
    private static int FrameHeight(Texture2D tex, int frames, int want)
    {
        int fh = tex.Height / Math.Max(1, frames);
        return fh * 4 == tex.Height && want > 0 ? want : fh;
    }

    private static Vector2 OriginFor(Anchor a, Rectangle src, bool grounded, bool flip)
    {
        float ox = (a.inkL + a.inkR) / 2f;
        float oy = grounded ? a.inkB : (a.inkT + a.inkB) / 2f;
        if (flip) ox = src.Width - ox;
        return new Vector2(ox, oy);
    }

    /// <summary>Called from the patched LivingNPCAnimation.PreDraw. True means "already drawn".</summary>
    internal static bool TryDrawNpc(NPC npc, SpriteBatch batch, Vector2 screenPos, Color drawColor)
    {
        if (!UseBakedFrames) return false;
        if (npc.ModNPC is null) return false;
        if (!Anchors.TryGetValue(npc.ModNPC.Name, out var a)) return false;

        var tex = TextureAssets.Npc[npc.type].Value;
        int frames = Math.Max(1, Main.npcFrameCount[npc.type]);
        int fh = FrameHeight(tex, frames, a.fh);
        int frameY = npc.frame.Height == fh ? npc.frame.Y : 0;
        if (frameY < 0 || frameY + fh > tex.Height) frameY = 0;

        var src = new Rectangle(0, frameY, tex.Width, fh);
        bool grounded = !npc.noGravity;
        bool flip = npc.spriteDirection == -1;
        var origin = OriginFor(a, src, grounded, flip);
        Vector2 pos = (grounded ? npc.Bottom : npc.Center) - screenPos;

        batch.Draw(tex, pos, src, drawColor * npc.Opacity,
            grounded ? 0f : npc.rotation, origin, npc.scale,
            flip ? SpriteEffects.FlipHorizontally : SpriteEffects.None, 0f);
        return true;
    }

    /// <summary>Called from the patched LivingProjectileAnimation.PreDraw.</summary>
    internal static bool TryDrawProjectile(Projectile projectile, ref Color lightColor)
    {
        if (!UseBakedFrames) return false;
        if (projectile.ModProjectile is null) return false;
        string name = projectile.ModProjectile.Name;
        if (!LivingAnimation.Projectiles.Contains(name)) return false;
        if (!Anchors.TryGetValue(name, out var a)) return false;

        var tex = TextureAssets.Projectile[projectile.type].Value;
        int frames = Math.Max(1, Main.projFrames[projectile.type]);
        int fh = FrameHeight(tex, frames, a.fh);
        int frameY = Math.Clamp(projectile.frame, 0, Math.Max(0, tex.Height / fh - 1)) * fh;
        if (frameY + fh > tex.Height) frameY = 0;

        var src = new Rectangle(0, frameY, tex.Width, fh);
        bool grounded = name == "DewFoxPet";
        bool flip = projectile.spriteDirection == -1 ||
                    (Math.Abs(projectile.velocity.X) > .15f && projectile.velocity.X < 0);
        var origin = OriginFor(a, src, grounded, flip);
        Vector2 pos = (grounded ? projectile.Bottom : projectile.Center) - Main.screenPosition;

        Main.spriteBatch.Draw(tex, pos, src, lightColor * projectile.Opacity,
            grounded ? 0f : projectile.rotation, origin, projectile.scale,
            flip ? SpriteEffects.FlipHorizontally : SpriteEffects.None, 0f);
        return true;
    }
}

/// <summary>Optional: trim creature hitboxes to their artwork.</summary>
public class NyuuHitboxFit : GlobalNPC
{
    public override void PostSetDefaults(NPC npc)
    {
        if (!NyuuArtFixes.FitHitboxToArt) return;
        if (npc.ModNPC is null) return;
        if (!NyuuArtFixes.Anchors.TryGetValue(npc.ModNPC.Name, out var a)) return;
        npc.width = Math.Max(8, a.inkR - a.inkL);
        npc.height = Math.Max(8, a.inkB - a.inkT);
    }
}
