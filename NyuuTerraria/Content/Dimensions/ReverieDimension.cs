using System;
using Microsoft.Xna.Framework;
using Terraria;
using Terraria.ID;
using Terraria.ModLoader;
using Terraria.WorldBuilding;
using NyuuTerraria.Content.Frontiers;
using NyuuTerraria.Content.Reverie;

namespace NyuuTerraria.Content.Dimensions;

/// <summary>
/// The Reverie as a world of its own: rolling flower meadows under the lavender sky, moonlit lakes,
/// floating flower islands, an abandoned observatory, and caves threaded with Moonprism.
/// </summary>
public class ReverieDimension : Dimension
{
    public override string Id => "Reverie";
    public override string DisplayName => "The Reverie";
    public override int Width => 1600;
    public override int Height => 900;
    public override int Realm => 0;

    private const int Ground = 300;   // average surface row
    private const int Margin = 60;    // keep features clear of the world edge
    private int spawnX;

    public override void Generate(GenerationProgress progress)
    {
        var rand = WorldGen.genRand;
        int w = Main.maxTilesX, h = Main.maxTilesY;
        int[] surface = new int[w];

        Main.worldSurface = Ground + 30;
        Main.rockLayer = Ground + 90;
        spawnX = 240;

        // ---- terrain
        progress.Message = "Growing the Reverie meadows";
        double p1 = rand.NextDouble() * 6, p2 = rand.NextDouble() * 6;
        for (int x = 0; x < w; x++)
        {
            double y = Ground + Math.Sin(x * .0055 + p1) * 16 + Math.Sin(x * .019 + p2) * 6 + Math.Sin(x * .061) * 2;
            int flat = Math.Abs(x - spawnX);
            if (flat < 30) y = Ground;
            else if (flat < 60) y = Ground + (y - Ground) * (flat - 30) / 30.0;
            surface[x] = (int)y;
            int dirtDepth = 14 + (int)(Math.Sin(x * .04) * 4);
            for (int ty = surface[x]; ty < h; ty++)
            {
                int type = ty == surface[x] ? TileID.Grass : ty < surface[x] + dirtDepth ? TileID.Dirt : TileID.Stone;
                Place(x, ty, type);
                if (ty > surface[x] + 2) Main.tile[x, ty].WallType = ty < Main.rockLayer ? WallID.DirtUnsafe : WallID.Stone;
            }
            progress.Set(x / (double)w * .2);
        }

        // ---- caves and ores
        progress.Message = "Hollowing moonlit caves";
        for (int n = 0; n < 70; n++)
            WorldGen.TileRunner(rand.Next(Margin, w - Margin), rand.Next(Ground + 45, h - 120),
                rand.Next(8, 16), rand.Next(40, 110), -1);
        for (int n = 0; n < 220; n++)
            WorldGen.TileRunner(rand.Next(Margin, w - Margin), rand.Next(Ground + 20, h - 60),
                rand.Next(3, 7), rand.Next(4, 10), TileID.Dirt);
        progress.Set(.35);

        progress.Message = "Seeding Moonprism";
        int moonprism = ModContent.TileType<MoonprismOreTile>();
        for (int n = 0; n < 190; n++)
            WorldGen.OreRunner(rand.Next(Margin, w - Margin), rand.Next((int)Main.rockLayer - 30, h - 80),
                rand.Next(3, 6), rand.Next(4, 9), (ushort)moonprism);
        for (int n = 0; n < 60; n++)
            WorldGen.OreRunner(rand.Next(Margin, w - Margin), rand.Next(Ground + 20, h - 80),
                rand.Next(3, 5), rand.Next(3, 7), (ushort)(n % 2 == 0 ? TileID.Silver : TileID.Iron));
        progress.Set(.45);

        // ---- moonlit lakes
        progress.Message = "Filling moonlit lakes";
        foreach (int lake in new[] { 520, 930, 1330 })
        {
            int cx = lake + rand.Next(-40, 40), half = rand.Next(22, 34), depth = rand.Next(9, 14);
            int water = surface[cx];
            for (int x = cx - half; x <= cx + half; x++)
            {
                double t = (x - cx) / (double)half;
                int bottom = water + (int)(depth * Math.Sqrt(1 - t * t));
                for (int y = Math.Min(surface[x], water) - 6; y <= bottom; y++)
                {
                    Tile tile = Main.tile[x, y];
                    tile.HasTile = false;
                    if (y >= water) { tile.LiquidType = LiquidID.Water; tile.LiquidAmount = 255; }
                }
                Place(x, bottom + 1, TileID.Sand);
                Place(x, bottom + 2, TileID.Sand);
                surface[x] = Math.Max(surface[x], water);
            }
        }
        progress.Set(.55);

        // ---- floating flower islands
        progress.Message = "Raising flower islands";
        for (int n = 0; n < 9; n++)
        {
            int cx = 180 + n * 160 + rand.Next(-30, 30), cy = Ground - 120 - rand.Next(0, 70);
            int rx = rand.Next(16, 28), ry = rand.Next(6, 10);
            for (int x = cx - rx; x <= cx + rx; x++)
            {
                double t = (x - cx) / (double)rx;
                int top = cy - (int)(2 * Math.Sqrt(1 - t * t));
                int bottom = cy + (int)(ry * Math.Sqrt(1 - t * t));
                for (int y = top; y <= bottom; y++)
                    Place(x, y, y == top ? TileID.Grass : y > bottom - 2 && rand.NextBool(3) ? TileID.Pearlstone : TileID.Dirt);
                if (x % 3 == 0) Flower(x, top - 1);
            }
            ExpeditionScenery.Tree(cx + rand.Next(-rx / 2, rx / 2), cy - 2, 12, n);
            if (n % 3 == 1) LootChest(cx - rx / 2, cy - 3, rand, island: true);
        }
        progress.Set(.65);

        // ---- meadow dressing: trees, flowers, vases
        progress.Message = "Planting crystal orchards";
        for (int x = Margin + 20; x < w - Margin - 20; x++)
        {
            if (Math.Abs(x - spawnX) < 24 || Main.tile[x, surface[x]].LiquidAmount > 0 || !Main.tile[x, surface[x]].HasTile) continue;
            if (x % 23 == 0 && rand.NextBool(2)) { ExpeditionScenery.Tree(x, surface[x], 14, x); x += 6; continue; }
            if (rand.NextBool(3)) Flower(x, surface[x] - 1);
            if (rand.NextBool(140)) WorldGen.PlaceObject(x, surface[x] - 1, ModContent.TileType<ReverieVaseTile>());
        }

        // ---- the abandoned observatory
        progress.Message = "Raising the old observatory";
        Observatory(1120, surface, rand);
        progress.Set(.75);

        // ---- underground caches
        for (int n = 0, tries = 0; n < 8 && tries < 4000; tries++)
        {
            int x = rand.Next(Margin, w - Margin), y = rand.Next((int)Main.rockLayer, h - 100);
            if (Main.tile[x, y].HasTile || !Main.tile[x, y + 1].HasTile || Main.tile[x + 1, y].HasTile) continue;
            if (LootChest(x, y, rand, island: false)) n++;
        }

        // ---- landing
        progress.Message = "Lighting the landing";
        int sy = surface[spawnX];
        for (int x = spawnX - 6; x <= spawnX + 6; x++) Place(x, sy, TileID.PearlstoneBrick);
        WorldGen.PlaceObject(spawnX - 5, sy - 1, TileID.Lampposts);
        WorldGen.PlaceObject(spawnX + 5, sy - 1, TileID.Lampposts);
        Main.spawnTileX = spawnX;
        Main.spawnTileY = sy;

        // ---- frame everything once, so the world loads looking finished
        progress.Message = "Settling the dream";
        for (int x = 1; x < w - 1; x++)
        {
            for (int y = 1; y < h - 1; y++)
            {
                if (Main.tile[x, y].HasTile && !Main.tileFrameImportant[Main.tile[x, y].TileType]) WorldGen.TileFrame(x, y, true);
                if (Main.tile[x, y].WallType > 0) Framing.WallFrame(x, y, true);
            }
            if (x % 64 == 0) progress.Set(.8 + .2 * x / w);
        }
    }

