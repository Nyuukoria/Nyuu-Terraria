using System;
using Microsoft.Xna.Framework;
using Terraria;
using Terraria.Audio;
using Terraria.GameContent.ItemDropRules;
using Terraria.ID;
using Terraria.ModLoader;

namespace NyuuTerraria.Content.Hollowglass;

public abstract class ReachSummon : ModItem
{
    protected abstract int Boss { get; }

    public override void SetDefaults()
    {
        Item.width = 28;
        Item.height = 28;
        Item.useStyle = ItemUseStyleID.HoldUp;
        Item.useAnimation = Item.useTime = 30;
        Item.consumable = true;
        Item.maxStack = Item.CommonMaxStack;
        Item.rare = ItemRarityID.Green;
    }

    public override bool CanUseItem(Player player) =>
        player.InModBiome<HollowglassReachBiome>() && !NPC.AnyNPCs(ModContent.NPCType<PrismSentinel>()) &&
        !NPC.AnyNPCs(ModContent.NPCType<MourningBell>());

    public override bool? UseItem(Player player)
    {
        if (player.whoAmI != Main.myPlayer)
            return true;
        SoundEngine.PlaySound(SoundID.Roar, player.Center);
        if (Main.netMode == NetmodeID.MultiplayerClient)
            NetMessage.SendData(MessageID.SpawnBossUseLicenseStartEvent, number: player.whoAmI, number2: Boss);
        else
            NPC.SpawnOnPlayer(player.whoAmI, Boss);
        return true;
    }
}

public class GlassboundSeal : ReachSummon
{
    protected override int Boss => ModContent.NPCType<PrismSentinel>();
    public override void AddRecipes() => CreateRecipe().AddIngredient<HollowglassBar>(4)
        .AddIngredient(ItemID.FallenStar, 2).AddTile(TileID.Anvils).Register();
}

public class ResonatingBell : ReachSummon
{
    protected override int Boss => ModContent.NPCType<MourningBell>();
    public override void AddRecipes() => CreateRecipe().AddIngredient<PrismHeart>(3)
        .AddIngredient<HollowglassBar>(5).AddIngredient(ItemID.Obsidian, 10).AddTile(TileID.Anvils).Register();
}

public abstract class ReachBoss : ModNPC
{
    protected abstract int Health { get; }
    protected abstract int Damage { get; }
    protected abstract int Reward { get; }

    public override void SetStaticDefaults() => NPCID.Sets.MPAllowedEnemies[Type] = true;

    public override void SetDefaults()
    {
        NPC.width = 106;
        NPC.height = 96;
        NPC.lifeMax = Health;
        NPC.damage = Damage;
        NPC.defense = 12;
        NPC.knockBackResist = 0f;
        NPC.noGravity = true;
        NPC.noTileCollide = true;
        NPC.boss = true;
        NPC.HitSound = SoundID.NPCHit5;
        NPC.DeathSound = SoundID.NPCDeath6;
        NPC.value = Item.buyPrice(gold: 3);
        if (!Main.dedServ)
            Music = MusicID.Boss2;
    }

    protected Player Target()
    {
        NPC.TargetClosest();
        Player player = Main.player[NPC.target];
        if (!player.active || player.dead || NPC.Distance(player.Center) > 3200f)
        {
            NPC.velocity.Y -= 0.18f;
            NPC.EncourageDespawn(30);
            return null;
        }
        return player;
    }

    protected void Hover(Vector2 position, float speed)
    {
        Vector2 difference = position - NPC.Center;
        Vector2 desired = difference.SafeNormalize(Vector2.Zero) * Math.Min(speed, difference.Length() * 0.08f);
        NPC.velocity = Vector2.Lerp(NPC.velocity, desired, 0.08f);
    }

    protected void Fire(Vector2 direction, float speed, int damage)
    {
        if (Main.netMode != NetmodeID.MultiplayerClient)
            Projectile.NewProjectile(NPC.GetSource_FromAI(), NPC.Center,
                direction * speed, ModContent.ProjectileType<ReachShard>(), damage, 0f, Main.myPlayer);
    }

