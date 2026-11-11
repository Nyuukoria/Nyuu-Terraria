using System;
using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Graphics;
using Terraria;
using Terraria.DataStructures;
using Terraria.GameContent;
using Terraria.ID;
using Terraria.ModLoader;

namespace NyuuTerraria.Content.Spellcraft;

internal static class SpellVisuals
{
    internal static Color Glow(Color color,float strength=1)=>new Color((int)(color.R*strength),(int)(color.G*strength),(int)(color.B*strength),0);
    internal static void Line(Vector2 from,Vector2 to,Color color,float width)
    {
        Vector2 d=to-from;if(d.LengthSquared()<.01f) return;
        Main.EntitySpriteDraw(TextureAssets.MagicPixel.Value,from-Main.screenPosition,new Rectangle(0,0,1,1),color,d.ToRotation(),new Vector2(0,.5f),new Vector2(d.Length(),width),SpriteEffects.None);
    }
    internal static void Circle(Vector2 center,float radius,Color color,float rotation=0,int sides=48,float width=1.5f)
    {
        for(int i=0;i<sides;i++)
            Line(center+Vector2.UnitX.RotatedBy(rotation+MathHelper.TwoPi*i/sides)*radius,
                center+Vector2.UnitX.RotatedBy(rotation+MathHelper.TwoPi*(i+1)/sides)*radius,color,width);
    }
    internal static void Rune(Vector2 center,float radius,Color color,float rotation,float opacity)
    {
        Color light=Glow(color,opacity*.8f);
        Circle(center,radius,light,rotation);Circle(center,radius*.72f,light,rotation,32,1);
        for(int i=0;i<6;i++)
        {
            float a=rotation+MathHelper.TwoPi*i/6;
            Vector2 tip=center+Vector2.UnitX.RotatedBy(a)*radius;
            Vector2 other=center+Vector2.UnitX.RotatedBy(a+MathHelper.TwoPi/3)*radius;
            Line(tip,other,Glow(color,opacity*.36f),1);
            Line(tip,center+Vector2.UnitX.RotatedBy(a)*(radius+5),light,2);
        }
    }
    internal static void CastSigil(IEntitySource source,Vector2 position,int spell,int owner)
    {
        if(Main.myPlayer==owner) Projectile.NewProjectile(source,position,Vector2.Zero,ModContent.ProjectileType<CastingSigil>(),0,0,owner,spell);
    }
    internal static void Spark(Vector2 center,Color color,int count=8,float speed=2)
    {
        if(Main.dedServ) return;
        for(int i=0;i<count;i++)
        {
            Dust d=Dust.NewDustPerfect(center,DustID.RainbowMk2,Vector2.UnitX.RotatedBy(MathHelper.TwoPi*i/count)*speed,60,color,.75f);
            d.noGravity=true;
        }
    }
}

public class CastingSigil : ModProjectile
{
    public override string Texture=>"NyuuTerraria/Content/Spellcraft/ArcaneMote";
    public override void SetDefaults() {Projectile.width=2;Projectile.height=2;Projectile.tileCollide=false;Projectile.timeLeft=24;}
    public override bool? CanDamage()=>false;
    public override bool ShouldUpdatePosition()=>false;
    public override bool PreDraw(Player player,ref Color lightColor)
    {
        int spell=Math.Clamp((int)Projectile.ai[0],0,Spells.Count-1);float life=Projectile.timeLeft/24f;
        SpellVisuals.Rune(Projectile.Center,18+(1-life)*18,Spells.Colors[spell],Projectile.timeLeft*.04f,life);
        return false;
    }
}

