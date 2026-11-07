using System;
using System.Collections.Generic;
using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Graphics;
using Terraria;
using Terraria.Audio;
using Terraria.GameContent;
using Terraria.ID;
using Terraria.ModLoader;
using Terraria.ModLoader.Config;
using Terraria.UI;

namespace NyuuTerraria.Content.Reverie;

public class ExpeditionClientConfig : ModConfig
{
    public override ConfigScope Mode=>ConfigScope.ClientSide;
    [System.ComponentModel.DefaultValue(true)]
    public bool HushJumpscares=true;
}
public class HushAtmosphere : ModSystem
{
    private int insideTicks, apparition, cooldown;
    public override void OnWorldUnload() {insideTicks=apparition=cooldown=0;}
    public override void PostUpdateEverything()
    {
        if(Main.dedServ || Main.gameMenu) return;
        if(RealmWorld.RealmAt(Main.LocalPlayer.Center)!=1 || Main.LocalPlayer.dead)
        {insideTicks=0;apparition=0;return;}
        insideTicks++;
        if(apparition>0) apparition--;
        if(cooldown>0) cooldown--;
        if(!ModContent.GetInstance<ExpeditionClientConfig>().HushJumpscares) {apparition=0;return;}
        // No flashing cycle, damage, camera teleport, or input lock; a short apparition and a quiet sting.
        if(cooldown==0 && insideTicks>150 && insideTicks%180==0 && Main.rand.NextBool(4))
        {
            apparition=28;cooldown=1800;
            SoundEngine.PlaySound(SoundID.NPCDeath6 with {Volume=.45f,Pitch=-.4f},Main.LocalPlayer.Center);
        }
    }
    public override void ModifySunLightColor(ref Color tileColor,ref Color backgroundColor)
    {
        if(Main.dedServ || Main.gameMenu) return;
        int realm=RealmWorld.RealmAt(Main.LocalPlayer.Center);
        if(realm==1) {tileColor=new Color(78,64,102);backgroundColor=new Color(12,10,24);}
        else if(realm==0) backgroundColor=Color.Lerp(backgroundColor,new Color(181,150,222),.3f);
    }
    public override void ModifyInterfaceLayers(List<GameInterfaceLayer> layers)
    {
        int index=layers.FindIndex(l=>l.Name=="Vanilla: Inventory");
        if(index>=0) layers.Insert(index,new LegacyGameInterfaceLayer("Nyuu: Hush apparition",Draw,InterfaceScaleType.UI));
    }
    private bool Draw()
    {
        if(apparition<=0 || Main.gameMenu) return true;
        Texture2D t=ModContent.Request<Texture2D>("NyuuTerraria/Content/Reverie/LanternStalker").Value;
        float alpha=Math.Min(1,apparition/8f);
        Vector2 center=new Vector2(Main.screenWidth,Main.screenHeight)*.5f;
        Main.spriteBatch.Draw(TextureAssets.MagicPixel.Value,new Rectangle(0,0,(int)(center.X*2),(int)(center.Y*2)),new Color(8,4,16)*(.5f*alpha));
        Main.spriteBatch.Draw(t,center,null,Color.White*alpha,0,t.Size()*.5f,Math.Min(4f,center.Y/t.Height),SpriteEffects.None,0);
        return true;
    }
}
