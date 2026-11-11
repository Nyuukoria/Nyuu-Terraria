using System;
using Microsoft.Xna.Framework;
using Terraria;
using Terraria.ID;
using Terraria.ModLoader;

namespace NyuuTerraria.Content.Spellcraft;

public class CrescentWard : ModProjectile
{
    public override string Texture=>"NyuuTerraria/Content/Spellcraft/WardSigil";
    public override void SetDefaults()
    {
        Projectile.width=90;Projectile.height=90;Projectile.tileCollide=false;Projectile.timeLeft=2;Projectile.netImportant=true;
    }
    public override bool? CanDamage()=>false;
    public override bool ShouldUpdatePosition()=>false;
    internal static bool CanBlock(Projectile shot,Vector2 wardCenter,Vector2 playerCenter,Vector2 aim,float radius)
    {
        if(!shot.active || !shot.hostile || shot.friendly || shot.netImportant || shot.damage<=0 || shot.damage>=120 ||
            shot.width>32 || shot.height>32 || shot.velocity.LengthSquared()<=4) return false;
        Vector2 delta=shot.Center-playerCenter;
        return Vector2.Dot(delta,aim)>8 && Vector2.DistanceSquared(shot.Center,wardCenter)<radius*radius;
    }
    public override void AI()
    {
        Player player=Main.player[Projectile.owner];
        if(!player.active || player.dead || !player.channel || player.HeldItem.ModItem is not AetherloomWand) {Projectile.Kill();return;}
        Projectile.timeLeft=2;Projectile.localAI[0]++;
        if(Projectile.owner==Main.myPlayer)
        {
            Projectile.velocity=(Main.MouseWorld-player.Center).SafeNormalize(new Vector2(player.direction,0));
            if((int)Projectile.localAI[0]%8==0) Projectile.netUpdate=true;
            if((int)Projectile.localAI[0]%12==0)
            {
                int cost=Math.Max(1,(int)(player.GetModPlayer<SpellcraftPlayer>().WardManaCost*player.manaCost));
                // Terraria 1.4.5's new mana rules can let CheckMana succeed on an empty pool, which kept the
                // ward up forever at 0 mana. Require the mana to be there (a Mana Flower may drink first).
                if(player.statMana<cost&&player.manaFlower)player.QuickMana();
                if(player.statMana<cost||!player.CheckMana(player.HeldItem,cost,true))
                {player.channel=false;Projectile.Kill();return;}
                player.manaRegenDelay=Math.Max(player.manaRegenDelay,60);
                if(Main.netMode==NetmodeID.MultiplayerClient) NetMessage.SendData(MessageID.PlayerMana,number:player.whoAmI);
            }
        }
        Vector2 aim=Projectile.velocity.SafeNormalize(new Vector2(player.direction,0));
        Projectile.Center=player.Center+aim*48;Projectile.rotation=aim.ToRotation();
        player.heldProj=Projectile.whoAmI;player.itemTime=2;player.itemAnimation=2;
        Lighting.AddLight(Projectile.Center,.25f,.35f,.55f);
        if(Projectile.localAI[1]>0) Projectile.localAI[1]--;
        if(Main.netMode==NetmodeID.MultiplayerClient || Projectile.localAI[1]>0) return;
        float radius=40+Math.Clamp(Projectile.ai[0],0,10)*1.5f;
        foreach(Projectile shot in Main.ActiveProjectiles)
        {
            if(!CanBlock(shot,Projectile.Center,player.Center,aim,radius)) continue;
            Vector2 impact=shot.Center;shot.Kill();Projectile.localAI[1]=12;
            if(Main.netMode==NetmodeID.Server)
            {
                var packet=Mod.GetPacket();packet.Write((byte)3);packet.Write((byte)Spells.Ward);packet.Write(4);packet.Send(player.whoAmI);
            }
            else player.GetModPlayer<SpellcraftPlayer>().GainExperience(Spells.Ward,4);
            SpellVisuals.CastSigil(Projectile.GetSource_FromAI(),impact,Spells.Ward,Projectile.owner);
            Projectile.netUpdate=true;break;
        }
    }
    public override bool PreDraw(Player player,ref Color lightColor)
    {
        float radius=40+Math.Clamp(Projectile.ai[0],0,10)*1.5f;
        SpellVisuals.Rune(Projectile.Center,radius,Spells.Colors[Spells.Ward],Projectile.localAI[0]*.025f,.85f);
        SpellVisuals.Circle(Projectile.Center,radius+6,SpellVisuals.Glow(new Color(100,220,255),.45f),-Projectile.localAI[0]*.04f,48,2);
        return false;
    }
}


