using System;
using System.Linq;
using Microsoft.Xna.Framework;
using Terraria;
using Terraria.ID;
using Terraria.ModLoader;
using Terraria.GameContent;
using NyuuTerraria.Content.Reverie;

namespace NyuuTerraria.Content.Frontiers;

public class WildlifeCare : GlobalNPC
{
    public override bool InstancePerEntity=>true;
    private int following=-1,followTicks,timer,restTicks;
    public override void SendExtraAI(NPC npc,Terraria.ModLoader.IO.BitWriter bits,System.IO.BinaryWriter writer){writer.Write((short)following);writer.Write((short)followTicks);writer.Write((short)restTicks);}
    public override void ReceiveExtraAI(NPC npc,Terraria.ModLoader.IO.BitReader bits,System.IO.BinaryReader reader){following=Math.Clamp((int)reader.ReadInt16(),-1,Main.maxPlayers-1);followTicks=Math.Clamp((int)reader.ReadInt16(),0,3600);restTicks=Math.Clamp((int)reader.ReadInt16(),0,180);}
    internal static bool Feed(Player p,int i,int j)
    {
        if(Main.netMode==NetmodeID.MultiplayerClient||!p.active||p.dead||!WorldGen.InWorld(i,j,10)||p.Distance(new Vector2(i*16,j*16))>160||!Main.tile[i,j].HasTile||Main.tile[i,j].TileType!=ModContent.TileType<FeedingBowlTile>())return false;
        NPC critter=Main.npc.Where(n=>n.active&&n.ModNPC is PeacefulExpeditionCreature&&n.Distance(new Vector2(i*16,j*16))<320).OrderBy(n=>n.Distance(p.Center)).FirstOrDefault();
        if(critter==null){if(!Main.dedServ)Main.NewText("Let a peaceful creature approach the bowl first.",Color.LightPink);return false;}
        var care=critter.GetGlobalNPC<WildlifeCare>();if(care.followTicks>0)return false;
        if(!p.ConsumeItem(ItemID.Daybloom)){if(!Main.dedServ)Main.NewText("Place a Daybloom in the feeding bowl.",Color.LightPink);return false;}
        care.following=p.whoAmI;care.followTicks=3600;care.restTicks=0;critter.life=critter.lifeMax;critter.netUpdate=true;
        if(Main.netMode==NetmodeID.Server){var packet=ModContent.GetInstance<global::NyuuTerraria.NyuuTerraria>().GetPacket();packet.Write((byte)8);packet.Write("feed-"+critter.ModNPC.Name);packet.Send(p.whoAmI);}else p.GetModPlayer<FrontierPlayer>().Discover("feed-"+critter.ModNPC.Name);
        return true;
    }
    public override void PostAI(NPC npc)
    {
        if(npc.ModNPC is not PeacefulExpeditionCreature)return;
        if(restTicks>0&&followTicks==0){restTicks--;npc.velocity.X=0;}
        if(followTicks>0)
        {
            followTicks--;if(following>=0&&following<Main.maxPlayers){var p=Main.player[following];if(p.active&&!p.dead&&p.Distance(npc.Center)<700){float delta=p.Center.X-npc.Center.X;npc.velocity.X=Math.Abs(delta)>55?Math.Sign(delta)*1.5f:0;}}
        }
        if(++timer%120==0)
        {
            int x=(int)npc.Center.X/16,y=(int)npc.Center.Y/16;
            for(int tx=x-8;tx<=x+8;tx++)for(int ty=y-4;ty<=y+4;ty++)if(WorldGen.InWorld(tx,ty,5)&&Main.tile[tx,ty].HasTile&&Main.tile[tx,ty].TileType==ModContent.TileType<PetBedTile>()){npc.life=npc.lifeMax;if(followTicks==0){npc.velocity.X=0;restTicks=180;npc.netUpdate=true;}return;}
        }
    }
}
public abstract class FrontierCritter : PeacefulExpeditionCreature
{
    protected abstract int Area{get;}
    public override float SpawnChance(NPC.Spawner i)=>FrontierWorld.AreaAt(i.Player.Center)==Area?(ModContent.GetInstance<FrontierWorld>().EventTicks>0&&ModContent.GetInstance<FrontierWorld>().EventKind==0?.32f:.1f):0;
}
public class TidepoolTurtle : FrontierCritter {protected override int Area=>0;}
public class GardenBird : FrontierCritter {protected override int Area=>1;public override void SetDefaults(){base.SetDefaults();NPC.noGravity=true;}public override void AI(){base.AI();NPC.velocity.Y=MathF.Sin(NPC.ai[0]*.025f)*.7f;}}
public class CrystalSnail : FrontierCritter {protected override int Area=>3;}
public class LanternFirefly : FrontierCritter {protected override int Area=>2;public override void SetDefaults(){base.SetDefaults();NPC.noGravity=true;}public override void AI(){base.AI();NPC.velocity.Y=MathF.Sin(NPC.ai[0]*.04f);Lighting.AddLight(NPC.Center,.45f,.3f,.1f);}}

[AutoloadHead]
public class CemeteryKeeper : ModNPC
{
    public override void SetStaticDefaults(){Main.npcFrameCount[Type]=4;NPCID.Sets.ExtraFramesCount[Type]=0;NPCID.Sets.AttackFrameCount[Type]=0;}
    public override void SetDefaults(){NPC.CloneDefaults(NPCID.Stylist);NPC.townNPC=true;NPC.friendly=true;NPC.width=18;NPC.height=40;NPC.lifeMax=350;NPC.damage=20;NPC.defense=20;AnimationType=-1;}
    public override bool CanTownNPCSpawn(int numTownNPCs)=>NPC.downedBoss1;
    public override System.Collections.Generic.List<string> SetNPCNameList()=>new(){"Mira"};
    public override string GetChat()=>"A memorial can be a garden, a light, or a quiet place to remember. I keep requests in the Worldbound journal.";
    public override void RegisterChatButtons(NPCInteractionList interactions)=>interactions.Prepend(new KeeperStories());
    public override void FindFrame(int h){NPC.frameCounter+=Math.Abs(NPC.velocity.X)*.15;NPC.frame.Y=(NPC.velocity.X==0?0:(int)NPC.frameCounter%4)*h;}
    public override void TownNPCAttackStrength(ref int damage,ref float knockback){damage=20;knockback=3;}
    public override void TownNPCAttackCooldown(ref int cooldown,ref int randExtraCooldown){cooldown=45;randExtraCooldown=10;}
    public override void TownNPCAttackProj(ref int projType,ref int attackDelay){projType=ProjectileID.Spark;attackDelay=8;}
    public override void TownNPCAttackProjSpeed(ref float multiplier,ref float gravityCorrection,ref float randomOffset){multiplier=8;randomOffset=1;}
}
public class KeeperStories : Terraria.GameContent.NPCInteraction
{
    public override bool Condition()=>true;public override string GetText()=>"Memorial stories";
    public override void Interact(){ChronicleInterface.Open=true;ChronicleInterface.Tab=4;FrontierView.Page=4;Main.npcChatText="Let us make room for gentler stories.";}
}
public class FrontierSpawnPool : GlobalNPC
{
    public override void EditSpawnPool(System.Collections.Generic.IDictionary<int,float> pool,NPC.Spawner info)
    {if(FrontierWorld.AreaAt(info.Player.Center)<0)return;foreach(int k in pool.Keys.Where(k=>k<NPCID.Count).ToArray())pool.Remove(k);}
}

