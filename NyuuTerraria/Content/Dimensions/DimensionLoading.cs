using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Graphics;
using Terraria;
using Terraria.GameContent;
using Terraria.UI;
using Terraria.WorldBuilding;

namespace NyuuTerraria.Content.Dimensions;

/// <summary>
/// Loading screen shown while a dimension is generated. Vanilla's UIWorldLoad can't be reused: it
/// expects the vanilla world generator's controller to exist and crashes without it.
/// </summary>
public class DimensionLoading : UIState
{
    private readonly GenerationProgress progress;
    private readonly string title;

    public DimensionLoading(GenerationProgress progress, string title)
    {
        this.progress = progress;
        this.title = title;
    }

    protected override void DrawSelf(SpriteBatch batch)
    {
        Vector2 center = new Vector2(Main.screenWidth / 2f, Main.screenHeight / 2f);
        string heading = "Entering " + title;
        Vector2 size = FontAssets.DeathText.Value.MeasureString(heading) * .8f;
        Utils.DrawBorderStringBig(batch, heading, center - new Vector2(size.X / 2f, 90), new Color(222, 196, 255), .8f);

        const int width = 520, height = 26;
        var bar = new Rectangle((int)center.X - width / 2, (int)center.Y - height / 2, width, height);
        Texture2D pixel = TextureAssets.MagicPixel.Value;
        batch.Draw(pixel, new Rectangle(bar.X - 3, bar.Y - 3, bar.Width + 6, bar.Height + 6), new Color(30, 22, 44));
        batch.Draw(pixel, bar, new Color(58, 42, 79));
        float done = MathHelper.Clamp((float)(progress?.TotalProgress ?? 0), 0, 1);
        batch.Draw(pixel, new Rectangle(bar.X, bar.Y, (int)(bar.Width * done), bar.Height), new Color(176, 132, 232));

        string message = progress?.Message ?? "";
        Vector2 messageSize = FontAssets.MouseText.Value.MeasureString(message);
        Utils.DrawBorderString(batch, message, center + new Vector2(-messageSize.X / 2f, 30), Color.White);
    }
}
