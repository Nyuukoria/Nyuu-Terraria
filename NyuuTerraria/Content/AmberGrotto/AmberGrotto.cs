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

namespace NyuuTerraria.Content.AmberGrotto;

public class AmberGrottoBiome : ModBiome
{
    public override SceneEffectPriority Priority => SceneEffectPriority.BiomeLow;
    public override int Music => MusicID.Underground;
    public override string BestiaryIcon => "NyuuTerraria/Content/AmberGrotto/AmberGrottoBiome";
    public override bool IsBiomeActive(Player player) =>
        (player.ZoneDirtLayerHeight || player.ZoneRockLayerHeight) &&
        ModContent.GetInstance<AmberGrottoSystem>().GrottoTileCount >= 85;
}

public class AmberGrottoSystem : ModSystem
{
    internal int GrottoTileCount { get; private set; }

    public override void TileCountsAvailable(ReadOnlySpan<int> tileCounts)
    {
        GrottoTileCount = tileCounts[ModContent.TileType<MossSlateTile>()] +
            tileCounts[ModContent.TileType<VerdigrisOreTile>()] +
            tileCounts[ModContent.TileType<EmberAmberOreTile>()];
    }

    public override void ModifyWorldGenTasks(List<GenPass> tasks)
    {
        int index = tasks.FindIndex(pass => pass.Name == "Skylight veins");
        if (index < 0)
            index = tasks.FindIndex(pass => pass.Name == "Shinies");
        if (index >= 0)
            tasks.Insert(index + 1, new PassLegacy("Amber Grotto pockets", Generate, 1f));
    }

    private static void Generate(GenerationProgress progress, GameConfiguration configuration)
    {
        progress.Message = "Growing Amber Grottos";
        int slate = ModContent.TileType<MossSlateTile>();
        int green = ModContent.TileType<VerdigrisOreTile>();
        int amber = ModContent.TileType<EmberAmberOreTile>();
        int urn = ModContent.TileType<AmberUrnTile>();
        int depth = Math.Clamp((int)Main.rockLayer, 130, Main.maxTilesY - 320);

        for (int pocket = 0; pocket < Main.maxTilesX / 900; pocket++)
        {
            int x = Main.maxTilesX * (pocket + 1) / (Main.maxTilesX / 900 + 1);
            x += WorldGen.genRand.Next(-65, 66);
            int y = depth + WorldGen.genRand.Next(35, 120);
            CarvePocket(x, y, slate, green, amber, urn);
        }
    }

    private static void CarvePocket(int centerX, int centerY, int slate, int green, int amber, int urn)
    {
        for (int dx = -53; dx <= 53; dx++)
        for (int dy = -28; dy <= 28; dy++)
        {
            float shape = dx * dx / 2809f + dy * dy / 784f;
            if (shape > 1f)
                continue;

            Tile tile = Main.tile[centerX + dx, centerY + dy];
            if (!tile.HasTile || tile.TileType is not (TileID.Stone or TileID.Dirt))
                continue;

            if (dx * dx / 484f + dy * dy / 144f < 1f)
            {
                tile.HasTile = false;
                continue;
            }

            tile.TileType = (ushort)slate;
        }

        for (int n = 0; n < 30; n++)
        {
            int x = centerX + WorldGen.genRand.Next(-40, 41);
            int y = centerY + WorldGen.genRand.Next(-23, 24);
            if (!Main.tile[x, y].HasTile || Main.tile[x, y].TileType != slate)
                continue;

            int type = n % 3 == 0 ? amber : green;
            for (int dx = -2; dx <= 2; dx++)
            for (int dy = -2; dy <= 2; dy++)
            {
                Tile tile = Main.tile[x + dx, y + dy];
                if (tile.HasTile && tile.TileType == slate && dx * dx + dy * dy <= 5)
                    tile.TileType = (ushort)type;
            }
        }

        for (int dx = -15; dx <= 15; dx += 5)
        {
            for (int dy = 2; dy <= 14; dy++)
            {
                int x = centerX + dx;
                int y = centerY + dy;
                if (!Main.tile[x, y].HasTile && Main.tile[x, y + 1].HasTile &&
                    WorldGen.PlaceObject(x, y, urn))
                    break;
            }
        }
    }
}

public class MossSlateTile : ModTile
{
    public override void SetStaticDefaults()
    {
        Main.tileSolid[Type] = true;
        Main.tileBlockLight[Type] = true;
        Main.tileMergeDirt[Type] = true;
        DustType = DustID.Stone;
        AddMapEntry(new Color(72, 106, 90), CreateMapEntryName());
    }
}

public class MossSlate : ModItem
{
    public override void SetDefaults() => Item.DefaultToPlaceableTile(ModContent.TileType<MossSlateTile>());

    public override void AddRecipes() => CreateRecipe(10).AddIngredient(ItemID.StoneBlock, 10)
        .AddIngredient(ItemID.GlowingMushroom).AddTile(TileID.WorkBenches).Register();
}

public class AmberUrnTile : ModTile
{
    public override void SetStaticDefaults()
    {
        Main.tileFrameImportant[Type] = true;
        Main.tileNoAttach[Type] = true;
        TileObjectData.newTile.CopyFrom(TileObjectData.Style2x2);
        TileObjectData.newTile.Origin = new Point16(0, 1);
        TileObjectData.newTile.CoordinateHeights = [16, 16];
        TileObjectData.addTile(Type);
        DustType = DustID.Clay;
        AddMapEntry(new Color(184, 123, 66), CreateMapEntryName());
    }

    public override IEnumerable<Item> GetItemDrops(int i, int j) => Array.Empty<Item>();

    public override void KillMultiTile(int i, int j, int frameX, int frameY)
    {
        if (Main.netMode == NetmodeID.MultiplayerClient)
            return;

        var source = new EntitySource_TileBreak(i, j);
        Rectangle area = new Rectangle(i * 16, j * 16, 32, 32);

        switch (Main.rand.Next(5))
        {
            case 0:
                Item.NewItem(source, area, ItemID.RecallPotion, 2);
                break;
            case 1:
                Item.NewItem(source, area, ItemID.LesserHealingPotion, 3);
                break;
            case 2:
                Item.NewItem(source, area, ItemID.Glowstick, 8);
                break;
            case 3:
                Item.NewItem(source, area, ModContent.ItemType<VerdigrisOre>(), 3);
                break;
            default:
                Item.NewItem(source, area, ItemID.SilverCoin, 5);
                break;
        }
    }
}

public class AmberUrn : ModItem
{
    public override void SetDefaults() => Item.DefaultToPlaceableTile(ModContent.TileType<AmberUrnTile>());

    public override void AddRecipes() => CreateRecipe().AddIngredient(ItemID.ClayBlock, 5)
        .AddIngredient<EmberAmberOre>().AddTile(TileID.Furnaces).Register();
}
