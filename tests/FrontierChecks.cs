using System;
using System.IO;
using System.Linq;
using MonoMod.RuntimeDetour;
using Microsoft.Xna.Framework;
using Terraria;
using Terraria.ID;
using Terraria.ModLoader;
using Terraria.ModLoader.IO;
using NyuuTerraria.Content.Frontiers;
using NyuuTerraria.Content.Reverie;
using NyuuTerraria.Content.Spellcraft;
namespace NyuuTerraria.Content;
public static class FrontierChecks
{
    static void Check(bool ok,string name){if(!ok)throw new Exception("FRONTIER_CHECK_FAILED: "+name);Console.WriteLine("PASS: FRONTIER_"+name);}
    public static void Run()
    {
        int mode=Main.netMode;bool hm=Main.hardMode,boss3=NPC.downedBoss3,moon=NPC.downedMoonlord;Main.netMode=0;
        Player p=Main.player[0];p.active=true;p.dead=false;var data=p.GetModPlayer<FrontierPlayer>();var chronicle=p.GetModPlayer<ChroniclePlayer>();
        using var silence=new Hook(typeof(ModPacket).GetMethod("Send"),(Action<Action<ModPacket,int,int>,ModPacket,int,int>)((original,packet,to,ignore)=>{}));
        try
        {
            Check(ChronicleContent.Quests.Length==26&&ChronicleContent.Quests.Select(q=>q.Id).Distinct().Count()==26,"OLD_AND_NEW_QUEST_IDS_ARE_UNIQUE");
            Check(FrontierStories.Scenes.Length==4&&FrontierStories.Scenes.All(s=>s.Choices.Length>0),"AUTHORED_DIALOGUE_PACK_LOADED");
            chronicle.Experience[4]=20000;chronicle.Profession=4;data.Initialize();
            Check(data.Invest(12)&&data.Talents[12]==1,"PROFESSION_TALENT_INVESTMENT");
            data.Talents[14]=1;Check(data.Activate()&&data.AbilityCooldown==1800&&p.HasBuff(BuffID.Spelunker),"EXPLORER_ABILITY_AND_COOLDOWN");
            Check(!data.Activate(),"ABILITY_CANNOT_BYPASS_COOLDOWN");
            data.Identity="Trans woman";data.Story="A life of my own";data.Tracked="wb-atelier4";data.Palette=2;data.Discoveries.Add("wildlife-DewFox");
            TagCompound tag=new();data.SaveData(tag);using var memory=new MemoryStream();TagIO.ToStream(tag,memory);memory.Position=0;data.LoadData(TagIO.FromStream(memory));
            Check(data.Identity=="Trans woman"&&data.Story=="A life of my own"&&data.Tracked=="wb-atelier4"&&data.Palette==2,"PROFILE_TRACKING_STYLE_BINARY_ROUNDTRIP");
            Check(data.Talents[12]==1&&data.Talents[14]==1&&data.AbilityCooldown==1800&&data.Discoveries.Contains("wildlife-DewFox"),"TALENTS_DISCOVERIES_COOLDOWN_PERSIST");
            chronicle.Profession=0;chronicle.Profession=4;Check(data.Talents[12]==1,"SWITCHING_PROFESSIONS_KEEPS_TALENTS");
            Check(!data.Discover("wildlife-DewFox"),"DISCOVERY_REWARDS_ONLY_ONCE");
            data.ResetBranch(4);Check(data.Talents.Skip(12).Sum()==0,"TALENT_RESET_ONLY_SELECTED_BRANCH");
            var first=new NPC();first.SetDefaults(NPCID.Zombie);var second=new NPC();second.SetDefaults(NPCID.Zombie);
            var mark=first.GetGlobalNPC<ElementMarks>();
            Check(mark.React(first,0,Spells.Ember)==-1&&mark.React(first,0,Spells.Tide)==0,"STEAM_ELEMENT_REACTION");
            Check(mark.React(first,0,Spells.Ember)==-1,"REACTION_COOLDOWN_PREVENTS_SPAM");
            Check(second.GetGlobalNPC<ElementMarks>().React(second,0,Spells.Tide)==-1,"ELEMENT_MARKS_ARE_NOT_SHARED_BETWEEN_NPCS");
            second.SetDefaults(NPCID.Zombie);var frost=second.GetGlobalNPC<ElementMarks>();
            Check(frost.React(second,0,Spells.Frost)==-1&&frost.React(second,0,Spells.Tide)==1,"FROZEN_BLOOM_REACTION");
            second.SetDefaults(NPCID.Zombie);var storm=second.GetGlobalNPC<ElementMarks>();
            Check(storm.React(second,0,Spells.Storm)==-1&&storm.React(second,0,Spells.Tide)==2,"CHARGED_TIDE_REACTION");
            Check(storm.React(second,1,Spells.Tide)==-1,"PLAYERS_DO_NOT_CONSUME_EACH_OTHERS_MARKS");
            var ritual=new Projectile();ritual.SetDefaults(ModContent.ProjectileType<RitualCircle>());ritual.timeLeft=200;Check(ritual.ModProjectile.CanDamage()==false,"RITUAL_HAS_DAMAGELESS_WINDUP");ritual.timeLeft=120;Check(ritual.ModProjectile.CanDamage()!=false,"RITUAL_DAMAGE_PULSE");
            var world=ModContent.GetInstance<FrontierWorld>();
            for(int i=0;i<4;i++)Check(world.EnsureArea(i),"DESTINATION_GENERATES_"+i);
            var all=world.Areas.Concat(ModContent.GetInstance<RealmWorld>().Areas).ToArray();
            Check(all.SelectMany((a,i)=>all.Skip(i+1).Select(b=>!a.Intersects(b))).All(v=>v),"ALL_SIX_DESTINATIONS_HAVE_NO_OVERLAP");
            for(int i=0;i<4;i++){var r=world.Areas[i];Check(Main.chest.Count(c=>c!=null&&r.Contains(c.x,c.y))==1,"ONE_FINITE_LOOT_CHEST_"+i);}
            var before=world.Areas.ToArray();Check(world.EnsureArea(0)&&world.Areas.SequenceEqual(before),"REVISITING_DOES_NOT_REBUILD_LOOT");
            TagCompound wt=new();world.SaveWorldData(wt);world.ClearWorld();world.LoadWorldData(wt);Check(world.Areas.SequenceEqual(before),"DESTINATION_COORDINATES_ROUNDTRIP");
            NPC.downedBoss3=false;NPC.downedMoonlord=false;Check(!FrontierWorld.Unlocked(2)&&!FrontierWorld.Unlocked(3),"MID_AND_LATE_DESTINATION_GATES");
            NPC.downedBoss3=true;NPC.downedMoonlord=true;Check(FrontierWorld.Unlocked(2)&&FrontierWorld.Unlocked(3),"DESTINATION_GATES_OPEN_AFTER_BOSSES");
            world.EventTicks=0;Check(world.StartEvent(0)&&world.EventTicks==18000&&!world.StartEvent(1),"ONE_WORLD_EVENT_AT_A_TIME");
            world.EventTicks=0;NPC.downedMoonlord=false;Check(!world.StartEvent(1),"CELESTIAL_EVENT_REQUIRES_MOON_LORD");
            var summon=new Item(ModContent.ItemType<GardenerInvitation>());p.Center=new Vector2((world.Areas[1].Left+120)*16,(world.Areas[1].Bottom-24)*16);Check(summon.ModItem.CanUseItem(p),"GARDENER_SUMMON_IN_CITADEL");
            Main.netMode=2;
            foreach(int type in new[]{ModContent.NPCType<PaleGardener>(),ModContent.NPCType<TheUnwritten>()})
            {
                var n=new NPC();n.SetDefaults(type);var r=type==ModContent.NPCType<PaleGardener>()?world.Areas[1]:ModContent.GetInstance<RealmWorld>().Areas[1];p.Center=new Vector2((r.Left+120)*16,(r.Bottom-24)*16);n.Center=p.Center+new Vector2(80,-180);
                for(int t=0;t<400;t++){n.ModNPC.AI();n.position+=n.velocity;if(t==170)n.life=n.lifeMax/3;}
                Check(float.IsFinite(n.position.X)&&float.IsFinite(n.position.Y),"TWO_PHASE_ATTACK_LOOP_"+n.ModNPC.Name);
                var rematch=new NPC();rematch.SetDefaults(type);rematch.ai[3]=1;rematch.Center=n.Center;int hp=rematch.lifeMax;rematch.ModNPC.AI();Check(rematch.lifeMax>hp&&rematch.life==rematch.lifeMax,"REMATCH_HEALTH_SCALING_"+rematch.ModNPC.Name);
            }
            Check(Main.recipe.Any(r=>r!=null&&r.createItem.type==ModContent.ItemType<MoonwellInfusion>()&&r.requiredTile==ModContent.TileType<CauldronWorkshopTile>()) ,"INFUSION_REQUIRES_CAULDRON");
            Console.WriteLine("FRONTIER_TESTS_COMPLETE");
        }
        finally {Main.netMode=mode;Main.hardMode=hm;NPC.downedBoss3=boss3;NPC.downedMoonlord=moon;foreach(var projectile in Main.projectile)projectile.active=false;}
    }
}


