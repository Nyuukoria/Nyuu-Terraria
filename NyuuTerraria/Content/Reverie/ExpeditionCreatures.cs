using System;
using System.Collections.Generic;
using Microsoft.Xna.Framework;
using Terraria;
using Terraria.ID;
using Terraria.ModLoader;
using Terraria.GameContent.Bestiary;
using Terraria.GameContent.ItemDropRules;
using NyuuTerraria.Content.AmberGrotto;

namespace NyuuTerraria.Content.Reverie;

public abstract class ExpeditionEnemy : ModNPC
{
    protected virtual int Realm=>1;
    protected virtual int Health=>280;
    protected virtual int Damage=>30;
    protected virtual float Speed=>3.5f;
    public override void SetDefaults()
    {
        NPC.width=34;NPC.height=40;NPC.lifeMax=Health;NPC.damage=Damage;NPC.defense=10;
        NPC.noGravity=true;NPC.noTileCollide=false;NPC.aiStyle=-1;NPC.knockBackResist=.35f;
        NPC.HitSound=SoundID.NPCHit5;NPC.DeathSound=SoundID.NPCDeath6;NPC.value=Item.buyPrice(silver:4);
    }
    public override void AI()
    {
        NPC.TargetClosest();var player=Main.player[NPC.target];
        Vector2 aim=(player.Center-NPC.Center).SafeNormalize(Vector2.Zero);
        NPC.velocity=Vector2.Lerp(NPC.velocity,aim*Speed,.035f);
        NPC.spriteDirection=NPC.direction;NPC.rotation=NPC.velocity.X*.035f;
    }
    public override float SpawnChance(NPC.Spawner info)=>RealmWorld.RealmAt(info.Player.Center)==Realm ? .22f : 0;
    public override void ModifyNPCLoot(NPCLoot loot)=>loot.Add(ItemDropRule.Common(ModContent.ItemType<EchoFragment>(),1,1,3));
    public override void SetBestiary(BestiaryDatabase database,BestiaryEntry entry)=>entry.Info.Add(new FlavorTextBestiaryInfoElement("A strange inhabitant of the portal expeditions."));
}
public class GlassSerpent : ExpeditionEnemy
{
    protected override int Realm=>0;
    protected override int Health=>180;
    public override void ModifyNPCLoot(NPCLoot loot)=>loot.Add(ItemDropRule.Common(ModContent.ItemType<MoonprismOre>(),2,1,3));
}
public class PaperWraith : ExpeditionEnemy
{
    public override void AI()
    {
        base.AI();NPC.ai[0]++;
        if(NPC.ai[0]%150==0 && Main.netMode!=NetmodeID.MultiplayerClient)
            Projectile.NewProjectile(NPC.GetSource_FromAI(),NPC.Center,(Main.player[NPC.target].Center-NPC.Center).SafeNormalize(Vector2.UnitY)*5,
                ModContent.ProjectileType<EchoWave>(),16,0,Main.myPlayer);
    }
}
public class LanternStalker : ExpeditionEnemy
{
    protected override int Health=>4200;
    protected override int Damage=>42;
    public override float SpawnChance(NPC.Spawner info)=>RealmWorld.RealmAt(info.Player.Center)==1 && !NPC.AnyNPCs(Type) ? .025f : 0;
    public override void AI()
    {
        base.AI();NPC.ai[1]++;
        bool charge=NPC.ai[1]%180>130;
        NPC.Opacity=charge ? 1 : .38f;
        if(charge) NPC.velocity*=1.035f;
    }
    public override void ModifyNPCLoot(NPCLoot loot) {base.ModifyNPCLoot(loot);loot.Add(ItemDropRule.Common(ModContent.ItemType<ArchiveLantern>()));}
}
public class TheMirrorless : ExpeditionEnemy
{
    protected override int Health=>6500;
    protected override int Damage=>45;
    public override float SpawnChance(NPC.Spawner info)=>RealmWorld.RealmAt(info.Player.Center)==1 && Main.hardMode && !NPC.AnyNPCs(Type) ? .015f : 0;
    public override void AI()
    {
        NPC.TargetClosest();var p=Main.player[NPC.target];
        NPC.ai[0]++;
        Vector2 target=p.Center+new Vector2(MathF.Sin(NPC.ai[0]*.025f)*170,-90);
        NPC.velocity=Vector2.Lerp(NPC.velocity,(target-NPC.Center)*.035f,.06f);
        NPC.velocity=Vector2.Clamp(NPC.velocity,new Vector2(-10),new Vector2(10));
        if(NPC.ai[0]%110==0 && Main.netMode!=NetmodeID.MultiplayerClient)
            Projectile.NewProjectile(NPC.GetSource_FromAI(),NPC.Center,(p.Center-NPC.Center).SafeNormalize(Vector2.UnitY)*7,ModContent.ProjectileType<EchoWave>(),22,0,Main.myPlayer);
    }
    public override void ModifyNPCLoot(NPCLoot loot) {base.ModifyNPCLoot(loot);loot.Add(ItemDropRule.Common(ModContent.ItemType<EchoScythe>(),3));}
}
public abstract class ExpeditionBoss : ExpeditionEnemy
{
    protected abstract int BossHealth { get; }
    protected override int Health=>BossHealth;
    protected override int Damage=>42;
    public override void SetStaticDefaults()=>NPCID.Sets.MPAllowedEnemies[Type]=true;
    public override void SetDefaults()
    {
        base.SetDefaults();NPC.width=90;NPC.height=90;NPC.boss=true;NPC.noTileCollide=true;
        NPC.defense=18;NPC.knockBackResist=0;NPC.value=Item.buyPrice(gold:5);
        if(!Main.dedServ) Music=MusicID.Boss2;
    }
    public override float SpawnChance(NPC.Spawner info)=>0;
    public override void ApplyDifficultyAndPlayerScaling(int numPlayers,float balance,float bossAdjustment)
    {
        NPC.lifeMax=(int)(NPC.lifeMax*balance*bossAdjustment);NPC.damage=(int)(NPC.damage*1.15f);
    }
    protected void FireRing(int count,float speed,float offset=0)
    {
        if(Main.netMode==NetmodeID.MultiplayerClient) return;
        for(int i=0;i<count;i++) Projectile.NewProjectile(NPC.GetSource_FromAI(),NPC.Center,
            Vector2.UnitX.RotatedBy(MathHelper.TwoPi*i/count+offset)*speed,ModContent.ProjectileType<EchoWave>(),NPC.damage/3,0,Main.myPlayer);
    }
    public override void AI()
    {
        NPC.TargetClosest();var p=Main.player[NPC.target];
        if(!p.active || p.dead || RealmWorld.RealmAt(p.Center)!=Realm) {NPC.velocity.Y-=.12f;NPC.EncourageDespawn(60);return;}
        int tick=(int)++NPC.ai[0];int cycle=tick%240;
        bool angry=NPC.life<NPC.lifeMax*.5f;
        if(cycle<145)
        {
            Vector2 goal=p.Center+new Vector2(MathF.Sin(tick*.02f)*230,-160);
            NPC.velocity=Vector2.Lerp(NPC.velocity,(goal-NPC.Center)*.035f,.07f);
            if(cycle%55==0) FireRing(angry ? 12 : 8,angry ? 5.5f : 4f,tick*.03f);
        }
        else if(cycle<185)
        {
            NPC.velocity*=.88f;
            for(int i=0;i<2;i++) Dust.NewDust(NPC.position,NPC.width,NPC.height,DustID.PinkCrystalShard);
        }
        else if(cycle==185)
        {
            NPC.velocity=(p.Center-NPC.Center).SafeNormalize(Vector2.UnitY)*(angry ? 17 : 13);NPC.netUpdate=true;
        }
        else NPC.velocity*=.985f;
        Lighting.AddLight(NPC.Center,.4f,.15f,.4f);
    }
    public override void ModifyNPCLoot(NPCLoot loot)
    {
        loot.Add(ItemDropRule.Common(ModContent.ItemType<EchoFragment>(),1,12,18));
        loot.Add(ItemDropRule.Common(ModContent.ItemType<MoonprismOre>(),1,18,30));
    }
}
[AutoloadBossHead]
public class RootboundKnight : ExpeditionBoss
{
    protected override int Realm=>0;
    protected override int BossHealth=>10000;
    public override void AI()
    {
        base.AI();int phase=(int)NPC.ai[0]%240;
        NPC.defense=phase<145 ? 32 : 8;
        NPC.rotation=NPC.velocity.X*.018f;
    }
    public override void ModifyNPCLoot(NPCLoot loot) {base.ModifyNPCLoot(loot);loot.Add(ItemDropRule.Common(ModContent.ItemType<PrismWingScale>(),1,5,8));}
}
[AutoloadBossHead]
public class Bellkeeper : ExpeditionBoss
{
    protected override int BossHealth=>19000;
    public override void AI()
    {
        base.AI();NPC.rotation=MathF.Sin(NPC.ai[0]*.065f)*.12f;
        if(NPC.ai[0]%180==100) FireRing(16,4.8f,MathHelper.Pi/16);
    }
    public override void ModifyNPCLoot(NPCLoot loot) {base.ModifyNPCLoot(loot);loot.Add(ItemDropRule.Common(ModContent.ItemType<EchoScythe>()));}
}
[AutoloadBossHead]
public class PrismMatriarch : ExpeditionBoss
{
    protected override int Realm=>0;
    protected override int BossHealth=>24000;
    public override void AI()
    {
        base.AI();NPC.scale=1f+MathF.Sin(NPC.ai[0]*.1f)*.04f;
        if(NPC.ai[0]%90==50 && Main.netMode!=NetmodeID.MultiplayerClient)
        {
            var p=Main.player[NPC.target];Vector2 aim=(p.Center-NPC.Center).SafeNormalize(Vector2.UnitY);
            for(int i=-2;i<=2;i++) Projectile.NewProjectile(NPC.GetSource_FromAI(),NPC.Center,aim.RotatedBy(i*.17f)*7,ModContent.ProjectileType<EchoWave>(),20,0,Main.myPlayer);
        }
    }
    public override void ModifyNPCLoot(NPCLoot loot)
    {
        loot.Add(ItemDropRule.Common(ModContent.ItemType<PrismWingScale>(),1,12,18));
        loot.Add(ItemDropRule.Common(ModContent.ItemType<PrideMothCharm>()));loot.Add(ItemDropRule.Common(ModContent.ItemType<PrismMothStaff>()));
    }
}
public class ReefbackGuardian : ExpeditionBoss
{
    protected override int Realm=>0;
    protected override int BossHealth=>3200;
    public override void SetDefaults() {base.SetDefaults();NPC.boss=false;NPC.value=Item.buyPrice(gold:1);}
    public override void ModifyNPCLoot(NPCLoot loot)
    {
        loot.Add(ItemDropRule.Common(ModContent.ItemType<MoonprismOre>(),1,10,18));loot.Add(ItemDropRule.Common(ItemID.Coral,1,8,16));
    }
}
public class EchoWave : ModProjectile
{
    public override string Texture=>"NyuuTerraria/Content/Reverie/ArchiveLantern";
    public override void SetDefaults() {Projectile.width=12;Projectile.height=12;Projectile.hostile=true;Projectile.tileCollide=false;Projectile.timeLeft=210;Projectile.scale=.45f;}
    public override void AI()
    {
        Projectile.rotation+=.045f;Lighting.AddLight(Projectile.Center,.3f,.2f,.5f);
        if(Projectile.timeLeft%3==0) {var d=Dust.NewDustPerfect(Projectile.Center,DustID.PurpleTorch,Vector2.Zero,100,default,.75f);d.noGravity=true;}
    }
}
public abstract class ExpeditionSummon : ModItem
{
    protected abstract int Boss { get; }
    protected virtual int Realm=>0;
    protected virtual bool Hardmode=>false;
    public override void SetDefaults() {Item.width=28;Item.height=30;Item.maxStack=99;Item.consumable=true;Item.useStyle=ItemUseStyleID.HoldUp;Item.useTime=Item.useAnimation=30;Item.rare=ItemRarityID.Green;}
    public override bool CanUseItem(Player player)=>RealmWorld.RealmAt(player.Center)==Realm && (!Hardmode || Main.hardMode) && !NPC.AnyNPCs(Boss);
    public override bool? UseItem(Player player)
    {
        if(player.whoAmI==Main.myPlayer)
        {
            if(Main.netMode==NetmodeID.MultiplayerClient) NetMessage.SendData(MessageID.SpawnBossUseLicenseStartEvent,number:player.whoAmI,number2:Boss);
            else NPC.SpawnOnPlayer(player.whoAmI,Boss);
        }
        return true;
    }
}
public class ReefShell : ExpeditionSummon
{
    protected override int Boss=>ModContent.NPCType<ReefbackGuardian>();
    public override void AddRecipes()=>CreateRecipe().AddIngredient(ItemID.Coral,10).AddIngredient(ItemID.Seashell,3).AddTile(TileID.WorkBenches).Register();
}
public class RootboundSeal : ExpeditionSummon
{
    public override string Texture=>"NyuuTerraria/Content/Reverie/RootboundSeal";
    protected override int Boss=>ModContent.NPCType<RootboundKnight>();
    public override void AddRecipes()=>CreateRecipe().AddIngredient<MoonprismBar>(5).AddIngredient(ItemID.Vine,3).AddTile(TileID.Anvils).Register();
}
public class CrackedBellSigil : ExpeditionSummon
{
    protected override int Boss=>ModContent.NPCType<Bellkeeper>();protected override int Realm=>1;protected override bool Hardmode=>true;
    public override void AddRecipes()=>CreateRecipe().AddIngredient<EchoFragment>(8).AddIngredient(ItemID.Obsidian,10).AddTile(TileID.Anvils).Register();
}
public class PrismCocoon : ExpeditionSummon
{
    public override string Texture=>"NyuuTerraria/Content/Reverie/PrismCocoon";
    protected override int Boss=>ModContent.NPCType<PrismMatriarch>();protected override bool Hardmode=>true;
    public override void AddRecipes()=>CreateRecipe().AddIngredient<PrismWingScale>(6).AddIngredient(ItemID.SoulofLight,5).AddTile(TileID.Anvils).Register();
}

