using System;
using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Graphics;
using Terraria;
using Terraria.DataStructures;
using Terraria.GameContent;
using Terraria.ID;
using Terraria.ModLoader;
using Terraria.ObjectData;

namespace NyuuTerraria.Content.Reverie;

public class MoonprismOreTile : ModTile
{
    public override void SetStaticDefaults()
    {
        Main.tileSolid[Type]=true;Main.tileBlockLight[Type]=true;Main.tileSpelunker[Type]=true;Main.tileMergeDirt[Type]=true;
        Main.tileOreFinderPriority[Type]=260;TileID.Sets.Ore[Type]=true;
        AddMapEntry(new Color(182,156,221),CreateMapEntryName());MineResist=1.6f;DustType=DustID.PinkCrystalShard;
    }
    public override void ModifyLight(int i,int j,ref float r,ref float g,ref float b) {r=.12f;g=.1f;b=.2f;}
}
public class MoonprismOre : ModItem
{
    public override void SetDefaults() {Item.DefaultToPlaceableTile(ModContent.TileType<MoonprismOreTile>());Item.width=26;Item.height=24;}
}
public class MoonprismBar : ModItem
{
    public override void SetDefaults() {Item.width=32;Item.height=22;Item.maxStack=Item.CommonMaxStack;Item.rare=ItemRarityID.Green;Item.value=Item.sellPrice(silver:7);}
    public override void AddRecipes()=>CreateRecipe().AddIngredient<MoonprismOre>(3).AddTile(TileID.Furnaces).Register();
}
public class EchoFragment : ModItem
{
    public override string Texture=>"NyuuTerraria/Content/Reverie/EchoFragment";
    public override void SetDefaults() {Item.width=24;Item.height=28;Item.maxStack=Item.CommonMaxStack;Item.rare=ItemRarityID.Green;}
}
public class PrismWingScale : ModItem
{
    public override string Texture=>"NyuuTerraria/Content/Reverie/PrismWingScale";
    public override void SetDefaults() {Item.width=24;Item.height=24;Item.maxStack=Item.CommonMaxStack;Item.rare=ItemRarityID.Green;}
}
public class PrismMothStaff : ModItem
{
    public override void SetStaticDefaults()=>Item.staff[Type]=true;
    public override void SetDefaults()
    {
        Item.width=46;Item.height=48;Item.damage=42;Item.DamageType=DamageClass.Magic;Item.mana=5;
        Item.useTime=Item.useAnimation=12;Item.useStyle=ItemUseStyleID.Shoot;Item.noMelee=true;Item.autoReuse=true;
        Item.knockBack=4;Item.crit=16;Item.shoot=ModContent.ProjectileType<PrismMothBolt>();Item.shootSpeed=12;
        Item.UseSound=SoundID.Item20;Item.rare=ItemRarityID.Green;Item.value=Item.sellPrice(gold:1);
    }
    public override void AddRecipes()=>CreateRecipe().AddIngredient<MoonprismBar>(10).AddIngredient(ItemID.FallenStar,4).AddIngredient(ItemID.Wood,12).AddTile(TileID.Anvils).Register();
}
public class EchoScythe : ModItem
{
    public override void SetDefaults()
    {
        Item.width=50;Item.height=50;Item.damage=56;Item.DamageType=DamageClass.Melee;Item.useTime=Item.useAnimation=23;
        Item.useStyle=ItemUseStyleID.Swing;Item.autoReuse=true;Item.knockBack=5;Item.UseSound=SoundID.Item1;
        Item.shoot=ModContent.ProjectileType<EchoSlash>();Item.shootSpeed=10;Item.rare=ItemRarityID.Orange;Item.value=Item.sellPrice(gold:1);
    }
    public override void AddRecipes()=>CreateRecipe().AddIngredient<EchoFragment>(12).AddIngredient<MoonprismBar>(8).AddTile(TileID.Anvils).Register();
}
public class PrismMothBolt : ModProjectile
{
    public override string Texture=>"NyuuTerraria/Content/Reverie/PrideMothCharm";
    public override void SetStaticDefaults() {ProjectileID.Sets.TrailCacheLength[Type]=16;ProjectileID.Sets.TrailingMode[Type]=0;}
    public override void SetDefaults()
    {
        Projectile.width=20;Projectile.height=20;Projectile.friendly=true;Projectile.DamageType=DamageClass.Magic;
        Projectile.timeLeft=210;Projectile.penetrate=3;Projectile.usesLocalNPCImmunity=true;Projectile.localNPCHitCooldown=12;
    }
    public override void AI()
    {
        Projectile.rotation=Projectile.velocity.ToRotation()+MathHelper.PiOver2;
        Projectile.velocity=Projectile.velocity.RotatedBy(MathF.Sin(Projectile.ai[0]++*.12f)*.012f);
        if(Projectile.timeLeft%3==0)
        {
            var d=Dust.NewDustPerfect(Projectile.Center,DustID.RainbowMk2,-Projectile.velocity*.08f,100,
                Projectile.timeLeft%2==0 ? new Color(245,169,184) : new Color(91,206,250),.8f);d.noGravity=true;
        }
        Lighting.AddLight(Projectile.Center,.35f,.22f,.4f);
    }
    public override bool PreDraw(Player player,ref Color lightColor)
    {
        Texture2D t=TextureAssets.Projectile[Type].Value;
        Vector2 origin=t.Size()/2;
        for(int i=Projectile.oldPos.Length-1;i>=0;i--)
        {
            if(Projectile.oldPos[i]==Vector2.Zero) continue;
            float fade=(1f-i/(float)Projectile.oldPos.Length)*.32f;
            Main.EntitySpriteDraw(t,Projectile.oldPos[i]+Projectile.Size/2-Main.screenPosition,null,new Color(180,150,255)*fade,Projectile.rotation,origin,.52f,SpriteEffects.None);
        }
        Main.EntitySpriteDraw(t,Projectile.Center-Main.screenPosition,null,Color.White,Projectile.rotation,origin,.6f,SpriteEffects.None);
        return false;
    }
}
public class EchoSlash : PrismMothBolt
{
    public override string Texture=>"NyuuTerraria/Content/Reverie/EchoScythe";
    public override void SetDefaults() {base.SetDefaults();Projectile.DamageType=DamageClass.Melee;Projectile.timeLeft=45;Projectile.penetrate=4;}
    public override void AI() {Projectile.rotation+=.3f;Lighting.AddLight(Projectile.Center,.3f,.1f,.4f);}
}
public class ArchiveLantern : ModItem
{
    public override void SetDefaults() {Item.width=22;Item.height=28;Item.accessory=true;Item.rare=ItemRarityID.Green;}
    public override void UpdateAccessory(Player player,bool hideVisual) {player.nightVision=true;Lighting.AddLight(player.Center,.45f,.7f,.75f);}
    public override void AddRecipes()=>CreateRecipe().AddIngredient<EchoFragment>(4).AddIngredient(ItemID.Torch,5).AddRecipeGroup("IronBar",3).AddTile(TileID.Anvils).Register();
}
public class PrismMemorial : ModItem
{
    public override void SetDefaults()=>Item.DefaultToPlaceableTile(ModContent.TileType<PrismMemorialTile>());
    public override void AddRecipes()=>CreateRecipe().AddIngredient(ItemID.StoneBlock,20).AddIngredient<MoonprismBar>(2).AddIngredient(ItemID.Daybloom,2).AddTile(TileID.WorkBenches).Register();
}
public class PrismMemorialTile : ModTile
{
    public override void SetStaticDefaults()
    {
        Main.tileFrameImportant[Type]=true;Main.tileSign[Type]=true;
        TileObjectData.newTile.CopyFrom(TileObjectData.Style2xX);TileObjectData.newTile.Height=3;
        TileObjectData.newTile.CoordinateHeights=new[]{16,16,16};TileObjectData.newTile.Origin=new Point16(0,2);TileObjectData.addTile(Type);
        AddMapEntry(new Color(185,159,206),CreateMapEntryName());
    }
    public override bool RightClick(int i,int j)
    {
        if(Main.netMode==NetmodeID.MultiplayerClient)
        {NetMessage.SendData(MessageID.OpenSignRequest,number:Main.myPlayer,number2:i,number3:j);return true;}
        int sign=Sign.ReadSign(i,j,true);
        if(sign>=0) {Main.LocalPlayer.sign=sign;Main.npcChatText=Main.sign[sign].text;}
        return true;
    }
    public override void KillMultiTile(int i,int j,int frameX,int frameY)=>Sign.KillSign(i,j);
}
