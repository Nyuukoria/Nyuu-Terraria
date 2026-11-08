using Microsoft.Xna.Framework.Graphics;
using ReLogic.Content;
using Terraria;
using Terraria.ModLoader;

namespace NyuuTerraria.Content.Title;

/// <summary>
/// The mod's own title screen, built on tModLoader's <see cref="ModMenu"/> type - the same
/// mechanism behind the "Switch Menu Theme" line at the bottom of the main menu. tModLoader
/// draws the background, the logo, the buttons and the music itself, so nothing here can
/// cover a button or fight the menu: this class only says *what* to draw.
///
/// Logo  : Content/Title/TitleLogo.png replaces the vanilla Terraria tree logo.
/// Music : Assets/Music/NyuuTitle.wav, autoloaded by tModLoader from Assets/Music.
///
/// The menu appears as its own entry in the menu-theme list; once selected tModLoader
/// remembers it. An earlier attempt hooked Main.DrawMenu, which painted over the buttons,
/// and then Main.DrawBackground, which the menu never calls - both are gone.
/// </summary>
public class TitleScreen : ModMenu
{
    private const string LogoPath = "NyuuTerraria/Content/Title/TitleLogo";
    private const string MusicPath = "Assets/Music/NyuuTitle";

    private Asset<Texture2D> logo;
    private int musicSlot = -1;

    public override void Load()
    {
        if (Main.dedServ) return;

        logo = ModContent.Request<Texture2D>(LogoPath); // async: no blocking load at startup

        try
        {
            // Autoloaded from Assets/Music, so only the slot id is needed.
            musicSlot = MusicLoader.GetMusicSlot(Mod, MusicPath);
        }
        catch (System.Exception e)
        {
            Mod.Logger.Warn("Title theme not registered: " + e.Message);
            musicSlot = -1;
        }
    }

    public override void Unload()
    {
        logo = null;
        musicSlot = -1;
    }

    /// <summary>Shown in the menu-theme list at the bottom of the main menu.</summary>
    public override string DisplayName => "Nyuu Terraria";

    /// <summary>Our wordmark, drawn in the slot the vanilla tree logo occupies.</summary>
    public override Asset<Texture2D> Logo => logo;

    /// <summary>Played while this menu theme is active. -1 keeps the vanilla menu track.</summary>
    public override int Music => musicSlot;
}
