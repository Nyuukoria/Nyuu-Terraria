using System;
using System.Linq;
using System.Collections.Generic;
using Microsoft.Xna.Framework;
using Terraria;
using Terraria.ID;
using Terraria.ModLoader;
using Terraria.UI;
using NyuuTerraria.Content.Reverie;

namespace NyuuTerraria.Content.Frontiers;

internal static class FrontierView
{
    internal static int Page,StoryIndex,QuestPage;private static string reply="";
    static void Text(string text,int x,int y,float scale=.7f,Color? c=null)=>Utils.DrawBorderString(Main.spriteBatch,text,new Vector2(x,y),c??Color.White,scale);
    static bool Button(string text,int x,int y,int width=110)=>ChronicleInterface.Button(text,new Rectangle(x,y,width,30));
    internal static void Draw(Rectangle panel)
    {
        Player p=Main.LocalPlayer;var d=p.GetModPlayer<FrontierPlayer>();var q=p.GetModPlayer<ChroniclePlayer>();int x=panel.X+18,y=panel.Y+90;
        string[] pages={"Requests","Talents","Profile","Wildlife","Stories","Workshop"};
        for(int i=0;i<6;i++)if(Button(pages[i],x+i*94,y,90)){Page=i;reply="";}
        y+=45;
        switch(Page)
        {
            case 0:
                Text("Requests — pin objectives and claim ready rewards here.",x,y);
                QuestPage=Math.Clamp(QuestPage,0,Math.Max(0,(ChronicleContent.Quests.Length-1)/5));
                for(int i=0;i<5&&QuestPage*5+i<ChronicleContent.Quests.Length;i++)
                {
                    var quest=ChronicleContent.Quests[QuestPage*5+i];int top=y+32+i*42;bool done=q.Completed.Contains(quest.Id),locked=quest.Requires.Length>0&&!q.Completed.Contains(quest.Requires);
                    string title=quest.Title.Length>42?quest.Title[..42]+"…":quest.Title;
                    Text((done?"✓ ":locked?"[Locked] ":"")+title,x,top,.67f,done?Color.LightGreen:Color.White);
                    Text($"{(done?quest.Count:Math.Min(q.Progress(quest),quest.Count))}/{quest.Count} — {quest.Experience} XP",x,top+17,.57f);
                    if(!done&&!locked&&q.Progress(quest)>=quest.Count&&Button("Claim",x+365,top,80))q.Claim(quest);
                    if(Button(d.Tracked==quest.Id?"Unpin":"Pin",x+453,top,95))d.Tracked=d.Tracked==quest.Id?"":quest.Id;
                }
                if(Button("<",x,panel.Bottom-52,45))QuestPage--;if(Button(">",x+500,panel.Bottom-52,45))QuestPage++;
                Text($"Page {QuestPage+1} · Ready requests show Claim",x+60,panel.Bottom-43,.65f);break;
            case 1:
                int branch=q.Profession;int spent=d.Talents.Skip(branch*3).Take(3).Sum();
                Text($"{ChroniclePlayer.Professions[branch]} — {q.Level(branch)/3-spent} talent points available",x,y,.77f,Color.LightCyan);
                Text("One point per 3 profession levels. Switch in Professions; progress stays.",x,y+25,.61f);
                for(int i=0;i<3;i++){int index=branch*3+i,top=y+65+i*58;Text($"{FrontierPlayer.TalentNames[index]}  {d.Talents[index]}/3",x,top);Text(FrontierPlayer.TalentDescriptions[index],x,top+21,.63f);if(Button("+ Rank",x+430,top,115))d.Invest(index);}
                if(Button("Reset talents",x,panel.Bottom-76,160))d.ResetBranch(branch);
                if(Button("Lv15 reward",x+180,panel.Bottom-76,170)&&q.Level(branch)>=15&&d.Discoveries.Add("mastery-reward-"+branch))p.QuickSpawnItem(p.GetSource_Misc("ProfessionMastery"),ModContent.ItemType<MasteryCrest>());
                Text($"G: active ability · cooldown {d.AbilityCooldown/60}s · rewards once per profession",x,panel.Bottom-35,.59f);break;
            case 2:
                Text("Your profile — saved to this character",x,y,.8f,Color.LightPink);
                Text("These choices describe you. They do not restrict clothing or abilities.",x,y+30,.62f);
                if(Button("Identity: "+d.Identity,x,y+65,540)){string[] a={"Adventurer","Woman","Trans woman","Nonbinary","Man","Trans man","Genderfluid","Agender","Unlabeled"};d.Identity=a[(Array.IndexOf(a,d.Identity)+1)%a.Length];}
                if(Button("Story: "+d.Story,x,y+108,540)){string[] a={"A new chapter","Finding my place","A life of my own","Curious wanderer","Keeper of small wonders"};d.Story=a[(Array.IndexOf(a,d.Story)+1)%a.Length];}
                if(Button("Spell style: "+new[]{"Elemental","Trans pride","Moonlit","Ember dusk"}[d.Palette],x,y+151,540))d.Palette=(d.Palette+1)%4;
                Text("Pronouns and saved outfits remain in Wardrobe.",x,y+207,.67f);break;
            case 3:
                string[] species={"DewFox","GlassDeer","Mothcat","MooncapSprite","TidepoolTurtle","GardenBird","CrystalSnail","LanternFirefly"};
                Text("Wildlife journal — observe within 220 pixels with a clear view.",x,y,.68f);
                for(int i=0;i<8;i++){int top=y+31+i*24;string seen=d.Discoveries.Contains("wildlife-"+species[i])?"Observed":"Undiscovered";string fed=d.Discoveries.Contains("feed-"+species[i])?" · befriended":"";Text(System.Text.RegularExpressions.Regex.Replace(species[i],"([a-z])([A-Z])","$1 $2")+" — "+seen+fed,x,top,.65f);}
                Text("Bowl + Daybloom: feed a nearby critter. Beds give them a resting place.",x,y+235,.59f);
                if(Button("Claim field reward",x,panel.Bottom-52,220)&&species.All(n=>d.Discoveries.Contains("wildlife-"+n))&&d.Discoveries.Add("wildlife-reward"))p.QuickSpawnItem(p.GetSource_Misc("WildlifeJournal"),ModContent.ItemType<ExplorerMedallion>());
                break;
            case 4:
                var scenes=FrontierStories.Scenes.Where(s=>s.Requires.Length==0||q.Completed.Contains(s.Requires)).ToArray();
                if(scenes.Length==0){Text("Complete an atelier request to open new conversations.",x,y);break;}
                StoryIndex=Math.Clamp(StoryIndex,0,scenes.Length-1);var scene=scenes[StoryIndex];
                Text(scene.Speaker+" — "+scene.Id.Replace('-',' '),x,y,.77f,Color.LightPink);
                string message=scene.Text.Replace("{player}",p.name).Replace("{identity}",d.Identity).Replace("{pronouns}",q.Pronouns);
                int line=0;foreach(string part in message.Split('|').Take(4))Text(part,x,y+35+line++*22,.66f);
                for(int i=0;i<scene.Choices.Length;i++)if(Button(scene.Choices[i].Text,x,y+140+i*39,540)){if(d.Discoveries.Add("dialogue-"+scene.Id))q.Record(scene.Choices[i].Event);reply=scene.Choices[i].Reply;}
                Text(reply.Length>76?reply[..76]:reply,x,y+225,.62f,Color.LightCyan);
                if(Button("Previous",x,panel.Bottom-52,120)){StoryIndex--;reply="";}if(Button("Next",x+425,panel.Bottom-52,120)){StoryIndex++;reply="";}break;
            case 5:
                Text("Cauldron workshop & expeditions",x,y,.8f,Color.LightCyan);
                string[] lines={"Cauldron: 10 Iron Bars + 5 Wood, Work Bench.","Stand beside it to craft an infusion:","Bottled Water + Daybloom + 1 destination material.","Tide Pearl → +5 mana regeneration bonus.","Cinder Bloom → +2 life regeneration.","Starglass Shard → +4 magic critical chance.","Infusions last 3 minutes. Drinking another replaces the effect.","Craft destination keys and use them to travel, then return.","Tidepool / Citadel: early-midgame. Cinderwood: after Skeletron.","Starglass: after Moon Lord. Existing terrain is preserved."};
                for(int i=0;i<lines.Length;i++)Text(lines[i],x,y+38+i*22,.62f);break;
        }
    }
}
public class FrontierHUD : ModSystem
{
    public override void ModifyInterfaceLayers(List<GameInterfaceLayer> layers)
    {
        int i=layers.FindIndex(l=>l.Name=="Vanilla: Mouse Text");if(i>=0)layers.Insert(i,new LegacyGameInterfaceLayer("Nyuu: Tracked request",Draw,InterfaceScaleType.UI));
    }
    bool Draw()
    {
        if(Main.gameMenu||Main.playerInventory||ChronicleInterface.Open||Main.LocalPlayer.dead)return true;
        var d=Main.LocalPlayer.GetModPlayer<FrontierPlayer>();var q=Main.LocalPlayer.GetModPlayer<ChroniclePlayer>();var quest=ChronicleContent.Quests.FirstOrDefault(v=>v.Id==d.Tracked);
        if(quest!=null){string title=quest.Title.Length>48?quest.Title[..48]+"…":quest.Title;Utils.DrawBorderString(Main.spriteBatch,title,new Vector2(24,265),Color.LightCyan,.66f);Utils.DrawBorderString(Main.spriteBatch,$"{(q.Completed.Contains(quest.Id)?quest.Count:Math.Min(q.Progress(quest),quest.Count))}/{quest.Count} · L: quest journal",new Vector2(24,287),Color.White,.62f);}
        var w=ModContent.GetInstance<FrontierWorld>();if(w.EventTicks>0)Utils.DrawBorderString(Main.spriteBatch,(w.EventKind==0?"Garden festival":"Starglass convergence")+$" · {w.EventTicks/60}s",new Vector2(24,316),Color.LightPink,.65f);
        return true;
    }
}


