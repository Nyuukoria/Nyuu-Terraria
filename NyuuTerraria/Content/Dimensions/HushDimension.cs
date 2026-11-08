using System;
using Terraria;
using Terraria.ID;
using Terraria.ModLoader;
using Terraria.Utilities;
using Terraria.WorldBuilding;
using NyuuTerraria.Content.Frontiers;
using NyuuTerraria.Content.Reverie;

namespace NyuuTerraria.Content.Dimensions;

/// <summary>
/// The Hush as a world of its own: an abandoned mining town under a starless sky, a mine shaft
/// down to a forgotten archive, and a dark manor on the eastern ridge. Quiet, dim, and finite in loot.
/// </summary>
public class HushDimension : Dimension
{
    public override string Id => "Hush";
    public override string DisplayName => "The Hush";
    public override int Width => 1600;
    public override int Height => 900;
    public override int Realm => 1;

    private const int Ground = 300;
    private const int Margin = 60;

    public override void Generate(GenerationProgress progress)
    {
        UnifiedRandom rand = WorldGen.genRand;
        int w = Main.maxTilesX, h = Main.maxTilesY;
        int[] surface = new int[w];
        Main.worldSurface = Ground + 30;
        Main.rockLayer = Ground + 80;

        // ---- grey, ash-capped hills
        progress.Message = "Silencing the hills";
        double p1 = rand.NextDouble() * 6, p2 = rand.NextDouble() * 6;
        for (int x = 0; x < w; x++)
        {
            double y = Ground + Math.Sin(x * .007 + p1) * 12 + Math.Sin(x * .023 + p2) * 5 + Math.Sin(x * .09) * 1.5;
            surface[x] = (int)y;
            for (int ty = surface[x]; ty < h; ty++)
            {
                int type = ty == surface[x] ? TileID.Ash : ty < surface[x] + 10 ? TileID.Silt : TileID.Stone;
                Put(x, ty, type);
                if (ty > surface[x] + 2) Main.tile[x, ty].WallType = ty < Main.rockLayer ? WallID.DirtUnsafe : WallID.Stone;
            }
            if (x % 80 == 0) progress.Set(x / (double)w * .2);
        }

        // ---- level plots for the three places
        Level(surface, 260, 700);        // mining town
        Level(surface, 1130, 1420);      // manor grounds

        // ---- caves and a little ore
        progress.Message = "Hollowing the old seams";
        for (int n = 0; n < 60; n++)
            WorldGen.TileRunner(rand.Next(Margin, w - Margin), rand.Next(Ground + 50, h - 120), rand.Next(7, 14), rand.Next(40, 100), -1);
        for (int n = 0; n < 90; n++)
            WorldGen.OreRunner(rand.Next(Margin, w - Margin), rand.Next(Ground + 30, h - 80), rand.Next(3, 5), rand.Next(3, 7),
                (ushort)(n % 3 == 0 ? TileID.Silver : n % 3 == 1 ? TileID.Iron : TileID.Lead));
        progress.Set(.35);

        // ---- the mining town
        progress.Message = "Boarding up the mining town";
        int[] shacks = { 290, 350, 415, 485, 560, 630 };
        for (int i = 0; i < shacks.Length; i++)
            Shack(shacks[i], surface[shacks[i]], 18 + i % 3 * 3, 11 + i % 2 * 3, rand, i);
        for (int x = 270; x < 700; x += 37)
            if (!Main.tile[x, surface[x] - 1].HasTile) WorldGen.PlaceObject(x, surface[x] - 1, TileID.Lampposts);
        progress.Set(.5);

        // ---- the mine shaft down to the archive
        progress.Message = "Unspooling the mine rope";
        int shaftX = 530, archiveTop = Ground + 70;
        for (int y = surface[shaftX] - 2; y < archiveTop + 4; y++)
        {
            for (int dx = -2; dx <= 2; dx++) Clear(shaftX + dx, y);
            Main.tile[shaftX - 3, y].WallType = WallID.Wood;
            Main.tile[shaftX + 3, y].WallType = WallID.Wood;
            if (y % 9 == 0) { Put(shaftX - 3, y, TileID.WoodenBeam); Put(shaftX + 3, y, TileID.WoodenBeam); }
            Put(shaftX, y, TileID.Rope);
        }

        // ---- the forgotten archive
        progress.Message = "Shelving the forgotten archive";
        Archive(shaftX - 10, archiveTop, 74, 30, rand);
        progress.Set(.65);

        // ---- the manor
        progress.Message = "Locking the manor";
        Manor(1180, surface[1180], 96, 46, rand);
        progress.Set(.75);

        // ---- charred trees between the places
        for (int x = Margin + 30; x < w - Margin - 30; x += 29)
        {
            if (x > 250 && x < 710 || x > 1120 && x < 1430) continue;
            if (rand.NextBool(2)) ExpeditionScenery.Tree(x, surface[x], 12, x, charred: true);
        }

        // ---- spawn
        int spawnX = 200, sy = surface[spawnX];
        for (int x = spawnX - 6; x <= spawnX + 6; x++)
        {
            for (int y = sy - 8; y < sy; y++) Clear(x, y);
            Put(x, sy, TileID.GrayBrick);
        }
        WorldGen.PlaceObject(spawnX - 5, sy - 1, TileID.Lampposts);
        Main.spawnTileX = spawnX;
        Main.spawnTileY = sy;

        // ---- frame everything
        progress.Message = "Letting the dust settle";
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

    // ------------------------------------------------------------------ helpers

    private static void Put(int x, int y, int type)
    {
        if (!WorldGen.InWorld(x, y, 2)) return;
        Tile t = Main.tile[x, y];
        t.HasTile = true; t.TileType = (ushort)type; t.TileFrameX = t.TileFrameY = 0; t.LiquidAmount = 0;
    }

    private static void Clear(int x, int y)
    {
        if (!WorldGen.InWorld(x, y, 2)) return;
        Tile t = Main.tile[x, y];
        t.HasTile = false; t.LiquidAmount = 0;
    }

    private static void Level(int[] surface, int from, int to)
    {
        int ground = 0;
        for (int x = from; x < to; x++) ground += surface[x];
        ground /= to - from;
        for (int x = from - 12; x < to + 12; x++)
        {
            int target = x < from || x >= to
                ? (int)Math.Round(surface[x] + (ground - surface[x]) * (1 - Math.Min(Math.Abs(x - from), Math.Abs(x - to)) / 12.0))
                : ground;
            for (int y = Math.Min(surface[x], target) - 1; y < Math.Max(surface[x], target) + 1; y++)
                if (y < target) Clear(x, y); else Put(x, y, y == target ? TileID.Ash : TileID.Silt);
            surface[x] = target;
        }
    }

    private static void Floor(int left, int right, int y, int type)
    {
        for (int x = left; x <= right; x++) Put(x, y, type);
    }

    private static void Shack(int x, int floor, int width, int height, UnifiedRandom rand, int index)
    {
        int left = x - width / 2, right = left + width - 1, top = floor - height;
        for (int tx = left; tx <= right; tx++)
            for (int ty = top; ty < floor; ty++)
            {
                Clear(tx, ty);
                bool wall = tx == left || tx == right;
                bool doorGap = wall && ty >= floor - 3;
                if (ty == top) Put(tx, ty, TileID.WoodBlock);                                  // roof
                else if (wall && !doorGap && !(rand.NextBool(6))) Put(tx, ty, TileID.WoodBlock); // weathered walls
                else if (!wall && rand.Next(10) > 1) Main.tile[tx, ty].WallType = WallID.Wood;  // missing boards
            }
        for (int tx = left - 1; tx <= right + 1; tx++) Put(tx, top - 1, TileID.Platforms);   // eaves
        WorldGen.PlaceObject(left + 3, floor - 1, TileID.Tables);
        WorldGen.PlaceObject(left + 6, floor - 1, TileID.Chairs);
        if (index % 2 == 0) WorldGen.PlaceObject(right - 3, floor - 1, TileID.Bookcases);
        Put(left + 1, top + 1, TileID.Cobweb); Put(right - 1, top + 1, TileID.Cobweb); Put(right - 2, top + 1, TileID.Cobweb);
        if (index == 2 || index == 5)
            Loot(right - 4, floor - 1, rand, ItemID.SpelunkerPotion);
    }

    private static void Archive(int left, int top, int width, int height, UnifiedRandom rand)
    {
        int right = left + width, floor = top + height;
        for (int x = left - 2; x <= right + 2; x++)
            for (int y = top - 2; y <= floor + 2; y++)
            {
                bool shell = x < left || x > right || y < top || y > floor;
                if (shell) Put(x, y, TileID.GrayBrick);
                else { Clear(x, y); Main.tile[x, y].WallType = WallID.GrayBrick; }
            }
        for (int x = left + 8; x <= left + 12; x++) Clear(x, top - 1);  // where the shaft comes in
        for (int x = left + 8; x <= left + 12; x++) Clear(x, top - 2);
        // two reading galleries
        Floor(left, right, top + 12, TileID.Platforms);
        Floor(left + 14, right, top + 22, TileID.Platforms);
        for (int x = left + 4; x < right - 4; x += 6)
        {
            WorldGen.PlaceObject(x, floor, TileID.Bookcases);
            WorldGen.PlaceObject(x + 1, top + 11, TileID.Bookcases);
        }
        for (int x = left + 16; x < right - 3; x += 3) WorldGen.PlaceTile(x, top + 21, TileID.Books, true, false, -1, rand.Next(6));
        for (int x = left + 6; x < right; x += 11) WorldGen.PlaceObject(x, floor, TileID.Candelabras);
        for (int i = 0; i < 6; i++) Put(left + rand.Next(width), top + rand.Next(height / 2), TileID.Cobweb);
        Loot(right - 6, floor, rand, ModContent.ItemType<ArchiveLantern>());
        Loot(left + 20, top + 11, rand, ItemID.BattlePotion);
    }

    private static void Manor(int left, int floor, int width, int height, UnifiedRandom rand)
    {
        int right = left + width, top = floor - height;
        for (int x = left; x <= right; x++)
            for (int y = top; y < floor; y++)
            {
                Clear(x, y);
                bool shell = x == left || x == left + 1 || x == right || x == right - 1 || y == top || y == top + 1;
                bool door = (x <= left + 1 || x >= right - 1) && y >= floor - 3;
                if (shell && !door) Put(x, y, TileID.StoneSlab);
                else if (!shell) Main.tile[x, y].WallType = WallID.GrayBrick;
            }
        // pitched roof
        for (int i = 0; i < width / 2; i++)
        {
            int y = top - 1 - i / 2;
            Put(left + i, y, TileID.BlueDynastyShingles); Put(right - i, y, TileID.BlueDynastyShingles);
            for (int yy = y + 1; yy < top; yy++) { Main.tile[left + i, yy].WallType = WallID.Wood; Main.tile[right - i, yy].WallType = WallID.Wood; }
        }
        // three floors and partitions with doorways
        int[] levels = { floor - 15, floor - 30 };
        foreach (int y in levels) Floor(left + 2, right - 2, y, TileID.Platforms);
        foreach (int px in new[] { left + width / 3, left + 2 * width / 3 })
            for (int y = top + 2; y < floor; y++)
            {
                bool gap = (y > floor - 4) || (y > levels[0] - 4 && y < levels[0]) || (y > levels[1] - 4 && y < levels[1]);
                if (!gap) Put(px, y, TileID.StoneSlab);
            }
        // furnishings: clocks, pianos, candelabras, cobwebs
        WorldGen.PlaceObject(left + 8, floor - 1, TileID.GrandfatherClocks);
        WorldGen.PlaceObject(left + 44, floor - 1, TileID.Pianos);
        WorldGen.PlaceObject(left + 70, levels[0] - 1, TileID.Bookcases);
        WorldGen.PlaceObject(left + 20, levels[1] - 1, TileID.Tables);
        for (int x = left + 6; x < right - 6; x += 15)
        {
            WorldGen.PlaceObject(x, floor - 1, TileID.Candelabras);
            WorldGen.PlaceObject(x + 4, levels[0] - 1, TileID.Candelabras);
        }
        for (int i = 0; i < 14; i++) Put(left + 3 + rand.Next(width - 6), top + 2 + rand.Next(height - 5), TileID.Cobweb);
        // the sigil that wakes the Bellkeeper waits upstairs
        int chest = WorldGen.PlaceChest(right - 12, levels[1] - 1, TileID.Containers, false, 1);
        if (chest >= 0)
        {
            Item[] loot = Main.chest[chest].item;
            loot[0].SetDefaults(ModContent.ItemType<CrackedBellSigil>());
            loot[1].SetDefaults(ModContent.ItemType<EchoFragment>()); loot[1].stack = 20;
            loot[2].SetDefaults(ItemID.HealingPotion); loot[2].stack = 6;
        }
        Loot(left + 30, floor - 1, rand, ItemID.NightOwlPotion);
    }

    private static void Loot(int x, int y, UnifiedRandom rand, int bonus)
    {
        int chest = WorldGen.PlaceChest(x, y, TileID.Containers, false, 1);
        if (chest < 0) return;
        Item[] loot = Main.chest[chest].item;
        loot[0].SetDefaults(ModContent.ItemType<EchoFragment>()); loot[0].stack = rand.Next(6, 12);
        loot[1].SetDefaults(bonus); loot[1].stack = 1 + (ContentSamples.ItemsByType[bonus].maxStack > 1 ? rand.Next(2) : 0);
        loot[2].SetDefaults(ItemID.HealingPotion); loot[2].stack = rand.Next(2, 5);
        loot[3].SetDefaults(ItemID.Bone); loot[3].stack = rand.Next(5, 15);
    }
}
