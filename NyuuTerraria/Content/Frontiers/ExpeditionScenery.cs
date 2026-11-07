using System;
using Terraria;
using Terraria.ID;
namespace NyuuTerraria.Content.Frontiers;

internal static class ExpeditionScenery
{
    private static void Put(int x,int y,int type){if(!WorldGen.InWorld(x,y,10))return;var t=Main.tile[x,y];t.HasTile=true;t.TileType=(ushort)type;t.TileFrameX=t.TileFrameY=0;}
    internal static void Tree(int x,int floor,int height,int variant,bool charred=false)
    {
        int wood=charred?TileID.Obsidian:TileID.LivingWood;
        for(int y=floor-height;y<floor;y++){Put(x,y,wood);if(y>floor-4)Put(x-1,y,wood);}
        for(int root=-2;root<=2;root++)Put(x+root,floor-Math.Max(0,2-Math.Abs(root)),wood);
        for(int side=-1;side<=1;side+=2)for(int length=1;length<=4;length++)Put(x+side*length,floor-height/2-length/2,wood);
        if(charred){for(int k=-3;k<=3;k++)if(k%2==0)Put(x+k,floor-height+Math.Abs(k),TileID.Ash);return;}
        // Rounded, layered crowns replace the former single horizontal strip of leaves.
        int radius=5+variant%3,top=floor-height-1;
        for(int dx=-radius;dx<=radius;dx++)for(int dy=-4;dy<=4;dy++)
            if(dx*dx*16+dy*dy*radius*radius<=radius*radius*16&&!(Math.Abs(dx)==radius&&Math.Abs(dy)>1))Put(x+dx,top+dy,TileID.LeafBlock);
        for(int side=-1;side<=1;side+=2)for(int dx=-3;dx<=3;dx++)for(int dy=-2;dy<=2;dy++)
            if(dx*dx+dy*dy*2<=10)Put(x+side*4+dx,top+3+dy,TileID.LeafBlock);
    }
}
