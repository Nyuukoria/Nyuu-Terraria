using System;
using Microsoft.Xna.Framework;
using Terraria;
using Terraria.DataStructures;
using Terraria.GameContent;
using Terraria.GameContent.ItemDropRules;
using Terraria.ID;
using Terraria.ModLoader;
using Terraria.Audio;
using Microsoft.Xna.Framework.Graphics;

namespace NyuuTerraria.Content.Skylight;

public class CloudIdol : ModItem
{
    public override void SetDefaults()
    {
        Item.width = 24;
        Item.height = 26;
        Item.maxStack = Item.CommonMaxStack;
        Item.consumable = true;
        Item.useStyle = ItemUseStyleID.HoldUp;
        Item.useTime = Item.useAnimation = 30;
        Item.rare = ItemRarityID.Blue;
    }

    public override bool CanUseItem(Player player) =>
        (player.ZoneOverworldHeight || player.ZoneSkyHeight) && !NPC.AnyNPCs(ModContent.NPCType<NimbusWarden>());

    public override bool? UseItem(Player player)
    {
        if (player.whoAmI != Main.myPlayer)
            return true;

        int boss = ModContent.NPCType<NimbusWarden>();
        SoundEngine.PlaySound(SoundID.Roar, player.Center);
        if (Main.netMode == NetmodeID.MultiplayerClient)
            NetMessage.SendData(MessageID.SpawnBossUseLicenseStartEvent, number: player.whoAmI, number2: boss);
        else
            NPC.SpawnOnPlayer(player.whoAmI, boss);

        return true;
    }

    public override void AddRecipes() => CreateRecipe().AddIngredient(ItemID.Cloud, 30)
        .AddIngredient<SkylightBar>(3).AddTile(TileID.Anvils).Register();
}

[AutoloadBossHead]
public class NimbusWarden : ModNPC
{
    public override void SetStaticDefaults()
    {
        NPCID.Sets.MPAllowedEnemies[Type] = true;
    }

    public override void SetDefaults()
    {
        NPC.width = 126;
        NPC.height = 88;
        NPC.lifeMax = 5600;
        NPC.damage = 26;
        NPC.defense = 9;
        NPC.knockBackResist = 0f;
        NPC.noGravity = true;
        NPC.noTileCollide = true;
        NPC.boss = true;
        NPC.value = Item.buyPrice(gold: 2);
        NPC.HitSound = SoundID.NPCHit5;
        NPC.DeathSound = SoundID.NPCDeath6;
        if (!Main.dedServ)
            Music = MusicID.Boss1;
    }

    public override void AI()
    {
        NPC.TargetClosest();
        Player player = Main.player[NPC.target];
        if (!player.active || player.dead || NPC.Distance(player.Center) > 3000f)
        {
            NPC.velocity.Y -= 0.3f;
            NPC.EncourageDespawn(30);
            return;
        }

        bool enraged = NPC.life * 2 < NPC.lifeMax;
        int cycle = enraged ? 145 : 190;
        int tick = (int)++NPC.ai[0];
        int phase = tick % cycle;
        float wave = MathF.Sin(tick * 0.032f);
        if (phase < 110)
        {
            Vector2 destination = player.Center + new Vector2(250f * wave, -190f);
            Vector2 desired = (destination - NPC.Center) * 0.09f;
            float maxSpeed = enraged ? 11f : 8f;
            if (desired.LengthSquared() > maxSpeed * maxSpeed)
                desired = desired.SafeNormalize(Vector2.Zero) * maxSpeed;
            NPC.velocity = Vector2.Lerp(NPC.velocity, desired, 0.10f);
        }
        else if (phase == 110)
        {
            NPC.velocity = (player.Center - NPC.Center).SafeNormalize(Vector2.UnitY) * (enraged ? 17f : 13f);
            NPC.netUpdate = true;
        }
        else
        {
            NPC.velocity *= 0.97f;
        }

        if ((phase == 42 || phase == 85) && Main.netMode != NetmodeID.MultiplayerClient)
        {
            Vector2 aim = (player.Center - NPC.Center).SafeNormalize(Vector2.UnitY);
            int spread = enraged ? 2 : 1;
            for (int i = -spread; i <= spread; i++)
                Projectile.NewProjectile(NPC.GetSource_FromAI(), NPC.Center, aim.RotatedBy(i * 0.19f) * 7.5f,
                    ModContent.ProjectileType<WardenSpark>(), 18, 0f, Main.myPlayer);
        }

        Lighting.AddLight(NPC.Center, 0.6f, 0.2f, 0.45f);
    }

    public override void ModifyNPCLoot(NPCLoot loot) =>
        loot.Add(ItemDropRule.Common(ModContent.ItemType<WardenCore>(), 1, 9, 14));
}

public class WardenCore : ModItem
{
    public override void SetDefaults()
    {
        Item.width = 22;
        Item.height = 22;
        Item.maxStack = Item.CommonMaxStack;
        Item.rare = ItemRarityID.Green;
        Item.value = Item.sellPrice(silver: 12);
    }
}

public class WardenSpark : ModProjectile
{
    public override void SetDefaults()
    {
        Projectile.width = 14;
        Projectile.height = 14;
        Projectile.hostile = true;
        Projectile.tileCollide = false;
        Projectile.timeLeft = 110;
    }

    public override void AI()
    {
        Projectile.rotation = Projectile.velocity.ToRotation();
        if (!Main.dedServ)
        {
            Lighting.AddLight(Projectile.Center, 0.7f, 0.22f, 0.55f);
            if (Main.GameUpdateCount % 2 == 0)
                Dust.NewDustPerfect(Projectile.Center, DustID.PinkTorch, Vector2.Zero, Scale: 0.75f).noGravity = true;
        }
    }
}
