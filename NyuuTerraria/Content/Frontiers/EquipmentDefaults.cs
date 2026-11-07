using Terraria;
using Terraria.ModLoader;
using NyuuTerraria.Content.Reverie;
using NyuuTerraria.Content.Spellcraft;
namespace NyuuTerraria.Content.Frontiers;

// 1.4.5-dev defaults mod items to CommonMaxStack. Gear must remain individually reforgeable.
public class EquipmentDefaults : GlobalItem
{
    public override void SetDefaults(Item item)
    {
        if(item.ModItem?.Mod!=Mod)return;
        if(item.accessory||item.defense>0||item.vanity||item.damage>0&&!item.consumable||
            item.ModItem is RealmKey or FrontierKey or ChronicleJournal or NyuuSpellbook or FieldJournal or RematchSigil or SpellPalette or FrontierSummon or FrontierEventItem)
            item.maxStack=1;
    }
}
