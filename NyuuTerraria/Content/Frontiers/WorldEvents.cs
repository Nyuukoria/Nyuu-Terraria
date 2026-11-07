using Terraria;
using Terraria.ID;
using Terraria.ModLoader;
namespace NyuuTerraria.Content.Frontiers;
public abstract class FrontierEventItem : ModItem
{
    public abstract int Kind{get;}
    public override string Texture=>"NyuuTerraria/Content/Frontiers/"+(Kind==0?"GardenLantern":"StarglassShard");
    public override void SetDefaults(){Item.width=28;Item.height=30;Item.useStyle=ItemUseStyleID.HoldUp;Item.useTime=Item.useAnimation=45;Item.rare=ItemRarityID.Orange;}
    public override bool CanUseItem(Player p)=>ModContent.GetInstance<FrontierWorld>().EventTicks==0&&(Kind==0||NPC.downedMoonlord);
    public override bool? UseItem(Player p){if(p.whoAmI==Main.myPlayer){if(Main.netMode==NetmodeID.MultiplayerClient){var packet=Mod.GetPacket();packet.Write((byte)9);packet.Write((byte)Kind);packet.Send();}else ModContent.GetInstance<FrontierWorld>().StartEvent(Kind);}return true;}
}
public class GardenFestivalBell : FrontierEventItem {public override int Kind=>0;public override void AddRecipes()=>CreateRecipe().AddIngredient<PaleBloom>(5).AddIngredient(ItemID.Daybloom,5).AddTile(TileID.WorkBenches).Register();}
public class StarlitBeacon : FrontierEventItem {public override int Kind=>1;public override void AddRecipes()=>CreateRecipe().AddIngredient<StarglassShard>(10).AddIngredient(ItemID.LunarBar,2).AddTile(TileID.LunarCraftingStation).Register();}
