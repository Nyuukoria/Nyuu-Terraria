using System;
using System.IO;
using Terraria;
using Terraria.ID;
using Terraria.IO;
using Terraria.ModLoader;
using Terraria.WorldBuilding;
using NyuuTerraria.Content.Dimensions;
namespace NyuuTerraria.Content;

// Export-only fixture, never included in the playable mod. Runs on a dedicated server whose save
// path contains "dimension-test": generates every dimension through the real pipeline, reloads the
// saved file, checks it, and writes a raw RGB map (<Id>.map + <Id>.map.txt) for inspection.
public class DimensionChecks : ModSystem
{
    private static bool running;

    public override void PostWorldLoad()
    {
        if (running) return; // LoadWorld below fires this hook again
        running = true;
        if (!Main.dedServ || !Main.SavePath.Contains("dimension-test", StringComparison.OrdinalIgnoreCase)) return;
        try
        {
            string report = "";
            foreach (Dimension d in DimensionSystem.All)
            {
                string path = DimensionSystem.PathFor(Guid.Empty, d);
                Directory.CreateDirectory(Path.GetDirectoryName(path));
                WorldFileData data = DimensionSystem.NewMetadata(d, path, 0);
                data.SetAsActive();
                Main.worldName = d.DisplayName;
                var progress = new GenerationProgress { TotalWeight = 1 };
                var gen = System.Diagnostics.Stopwatch.StartNew();
                if (!DimensionSystem.Build(d, data, progress)) throw new Exception(d.Id + ": Build returned false (see log)");
                if (!File.Exists(path)) throw new Exception(d.Id + ": no world file written");
                report += $"{d.Id}: generated and saved in {gen.ElapsedMilliseconds} ms\n";

                // reload from disk exactly as the client would
                WorldFileData read = WorldFile.GetAllMetadata(path, false);
                if (read == null || !read.IsValid) throw new Exception(d.Id + ": saved world is not valid");
                read.SetAsActive();
                var clock = System.Diagnostics.Stopwatch.StartNew();
                WorldFile.LoadWorld();
                report += $"{d.Id}: loaded in {clock.ElapsedMilliseconds} ms, file {new FileInfo(path).Length / 1024} KB\n";
                if (Main.maxTilesX != d.Width || Main.maxTilesY != d.Height) throw new Exception(d.Id + ": wrong size after load " + Main.maxTilesX + "x" + Main.maxTilesY);
                if (DimensionSystem.Current != d) throw new Exception(d.Id + ": not recognised as a dimension after load");
                Tile ground = Main.tile[Main.spawnTileX, Main.spawnTileY];
                if (!ground.HasTile || !Main.tileSolid[ground.TileType]) throw new Exception(d.Id + ": spawn is not on solid ground");
                for (int y = Main.spawnTileY - 3; y < Main.spawnTileY; y++)
                    if (Main.tile[Main.spawnTileX, y].HasTile && Main.tileSolid[Main.tile[Main.spawnTileX, y].TileType]) throw new Exception(d.Id + ": spawn is buried");

                int solid = 0, chests = 0;
                for (int x = 0; x < Main.maxTilesX; x++) for (int y = 0; y < Main.maxTilesY; y++) if (Main.tile[x, y].HasTile) solid++;
                foreach (Chest c in Main.chest) if (c != null) chests++;
                if (chests < 5) throw new Exception(d.Id + ": only " + chests + " chests placed");
                report += $"{d.Id}: {Main.maxTilesX}x{Main.maxTilesY}, {solid} tiles, {chests} chests, spawn {Main.spawnTileX},{Main.spawnTileY}\n";
                WriteMap(Path.Combine(Main.SavePath, d.Id + ".map"));
            }
            File.WriteAllText(Path.Combine(Main.SavePath, "dimension-result.txt"), report);
            Console.WriteLine("DIMENSIONS_OK\n" + report);
            Environment.Exit(0);
        }
        catch (Exception e)
        {
            File.WriteAllText(Path.Combine(Main.SavePath, "dimension-failure.txt"), e.ToString());
            Console.WriteLine(e);
            Environment.Exit(1);
        }
    }

    private static void WriteMap(string file)
    {
        int w = Main.maxTilesX, h = Main.maxTilesY;
        byte[] rgb = new byte[w * h * 3];
        for (int y = 0; y < h; y++)
        for (int x = 0; x < w; x++)
        {
            Tile t = Main.tile[x, y];
            (int r, int g, int b) c = y < Main.worldSurface ? (150, 130, 190) : (40, 34, 48);
            if (t.WallType > 0) c = (70, 58, 60);
            if (t.LiquidAmount > 0) c = (60, 110, 220);
            if (t.HasTile)
            {
                ushort k = t.TileType;
                c = k == TileID.Grass ? (70, 170, 70) : k == TileID.Dirt ? (130, 90, 60) : k == TileID.Stone ? (110, 110, 118)
                  : k == TileID.Sand ? (220, 200, 120) : k == TileID.PearlstoneBrick || k == TileID.Pearlstone ? (230, 210, 230)
                  : k == TileID.Glass ? (150, 230, 240) : k == TileID.Trees ? (110, 70, 40) : k == TileID.Containers ? (255, 40, 40)
                  : k >= TileID.Count ? (255, 120, 200) : (200, 200, 90);
            }
            int i = (y * w + x) * 3;
            rgb[i] = (byte)c.r; rgb[i + 1] = (byte)c.g; rgb[i + 2] = (byte)c.b;
        }
        File.WriteAllBytes(file, rgb);
        File.WriteAllText(file + ".txt", w + " " + h);
    }
}