public class SpellShot : ModProjectile
{
    public override string Texture=>"NyuuTerraria/Content/Spellcraft/ArcaneMote";
    internal int Spell=>Math.Clamp((int)Projectile.ai[0],0,Spells.Storm);
    internal int Mastery=>Math.Clamp((int)Projectile.ai[1],0,10);
    public override void SetStaticDefaults() {ProjectileID.Sets.TrailCacheLength[Type]=18;ProjectileID.Sets.TrailingMode[Type]=0;}
    public override void SetDefaults()
    {
        Projectile.width=16;Projectile.height=16;Projectile.friendly=true;Projectile.DamageType=DamageClass.Magic;
        Projectile.timeLeft=160;Projectile.penetrate=1;Projectile.usesLocalNPCImmunity=true;Projectile.localNPCHitCooldown=18;
    }
    public override void AI()
    {
        if(Projectile.localAI[0]++==0)
        {
            Projectile.penetrate=Spell is Spells.Frost or Spells.Tide ? 3 : 1;
            if(Spell==Spells.Tide) Projectile.scale=1.15f;
        }
        Projectile.rotation=Projectile.velocity.ToRotation();
        Lighting.AddLight(Projectile.Center,Spells.Colors[Spell].ToVector3()*.5f);
        if(!Main.dedServ && Projectile.timeLeft%3==0)
        {
            Vector2 offset=Projectile.velocity.SafeNormalize(Vector2.UnitX).RotatedBy(MathHelper.PiOver2)*MathF.Sin(Projectile.timeLeft*.3f)*5;
            Dust d=Dust.NewDustPerfect(Projectile.Center+offset,DustID.RainbowMk2,-Projectile.velocity*.05f,90,Spells.Colors[Spell],.6f);d.noGravity=true;
        }
    }
    public override void OnHitNPC(NPC target,NPC.HitInfo hit,int damageDone)
    {
        if(Projectile.owner!=Main.myPlayer) return;
        Main.player[Projectile.owner].GetModPlayer<SpellcraftPlayer>().RecordHit(Spell,target,damageDone);
        if(Spell==Spells.Ember) target.AddBuff(BuffID.OnFire,240);
        if(Spell==Spells.Frost) target.AddBuff(BuffID.Frostburn,180);
        if(Spell==Spells.Tide)
        {
            target.AddBuff(BuffID.Wet,240);
            if(Projectile.ai[2]==0)
                for(int i=-1;i<=1;i++) Projectile.NewProjectile(Projectile.GetSource_OnHit(target),target.Center,
                    Projectile.velocity.SafeNormalize(Vector2.UnitX).RotatedBy(i*.45f)*7,Type,Math.Max(1,Projectile.damage/3),1,Projectile.owner,Spells.Tide,Mastery,1);
        }
        if(Spell==Spells.Storm) Chain(target);
        SpellVisuals.CastSigil(Projectile.GetSource_OnHit(target),target.Center,Spell,Projectile.owner);
    }
    private void Chain(NPC first)
    {
        var used=new System.Collections.Generic.HashSet<int>{first.whoAmI};NPC previous=first;
        for(int i=0;i<2+Mastery/4;i++)
        {
            NPC closest=null;float range=260*260;
            foreach(NPC n in Main.ActiveNPCs)
            {
                if(used.Contains(n.whoAmI)||!n.CanBeChasedBy()||!Collision.CanHitLine(previous.Center,1,1,n.Center,1,1)) continue;
                float distance=Vector2.DistanceSquared(previous.Center,n.Center);
                if(distance<range) {range=distance;closest=n;}
            }
            if(closest==null) break;
            int damage=(int)(Projectile.damage*(closest.wet ? .8f : .6f));
            Projectile.NewProjectile(Projectile.GetSource_OnHit(first),closest.Center,Vector2.Zero,ModContent.ProjectileType<StormLink>(),Math.Max(1,damage),1,
                Projectile.owner,previous.Center.X,previous.Center.Y,closest.whoAmI);
            used.Add(closest.whoAmI);previous=closest;
        }
    }
    public override void OnKill(int timeLeft)
    {
        SpellVisuals.Spark(Projectile.Center,Spells.Colors[Spell],10);
        if(Spell==Spells.Ember && Projectile.owner==Main.myPlayer)
            Projectile.NewProjectile(Projectile.GetSource_Death(),Projectile.Center,Vector2.Zero,ModContent.ProjectileType<EmberDetonation>(),
                Math.Max(1,Projectile.damage/2),5,Projectile.owner,64+Mastery*3,Mastery);
    }
    public override bool PreDraw(Player player,ref Color lightColor)
    {
        int palette=Main.player[Projectile.owner].GetModPlayer<global::NyuuTerraria.Content.Frontiers.FrontierPlayer>().Palette;
        Color color=palette==1?new Color(245,169,184):palette==2?new Color(159,189,255):palette==3?new Color(255,159,108):Spells.Colors[Spell];Vector2 previous=Projectile.Center;
        for(int i=0;i<Projectile.oldPos.Length;i++)
        {
            if(Projectile.oldPos[i]==Vector2.Zero) break;
            Vector2 point=Projectile.oldPos[i]+Projectile.Size*.5f;float fade=1-i/(float)Projectile.oldPos.Length;
            SpellVisuals.Line(previous,point,SpellVisuals.Glow(color,fade*.7f),7*fade);
            SpellVisuals.Line(previous,point,SpellVisuals.Glow(Color.White,fade*.6f),1.8f*fade);previous=point;
        }
        Texture2D texture=ModContent.Request<Texture2D>("NyuuTerraria/Content/Spellcraft/"+Spells.Icons[Spell]).Value;
        Main.EntitySpriteDraw(texture,Projectile.Center-Main.screenPosition,null,Color.White,Projectile.rotation,texture.Size()*.5f,Projectile.scale*.72f,SpriteEffects.None);
        return false;
    }
}

