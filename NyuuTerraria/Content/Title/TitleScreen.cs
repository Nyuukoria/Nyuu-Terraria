using System;
using System.Reflection;
using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Graphics;
using ReLogic.Content;
using Terraria;
using Terraria.ModLoader;

namespace NyuuTerraria.Content.Title;

/// <summary>
/// Custom main menu: a violet sky backdrop and the Nyuu Terraria wordmark over the vanilla
/// title screen, plus a looping theme of its own.
///
/// Both halves fail soft. The draw hook is installed by reflection, so if a future tModLoader
/// renames Main.DrawMenu the menu simply stays vanilla; the music is registered once and the
/// menu only ever sets Main.newMusic, which is a single int vanilla already reads.
///
/// Art:  Content/Title/TitleBackdrop.png, Content/Title/TitleLogo.png
/// Music: Assets/Music/NyuuTitle.wav
/// </summary>
public class TitleScreen : ModSystem
{
    private const string LogoPath = "Content/Title/TitleLogo";
    private const string BackdropPath = "Content/Title/TitleBackdrop";
    private const string MusicPath = "Assets/Music/NyuuTitle";

    private const BindingFlags Flags = BindingFlags.Instance | BindingFlags.Public | BindingFlags.NonPublic;

    private Asset<Texture2D> logo;
    private Asset<Texture2D> backdrop;
    private MethodInfo drawMenu;
    private Action<Action<Main, GameTime>, Main, GameTime> hook;
    private int musicSlot = -1;

    public override void Load()
    {
        if (Main.dedServ) return;

        try
        {
            musicSlot = MusicLoader.AddMusic(Mod, MusicPath);
        }
        catch (Exception e)
        {
            Mod.Logger.Warn("Title theme not registered: " + e.Message);
            musicSlot = -1;
        }

        try
        {
            logo = ModContent.Request<Texture2D>("NyuuTerraria/" + LogoPath, AssetRequestMode.AsyncLoad);
            backdrop = ModContent.Request<Texture2D>("NyuuTerraria/" + BackdropPath, AssetRequestMode.AsyncLoad);
        }
        catch (Exception e)
        {
            Mod.Logger.Warn("Title art not loaded: " + e.Message);
        }

        try
        {
            drawMenu = typeof(Main).GetMethod("DrawMenu", Flags);
            if (drawMenu == null)
            {
                Mod.Logger.Warn("Title screen: Main.DrawMenu not found, using the vanilla title.");
                return;
            }
            hook = Hook;
            MonoModHooks.Add(drawMenu, hook);
        }
        catch (Exception e)
        {
            Mod.Logger.Warn("Title screen draw hook disabled: " + e.Message);
            hook = null;
        }
    }

    public override void Unload()
    {
        // MonoModHooks has no Remove; the hook goes with the assembly on reload.
        hook = null;
        drawMenu = null;
        logo = null;
        backdrop = null;
        musicSlot = -1;
    }

    /// <summary>Vanilla picks this up on the next music tick.</summary>
    public override void PostUpdateEverything()
    {
        if (Main.dedServ || !Main.gameMenu || musicSlot < 0) return;
        Main.newMusic = musicSlot;
    }

    private void Hook(Action<Main, GameTime> orig, Main self, GameTime time)
    {
        orig(self, time);
        Draw();
    }

    private void Draw()
    {
        if (!Main.gameMenu || Main.menuMode != 0) return;
        if (backdrop == null || !backdrop.IsLoaded) return;

        SpriteBatch batch = Main.spriteBatch;
        Texture2D sky = backdrop.Value;
        float cover = Math.Max((float)Main.screenWidth / sky.Width, (float)Main.screenHeight / sky.Height);
        batch.Begin(SpriteSortMode.Deferred, BlendState.AlphaBlend, SamplerState.PointClamp,
            DepthStencilState.None, RasterizerState.CullNone, null, Main.UIScaleMatrix);

        batch.Draw(sky, new Vector2(Main.screenWidth * 0.5f, Main.screenHeight * 0.5f), null,
            Color.White, 0f, new Vector2(sky.Width * 0.5f, sky.Height * 0.5f), cover,
            SpriteEffects.None, 0f);

        // keep the menu text readable over a bright sky
        batch.Draw(Terraria.GameContent.TextureAssets.MagicPixel.Value,
            new Rectangle(0, 0, Main.screenWidth, Main.screenHeight), new Color(8, 6, 18) * 0.42f);

        if (logo != null && logo.IsLoaded)
        {
            Texture2D mark = logo.Value;
            float scale = MathHelper.Clamp(Main.screenWidth / (float)mark.Width * 0.34f, 1f, 3f);
            batch.Draw(mark, new Vector2(Main.screenWidth * 0.5f, Main.screenHeight * 0.20f), null,
                Color.White, 0f, new Vector2(mark.Width * 0.5f, mark.Height * 0.5f), scale,
                SpriteEffects.None, 0f);
        }

        batch.End();
    }
}
