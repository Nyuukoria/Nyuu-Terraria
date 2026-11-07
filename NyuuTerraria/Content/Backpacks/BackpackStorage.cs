using System;
using System.Collections.Generic;
using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Graphics;
using Terraria;
using Terraria.GameInput;
using Terraria.ModLoader;
using Terraria.ModLoader.IO;
using Terraria.UI;

namespace NyuuTerraria.Content.Backpacks;

public class BackpackStorage : ModPlayer
{
    internal const int MaximumSlots = 100;
    internal readonly Item[] Items = new Item[MaximumSlots];
    internal bool Open;
    internal int Page;

    internal bool IsEmpty
    {
        get
        {
            foreach (Item item in Items)
                if (!item.IsAir)
                    return false;
            return true;
        }
    }

    internal int Capacity =>
        ModContent.GetInstance<BackpackSlot>().FunctionalItem.ModItem is BackpackItem pack ? pack.Capacity : 0;

    public override void Initialize()
    {
        for (int i = 0; i < Items.Length; i++)
            Items[i] = new Item();
    }

    internal int VisibleSlots
    {
        get
        {
            int count = Capacity;
            for (int i = Items.Length - 1; i >= count; i--)
                if (!Items[i].IsAir)
                    return i + 1;
            return count;
        }
    }

    public override void ProcessTriggers(TriggersSet triggersSet)
    {
        if (!BackpackInterface.Toggle.JustPressed || Main.gameMenu || Main.drawingPlayerChat ||
            Main.editSign || Main.editChest)
            return;

        if (VisibleSlots == 0)
        {
            Main.NewText("Equip a backpack in the Backpack slot first.", new Color(203, 179, 237));
            return;
        }

        Open = !Open;
        if (Open)
        {
            Main.playerInventory = true;
            Player.chest = -1;
        }
    }

    public override void PostUpdate()
    {
        if (Player.dead || !Main.playerInventory || Main.gameMenu)
            Open = false;
    }

    public override void SaveData(TagCompound tag)
    {
        var entries = new List<TagCompound>();
        for (int i = 0; i < Items.Length; i++)
            if (!Items[i].IsAir)
                entries.Add(new TagCompound { ["slot"] = i, ["item"] = ItemIO.Save(Items[i]) });
        tag["backpackItems"] = entries;
    }

    public override void LoadData(TagCompound tag)
    {
        Initialize();
        foreach (TagCompound entry in tag.GetList<TagCompound>("backpackItems"))
        {
            int slot = entry.GetInt("slot");
            if (slot >= 0 && slot < Items.Length)
                Items[slot] = ItemIO.Load(entry.GetCompound("item"));
        }
    }
}

public class BackpackInterface : ModSystem
{
    internal static ModKeybind Toggle;

    public override void Load()
    {
        if (!Main.dedServ)
            Toggle = KeybindLoader.RegisterKeybind(Mod, "Backpack", "V");
    }

    public override void Unload() => Toggle = null;

    public override void ModifyInterfaceLayers(List<GameInterfaceLayer> layers)
    {
        int index = layers.FindIndex(layer => layer.Name == "Vanilla: Mouse Text");
        if (index >= 0)
            layers.Insert(index, new LegacyGameInterfaceLayer("Nyuu Terraria: Backpack",
                Draw, InterfaceScaleType.UI));
    }

