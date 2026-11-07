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
    private ILHook buffsHook;
    internal static bool BuffLayoutPatched;
    public override void UpdateUI(Microsoft.Xna.Framework.GameTime time)
    {
        if(!Main.gameMenu&&Main.playerInventory&&Main.mouseItem.ModItem is BackpackItem)Main.EquipPage=2;
    }
    public override void Load()
    {
        if(Main.dedServ)return;
        var method=typeof(Main).GetMethod("DrawInventory",BindingFlags.Instance|BindingFlags.NonPublic|BindingFlags.Public);
        if(method==null)return;
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
    public override void Unload(){buffsHook?.Dispose();buffsHook=null;BuffLayoutPatched=false;}
}
