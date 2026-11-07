using System;
using System.Collections.Generic;
using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Graphics;
using Terraria;
using Terraria.GameInput;
using Terraria.ModLoader;
using Terraria.UI;

namespace NyuuTerraria.Content.Reverie;

public class ChronicleInterface : ModSystem
{
    internal static ModKeybind JournalKey;
    internal static bool Open;
    internal static int Tab, Page;
    internal static Point WardrobeOrigin = new(-1, -1);
    public override void Load() { if (!Main.dedServ) JournalKey = KeybindLoader.RegisterKeybind(Mod, "QuestJournal", "J"); }
    public override void Unload() { JournalKey = null; Open = false; }
    public override void OnWorldUnload() { Open = false; WardrobeOrigin = new(-1, -1); }
    private static Rectangle Bounds => new((int)(Main.screenWidth / 2 - 300), Math.Max(24, (int)(Main.screenHeight / 2 - 230)), 600, 460);
    public override void UpdateUI(GameTime gameTime)
    {
        if (Main.gameMenu || Main.LocalPlayer.dead) Open = false;
        if (!Open) return;
        Main.LocalPlayer.GetModPlayer<global::NyuuTerraria.Content.Backpacks.BackpackStorage>().Open = false;
        // UpdateUI can run with world coordinates; Draw layers already use UI coordinates.
        // Normalize once, then restore the caller's input/screen state.
        int mx = Main.mouseX, my = Main.mouseY, lx = Main.lastMouseX, ly = Main.lastMouseY;
        int sw = Main.screenWidth, sh = Main.screenHeight;
        try
        {
            PlayerInput.SetZoom_UI();
            if (Bounds.Contains(Mouse))
            {
                Main.LocalPlayer.mouseInterface = true;
                PlayerInput.LockVanillaMouseScroll("NyuuChronicle");
            }
        }
        finally
        {
            Main.mouseX = mx; Main.mouseY = my; Main.lastMouseX = lx; Main.lastMouseY = ly;
            Main.screenWidth = sw; Main.screenHeight = sh;
        }
    }
    private static Point Mouse => new(Main.mouseX, Main.mouseY);
    public override void ModifyInterfaceLayers(List<GameInterfaceLayer> layers)
    {
        int index = layers.FindIndex(l => l.Name == "Vanilla: Mouse Text");
        if (index >= 0) layers.Insert(index, new LegacyGameInterfaceLayer("Nyuu: Chronicle", Draw, InterfaceScaleType.UI));
    }
    private static void Text(string value, float x, float y, Color? color = null, float scale = .8f) =>
        Utils.DrawBorderString(Main.spriteBatch, value, new Vector2(x,y), color ?? Color.White, scale);
    internal static bool Button(string label, Rectangle rect)
    {
        bool hover = rect.Contains(Mouse);
        Utils.DrawInvBG(Main.spriteBatch, rect, hover ? new Color(113,81,146) : new Color(64,48,92));
        Text(label, rect.X + 8, rect.Y + 5);
        if (!hover || !Main.mouseLeft || !Main.mouseLeftRelease) return false;
        Main.mouseLeftRelease = false; return true;
    }
    private static bool Draw()
    {
        if (!Open || Main.gameMenu) return true;
        Rectangle p = Bounds;
        Utils.DrawInvBG(Main.spriteBatch, p, new Color(32,25,51,250));
        Text("Nyuu Chronicle", p.X+18, p.Y+12, new Color(235,184,223), 1f);
        if (Button("X", new Rectangle(p.Right-42,p.Y+9,30,28))) Open = false;
        string[] tabs = { "Quests", "Professions", "Wardrobe" };
        for (int i=0;i<3;i++) if (Button(tabs[i],new Rectangle(p.X+18+i*185,p.Y+48,176,30))) { Tab=i; Page=0; }
        var data = Main.LocalPlayer.GetModPlayer<ChroniclePlayer>();
        int x=p.X+18,y=p.Y+94;
        if (Tab == 0)
        {
            int pages=Math.Max(1,(ChronicleContent.Quests.Length+2)/3);
            Page=Math.Clamp(Page,0,pages-1);
            for (int i=0;i<3 && Page*3+i<ChronicleContent.Quests.Length;i++)
            {
                var q=ChronicleContent.Quests[Page*3+i]; int top=y+i*101;
                bool done=data.Completed.Contains(q.Id);
                bool locked=q.Requires.Length>0 && !data.Completed.Contains(q.Requires);
                Text(q.Title+(done ? "  [Complete]" : locked ? "  [Locked]" : ""),x,top,new Color(157,223,238));
                // Authoring guide caps descriptions at two short lines.
                string[] lines=q.Description.Split('|');
                for(int n=0;n<Math.Min(lines.Length,2);n++) Text(lines[n],x,top+23+n*18,scale:.7f);
                Text($"{Math.Min(data.Progress(q),q.Count)}/{q.Count}   Reward: {q.Experience} XP + {q.RewardCount} {q.Reward}",x,top+63,scale:.63f);
                if(!done && !locked && Button("Claim",new Rectangle(p.Right-92,top+48,73,30))) data.Claim(q);
            }
            if(Button("<",new Rectangle(x,p.Bottom-43,35,28))) Page=Math.Max(0,Page-1);
            Text($"Page {Page+1}/{pages}  -  J to close",x+52,p.Bottom-36,scale:.7f);
            if(Button(">",new Rectangle(p.Right-55,p.Bottom-43,35,28))) Page=Math.Min(pages-1,Page+1);
        }
        else if(Tab == 1)
        {
            Text("Switch freely. Every profession keeps its XP and skills.",x,y,scale:.72f);
            for(int i=0;i<5;i++)
            {
                int top=y+33+i*48;
                Text($"{ChroniclePlayer.Professions[i]}   Lv {data.Level(i)}   XP {data.Experience[i]}",x,top,scale:.73f);
                Text($"Skills {data.Skills[i]} / {data.Level(i)}",x,top+19,scale:.65f);
                if(Button(data.Profession==i ? "Active" : "Switch",new Rectangle(x+340,top,85,31))) { data.Profession=i; data.SyncProfession(); }
                if(Button("+ Skill",new Rectangle(x+435,top,99,31)) && data.Available(i)>0) { data.Skills[i]++; data.SyncProfession(); }
            }
            string[] effects={"+0.5% melee damage per rank; +1 defense per 5 ranks", "+0.5% ranged damage per rank", "+0.5% magic damage and +1 mana regen per rank", "+0.5% summon damage per rank", "+0.4% speed and +0.3% mining speed per rank (max 15%)"};
            Text(effects[data.Profession],x,y+283,new Color(184,222,227),.63f);
            if(Button("Reset active skills",new Rectangle(x,p.Bottom-47,178,30))) { data.Skills[data.Profession]=0; data.SyncProfession(); }
        }
        else
        {
            Text("Appearance presets - save your current look in three slots.",x,y,scale:.73f);
            bool nearby=WardrobeOrigin.X>=0 && Vector2.Distance(Main.LocalPlayer.Center,WardrobeOrigin.ToVector2()*16)<160;
            Text(nearby ? "Prism Wardrobe is nearby." : "Right-click a Prism Wardrobe to restore a look.",x,y+28,scale:.7f);
            for(int i=0;i<3;i++)
            {
                int top=y+75+i*55;
                Text("Look "+(i+1),x,top+6);
                if(Button("Save",new Rectangle(x+120,top,100,32))) data.SaveLook(i);
                if(Button("Restore",new Rectangle(x+235,top,110,32)) && nearby) data.RestoreLook(i);
            }
            if(Button("Pronouns: "+data.Pronouns,new Rectangle(x,y+255,255,32)))
            {
                string[] options={"they/them","she/her","he/him","any","name only"};
                data.Pronouns=options[(Array.IndexOf(options,data.Pronouns)+1)%options.Length];
            }
            Text("Use /nyuuprofile for custom dialogue pronouns.",x,y+300,scale:.68f);
        }
        return true;
    }
}

public class ProfileCommand : ModCommand
{
    public override CommandType Type => CommandType.Chat;
    public override string Command => "nyuuprofile";
    public override string Usage => "/nyuuprofile [pronouns]";
    public override string Description => "Set optional pronouns used by mod dialogue.";
    public override void Action(CommandCaller caller,string input,string[] args)
    {
        string value=string.Join(" ",args).Trim();
        if(value.Length>0 && value.Length<=32) caller.Player.GetModPlayer<ChroniclePlayer>().Pronouns=value;
        caller.Reply("Dialogue pronouns: "+caller.Player.GetModPlayer<ChroniclePlayer>().Pronouns);
    }
}
