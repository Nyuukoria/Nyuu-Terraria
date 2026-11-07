using System;
using Microsoft.Xna.Framework;
using Terraria;
using Terraria.ID;
using Terraria.ModLoader;
using NyuuTerraria.Content.Spellcraft;

namespace NyuuTerraria.Content.Frontiers;

// Marks are independent per NPC and per player: teammates cannot consume one another's setup.
public class ElementMarks : GlobalNPC
{
    public override bool InstancePerEntity=>true;
    private int[] element=new int[Main.maxPlayers],expires=new int[Main.maxPlayers],cooldown=new int[Main.maxPlayers];
    public override void SetDefaults(NPC entity) {element=new int[Main.maxPlayers];expires=new int[Main.maxPlayers];cooldown=new int[Main.maxPlayers];Array.Fill(element,-1);}
    public override void ResetEffects(NPC npc)
    {for(int i=0;i<Main.maxPlayers;i++){if(expires[i]>0)expires[i]--;if(cooldown[i]>0)cooldown[i]--;}}
    internal int React(NPC npc,int owner,int spell)
    {
        if(owner<0||owner>=Main.maxPlayers||spell is not (Spells.Ember or Spells.Frost or Spells.Tide or Spells.Storm))return -1;
        int previous=expires[owner]>0?element[owner]:-1;int reaction=-1;
        if(cooldown[owner]==0)
        {
            if((previous==Spells.Ember&&spell==Spells.Tide)||(previous==Spells.Tide&&spell==Spells.Ember))reaction=0;
            if((previous==Spells.Frost&&spell==Spells.Tide)||(previous==Spells.Tide&&spell==Spells.Frost))reaction=1;
            if((previous==Spells.Storm&&spell==Spells.Tide)||(previous==Spells.Tide&&spell==Spells.Storm))reaction=2;
        }
        element[owner]=spell;expires[owner]=240;
        if(reaction>=0){cooldown[owner]=60;expires[owner]=0;}
        return reaction;
    }
    public override void OnHitByProjectile(NPC npc,Projectile projectile,NPC.HitInfo hit,int damageDone)
    {
        if(projectile.ModProjectile is not SpellShot || projectile.owner!=Main.myPlayer || npc.friendly||damageDone<=0)return;
        int kind=React(npc,projectile.owner,(int)projectile.ai[0]);if(kind<0)return;
        if(kind==1)npc.AddBuff(BuffID.Frostburn,300);
        Projectile.NewProjectile(projectile.GetSource_OnHit(npc),npc.Center,Vector2.Zero,ModContent.ProjectileType<ReactionBurst>(),Math.Max(1,(int)(damageDone*(kind==2?.65f:.45f))),3,projectile.owner,kind);
        Main.player[projectile.owner].GetModPlayer<FrontierPlayer>().Discover("reaction-"+kind);
    }
}
public class ReactionBurst : ModProjectile
{
    public override string Texture=>"NyuuTerraria/Content/Spellcraft/TideOrb";
    public override void SetDefaults(){Projectile.width=Projectile.height=128;Projectile.friendly=true;Projectile.DamageType=DamageClass.Magic;Projectile.penetrate=-1;Projectile.timeLeft=22;Projectile.tileCollide=false;Projectile.usesLocalNPCImmunity=true;Projectile.localNPCHitCooldown=-1;}
    public override bool? CanDamage()=>Projectile.timeLeft>=20?null:false;
    public override bool ShouldUpdatePosition()=>false;
    public override void OnHitNPC(NPC target,NPC.HitInfo hit,int damageDone){if(Projectile.ai[0]==1)target.AddBuff(BuffID.Frostburn,300);}
    public override bool PreDraw(Player player,ref Color lightColor)
    {
        Color c=Projectile.ai[0]==0?new Color(235,210,255):Projectile.ai[0]==1?Color.LightCyan:Color.DeepSkyBlue;
        float life=Projectile.timeLeft/22f;SpellVisuals.Rune(Projectile.Center,28+(1-life)*65,c,life,life);
        return false;
    }
}