public class EmberCharge : ModProjectile
{
    public override string Texture=>"NyuuTerraria/Content/Spellcraft/EmberSeed";
    public override void SetDefaults() {Projectile.width=16;Projectile.height=16;Projectile.tileCollide=false;Projectile.timeLeft=2;}
    public override bool? CanDamage()=>false;
    public override bool ShouldUpdatePosition()=>false;
    public override void AI()
    {
        Player p=Main.player[Projectile.owner];
        if(!p.active || p.dead || p.HeldItem.ModItem is not AetherloomWand || (Projectile.owner==Main.myPlayer && p.GetModPlayer<SpellcraftPlayer>().SelectedSpell!=Spells.Ember)) {Projectile.Kill();return;}
        Projectile.timeLeft=2;Projectile.ai[0]++;
        if(Projectile.owner==Main.myPlayer)
        {
            Projectile.velocity=(Main.MouseWorld-p.Center).SafeNormalize(new Vector2(p.direction,0));
            Projectile.Center=p.Center+Projectile.velocity*48;
            if(Projectile.ai[0]%8==0) Projectile.netUpdate=true;
            int chargeCost=Math.Max(1,(int)(2*p.manaCost));bool pay=Projectile.ai[0]%20!=0 || (p.statMana>=chargeCost && p.CheckMana(p.HeldItem,chargeCost,true)); // 1.4.5 CheckMana can pass on an empty pool
            if(!p.channel || !pay || Projectile.ai[0]>=90)
            {
                float charge=Math.Clamp(Projectile.ai[0]/90f,0,1);
                int id=Projectile.NewProjectile(Projectile.GetSource_FromAI(),Projectile.Center,Projectile.velocity*13,ModContent.ProjectileType<SpellShot>(),
                    (int)(Projectile.damage*(1+charge*.75f)),Projectile.knockBack,Projectile.owner,Spells.Ember,Projectile.ai[1]);
                Main.projectile[id].scale=1+charge*.45f;
                SpellVisuals.CastSigil(Projectile.GetSource_FromAI(),Projectile.Center,Spells.Ember,Projectile.owner);
                p.channel=false;Projectile.Kill();return;
            }
            if(Projectile.ai[0]%20==0 && Main.netMode==NetmodeID.MultiplayerClient) NetMessage.SendData(MessageID.PlayerMana,number:p.whoAmI);
        }
        Projectile.Center=p.Center+Projectile.velocity.SafeNormalize(new Vector2(p.direction,0))*48;
        p.heldProj=Projectile.whoAmI;p.itemTime=2;p.itemAnimation=2;
        Lighting.AddLight(Projectile.Center,.7f,.3f,.1f);
    }
    public override bool PreDraw(Player player,ref Color lightColor)
    {
        float charge=Math.Clamp(Projectile.ai[0]/90f,0,1);
        SpellVisuals.Rune(Projectile.Center,17+charge*21,Spells.Colors[Spells.Ember],Projectile.ai[0]*.045f,.7f);
        Texture2D t=ModContent.Request<Texture2D>(Texture).Value;
        Main.EntitySpriteDraw(t,Projectile.Center-Main.screenPosition,null,Color.White,Projectile.ai[0]*.04f,t.Size()*.5f,.45f+charge*.5f,SpriteEffects.None);
        return false;
    }
}

