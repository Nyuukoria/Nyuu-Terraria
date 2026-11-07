using System;
using System.Collections.Generic;
using Microsoft.Xna.Framework;
using Terraria;
using Terraria.Audio;
using Terraria.DataStructures;
using Terraria.ID;
using Terraria.ModLoader;
using NyuuTerraria.Content.Reverie;
using NyuuTerraria.Content.Skylight;
using NyuuTerraria.Content.AmberGrotto;

namespace NyuuTerraria.Content.Spellcraft;

public class AetherloomWand : ModItem
{
    public override void SetStaticDefaults()=>Item.staff[Type]=true;
    public override void SetDefaults()
    {
        Item.width=46;Item.height=48;Item.damage=36;Item.DamageType=DamageClass.Magic;Item.mana=4;
        Item.useTime=Item.useAnimation=12;Item.useStyle=ItemUseStyleID.Shoot;Item.noMelee=true;Item.autoReuse=true;
        Item.knockBack=4;Item.crit=16;Item.shoot=ModContent.ProjectileType<SpellShot>();Item.shootSpeed=13;
        Item.rare=ItemRarityID.Green;Item.value=Item.sellPrice(gold:1);Item.UseSound=SoundID.Item20;
    }
    public override bool AltFunctionUse(Player player)=>true;
    public override bool CanUseItem(Player player)
    {
        var data=player.GetModPlayer<SpellcraftPlayer>();
        if(!data.Learned[data.SelectedSpell]) data.SelectedSpell=Spells.Arcane;
        bool ward=player.altFunctionUse==2;int spell=data.SelectedSpell;
        Item.channel=ward || spell==Spells.Ember;
        if(player.ownedProjectileCounts[ModContent.ProjectileType<EmberCharge>()]>0 ||
            player.ownedProjectileCounts[ModContent.ProjectileType<CrescentWard>()]>0) return false;
        return ward || spell!=Spells.Mend || (data.MendCooldown==0 && player.statLife<player.statLifeMax2);
    }
    public override void ModifyManaCost(Player player,ref float reduce,ref float mult)
    {
        var data=player.GetModPlayer<SpellcraftPlayer>();
        mult*=(player.altFunctionUse==2 ? 2 : Spells.Mana[data.SelectedSpell])/4f;
    }
    public override float UseTimeMultiplier(Player player)=>
        (player.altFunctionUse==2 ? 20 : Spells.UseTime[player.GetModPlayer<SpellcraftPlayer>().SelectedSpell])/12f;
    public override float UseAnimationMultiplier(Player player)=>UseTimeMultiplier(player);
    public override void ModifyShootStats(Player player,ref Vector2 position,ref Vector2 velocity,ref int type,ref int damage,ref float knockback)
    {
        damage=(int)MathF.Round(damage*Spells.Damage[player.GetModPlayer<SpellcraftPlayer>().SelectedSpell]/36f);
    }
    public override void ModifyWeaponDamage(Player player,ref StatModifier damage)
    {
        var data=player.GetModPlayer<SpellcraftPlayer>();damage+=data.EffectiveLevel(data.SelectedSpell)*.03f;
    }
    public override bool Shoot(Player player,EntitySource_ItemUse_WithAmmo source,Vector2 position,Vector2 velocity,int type,int damage,float knockback)
    {
        if(player.whoAmI!=Main.myPlayer) return false;
        var data=player.GetModPlayer<SpellcraftPlayer>();int spell=data.SelectedSpell,level=data.EffectiveLevel(spell);
        Vector2 aim=velocity.SafeNormalize(new Vector2(player.direction,0));
        if(player.altFunctionUse==2)
        {
            Projectile.NewProjectile(source,player.Center,aim,ModContent.ProjectileType<CrescentWard>(),0,0,player.whoAmI,data.EffectiveLevel(Spells.Ward));
            return false;
        }
        if(spell==Spells.Mend)
        {
            if(data.TryMend())
            {
                SoundEngine.PlaySound(SoundID.Item4,player.Center);
                Projectile.NewProjectile(source,player.Center,Vector2.Zero,ModContent.ProjectileType<MendingBloom>(),0,0,player.whoAmI);
            }
            return false;
        }
        if(spell==Spells.Ember)
        {
            Projectile.NewProjectile(source,player.Center+aim*48,aim,ModContent.ProjectileType<EmberCharge>(),damage,knockback,player.whoAmI,0,level);
            return false;
        }
        int shots=spell==Spells.Frost && level>=5 ? 3 : 1;
        for(int i=0;i<shots;i++)
        {
            Vector2 direction=velocity.RotatedBy((i-(shots-1)*.5f)*.13f);
            Projectile.NewProjectile(source,position,direction,ModContent.ProjectileType<SpellShot>(),shots>1 ? (int)(damage*.7f) : damage,knockback,player.whoAmI,spell,level);
        }
        SpellVisuals.CastSigil(source,player.Center+aim*42,spell,player.whoAmI);
        return false;
    }
    public override void ModifyTooltips(List<TooltipLine> tooltips)
    {
        var player=Main.LocalPlayer;var data=player.GetModPlayer<SpellcraftPlayer>();int spell=data.SelectedSpell;
        foreach(var line in tooltips)
        {
            if(line.Mod=="Terraria" && line.Name=="Damage")
                line.Text=spell==Spells.Mend ? "Healing spell" : $"{(int)MathF.Round(player.GetWeaponDamage(Item)*Spells.Damage[spell]/36f)} magic damage";
            if(line.Mod=="Terraria" && line.Name=="UseMana") line.Text=$"Uses {player.GetManaCost(Item)} mana";
        }
        tooltips.Add(new TooltipLine(Mod,"SelectedSpell",$"Selected: {Spells.Names[spell]} (mastery {data.EffectiveLevel(spell)})"));
    }
    public override void AddRecipes()=>CreateRecipe().AddIngredient<SkylightBar>(3).AddIngredient(ItemID.Wood,8)
        .AddIngredient(ItemID.FallenStar,2).AddTile(TileID.Anvils).Register();
}

