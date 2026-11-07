using System;
using System.Linq;
using Microsoft.Xna.Framework;
using Terraria;
using Terraria.ID;
using Terraria.ModLoader;
using NyuuTerraria.Content.Reverie;
using NyuuTerraria.Content.Spellcraft;

namespace NyuuTerraria.Content.Frontiers;

public abstract class FrontierMaterial : ModItem
{public override void SetDefaults(){Item.width=28;Item.height=30;Item.maxStack=9999;Item.rare=ItemRarityID.Green;Item.value=Item.sellPrice(silver:10);}}
public class TidePearl : FrontierMaterial { }
public class PaleBloom : FrontierMaterial { }
public class CinderBloom : FrontierMaterial { }
public class StarglassShard : FrontierMaterial { }
public class GardenerRelic : FrontierMaterial { }
public class ArchiveRelic : FrontierMaterial { }
public abstract class FrontierKey : ModItem
{
    public abstract int Area{get;}
    public override void SetDefaults(){Item.width=32;Item.height=36;Item.useStyle=ItemUseStyleID.HoldUp;Item.useTime=Item.useAnimation=45;Item.rare=ItemRarityID.Green;}
    public override bool CanUseItem(Player p)=>FrontierWorld.Unlocked(Area)&&!p.dead&&!Main.npc.Any(n=>n.active&&n.boss);
    public override bool? UseItem(Player p)
    {
        if(p.whoAmI==Main.myPlayer)
        {if(Main.netMode==NetmodeID.MultiplayerClient){var packet=Mod.GetPacket();packet.Write((byte)4);packet.Write((byte)Area);packet.Send();}else if(!ModContent.GetInstance<FrontierWorld>().Travel(p,Area))Main.NewText("No unused space was found for this destination. Existing builds are preserved.",Color.LightPink);}
        return true;
    }
}
public class TideCompass : FrontierKey
{public override int Area=>0;public override void AddRecipes()=>CreateRecipe().AddIngredient(ItemID.Coral,8).AddIngredient(ItemID.Glass,10).AddIngredient(ItemID.FallenStar,2).AddTile(TileID.WorkBenches).Register();}
public class CitadelSigil : FrontierKey
{public override int Area=>1;public override void AddRecipes()=>CreateRecipe().AddIngredient<MoonprismBar>(5).AddIngredient(ItemID.Vine,3).AddTile(TileID.Anvils).Register();}
public class CinderKey : FrontierKey
{public override int Area=>2;public override void AddRecipes()=>CreateRecipe().AddIngredient(ItemID.Obsidian,20).AddIngredient(ItemID.HellstoneBar,5).AddTile(TileID.Anvils).Register();}
public class StarglassChart : FrontierKey
{public override int Area=>3;public override void AddRecipes()=>CreateRecipe().AddIngredient(ItemID.LunarBar,5).AddIngredient(ItemID.FallenStar,15).AddTile(TileID.LunarCraftingStation).Register();}
public class FieldJournal : ModItem
{
    public override void SetDefaults(){Item.width=30;Item.height=32;Item.useStyle=ItemUseStyleID.HoldUp;Item.useTime=Item.useAnimation=20;}
    public override bool? UseItem(Player p){if(p.whoAmI==Main.myPlayer){ChronicleInterface.Tab=4;ChronicleInterface.Open=true;}return true;}
    public override void AddRecipes()=>CreateRecipe().AddIngredient(ItemID.Book).AddIngredient(ItemID.Silk,2).AddTile(TileID.WorkBenches).Register();
}
public class RitualFocus : ModItem
{
    public override void SetDefaults(){Item.width=30;Item.height=32;Item.maxStack=999;Item.consumable=true;Item.useStyle=ItemUseStyleID.HoldUp;Item.useTime=Item.useAnimation=45;Item.mana=25;Item.rare=ItemRarityID.Orange;Item.shoot=ModContent.ProjectileType<RitualCircle>();}
    public override bool CanUseItem(Player p)=>p.ownedProjectileCounts[ModContent.ProjectileType<RitualCircle>()]==0;
    public override bool? UseItem(Player p){if(p.whoAmI==Main.myPlayer)Projectile.NewProjectile(p.GetSource_ItemUse(Item),p.Center+new Vector2(p.direction*100,0),Vector2.Zero,ModContent.ProjectileType<RitualCircle>(),45,4,p.whoAmI);return true;}
    public override void AddRecipes()=>CreateRecipe(3).AddIngredient<MoonprismBar>(2).AddIngredient(ItemID.FallenStar,2).AddTile(TileID.Anvils).Register();
}
public class RitualCircle : ModProjectile
{
    public override string Texture=>"NyuuTerraria/Content/Frontiers/RitualFocus";
    public override void SetDefaults(){Projectile.width=Projectile.height=220;Projectile.timeLeft=240;Projectile.friendly=true;Projectile.DamageType=DamageClass.Magic;Projectile.penetrate=-1;Projectile.tileCollide=false;Projectile.usesLocalNPCImmunity=true;Projectile.localNPCHitCooldown=40;}
    public override bool ShouldUpdatePosition()=>false;
    public override bool? CanDamage()=>Projectile.timeLeft<=150&&Projectile.timeLeft%40<4?null:false;
    public override void AI(){Lighting.AddLight(Projectile.Center,.35f,.2f,.5f);if(Projectile.timeLeft==150&&Projectile.owner==Main.myPlayer)Main.player[Projectile.owner].GetModPlayer<FrontierPlayer>().Discover("first-ritual");}
    public override bool PreDraw(Player p,ref Color lightColor){float strength=Math.Min(1,(240-Projectile.timeLeft)/90f);SpellVisuals.Rune(Projectile.Center,95*strength,new Color(211,141,247),Projectile.timeLeft*.012f,Projectile.timeLeft>150?.35f: .9f);return false;}
}
public class CelestialLoom : ModItem
{
    public override void SetDefaults(){Item.width=46;Item.height=48;Item.damage=105;Item.DamageType=DamageClass.Magic;Item.mana=14;Item.useTime=Item.useAnimation=28;Item.useStyle=ItemUseStyleID.Shoot;Item.noMelee=true;Item.shoot=ModContent.ProjectileType<CelestialStar>();Item.shootSpeed=11;Item.autoReuse=true;Item.rare=ItemRarityID.Red;Item.value=Item.sellPrice(gold:8);}
    public override void SetStaticDefaults()=>Item.staff[Type]=true;
    public override void AddRecipes()=>CreateRecipe().AddIngredient<StarglassShard>(15).AddIngredient<ArchiveRelic>(3).AddIngredient(ItemID.LunarBar,8).AddTile(TileID.LunarCraftingStation).Register();
}
public class CelestialStar : ModProjectile
{
    public override string Texture=>"NyuuTerraria/Content/Frontiers/StarglassShard";
    public override void SetDefaults(){Projectile.width=Projectile.height=24;Projectile.timeLeft=180;Projectile.friendly=true;Projectile.DamageType=DamageClass.Magic;Projectile.penetrate=3;Projectile.usesLocalNPCImmunity=true;Projectile.localNPCHitCooldown=20;}
    public override void AI(){Projectile.rotation+=.1f;Lighting.AddLight(Projectile.Center,.35f,.3f,.65f);if(Projectile.timeLeft%18==0&&Projectile.owner==Main.myPlayer)SpellVisuals.CastSigil(Projectile.GetSource_FromAI(),Projectile.Center,Spells.Arcane,Projectile.owner);}
    public override void OnKill(int timeLeft){if(Projectile.owner==Main.myPlayer)for(int i=0;i<5;i++)Projectile.NewProjectile(Projectile.GetSource_Death(),Projectile.Center,Vector2.UnitX.RotatedBy(i*MathHelper.TwoPi/5)*7,ModContent.ProjectileType<SpellShot>(),Projectile.damage/3,2,Projectile.owner,Spells.Arcane);}
    public override bool PreDraw(Player p,ref Color color){SpellVisuals.Rune(Projectile.Center,20,new Color(181,166,255),Projectile.rotation,1);return false;}
}
public class ExplorerMedallion : ModItem
{
    public override void SetDefaults(){Item.width=28;Item.height=30;Item.accessory=true;Item.rare=ItemRarityID.Orange;}
    public override void UpdateAccessory(Player p,bool hideVisual){p.pickSpeed-=.08f;p.moveSpeed+=.05f;p.findTreasure=true;}
}
public class SpellPalette : ModItem
{
    public override void SetDefaults(){Item.width=28;Item.height=30;Item.useStyle=ItemUseStyleID.HoldUp;Item.useTime=Item.useAnimation=20;}
    public override bool? UseItem(Player p){if(p.whoAmI==Main.myPlayer){var d=p.GetModPlayer<FrontierPlayer>();d.Palette=(d.Palette+1)%4;Main.NewText("Spell style: "+new[]{"Elemental","Trans pride","Moonlit","Ember dusk"}[d.Palette],Color.LightPink);}return true;}
    public override void AddRecipes()=>CreateRecipe().AddIngredient(ItemID.Glass,5).AddIngredient(ItemID.FallenStar,3).AddIngredient(ItemID.Silk,2).AddTile(TileID.Loom).Register();
}
public class HeritageRibbon : ModItem
{
    public override void SetDefaults(){Item.width=28;Item.height=30;Item.accessory=true;Item.rare=ItemRarityID.Orange;}
    public override void UpdateAccessory(Player p,bool hideVisual){p.statManaMax2+=20;p.lifeRegen+=1;}
}
