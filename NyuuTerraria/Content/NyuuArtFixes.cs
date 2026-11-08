// Nyuu Art Fixes - drop-in code for Nyuu Terraria
//
// Goes to:  NyuuTerraria/Content/NyuuArtFixes.cs
//           NyuuTerraria/Content/creature-anchors.g.cs      (measured artwork bounds)
//
// install.py also adds ONE line to each of two PreDraw methods in LivingAnimation.cs.
// Nothing else in the mod is edited.
//
// What it does:
//
//  1. Baked frame playback.  CreatureAnimationSetup already forces Main.npcFrameCount = 4
//     for every creature and Main.projFrames = 4 for the pets, but LivingNPCAnimation
//     ignores frames 1-3 and rebuilds the motion at runtime by slicing the sprite into
//     joints - which is why limbs and tails separate from the body. With UseBakedFrames
//     the four poses in the sheet are played instead.
//
//  2. Ink-accurate anchoring.  The art inside an animation frame is smaller than the frame
//     and not centred, and the hitbox is a third independent size (AshBurrower art 36x19 in
//     a 38x38 hitbox, NimbusWarden art 124x120 in 126x88). This puts the artwork's ink centre
//     on the hitbox centre and its bottom on the ground.
//
// Hitbox sizes are deliberately NOT touched - that is combat balance.
// No world writes, no save format changes, no network state.
//
// Deliberately self-contained: it does not reference LivingRig, LivingAnimation or any
// other mod type, so it cannot break if those move. Only core Terraria/XNA types are used.

using System;
using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Graphics;
using Terraria;
using Terraria.GameContent;
using Terraria.ModLoader;

namespace NyuuTerraria.Content;

internal readonly struct Anchor
{
    internal readonly int Fw, Fh, InkL, InkR, InkT, InkB;

    internal Anchor(int fw, int fh, int inkL, int inkR, int inkT, int inkB)
    {
        Fw = fw; Fh = fh;
        InkL = inkL; InkR = inkR; InkT = inkT; InkB = inkB;
    }
}

internal static partial class NyuuArtFixes
{
    /// <summary>Play the four poses in the PNG instead of the runtime joint slicing.</summary>
    public static bool UseBakedFrames = true;

    /// <summary>Pets and minions that animate from a 4-frame sheet.</summary>
    private static readonly string[] Pets =
    {
        "GlowcapFirefly", "AmberWisp", "LittleGlassBell",
        "DewFoxPet", "PrideMothPet", "MooncapMinion"
    };

    private static bool IsPet(string name)
    {
        for (int i = 0; i < Pets.Length; i++)
            if (Pets[i] == name) return true;
        return false;
    }

    /// <summary>Origin that puts the artwork's ink centre on the draw position.</summary>
    private static Vector2 OriginFor(Anchor a, int srcWidth, bool grounded, bool flip)
    {
        float ox = (a.InkL + a.InkR) / 2f;
        float oy = grounded ? a.InkB : (a.InkT + a.InkB) / 2f;
        if (flip) ox = srcWidth - ox;
        return new Vector2(ox, oy);
    }

    private static int FrameHeight(Texture2D tex, int frames)
    {
        if (frames < 1) frames = 1;
        return tex.Height / frames;
    }

    /// <summary>Called from the patched LivingNPCAnimation.PreDraw. True means "already drawn".</summary>
    internal static bool TryDrawNpc(NPC npc, SpriteBatch batch, Vector2 screenPos, Color drawColor)
    {
        if (!UseBakedFrames) return false;
        if (npc == null || npc.ModNPC == null) return false;
        if (!Anchors.TryGetValue(npc.ModNPC.Name, out Anchor a)) return false;

        Texture2D tex = TextureAssets.Npc[npc.type].Value;
        int fh = FrameHeight(tex, Main.npcFrameCount[npc.type]);
        if (fh < 1) return false;

        int frameY = npc.frame.Height == fh ? npc.frame.Y : 0;
        if (frameY < 0 || frameY + fh > tex.Height) frameY = 0;

        bool grounded = !npc.noGravity;
        bool flip = npc.spriteDirection == -1;
        Vector2 origin = OriginFor(a, tex.Width, grounded, flip);
        Vector2 pos = (grounded ? npc.Bottom : npc.Center) - screenPos;
        Rectangle src = new Rectangle(0, frameY, tex.Width, fh);

        batch.Draw(tex, pos, src, drawColor * npc.Opacity,
            grounded ? 0f : npc.rotation, origin, npc.scale,
            flip ? SpriteEffects.FlipHorizontally : SpriteEffects.None, 0f);
        return true;
    }

    /// <summary>Called from the patched LivingProjectileAnimation.PreDraw.</summary>
    internal static bool TryDrawProjectile(Projectile projectile, ref Color lightColor)
    {
        if (!UseBakedFrames) return false;
        if (projectile == null || projectile.ModProjectile == null) return false;
        string name = projectile.ModProjectile.Name;
        if (!IsPet(name)) return false;
        if (!Anchors.TryGetValue(name, out Anchor a)) return false;

        Texture2D tex = TextureAssets.Projectile[projectile.type].Value;
        int frames = Main.projFrames[projectile.type];
        if (frames < 1) frames = 1;
        int fh = FrameHeight(tex, frames);
        if (fh < 1) return false;

        int f = projectile.frame;
        if (f < 0) f = 0;
        if (f > frames - 1) f = frames - 1;
        int frameY = f * fh;
        if (frameY + fh > tex.Height) frameY = 0;

        bool grounded = name == "DewFoxPet";
        bool flip = projectile.spriteDirection == -1 ||
                    (Math.Abs(projectile.velocity.X) > .15f && projectile.velocity.X < 0);
        Vector2 origin = OriginFor(a, tex.Width, grounded, flip);
        Vector2 pos = (grounded ? projectile.Bottom : projectile.Center) - Main.screenPosition;
        Rectangle src = new Rectangle(0, frameY, tex.Width, fh);

        Main.spriteBatch.Draw(tex, pos, src, lightColor,
            grounded ? 0f : projectile.rotation, origin, projectile.scale,
            flip ? SpriteEffects.FlipHorizontally : SpriteEffects.None, 0f);
        return true;
    }
}
