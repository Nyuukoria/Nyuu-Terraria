using System;
using System.Collections.Generic;
using Terraria;
using Terraria.ModLoader;
namespace NyuuTerraria.Content.Frontiers;

public class CreatureAnimationSetup : ModSystem
{
    internal static readonly HashSet<string> Names=new(){"NimbusWarden","PrismSentinel","MourningBell","RootboundKnight","Bellkeeper","PrismMatriarch","ReefbackGuardian","LanternStalker","TheMirrorless","GlassSerpent","PaperWraith","DewFox","GlassDeer","Mothcat","MooncapSprite","PaleGardener","TheUnwritten","ArchiveCaretaker","ReefAmbusher","ReefSkitterer","VineboundArmor","AshBurrower","StarfallenConstruct","TidepoolTurtle","GardenBird","CrystalSnail","LanternFirefly","MossbackBeetle","LanternMoth","GlassCrawler","GlasswingMoth"};
    public override void PostSetupContent(){foreach(var npc in Mod.GetContent<ModNPC>())if(Names.Contains(npc.Name))Main.npcFrameCount[npc.Type]=4;}
}
public class CreatureAnimation : GlobalNPC
{
    public override void FindFrame(NPC npc,int frameHeight)
    {if(npc.ModNPC?.Mod!=Mod||!CreatureAnimationSetup.Names.Contains(npc.ModNPC.Name))return;bool grounded=!npc.noGravity;npc.frame.Y=(grounded&&Math.Abs(npc.velocity.X)<.1f?0:(int)(Main.GameUpdateCount/(grounded?8u:12u)+((uint)npc.whoAmI))%4)*frameHeight;}
}
