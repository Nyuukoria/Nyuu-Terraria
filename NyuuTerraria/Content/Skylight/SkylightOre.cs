using System.Collections.Generic;
using Microsoft.Xna.Framework;
using Terraria;
using Terraria.GameContent.Generation;
using Terraria.ID;
using Terraria.IO;
using Terraria.ModLoader;
using Terraria.WorldBuilding;

namespace NyuuTerraria.Content.Skylight;

public class SkylightOreTile : ModTile
{
    public override void SetStaticDefaults()
    {
        Main.tileSolid[Type] = true;
        Main.tileBlockLight[Type] = true;
        Main.tileSpelunker[Type] = true;
        Main.tileOreFinderPriority[Type] = 235;
        Main.tileMergeDirt[Type] = true;
        TileID.Sets.Ore[Type] = true;
        DustType = DustID.Silver;
        MineResist = 1.2f;
        AddMapEntry(new Color(189, 155, 212), CreateMapEntryName());
    }
}

public class SkylightOre : ModItem
{
    public override void SetDefaults()
    {
        Item.DefaultToPlaceableTile(ModContent.TileType<SkylightOreTile>());
        Item.width = 20;
        Item.height = 20;
        Item.value = Item.sellPrice(copper: 90);
    }
}

public class SkylightBar : ModItem
{
    public override void SetDefaults()
    {
        Item.width = 24;
        Item.height = 18;
        Item.maxStack = Item.CommonMaxStack;
        Item.value = Item.sellPrice(silver: 4);
        Item.rare = ItemRarityID.Blue;
    }

    public override void AddRecipes()
    {
        CreateRecipe().AddIngredient<SkylightOre>(3).AddTile(TileID.Furnaces).Register();
        CreateRecipe().AddRecipeGroup("IronBar", 2)
            .AddIngredient(ItemID.Cloud, 3).AddTile(TileID.Furnaces).Register();
    }
}

public class SkylightGeneration : ModSystem
{
    public override void ModifyWorldGenTasks(List<GenPass> tasks)
    {
        int position = tasks.FindIndex(pass => pass.Name == "Shinies");
        if (position < 0)
            return;

        var orePass = new PassLegacy("Skylight veins", GenerateOre, 1f);
        tasks.Insert(position + 1, orePass);

    }

    private static void GenerateOre(GenerationProgress progress, GameConfiguration configuration)
    {
        progress.Message = "Seeding Skylight veins";
        int type = ModContent.TileType<SkylightOreTile>();
        int top = (int)Main.worldSurface + 10;
        int bottom = (int)MathHelper.Lerp((float)Main.rockLayer, Main.maxTilesY - 210, 0.50f);
        if (bottom <= top)
            return;

        for (int n = 0; n < Main.maxTilesX / 8; n++)
        {
            int x = WorldGen.genRand.Next(40, Main.maxTilesX - 40);
            int y = WorldGen.genRand.Next(top, bottom);
            if (Main.tile[x, y].HasTile && Main.tile[x, y].TileType == TileID.Stone)
                WorldGen.OreRunner(x, y, WorldGen.genRand.Next(4, 8),
                    WorldGen.genRand.Next(4, 8), (ushort)type);
        }
    }
}