public abstract class LearningTome : ModItem
{
    protected abstract int Spell {get;}
    public override void SetDefaults()
    {
        Item.width=28;Item.height=32;Item.maxStack=99;Item.consumable=true;Item.useStyle=ItemUseStyleID.HoldUp;
        Item.useTime=Item.useAnimation=30;Item.rare=ItemRarityID.Green;Item.UseSound=SoundID.Item4;
    }
    public override bool CanUseItem(Player player)=>!player.GetModPlayer<SpellcraftPlayer>().Learned[Spell];
    public override bool? UseItem(Player player)
    {
        if(player.whoAmI==Main.myPlayer && player.GetModPlayer<SpellcraftPlayer>().Learn(Spell))
            Main.NewText("Learned "+Spells.Names[Spell]+". Press K to choose a spell.",Spells.Colors[Spell]);
        return true;
    }
}
public class EmberbloomTome : LearningTome
{
    protected override int Spell=>Spells.Ember;
    public override void AddRecipes()=>CreateRecipe().AddIngredient(ItemID.Book).AddIngredient<EmberAmberBar>(2)
        .AddIngredient(ItemID.Torch,10).AddTile(TileID.Anvils).Register();
}
public class FrostbloomTome : LearningTome
{
    protected override int Spell=>Spells.Frost;
    public override void AddRecipes()=>CreateRecipe().AddIngredient(ItemID.Book).AddIngredient<FrostveilBar>(3)
        .AddIngredient(ItemID.Shiverthorn,5).AddTile(TileID.Anvils).Register();
}
public class TideglassTome : LearningTome
{
    protected override int Spell=>Spells.Tide;
    public override void AddRecipes()=>CreateRecipe().AddIngredient(ItemID.Book).AddIngredient<MoonprismBar>(3)
        .AddIngredient(ItemID.Coral,5).AddIngredient(ItemID.BottledWater,3).AddTile(TileID.Anvils).Register();
}
public class DawnmendTome : LearningTome
{
    protected override int Spell=>Spells.Mend;
    public override void AddRecipes()=>CreateRecipe().AddIngredient(ItemID.Book).AddIngredient<MoonprismBar>(2)
        .AddIngredient(ItemID.Daybloom,5).AddIngredient(ItemID.LesserHealingPotion,3).AddTile(TileID.WorkBenches).Register();
}
public class StormglassTome : LearningTome
{
    protected override int Spell=>Spells.Storm;
    public override void AddRecipes()=>CreateRecipe().AddIngredient(ItemID.Book).AddIngredient<StormglassBar>(4)
        .AddIngredient(ItemID.FallenStar,3).AddIngredient(ItemID.Lens).AddTile(TileID.Anvils).Register();
}
public class NyuuSpellbook : ModItem
{
    public override void SetDefaults() {Item.width=32;Item.height=28;Item.useStyle=ItemUseStyleID.HoldUp;Item.useTime=Item.useAnimation=20;Item.rare=ItemRarityID.Blue;}
    public override bool? UseItem(Player player)
    {
        if(player.whoAmI==Main.myPlayer) {ChronicleInterface.Tab=3;ChronicleInterface.Open=true;}return true;
    }
    public override void AddRecipes()=>CreateRecipe().AddIngredient(ItemID.Book).AddIngredient(ItemID.Silk,2)
        .AddIngredient(ItemID.FallenStar,2).AddTile(TileID.WorkBenches).Register();
}
public class WardSigil : ModItem
{
    public override void SetDefaults() {Item.width=26;Item.height=28;Item.accessory=true;Item.rare=ItemRarityID.Green;Item.value=Item.sellPrice(silver:70);}
    public override void UpdateAccessory(Player player,bool hideVisual)
    {
        player.statManaMax2+=20;player.GetModPlayer<SpellcraftPlayer>().EfficientWard=true;
    }
    public override void AddRecipes()=>CreateRecipe().AddIngredient<MoonprismBar>(3).AddIngredient(ItemID.Lens,2)
        .AddIngredient(ItemID.Silk,4).AddTile(TileID.Anvils).Register();
}

