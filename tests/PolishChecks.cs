using System;
using System.Linq;
using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Input;
using Terraria;
using Terraria.ID;
using Terraria.ModLoader;
using Terraria.ObjectData;
using NyuuTerraria.Content.Frontiers;
using NyuuTerraria.Content.Backpacks;
using NyuuTerraria.Content.Reverie;
namespace NyuuTerraria.Content;
public static class PolishChecks
{
    static void Check(bool v,string name){if(!v)throw new Exception("POLISH_FAILED: "+name);Console.WriteLine("PASS: POLISH_"+name);}
    public static void Run()
    {
        var p=Main.LocalPlayer;Main.gameMenu=false;Main.playerInventory=true;Main.EquipPage=0;
        Main.mouseItem=new Item(ModContent.ItemType<CopperBackpack>());ModContent.GetInstance<BackpackLayout>().UpdateUI(new GameTime());Check(Main.EquipPage==2,"HOLDING_BACKPACK_SELECTS_EQUIPMENT_PAGE");Main.mouseItem=new Item();
        Main.oldKeyState=new KeyboardState();Main.keyState=new KeyboardState(Keys.Escape);ChronicleInterface.Open=true;ModContent.GetInstance<ChronicleInterface>().PostUpdateInput();Check(!ChronicleInterface.Open&&!Terraria.GameInput.PlayerInput.Triggers.Current.Inventory,"ESCAPE_CLOSES_CHRONICLE_WITHOUT_OPENING_INVENTORY");Main.keyState=Main.oldKeyState=new KeyboardState();
        foreach(var item in ModContent.GetInstance<global::NyuuTerraria.NyuuTerraria>().GetContent<ModItem>().Select(m=>new Item(m.Type)).Where(i=>i.accessory||i.defense>0||i.vanity||i.damage>0&&!i.consumable))Check(item.maxStack==1,"INDIVIDUAL_GEAR_"+item.ModItem.Name);
        Check(TileObjectData.GetTileData(ModContent.TileType<PetBedTile>(),0).Width==3,"PET_BED_HAS_PROPER_FURNITURE_FOOTPRINT");
        Check(TileObjectData.GetTileData(ModContent.TileType<NoticeboardTile>(),0).Height==3,"NOTICEBOARD_HAS_FULL_HEIGHT");
        foreach(var n in ModContent.GetInstance<global::NyuuTerraria.NyuuTerraria>().GetContent<ModNPC>().Where(n=>CreatureAnimationSetup.Names.Contains(n.Name)))Check(Main.npcFrameCount[n.Type]==4,"ANIMATION_"+n.Name);
        bool server=Main.dedServ;try{Main.dedServ=false;ModContent.GetInstance<BackpackLayout>().Load();Check(BackpackLayout.BuffLayoutPatched,"BUFF_LAYOUT_PATCH_MATCHES_DEV_BUILD");Main.playerInventory=true;Main.mouseItem=new Item(ModContent.ItemType<CopperBackpack>());Main.EquipPage=0;typeof(Terraria.UI.ItemSlot).GetMethod("SelectEquipPage",System.Reflection.BindingFlags.Public|System.Reflection.BindingFlags.NonPublic|System.Reflection.BindingFlags.Static).Invoke(null,new object[]{Main.mouseItem});Check(Main.EquipPage==2,"VANILLA_SELECTION_IS_OVERRIDDEN_FOR_BACKPACK");Main.mouseItem=new Item();}finally{ModContent.GetInstance<BackpackLayout>().Unload();Main.dedServ=server;}
        Console.WriteLine("POLISH_TESTS_COMPLETE");
    }
}
