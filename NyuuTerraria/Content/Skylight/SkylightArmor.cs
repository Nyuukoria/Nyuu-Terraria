using Terraria;
using Terraria.ID;
using Terraria.ModLoader;

namespace NyuuTerraria.Content.Skylight;

public abstract class WardenArmor : ModItem
{
    protected abstract int Defense { get; }
    protected abstract int BarCost { get; }

    public override void SetDefaults()
    {
        Item.width = 28;
        Item.height = 24;
        Item.defense = Defense;
        Item.rare = ItemRarityID.Green;
        Item.value = Item.sellPrice(silver: 65);
    }

    public override void AddRecipes() => CreateRecipe().AddIngredient<SkylightBar>(BarCost)
        .AddIngredient<WardenCore>(2).AddTile(TileID.Anvils).Register();
}

[AutoloadEquip(EquipType.Head)]
public class WardenCrown : WardenArmor
{
    protected override int Defense => 4;
    protected override int BarCost => 5;

    public override void UpdateEquip(Player player) => player.statManaMax2 += 20;

    public override void SetStaticDefaults()
    {
        AddArmorSet<WardenCrown, WardenMantle, WardenGreaves>(
            Effect: static player =>
            {
                player.manaCost -= 0.12f;
                player.GetCritChance(DamageClass.Magic) += 6f;
            });
    }
}

[AutoloadEquip(EquipType.Body)]
public class WardenMantle : WardenArmor
{
    protected override int Defense => 6;
    protected override int BarCost => 8;

    public override void UpdateEquip(Player player) => player.GetDamage(DamageClass.Magic) += 0.06f;
}

[AutoloadEquip(EquipType.Legs)]
public class WardenGreaves : WardenArmor
{
    protected override int Defense => 5;
    protected override int BarCost => 6;

    public override void UpdateEquip(Player player) => player.moveSpeed += 0.08f;
}

public abstract class CloudVanity : ModItem
{
    public override void SetDefaults()
    {
        Item.width = 26;
        Item.height = 24;
        Item.vanity = true;
        Item.rare = ItemRarityID.Blue;
    }

    public override void AddRecipes() => CreateRecipe().AddIngredient(ItemID.Silk, 5)
        .AddIngredient(ItemID.Cloud, 3).AddTile(TileID.Loom).Register();
}

[AutoloadEquip(EquipType.Head)]
public class CloudBeret : CloudVanity { }

[AutoloadEquip(EquipType.Body)]
public class CloudtailCoat : CloudVanity { }

[AutoloadEquip(EquipType.Legs)]
public class CloudtailTrousers : CloudVanity { }

public class PetalBob : ModHair
{
    public override bool AvailableDuringCharacterCreation => true;
    public override string AltTexture => Texture + "_Alt";
}

public class StormTuft : ModHair
{
    public override bool AvailableDuringCharacterCreation => true;
    public override string AltTexture => Texture + "_Alt";
}
