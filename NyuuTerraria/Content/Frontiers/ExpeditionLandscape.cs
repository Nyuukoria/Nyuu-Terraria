using System;
using Microsoft.Xna.Framework;
using Terraria;
using Terraria.ID;
namespace NyuuTerraria.Content.Frontiers;

// Tile-built landscapes. A shared grammar keeps landing pads safe without turning every area into an arena.
internal static class ExpeditionLandscape
{
    internal static void Tile(int x,int y,int type){if(!WorldGen.InWorld(x,y,10))return;var t=Main.tile[x,y];t.HasTile=true;t.TileType=(ushort)type;t.TileFrameX=t.TileFrameY=0;}
    internal static int Surface(Rectangle r,int floor,int x)
    {int u=x-r.Left;if(u<42||u>r.Width-70||u>=112&&u<=184)return floor;return floor+(int)Math.Round(Math.Sin(u*.072)*3+Math.Sin(u*.161)*1.5);}
    internal static void Build(Rectangle r,int floor,int theme)
    {
        int[] soil={TileID.Dirt,TileID.GrayBrick,TileID.Sandstone,TileID.Dirt,TileID.Ash,TileID.Pearlstone};
        int[] cap={TileID.Grass,TileID.GrayBrick,TileID.Sand,TileID.Grass,TileID.Ash,TileID.Pearlstone};
        for(int x=r.Left;x<r.Right;x++){
            int top=Surface(r,floor,x);
            for(int y=top;y<r.Bottom;y++){Tile(x,y,y==top?cap[theme]:soil[theme]);if(y>top+4&&((x*17+y*23)%43<4))Tile(x,y,theme==4?TileID.Obsidian:theme==5?TileID.Glass:TileID.Stone);}
        }
        if(theme==1){Room(r.Left+42,floor,56,23,false);Room(r.Right-70,floor,46,28,false);Bridge(r.Left+101,r.Left+121,floor-9);}
        if(theme==3){Room(r.Right-71,floor,48,25,true);Room(r.Left+51,Surface(r,floor,r.Left+51),32,15,true);}
        if(theme==4){Room(r.Right-69,floor,45,20,false);for(int x=r.Left+52;x<r.Left+88;x++)for(int y=floor+3;y<floor+7;y++){var t=Main.tile[x,y];t.HasTile=false;t.LiquidType=LiquidID.Lava;t.LiquidAmount=230;}}
        if(theme==0||theme==3){for(int n=0;n<5;n++){int x=r.Left+48+n*38;if(theme==3&&n==4)continue;int ground=Surface(r,floor,x);ExpeditionScenery.Tree(x,ground,12+n%3*2,n);}}
        if(theme==4){for(int n=0;n<4;n++){int x=r.Left+104+n*26;if(x>r.Right-80)break;ExpeditionScenery.Tree(x,Surface(r,floor,x),12+n%3,n,true);}}
        if(theme==2){for(int n=0;n<4;n++){int x=r.Left+48+n*44;Pool(x,Surface(r,floor,x),20,7);for(int k=0;k<5;k++)Tile(x+2+k*3,floor+6,TileID.Coralstone);}Room(r.Right-68,floor,44,17,false);}
        if(theme==0){Pool(r.Left+68,Surface(r,floor,r.Left+68),18,5);Room(r.Right-65,floor,40,14,true);}
        if(theme==5){
            for(int n=0;n<5;n++){
                int cx=r.Left+55+n*43,cy=floor-19-(n%3)*10;
                for(int dx=-12;dx<=12;dx++)for(int dy=0;dy<7;dy++)if(Math.Abs(dx)+dy*2<22)Tile(cx+dx,cy+dy,dy<2?TileID.Pearlstone:TileID.Glass);
                for(int h=0;h<6;h++)Tile(cx+5,cy-h,TileID.Glass);
                if(n<4)Bridge(cx+12,cx+30,cy+2);
            }Room(r.Right-68,floor,43,24,false);
        }
        // Two short, optional fighting ledges; never three wall-to-wall scaffold lines.
        if(theme==1||theme==4){Bridge(r.Left+117,r.Left+178,floor-10);Bridge(r.Left+126,r.Left+169,floor-21);}
        for(int x=r.Left+5;x<r.Right-5;x+=11){int top=Surface(r,floor,x);if(x<r.Left+42||x>r.Right-73)continue;
            if(theme==0||theme==3){if(!Main.tile[x,top-1].HasTile)Tile(x,top-1,TileID.Plants);}
            else if(theme==4&&x%2==0)Tile(x,top-1,TileID.Obsidian);
        }
        // Give the landing an unmistakable, properly anchored lantern.
        WorldGen.PlaceObject(r.Left+17,floor-1,TileID.Lampposts);
    }
    private static void Pool(int x,int floor,int width,int depth){for(int dx=0;dx<width;dx++)for(int dy=0;dy<depth;dy++)if(dy<depth-1&&dx>0&&dx<width-1){var t=Main.tile[x+dx,floor+dy];t.HasTile=false;t.LiquidType=LiquidID.Water;t.LiquidAmount=255;}else Tile(x+dx,floor+dy,TileID.Sandstone);}
    private static void Bridge(int left,int right,int y){for(int x=left;x<=right;x++)Tile(x,y,TileID.Platforms);}
    private static void Room(int x,int floor,int width,int height,bool garden){
        int stone=garden?TileID.StoneSlab:TileID.GrayBrick;
        for(int dx=0;dx<width;dx++)for(int dy=0;dy<height;dy++){
            int tx=x+dx,ty=floor-height+dy;
            if(dy<2||dx<2&&dy<height-4||dx>=width-2&&dy<height-4)Tile(tx,ty,stone);
            else if(!Main.tile[tx,ty].HasTile){var tile=Main.tile[tx,ty];tile.WallType=(ushort)(garden?WallID.LivingWood:WallID.GrayBrick);}
            if(dy==height-7&&dx>3&&dx<width-4)Tile(tx,ty,TileID.Platforms);
        }
        for(int dx=5;dx<width-5;dx+=9){WorldGen.PlaceObject(x+dx,floor-8,TileID.Bookcases);WorldGen.PlaceObject(x+dx+3,floor-8,TileID.Candles);}
        WorldGen.PlaceObject(x+8,floor-1,TileID.Tables);
        if(garden)for(int dx=2;dx<width-2;dx+=4)for(int dy=0;dy<3;dy++)Tile(x+dx,floor-height-1-dy,TileID.LeafBlock);
    }
}