    private static void Place(int x, int y, int type)
    {
        if (!WorldGen.InWorld(x, y, 2)) return;
        Tile t = Main.tile[x, y];
        t.HasTile = true;
        t.TileType = (ushort)type;
        t.TileFrameX = t.TileFrameY = 0;
        t.LiquidAmount = 0;
    }

    private static void Flower(int x, int y)
    {
        if (Main.tile[x, y].HasTile || Main.tile[x, y + 1].TileType != TileID.Grass) return;
        WorldGen.PlaceTile(x, y, TileID.Plants, true, false, -1, WorldGen.genRand.Next(6, 11));
    }

    private static void Observatory(int x, int[] surface, Terraria.Utilities.UnifiedRandom rand)
    {
        int width = 44, height = 26, floor = surface[x + width / 2];
        // level the plot
        for (int dx = -4; dx < width + 4; dx++)
        {
            for (int y = floor - height - 16; y < floor; y++) { Tile t = Main.tile[x + dx, y]; t.HasTile = false; t.LiquidAmount = 0; }
            for (int y = floor; y < floor + 8; y++) Place(x + dx, y, y == floor ? TileID.PearlstoneBrick : TileID.Stone);
        }
        // shell with a few collapsed gaps
        for (int dx = 0; dx < width; dx++)
            for (int dy = 0; dy < height; dy++)
            {
                int tx = x + dx, ty = floor - height + dy;
                bool shell = dy < 2 || dx < 2 || dx >= width - 2;
                bool doorway = (dx < 2 || dx >= width - 2) && dy >= height - 5;
                if (shell && !doorway && !(dy < 2 && rand.NextBool(7))) Place(tx, ty, TileID.PearlstoneBrick);
                else if (!shell) Main.tile[tx, ty].WallType = WallID.PearlstoneBrickUnsafe;
                if (dy == height - 12 && dx > 3 && dx < width - 4) Place(tx, ty, TileID.Platforms);
            }
        // glass dome
        int cx = x + width / 2, top = floor - height;
        for (int dx = -14; dx <= 14; dx++)
        {
            int rise = (int)(Math.Sqrt(196 - dx * dx) * .75);
            Place(cx + dx, top - rise, TileID.Glass);
            for (int y = top - rise + 1; y < top; y++) Main.tile[cx + dx, y].WallType = WallID.Glass;
        }
        // furnishings
        for (int dx = 5; dx < width - 6; dx += 9)
        {
            WorldGen.PlaceObject(x + dx, floor - 13, TileID.Bookcases);
            WorldGen.PlaceObject(x + dx + 4, floor - 13, TileID.Candles);
        }
        WorldGen.PlaceObject(x + 8, floor - 1, TileID.Tables);
        WorldGen.PlaceObject(x + 30, floor - 1, ModContent.TileType<ReverieVaseTile>());
        WorldGen.PlaceObject(x + 34, floor - 1, ModContent.TileType<ReverieVaseTile>());
        int chest = WorldGen.PlaceChest(x + 20, floor - 1, TileID.Containers, false, 0);
        if (chest >= 0)
        {
            Item[] loot = Main.chest[chest].item;
            loot[0].SetDefaults(ModContent.ItemType<MoonprismOre>()); loot[0].stack = 24;
            loot[1].SetDefaults(ModContent.ItemType<DewBerryCharm>());
            loot[2].SetDefaults(ModContent.ItemType<PrismCocoon>());
            loot[3].SetDefaults(ItemID.HealingPotion); loot[3].stack = 6;
            loot[4].SetDefaults(ItemID.FallenStar); loot[4].stack = 12;
        }
    }

    private static bool LootChest(int x, int y, Terraria.Utilities.UnifiedRandom rand, bool island)
    {
        int chest = WorldGen.PlaceChest(x, y, TileID.Containers, false, island ? 0 : 1);
        if (chest < 0) return false;
        Item[] loot = Main.chest[chest].item;
        loot[0].SetDefaults(ModContent.ItemType<MoonprismOre>()); loot[0].stack = rand.Next(8, 16);
        loot[1].SetDefaults(island ? ItemID.SwiftnessPotion : ItemID.SpelunkerPotion); loot[1].stack = rand.Next(1, 3);
        loot[2].SetDefaults(ItemID.HealingPotion); loot[2].stack = rand.Next(2, 5);
        if (rand.NextBool(3)) loot[3].SetDefaults(ModContent.ItemType<DewBerryCharm>());
        return true;
    }
}