    private static bool Draw()
    {
        if (Main.gameMenu || !Main.playerInventory)
            return true;

        Player player = Main.LocalPlayer;
        BackpackStorage storage = player.GetModPlayer<BackpackStorage>();
        if (!storage.Open || storage.VisibleSlots == 0)
            return true;

        int count = storage.VisibleSlots;
        int pages = (count + 49) / 50;
        storage.Page = Math.Clamp(storage.Page, 0, pages - 1);
        int pageStart = storage.Page * 50;
        int rows = (Math.Min(50, count - pageStart) + 9) / 10;
        float screenWidth = Main.screenWidth / Main.UIScale;
        float screenHeight = Main.screenHeight / Main.UIScale;
        int x = Math.Max(8, (int)(screenWidth / 2 - 252));
        int y = Math.Max(8, Math.Min((int)screenHeight - rows * 44 - 92, 340));
        var panel = new Rectangle(x, y, 504, rows * 44 + 80);
        Vector2 mouse = new Vector2(Main.mouseX, Main.mouseY) / Main.UIScale;
        SpriteBatch batch = Main.spriteBatch;
        Utils.DrawInvBG(batch, panel, new Color(58, 42, 79, 245));
        Utils.DrawBorderString(batch, $"Backpack — {storage.Capacity} slots", new Vector2(x + 14, y + 9), Color.White);

        if (panel.Contains(mouse.ToPoint()))
        {
            player.mouseInterface = true;
            player.cursorItemIconEnabled = false;
        }

        if (Button(batch, new Rectangle(x + 462, y + 8, 28, 26), "X", mouse))
            storage.Open = false;

        float previousScale = Main.inventoryScale;
        Main.inventoryScale = 0.76f;
        try
        {
            for (int i = 0; i < 50 && pageStart + i < count; i++)
            {
                int slot = pageStart + i;
                Vector2 position = new Vector2(x + 14 + i % 10 * 47, y + 42 + i / 10 * 44);
                Rectangle hitbox = new Rectangle((int)position.X, (int)position.Y, 40, 40);
                bool overflow = slot >= storage.Capacity;
                ItemSlot.Draw(batch, storage.Items, ItemSlot.Context.BankItem, slot, position,
                    overflow ? new Color(170, 135, 140) : Color.White);
                if (!hitbox.Contains(mouse.ToPoint()))
                    continue;

                player.mouseInterface = true;
                bool wasEmpty = storage.IsEmpty;
                if (overflow)
                {
                    if (Main.mouseLeft && Main.mouseLeftRelease && Main.mouseItem.IsAir)
                    {
                        Main.mouseItem = storage.Items[slot];
                        storage.Items[slot] = new Item();
                        Main.mouseLeftRelease = false;
                    }
                }
                else
                {
                    ItemSlot.LeftClick(storage.Items, ItemSlot.Context.BankItem, slot);
                    ItemSlot.RightClick(storage.Items, ItemSlot.Context.BankItem, slot);
                }
                if (wasEmpty != storage.IsEmpty)
                    Recipe.UpdateRecipeList();
                ItemSlot.MouseHover(storage.Items, ItemSlot.Context.BankItem, slot);
            }
        }
        finally
        {
            Main.inventoryScale = previousScale;
        }

        int footerY = y + 46 + rows * 44;
        Utils.DrawBorderString(batch, $"Page {storage.Page + 1}/{pages}  ·  V to close",
            new Vector2(x + 14, footerY), new Color(223, 201, 242), 0.7f);
        if (pages > 1)
        {
            if (Button(batch, new Rectangle(x + 416, footerY - 2, 30, 24), "<", mouse))
                storage.Page = Math.Max(0, storage.Page - 1);
            if (Button(batch, new Rectangle(x + 457, footerY - 2, 30, 24), ">", mouse))
                storage.Page = Math.Min(pages - 1, storage.Page + 1);
        }
        return true;
    }

    private static bool Button(SpriteBatch batch, Rectangle rectangle, string text, Vector2 mouse)
    {
        bool hovered = rectangle.Contains(mouse.ToPoint());
        Utils.DrawInvBG(batch, rectangle, hovered ? new Color(133, 92, 163) : new Color(78, 56, 101));
        Utils.DrawBorderString(batch, text, rectangle.TopLeft() + new Vector2(7, 1), Color.White, 0.8f);
        if (!hovered || !Main.mouseLeft || !Main.mouseLeftRelease)
            return false;
        Main.mouseLeftRelease = false;
        return true;
    }
}
