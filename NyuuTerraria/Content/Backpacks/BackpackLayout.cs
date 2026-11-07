using System;
using System.Reflection;
using Mono.Cecil.Cil;
using MonoMod.Cil;
using MonoMod.RuntimeDetour;
using Terraria;
using Terraria.ModLoader;
namespace NyuuTerraria.Content.Backpacks;

public class BackpackLayout : ModSystem
{
    private ILHook buffsHook,inventoryHook;
    internal static bool BuffLayoutPatched;
    internal static void SelectEquipmentPage()
    {if(!Main.gameMenu&&Main.playerInventory&&Main.mouseItem.ModItem is BackpackItem)Main.EquipPage=2;}
    public override void UpdateUI(Microsoft.Xna.Framework.GameTime time)=>SelectEquipmentPage();
    public override void Load()
    {
        if(Main.dedServ)return;
        var method=typeof(Main).GetMethod("DrawInventory",BindingFlags.Instance|BindingFlags.NonPublic|BindingFlags.Public);
        if(method==null)return;
        // Vanilla auto-selects an armor page for accessory cursor items during inventory drawing.
        // Apply our page after that selection, before the first equipment-page branch is drawn.
        var select=typeof(Terraria.UI.ItemSlot).GetMethod("SelectEquipPage",BindingFlags.Static|BindingFlags.Public|BindingFlags.NonPublic);
        if(select!=null)inventoryHook=new ILHook(select,il=>{
            var cursor=new ILCursor(il);
            while(cursor.TryGotoNext(MoveType.Before,i=>i.OpCode==OpCodes.Ret)){cursor.EmitDelegate<Action>(SelectEquipmentPage);cursor.Index++;}
        });
        buffsHook=new ILHook(method,il=>
        {
            var cursor=new ILCursor(il);
            // The equipment page advances past five pet/mount slots, then builds both buff draw and hover positions.
            if(!cursor.TryGotoNext(MoveType.After,i=>i.MatchLdcI4(247),i=>i.OpCode==OpCodes.Add))
            {Mod.Logger.Warn("Backpack buff layout did not match this tModLoader version.");return;}
            cursor.EmitDelegate<Func<int,int>>(y=>y+47);
            BuffLayoutPatched=true;
        });
    }
    public override void Unload(){buffsHook?.Dispose();buffsHook=null;inventoryHook?.Dispose();inventoryHook=null;BuffLayoutPatched=false;}
}
