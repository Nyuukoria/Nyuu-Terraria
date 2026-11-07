using Terraria;
using Terraria.ID;
using Terraria.ModLoader;
using NyuuTerraria.Content.Reverie;
namespace NyuuTerraria.Content.Frontiers;
[AutoloadEquip(EquipType.Body)] public class CoastalCoat : AtelierClothing {public override void AddRecipes()=>CreateRecipe().AddIngredient(ItemID.Silk,10).AddIngredient<TidePearl>(3).AddTile(TileID.Loom).Register();}
[AutoloadEquip(EquipType.Legs)] public class CoastalSkirt : AtelierClothing {public override void AddRecipes()=>CreateRecipe().AddIngredient(ItemID.Silk,10).AddIngredient<TidePearl>(3).AddTile(TileID.Loom).Register();}
[AutoloadEquip(EquipType.Body)] public class Cindercoat : AtelierClothing {public override void AddRecipes()=>CreateRecipe().AddIngredient(ItemID.Silk,10).AddIngredient<CinderBloom>(3).AddTile(TileID.Loom).Register();}
[AutoloadEquip(EquipType.Legs)] public class CinderSkirt : AtelierClothing {public override void AddRecipes()=>CreateRecipe().AddIngredient(ItemID.Silk,10).AddIngredient<CinderBloom>(3).AddTile(TileID.Loom).Register();}
[AutoloadEquip(EquipType.Body)] public class StarglassCoat : AtelierClothing {public override void AddRecipes()=>CreateRecipe().AddIngredient(ItemID.Silk,10).AddIngredient<StarglassShard>(3).AddTile(TileID.Loom).Register();}
[AutoloadEquip(EquipType.Legs)] public class StarglassSkirt : AtelierClothing {public override void AddRecipes()=>CreateRecipe().AddIngredient(ItemID.Silk,10).AddIngredient<StarglassShard>(3).AddTile(TileID.Loom).Register();}
[AutoloadEquip(EquipType.Body)] public class GardenDress : AtelierClothing {public override void AddRecipes()=>CreateRecipe().AddIngredient(ItemID.Silk,10).AddIngredient<PaleBloom>(3).AddTile(TileID.Loom).Register();}
[AutoloadEquip(EquipType.Legs)] public class GardenSkirt : AtelierClothing {public override void AddRecipes()=>CreateRecipe().AddIngredient(ItemID.Silk,10).AddIngredient<PaleBloom>(3).AddTile(TileID.Loom).Register();}
