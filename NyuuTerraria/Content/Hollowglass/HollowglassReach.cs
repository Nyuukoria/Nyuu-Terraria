using System;
using System.Collections.Generic;
using Microsoft.Xna.Framework;
using Terraria;
using Terraria.DataStructures;
using Terraria.GameContent.Generation;
using Terraria.ID;
using Terraria.IO;
using Terraria.ModLoader;
using Terraria.ObjectData;
using Terraria.WorldBuilding;
using NyuuTerraria.Content.Skylight;

namespace NyuuTerraria.Content.Hollowglass;

public class HollowglassReachBiome : ModBiome
{
    public override string BestiaryIcon => "NyuuTerraria/Content/Hollowglass/HollowglassReachBiome";
    public override SceneEffectPriority Priority => SceneEffectPriority.BiomeLow;
    public override int Music => MusicID.Underground;
    public override bool IsBiomeActive(Player player) =>
        (player.ZoneDirtLayerHeight || player.ZoneRockLayerHeight) &&
        ModContent.GetInstance<HollowglassGeneration>().TileCount >= 100;
}

public class HollowglassGeneration : ModSystem
{
    internal int TileCount;
    private readonly List<Point> generatedRuins = new();

    public override void TileCountsAvailable(ReadOnlySpan<int> tileCounts) =>
        TileCount = tileCounts[ModContent.TileType<GlassSlateTile>()] +
            tileCounts[ModContent.TileType<HollowglassOreTile>()];

    public override void ModifyWorldGenTasks(List<GenPass> tasks)
    {
        int index = tasks.FindIndex(pass => pass.Name == "Amber Grotto pockets");
        if (index < 0)
            index = tasks.FindIndex(pass => pass.Name == "Shinies");
        if (index >= 0)
            tasks.Insert(index + 1, new PassLegacy("Hollowglass Reach", Generate, 1f));
    }

    private static void Generate(GenerationProgress progress, GameConfiguration configuration)
    {
        progress.Message = "Raising Hollowglass ruins";
        var system = ModContent.GetInstance<HollowglassGeneration>();
        system.generatedRuins.Clear();
        int slate = ModContent.TileType<GlassSlateTile>();
        int ore = ModContent.TileType<HollowglassOreTile>();
        int vase = ModContent.TileType<GlassVaseTile>();
        int lantern = ModContent.TileType<ReachLanternTile>();
        int pockets = Math.Max(2, Main.maxTilesX / 1400);

        for (int n = 0; n < pockets; n++)
        {
            int centerX = (int)(Main.maxTilesX * (n + 0.6f) / (pockets + 0.5f));
            int centerY = Math.Clamp((int)Main.rockLayer + 140 + n * 25, 150, Main.maxTilesY - 230);
            system.generatedRuins.Add(new Point(centerX, centerY));
            for (int dx = -65; dx <= 65; dx++)
            for (int dy = -36; dy <= 36; dy++)
            {
                if (dx * dx / 4225f + dy * dy / 1296f > 1f)
                    continue;
                Tile tile = Main.tile[centerX + dx, centerY + dy];
                if (tile.HasTile && tile.TileType is not (TileID.Stone or TileID.Dirt))
                    continue;
                bool chamber = Math.Abs(dx) < 34 && dy > -15 && dy < 12;
                if (chamber)
                {
                    tile.HasTile = false;
                    tile.LiquidAmount = 0;
                    WorldGen.PlaceWall(centerX + dx, centerY + dy, ModContent.WallType<GlassSlateWall>(), true);
                }
                else if (tile.HasTile)
                    tile.TileType = (ushort)slate;
            }

            for (int dx = -35; dx <= 35; dx++)
            {
                Tile floor = Main.tile[centerX + dx, centerY + 12];
                if (!floor.HasTile || floor.TileType == slate)
                {
                    floor.HasTile = true;
                    floor.TileType = (ushort)slate;
                }
            }

            for (int vein = 0; vein < 40; vein++)
            {
                int x = centerX + WorldGen.genRand.Next(-55, 56);
                int y = centerY + WorldGen.genRand.Next(-30, 31);
                for (int dx = -2; dx <= 2; dx++)
                for (int dy = -2; dy <= 2; dy++)
                {
                    Tile tile = Main.tile[x + dx, y + dy];
                    if (tile.HasTile && tile.TileType == slate && dx * dx + dy * dy <= 5)
                        tile.TileType = (ushort)ore;
                }
            }

            for (int dx = -28; dx <= 28; dx += 14)
                WorldGen.PlaceObject(centerX + dx, centerY + 11, vase);
            WorldGen.PlaceObject(centerX - 20, centerY + 11, lantern);
            WorldGen.PlaceObject(centerX + 20, centerY + 11, lantern);

        }
    }

    public override void PostWorldGen()
    {
        foreach (Point ruin in generatedRuins)
        {
            int chestIndex = PlaceRuinChest(ruin);
            if (chestIndex < 0)
                continue;
            Chest loot = Main.chest[chestIndex];
            loot.item[0].SetDefaults(ModContent.ItemType<HollowglassOre>());
            loot.item[0].stack = WorldGen.genRand.Next(12, 19);
            loot.item[1].SetDefaults(ModContent.ItemType<GlassboundSeal>());
            loot.item[2].SetDefaults(ItemID.SpelunkerPotion);
            loot.item[2].stack = 2;
            loot.item[3].SetDefaults(ItemID.HealingPotion);
            loot.item[3].stack = 3;
        }
        generatedRuins.Clear();
    }

