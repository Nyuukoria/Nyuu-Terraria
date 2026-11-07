using System;
using System.IO;
using System.Linq;
using Microsoft.Xna.Framework;
using Terraria;
using Terraria.ID;
using Terraria.ModLoader;
using Terraria.ModLoader.IO;
using NyuuTerraria.Content.Reverie;

namespace NyuuTerraria.Content.Frontiers;

public class FrontierWorld : ModSystem
{
    internal static readonly string[] Names={"Tidepool Hollows","Overgrown Citadel","Cinderwood Depths","Starglass Expanse"};
    internal static readonly string[] Keys={"tidepool","citadel","cinderwood","starglass"};
    internal Rectangle[] Areas=new Rectangle[4];
    internal bool GardenerDown,UnwrittenDown;internal int EventTicks,EventKind;private int days;private bool previousDay;
    private ulong[] travel=new ulong[Main.maxPlayers];
    public override void ClearWorld(){Areas=new Rectangle[4];GardenerDown=UnwrittenDown=false;EventTicks=days=0;travel=new ulong[Main.maxPlayers];previousDay=Main.dayTime;}
    internal static int AreaAt(Vector2 p){var a=ModContent.GetInstance<FrontierWorld>().Areas;for(int i=0;i<4;i++)if(a[i].Width>0&&a[i].Contains((p/16f).ToPoint()))return i;return -1;}
    internal static bool Unlocked(int area)=>area<2||area==2&&NPC.downedBoss3||area==3&&NPC.downedMoonlord;
    internal bool EnsureArea(int area)
    {
        if(area<0||area>=4||Main.netMode==NetmodeID.MultiplayerClient)return false;
        if(Areas[area].Width>0)return true;
        for(int row=0;row<2;row++)for(int step=0;step<30;step++)
        {
            int x=45+(area*337+step*277)%(Main.maxTilesX-360),y=35+row*118;
            var r=new Rectangle(x,y,280,96);var margin=r;margin.Inflate(10,10);
            if(margin.Bottom>Math.Min(Main.worldSurface-18,Main.maxTilesY-40))continue;
            if(Areas.Any(a=>a.Width>0&&a.Intersects(margin))||ModContent.GetInstance<RealmWorld>().Areas.Any(a=>a.Width>0&&a.Intersects(margin)))continue;
            bool free=true;
            for(int tx=margin.Left;tx<margin.Right&&free;tx++)for(int ty=margin.Top;ty<margin.Bottom;ty++){var t=Main.tile[tx,ty];if(t.HasTile||t.WallType!=0||t.LiquidAmount>0){free=false;break;}}
            if(!free)continue;
            Areas[area]=r;Build(area,r);Sync();
            if(Main.netMode==NetmodeID.Server)for(int tx=r.Left;tx<r.Right;tx+=20)for(int ty=r.Top;ty<r.Bottom;ty+=20)NetMessage.SendTileSquare(-1,tx,ty,20,20);
            return true;
        }
        return false;
    }
    private static void Put(int x,int y,int type){var t=Main.tile[x,y];t.HasTile=true;t.TileType=(ushort)type;t.TileFrameX=t.TileFrameY=0;}
    private static void Build(int area,Rectangle r)
    {
        int[] ground={TileID.Sandstone,TileID.LivingWood,TileID.Ash,TileID.Pearlstone};int stone=ground[area],floor=r.Bottom-15;
        for(int x=r.Left;x<r.Right;x++)for(int y=r.Top;y<r.Bottom;y++)
        {if(x<r.Left+3||x>=r.Right-3||y>=floor)Put(x,y,stone);}
        // Keep the left landing and middle arena open; landmarks occupy the far side.
        for(int layer=0;layer<3;layer++)for(int x=r.Left+66;x<r.Right-45;x++)Put(x,floor-12-layer*14,TileID.Platforms);
        for(int n=0;n<8;n++)
        {
            int cx=r.Left+48+n*27;
            if(area==0)
            {
                for(int x=cx;x<cx+13&&x<r.Right-4;x++)for(int y=floor+1;y<floor+6;y++){var poolTile=Main.tile[x,y];poolTile.HasTile=false;poolTile.LiquidAmount=200;}
                for(int h=1;h<5;h++)Put(cx-2,floor-h,TileID.Coralstone);
            }
            else if(area==1||area==2)
            {
                if(n%2==0)ExpeditionScenery.Tree(cx,floor,15+n%3*2,n,area==2);
            }
            else for(int h=0;h<6;h++)for(int dx=-2;dx<=2;dx++)if(Math.Abs(dx)+h<7)Put(cx+dx,floor-h,TileID.Glass);
        }
        // A discoverable ruin with a unique, finite chest and plaque.
        for(int x=r.Right-62;x<r.Right-15;x++){Put(x,floor-22,area==1?TileID.StoneSlab:TileID.GrayBrick);if(area==1)for(int y=floor-21;y<floor;y++){var tile=Main.tile[x,y];tile.WallType=WallID.LivingWood;}}
        for(int y=floor-21;y<floor;y++){Put(r.Right-62,y,TileID.GrayBrick);Put(r.Right-16,y,TileID.GrayBrick);}
        for(int x=r.Left;x<r.Right;x++)for(int y=r.Top;y<r.Bottom;y++)WorldGen.SquareTileFrame(x,y);
        int chest=WorldGen.PlaceChest(r.Right-47,floor-1,TileID.Containers,false,0);
        if(chest>=0)
        {
            int[] loot={ModContent.ItemType<TidePearl>(),ModContent.ItemType<PaleBloom>(),ModContent.ItemType<CinderBloom>(),ModContent.ItemType<StarglassShard>()};
            Main.chest[chest].item[0].SetDefaults(loot[area]);Main.chest[chest].item[0].stack=8;
            Main.chest[chest].item[1].SetDefaults(ModContent.ItemType<FieldJournal>());
            Main.chest[chest].item[2].SetDefaults(ItemID.HealingPotion);Main.chest[chest].item[2].stack=5;
        }
        WorldGen.PlaceObject(r.Right-38,floor-1,ModContent.TileType<DiscoveryPlaqueTile>());
    }
    internal bool Travel(Player p,int area)
    {
        if(area<0||area>=4||!Unlocked(area)||p.dead||!p.active||Main.netMode==NetmodeID.MultiplayerClient||Main.npc.Any(n=>n.active&&n.boss))return false;
        ulong now=Main.GameUpdateCount+1;if(travel[p.whoAmI]>0&&now-travel[p.whoAmI]<90)return false;travel[p.whoAmI]=now;
        var d=p.GetModPlayer<FrontierPlayer>();Vector2 pos;
        if(AreaAt(p.Center)>=0)pos=d.ReturnWorld==Main.ActiveWorldFileData.UniqueId.ToString()?d.ReturnPosition:new Vector2(Main.spawnTileX*16,Main.spawnTileY*16-48);
        else
        {
            if(!EnsureArea(area))return false;
            d.ReturnPosition=p.position;d.ReturnWorld=Main.ActiveWorldFileData.UniqueId.ToString();var r=Areas[area];pos=new Vector2((r.Left+22)*16,(r.Bottom-15)*16-p.height-2);
        }
        if(!WorldGen.InWorld((int)pos.X/16,(int)pos.Y/16,20)||Collision.SolidCollision(pos,p.width,p.height))pos=new Vector2(Main.spawnTileX*16,Main.spawnTileY*16-48);
        p.Teleport(pos);p.velocity=Vector2.Zero;p.fallStart=(int)p.position.Y/16;p.immune=true;p.immuneTime=120;
        if(Main.netMode==NetmodeID.Server)NetMessage.SendData(MessageID.TeleportEntity,number:0,number2:p.whoAmI,number3:pos.X,number4:pos.Y);
        return true;
    }
    internal bool StartEvent(int kind)
    {
        if(Main.netMode==NetmodeID.MultiplayerClient||EventTicks>0||kind<0||kind>1||kind==1&&!NPC.downedMoonlord)return false;
        EventKind=kind;EventTicks=kind==0?18000:10800;Sync();return true;
    }
    public override void PostUpdateWorld()
    {
        if(Main.netMode==NetmodeID.MultiplayerClient)return;
        if(EventTicks>0){EventTicks--;if(EventTicks==0)Sync();}
        if(Main.dayTime&&!previousDay){days++;if(days%3==0)StartEvent(0);}previousDay=Main.dayTime;
    }
    internal void Sync(){if(Main.netMode==NetmodeID.Server)NetMessage.SendData(MessageID.WorldData);}
    public override void SaveWorldData(TagCompound t)
    {for(int i=0;i<4;i++)t["frontierArea"+i]=new[]{Areas[i].X,Areas[i].Y,Areas[i].Width,Areas[i].Height};t["gardenerDown"]=GardenerDown;t["unwrittenDown"]=UnwrittenDown;t["frontierDays"]=days;t["frontierEventTicks"]=EventTicks;t["frontierEventKind"]=EventKind;}
    public override void LoadWorldData(TagCompound t)
    {for(int i=0;i<4;i++){var a=t.GetIntArray("frontierArea"+i);if(a.Length==4&&a[2]==280&&a[3]==96&&a[0]>20&&a[1]>20&&a[0]+280<Main.maxTilesX-20&&a[1]+96<Main.maxTilesY-20)Areas[i]=new Rectangle(a[0],a[1],280,96);}GardenerDown=t.GetBool("gardenerDown");UnwrittenDown=t.GetBool("unwrittenDown");days=Math.Max(0,t.GetInt("frontierDays"));EventTicks=Math.Clamp(t.GetInt("frontierEventTicks"),0,18000);EventKind=Math.Clamp(t.GetInt("frontierEventKind"),0,1);}
    public override void NetSend(BinaryWriter w){foreach(var r in Areas){w.Write(r.X);w.Write(r.Y);w.Write(r.Width);w.Write(r.Height);}w.Write(GardenerDown);w.Write(UnwrittenDown);w.Write(EventTicks);w.Write((byte)EventKind);}
    public override void NetReceive(BinaryReader r){for(int i=0;i<4;i++)Areas[i]=new Rectangle(r.ReadInt32(),r.ReadInt32(),r.ReadInt32(),r.ReadInt32());GardenerDown=r.ReadBoolean();UnwrittenDown=r.ReadBoolean();EventTicks=r.ReadInt32();EventKind=r.ReadByte();}
}
public abstract class FrontierBiome : ModBiome
{protected abstract int Area{get;}public override string BestiaryIcon=>"NyuuTerraria/Content/Frontiers/"+new[]{"TideCompass","CitadelSigil","CinderKey","StarglassChart"}[Area];public override SceneEffectPriority Priority=>SceneEffectPriority.BiomeHigh;public override int Music=>Area==0?MusicID.Hallow:Area==1?MusicID.Hallow:Area==2?MusicID.Eerie:MusicID.Hallow;public override bool IsBiomeActive(Player p)=>FrontierWorld.AreaAt(p.Center)==Area;}
public class TidepoolHollowsBiome : FrontierBiome {protected override int Area=>0;}
public class OvergrownCitadelBiome : FrontierBiome {protected override int Area=>1;}
public class CinderwoodDepthsBiome : FrontierBiome {protected override int Area=>2;}
public class StarglassExpanseBiome : FrontierBiome {protected override int Area=>3;}