public abstract class PeacefulExpeditionCreature : ModNPC
{
    public override void SetStaticDefaults()=>NPCID.Sets.CountsAsCritter[Type]=true;
    public override void SetDefaults()
    {
        NPC.CloneDefaults(NPCID.Bunny);NPC.width=30;NPC.height=26;NPC.lifeMax=20;NPC.damage=0;
        NPC.friendly=true;NPC.aiStyle=-1;
    }
    public override float SpawnChance(NPC.Spawner info)=>RealmWorld.RealmAt(info.Player.Center)==0 ? .1f : 0;
    public override void AI()
    {
        NPC.ai[0]++;
        if(NPC.ai[0]%180==0 && Main.netMode!=NetmodeID.MultiplayerClient) {NPC.direction=Main.rand.NextBool() ? 1 : -1;NPC.netUpdate=true;}
        NPC.velocity.X=NPC.direction*(MathF.Sin(NPC.ai[0]*.025f)>.1f ? .7f : 0);NPC.spriteDirection=NPC.direction;
        if(NPC.collideX && NPC.velocity.Y==0) NPC.velocity.Y=-3;
    }
    public override void FindFrame(int frameHeight)=>NPC.frame.Y=0;
    public override void SetBestiary(BestiaryDatabase database,BestiaryEntry entry)=>entry.Info.Add(new FlavorTextBestiaryInfoElement("A gentle creature of the Reverie. Prefers quiet gardens to battles."));
}
public class DewFox : PeacefulExpeditionCreature { }
public class GlassDeer : PeacefulExpeditionCreature {public override void SetDefaults() {base.SetDefaults();NPC.width=34;NPC.height=40;}}
public class Mothcat : PeacefulExpeditionCreature { }
public class MooncapSprite : PeacefulExpeditionCreature { }

