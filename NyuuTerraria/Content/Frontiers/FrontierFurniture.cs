using System.Linq;
using System;
using Microsoft.Xna.Framework;
using Terraria;
using Terraria.ID;
using Terraria.ModLoader;
using Terraria.ObjectData;
using NyuuTerraria.Content.Reverie;
namespace NyuuTerraria.Content.Frontiers;
public abstract class FrontierFurniture : ModItem
{
 protected abstract int Tile{get;}
 public override void SetDefaults()=>Item.DefaultToPlaceableTile(Tile);
}
public abstract class FrontierFurnitureTile : ModTile
{
 internal virtual int Columns=>this is CauldronWorkshopTile or NoticeboardTile or PetBedTile?3:2;
 internal virtual int Rows=>this is NoticeboardTile or GardenLanternTile or MemorialWreathTile or TidalMemorialTile or CinderMemorialTile or StarglassMemorialTile?3:this is FeedingBowlTile?1:2;
 public override void SetStaticDefaults()
 {
  Main.tileFrameImportant[Type]=true;Main.tileLavaDeath[Type]=true;Main.tileLighted[Type]=this is GardenLanternTile;
  TileObjectData.newTile.CopyFrom(TileObjectData.Style2x2);TileObjectData.newTile.Width=Columns;TileObjectData.newTile.Height=Rows;
  TileObjectData.newTile.CoordinateHeights=System.Linq.Enumerable.Repeat(16,Rows).ToArray();TileObjectData.newTile.Origin=new Terraria.DataStructures.Point16(Columns/2,Rows-1);
  TileObjectData.newTile.AnchorBottom=new Terraria.DataStructures.AnchorData(Terraria.Enums.AnchorType.SolidTile|Terraria.Enums.AnchorType.SolidWithTop|Terraria.Enums.AnchorType.SolidSide,Columns,0);
  TileObjectData.addTile(Type);AddMapEntry(new Color(164,146,190),CreateMapEntryName());
 }
 public override void ModifyLight(int i,int j,ref float r,ref float g,ref float b){if(this is GardenLanternTile){r=.55f;g=.36f;b=.2f;}}
 public override bool RightClick(int i,int j)
 {
  Player p=Main.LocalPlayer;
  if(this is DiscoveryPlaqueTile){int area=FrontierWorld.AreaAt(new Vector2(i*16,j*16));if(area>=0){p.GetModPlayer<FrontierPlayer>().Discover("landmark-"+FrontierWorld.Keys[area]);Main.NewText(new[]{"The tide remembers every traveler.","Someone tended these gardens long after the gates closed.","The last forge burned for a promise, not a war.","Every shattered star still knows its constellation."}[area],Color.LightCyan);}return true;}
  if(this is FeedingBowlTile){if(Main.netMode==NetmodeID.MultiplayerClient){var packet=Mod.GetPacket();packet.Write((byte)6);packet.Write(i);packet.Write(j);packet.Send();}else WildlifeCare.Feed(p,i,j);return true;}
  if(this is NoticeboardTile){ChronicleInterface.Open=true;ChronicleInterface.Tab=4;FrontierView.Page=0;return true;}
  if(this is CauldronWorkshopTile){Main.playerInventory=true;Recipe.UpdateRecipeList();Main.NewText("Cauldron: craft an infusion from Bottled Water, Daybloom and a destination material while standing nearby.",new Color(191,211,245));return true;}
  return false;
 }
 public override void MouseOver(int i,int j){Main.LocalPlayer.noThrow=2;Main.hoverItemName=System.Text.RegularExpressions.Regex.Replace(Name.Replace("Tile",""),"([a-z])([A-Z])","$1 $2");}
}
public class CauldronWorkshop : FrontierFurniture
{
 public override string Texture=>"NyuuTerraria/Content/Frontiers/CauldronWorkshop";
 protected override int Tile=>ModContent.TileType<CauldronWorkshopTile>();
 public override void AddRecipes()=>CreateRecipe().AddIngredient(ItemID.IronBar,10).AddIngredient(ItemID.Wood,5).AddTile(TileID.WorkBenches).Register();
}
public class CauldronWorkshopTile : FrontierFurnitureTile { }
public class Noticeboard : FrontierFurniture
{
 public override string Texture=>"NyuuTerraria/Content/Frontiers/Noticeboard";
 protected override int Tile=>ModContent.TileType<NoticeboardTile>();
 public override void AddRecipes()=>CreateRecipe().AddIngredient(ItemID.Wood,20).AddIngredient(ItemID.Wood,5).AddTile(TileID.WorkBenches).Register();
}
public class NoticeboardTile : FrontierFurnitureTile { }
public class PetBed : FrontierFurniture
{
 public override string Texture=>"NyuuTerraria/Content/Frontiers/PetBed";
 protected override int Tile=>ModContent.TileType<PetBedTile>();
 public override void AddRecipes()=>CreateRecipe().AddIngredient(ItemID.Silk,8).AddIngredient(ItemID.Wood,5).AddTile(TileID.WorkBenches).Register();
}
public class PetBedTile : FrontierFurnitureTile { }
public class FeedingBowl : FrontierFurniture
{
 public override string Texture=>"NyuuTerraria/Content/Frontiers/FeedingBowl";
 protected override int Tile=>ModContent.TileType<FeedingBowlTile>();
 public override void AddRecipes()=>CreateRecipe().AddIngredient(ItemID.ClayBlock,8).AddIngredient(ItemID.Wood,5).AddTile(TileID.WorkBenches).Register();
}
public class FeedingBowlTile : FrontierFurnitureTile { }
public class GardenLantern : FrontierFurniture
{
 public override string Texture=>"NyuuTerraria/Content/Frontiers/GardenLantern";
 protected override int Tile=>ModContent.TileType<GardenLanternTile>();
 public override void AddRecipes()=>CreateRecipe().AddIngredient(ItemID.Glass,6).AddIngredient(ItemID.Wood,5).AddTile(TileID.WorkBenches).Register();
}
public class GardenLanternTile : FrontierFurnitureTile { }
public class MemorialWreath : FrontierFurniture
{
 public override string Texture=>"NyuuTerraria/Content/Frontiers/MemorialWreath";
 protected override int Tile=>ModContent.TileType<MemorialWreathTile>();
 public override void AddRecipes()=>CreateRecipe().AddIngredient(ItemID.Daybloom,5).AddIngredient(ItemID.Wood,5).AddTile(TileID.WorkBenches).Register();
}
public class MemorialWreathTile : FrontierFurnitureTile { }
public class DiscoveryPlaque : FrontierFurniture
{
 public override string Texture=>"NyuuTerraria/Content/Frontiers/MemorialWreath";
 protected override int Tile=>ModContent.TileType<DiscoveryPlaqueTile>();
 public override void AddRecipes()=>CreateRecipe().AddIngredient(ItemID.StoneBlock,10).AddIngredient(ItemID.Wood,5).AddTile(TileID.WorkBenches).Register();
}
public class DiscoveryPlaqueTile : FrontierFurnitureTile { }
public class TidalMemorial : FrontierFurniture
{
 public override string Texture=>"NyuuTerraria/Content/Frontiers/MemorialWreath";
 protected override int Tile=>ModContent.TileType<TidalMemorialTile>();
 public override void AddRecipes()=>CreateRecipe().AddIngredient(ItemID.Coral,8).AddIngredient(ItemID.Wood,5).AddTile(TileID.WorkBenches).Register();
}
public class TidalMemorialTile : FrontierFurnitureTile { }
public class CinderMemorial : FrontierFurniture
{
 public override string Texture=>"NyuuTerraria/Content/Frontiers/MemorialWreath";
 protected override int Tile=>ModContent.TileType<CinderMemorialTile>();
 public override void AddRecipes()=>CreateRecipe().AddIngredient(ItemID.Obsidian,10).AddIngredient(ItemID.Wood,5).AddTile(TileID.WorkBenches).Register();
}
public class CinderMemorialTile : FrontierFurnitureTile { }
public class StarglassMemorial : FrontierFurniture
{
 public override string Texture=>"NyuuTerraria/Content/Frontiers/MemorialWreath";
 protected override int Tile=>ModContent.TileType<StarglassMemorialTile>();
 public override void AddRecipes()=>CreateRecipe().AddIngredient(ItemID.FallenStar,5).AddIngredient(ItemID.Wood,5).AddTile(TileID.WorkBenches).Register();
}
public class StarglassMemorialTile : FrontierFurnitureTile { }
public abstract class WorkshopInfusion : ModItem
{
 protected abstract int Kind{get;}
 public override string Texture=>"NyuuTerraria/Content/Frontiers/"+new[]{"TidePearl","CinderBloom","StarglassShard"}[Kind];
 public override void SetDefaults(){Item.width=24;Item.height=28;Item.maxStack=999;Item.consumable=true;Item.useStyle=ItemUseStyleID.DrinkLiquid;Item.useTime=Item.useAnimation=20;Item.UseSound=SoundID.Item3;}
 public override bool? UseItem(Player p){var d=p.GetModPlayer<FrontierPlayer>();d.Infusion=Kind;d.InfusionTicks=10800;return true;}
 public override void AddRecipes()=>CreateRecipe().AddIngredient(ItemID.BottledWater).AddIngredient(ItemID.Daybloom).AddIngredient(Kind==0?ModContent.ItemType<TidePearl>():Kind==1?ModContent.ItemType<CinderBloom>():ModContent.ItemType<StarglassShard>()).AddTile(ModContent.TileType<CauldronWorkshopTile>()).Register();
}
public class MoonwellInfusion : WorkshopInfusion {protected override int Kind=>0;}
public class HearthInfusion : WorkshopInfusion {protected override int Kind=>1;}
public class StarfocusInfusion : WorkshopInfusion {protected override int Kind=>2;}

