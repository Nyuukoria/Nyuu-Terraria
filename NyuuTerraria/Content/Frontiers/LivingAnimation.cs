using System;
using System.Collections.Generic;
using System.Linq;
using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Graphics;
using Terraria;
using Terraria.GameContent;
using Terraria.ModLoader;
namespace NyuuTerraria.Content.Frontiers;

internal enum LivingRig { Humanoid, Town, Quadruped, Moth, Bird, Serpent, Bell, Spirit, Crystal, Mushroom }
internal readonly record struct LivingPart(Rectangle Area,Vector2 Pivot,float Angle,Vector2 Shift,Vector2 Scale);
internal static class LivingAnimation
{
    // Animate regions of the existing artwork. No replacement designs, shaders or AI/gameplay state writes.
    internal static readonly Dictionary<string,LivingRig> Rigs=new(){
        ["Lyra"]=LivingRig.Town,["CemeteryKeeper"]=LivingRig.Town,
        ["DewFox"]=LivingRig.Quadruped,["DewFoxPet"]=LivingRig.Quadruped,["GlassDeer"]=LivingRig.Quadruped,["Mothcat"]=LivingRig.Quadruped,
        ["TidepoolTurtle"]=LivingRig.Quadruped,["MossbackBeetle"]=LivingRig.Quadruped,["GlassCrawler"]=LivingRig.Quadruped,["ReefAmbusher"]=LivingRig.Quadruped,["ReefSkitterer"]=LivingRig.Quadruped,
        ["LanternMoth"]=LivingRig.Moth,["GlasswingMoth"]=LivingRig.Moth,["PrismMatriarch"]=LivingRig.Moth,["PrideMothPet"]=LivingRig.Moth,
        ["GardenBird"]=LivingRig.Bird,["GlowcapFirefly"]=LivingRig.Bird,["LanternFirefly"]=LivingRig.Bird,
        ["AshBurrower"]=LivingRig.Serpent,["GlassSerpent"]=LivingRig.Serpent,["CrystalSnail"]=LivingRig.Serpent,
        ["MourningBell"]=LivingRig.Bell,["Bellkeeper"]=LivingRig.Bell,["LittleGlassBell"]=LivingRig.Bell,
        ["NimbusWarden"]=LivingRig.Spirit,["PaperWraith"]=LivingRig.Spirit,["LanternStalker"]=LivingRig.Spirit,["TheMirrorless"]=LivingRig.Spirit,["AmberWisp"]=LivingRig.Spirit,
        ["PrismSentinel"]=LivingRig.Crystal,["StarfallenConstruct"]=LivingRig.Crystal,
        ["MooncapSprite"]=LivingRig.Mushroom,["MooncapMinion"]=LivingRig.Mushroom,
        ["RootboundKnight"]=LivingRig.Humanoid,["ReefbackGuardian"]=LivingRig.Quadruped,["PaleGardener"]=LivingRig.Humanoid,["TheUnwritten"]=LivingRig.Humanoid,["ArchiveCaretaker"]=LivingRig.Humanoid,["VineboundArmor"]=LivingRig.Humanoid
    };
    internal static readonly HashSet<string> Projectiles=new(){"GlowcapFirefly","AmberWisp","LittleGlassBell","DewFoxPet","PrideMothPet","MooncapMinion"};
    internal static bool FacesRight(string name)=>name is "GardenBird" or "CrystalSnail" or "LanternFirefly" or "GlowcapFirefly" or "AshBurrower";
    private static readonly Dictionary<Texture2D,Rectangle> bounds=new();
    private static readonly Dictionary<(Texture2D,LivingRig),Rectangle[]> bodies=new();
    internal static void Clear(){bounds.Clear();bodies.Clear();}
    private static Rectangle Ink(Texture2D texture,int h)
    {
        if(bounds.TryGetValue(texture,out var box))return box;
        var pixels=new Color[texture.Width*h];texture.GetData(0,new Rectangle(0,0,texture.Width,h),pixels,0,pixels.Length);
        int l=texture.Width,r=-1,t=h,b=-1;
        for(int y=0;y<h;y++)for(int x=0;x<texture.Width;x++)if(pixels[y*texture.Width+x].A>0){l=Math.Min(l,x);r=Math.Max(r,x);t=Math.Min(t,y);b=Math.Max(b,y);}
        box=r<0?new Rectangle(0,0,texture.Width,h):new Rectangle(l,t,r-l+1,b-t+1);bounds[texture]=box;return box;
    }
    internal static List<LivingPart> Pose(LivingRig kind,Rectangle b,float phase,float speed,bool action,bool grounded)
    {
        var parts=new List<LivingPart>();float wave=MathF.Sin(phase),stride=Math.Clamp(speed/2.5f,0,1),force=action?1.6f:1;
        Rectangle R(float x,float y,float w,float h)=>new(b.X+(int)(x*b.Width),b.Y+(int)(y*b.Height),Math.Max(1,(int)(w*b.Width)),Math.Max(1,(int)(h*b.Height)));
        void Add(Rectangle r,float angle=0,float dx=0,float dy=0,float sx=1,float sy=1,float pivotX=.5f,float pivotY=.5f)=>parts.Add(new(r,new Vector2(r.X+r.Width*pivotX,r.Y+r.Height*pivotY),angle,new Vector2(dx,dy),new Vector2(sx,sy)));
        switch(kind){
            case LivingRig.Moth:
                Add(R(0,0,.44f,1),wave*.24f*force,sx:.76f+.24f*MathF.Abs(MathF.Cos(phase)),pivotX:1);
                Add(R(.56f,0,.44f,1),-wave*.24f*force,sx:.76f+.24f*MathF.Abs(MathF.Cos(phase)),pivotX:0);break;
            case LivingRig.Bird:
                Add(R(.22f,.08f,.5f,.53f),wave*.28f*force,sy:.82f+.18f*MathF.Abs(MathF.Cos(phase)),pivotY:1);
                Add(R(.8f,.65f,.2f,.25f),wave*.07f,pivotX:0);break;
            case LivingRig.Quadruped:
                for(int i=0;i<4;i++)Add(R(i*.25f,.76f,.25f,.24f),MathF.Sin(phase+(i%2)*MathF.PI)*.15f*stride,dx:MathF.Sin(phase+(i%2)*MathF.PI)*stride,pivotY:0);
                Add(R(.78f,.2f,.22f,.43f),wave*.09f*force,pivotX:0);break;
            case LivingRig.Serpent:
                for(int i=0;i<4;i++)Add(R(i*.25f,0,.25f,1),dy:MathF.Sin(phase-i*.55f)*(grounded?.7f:1.5f)*(1+stride));break;
            case LivingRig.Bell:Add(R(.25f,.63f,.5f,.37f),wave*.17f*force,pivotY:0);break;
            case LivingRig.Spirit:Add(R(.05f,.57f,.9f,.43f),wave*.035f*force,dx:wave*1.5f*force,pivotY:0);break;
            case LivingRig.Crystal:
                Add(R(0,.24f,.22f,.6f),wave*.05f,dy:wave*1.3f,pivotX:1);
                Add(R(.78f,.24f,.22f,.6f),-wave*.05f,dy:-wave*1.3f,pivotX:0);
                Add(R(.35f,.3f,.3f,.35f),sx:1+.035f*wave*force,sy:1+.035f*wave*force);break;
            case LivingRig.Mushroom:Add(R(0,0,1,.48f),sx:1+.025f*wave,sy:1+.025f*wave,pivotY:1);break;
            case LivingRig.Town:
                Add(R(.1f,.62f,.8f,.22f),dx:wave*(.35f+stride*.7f),pivotY:0);
                Add(R(.8f,.04f,.2f,.35f),wave*.045f,pivotY:0);break;
            default:
                Add(R(0,.34f,.23f,.37f),wave*(.035f+stride*.16f)+(action?-.3f:0),pivotY:0,pivotX:1);
                Add(R(.77f,.34f,.23f,.37f),-wave*(.035f+stride*.16f)+(action?.3f:0),pivotY:0,pivotX:0);
                if(grounded){Add(R(.2f,.78f,.3f,.22f),wave*.12f*stride,pivotY:0);Add(R(.5f,.78f,.3f,.22f),-wave*.12f*stride,pivotY:0);}
                else Add(R(.24f,.78f,.52f,.22f),dx:wave*force,pivotY:0);break;
        }
        return parts;
    }
    internal static void Draw(SpriteBatch batch,Texture2D texture,string name,int frameHeight,int walkFrame,Vector2 anchor,bool grounded,int direction,float scale,float rotation,Color color,float speed,float phase,bool action)
    {
        var rig=Rigs[name];var ink=Ink(texture,frameHeight);var parts=Pose(rig,ink,phase,speed,action,grounded);
        bool flip=FacesRight(name)?direction<0:direction>0;var effects=flip?SpriteEffects.FlipHorizontally:SpriteEffects.None;
        var origin=new Vector2(texture.Width/2f,grounded?frameHeight:frameHeight/2f);
        int frameY=rig==LivingRig.Town?Math.Clamp(walkFrame,0,3)*frameHeight:0;
        void DrawPiece(Rectangle rect,Vector2 pivot,float angle,Vector2 shift,Vector2 stretch){
            var local=(pivot-origin+shift)*scale;if(flip)local.X=-local.X;local=local.RotatedBy(rotation);
            var src=new Rectangle(rect.X,rect.Y+frameY,rect.Width,rect.Height);var partOrigin=pivot-rect.TopLeft();if(flip)partOrigin.X=rect.Width-partOrigin.X;
            batch.Draw(texture,anchor+local,src,color,rotation+(flip?-angle:angle),partOrigin,stretch*scale,effects,0);
        }
        // Draw the untouched body around the moving regions, then attach each region at its joint.
        if(!bodies.TryGetValue((texture,rig),out var body)){
        var xs=new SortedSet<int>{ink.Left,ink.Right};var ys=new SortedSet<int>{ink.Top,ink.Bottom};
        foreach(var part in parts){xs.Add(part.Area.Left);xs.Add(part.Area.Right);ys.Add(part.Area.Top);ys.Add(part.Area.Bottom);}
        var xx=xs.ToArray();var yy=ys.ToArray();var cells=new List<Rectangle>();
        for(int y=0;y<yy.Length-1;y++)for(int x=0;x<xx.Length-1;x++){
            var rect=new Rectangle(xx[x],yy[y],xx[x+1]-xx[x],yy[y+1]-yy[y]);
            if(rect.Width<=0||rect.Height<=0||parts.Any(p=>p.Area.Contains(rect.Center)))continue;
            cells.Add(rect);
        }
        body=cells.ToArray();bodies[(texture,rig)]=body;}
        foreach(var rect in body)DrawPiece(rect,rect.Center.ToVector2(),0,Vector2.Zero,Vector2.One);
        foreach(var part in parts)DrawPiece(part.Area,part.Pivot,part.Angle,part.Shift,part.Scale);
    }
}
public class LivingNPCAnimation : GlobalNPC
{
    public override bool PreDraw(NPC npc,SpriteBatch batch,Vector2 screenPos,Color drawColor)
    {
        if(npc.ModNPC?.Mod!=Mod||!LivingAnimation.Rigs.ContainsKey(npc.ModNPC.Name))return true;
        if(NyuuTerraria.Content.NyuuArtFixes.TryDrawNpc(npc,batch,screenPos,drawColor))return false;
        var texture=TextureAssets.Npc[npc.type].Value;int h=texture.Height/Main.npcFrameCount[npc.type];bool ground=!npc.noGravity;
        bool action=!npc.friendly&&(npc.velocity.LengthSquared()>25||npc.boss&&npc.ai[0]%300>=150&&npc.ai[0]%300<210);
        float phase=(float)(Main.GameUpdateCount%100000)*.1f+npc.whoAmI*1.7f;
        if(LivingAnimation.Rigs[npc.ModNPC.Name] is LivingRig.Moth or LivingRig.Bird)phase*=2;
        LivingAnimation.Draw(batch,texture,npc.ModNPC.Name,h,npc.frame.Y/h,(ground?npc.Bottom:npc.Center)-screenPos,ground,npc.spriteDirection,npc.scale,ground?0:npc.rotation,drawColor,Math.Abs(npc.velocity.X),phase,action);
        return false;
    }
    public override void Unload()=>LivingAnimation.Clear();
}
public class LivingProjectileAnimation : GlobalProjectile
{
    public override bool PreDraw(Projectile projectile,Player player,ref Color lightColor)
    {
        if(NyuuTerraria.Content.NyuuArtFixes.TryDrawNpc(npc,batch,screenPos,drawColor))return false;
        if(projectile.ModProjectile?.Mod!=Mod||!LivingAnimation.Projectiles.Contains(projectile.ModProjectile.Name))return true;
        string name=projectile.ModProjectile.Name;bool ground=name=="DewFoxPet";var texture=TextureAssets.Projectile[projectile.type].Value;
        int direction=Math.Abs(projectile.velocity.X)>.15f?Math.Sign(projectile.velocity.X):projectile.spriteDirection;
        float phase=(float)(Main.GameUpdateCount%100000)*.12f+projectile.whoAmI*1.7f;
        if(LivingAnimation.Rigs[name] is LivingRig.Moth or LivingRig.Bird)phase*=2;
        LivingAnimation.Draw(Main.spriteBatch,texture,name,texture.Height/Main.projFrames[projectile.type],0,(ground?projectile.Bottom:projectile.Center)-Main.screenPosition,ground,direction,projectile.scale,ground?0:projectile.rotation,lightColor,Math.Abs(projectile.velocity.X),phase,projectile.minion&&projectile.velocity.LengthSquared()>36);
        return false;
    }
}
