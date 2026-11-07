using System;using System.Linq;using Microsoft.Xna.Framework;using Terraria;using Terraria.ID;using Terraria.ModLoader;using NyuuTerraria.Content.Backpacks;using NyuuTerraria.Content.Reverie;using NyuuTerraria.Content.Frontiers;
namespace NyuuTerraria.Content;
public static class Audit051Checks{
 static void Check(bool value,string name){if(!value)throw new Exception("AUDIT051_FAILED: "+name);Console.WriteLine("PASS: AUDIT051_"+name);}
 public static void Run(){
  foreach(var t in new[]{typeof(Main),typeof(Terraria.UI.ItemSlot)})foreach(var m in t.GetMethods(System.Reflection.BindingFlags.Static|System.Reflection.BindingFlags.Public|System.Reflection.BindingFlags.NonPublic).Where(m=>m.Name.Contains("Equip")&&m.Name.Contains("Page")))Console.WriteLine("EQUIP_API: "+t.Name+" "+m);var p=Main.LocalPlayer;var q=p.GetModPlayer<ChroniclePlayer>();p.dead=false;var pack=p.GetModPlayer<BackpackStorage>();var quest=ChronicleContent.Quests[0];q.Completed.Remove(quest.Id);
  for(int i=0;i<p.inventory.Length;i++)p.inventory[i]=new Item();for(int i=0;i<100;i++)pack.Items[i]=new Item();
  int itemType=quest.Item;if(quest.ModItem.Length>0)itemType=ModContent.GetInstance<global::NyuuTerraria.NyuuTerraria>().Find<ModItem>(quest.ModItem).Type;
  pack.Items[99]=new Item(itemType,quest.Count);Check(q.Progress(quest)==quest.Count,"QUEST_READS_BACKPACK_OVERFLOW");Check(q.Claim(quest),"QUEST_CLAIMS_FROM_BACKPACK");Check(pack.Items[99].IsAir,"QUEST_REMOVES_EXACT_MATERIALS");Check(!q.Claim(quest),"CLAIM_CANNOT_DUPLICATE_REWARDS");
  foreach(var n in new ModNPC[]{new ReefSkitterer(),new VineboundArmor(),new AshBurrower(),new ArchiveCaretaker()}){var npc=new NPC();npc.SetDefaults(ModContent.GetInstance<global::NyuuTerraria.NyuuTerraria>().Find<ModNPC>(n.Name).Type);Check(!npc.noGravity,n.Name+"_GROUND_MOVEMENT");}
  var f=new NPC();f.SetDefaults(ModContent.NPCType<StarfallenConstruct>());Check(f.noGravity,"CONSTRUCT_REMAINS_AERIAL");
  var areas=ModContent.GetInstance<FrontierWorld>().Areas;foreach(var area in areas.Where(a=>a.Width>0)){int maxRun=0,run=0;for(int x=area.Left+10;x<area.Right-10;x++){var t=Main.tile[x,area.Bottom-15-10];if(t.HasTile&&t.TileType==TileID.Platforms)maxRun=Math.Max(maxRun,++run);else run=0;}Check(maxRun<100,"NO_WALL_TO_WALL_SCAFFOLD");}
  var grove=ModContent.GetInstance<RealmWorld>().Areas[0];for(int tx=grove.Left+12;tx<grove.Left+40;tx++)for(int ty=grove.Top+2;ty<grove.Bottom-13;ty++){var tile=Main.tile[tx,ty];tile.HasTile=false;}ExpeditionScenery.Tree(grove.Left+26,grove.Bottom-13,14,0);int trees=0;for(int x=grove.Left;x<grove.Right;x++)for(int y=grove.Top;y<grove.Bottom;y++)if(Main.tile[x,y].HasTile&&Main.tile[x,y].TileType==TileID.Trees)trees++;Check(trees>0,"NATIVE_CONNECTED_TREES_GENERATED");Console.WriteLine("AUDIT051_TESTS_COMPLETE");
 }
}
