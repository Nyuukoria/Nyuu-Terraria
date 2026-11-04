using Microsoft.Xna.Framework;
using Terraria;
using Terraria.Audio;
using Terraria.DataStructures;
using Terraria.GameContent.ObjectInteractions;
using Terraria.ID;
using Terraria.Localization;
using Terraria.ModLoader;
using Terraria.ObjectData;

namespace NyuuTerraria.Content.Skylight;

public class SkylightChest : ModItem
{
    public override void SetDefaults()
    {
        Item.DefaultToPlaceableTile(ModContent.TileType<SkylightChestTile>());
        Item.width = 32;
        Item.height = 28;
        Item.value = Item.sellPrice(silver: 20);
    }

    public override void AddRecipes() => CreateRecipe().AddRecipeGroup("Wood", 8)
        .AddIngredient<SkylightBar>(2).AddTile(TileID.WorkBenches).Register();
}

public class SkylightChestTile : ModTile
{
    public override void SetStaticDefaults()
    {
        Main.tileContainer[Type] = true;
        Main.tileFrameImportant[Type] = true;
        Main.tileNoAttach[Type] = true;
        TileID.Sets.BasicChest[Type] = true;
        TileID.Sets.IsAContainer[Type] = true;
        TileID.Sets.DisableSmartCursor[Type] = true;
        TileID.Sets.InteractableByNPCs[Type] = true;
        AddMapEntry(new Color(166, 125, 177), CreateMapEntryName());
        AdjTiles = [TileID.Containers];
        VanillaFallbackOnModDeletion = TileID.Containers;
        DustType = DustID.WoodFurniture;

        TileObjectData.newTile.CopyFrom(TileObjectData.Style2x2);
        TileObjectData.newTile.Origin = new Point16(0, 1);
        TileObjectData.newTile.CoordinateHeights = [16, 18];
        TileObjectData.newTile.HookCheckIfCanPlace = new PlacementHook(Chest.FindEmptyChest, -1, 0, true);
        TileObjectData.newTile.HookPostPlaceMyPlayer = new PlacementHook(Chest.AfterPlacement_Hook, -1, 0, false);
        TileObjectData.newTile.StyleHorizontal = true;
        TileObjectData.addTile(Type);
    }

    public override LocalizedText DefaultContainerName(int frameX, int frameY) =>
        this.GetLocalization("MapEntry");

    public override bool HasSmartInteract(int i, int j, SmartInteractScanSettings settings) => true;

    public override void KillMultiTile(int i, int j, int frameX, int frameY) => Chest.DestroyChest(i, j);

    public override bool RightClick(int i, int j)
    {
        Player player = Main.LocalPlayer;
        Tile tile = Main.tile[i, j];
        int left = i - tile.TileFrameX % 36 / 18;
        int top = j - tile.TileFrameY / 18;
        Main.mouseRightRelease = false;
        player.CloseSign();
        player.SetTalkNPC(-1);
        if (Main.editChest)
        {
            Main.editChest = false;
            Main.npcChatText = string.Empty;
        }
        if (player.editedChestName && player.chest >= 0)
        {
            if (Main.netMode == NetmodeID.MultiplayerClient)
                NetMessage.SendData(MessageID.SyncPlayerChest,
                    text: NetworkText.FromLiteral(Main.chest[player.chest].name),
                    number: player.chest, number2: 1f);
            player.editedChestName = false;
        }

        if (player.chest >= 0 && player.chestX == left && player.chestY == top)
        {
            player.chest = -1;
            SoundEngine.PlaySound(SoundID.MenuClose);
            return true;
        }

        Main.stackSplit = 600;
        if (Main.netMode == NetmodeID.MultiplayerClient)
            NetMessage.SendData(MessageID.RequestChestOpen, number: left, number2: top);
        else
        {
            int chest = Chest.FindChest(left, top);
            if (chest >= 0)
            {
                SoundEngine.PlaySound(player.chest < 0 ? SoundID.MenuOpen : SoundID.MenuTick);
                player.OpenChest(left, top, chest);
            }
        }
        return true;
    }
}
