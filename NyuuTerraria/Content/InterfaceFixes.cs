using System;
using System.Reflection;
using Mono.Cecil.Cil;
using MonoMod.Cil;
using MonoMod.RuntimeDetour;
using Terraria;
using Terraria.ModLoader;
namespace NyuuTerraria.Content;
public class InterfaceFixes : ModSystem
{
    private ILHook scrollHook;
    internal static bool ScrollPatched;
    public override void Load()
    {
        if (Main.dedServ) return;
        MethodInfo method = typeof(AccessorySlotLoader).GetMethod("DrawScrollbar", BindingFlags.Instance | BindingFlags.Static | BindingFlags.NonPublic | BindingFlags.Public);
        if (method == null) return;
        scrollHook = new ILHook(method, il => {
            for (int i = 0; i < il.Body.Instructions.Count - 3; i++)
            {
                var code = il.Body.Instructions;
                if (code[i].MatchLdsfld("Terraria.GameInput.PlayerInput", "ScrollWheelDelta") &&
                    code[i + 2].OpCode == OpCodes.Div && code[i + 3].OpCode == OpCodes.Add)
                {
                    code[i + 3].OpCode = OpCodes.Sub;
                    ScrollPatched = true;
                    return;
                }
            }
            Mod.Logger.Warn("Accessory scrollbar patch did not match this loader version.");
        });
    }
    public override void Unload() { scrollHook?.Dispose(); scrollHook = null; ScrollPatched = false; }
}
