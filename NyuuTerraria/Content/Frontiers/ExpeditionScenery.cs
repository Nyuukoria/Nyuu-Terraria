using System;
using Terraria;
using Terraria.ID;
namespace NyuuTerraria.Content.Frontiers;

internal static class ExpeditionScenery
{
    private static void Put(int x,int y,int type){if(!WorldGen.InWorld(x,y,10))return;var t=Main.tile[x,y];t.HasTile=true;t.TileType=(ushort)type;t.TileFrameX=t.TileFrameY=0;}
    internal static void Tree(int x,int floor,int height,int variant,bool charred=false)
    {
        // Native trees supply Terraria's connected trunks, branches, foliage and animation.
        for(int dx=-2;dx<=2;dx++)for(int dy=0;dy<7;dy++)Put(x+dx,floor+dy,dy==0?TileID.Grass:TileID.Dirt);
        WorldGen.PlaceObject(x,floor-1,TileID.Saplings);
        WorldGen.GrowTree(x,floor-1);
        if(charred)for(int tx=x-8;tx<=x+8;tx++)for(int ty=floor-38;ty<=floor;ty++){
            var tile=Main.tile[tx,ty];if(tile.HasTile&&(tile.TileType==TileID.Trees||tile.TileType==TileID.Grass))tile.TileColor=29;
        }
    }
}
