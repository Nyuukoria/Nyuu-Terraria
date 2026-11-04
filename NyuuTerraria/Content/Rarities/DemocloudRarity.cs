using Microsoft.Xna.Framework;
using Terraria.ModLoader;

namespace NyuuTerraria.Content.Rarities;

public class DemocloudRarity : ModRarity
{
    public override Color RarityColor => new Color(178, 155, 255);

    public override int GetPrefixedRarity(int offset, float valueMult) => Type;
}
