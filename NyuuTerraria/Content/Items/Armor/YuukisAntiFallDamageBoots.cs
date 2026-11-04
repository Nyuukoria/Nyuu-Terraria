using System.Linq;
using Microsoft.Xna.Framework;
using NyuuTerraria.Content.Rarities;
using Terraria;
using Terraria.DataStructures;
using Terraria.ID;
using Terraria.ModLoader;

namespace NyuuTerraria.Content.Items.Armor;

public class YuukisAntiFallDamageBoots : ModItem
{
    public override void SetDefaults()
    {
        Item.width = 40;
        Item.height = 42;
        Item greaves = new Item(ItemID.ShadowGreaves);
        Item.defense = greaves.defense;
        Item.legSlot = greaves.legSlot;
        Item.rare = ModContent.RarityType<DemocloudRarity>();
        Item.value = Item.sellPrice(silver: 90);
    }

    public override void SetStaticDefaults()
    {
        var shadowSets = ArmorSetBonuses.All
            .Where(set => set.Legs is ItemID.ShadowGreaves or ItemID.AncientShadowGreaves)
            .ToArray();

        foreach (var set in shadowSets)
            AddArmorSet(set.Head, set.Body, Type, set.Description,
                set.Identifier, set.Effect, set.PrimaryPart);
    }

    public override void UpdateEquip(Player player)
    {
        player.noFallDmg = true;
        player.GetCritChance(DamageClass.Generic) += 5f;

        if (Main.dedServ || Main.GameUpdateCount % 3 != 0 ||
            player.velocity.LengthSquared() < 0.35f)
            return;

        Vector2 gravity = new Vector2(0f, player.gravDir);
        Vector2 feet = player.Center + gravity * (player.height / 2f - 3f);
        Vector2 drift = new Vector2(-player.velocity.X * 0.20f, 0f) - gravity * 0.45f;
        for (int i = 0; i < 3; i++)
        {
            Vector2 offset = new Vector2(Main.rand.NextFloat(-7f, 7f), Main.rand.NextFloat(-2f, 2f));
            Color tint = i == 0 ? new Color(193, 158, 244) : new Color(235, 225, 255);
            Dust cloud = Dust.NewDustPerfect(feet + offset, DustID.Cloud, drift, 25, tint,
                Main.rand.NextFloat(0.95f, 1.35f));
            cloud.noGravity = true;
        }
    }

    public override void AddRecipes()
    {
        CreateRecipe()
            .AddIngredient(ItemID.DemoniteBar, 15)
            .AddIngredient(ItemID.ShadowScale, 10)
            .AddIngredient(ItemID.Cloud, 10)
            .AddTile(TileID.Anvils)
            .Register();
    }
}
