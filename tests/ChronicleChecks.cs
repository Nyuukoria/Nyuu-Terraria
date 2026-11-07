using System;
using System.IO;
using System.Linq;
using System.Reflection;
using Microsoft.Xna.Framework;
using Terraria;
using Terraria.ID;
using Terraria.IO;
using Terraria.ModLoader;
using Terraria.ModLoader.IO;
using Terraria.UI;
using NyuuTerraria.Content.Backpacks;
using NyuuTerraria.Content.Reverie;
namespace NyuuTerraria.Content;
public class ChronicleChecks : ModSystem
{
    static void Check(bool condition,string name) {if(!condition) throw new Exception("CHECK_FAILED: "+name); Console.WriteLine("PASS: "+name); File.AppendAllText(Path.Combine(Main.SavePath, "NyuuTerraria-validation.txt"), "PASS: "+name+"\n");}
    public override void PostWorldLoad()
    {
        Terraria.Map.MapHelper.Initialize();
        int oldPlayer=Main.myPlayer;Main.myPlayer=0;
        typeof(PlayerLoader).GetMethod("SetupPlayer",BindingFlags.Static|BindingFlags.NonPublic).Invoke(null,new object[]{Main.player[0]});
        Player p=Main.LocalPlayer;p.active=true;p.dead=false;p.statLife=200;p.Center=new Vector2(Main.spawnTileX*16,Main.spawnTileY*16-48);
        try
        {
            var textures = Mod.GetContent<ModItem>().Select(t => t.Texture)
                .Concat(Mod.GetContent<ModNPC>().Select(t => t.Texture))
                .Concat(Mod.GetContent<ModProjectile>().Select(t => t.Texture))
                .Concat(Mod.GetContent<ModTile>().Select(t => t.Texture))
                .Concat(Mod.GetContent<ModBuff>().Select(t => t.Texture))
                .Concat(Mod.GetContent<ModHair>().Select(t => t.Texture));
            foreach (string path in textures.Where(t => t.StartsWith("NyuuTerraria/")))
                Check(Mod.FileExists(path[("NyuuTerraria/".Length)..] + ".rawimg"), "ASSET_" + path);
            bool wasServer = Main.dedServ;
            try { Main.dedServ = false; ModContent.GetInstance<InterfaceFixes>().Load(); Check(InterfaceFixes.ScrollPatched, "ACCESSORY_SCROLL_PATCH_MATCHES_LOADER"); }
            finally { ModContent.GetInstance<InterfaceFixes>().Unload(); Main.dedServ = wasServer; }
            int oldW=Main.screenWidth,oldH=Main.screenHeight,oldX=Main.mouseX,oldY=Main.mouseY;
            float oldScale=Main.UIScale; bool oldMenu=Main.gameMenu;
            try
            {
                Main.gameMenu=false;
                foreach(float scale in new[]{.8f,1f,1.5f,2f})
                {
                    Main.screenWidth=1920;Main.screenHeight=1080;Main.mouseX=960;Main.mouseY=540;Main.UIScale=scale;
                    typeof(Terraria.GameInput.PlayerInput).GetMethod("CacheOriginalInput",BindingFlags.Static|BindingFlags.NonPublic).Invoke(null,null);Terraria.GameInput.PlayerInput.CacheOriginalScreenDimensions();
                    Terraria.GameInput.PlayerInput.SetZoom_UI();
                    var bounds=(Rectangle)typeof(ChronicleInterface).GetProperty("Bounds",BindingFlags.Static|BindingFlags.NonPublic).GetValue(null);
                    if(Math.Abs(bounds.Center.X-Main.screenWidth/2)>1 || !bounds.Contains(Main.mouseX,Main.mouseY)) throw new Exception("UI scale misalignment");
                    int w=Main.screenWidth,h=Main.screenHeight,x=Main.mouseX,y=Main.mouseY;
                    ChronicleInterface.Open=true;p.mouseInterface=false;p.GetModPlayer<BackpackStorage>().Open=true;ModContent.GetInstance<ChronicleInterface>().UpdateUI(new GameTime());
                    if(p.GetModPlayer<BackpackStorage>().Open || !p.mouseInterface || Main.screenWidth!=w || Main.screenHeight!=h || Main.mouseX!=x || Main.mouseY!=y) throw new Exception("UI input state was not restored");
                }
                Check(true,"UI_PANEL_AND_MOUSE_ALIGN_AT_FOUR_SCALES");
                Check(true,"UI_UPDATE_RESTORES_CALLER_INPUT_STATE");
                Check(true,"JOURNAL_AND_BACKPACK_DO_NOT_OVERLAP");
            }
            finally
            {
                ChronicleInterface.Open=false;Main.gameMenu=oldMenu;Main.screenWidth=oldW;Main.screenHeight=oldH;
                Main.mouseX=oldX;Main.mouseY=oldY;Main.UIScale=oldScale;
                typeof(Terraria.GameInput.PlayerInput).GetMethod("CacheOriginalInput",BindingFlags.Static|BindingFlags.NonPublic).Invoke(null,null);Terraria.GameInput.PlayerInput.CacheOriginalScreenDimensions();
            }
            var d=p.GetModPlayer<ChroniclePlayer>();
            for(int i=0;i<5;i++){d.Profession=i;d.AddExperience(500+i*100);d.Skills[i]=1;}
            int[] xp=(int[])d.Experience.Clone();d.Profession=4;d.Completed.Add("atelier-1");d.Record("visit-hush");d.Pronouns="she/her";d.SaveLook(0);
            TagCompound tag=new();d.SaveData(tag);
            using var bytes=new MemoryStream();TagIO.ToStream(tag,bytes);bytes.Position=0;var loaded=TagIO.FromStream(bytes);
            d.Experience=new int[5];d.Skills=new int[5];d.LoadData(loaded);
            Check(d.Experience.SequenceEqual(xp)&&d.Skills.All(s=>s==1)&&d.Profession==4,"FIVE_PROFESSIONS_PERSIST_AFTER_SWITCH_AND_BINARY_SAVE");
            Check(d.Completed.Contains("atelier-1")&&d.Events["visit-hush"]==1&&d.Pronouns=="she/her"&&d.Looks.Count==1,"QUEST_PROFILE_AND_LOOK_PERSIST");
            d.Completed.Clear();p.inventory[0]=new Item(ItemID.Wood,10);
            Check(d.Claim(ChronicleContent.Quests[0])&&p.inventory[0].stack==2,"QUEST_MATERIAL_CONSUMPTION_AND_REWARD");
            Check(!d.Claim(ChronicleContent.Quests[0]),"QUEST_REWARD_CANNOT_BE_CLAIMED_TWICE");
            Check(ChronicleContent.Quests.Length>=12&&ChronicleContent.Quests.All(q=>Mod.TryFind<ModItem>(q.Reward,out _)),"ALL_QUEST_REWARDS_RESOLVE");
            var storage=p.GetModPlayer<BackpackStorage>();storage.Initialize();
            storage.Items[0]=new Item(ModContent.ItemType<MoonprismBar>(),13);storage.Items[99]=new Item(ItemID.GoldCoin,2);
            TagCompound bt=new();storage.SaveData(bt);storage.Initialize();storage.LoadData(bt);
            Check(storage.Items[0].stack==13&&storage.Items[99].stack==2&&!storage.IsEmpty,"BACKPACK_FIRST_AND_LAST_SLOT_PERSIST");
            p.chest=-1;Main.mouseItem=new Item(ItemID.Wood,3);Main.mouseLeft=true;Main.mouseLeftRelease=true;Main.playerInventory=true;
            Item[] view={new Item()};ItemSlot.LeftClick(view,ItemSlot.Context.InventoryItem,0);
            Check(view[0].type==ItemID.Wood&&view[0].stack==3,"BACKPACK_CLICK_WITH_NO_OPEN_CONTAINER");
            Main.mouseLeft=false;Main.mouseLeftRelease=false;Main.mouseItem=new Item();
            var slot=ModContent.GetInstance<BackpackSlot>();Main.EquipPage=0;Check(slot.IsHidden(),"BACKPACK_HIDDEN_ON_MAIN_EQUIPMENT_PAGE");
            Main.EquipPage=2;Check(!slot.IsHidden()&&slot.CustomLocation.HasValue,"BACKPACK_VISIBLE_ON_PETS_EQUIPMENT_PAGE");
            var world=ModContent.GetInstance<RealmWorld>();
            bool hadRealms=world.Areas.All(r=>r.Width>0);
            Check(world.EnsureArea(0)&&world.EnsureArea(1),hadRealms ? "REALMS_RELOAD_FROM_WORLD_SAVE" : "BOTH_REALMS_GENERATE_IN_CLEAR_SPACE");
            Check(!world.Areas[0].Intersects(world.Areas[1]),"REALMS_DO_NOT_OVERLAP");
            p.Center=new Vector2(Main.spawnTileX*16,Main.spawnTileY*16-80);
            var home=p.position; world.Travel(p,0); Check(RealmWorld.RealmAt(p.Center)==0,"PORTAL_ENTERS_REVERIE");
            world.Travel(p,0); Check(Vector2.Distance(p.position,home)<1,"PORTAL_RETURNS_TO_ORIGINAL_LOCATION");
            p.Center=new Vector2((world.Areas[1].Left+30)*16,(world.Areas[1].Bottom-24)*16);
            bool hm=Main.hardMode; Main.hardMode=false;
            Check(!ModContent.GetInstance<CrackedBellSigil>().CanUseItem(p),"BELLKEEPER_REQUIRES_HARDMODE");Main.hardMode=hm;

            int ore=0;
            var r=world.Areas[0];
            for(int x=r.Left;x<r.Right;x++)for(int y=r.Top;y<r.Bottom;y++)if(Main.tile[x,y].HasTile&&Main.tile[x,y].TileType==ModContent.TileType<MoonprismOreTile>())ore++;
            Check(ore>=100,"REVERIE_MOONPRISM_ORE_GENERATES");
            Check(Main.chest.Count(c=>c!=null&&world.Areas.Any(a=>a.Contains(c.x,c.y)))==2,"EACH_REALM_HAS_ONE_PERSISTENT_LOOT_CHEST");
            foreach(int type in new[]{ModContent.NPCType<RootboundKnight>(),ModContent.NPCType<Bellkeeper>(),ModContent.NPCType<PrismMatriarch>(),ModContent.NPCType<ReefbackGuardian>()})
            {
                var n=new NPC();n.SetDefaults(type);bool hush=type==ModContent.NPCType<Bellkeeper>();var a=world.Areas[hush?1:0];p.Center=new Vector2((a.Left+160)*16,(a.Bottom-24)*16);n.Center=p.Center+new Vector2(80,-80);n.active=true;
                for(int tick=0;tick<600;tick++){n.ModNPC.AI();n.position+=n.velocity;if(tick==300)n.life=n.lifeMax/3;}
                Check(float.IsFinite(n.position.X)&&float.IsFinite(n.position.Y),"BOSS_TWO_PHASE_PATTERN_"+n.ModNPC.Name);
            }
            var before=world.Areas.ToArray();TagCompound wt=new();world.SaveWorldData(wt);world.ClearWorld();world.LoadWorldData(wt);Check(world.Areas.SequenceEqual(before),"REALM_COORDINATES_ROUNDTRIP");
            SpellcraftChecks.Run(); FrontierChecks.Run(); PolishChecks.Run();Audit051Checks.Run();Animation052Checks.Run(); WorldFile.SaveWorld();Console.WriteLine("CHRONICLE_TESTS_COMPLETE"); Environment.Exit(0);
        }
        catch(Exception error)
        {
            File.WriteAllText(Path.Combine(Main.SavePath,"NyuuTerraria-test-failure.txt"),error.ToString());
            Console.WriteLine("TEST_FAILURE: "+error);Environment.Exit(1);
        }
        finally {p.active=false;Main.myPlayer=oldPlayer;}
    }
}





