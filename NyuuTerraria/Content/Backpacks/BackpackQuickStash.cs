using System;
using System.Reflection;
using Microsoft.Xna.Framework.Input;
using Terraria;
using Terraria.Audio;
using Terraria.ID;
using Terraria.ModLoader;
using Terraria.UI;

namespace NyuuTerraria.Content.Backpacks;

/// <summary>
/// Shift + left click an inventory slot while the backpack is open and the item goes into the
/// backpack, stacking onto a matching stack first. Without a chest open vanilla has nothing
/// bound to that combination, so this costs no existing behaviour.
///
/// The hook is installed by reflection through MonoModHooks, so if a future tModLoader renames
/// or re-signs ItemSlot.LeftClick the mod still loads and the feature simply stays off.
/// </summary>
public class BackpackQuickStash : ModSystem
{
    private const BindingFlags Flags = BindingFlags.Static | BindingFlags.Public | BindingFlags.NonPublic;
    private MethodInfo leftClick;
    private Action<Action<Item[], int, int>, Item[], int, int> hook;

    public override void Load()
    {
        if (Main.dedServ) return;
        try
        {
            leftClick = typeof(ItemSlot).GetMethod("LeftClick", Flags, null,
                new[] { typeof(Item[]), typeof(int), typeof(int) }, null);
            if (leftClick == null)
            {
                Mod.Logger.Warn("Backpack quick stash: ItemSlot.LeftClick not found, feature disabled.");
                return;
            }
            hook = HookLeftClick;
            MonoModHooks.Add(leftClick, hook);
        }
        catch (Exception e)
        {
            Mod.Logger.Warn("Backpack quick stash disabled: " + e.Message);
            leftClick = null;
        }
    }

    public override void Unload()
    {
        // MonoModHooks has no Remove: the hook lives and dies with the mod's assembly,
        // which tModLoader unloads and discards on reload. Only the managed references go.
        hook = null;
        leftClick = null;
    }

    private static void HookLeftClick(Action<Item[], int, int> orig, Item[] inv, int context, int slot)
    {
        if (Stash(inv, context, slot))
            return;
        orig(inv, context, slot);
    }

    /// <summary>
    /// True only on the frame the button goes down. Vanilla clears mouseLeftRelease on a fresh
    /// press, so holding shift and *then* clicking reads as a fresh press, while clicking and
    /// then pressing shift does not - which is the order that was asked for.
    /// </summary>
    private static bool FreshClick()
    {
        return Main.mouseLeft && Main.mouseLeftRelease;
    }

    private static bool ShiftHeld()
    {
        return Main.keyState.IsKeyDown(Keys.LeftShift) || Main.keyState.IsKeyDown(Keys.RightShift);
    }

    private static bool Stash(Item[] inv, int context, int slot)
    {
        if (Main.gameMenu || Main.dedServ || !Main.playerInventory) return false;
        if (context != (int)ItemSlot.Context.InventoryItem) return false;
        if (inv == null || slot < 0 || slot >= inv.Length) return false;

        Player player = Main.LocalPlayer;
        if (player == null || !player.active || inv != player.inventory) return false;
        if (!ShiftHeld() || !Main.mouseItem.IsAir) return false;
        if (!FreshClick()) return false;

        BackpackStorage storage = player.GetModPlayer<BackpackStorage>();
        if (storage == null || !storage.Open) return false;

        int capacity = storage.Capacity;
        if (capacity <= 0) return false;

        Item item = inv[slot];
        if (item == null || item.IsAir || item.stack <= 0) return false;
        // Favourited items stay put, like vanilla quick-stack; a backpack never goes inside a backpack.
        if (item.favorited || item.ModItem is BackpackItem) return false;
        if (!HasRoom(storage, capacity, item)) return false;

        // 1. top up matching stacks that are not full
        for (int i = 0; i < capacity && !item.IsAir; i++)
        {
            Item dest = storage.Items[i];
            if (!Stackable(dest, item)) continue;
            // ItemLoader handles ModItem.CanStack/OnStack, so modded per-item data is respected.
            ItemLoader.TryStackItems(dest, item, out _);
            if (item.stack <= 0) item.TurnToAir();
        }

        // 2. then the first free slot
        if (!item.IsAir)
        {
            for (int i = 0; i < capacity; i++)
            {
                if (!storage.Items[i].IsAir) continue;
                storage.Items[i] = item.Clone();
                item.TurnToAir();
                break;
            }
        }

        SoundEngine.PlaySound(SoundID.Grab);
        Recipe.UpdateRecipeList();
        return true;
    }

    private static bool Stackable(Item dest, Item item)
    {
        return !dest.IsAir && dest.type == item.type && dest.stack < dest.maxStack
            && ItemLoader.CanStack(dest, item);
    }

    private static bool HasRoom(BackpackStorage storage, int capacity, Item item)
    {
        for (int i = 0; i < capacity; i++)
        {
            Item dest = storage.Items[i];
            if (dest.IsAir) return true;
            if (Stackable(dest, item)) return true;
        }
        return false;
    }
}
