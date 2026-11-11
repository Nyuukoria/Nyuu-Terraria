using System;
using System.IO;
using System.Linq;
using MonoMod.RuntimeDetour;
using Microsoft.Xna.Framework;
using Terraria;
using Terraria.ID;
using Terraria.ModLoader;
using Terraria.ModLoader.IO;
using NyuuTerraria.Content.Spellcraft;

namespace NyuuTerraria.Content;
public static class SpellcraftChecks
{
    static void Check(bool ok,string label) {if(!ok) throw new Exception("MAGE_CHECK_FAILED: "+label);Console.WriteLine("PASS: MAGE_"+label);}
    static void ClearProjectiles() {foreach(var p in Main.projectile) p.active=false;Array.Clear(Main.player[0].ownedProjectileCounts);}
    public static void Run()
    {
        Player p=Main.player[0];int mode=Main.netMode;bool hard=Main.hardMode;Main.netMode=2;Main.myPlayer=0;
        var data=p.GetModPlayer<SpellcraftPlayer>();
        using var silentNetwork=new Hook(typeof(ModPacket).GetMethod("Send"),
            (Action<Action<ModPacket,int,int>,ModPacket,int,int>)((original,packet,to,ignore)=>{}));
        try
        {
            p.Center=new Vector2(320,320);ClearProjectiles();data.Initialize();
            Check(data.Learned[0]&&data.Learned[6]&&data.Learned.Count(x=>x)==2,"STARTER_SPELLS_ONLY");
            Check(!data.Select(1)&&data.Select(0),"LOCKED_SPELL_CANNOT_BE_SELECTED");
            Check(data.Learn(1)&&!data.Learn(1),"TOME_LEARNING_ONCE");
            Main.hardMode=false;
            for(int i=0;i<500;i++) data.GainExperience(1,25);
            Check(data.EffectiveLevel(1)==5&&data.Experience[1]==1875,"PREHARDMODE_MASTERY_CAP");
            Main.hardMode=true;for(int i=0;i<500;i++)data.GainExperience(1,25);
            Check(data.EffectiveLevel(1)==10&&data.Experience[1]==7500,"HARDMODE_MASTERY_CAP");
            Main.hardMode=false;data.GainExperience(1,10);
            Check(data.StoredLevel(1)==10&&data.EffectiveLevel(1)==5&&data.Experience[1]==7500,"WORLD_SWITCH_PRESERVES_ADVANCED_MASTERY");
            for(int i=0;i<6;i++)data.Learn(i);data.Select(5);data.MendCooldown=321;
            TagCompound tag=new();data.SaveData(tag);using var buffer=new MemoryStream();TagIO.ToStream(tag,buffer);buffer.Position=0;
            data.Initialize();data.LoadData(TagIO.FromStream(buffer));
            Check(data.Experience[1]==7500&&data.Learned.All(x=>x)&&data.SelectedSpell==5&&data.MendCooldown==321,"BINARY_CHARACTER_ROUNDTRIP");
            data.Select(2);data.Select(1);Check(data.Experience[1]==7500,"SPELL_SWITCH_PRESERVES_PROGRESS");
            typeof(PlayerLoader).GetMethod("SetupPlayer",System.Reflection.BindingFlags.Static|System.Reflection.BindingFlags.NonPublic).Invoke(null,new object[]{Main.player[1]});
            Check(!ReferenceEquals(data.Experience,Main.player[1].GetModPlayer<SpellcraftPlayer>().Experience),"CHARACTERS_HAVE_SEPARATE_MASTERY_ARRAYS");
            var bad=new TagCompound { ["spellcraftKnown"]=127,["spellcraftXP"]=new[]{-3,int.MaxValue},["spellcraftSelected"]=999,["spellcraftMendCooldown"]=9999 };
            data.LoadData(bad);
            Check(data.Experience[0]==0&&data.Experience[1]==7500&&data.SelectedSpell==5&&data.MendCooldown==480,"INVALID_SAVE_VALUES_ARE_BOUNDED");
            data.Initialize();data.Learn(Spells.Mend);p.statLifeMax=p.statLifeMax2=100;p.statLife=70;
            Check(data.TryMend()&&p.statLife==80&&data.MendCooldown==480,"MENDING_HEALS_AND_STARTS_COOLDOWN");
            Check(!data.TryMend()&&p.statLife==80,"MENDING_CANNOT_BYPASS_COOLDOWN");
            data.MendCooldown=0;p.statLife=100;
            Check(!data.TryMend(),"FULL_HEALTH_CANNOT_FARM_MENDING");
            for(int i=0;i<6;i++)data.Learn(i);
            p.inventory[p.selectedItem]=new Item(ModContent.ItemType<AetherloomWand>());
            p.ownedProjectileCounts=new int[ProjectileLoader.ProjectileCount];
            Item wand=p.inventory[p.selectedItem];wand.Prefix(PrefixID.Mythical);
            int damage=wand.damage,mana=wand.mana,speed=wand.useTime;
            p.statLife=80;
            for(int spell=0;spell<6;spell++)
            {
                data.Select(spell);p.altFunctionUse=0;wand.ModItem.CanUseItem(p);
                float reduce=0,mult=1;wand.ModItem.ModifyManaCost(p,ref reduce,ref mult);
                Check(wand.damage==damage&&wand.mana==mana&&wand.useTime==speed&&Math.Abs(mult-Spells.Mana[spell]/4f)<.001f,"PREFIX_STATS_PRESERVED_MODE_"+spell);
            }
            var target=Main.npc[20];target.SetDefaults(NPCID.Zombie);target.whoAmI=20;target.active=true;target.SpawnedFromStatue=false;target.friendly=false;
            data.Experience[0]=0;data.RecordHit(0,target,20);data.RecordHit(0,target,20);
            Check(data.Experience[0]==2,"SAME_TICK_HITS_CANNOT_MULTIPLY_MASTERY");
            target.SpawnedFromStatue=true;data.RecordHit(2,target,90);
            Check(data.Experience[2]==0,"STATUE_ENEMIES_DO_NOT_GIVE_MASTERY");target.SpawnedFromStatue=false;
            var bullet=new Projectile {active=true,hostile=true,friendly=false,damage=20,width=12,height=12,velocity=new Vector2(-6,0)};
            bullet.Center=new Vector2(65,0);
            Check(CrescentWard.CanBlock(bullet,new Vector2(48,0),Vector2.Zero,Vector2.UnitX,45),"WARD_ACCEPTS_SMALL_FRONT_PROJECTILE");
            bullet.Center=new Vector2(-10,0);Check(!CrescentWard.CanBlock(bullet,new Vector2(48,0),Vector2.Zero,Vector2.UnitX,45),"WARD_DOES_NOT_PROTECT_BEHIND_PLAYER");
            bullet.Center=new Vector2(65,0);bullet.netImportant=true;
            Check(!CrescentWard.CanBlock(bullet,new Vector2(48,0),Vector2.Zero,Vector2.UnitX,45),"WARD_PRESERVES_IMPORTANT_PROJECTILES");
            bullet.netImportant=false;bullet.damage=200;
            Check(!CrescentWard.CanBlock(bullet,new Vector2(48,0),Vector2.Zero,Vector2.UnitX,45),"WARD_PRESERVES_HEAVY_ATTACKS");
            Main.netMode=2; // Server logic with outbound packets captured by the test hook.
            p.active=true;p.dead=false;p.manaCost=1f;p.statMana=p.statManaMax2=200;p.channel=true;data.SelectedSpell=0;
            Main.mouseX=(int)(p.Center.X+200-Main.screenPosition.X);Main.mouseY=(int)(p.Center.Y-Main.screenPosition.Y);
            ClearProjectiles();int wardId=Projectile.NewProjectile(p.GetSource_Misc("Test"),p.Center,Vector2.UnitX,ModContent.ProjectileType<CrescentWard>(),0,0,0);
            var ward=Main.projectile[wardId];ward.ModProjectile.AI();
            int hostile=Projectile.NewProjectile(p.GetSource_Misc("Test"),ward.Center,new Vector2(-6,0),ProjectileID.Bullet,20,0,0);
            var shot=Main.projectile[hostile];shot.friendly=false;shot.hostile=true;shot.netImportant=false;
            ward.ModProjectile.AI();Check((!shot.active||shot.type!=ProjectileID.Bullet)&&ward.localAI[1]>0,"WARD_BLOCKS_PROJECTILE_AND_ENTERS_RECOVERY");
            int second=Projectile.NewProjectile(p.GetSource_Misc("Test"),ward.Center,new Vector2(-6,0),ProjectileID.Bullet,20,0,0);
            Main.projectile[second].friendly=false;Main.projectile[second].hostile=true;Main.projectile[second].netImportant=false;
            ward.ModProjectile.AI();Check(Main.projectile[second].active,"WARD_RECOVERY_PREVENTS_UNLIMITED_BLOCKS");
            for(int inv=0;inv<p.inventory.Length;inv++)if(inv!=p.selectedItem)p.inventory[inv].TurnToAir();p.manaFlower=false;ward.localAI[0]=11;p.statMana=0;ward.ModProjectile.AI();Check(!ward.active&&!p.channel,"WARD_STOPS_WITHOUT_MANA");
            data.EfficientWard=false;int cost=data.WardManaCost;data.EfficientWard=true;
            Check(cost==2&&data.WardManaCost==1,"WARD_SIGIL_REDUCES_CONTINUING_COST");
            data.ResetEffects();Check(!data.EfficientWard,"REMOVING_WARD_SIGIL_CLEARS_EFFECT");
            foreach(int spell in new[]{0,1,2,3,5})
            {
                ClearProjectiles();int id=Projectile.NewProjectile(p.GetSource_Misc("Test"),p.Center,Vector2.UnitX*13,ModContent.ProjectileType<SpellShot>(),40,3,0,spell,5);
                var projectile=Main.projectile[id];
                for(int tick=0;tick<40;tick++){projectile.ModProjectile.AI();projectile.position+=projectile.velocity;projectile.timeLeft--;}
                Check(float.IsFinite(projectile.Center.X)&&projectile.Distance(p.Center)>400,"LONG_RANGE_PROJECTILE_MODE_"+spell);
            }
            ClearProjectiles();data.SelectedSpell=1;p.channel=true;p.statMana=200;
            int chargeId=Projectile.NewProjectile(p.GetSource_Misc("Test"),p.Center,Vector2.UnitX,ModContent.ProjectileType<EmberCharge>(),49,4,0,0,5);
            var charge=Main.projectile[chargeId];for(int i=0;i<45;i++)charge.ModProjectile.AI();p.channel=false;charge.ModProjectile.AI();
            Check(!charge.active&&Main.projectile.Any(q=>q.active&&q.type==ModContent.ProjectileType<SpellShot>()&&q.ai[0]==1&&q.damage>49),"EMBER_CHARGE_RELEASES_STRONGER_SHOT");
            ClearProjectiles();data.SelectedSpell=1;p.channel=true;
            chargeId=Projectile.NewProjectile(p.GetSource_Misc("Test"),p.Center,Vector2.UnitX,ModContent.ProjectileType<EmberCharge>(),49,4,0);
            data.SelectedSpell=0;Main.projectile[chargeId].ModProjectile.AI();
            Check(!Main.projectile.Any(q=>q.active&&q.type==ModContent.ProjectileType<SpellShot>()),"CHANGING_SPELL_CANCELS_HELD_CHARGE");
            for(int i=0;i<3;i++) {var n=Main.npc[20+i];n.SetDefaults(NPCID.Zombie);n.whoAmI=20+i;n.active=true;n.Center=p.Center+new Vector2(80+i*60,0);}
            ClearProjectiles();int stormId=Projectile.NewProjectile(p.GetSource_Misc("Test"),p.Center,Vector2.UnitX*13,ModContent.ProjectileType<SpellShot>(),46,3,0,5,5);
            Main.projectile[stormId].ModProjectile.OnHitNPC(Main.npc[20],new NPC.HitInfo(),46);
            Check(Main.projectile.Any(q=>q.active&&q.type==ModContent.ProjectileType<StormLink>()),"LIGHTNING_CHAINS_TO_OTHER_ENEMIES");
            ClearProjectiles();int tideId=Projectile.NewProjectile(p.GetSource_Misc("Test"),p.Center,Vector2.UnitX*13,ModContent.ProjectileType<SpellShot>(),40,3,0,3,5,1);
            Main.projectile[tideId].ModProjectile.OnHitNPC(Main.npc[20],new NPC.HitInfo(),20);
            Check(Main.projectile.Count(q=>q.active&&q.type==ModContent.ProjectileType<SpellShot>())==1,"WATER_DROPLETS_CANNOT_RECURSIVELY_SPLIT");
            Check(Main.npc[20].HasBuff(BuffID.Wet),"WATER_APPLIES_WET");
            int blastId=Projectile.NewProjectile(p.GetSource_Misc("Test"),p.Center,Vector2.Zero,ModContent.ProjectileType<EmberDetonation>(),20,3,0,80,5);
            var blast=Main.projectile[blastId];blast.ModProjectile.AI();blast.timeLeft=20;
            Check(blast.width==160&&blast.ModProjectile.CanDamage()==false,"EXPLOSION_DAMAGE_WINDOW_AND_RADIUS");
            Console.WriteLine("SPELLCRAFT_TESTS_COMPLETE");
        }
        finally {ClearProjectiles();for(int i=20;i<23;i++)Main.npc[i].active=false;Main.netMode=mode;Main.hardMode=hard;}
    }
}






