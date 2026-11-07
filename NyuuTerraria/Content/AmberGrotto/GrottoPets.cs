using Microsoft.Xna.Framework;
using Terraria;
using Terraria.ID;
using Terraria.ModLoader;

namespace NyuuTerraria.Content.AmberGrotto;

public abstract class GrottoPetItem : ModItem
{
    protected abstract int Pet { get; }
    protected abstract int Buff { get; }

    public override void SetDefaults()
    {
        Item.CloneDefaults(ItemID.ZephyrFish);
        Item.shoot = Pet;
        Item.buffType = Buff;
    }

    public override bool? UseItem(Player player)
    {
        if (player.whoAmI == Main.myPlayer)
            player.AddBuff(Item.buffType, 3600);
        return true;
    }
}

public class GlowcapSpore : GrottoPetItem
{
    protected override int Pet => ModContent.ProjectileType<GlowcapFirefly>();
    protected override int Buff => ModContent.BuffType<GlowcapCompanion>();
}

public class AmberWispSeed : GrottoPetItem
{
    protected override int Pet => ModContent.ProjectileType<AmberWisp>();
    protected override int Buff => ModContent.BuffType<AmberWispCompanion>();
}

public abstract class GrottoPetBuff : ModBuff
{
    protected abstract int Pet { get; }
    protected abstract string PetTexture { get; }
    public override string Texture => PetTexture;

    public override void SetStaticDefaults()
    {
        Main.vanityPet[Type] = true;
        Main.buffNoTimeDisplay[Type] = true;
    }

    public override void Update(Player player, ref int buffIndex)
    {
        player.buffTime[buffIndex] = 18000;
        if (player.whoAmI == Main.myPlayer && player.ownedProjectileCounts[Pet] == 0)
            Projectile.NewProjectile(player.GetSource_Buff(buffIndex), player.Center,
                Vector2.Zero, Pet, 0, 0f, player.whoAmI);
    }
}

public class GlowcapCompanion : GrottoPetBuff
{
    protected override int Pet => ModContent.ProjectileType<GlowcapFirefly>();
    protected override string PetTexture => ModContent.GetInstance<GlowcapSpore>().Texture;
}

public class AmberWispCompanion : GrottoPetBuff
{
    protected override int Pet => ModContent.ProjectileType<AmberWisp>();
    protected override string PetTexture => ModContent.GetInstance<AmberWispSeed>().Texture;
}

public abstract class GrottoPet : ModProjectile
{
    protected abstract int PetBuff { get; }
    protected abstract Vector3 Glow { get; }
    protected abstract float FollowHeight { get; }

    public override void SetStaticDefaults() {Main.projPet[Type] = true;Main.projFrames[Type]=4;}

    public override void SetDefaults()
    {
        Projectile.width = 22;
        Projectile.height = 22;
        Projectile.friendly = true;
        Projectile.tileCollide = false;
        Projectile.penetrate = -1;
        Projectile.timeLeft = 2;
    }

    public override bool? CanDamage() => false;

    public override void AI()
    {
        Projectile.frame=(int)(Main.GameUpdateCount/10)%4;
        Player player = Main.player[Projectile.owner];
        if (!player.active || player.dead || !player.HasBuff(PetBuff))
            return;

        Projectile.timeLeft = 2;
        float bob = (float)System.Math.Sin(Main.GameUpdateCount * 0.075) * 8f;
        Vector2 destination = player.Center + new Vector2(-player.direction * 48f, FollowHeight + bob);
        Vector2 delta = destination - Projectile.Center;
        if (delta.LengthSquared() > 900f * 900f && Main.myPlayer == Projectile.owner)
        {
            Projectile.Center = destination;
            Projectile.velocity = Vector2.Zero;
            Projectile.netUpdate = true;
        }
        else
        {
            Projectile.velocity = Vector2.Lerp(Projectile.velocity, Vector2.Clamp(delta * 0.11f,
                new Vector2(-10f), new Vector2(10f)), 0.16f);
        }

        Projectile.rotation = Projectile.velocity.X * 0.025f;
        Lighting.AddLight(Projectile.Center, Glow);
    }
}

public class GlowcapFirefly : GrottoPet
{
    protected override int PetBuff => ModContent.BuffType<GlowcapCompanion>();
    protected override Vector3 Glow => new Vector3(0.10f, 0.40f, 0.30f);
    protected override float FollowHeight => -35f;
}

public class AmberWisp : GrottoPet
{
    protected override int PetBuff => ModContent.BuffType<AmberWispCompanion>();
    protected override Vector3 Glow => new Vector3(0.48f, 0.25f, 0.06f);
    protected override float FollowHeight => -53f;
}