    public override void ModifyNPCLoot(NPCLoot loot)
    {
        loot.Add(ItemDropRule.Common(Reward, 1, 7, 11));
        loot.Add(ItemDropRule.Common(ModContent.ItemType<HollowglassOre>(), 1, 12, 20));
    }
}

[AutoloadBossHead]
public class PrismSentinel : ReachBoss
{
    protected override int Health => 7200;
    protected override int Damage => 28;
    protected override int Reward => ModContent.ItemType<PrismHeart>();

    public override void AI()
    {
        Player player = Target();
        if (player == null)
            return;
        int tick = (int)++NPC.ai[0];
        int phase = tick % 240;
        bool angry = NPC.life < NPC.lifeMax / 2;
        NPC.defense = phase >= 160 ? 5 : 16;
        if (phase < 160)
        {
            Hover(player.Center + new Vector2(MathF.Sin(tick * 0.024f) * 230f, -145f), angry ? 9f : 7f);
            if (phase is 48 or 96 or 144)
            {
                Vector2 aim = (player.Center - NPC.Center).SafeNormalize(Vector2.UnitY);
                for (int i = -1; i <= 1; i++)
                    Fire(aim.RotatedBy(i * 0.22f), angry ? 8f : 6f, 18);
            }
        }
        else if (phase < 190)
        {
            NPC.velocity *= 0.88f;
            Lighting.AddLight(NPC.Center, 0.9f, 0.6f, 0.15f);
        }
        else if (phase == 190)
        {
            NPC.velocity = (player.Center - NPC.Center).SafeNormalize(Vector2.UnitY) * (angry ? 16f : 12f);
            NPC.netUpdate = true;
        }
        else
            NPC.velocity *= 0.98f;
    }
}

[AutoloadBossHead]
public class MourningBell : ReachBoss
{
    protected override int Health => 10400;
    protected override int Damage => 33;
    protected override int Reward => ModContent.ItemType<ResonantClapper>();

    public override void AI()
    {
        Player player = Target();
        if (player == null)
            return;
        int tick = (int)++NPC.ai[0];
        int cycle = NPC.life < NPC.lifeMax / 2 ? 200 : 260;
        int phase = tick % cycle;
        float side = tick / cycle % 2 == 0 ? -1f : 1f;
        Hover(player.Center + new Vector2(side * 210f, -145f), 7f);
        if (phase < 45)
        {
            NPC.rotation = MathF.Sin(tick * 0.35f) * 0.10f;
            Lighting.AddLight(NPC.Center, 0.65f, 0.2f, 0.9f);
        }
        else
            NPC.rotation *= 0.9f;

        if (phase is 45 or 105)
        {
            float offset = phase == 45 ? 0f : MathHelper.Pi / 12f;
            for (int i = 0; i < 12; i++)
                Fire(Vector2.UnitX.RotatedBy(MathHelper.TwoPi * i / 12f + offset), 5f, 21);
        }
        if (phase is 155 or 185)
        {
            Vector2 aim = (player.Center - NPC.Center).SafeNormalize(Vector2.UnitY);
            Fire(aim, 9f, 23);
        }
    }
}

public class ReachShard : ModProjectile
{
    public override void SetDefaults()
    {
        Projectile.width = 12;
        Projectile.height = 12;
        Projectile.hostile = true;
        Projectile.tileCollide = false;
        Projectile.timeLeft = 170;
    }

    public override void AI()
    {
        Projectile.rotation = Projectile.velocity.ToRotation();
        Lighting.AddLight(Projectile.Center, 0.12f, 0.45f, 0.5f);
    }
}

public class PrismHeart : ModItem
{
    public override void SetDefaults()
    {
        Item.width = 24;
        Item.height = 24;
        Item.maxStack = Item.CommonMaxStack;
        Item.rare = ItemRarityID.Green;
    }
}

public class ResonantClapper : PrismHeart { }
