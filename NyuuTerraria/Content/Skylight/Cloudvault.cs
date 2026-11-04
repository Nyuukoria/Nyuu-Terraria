using System.Collections.Generic;
using Terraria;
using Terraria.ID;
using Terraria.ModLoader;
using Terraria.ModLoader.IO;

namespace NyuuTerraria.Content.Skylight;

public class Cloudvault : ModItem
{
    public override void SetDefaults()
    {
        Item.DefaultToPlaceableTile(ModContent.TileType<CloudvaultTile>());
        Item.width = 32;
        Item.height = 28;
    }

    public override void AddRecipes() => CreateRecipe().AddIngredient<SkylightChest>()
        .AddIngredient<StormglassBar>(4).AddIngredient<FrostveilBar>(4)
        .AddIngredient(ItemID.Cloud, 10).AddTile(TileID.WorkBenches).Register();
}

public class CloudvaultTile : SkylightChestTile
{
    internal const int Capacity = 160;

    public override bool RightClick(int i, int j)
    {
        if (Main.netMode == NetmodeID.SinglePlayer)
        {
            Tile tile = Main.tile[i, j];
            int index = Chest.FindChest(i - tile.TileFrameX % 36 / 18, j - tile.TileFrameY / 18);
            if (index >= 0)
                EnsureCapacity(Main.chest[index]);
        }

        return base.RightClick(i, j);
    }

    internal static void EnsureCapacity(Chest chest)
    {
        if (chest.maxItems < Capacity)
            chest.Resize(Capacity);
    }
}

public class CloudvaultStorage : ModSystem
{
    public override void OnWorldLoad() => ResizeVaults();

    public override void PostUpdateWorld()
    {
        if (Main.GameUpdateCount % 20 == 0)
            ResizeVaults();
    }

    public override void SaveWorldData(TagCompound tag)
    {
        var items = new List<TagCompound>();
        foreach (Chest chest in Main.chest)
        {
            if (!IsVault(chest))
                continue;

            for (int slot = Chest.DefaultMaxItems; slot < chest.maxItems; slot++)
            {
                if (chest.item[slot].IsAir)
                    continue;

                items.Add(new TagCompound
                {
                    ["x"] = chest.x,
                    ["y"] = chest.y,
                    ["slot"] = slot,
                    ["item"] = ItemIO.Save(chest.item[slot])
                });
            }
        }

        if (items.Count > 0)
            tag["extraVaultItems"] = items;
    }

    public override void LoadWorldData(TagCompound tag)
    {
        foreach (TagCompound entry in tag.GetList<TagCompound>("extraVaultItems"))
        {
            int index = Chest.FindChest(entry.GetInt("x"), entry.GetInt("y"));
            int slot = entry.GetInt("slot");
            if (index < 0 || slot < Chest.DefaultMaxItems || slot >= CloudvaultTile.Capacity ||
                !IsVault(Main.chest[index]))
                continue;

            Chest chest = Main.chest[index];
            CloudvaultTile.EnsureCapacity(chest);
            chest.item[slot] = ItemIO.Load(entry.GetCompound("item"));
        }
    }

    private static void ResizeVaults()
    {
        if (Main.netMode == NetmodeID.MultiplayerClient)
            return;

        foreach (Chest chest in Main.chest)
            if (chest != null && chest.maxItems < CloudvaultTile.Capacity && IsVault(chest))
                CloudvaultTile.EnsureCapacity(chest);
    }

    private static bool IsVault(Chest chest)
    {
        if (chest == null || !WorldGen.InWorld(chest.x, chest.y, 1))
            return false;

        Tile tile = Main.tile[chest.x, chest.y];
        return tile.HasTile && tile.TileType == ModContent.TileType<CloudvaultTile>();
    }
}

public class CloudSatchel : ModItem
{
    public override void SetDefaults()
    {
        Item.CloneDefaults(ItemID.MoneyTrough);
        Item.width = 26;
        Item.height = 24;
        Item.rare = ItemRarityID.Blue;
    }

    public override void AddRecipes() => CreateRecipe().AddIngredient<SkylightBar>(4)
        .AddIngredient(ItemID.Cloud, 8).AddIngredient(ItemID.Silk, 6)
        .AddTile(TileID.Loom).Register();
}
