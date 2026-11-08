using Terraria;
using Terraria.ID;
using Terraria.ModLoader;
using NyuuTerraria.Content.Reverie;
using NyuuTerraria.Content.Skylight;

namespace NyuuTerraria.Content.Tweaks;

/// <summary>
/// Turns the accessories that did nothing into ones that do. None of the mod's own files are
/// edited: this rides on GlobalItem, so the original item classes stay untouched and the
/// behaviour is additive.
///
/// Archive Lantern - set nightVision directly, which vanilla re-derives every frame, so it had
///                   no visible effect. Now applies the Night Owl buff plus a steady light.
/// Glowglass Lens  - same fault, plus Shine for the glow radius.
/// Pride Ribbon    - was cosmetic dust only. Keeps the dust, adds speed and a little defence.
/// </summary>
public class NyuuAccessoryTweaks : GlobalItem
{
    public override void UpdateAccessory(Item item, Player player, bool hideVisual)
    {
        if (item.type == ModContent.ItemType<ArchiveLantern>())
        {
            player.AddBuff(BuffID.NightOwl, 2);
            Lighting.AddLight(player.Center, 0.82f, 0.74f, 0.66f);
        }
        else if (item.type == ModContent.ItemType<GlowglassLens>())
        {
            player.AddBuff(BuffID.NightOwl, 2);
            player.AddBuff(BuffID.Shine, 2);
            Lighting.AddLight(player.Center, 0.95f, 0.82f, 1.00f);
        }
        else if (item.type == ModContent.ItemType<PrideRibbon>())
        {
            player.moveSpeed += 0.10f;
            player.maxRunSpeed += 0.50f;
            player.statDefense += 2;
        }
    }
}
