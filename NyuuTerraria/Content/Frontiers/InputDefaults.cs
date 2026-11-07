using System;
using System.IO;
using System.Linq;
using Terraria;
using Terraria.GameInput;
using Terraria.ModLoader;
using NyuuTerraria.Content.Reverie;
using NyuuTerraria.Content.Spellcraft;
using NyuuTerraria.Content.Backpacks;
namespace NyuuTerraria.Content.Frontiers;

public class InputDefaults : ModSystem
{
    public override void OnWorldLoad()
    {
        if(Main.dedServ)return;
        string path=Path.Combine(Main.SavePath,"NyuuTerraria-initialized-controls.txt");
        string profile=PlayerInput.CurrentProfile.Name;
        try
        {
            var done=File.Exists(path)?File.ReadAllLines(path).ToHashSet():new System.Collections.Generic.HashSet<string>();
            if(done.Contains(profile))return;
            // Repair only missing defaults once per profile. An intentionally unbound key stays unbound afterward.
            Bind(ChronicleInterface.JournalKey,"L");Bind(SpellcraftKeys.Spellbook,"K");Bind(BackpackInterface.Toggle,"V");Bind(FrontierKeys.Ability,"G");
            if(!PlayerInput.Save())return;
            done.Add(profile);File.WriteAllLines(path,done);
        }
        catch(Exception e){Mod.Logger.Warn("Could not initialize missing default controls: "+e.Message);}
    }
    private static void Bind(ModKeybind key,string fallback)
    {if(key!=null&&key.GetAssignedKeys(InputMode.Keyboard).Count==0)key.GetAssignedKeys(InputMode.Keyboard).Add(fallback);}
}

