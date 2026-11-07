using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Graphics;
using Terraria;
using Terraria.GameContent;
using Terraria.ID;
using Terraria.ModLoader;
using Terraria.ObjectData;

namespace NyuuTerraria.Content.Reverie;

public class PrideRibbon : ModItem
{
    public override void SetDefaults() {Item.width=28;Item.height=26;Item.accessory=true;Item.rare=ItemRarityID.Blue;Item.value=Item.sellPrice(silver:30);}
    public override void UpdateAccessory(Player player,bool hideVisual)
    {
        if(!hideVisual && Main.rand.NextBool(7))
        {
            var dust=Dust.NewDustPerfect(player.Center+new Vector2(Main.rand.Next(-12,13),-20),DustID.RainbowMk2,new Vector2(0,-.4f),110,
                Main.rand.NextBool() ? new Color(245,169,184) : new Color(91,206,250),.6f);dust.noGravity=true;
        }
    }
    public override void AddRecipes()=>CreateRecipe().AddIngredient(ItemID.Silk,3).AddIngredient(ItemID.FallenStar).AddTile(TileID.Loom).Register();
}
public class PrismWardrobe : ModItem
{
    public override string Texture=>"NyuuTerraria/Content/Reverie/GardenMirror";
    public override void SetDefaults()=>Item.DefaultToPlaceableTile(ModContent.TileType<PrismWardrobeTile>());
    public override void AddRecipes()=>CreateRecipe().AddIngredient(ItemID.Wood,25).AddIngredient(ItemID.Glass,10).AddIngredient(ItemID.Silk,5).AddTile(TileID.WorkBenches).Register();
}
public class PrismWardrobeTile : ModTile
{
    public override void SetStaticDefaults()
    {
        Main.tileFrameImportant[Type]=true;
        TileObjectData.newTile.CopyFrom(TileObjectData.Style3x4);TileObjectData.addTile(Type);
        AddMapEntry(new Color(199,159,220),CreateMapEntryName());
    }
    public override bool RightClick(int i,int j)
    {
        ChronicleInterface.WardrobeOrigin=new Point(i,j);ChronicleInterface.Tab=2;ChronicleInterface.Open=true;return true;
    }
    public override void MouseOver(int i,int j) {Main.LocalPlayer.noThrow=2;Main.hoverItemName="Prism Wardrobe - appearance presets";}
}

// Worn layers are drawn from generated clothing art while preserving vanilla player animation.
public abstract class AtelierClothing : ModItem
{
    public override void SetDefaults() {Item.width=28;Item.height=30;Item.vanity=true;Item.rare=ItemRarityID.Blue;}
    public override void AddRecipes()=>CreateRecipe().AddIngredient(ItemID.Silk,10).AddIngredient<MoonprismBar>(2).AddTile(TileID.Loom).Register();
}
[AutoloadEquip(EquipType.Body)]
public class PrismLongcoat : AtelierClothing { }
[AutoloadEquip(EquipType.Legs)]
public class PrismSkirt : AtelierClothing { }

public abstract class LongFeminineHair : ModHair
{
    public override bool AvailableDuringCharacterCreation=>true;
    public override Gender RandomizedCharacterCreationGender=>Gender.Female;
    public override string AltTexture=>Texture+"_Alt";
}
public class WaistlengthWaves : LongFeminineHair { }
public class SilkenCurtain : LongFeminineHair { }
public class TrailingPonytail : LongFeminineHair { }
public class LongSideBraid : LongFeminineHair { }
public class TwinRibbonBraids : LongFeminineHair { }
public class MoonlitCurls : LongFeminineHair { }
public class CascadingLayers : LongFeminineHair { }
public class HalfupRibbon : LongFeminineHair { }

[AutoloadHead]
public class Lyra : ModNPC
{
    public override void SetStaticDefaults()
    {
        Main.npcFrameCount[Type]=4;
        NPCID.Sets.ExtraFramesCount[Type]=0;NPCID.Sets.AttackFrameCount[Type]=0;
    }
    public override void SetDefaults()
    {
        NPC.CloneDefaults(NPCID.Stylist);NPC.townNPC=true;NPC.friendly=true;NPC.width=18;NPC.height=40;
        NPC.lifeMax=300;NPC.damage=20;NPC.defense=16;NPC.aiStyle=7;AnimationType=-1;
    }
    public override bool CanTownNPCSpawn(int numTownNPCs)=>true;
    public override System.Collections.Generic.List<string> SetNPCNameList()=>new(){"Lyra"};
    public override string GetChat()
    {
        string line=ChronicleContent.LyraDialogue.Length==0 ? "Welcome to the atelier." : Main.rand.Next(ChronicleContent.LyraDialogue);
        return line.Replace("{player}",Main.LocalPlayer.name).Replace("{pronouns}",Main.LocalPlayer.GetModPlayer<ChroniclePlayer>().Pronouns);
    }
    public override void RegisterChatButtons(NPCInteractionList interactions)
    {
        interactions.Prepend(new LyraJournalInteraction());interactions.Prepend(new LyraStyleInteraction());
    }
    public override void FindFrame(int frameHeight)
    {
        NPC.frameCounter+=System.Math.Abs(NPC.velocity.X)*.15;
        NPC.frame.Y=(NPC.velocity.X==0 ? 0 : (int)NPC.frameCounter%4)*frameHeight;
    }
    public override void TownNPCAttackStrength(ref int damage,ref float knockback) {damage=20;knockback=3;}
    public override void TownNPCAttackCooldown(ref int cooldown,ref int randExtraCooldown) {cooldown=40;randExtraCooldown=15;}
    public override void TownNPCAttackProj(ref int projType,ref int attackDelay) {projType=ProjectileID.Spark;attackDelay=8;}
    public override void TownNPCAttackProjSpeed(ref float multiplier,ref float gravityCorrection,ref float randomOffset) {multiplier=8;randomOffset=1;}
}
public class LyraJournalInteraction : Terraria.GameContent.NPCInteraction
{
    public override bool Condition()=>true;
    public override string GetText()=>"Stories & quests";
    public override void Interact() {ChronicleInterface.Tab=0;ChronicleInterface.Open=true;Main.npcChatText="I keep our requests in the journal. We can add more stories as we travel.";}
}
public class LyraStyleInteraction : Terraria.GameContent.NPCInteraction
{
    public override bool Condition()=>true;
    public override string GetText()=>"Long hairstyles";
    public override void Interact()
    {
        int[] styles={ModContent.GetInstance<WaistlengthWaves>().Type,ModContent.GetInstance<SilkenCurtain>().Type,ModContent.GetInstance<TrailingPonytail>().Type,
            ModContent.GetInstance<LongSideBraid>().Type,ModContent.GetInstance<TwinRibbonBraids>().Type,ModContent.GetInstance<MoonlitCurls>().Type,
            ModContent.GetInstance<CascadingLayers>().Type,ModContent.GetInstance<HalfupRibbon>().Type};
        int current=System.Array.IndexOf(styles,Main.LocalPlayer.hair);
        Main.LocalPlayer.hair=styles[(current+1)%styles.Length];
        if(Main.netMode==NetmodeID.MultiplayerClient) NetMessage.SendData(MessageID.PlayerInfo,number:Main.myPlayer);
        Main.npcChatText="A new style, whenever you like. All eight are free to try.";
    }
}
