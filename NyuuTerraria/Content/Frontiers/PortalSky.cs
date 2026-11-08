using System;
using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Graphics;
using Terraria;
using Terraria.GameContent;
using Terraria.Graphics.Effects;
using Terraria.ModLoader;
using NyuuTerraria.Content.Reverie;
namespace NyuuTerraria.Content.Frontiers;
public class PortalSkySystem : ModSystem
{
    internal const string Key="NyuuTerraria:PortalWorld";
    public override void Load(){if(!Main.dedServ)SkyManager.Instance[Key]=new PortalSky();}
    public override void PostUpdateEverything(){if(Main.dedServ)return;int realm=Main.gameMenu?-1:RealmWorld.RealmAt(Main.LocalPlayer.Center),area=Main.gameMenu?-1:FrontierWorld.AreaAt(Main.LocalPlayer.Center);int theme=realm>=0?realm:area>=0?area+2:-1;if(theme>=0){PortalSky.Theme=theme;SkyManager.Instance.Activate(Key);}else SkyManager.Instance.Deactivate(Key);}
    public override void OnWorldUnload(){if(!Main.dedServ)SkyManager.Instance[Key].Reset();}
    // The sky lives in Terraria's global sky list; leaving it there after unload keeps the whole old
    // copy of the mod in memory ("mod class still using memory" on every reload).
    public override void Unload()
    {
        if(Main.dedServ||SkyManager.Instance==null)return;
        var manager=SkyManager.Instance;
        if(manager[Key] is CustomSky sky)
        {
            sky.Deactivate();
            var active=typeof(SkyManager).GetField("_activeSkies",System.Reflection.BindingFlags.Instance|System.Reflection.BindingFlags.NonPublic)?.GetValue(manager) as System.Collections.Generic.LinkedList<CustomSky>;
            active?.Remove(sky);
        }
        var effects=typeof(EffectManager<CustomSky>).GetField("_effects",System.Reflection.BindingFlags.Instance|System.Reflection.BindingFlags.NonPublic)?.GetValue(manager) as System.Collections.Generic.Dictionary<string,CustomSky>;
        effects?.Remove(Key);
    }
}
public class PortalSky : CustomSky
{
    internal static int Theme=-1;bool active;float fade;
    public override void Activate(Vector2 position,params object[] args)=>active=true;
    public override void Deactivate(params object[] args)=>active=false;
    public override void Reset(){active=false;fade=0;Theme=-1;}
    public override bool IsActive()=>active||fade>.001f;
    public override void Update(GameTime time)=>fade=MathHelper.Clamp(fade+(active?.035f:-.035f),0,1);
    public override float GetCloudAlpha()=>1-fade;
    public override Color OnTileColor(Color color)=>color;
    public override void Draw(SpriteBatch batch,float minDepth,float maxDepth){if(maxDepth<float.MaxValue||minDepth>=float.MaxValue||Theme<0)return;
        Color[] tops={new(37,29,74),new(11,13,23),new(12,47,65),new(17,38,34),new(45,19,25),new(15,13,48)};
        Color[] bottoms={new(139,116,163),new(49,46,62),new(69,144,155),new(66,99,76),new(143,62,38),new(88,58,138)};
        int theme=Math.Clamp(Theme,0,5);var pixel=TextureAssets.MagicPixel.Value;
        for(int y=0;y<Main.screenHeight;y+=16)batch.Draw(pixel,new Rectangle(0,y,Main.screenWidth,16),Color.Lerp(tops[theme],bottoms[theme],y/(float)Main.screenHeight)*fade);
        for(int i=0;i<44;i++){float drift=(float)(Main.GameUpdateCount%100000)*.08f;float x=(i*179f+drift*(theme==4?.7f:.12f))%Main.screenWidth;float y=(i*97f+(theme==4?-drift:theme==2?-drift*.3f:0))%Main.screenHeight;if(y<0)y+=Main.screenHeight;Color c=theme==4?new Color(255,146,77):theme==2?new Color(140,221,225):new Color(213,193,244);batch.Draw(pixel,new Rectangle((int)x,(int)y,i%6==0?3:2,2),c*(fade*(theme==1?.08f:.3f)));}
    }
}