public class DewBerryCharm : GrottoPetItem
{
    protected override int Pet=>ModContent.ProjectileType<DewFoxPet>();protected override int Buff=>ModContent.BuffType<DewFoxCompanion>();
    public override void AddRecipes()=>CreateRecipe().AddIngredient<MoonprismBar>(3).AddIngredient(ItemID.Daybloom,3).AddTile(TileID.WorkBenches).Register();
}
public class PrideMothCharm : GrottoPetItem
{
    protected override int Pet=>ModContent.ProjectileType<PrideMothPet>();protected override int Buff=>ModContent.BuffType<PrideMothCompanion>();
    public override void AddRecipes()=>CreateRecipe().AddIngredient(ItemID.Silk,8).AddIngredient<MoonprismBar>(4).AddTile(TileID.Loom).Register();
}
public class DewFoxCompanion : GrottoPetBuff
{
    protected override int Pet=>ModContent.ProjectileType<DewFoxPet>();protected override string PetTexture=>"NyuuTerraria/Content/Reverie/DewBerryCharm";
}
public class PrideMothCompanion : GrottoPetBuff
{
    protected override int Pet=>ModContent.ProjectileType<PrideMothPet>();protected override string PetTexture=>"NyuuTerraria/Content/Reverie/PrideMothCharm";
}
public class DewFoxPet : GrottoPet
{
    public override string Texture=>"NyuuTerraria/Content/Reverie/DewFox";
    protected override int PetBuff=>ModContent.BuffType<DewFoxCompanion>();protected override Vector3 Glow=>new(.15f,.12f,.22f);protected override float FollowHeight=>8;
}
public class PrideMothPet : GrottoPet
{
    public override string Texture=>"NyuuTerraria/Content/Reverie/PrideMothPet";
    protected override int PetBuff=>ModContent.BuffType<PrideMothCompanion>();protected override Vector3 Glow=>new(.25f,.3f,.4f);protected override float FollowHeight=>-50;
}

