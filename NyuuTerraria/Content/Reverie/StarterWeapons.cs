using Microsoft.Xna.Framework;
using Terraria;
using Terraria.DataStructures;
using Terraria.ID;
using Terraria.ModLoader;

namespace NyuuTerraria.Content.Reverie;
public class MoonprismBow : ModItem
{
    public override void SetDefaults()
    {
        Item.CloneDefaults(ItemID.GoldBow);Item.width=28;Item.height=42;Item.damage=27;Item.knockBack=3;
        Item.useTime=Item.useAnimation=19;Item.autoReuse=true;Item.rare=ItemRarityID.Green;Item.value=Item.sellPrice(silver:70);
    }
    public override bool Shoot(Player player,EntitySource_ItemUse_WithAmmo source,Vector2 position,Vector2 velocity,int type,int damage,float knockback)
    {
        Projectile.NewProjectile(source,position,velocity,ModContent.ProjectileType<MoonprismArrow>(),damage,knockback,player.whoAmI);return false;
    }
    public override void AddRecipes()=>CreateRecipe().AddIngredient<MoonprismBar>(7).AddIngredient(ItemID.Wood,12).AddTile(TileID.Anvils).Register();
}
public class MoonprismArrow : PrismMothBolt
{
    public override string Texture=>"NyuuTerraria/Content/Reverie/MoonprismOre";
    public override void SetDefaults() {base.SetDefaults();Projectile.DamageType=DamageClass.Ranged;Projectile.penetrate=2;Projectile.arrow=true;}
}
public class CoralBoomerang : ModItem
{
    public override void SetDefaults()
    {
        Item.CloneDefaults(ItemID.WoodenBoomerang);Item.width=32;Item.height=30;Item.damage=22;Item.knockBack=5;
        Item.shoot=ModContent.ProjectileType<CoralBoomerangFlight>();Item.shootSpeed=12;Item.rare=ItemRarityID.Blue;
    }
    public override bool CanUseItem(Player player)=>player.ownedProjectileCounts[Item.shoot]==0;
    public override void AddRecipes()=>CreateRecipe().AddIngredient(ItemID.Coral,8).AddIngredient(ItemID.Wood,15).AddTile(TileID.WorkBenches).Register();
}
public class CoralBoomerangFlight : PrismMothBolt
{
    public override string Texture=>"NyuuTerraria/Content/Reverie/CoralBoomerang";
    public override void SetDefaults() {base.SetDefaults();Projectile.DamageType=DamageClass.Melee;Projectile.penetrate=-1;Projectile.timeLeft=240;}
    public override void AI()
    {
        Projectile.frame=(int)(Main.GameUpdateCount/10)%4;
        Player p=Main.player[Projectile.owner];
        if(!p.active || p.dead) {Projectile.Kill();return;}
        Projectile.rotation+=.35f;
        if(++Projectile.ai[0]>26)
        {
            Projectile.tileCollide=false;
            Projectile.velocity=Vector2.Lerp(Projectile.velocity,(p.Center-Projectile.Center).SafeNormalize(Vector2.Zero)*17,.16f);
            if(Projectile.Distance(p.Center)<24) Projectile.Kill();
        }
        Lighting.AddLight(Projectile.Center,.3f,.15f,.1f);
    }
    public override bool OnTileCollide(Vector2 oldVelocity) {Projectile.ai[0]=27;return false;}
}
public class MooncapScepter : ModItem
{
    public override void SetDefaults()
    {
        Item.width=42;Item.height=44;Item.damage=19;Item.DamageType=DamageClass.Summon;Item.mana=10;
        Item.useStyle=ItemUseStyleID.Swing;Item.useAnimation=Item.useTime=25;Item.noMelee=true;Item.knockBack=2;
        Item.shoot=ModContent.ProjectileType<MooncapMinion>();Item.buffType=ModContent.BuffType<MooncapBond>();Item.rare=ItemRarityID.Green;Item.UseSound=SoundID.Item44;
    }
    public override bool Shoot(Player player,EntitySource_ItemUse_WithAmmo source,Vector2 position,Vector2 velocity,int type,int damage,float knockback)
    {
        player.AddBuff(Item.buffType,2);int id=Projectile.NewProjectile(source,Main.MouseWorld,Vector2.Zero,type,damage,knockback,player.whoAmI);
        Main.projectile[id].originalDamage=Item.damage;return false;
    }
    public override void AddRecipes()=>CreateRecipe().AddIngredient<MoonprismBar>(5).AddIngredient(ItemID.GlowingMushroom,15).AddIngredient(ItemID.Wood,10).AddTile(TileID.Anvils).Register();
}
public class MooncapBond : ModBuff
{
    public override string Texture=>"NyuuTerraria/Content/Reverie/MooncapScepter";
    public override void SetStaticDefaults() {Main.buffNoTimeDisplay[Type]=true;Main.buffNoSave[Type]=true;}
    public override void Update(Player player,ref int buffIndex)
    {
        if(player.ownedProjectileCounts[ModContent.ProjectileType<MooncapMinion>()]>0) player.buffTime[buffIndex]=18000;
        else {player.DelBuff(buffIndex);buffIndex--;}
    }
}
public class MooncapMinion : ModProjectile
{
    public override string Texture=>"NyuuTerraria/Content/Reverie/MooncapSprite";
    public override void SetStaticDefaults() {Main.projPet[Type]=true;Main.projFrames[Type]=4;ProjectileID.Sets.MinionSacrificable[Type]=true;}
    public override void SetDefaults()
    {
        Projectile.width=24;Projectile.height=26;Projectile.minion=true;Projectile.minionSlots=1;Projectile.friendly=true;
        Projectile.penetrate=-1;Projectile.tileCollide=false;Projectile.DamageType=DamageClass.Summon;Projectile.timeLeft=2;
        Projectile.usesLocalNPCImmunity=true;Projectile.localNPCHitCooldown=25;
    }
    public override void AI()
    {
        Projectile.frame=(int)(Main.GameUpdateCount/10)%4;
        Player p=Main.player[Projectile.owner];
        if(!p.active || p.dead || !p.HasBuff(ModContent.BuffType<MooncapBond>())) return;
        Projectile.timeLeft=2;NPC target=null;float closest=550;
        if(p.HasMinionAttackTargetNPC)
        {
            NPC selected=Main.npc[p.MinionAttackTargetNPC];
            if(selected.CanBeChasedBy() && Projectile.Distance(selected.Center)<750) target=selected;
        }
        if(target==null) foreach(NPC npc in Main.ActiveNPCs)
        {
            float distance=Projectile.Distance(npc.Center);
            if(distance<closest && npc.CanBeChasedBy() && Collision.CanHitLine(Projectile.position,24,26,npc.position,npc.width,npc.height)) {target=npc;closest=distance;}
        }
        Vector2 goal=target?.Center ?? p.Center+new Vector2(-p.direction*44,-45);
        if(Projectile.Distance(p.Center)>1300 && Main.myPlayer==p.whoAmI) {Projectile.Center=p.Center;Projectile.netUpdate=true;}
        Projectile.velocity=Vector2.Lerp(Projectile.velocity,(goal-Projectile.Center).SafeNormalize(Vector2.Zero)*(target==null ? 6 : 10),.08f);
        if(Vector2.Distance(goal,Projectile.Center)<20) Projectile.velocity*=.85f;
        Lighting.AddLight(Projectile.Center,.16f,.12f,.26f);
    }
}
public class ObsidianMemorial : ModItem
{
    public override void SetDefaults()=>Item.DefaultToPlaceableTile(ModContent.TileType<ObsidianMemorialTile>());
    public override void AddRecipes()=>CreateRecipe().AddIngredient(ItemID.Obsidian,15).AddIngredient(ItemID.Daybloom,2).AddTile(TileID.WorkBenches).Register();
}
public class ObsidianMemorialTile : PrismMemorialTile { }
public class CopperMemorial : ModItem
{
    public override void SetDefaults()=>Item.DefaultToPlaceableTile(ModContent.TileType<CopperMemorialTile>());
    public override void AddRecipes()=>CreateRecipe().AddIngredient(ItemID.CopperBar,4).AddIngredient(ItemID.StoneBlock,15).AddTile(TileID.WorkBenches).Register();
}
public class CopperMemorialTile : PrismMemorialTile { }
public class ReverieVase : ModItem
{
    public override void SetDefaults()=>Item.DefaultToPlaceableTile(ModContent.TileType<ReverieVaseTile>());
    public override void AddRecipes()=>CreateRecipe().AddIngredient(ItemID.ClayBlock,5).AddIngredient<MoonprismOre>().AddTile(TileID.Furnaces).Register();
}
public class ReverieVaseTile : ModTile
{
    public override void SetStaticDefaults()
    {
        Main.tileFrameImportant[Type]=true;
        Terraria.ObjectData.TileObjectData.newTile.CopyFrom(Terraria.ObjectData.TileObjectData.Style1x2);
        Terraria.ObjectData.TileObjectData.addTile(Type);AddMapEntry(new Color(172,148,194),CreateMapEntryName());
    }
}


