using System;
using System.Collections.Generic;
using System.Linq;
using Microsoft.Xna.Framework;
using Terraria;
using Terraria.ID;
using Terraria.ModLoader;
using Terraria.ModLoader.IO;
using Terraria.DataStructures;
using Terraria.GameInput;
using NyuuTerraria.Content.Reverie;
using NyuuTerraria.Content.Spellcraft;

namespace NyuuTerraria.Content.Frontiers;

public class FrontierPlayer : ModPlayer
{
    internal HashSet<string> Discoveries=new();
    internal int[] Talents=new int[15];
    internal string Identity="Adventurer",Story="A new chapter",Tracked="";
    internal int Palette,AbilityCooldown,InfusionTicks,Infusion;
    internal Vector2 ReturnPosition;internal string ReturnWorld="";
    private int ticks;
    internal static readonly string[] TalentNames={"Bulwark","Tempo","Second wind","Precision","Conservation","Skirmish","Focus","Reservoir","Overcharge","Bond","Command","Shelter","Trailblazer","Prospector","Forager"};
    internal static readonly string[] TalentDescriptions={"+2 defense per rank","+2% melee speed per rank","G: 5 seconds of regeneration","+1 ranged crit per rank","5% ammo conservation per rank","G: 5 seconds of swiftness","+1 magic crit per rank","+10 mana per rank","G: 5 seconds of magic power","+2% summon damage per rank","+1 minion at rank 3","G: 5 seconds of ironskin","+2% move speed per rank","+2% mining speed per rank","G: 5 seconds of spelunker"};
    public override void Initialize(){Discoveries=new();Talents=new int[15];}
    internal bool Discover(string id)
    {
        if(!Discoveries.Add(id))return false;
        Player.GetModPlayer<ChroniclePlayer>().Record("discover-"+id);
        Player.GetModPlayer<ChroniclePlayer>().AddExperience(35);
        if(!Main.dedServ&&Player.whoAmI==Main.myPlayer)Main.NewText("Discovery: "+id.Replace('-',' '),new Color(170,225,224));
        return true;
    }
    internal bool Invest(int index)
    {
        if(index<0||index>=15||Talents[index]>=3)return false;
        var d=Player.GetModPlayer<ChroniclePlayer>();int branch=index/3;
        if(d.Level(branch)/3-Talents.Skip(branch*3).Take(3).Sum()<=0)return false;
        Talents[index]++;Sync();return true;
    }
    internal void ResetBranch(int branch){if(branch<0||branch>=5)return;Array.Clear(Talents,branch*3,3);Sync();}
    public override void PostUpdateEquips()
    {
        int b=Player.GetModPlayer<ChroniclePlayer>().Profession,i=b*3;
        switch(b)
        {
            case 0:Player.statDefense+=Talents[i]*2;Player.GetAttackSpeed(DamageClass.Melee)+=Talents[i+1]*.02f;break;
            case 1:Player.GetCritChance(DamageClass.Ranged)+=Talents[i];break;
            case 2:Player.GetCritChance(DamageClass.Magic)+=Talents[i];Player.statManaMax2+=Talents[i+1]*10;break;
            case 3:Player.GetDamage(DamageClass.Summon)+=Talents[i]*.02f;if(Talents[i+1]>=3)Player.maxMinions++;break;
            case 4:Player.moveSpeed+=Talents[i]*.02f;Player.pickSpeed-=Talents[i+1]*.02f;break;
        }
        if(InfusionTicks>0){if(Infusion==0)Player.manaRegenBonus+=5;else if(Infusion==1)Player.lifeRegen+=2;else Player.GetCritChance(DamageClass.Magic)+=4;}
    }
    public override bool CanConsumeAmmo(Item weapon,Item ammo)=>!(Player.GetModPlayer<ChroniclePlayer>().Profession==1&&Main.rand.NextFloat()<Talents[4]*.05f);
    internal bool Activate()
    {
        int b=Player.GetModPlayer<ChroniclePlayer>().Profession;
        if(AbilityCooldown>0||Talents[b*3+2]==0||Player.dead)return false;
        int[] buffs={BuffID.Regeneration,BuffID.Swiftness,BuffID.MagicPower,BuffID.Ironskin,BuffID.Spelunker};
        Player.AddBuff(buffs[b],300);AbilityCooldown=1800;return true;
    }
    public override void ProcessTriggers(TriggersSet triggersSet){if(FrontierKeys.Ability?.JustPressed==true&&!Main.drawingPlayerChat&&!Main.editSign&&!Main.editChest)Activate();}
    public override void PostUpdate()
    {
        if(AbilityCooldown>0)AbilityCooldown--;if(InfusionTicks>0)InfusionTicks--;
        if(Player.whoAmI!=Main.myPlayer||Player.dead)return;
        if(++ticks%90!=0)return;
        int area=FrontierWorld.AreaAt(Player.Center);if(area>=0)Discover("visit-"+FrontierWorld.Keys[area]);
        foreach(NPC n in Main.ActiveNPCs)
            if(n.ModNPC is PeacefulExpeditionCreature&&Player.Distance(n.Center)<220&&Collision.CanHitLine(Player.Center,1,1,n.Center,1,1))Discover("wildlife-"+n.ModNPC.Name);
    }
    public override void Kill(double damage,int hitDirection,bool pvp,PlayerDeathReason damageSource)
    {
        if(Main.netMode==NetmodeID.MultiplayerClient||pvp)return;
        int area=FrontierWorld.AreaAt(Player.Center);
        if(area<0)return;
        int item=area==0?ModContent.ItemType<TidalMemorial>():area==2?ModContent.ItemType<CinderMemorial>():ModContent.ItemType<StarglassMemorial>();
        Item.NewItem(Player.GetSource_Death(),Player.Hitbox,item);
    }
    internal void Sync(int to=-1,int ignore=-1)
    {
        if(Main.netMode==NetmodeID.SinglePlayer)return;
        var p=Mod.GetPacket();p.Write((byte)5);p.Write((byte)Player.whoAmI);foreach(int t in Talents)p.Write((byte)t);p.Send(to,ignore);
    }
    public override void SyncPlayer(int toWho,int fromWho,bool newPlayer)=>Sync(toWho,fromWho);
    public override void SaveData(TagCompound tag)
    {
        tag["frontierDiscoveries"]=Discoveries.ToList();tag["frontierTalents"]=Talents;tag["frontierIdentity"]=Identity;tag["frontierStory"]=Story;tag["frontierTracked"]=Tracked;tag["frontierPalette"]=Palette;
        tag["frontierAbility"]=AbilityCooldown;tag["frontierInfusionTicks"]=InfusionTicks;tag["frontierInfusion"]=Infusion;tag["frontierReturn"]=ReturnPosition;tag["frontierWorld"]=ReturnWorld;
    }
    public override void LoadData(TagCompound tag)
    {
        Initialize();Discoveries=tag.GetList<string>("frontierDiscoveries").ToHashSet();int[] a=tag.GetIntArray("frontierTalents");
        for(int i=0;i<15;i++)Talents[i]=i<a.Length?Math.Clamp(a[i],0,3):0;
        Identity=tag.ContainsKey("frontierIdentity")?tag.GetString("frontierIdentity"):"Adventurer";Story=tag.ContainsKey("frontierStory")?tag.GetString("frontierStory"):"A new chapter";Tracked=tag.GetString("frontierTracked");Palette=Math.Clamp(tag.GetInt("frontierPalette"),0,3);
        AbilityCooldown=Math.Clamp(tag.GetInt("frontierAbility"),0,1800);InfusionTicks=Math.Clamp(tag.GetInt("frontierInfusionTicks"),0,10800);Infusion=Math.Clamp(tag.GetInt("frontierInfusion"),0,2);ReturnPosition=tag.Get<Vector2>("frontierReturn");ReturnWorld=tag.GetString("frontierWorld");
    }
}
public class FrontierKeys : ModSystem
{
    internal static ModKeybind Ability;
    public override void Load(){if(!Main.dedServ)Ability=KeybindLoader.RegisterKeybind(Mod,"ProfessionAbility","G");}
    public override void Unload()=>Ability=null;
}
