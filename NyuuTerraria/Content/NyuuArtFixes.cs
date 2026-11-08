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
//  3. Draw scale.  A handful of creatures were painted on the canvas much smaller than the
//     hitbox they fight with - VineboundArmor is 22x32 pixels inside a 38x38 body, so it reads
//     as ankle height next to the player. Sizes[] below multiplies the draw scale ONLY; the
//     artwork file, the animation and the hitbox are all untouched, so the sprite is drawn
//     larger around the same ink centre and the same ground line.
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

    /// <summary>
    /// Ink bounds for the six pet/minion sheets. Keyed by ModProjectile name, because
    /// DewFoxPet and MooncapMinion reuse a creature texture. Measured across all four
    /// frames so a flapping wing cannot drag the sprite off centre.
    /// </summary>
    private static readonly System.Collections.Generic.Dictionary<string, Anchor> PetAnchors =
        new System.Collections.Generic.Dictionary<string, Anchor>
    {
        { "GlowcapFirefly", new Anchor(64, 64, 2, 62, 5, 57) },
        { "AmberWisp", new Anchor(48, 48, 13, 39, 7, 41) },
        { "LittleGlassBell", new Anchor(48, 48, 7, 43, 11, 37) },
        { "PrideMothPet", new Anchor(64, 64, 2, 62, 7, 57) },
        { "DewFoxPet", new Anchor(48, 48, 6, 44, 15, 48) },
        { "MooncapMinion", new Anchor(48, 48, 8, 40, 15, 48) }
    };

    /// <summary>
    /// Creatures whose artwork is painted facing LEFT. Terraria's convention is that a sprite
    /// faces right and gets mirrored for spriteDirection -1, so for these the mirror has to be
    /// inverted or they walk backwards while looking forwards. Read off the artwork: head,
    /// beak, snout or jaw end sits on the left of the frame.
    /// </summary>
    private static readonly string[] FacesLeft =
    {
        "DewFox", "DewFoxPet", "GlassDeer", "Mothcat",
        "GardenBird", "GlassSerpent", "ReefbackGuardian"
    };

    private static bool FlipsWhenRight(string name)
    {
        for (int i = 0; i < FacesLeft.Length; i++)
            if (FacesLeft[i] == name) return true;
        return false;
    }

    /// <summary>Mirror this sprite for a facing direction of 1 (right) or -1 (left).</summary>
    private static bool ShouldFlip(string name, int direction)
    {
        return FlipsWhenRight(name) ? direction > 0 : direction < 0;
    }

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

    /// <summary>
    /// Draw-only size correction. Key = ModNPC name, value = multiplier applied on top of
    /// npc.scale. Only creatures whose artwork is noticeably smaller than their hitbox are
    /// listed; everything else draws at its authored size.
    ///
    /// name                art px    hitbox   x     drawn    note
    /// VineboundArmor      22x32     38x38   1.50   33x48    was ankle height
    /// ArchiveCaretaker    36x27     38x38   1.50   54x40
    /// GlassDeer           22x32     34x40   1.55   34x50
    /// ReefAmbusher        36x31     38x38   1.45   52x45
    /// AshBurrower         36x19     38x38   1.35   49x26    hitbox shape still odd
    /// CrystalSnail        36x27     30x26   1.20   43x32
    /// TidepoolTurtle      36x28     30x26   1.15   41x32
    /// </summary>
    private static readonly System.Collections.Generic.Dictionary<string, float> Sizes =
        new System.Collections.Generic.Dictionary<string, float>
    {
        { "VineboundArmor", 1.50f },
        { "ArchiveCaretaker", 1.50f },
        { "GlassDeer", 1.55f },
        { "ReefAmbusher", 1.45f },
        { "AshBurrower", 1.35f },
        { "CrystalSnail", 1.20f },
        { "TidepoolTurtle", 1.15f },

        // bosses: painted at 102-124 px, which is barely two players tall, so they read
        // as large enemies rather than encounters. Vanilla's mid bosses sit near 160 px.
        { "NimbusWarden", 1.25f },
        { "PrismSentinel", 1.30f },
        { "MourningBell", 1.30f },
        { "RootboundKnight", 1.25f },
        { "Bellkeeper", 1.30f },
        { "PrismMatriarch", 1.30f },
        { "ReefbackGuardian", 1.35f },
        { "PaleGardener", 1.20f },
        { "TheUnwritten", 1.25f }
    };

    /// <summary>Extra draw scale for a creature, 1 when it is not in the table.</summary>
    private static float SizeFor(string name)
    {
        float s;
        return Sizes.TryGetValue(name, out s) ? s : 1f;
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
        string name = npc.ModNPC.Name;
        if (!Anchors.TryGetValue(name, out Anchor a)) return false;

        Texture2D tex = TextureAssets.Npc[npc.type].Value;
        int fh = FrameHeight(tex, Main.npcFrameCount[npc.type]);
        if (fh < 1) return false;

        int frameY = npc.frame.Height == fh ? npc.frame.Y : 0;
        if (frameY < 0 || frameY + fh > tex.Height) frameY = 0;

        bool grounded = !npc.noGravity;
        bool flip = ShouldFlip(name, npc.spriteDirection);
        Vector2 origin = OriginFor(a, tex.Width, grounded, flip);
        Vector2 pos = (grounded ? npc.Bottom : npc.Center) - screenPos;
        Rectangle src = new Rectangle(0, frameY, tex.Width, fh);

        batch.Draw(tex, pos, src, drawColor * npc.Opacity,
            grounded ? 0f : npc.rotation, origin, npc.scale * SizeFor(name),
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
        Anchor a;
        if (!PetAnchors.TryGetValue(name, out a) && !Anchors.TryGetValue(name, out a)) return false;

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
        int face = projectile.spriteDirection;
        if (Math.Abs(projectile.velocity.X) > .15f) face = projectile.velocity.X > 0 ? 1 : -1;
        bool flip = ShouldFlip(name, face);
        Vector2 origin = OriginFor(a, tex.Width, grounded, flip);
        Vector2 pos = (grounded ? projectile.Bottom : projectile.Center) - Main.screenPosition;
        Rectangle src = new Rectangle(0, frameY, tex.Width, fh);

        Main.spriteBatch.Draw(tex, pos, src, lightColor,
            grounded ? 0f : projectile.rotation, origin, projectile.scale * SizeFor(name),
            flip ? SpriteEffects.FlipHorizontally : SpriteEffects.None, 0f);
        return true;
    }
}
