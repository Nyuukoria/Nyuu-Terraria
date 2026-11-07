using System.Linq;
using System;
using Microsoft.Xna.Framework;
using Terraria;
using Terraria.ID;
using Terraria.ModLoader;
using Terraria.GameContent.ItemDropRules;
using NyuuTerraria.Content.Reverie;
using NyuuTerraria.Content.Spellcraft;

namespace NyuuTerraria.Content.Frontiers;

public abstract class FrontierEnemy : ModNPC
{
    protected virtual int Area=>0;protected virtual int Health=>200;protected virtual int Attack=>24;
    public override void SetDefaults(){NPC.width=38;NPC.height=38;NPC.lifeMax=Health;NPC.damage=Attack;NPC.defense=10;NPC.aiStyle=-1;NPC.noGravity=true;NPC.knockBackResist=.4f;NPC.value=Item.buyPrice(silver:8);NPC.HitSound=SoundID.NPCHit5;NPC.DeathSound=SoundID.NPCDeath6;}
    public override float SpawnChance(NPC.Spawner info)=>FrontierWorld.AreaAt(info.Player.Center)==Area?.12f:0;
    public override void AI()
    {
        NPC.TargetClosest();Player p=Main.player[NPC.target];NPC.ai[0]++;
        if(!p.active||p.dead){NPC.velocity.Y-=.1f;NPC.EncourageDespawn(90);return;}
        Vector2 goal=p.Center+new Vector2(0,MathF.Sin(NPC.ai[0]*.03f)*45);
        NPC.velocity=Vector2.Lerp(NPC.velocity,(goal-NPC.Center).SafeNormalize(Vector2.UnitX)*3,.035f);NPC.spriteDirection=NPC.velocity.X>=0?1:-1;
        if(NPC.collideX)NPC.velocity.Y=-3;
    }
    public override void ModifyNPCLoot(NPCLoot loot)
    {int[] drops={ModContent.ItemType<TidePearl>(),ModContent.ItemType<PaleBloom>(),ModContent.ItemType<CinderBloom>(),ModContent.ItemType<StarglassShard>()};loot.Add(ItemDropRule.Common(drops[Area],2,1,3));}
}
public class ReefSkitterer : FrontierEnemy {public override string Texture=>"NyuuTerraria/Content/Frontiers/ReefAmbusher";}
public class VineboundArmor : FrontierEnemy {protected override int Area=>1;protected override int Health=>360;}
public class AshBurrower : FrontierEnemy {protected override int Area=>2;protected override int Health=>480;protected override int Attack=>38;}
public class StarfallenConstruct : FrontierEnemy {protected override int Area=>3;protected override int Health=>2200;protected override int Attack=>70;public override float SpawnChance(NPC.Spawner i)=>base.SpawnChance(i)*(ModContent.GetInstance<FrontierWorld>().EventTicks>0?2:1);}
public class ReefAmbusher : FrontierEnemy
{
    protected override int Health=>3600;
    public override float SpawnChance(NPC.Spawner i)=>FrontierWorld.AreaAt(i.Player.Center)==0&&!NPC.AnyNPCs(Type)?.012f:0;
    public override void AI(){base.AI();if((int)NPC.ai[0]%150==115){NPC.velocity=(Main.player[NPC.target].Center-NPC.Center).SafeNormalize(Vector2.UnitX)*11;NPC.netUpdate=true;}}
    public override void ModifyNPCLoot(NPCLoot l){l.Add(ItemDropRule.Common(ModContent.ItemType<TidePearl>(),1,8,14));l.Add(ItemDropRule.Common(ModContent.ItemType<RitualFocus>(),1,2,4));}
}
public class ArchiveCaretaker : FrontierEnemy
{
    protected override int Health=>8000;protected override int Attack=>42;
    public override float SpawnChance(NPC.Spawner i)=>RealmWorld.RealmAt(i.Player.Center)==1&&Main.hardMode&&!NPC.AnyNPCs(Type)?.01f:0;
    public override void AI(){base.AI();if(NPC.ai[0]%150==90&&Main.netMode!=NetmodeID.MultiplayerClient)for(int n=-1;n<=1;n++)Projectile.NewProjectile(NPC.GetSource_FromAI(),NPC.Center,(Main.player[NPC.target].Center-NPC.Center).SafeNormalize(Vector2.UnitX).RotatedBy(n*.3f)*6,ModContent.ProjectileType<FrontierHazard>(),20,0,Main.myPlayer,1);}
    public override void ModifyNPCLoot(NPCLoot l)=>l.Add(ItemDropRule.Common(ModContent.ItemType<ArchiveRelic>(),1,1,2));
}
public abstract class FrontierBoss : ModNPC
{
    protected abstract bool Hush{get;}protected virtual int Health=>18000;
    public override void SetStaticDefaults()=>NPCID.Sets.MPAllowedEnemies[Type]=true;
    public override void SetDefaults(){NPC.width=100;NPC.height=110;NPC.lifeMax=Health;NPC.damage=Hush?68:42;NPC.defense=Hush?30:20;NPC.boss=true;NPC.noGravity=true;NPC.noTileCollide=true;NPC.aiStyle=-1;NPC.knockBackResist=0;NPC.value=Item.buyPrice(gold:10);NPC.HitSound=SoundID.NPCHit5;NPC.DeathSound=SoundID.NPCDeath6;if(!Main.dedServ)Music=Hush?MusicID.Boss2:MusicID.Boss1;}
    public override void ApplyDifficultyAndPlayerScaling(int numPlayers,float balance,float bossAdjustment){NPC.lifeMax=(int)(NPC.lifeMax*balance*bossAdjustment);}
    public override void AI()
    {
        if(NPC.localAI[0]++==0&&NPC.ai[3]==1){NPC.lifeMax=(int)(NPC.lifeMax*1.65f);NPC.life=NPC.lifeMax;NPC.damage=(int)(NPC.damage*1.2f);NPC.netUpdate=true;}
        NPC.TargetClosest();Player p=Main.player[NPC.target];
        bool region=Hush?RealmWorld.RealmAt(p.Center)==1:FrontierWorld.AreaAt(p.Center)==1;
        if(!p.active||p.dead||!region){NPC.velocity.Y-=.12f;NPC.EncourageDespawn(60);return;}
        int tick=(int)++NPC.ai[0],cycle=tick%300;bool phase=NPC.life<NPC.lifeMax*.5f;
        if(cycle<160)
        {
            float side=Hush?MathF.Sin(tick*.025f)*240:MathF.Sin(tick*.018f)*160;
            Vector2 goal=p.Center+new Vector2(side,-190);NPC.velocity=Vector2.Lerp(NPC.velocity,(goal-NPC.Center)*.03f,.07f);
            if(cycle%50==20)Fire(p,phase?9:6,Hush?6:4);
        }
        else if(cycle<210){NPC.velocity*=.87f;Lighting.AddLight(NPC.Center,.65f,.35f,.6f);}
        else if(cycle==210){NPC.velocity=(p.Center-NPC.Center).SafeNormalize(Vector2.UnitY)*(phase?18:13);NPC.netUpdate=true;}
        else {NPC.velocity*=.985f;if(cycle==250&&phase)Fire(p,12,5);}
        NPC.rotation=NPC.velocity.X*.008f;
    }
    private void Fire(Player p,int count,float speed)
    {
        if(Main.netMode==NetmodeID.MultiplayerClient)return;
        for(int i=0;i<count;i++)
        {
            Vector2 velocity=Hush?(p.Center-NPC.Center).SafeNormalize(Vector2.UnitY).RotatedBy((i-(count-1)*.5f)*.12f)*speed:Vector2.UnitX.RotatedBy(MathHelper.TwoPi*i/count)*speed;
            Projectile.NewProjectile(NPC.GetSource_FromAI(),NPC.Center,velocity,ModContent.ProjectileType<FrontierHazard>(),NPC.damage/3,0,Main.myPlayer,Hush?1:0);
        }
    }
    public override void OnKill(){var w=ModContent.GetInstance<FrontierWorld>();if(Hush)w.UnwrittenDown=true;else w.GardenerDown=true;w.Sync();if(NPC.ai[3]==1)Item.NewItem(NPC.GetSource_Loot(),NPC.Hitbox,ModContent.ItemType<ChallengeLaurel>());}
    public override void ModifyNPCLoot(NPCLoot l)=>l.Add(ItemDropRule.Common(Hush?ModContent.ItemType<ArchiveRelic>():ModContent.ItemType<GardenerRelic>(),1,3,5));
}
[AutoloadBossHead] public class PaleGardener : FrontierBoss {protected override bool Hush=>false;}
[AutoloadBossHead] public class TheUnwritten : FrontierBoss {protected override bool Hush=>true;protected override int Health=>48000;}
public class FrontierHazard : ModProjectile
{
    public override string Texture=>"NyuuTerraria/Content/Frontiers/PaleBloom";
    public override void SetDefaults(){Projectile.width=Projectile.height=14;Projectile.hostile=true;Projectile.tileCollide=false;Projectile.timeLeft=210;}
    public override void AI(){Projectile.rotation+=.06f;Lighting.AddLight(Projectile.Center,.3f,.15f,.25f);}
    public override bool PreDraw(Player p,ref Color c){SpellVisuals.Rune(Projectile.Center,9,Projectile.ai[0]==0?Color.LightPink:Color.MediumPurple,Projectile.rotation,.9f);return false;}
}
public abstract class FrontierSummon : ModItem
{
    protected abstract bool Hush{get;}
    public override void SetDefaults(){Item.width=30;Item.height=32;Item.useStyle=ItemUseStyleID.HoldUp;Item.useTime=Item.useAnimation=45;Item.rare=ItemRarityID.Orange;}
    public override bool CanUseItem(Player p)=>!Main.npc.Any(n=>n.active&&n.boss)&&(Hush?NPC.downedPlantBoss&&RealmWorld.RealmAt(p.Center)==1:FrontierWorld.AreaAt(p.Center)==1);
    public override bool? UseItem(Player p){if(p.whoAmI==Main.myPlayer){int type=Hush?ModContent.NPCType<TheUnwritten>():ModContent.NPCType<PaleGardener>();if(Main.netMode==NetmodeID.MultiplayerClient)NetMessage.SendData(MessageID.SpawnBossUseLicenseStartEvent,number:p.whoAmI,number2:type);else NPC.SpawnOnPlayer(p.whoAmI,type);}return true;}
}
public class GardenerInvitation : FrontierSummon {public override string Texture=>"NyuuTerraria/Content/Frontiers/PaleBloom";protected override bool Hush=>false;public override void AddRecipes()=>CreateRecipe().AddIngredient<PaleBloom>(8).AddIngredient(ItemID.Daybloom,5).AddTile(TileID.Anvils).Register();}
public class UnwrittenPage : FrontierSummon {protected override bool Hush=>true;public override void AddRecipes()=>CreateRecipe().AddIngredient<ArchiveRelic>(2).AddIngredient<EchoFragment>(10).AddTile(TileID.Anvils).Register();}
public class RematchSigil : ModItem
{
    public override void SetDefaults(){Item.width=30;Item.height=32;Item.useStyle=ItemUseStyleID.HoldUp;Item.useTime=Item.useAnimation=45;Item.rare=ItemRarityID.Pink;}
    internal static bool CanSummon(Player p){var w=ModContent.GetInstance<FrontierWorld>();return !Main.npc.Any(n=>n.active&&n.boss)&&(FrontierWorld.AreaAt(p.Center)==1&&w.GardenerDown||RealmWorld.RealmAt(p.Center)==1&&w.UnwrittenDown);}
    public override bool CanUseItem(Player p)=>CanSummon(p);
    internal static void Summon(Player p)
    {
        if(Main.netMode==NetmodeID.MultiplayerClient||!CanSummon(p))return;
        int type=FrontierWorld.AreaAt(p.Center)==1?ModContent.NPCType<PaleGardener>():ModContent.NPCType<TheUnwritten>();
        int id=NPC.NewNPC(p.GetSource_ItemUse(p.HeldItem),(int)p.Center.X,(int)p.Center.Y-220,type,ai3:1);Main.npc[id].netUpdate=true;
    }
    public override bool? UseItem(Player p){if(p.whoAmI==Main.myPlayer){if(Main.netMode==NetmodeID.MultiplayerClient){var packet=Mod.GetPacket();packet.Write((byte)7);packet.Send();}else Summon(p);}return true;}
    public override void AddRecipes()=>CreateRecipe().AddIngredient<GardenerRelic>(3).AddIngredient<ArchiveRelic>(2).AddTile(TileID.Anvils).Register();
}
public class MasteryCrest : ModItem
{
    public override string Texture=>"NyuuTerraria/Content/Frontiers/ExplorerMedallion";
    public override void SetDefaults(){Item.width=28;Item.height=30;Item.accessory=true;Item.rare=ItemRarityID.Pink;}
    public override void UpdateAccessory(Player p,bool hideVisual){int level=p.GetModPlayer<ChroniclePlayer>().Level(p.GetModPlayer<ChroniclePlayer>().Profession);p.GetDamage(DamageClass.Generic)+=Math.Min(.06f,level*.002f);}
}
public class ChallengeLaurel : ModItem
{
    public override string Texture=>"NyuuTerraria/Content/Frontiers/RematchSigil";
    public override void SetDefaults(){Item.width=28;Item.height=30;Item.accessory=true;Item.vanity=true;Item.rare=ItemRarityID.Pink;}
    public override void UpdateVanity(Player p){if(Main.dedServ||!Main.rand.NextBool(8))return;var d=Dust.NewDustPerfect(p.Center+new Vector2(Main.rand.Next(-12,13),-22),DustID.RainbowMk2,new Vector2(0,-.4f),80,Color.LightPink,.7f);d.noGravity=true;}
}

