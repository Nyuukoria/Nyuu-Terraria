using Microsoft.Xna.Framework;
using Terraria;
using Terraria.DataStructures;
using Terraria.ID;
using Terraria.ModLoader;

namespace NyuuTerraria.Content.Testing;

/// <summary>
/// Hands you every item this mod adds, so a fresh character can test the whole pack without a
/// save file. Craft it at any work bench from one dirt block, then use it.
///
/// It walks the item id range above ItemID.Count, keeps the ones whose ModItem belongs to this
/// mod, and spawns each beside the player.
///
/// Spawning goes through Item.NewItem(IEntitySource, Vector2, int, int). An earlier version wrote
/// into Main.item[] and did not compile: Terraria 1.4.5 split Item, moving the in-world half into
/// WorldItem, so Main.item is now WorldItem[] and Item no longer carries active, position or
/// velocity. A Vector2 position is also what lets the drops land in a grid instead of a pile.
/// </summary>
public class NyuuTestKit : ModItem
{
    private const int GivePerItem = 30;
    private const int ColumnWidth = 26;
    private const int RowHeight = 26;
    private const int PerRow = 14;

    public override void SetDefaults()
    {
        Item.width = 20;
        Item.height = 20;
        Item.useStyle = ItemUseStyleID.HoldUp;
        Item.useTime = 24;
        Item.useAnimation = 24;
        Item.rare = ItemRarityID.Purple;
        Item.value = 0;
        Item.maxStack = 1;
        Item.consumable = false;
    }

    public override bool? UseItem(Player player)
    {
        if (player.whoAmI != Main.myPlayer)
            return true;

        int made = SpawnAll(player);

        if (made > 0)
            Main.NewText("Nyuu test kit: " + made + " items dropped at your feet.", 180, 150, 255);
        else
            Main.NewText("Nyuu test kit: no mod items found.", 255, 120, 120);

        return true;
    }

    private int SpawnAll(Player player)
    {
        IEntitySource source = new EntitySource_Misc("NyuuTestKit");
        int made = 0;

        int last = ItemLoader.ItemCount;
        if (last <= ItemID.Count)
            last = ItemID.Count + 1000;

        for (int type = ItemID.Count; type < last; type++)
        {
            Item probe = new Item();
            probe.SetDefaults(type);

            if (probe.ModItem == null)
                continue;
            if (probe.ModItem.Mod != Mod)
                continue;

            int give = probe.maxStack;
            if (give > GivePerItem)
                give = GivePerItem;
            if (give < 1)
                give = 1;

            int col = made % PerRow;
            int row = made / PerRow;
            Vector2 spot = new Vector2(player.Center.X - 170f + col * ColumnWidth,
                                       player.Center.Y - 40f + row * RowHeight);
            int index = Item.NewItem(source, spot, type, give);
            // A multiplayer client only spawns the drop locally; tell the server about it.
            if (Main.netMode == NetmodeID.MultiplayerClient)
                NetMessage.SendData(MessageID.SyncItem, -1, -1, null, index, 1f);
            made++;
        }

        return made;
    }

    public override void AddRecipes()
    {
        CreateRecipe()
            .AddIngredient(ItemID.DirtBlock, 1)
            .AddTile(TileID.WorkBenches)
            .Register();
    }
}