    private static int PlaceRuinChest(Point ruin)
    {
        int slate = ModContent.TileType<GlassSlateTile>();
        for (int dy = -12; dy <= 24; dy++)
        for (int dx = -30; dx <= 28; dx++)
        {
            int x = ruin.X + dx;
            int y = ruin.Y + dy;
            if (!Main.tile[x, y].HasTile || Main.tile[x, y].TileType != slate ||
                !Main.tile[x + 1, y].HasTile || !Main.tileSolid[Main.tile[x + 1, y].TileType] ||
                Main.tile[x, y - 1].HasTile || Main.tile[x + 1, y - 1].HasTile ||
                Main.tile[x, y - 2].HasTile || Main.tile[x + 1, y - 2].HasTile)
                continue;
            int placed = WorldGen.PlaceChest(x, y - 1, (ushort)ModContent.TileType<ReachChestTile>());
            if (placed >= 0)
                return placed;
        }
        return -1;
    }
}

public class GlassSlateTile : ModTile
{
    public override void SetStaticDefaults()
    {
        Main.tileSolid[Type] = true;
        Main.tileBlockLight[Type] = true;
        Main.tileMergeDirt[Type] = true;
        DustType = DustID.Stone;
        AddMapEntry(new Color(62, 102, 110), CreateMapEntryName());
    }
}

public class GlassSlateWall : ModWall
{
    public override void SetStaticDefaults() => AddMapEntry(new Color(32, 57, 65));
}

public class HollowglassOreTile : VeinTile
{
    protected override Color MapColor => new Color(100, 192, 192);
}

public class GlassSlate : ModItem
{
    public override void SetDefaults() => Item.DefaultToPlaceableTile(ModContent.TileType<GlassSlateTile>());
    public override void AddRecipes() => CreateRecipe(10).AddIngredient(ItemID.StoneBlock, 10)
        .AddIngredient(ItemID.Glass, 2).AddTile(TileID.Furnaces).Register();
}

public class HollowglassOre : VeinOre
{
    protected override int OreTile => ModContent.TileType<HollowglassOreTile>();
}

public class HollowglassBar : VeinBar
{
    public override void AddRecipes()
    {
        CreateRecipe().AddIngredient<HollowglassOre>(3).AddTile(TileID.Furnaces).Register();
        CreateRecipe().AddIngredient(ItemID.Glass, 4).AddIngredient(ItemID.SilverBar)
            .AddIngredient(ItemID.Amethyst).AddTile(TileID.Furnaces).Register();
        CreateRecipe().AddIngredient(ItemID.Glass, 4).AddIngredient(ItemID.TungstenBar)
            .AddIngredient(ItemID.Amethyst).AddTile(TileID.Furnaces).Register();
    }
}

public class GlassVaseTile : ModTile
{
    public override void SetStaticDefaults()
    {
        Main.tileFrameImportant[Type] = true;
        Main.tileNoAttach[Type] = true;
        TileObjectData.newTile.CopyFrom(TileObjectData.Style2x2);
        TileObjectData.newTile.Origin = new Point16(0, 1);
        TileObjectData.newTile.CoordinateHeights = [16, 16];
        TileObjectData.addTile(Type);
        DustType = DustID.Glass;
        AddMapEntry(new Color(119, 174, 179), CreateMapEntryName());
    }

    public override IEnumerable<Item> GetItemDrops(int i, int j) => Array.Empty<Item>();

    public override void KillMultiTile(int i, int j, int frameX, int frameY)
    {
        if (Main.netMode == NetmodeID.MultiplayerClient)
            return;
        var source = new EntitySource_TileBreak(i, j);
        var area = new Rectangle(i * 16, j * 16, 32, 32);
        switch (Main.rand.Next(4))
        {
            case 0: Item.NewItem(source, area, ItemID.HealingPotion, 2); break;
            case 1: Item.NewItem(source, area, ModContent.ItemType<HollowglassOre>(), 3); break;
            case 2: Item.NewItem(source, area, ItemID.SpelunkerPotion); break;
            default: Item.NewItem(source, area, ItemID.SilverCoin, 10); break;
        }
    }
}

public class GlassVase : ModItem
{
    public override void SetDefaults() => Item.DefaultToPlaceableTile(ModContent.TileType<GlassVaseTile>());
    public override void AddRecipes() => CreateRecipe().AddIngredient(ItemID.Glass, 4)
        .AddIngredient<HollowglassOre>().AddTile(TileID.Furnaces).Register();
}

public class ReachLantern : ModItem
{
    public override void SetDefaults() => Item.DefaultToPlaceableTile(ModContent.TileType<ReachLanternTile>());
    public override void AddRecipes() => CreateRecipe().AddIngredient<HollowglassBar>()
        .AddIngredient(ItemID.Torch, 2).AddTile(TileID.WorkBenches).Register();
}

public class ReachLanternTile : ModTile
{
    public override void SetStaticDefaults()
    {
        Main.tileFrameImportant[Type] = true;
        Main.tileLighted[Type] = true;
        Main.tileNoAttach[Type] = true;
        TileObjectData.newTile.CopyFrom(TileObjectData.Style1x2);
        TileObjectData.addTile(Type);
        DustType = DustID.Glass;
        AddMapEntry(new Color(77, 195, 188), CreateMapEntryName());
    }

    public override void ModifyLight(int i, int j, ref float r, ref float g, ref float b)
    {
        r = 0.12f;
        g = 0.65f;
        b = 0.58f;
    }
}

public class ReachChest : ModItem
{
    public override void SetDefaults() => Item.DefaultToPlaceableTile(ModContent.TileType<ReachChestTile>());
    public override void AddRecipes() => CreateRecipe().AddIngredient<GlassSlate>(8)
        .AddIngredient<HollowglassBar>(2).AddTile(TileID.WorkBenches).Register();
}

public class ReachChestTile : SkylightChestTile { }
