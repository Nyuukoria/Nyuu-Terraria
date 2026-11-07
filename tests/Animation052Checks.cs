using System;
using System.Linq;
using Microsoft.Xna.Framework;
using Terraria.ModLoader;
using NyuuTerraria.Content.Frontiers;
namespace NyuuTerraria.Content;
internal static class Animation052Checks
{
    internal static void Run()
    {
        void Check(bool ok,string name){if(!ok)throw new Exception(name);Console.WriteLine("PASS: "+name);}
        var mod=ModContent.GetInstance<NyuuTerraria>();
        foreach(var n in mod.GetContent<ModNPC>())Check(LivingAnimation.Rigs.ContainsKey(n.Name),"ANIMATION_COVERAGE_"+n.Name);
        foreach(var name in LivingAnimation.Projectiles)Check(mod.TryFind<ModProjectile>(name,out _)&&LivingAnimation.Rigs.ContainsKey(name),"PET_MINION_ANIMATION_"+name);
        var box=new Rectangle(2,3,48,64);
        foreach(var rig in Enum.GetValues<LivingRig>())foreach(bool grounded in new[]{false,true})foreach(float speed in new[]{0f,3f}){
            var a=LivingAnimation.Pose(rig,box,0,speed,false,grounded);var b=LivingAnimation.Pose(rig,box,1.1f,speed,true,grounded);
            Check(a.Count>0&&a.Count==b.Count&&a.Zip(b).Any(p=>p.First!=p.Second),"IDLE_MOVING_ACTION_POSES_"+rig+"_"+grounded+"_"+speed);
            Check(a.Concat(b).All(p=>box.Contains(p.Area)&&float.IsFinite(p.Angle)&&float.IsFinite(p.Shift.X)&&float.IsFinite(p.Shift.Y)&&p.Scale.X>0&&p.Scale.Y>0),"FINITE_IN_BOUNDS_JOINTS_"+rig+"_"+grounded+"_"+speed);
        }
        Check(LivingAnimation.FacesRight("GardenBird")&&!LivingAnimation.FacesRight("Lyra"),"ORIGINAL_SPRITE_FACING_CONVENTIONS");
    }
}