public class ExpeditionKills : GlobalNPC
{
    public override void OnKill(NPC npc)
    {
        if(Main.netMode==NetmodeID.MultiplayerClient || npc.friendly || npc.lifeMax<10 || npc.SpawnedFromStatue) return;
        string key="defeat-"+(npc.ModNPC?.Name ?? npc.type.ToString());
        int xp=Math.Clamp(npc.lifeMax/15,3,npc.boss ? 500 : 40);
        foreach(Player p in Main.player)
        {
            if(!p.active || p.dead || !npc.playerInteraction[p.whoAmI] || p.Distance(npc.Center)>4000) continue;
            if(Main.netMode==NetmodeID.Server)
            {
                var packet=Mod.GetPacket();packet.Write((byte)1);packet.Write(key);packet.Write(xp);packet.Send(p.whoAmI);
            }
            else
            {
                var data=p.GetModPlayer<ChroniclePlayer>();data.Record("defeat-enemies");data.Record(key);data.AddExperience(xp);
            }
        }
    }
    public override void EditSpawnPool(IDictionary<int,float> pool,NPC.Spawner info)
    {
        int realm=RealmWorld.RealmAt(info.Player.Center);
        if(realm<0) return;
        var remove=new List<int>();
        foreach(int type in pool.Keys) if(type==0 || type<NPCID.Count) remove.Add(type);
        foreach(int type in remove) pool.Remove(type);
    }
}