public class EmberDetonation : ModProjectile
{
    public override string Texture=>"NyuuTerraria/Content/Spellcraft/EmberSeed";
    public override void SetDefaults()
    {
        Projectile.width=2;Projectile.height=2;Projectile.friendly=true;Projectile.DamageType=DamageClass.Magic;Projectile.tileCollide=false;
        Projectile.penetrate=-1;Projectile.timeLeft=24;Projectile.usesLocalNPCImmunity=true;Projectile.localNPCHitCooldown=-1;
    }
    public override bool ShouldUpdatePosition()=>false;
    public override void AI()
    {
        if(Projectile.localAI[0]++==0)
        {
            Vector2 center=Projectile.Center;int radius=Math.Clamp((int)Projectile.ai[0],32,100);
            Projectile.width=Projectile.height=radius*2;Projectile.Center=center;
        }
    }
    public override bool? CanDamage()=>Projectile.timeLeft>=22 ? null : false;
    public override void OnHitNPC(NPC target,NPC.HitInfo hit,int damageDone)
    {
        target.AddBuff(BuffID.OnFire,240);
        if(Projectile.owner==Main.myPlayer) Main.player[Projectile.owner].GetModPlayer<SpellcraftPlayer>().RecordHit(Spells.Ember,target,damageDone);
    }
    public override bool PreDraw(Player player,ref Color lightColor)
    {
        float fade=Projectile.timeLeft/24f;float radius=Projectile.width*.5f*(1.1f-fade*.7f);
        SpellVisuals.Rune(Projectile.Center,radius,Spells.Colors[Spells.Ember],fade*.9f,fade);
        return false;
    }
}

public class StormLink : ModProjectile
{
    public override string Texture=>"NyuuTerraria/Content/Spellcraft/StormSpark";
    public override void SetDefaults()
    {
        Projectile.width=12;Projectile.height=12;Projectile.friendly=true;Projectile.DamageType=DamageClass.Magic;Projectile.tileCollide=false;
        Projectile.penetrate=-1;Projectile.timeLeft=18;Projectile.usesLocalNPCImmunity=true;Projectile.localNPCHitCooldown=-1;
    }
    public override bool ShouldUpdatePosition()=>false;
    public override bool? CanDamage()=>Projectile.timeLeft>=15 ? null : false;
    public override bool? CanHitNPC(NPC target)=>target.whoAmI==(int)Projectile.ai[2] ? null : false;
    public override void AI()
    {
        int target=(int)Projectile.ai[2];
        if(target>=0 && target<Main.maxNPCs && Main.npc[target].active) Projectile.Center=Main.npc[target].Center;
        Lighting.AddLight(Projectile.Center,.2f,.4f,.5f);
    }
    public override void OnHitNPC(NPC target,NPC.HitInfo hit,int damageDone)
    {
        if(Projectile.owner==Main.myPlayer) Main.player[Projectile.owner].GetModPlayer<SpellcraftPlayer>().RecordHit(Spells.Storm,target,damageDone);
    }
    public override bool PreDraw(Player player,ref Color lightColor)
    {
        Vector2 from=new(Projectile.ai[0],Projectile.ai[1]),to=Projectile.Center;
        Vector2 side=(to-from).SafeNormalize(Vector2.UnitX).RotatedBy(MathHelper.PiOver2);Vector2 previous=from;
        for(int i=1;i<=12;i++)
        {
            Vector2 point=Vector2.Lerp(from,to,i/12f);
            if(i<12) point+=side*MathF.Sin(i*5.3f+Projectile.whoAmI+Main.GameUpdateCount/3)*9;
            float fade=Projectile.timeLeft/18f;
            SpellVisuals.Line(previous,point,SpellVisuals.Glow(Spells.Colors[Spells.Storm],fade),5);
            SpellVisuals.Line(previous,point,SpellVisuals.Glow(Color.White,fade),1.4f);previous=point;
        }
        return false;
    }
}

public class MendingBloom : ModProjectile
{
    public override string Texture=>"NyuuTerraria/Content/Spellcraft/DawnPetal";
    public override void SetDefaults() {Projectile.width=2;Projectile.height=2;Projectile.tileCollide=false;Projectile.timeLeft=40;}
    public override bool? CanDamage()=>false;
    public override bool ShouldUpdatePosition()=>false;
    public override void AI()
    {
        Projectile.Center=Main.player[Projectile.owner].Center;Lighting.AddLight(Projectile.Center,.5f,.35f,.4f);
    }
    public override bool PreDraw(Player player,ref Color lightColor)
    {
        float fade=Projectile.timeLeft/40f;float radius=30+(1-fade)*35;
        SpellVisuals.Rune(Projectile.Center,radius,Spells.Colors[Spells.Mend],(1-fade)*1.5f,fade);
        Texture2D t=ModContent.Request<Texture2D>(Texture).Value;
        for(int i=0;i<6;i++)
        {
            float a=MathHelper.TwoPi*i/6+fade;
            Main.EntitySpriteDraw(t,Projectile.Center+Vector2.UnitX.RotatedBy(a)*radius-Main.screenPosition,null,Color.White*fade,a,t.Size()*.5f,.45f,SpriteEffects.None);
        }
        return false;
    }
}


