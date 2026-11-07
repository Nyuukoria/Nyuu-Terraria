using System;
using System.IO;
using System.Linq;
using System.Text.Json;
using System.Reflection;
using Microsoft.Xna.Framework;
using Terraria;
using Terraria.ID;
using Terraria.IO;
using Terraria.ModLoader;
using NyuuTerraria.Content.Backpacks;
using NyuuTerraria.Content.Frontiers;
using NyuuTerraria.Content.Reverie;
using NyuuTerraria.Content.Spellcraft;
namespace NyuuTerraria.Content;

// Export-only fixture, never included in the playable mod.
public class ShowcaseBuilder : ModSystem
{
    static void Put(int x,int y,int type){var tile=Main.tile[x,y];tile.HasTile=true;tile.TileType=(ushort)type;tile.TileFrameX=tile.TileFrameY=0;}
    public override void PostWorldLoad()
    {
        if(!Main.dedServ||!Main.SavePath.Contains("showcase-export",StringComparison.OrdinalIgnoreCase))return;
        try
        {
            Main.myPlayer=0;var p=Main.player[0];typeof(PlayerLoader).GetMethod("SetupPlayer",BindingFlags.Static|BindingFlags.NonPublic).Invoke(null,new object[]{p});
            p.ConsumedLifeCrystals=15;p.ConsumedLifeFruit=20;p.ConsumedManaCrystals=9;p.active=true;p.dead=false;p.name="Nyuu Showcase";p.statLifeMax=p.statLifeMax2=p.statLife=500;p.statManaMax=p.statManaMax2=p.statMana=400;
            p.hair=ModContent.GetInstance<WaistlengthWaves>().Type;p.skinVariant=6;p.hairColor=new Color(182,145,224);p.skinColor=new Color(243,202,184);p.eyeColor=new Color(100,200,216);
            p.armor[0].SetDefaults(ItemID.NebulaHelmet);p.armor[1].SetDefaults(ItemID.NebulaBreastplate);p.armor[2].SetDefaults(ItemID.NebulaLeggings);
            int[] accessories={ItemID.FishronWings,ItemID.TerrasparkBoots,ItemID.CelestialCuffs,ItemID.AnkhShield,ItemID.ManaCloak};for(int i=0;i<5;i++)p.armor[3+i].SetDefaults(accessories[i]);
            p.armor[11].SetDefaults(ModContent.ItemType<CoastalCoat>());p.armor[12].SetDefaults(ModContent.ItemType<CoastalSkirt>());
            ModContent.GetInstance<BackpackSlot>().FunctionalItem.SetDefaults(ModContent.ItemType<DemoniteBackpack>());
            var spells=p.GetModPlayer<SpellcraftPlayer>();for(int i=0;i<7;i++){spells.Learn(i);spells.Experience[i]=7500;}
            var journal=p.GetModPlayer<ChroniclePlayer>();for(int i=0;i<5;i++)journal.Experience[i]=125000;
            p.GetModPlayer<FrontierPlayer>().Story="Keeper of small wonders";
            Main.hardMode=true;foreach(var field in typeof(NPC).GetFields(BindingFlags.Public|BindingFlags.Static).Where(f=>f.FieldType==typeof(bool)&&f.Name.StartsWith("downed")))field.SetValue(null,true);
            int network=Main.netMode;Main.netMode=NetmodeID.SinglePlayer;
            var old=ModContent.GetInstance<RealmWorld>();var fresh=ModContent.GetInstance<FrontierWorld>();
            for(int i=0;i<2;i++)if(!old.EnsureArea(i))throw new Exception("Could not create original destination "+i);
            for(int i=0;i<4;i++)if(!fresh.EnsureArea(i))throw new Exception("Could not create new destination "+i);
            fresh.GardenerDown=fresh.UnwrittenDown=true;
            Main.netMode=network;
            int center=Main.maxTilesX/2,floor=(int)Main.worldSurface+10;
            for(int c=0;c<Main.chest.Length;c++)if(Main.chest[c]!=null&&Math.Abs(Main.chest[c].x-center)<160&&Main.chest[c].y>=floor-32&&Main.chest[c].y<floor+12)Main.chest[c]=null;
            for(int x=center-160;x<center+160;x++)for(int y=floor-32;y<floor+12;y++)
            {var t=Main.tile[x,y];t.ClearEverything();if(y>=floor)Put(x,y,TileID.GrayBrick);}
            Main.spawnTileX=center;Main.spawnTileY=floor-2;p.SpawnX=center;p.SpawnY=floor-2;p.position=new Vector2(center*16,(floor-3)*16);
            // Three broad platforms keep the library clear of the combat ground outside it.
            for(int row=0;row<3;row++)for(int x=center-150;x<center+150;x++)Put(x,floor-row*10,TileID.GrayBrick);
            var items=Mod.GetContent<ModItem>().Select(m=>new Item(m.Type)).OrderBy(i=>Category(i)).ThenBy(i=>i.Name,StringComparer.Ordinal).ToArray();
            int chestIndex=0;string category="";int slot=40;Chest chest=null;
            foreach(var item in items)
            {
                string next=Category(item);
                if(next!=category||slot==40)
                {
                    category=next;int row=1+chestIndex/6,col=chestIndex%6;int x=center-42+col*14,y=floor-row*10-1;
                    int index=WorldGen.PlaceChest(x,y,TileID.Containers,false,0);if(index<0)throw new Exception("Could not place catalog chest "+chestIndex);
                    chest=Main.chest[index];chest.name=$"{chestIndex+1:00} {category}";WorldGen.PlaceObject(x+4,y,TileID.Signs);int sign=Sign.ReadSign(x+4,y);if(sign>=0)Main.sign[sign].text=chest.name;slot=0;chestIndex++;
                }
                chest.item[slot].SetDefaults(item.type);chest.item[slot].stack=Math.Min(item.maxStack,item.consumable?999:item.createTile>=0?100:item.maxStack>1?999:1);slot++;
            }
            var ingredients=Main.recipe.Where(r=>r!=null&&r.createItem.ModItem?.Mod==Mod).SelectMany(r=>r.requiredItem).Where(i=>i!=null&&!i.IsAir&&i.ModItem==null).Select(i=>i.type)
                .Concat(new int[]{ItemID.SuperHealingPotion,ItemID.SuperManaPotion,ItemID.Wood,ItemID.StoneBlock,ItemID.Silk,ItemID.Daybloom,ItemID.Book,ItemID.Gel,ItemID.BottledWater,ItemID.CopperBar,ItemID.IronBar,ItemID.GoldBar,ItemID.DemoniteBar}).Distinct().OrderBy(t=>t).ToArray();
            for(int i=0;i<ingredients.Length;i++)
            {
                if(i%40==0){int row=1+chestIndex/6,col=chestIndex%6;int index=WorldGen.PlaceChest(center-42+col*14,floor-row*10-1,TileID.Containers,false,0);if(index<0)throw new Exception("Ingredient chest failed");chest=Main.chest[index];chest.name="Ingredients "+(i/40+1);chestIndex++;}
                chest.item[i%40].SetDefaults(ingredients[i]);chest.item[i%40].stack=chest.item[i%40].maxStack;
            }
            int[] hotbar={ModContent.ItemType<AetherloomWand>(),ModContent.ItemType<FieldJournal>(),ModContent.ItemType<GardenMirror>(),ModContent.ItemType<CrackedBellPortal>(),ModContent.ItemType<TideCompass>(),ModContent.ItemType<CitadelSigil>(),ModContent.ItemType<CinderKey>(),ModContent.ItemType<StarglassChart>(),ModContent.ItemType<CelestialLoom>(),ItemID.Zenith};
            for(int i=0;i<hotbar.Length;i++)p.inventory[i].SetDefaults(hotbar[i]);
            p.inventory[10].SetDefaults(ItemID.SuperHealingPotion);p.inventory[10].stack=999;p.inventory[11].SetDefaults(ItemID.SuperManaPotion);p.inventory[11].stack=999;
            int[] stations={TileID.WorkBenches,TileID.Anvils,TileID.Furnaces,TileID.Loom,TileID.AlchemyTable,TileID.LunarCraftingStation};for(int i=0;i<stations.Length;i++)WorldGen.PlaceObject(center-50+i*8,floor-1,stations[i]);
            int[] furniture={ModContent.TileType<CauldronWorkshopTile>(),ModContent.TileType<NoticeboardTile>(),ModContent.TileType<PetBedTile>(),ModContent.TileType<FeedingBowlTile>(),ModContent.TileType<GardenLanternTile>(),ModContent.TileType<MemorialWreathTile>()};for(int i=0;i<furniture.Length;i++)WorldGen.PlaceObject(center+10+i*7,floor-1,furniture[i]);
            for(int x=center-160;x<center+160;x++)for(int y=floor-32;y<floor+12;y++)WorldGen.SquareTileFrame(x,y);
            foreach(var town in Main.npc)if(town.active&&(town.type==ModContent.NPCType<Lyra>()||town.type==ModContent.NPCType<CemeteryKeeper>()))town.active=false;
            NPC.NewNPC(p.GetSource_Misc("Showcase"),(center-60)*16,(floor-3)*16,ModContent.NPCType<Lyra>());NPC.NewNPC(p.GetSource_Misc("Showcase"),(center+60)*16,(floor-3)*16,ModContent.NPCType<CemeteryKeeper>());
            Main.worldName="Nyuu Showcase";Main.ActiveWorldFileData.Name=Main.worldName;
            WorldFile.SaveWorld();
            string players=Path.Combine(Main.SavePath,"Players");Directory.CreateDirectory(players);string playerPath=Path.Combine(players,"Nyuu_Showcase.plr");var file=new PlayerFileData(playerPath,false){Player=p,Metadata=FileMetadata.FromCurrentSettings(FileType.Player)};typeof(Player).GetMethod("InternalSavePlayerFile",BindingFlags.Static|BindingFlags.NonPublic).Invoke(null,new object[]{file});
            var read=Player.LoadPlayer(playerPath,false);if(read.Player.ConsumedLifeCrystals!=15||read.Player.ConsumedLifeFruit!=20||read.Player.ConsumedManaCrystals!=9||read.Player.name!=p.name||read.Player.inventory[0].type!=hotbar[0]||read.Player.GetModPlayer<SpellcraftPlayer>().Experience[0]!=7500)throw new Exception("Player save roundtrip failed");
            int stored=0;foreach(var c in Main.chest)if(c!=null)foreach(var i in c.item)if(i.ModItem?.Mod==Mod)stored++;
            if(stored<items.Length)throw new Exception("The showcase catalog is missing items");
            var catalog=items.Select(i=>new{InternalName=i.ModItem.Name,Name=i.Name,Category=Category(i),Texture=i.ModItem.Texture,MaxStack=i.maxStack,Recipes=Main.recipe.Where(r=>r!=null&&r.createItem.type==i.type).Select(r=>new{Count=r.createItem.stack,Ingredients=r.requiredItem.Where(v=>v!=null&&!v.IsAir).Select(v=>new{Name=v.Name,Count=v.stack}).ToArray(),Station=r.requiredTile<0?"By hand":ModContent.GetModTile(r.requiredTile)?.Name??TileID.Search.GetName(r.requiredTile)}).ToArray()}).ToArray();
            File.WriteAllText(Path.Combine(Main.SavePath,"showcase-catalog.json"),JsonSerializer.Serialize(catalog,new JsonSerializerOptions{WriteIndented=true}));
            File.WriteAllText(Path.Combine(Main.SavePath,"showcase-result.txt"),$"Character save roundtrip passed. {items.Length} mod items, {ingredients.Length} vanilla ingredients, {chestIndex} catalog chests, six separate destinations.");
            Console.WriteLine("SHOWCASE_COMPLETE: "+items.Length+" items in "+chestIndex+" chests; player save reloaded successfully.");Environment.Exit(0);
        }
        catch(Exception e){File.WriteAllText(Path.Combine(Main.SavePath,"showcase-failure.txt"),e.ToString());Console.WriteLine(e);Environment.Exit(1);}
    }
    static string Category(Item i)=>i.ModItem is BackpackItem?"Backpacks":i.accessory?"Accessories":i.vanity||i.defense>0?"Outfits and armor":i.damage>0&&!i.consumable?"Weapons":i.createTile>=0?"Building":i.consumable?"Consumables":"Materials and tools";
}




