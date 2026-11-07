using System;
using System.Linq;
using System.Text;
using System.Text.Json;
using Terraria.ModLoader;
using NyuuTerraria.Content.Reverie;
namespace NyuuTerraria.Content.Frontiers;

public class StoryChoice {public string Text{get;set;}="";public string Event{get;set;}="";public string Reply{get;set;}="";}
public class StoryScene {public string Id{get;set;}="";public string Speaker{get;set;}="Lyra";public string Text{get;set;}="";public string Requires{get;set;}="";public StoryChoice[] Choices{get;set;}=Array.Empty<StoryChoice>();}
public class StoryPack {public QuestDefinition[] Quests{get;set;}=Array.Empty<QuestDefinition>();public StoryScene[] Scenes{get;set;}=Array.Empty<StoryScene>();}
public static class FrontierStories
{
    internal static StoryScene[] Scenes=Array.Empty<StoryScene>();
    internal static QuestDefinition[] Load(Mod mod)
    {
        var quests=new System.Collections.Generic.List<QuestDefinition>();var scenes=new System.Collections.Generic.List<StoryScene>();
        foreach(string file in mod.GetFileNames().Where(n=>n.StartsWith("Data/packs/",StringComparison.Ordinal)&&n.EndsWith(".json",StringComparison.Ordinal)).OrderBy(n=>n,StringComparer.Ordinal))
        {
            var pack=JsonSerializer.Deserialize<StoryPack>(Encoding.UTF8.GetString(mod.GetFileBytes(file)))??throw new Exception("Invalid story pack: "+file);
            quests.AddRange(pack.Quests);scenes.AddRange(pack.Scenes);
        }
        if(scenes.Select(s=>s.Id).Distinct().Count()!=scenes.Count||scenes.Any(s=>string.IsNullOrWhiteSpace(s.Id)||s.Choices.Length>2||s.Choices.Any(c=>string.IsNullOrWhiteSpace(c.Event))))throw new Exception("Invalid or duplicate story scene");
        Scenes=scenes.ToArray();return quests.ToArray();
    }
}
