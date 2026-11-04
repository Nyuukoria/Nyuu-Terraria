using Microsoft.Xna.Framework;
using Terraria;
using Terraria.DataStructures;
using Terraria.ID;
using Terraria.ModLoader;

namespace NyuuTerraria.Content.Skylight;

public abstract class SkylightStaff : ModItem
{
    protected abstract int Damage { get; }
    protected abstract int Mana { get; }
    protected abstract int CastTime { get; }
    protected abstract int Spell { get; }

    public override void SetStaticDefaults() => Item.staff[Type] = true;

    public override void SetDefaults()
    {
        Item.width = 44;
        Item.height = 46;
        Item.damage = Damage;
        Item.mana = Mana;
        Item.DamageType = DamageClass.Magic;
        Item.useStyle = ItemUseStyleID.Shoot;
        Item.useTime = Item.useAnimation = CastTime;
        Item.noMelee = true;
        Item.autoReuse = true;
        Item.knockBack = 3.5f;
        Item.shoot = Spell;
        Item.shootSpeed = 10f;
        Item.UseSound = SoundID.Item43;
        Item.rare = ItemRarityID.Blue;
        Item.value = Item.sellPrice(silver: 75);
    }
}

public class DawnpetalStaff : SkylightStaff
{
    protected override int Damage => 17;
    protected override int Mana => 3;
    protected override int CastTime => 18;
    protected override int Spell => ModContent.ProjectileType<DawnpetalBolt>();

    public override void AddRecipes() => CreateRecipe().AddIngredient<SkylightBar>(5)
        .AddIngredient(ItemID.Daybloom, 3).AddIngredient(ItemID.Wood, 8).AddTile(TileID.Anvils).Register();
}

public class StormweaverStaff : SkylightStaff
{
    protected override int Damage => 22;
    protected override int Mana => 6;
    protected override int CastTime => 24;
    protected override int Spell => ModContent.ProjectileType<StormweaverBolt>();

    public override bool Shoot(Player player, EntitySource_ItemUse_WithAmmo source,
        Vector2 position, Vector2 velocity, int type, int damage, float knockback)
    {
        for (int i = -1; i <= 1; i++)
            Projectile.NewProjectile(source, position, velocity.RotatedBy(i * 0.14f),
                type, damage, knockback, player.whoAmI);

        return false;
    }

    public override void AddRecipes() => CreateRecipe().AddIngredient<SkylightBar>(7)
        .AddIngredient<WardenCore>(4).AddIngredient(ItemID.Cloud, 15).AddTile(TileID.Anvils).Register();
}

public abstract class SkylightSpell : ModProjectile
{
    protected abstract Color Tint { get; }
    protected virtual int Hits => 1;

    public override void SetDefaults()
    {
        Projectile.width = 14;
        Projectile.height = 14;
        Projectile.friendly = true;
        Projectile.DamageType = DamageClass.Magic;
        Projectile.penetrate = Hits;
        Projectile.usesLocalNPCImmunity = true;
        Projectile.localNPCHitCooldown = -1;
        Projectile.timeLeft = 105;
    }

    public override Color? GetAlpha(Color lightColor) => Color.White;

    public override void AI()
    {
        Projectile.rotation = Projectile.velocity.ToRotation();
        if (Main.dedServ)
            return;

        Lighting.AddLight(Projectile.Center, Tint.ToVector3() * 0.58f);
        Vector2 side = Projectile.velocity.SafeNormalize(Vector2.UnitX).RotatedBy(MathHelper.PiOver2);
        float ripple = (float)System.Math.Sin(Projectile.timeLeft * 0.45) * 4f;
        Spark(Projectile.Center - Projectile.velocity * 0.5f + side * ripple, Vector2.Zero, 0.78f);
        Spark(Projectile.Center - Projectile.velocity * 0.5f - side * ripple, Vector2.Zero, 0.78f);
    }

    public override void OnKill(int timeLeft)
    {
        if (Main.dedServ)
            return;

        for (int i = 0; i < 14; i++)
        {
            Vector2 radial = Vector2.UnitX.RotatedBy(MathHelper.TwoPi * i / 14f);
            Spark(Projectile.Center + radial * 5f, radial * 2.5f, 1.1f);
        }
    }

    private void Spark(Vector2 position, Vector2 velocity, float scale) =>
        Dust.NewDustPerfect(position, DustID.TintableDustLighted, velocity, 0, Tint, scale).noGravity = true;
}

public class DawnpetalBolt : SkylightSpell
{
    protected override Color Tint => new Color(255, 170, 118);

    public override void OnHitNPC(NPC target, NPC.HitInfo hit, int damageDone) =>
        target.AddBuff(BuffID.OnFire, 120);
}

public class StormweaverBolt : SkylightSpell
{
    protected override Color Tint => new Color(164, 150, 255);
    protected override int Hits => 2;
}

public class SkylightSaber : ModItem
{
    public override void SetDefaults()
    {
        Item.width = 44;
        Item.height = 46;
        Item.damage = 21;
        Item.DamageType = DamageClass.Melee;
        Item.useStyle = ItemUseStyleID.Swing;
        Item.useTime = Item.useAnimation = 19;
        Item.knockBack = 5f;
        Item.autoReuse = true;
        Item.UseSound = SoundID.Item1;
        Item.rare = ItemRarityID.Blue;
        Item.value = Item.sellPrice(silver: 70);
    }

    public override void MeleeEffects(Player player, Rectangle hitbox)
    {
        if (!Main.dedServ)
            Dust.NewDust(hitbox.TopLeft(), hitbox.Width, hitbox.Height, DustID.Enchanted_Gold);
    }

    public override void OnHitNPC(Player player, NPC target, NPC.HitInfo hit, int damageDone)
    {
        target.AddBuff(BuffID.OnFire, 90);
        if (!Main.dedServ)
            for (int i = 0; i < 10; i++)
                Dust.NewDustPerfect(target.Center, DustID.Enchanted_Gold,
                    Vector2.UnitX.RotatedBy(MathHelper.TwoPi * i / 10f) * 2.4f).noGravity = true;
    }

    public override void AddRecipes() => CreateRecipe().AddIngredient<SkylightBar>(7)
        .AddIngredient(ItemID.FallenStar, 1).AddTile(TileID.Anvils).Register();
}
