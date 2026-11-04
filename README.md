# Nyuu Terraria

An early-game expansion for **tModLoader 1.4.5-dev**. It adds five ores, two underground progression paths, the Nimbus Warden boss, spells, armor, trinkets, pets, and storage.

[Download NyuuTerraria.tmod](https://github.com/Nyuukoria/Nyuu-Terraria/raw/refs/heads/main/downloads/NyuuTerraria.tmod) · [Source and sprites](NyuuTerraria/)

- [Install and play](#install-and-play)
- [Where to find materials](#where-to-find-materials)
- [Amber Grotto, enemies, pets, and urns](#amber-grotto)
- [Nimbus Warden](#summoning-and-fighting-nimbus-warden)
- [Complete crafting guide](#complete-crafting-guide)
- [Trinket slots, storage, hairstyles, and Steam](#using-the-new-features)
- [Version notes](#version-notes)

## Install and play

1. Put `NyuuTerraria.tmod` in `Documents\My Games\Terraria\tModLoader-dev\Mods`.
2. Open **Workshop → Manage Mods**, enable **Nyuu Terraria**, and reload mods. For multiplayer, use the same tModLoader dev build and mod file on the server and every client.
3. Start a new world to find natural ore veins and Amber Grottos. In an existing world such as Meow, use the alternate Furnace recipes below and craft Moss Slate to build an underground Grotto.

To contribute, copy the [NyuuTerraria source folder](NyuuTerraria/) into `tModLoader-dev\ModSources`, then use **Build + Reload** under **Develop Mods**.

## Where to find materials

| Material | Where to look |
| --- | --- |
| **Skylight Ore** | Stone in the Underground and upper Cavern layers of new worlds. |
| **Stormglass Ore** | Cavern stone in new worlds, deeper than the first Skylight veins. |
| **Frostveil Ore** | Underground snow and ice in new worlds. |
| **Verdigris Ore** | Moss Slate pockets in Amber Grottos. Mossback Beetles can also drop it, and it can be found in Amber Urns. |
| **Ember Amber Ore** | Amber Grottos. The amber veins give off a faint glow. Lantern Moths can also drop it. |
| **Moss Slate** | Mine the green-gray stone around an Amber Grotto, or craft it at a Work Bench. |
| **Warden Core** | Defeat Nimbus Warden for **9–14 cores**. The staff, armor pieces, and charm all use them. |
| **Cloud** | Mine the fluffy blocks on Floating Islands. |
| **Wood, Gel, and Bombs** | Chop trees for Wood and fight Slimes for Gel. Look for Bombs in pots and chests, or buy them from the Demolitionist. |
| **Copper, Iron, Lead, Silver, Gold, and Platinum Bars** | Mine and smelt the corresponding ores at a Furnace. Recipes that list alternatives accept either named bar. The alternate Verdigris recipe specifically requires **Copper**, and Cloudy Bombs specifically require **Silver**. |
| **Demonite Bars and Shadow Scales** | Smelt Demonite Ore into bars at a Furnace. Eater of Worlds is a source of Demonite Ore and Shadow Scales. |
| **Ice Blocks and Snow Blocks** | Mine in the Snow biome. The Frostveil Bar recipe takes **Ice Blocks**, and the Frostveil Sigil takes **Snow Blocks**. |
| **Glowing Mushrooms, Amber, and Clay** | Look underground for Glowing Mushroom biomes. Mine Amber in the Underground Desert, or process Silt, Slush, or Desert Fossil in an Extractinator. Clay occurs in reddish deposits. [Amber sources](https://terraria.wiki.gg/wiki/Amber) |
| **Daybloom, Fallen Stars, Lenses, and Glowsticks** | Gather Daybloom from surface grass, pick up Fallen Stars at night, fight Demon Eyes for Lenses, and look for Glowsticks in pots or buy them from the Merchant at night. |
| **Silk and Chains** | Make Silk from Cobwebs at a Loom. Make Chains from Iron or Lead Bars at an anvil. |
| **Souls of Light and Night** | In Hardmode, fight enemies in the Cavern layer of the Hallow for Light and the Corruption or Crimson for Night. |

## Amber Grotto

In a new world, look for **Moss Slate** near the Cavern layer. Follow the green-gray rock and the glow of Ember Amber to find the chamber. The biome activates in the Underground or Cavern layer with at least **85 nearby Moss Slate or Grotto ore blocks**.

For an existing world, use a **Work Bench** to turn **10 Stone Blocks + 1 Glowing Mushroom** into **10 Moss Slate**. Build an underground area with at least 85 nearby Grotto blocks. Use Moss Slate for exposed floors so the cave enemies have places to spawn.

| Find | What you can get |
| --- | --- |
| ![Mossback Beetle](NyuuTerraria/Content/AmberGrotto/MossbackBeetle.png) **Mossback Beetle** | Spawns on Moss Slate underground. Base stats are **65 HP, 19 contact damage, 7 defense**. Drops **1–3 Verdigris Ore** with a base **1-in-2** chance, and **Glowcap Spore** with a base **1-in-14** chance. |
| ![Lantern Moth](NyuuTerraria/Content/AmberGrotto/LanternMoth.png) **Lantern Moth** | Spawns on Moss Slate underground. Base stats are **48 HP, 16 contact damage, 3 defense**. Drops **1–2 Ember Amber Ore** with a base **1-in-2** chance, and **Amber Wisp Seed** with a base **1-in-14** chance. |
| ![Amber Urn](NyuuTerraria/Content/AmberGrotto/AmberUrn.png) **Amber Urn** | Break it for one of five drops. Each has a **1-in-5** chance: **2 Recall Potions**, **3 Lesser Healing Potions**, **8 Glowsticks**, **3 Verdigris Ore**, or **5 Silver Coins**. The urn is consumed when broken. |
| ![Glowcap Firefly](NyuuTerraria/Content/AmberGrotto/GlowcapFirefly.png) **Glowcap Firefly** | Use a **Glowcap Spore** from a Mossback Beetle to summon this teal-glowing vanity pet. |
| ![Amber Wisp](NyuuTerraria/Content/AmberGrotto/AmberWisp.png) **Amber Wisp** | Use an **Amber Wisp Seed** from a Lantern Moth to summon this amber-glowing vanity pet. |

## Summoning and fighting Nimbus Warden

![Nimbus Warden](NyuuTerraria/Content/Skylight/NimbusWarden.png)

1. At an **Iron or Lead Anvil**, craft a **Cloud Idol** from **30 Cloud + 3 Skylight Bars**.
2. Use it aboveground or in the sky. It is consumed when summoning. A long two-level platform gives you room to run and change height.
3. The Warden hovers, fires volleys, then dives. Run sideways during volleys. Move out of the dive path as it lines up, then attack while it passes.
4. **Below half health**, the Warden dives faster and fires wider volleys. Save flight for dodging. The **Dawnpetal Staff**, **Amberflare Staff**, or **Staff of Democloud** are all craftable before the fight. Stormweaver gear requires cores from a kill.

The Warden has **2,800 base HP**, **26 base contact damage**, and **9 defense**. It drops **9–14 Warden Cores** per kill before any difficulty-specific adjustments. The three armor pieces use six cores in total. The staff and charm use another seven.

## Complete crafting guide

All recipes below produce **one item** unless a quantity is shown. “Anvil” means an **Iron or Lead Anvil**. Weapon values are base stats before reforges and player bonuses.

### Furnace

| Result | Ingredients |
| --- | --- |
| ![Skylight Bar](NyuuTerraria/Content/Skylight/SkylightBar.png) **Skylight Bar** | **3 Skylight Ore** or **2 Iron or Lead Bars + 3 Cloud** |
| ![Stormglass Bar](NyuuTerraria/Content/Skylight/StormglassBar.png) **Stormglass Bar** | **3 Stormglass Ore** or **1 Skylight Bar + 1 Demonite Bar + 2 Cloud** |
| ![Frostveil Bar](NyuuTerraria/Content/Skylight/FrostveilBar.png) **Frostveil Bar** | **3 Frostveil Ore** or **1 Skylight Bar + 6 Ice Blocks** |
| ![Verdigris Bar](NyuuTerraria/Content/AmberGrotto/VerdigrisBar.png) **Verdigris Bar** | **3 Verdigris Ore** or **1 Iron or Lead Bar + 1 Copper Bar + 2 Glowing Mushrooms** |
| ![Ember Amber Bar](NyuuTerraria/Content/AmberGrotto/EmberAmberBar.png) **Ember Amber Bar** | **3 Ember Amber Ore** or **1 Amber + 1 Gold or Platinum Bar** |
| ![Amber Urn](NyuuTerraria/Content/AmberGrotto/AmberUrn.png) **Amber Urn** | **5 Clay Blocks + 1 Ember Amber Ore** |

### Anvil weapons and tools

| Result | Ingredients | What it does |
| --- | --- | --- |
| ![Dawnpetal Staff](NyuuTerraria/Content/Skylight/DawnpetalStaff.png) **Dawnpetal Staff** | 5 Skylight Bars + 3 Daybloom + 8 Wood | **17 magic damage, 3 mana**. Fires a bolt that ignites enemies. |
| ![Skylight Saber](NyuuTerraria/Content/Skylight/SkylightSaber.png) **Skylight Saber** | 7 Skylight Bars + 1 Fallen Star | **21 melee damage**. Ignites enemies and scatters golden sparks. |
| ![Amberflare Staff](NyuuTerraria/Content/AmberGrotto/AmberflareStaff.png) **Amberflare Staff** | 7 Ember Amber Bars + 3 Verdigris Bars + 8 Torches | **29 magic damage, 5 mana**. Fires a flame bolt that ignites and can hit two enemies. |
| ![Stormglass Scepter](NyuuTerraria/Content/Skylight/StormglassScepter.png) **Stormglass Scepter** | 8 Stormglass Bars + 12 Cloud + 2 Fallen Stars | **34 magic damage, 4 mana**. Its frost arc can hit three enemies. Boosts mana regeneration while held. |
| ![Frostveil Blade](NyuuTerraria/Content/Skylight/FrostveilBlade.png) **Frostveil Blade** | 9 Frostveil Bars + 10 Cloud | **38 melee damage**. Launches a Frostburn crescent that can hit two enemies. The crescent uses two-thirds of the swing's damage. |
| ![Staff of Democloud](NyuuTerraria/Content/Items/Weapons/StaffOfDemocloud.png) **Staff of Democloud** | 25 Demonite Bars + 50 Cloud + 15 Wood | **50 magic damage, 4 mana, 30% crit before gear or reforges**. Autofires five times per second, boosts mana regen while held, and pierces up to four enemies. Each hit has a **1-in-3** chance to spawn three smaller shards. |
| ![Stormweaver Staff](NyuuTerraria/Content/Skylight/StormweaverStaff.png) **Stormweaver Staff** | 7 Skylight Bars + 4 Warden Cores + 15 Cloud | **22 magic damage per bolt, 6 mana per cast**. Fires three bolts in a fan. Each can hit up to two enemies. |
| ![Verdigris Pickaxe](NyuuTerraria/Content/AmberGrotto/VerdigrisPickaxe.png) **Verdigris Pickaxe** | 8 Verdigris Bars + 4 Wood of any type | **60% pickaxe power**, **+1 tile reach**, and **10 melee damage**. |

### Anvil armor

| Result | Ingredients | What it does |
| --- | --- | --- |
| ![Wings of Democloud](NyuuTerraria/Content/Items/Accessories/WingsOfDemocloud.png) **Wings of Democloud** | 20 Cloud + 10 Demonite Bars | Hold Jump for about one second of powered flight, then glide. **+3 base defense**. Accessory reforges apply. |
| ![Yuuki’s boots](NyuuTerraria/Content/Items/Armor/YuukisAntiFallDamageBoots.png) **Yuuki’s Anti Fall Damage Boots** | 15 Demonite Bars + 10 Shadow Scales + 10 Cloud | Leg armor with **6 defense**, **+5 percentage points of crit**, fall damage immunity, and a cloud trail. Can replace the greaves in a Shadow armor set. |
| ![Warden Crown](NyuuTerraria/Content/Skylight/WardenCrown.png) **Warden Crown** | 5 Skylight Bars + 2 Warden Cores | **4 defense**, **+20 maximum mana**. Head armor slot. |
| ![Warden Mantle](NyuuTerraria/Content/Skylight/WardenMantle.png) **Warden Mantle** | 8 Skylight Bars + 2 Warden Cores | **6 defense**, **+6% magic damage**. Body armor slot. |
| ![Warden Greaves](NyuuTerraria/Content/Skylight/WardenGreaves.png) **Warden Greaves** | 6 Skylight Bars + 2 Warden Cores | **5 defense**, **+8% movement speed**. Leg armor slot. |

The **full Warden set** gives **15 total defense**, **12% reduced mana cost**, and **+6 percentage points of magic crit**, alongside each piece’s individual effect.

### Anvil trinkets and boss summon

| Result | Ingredients | What it does |
| --- | --- | --- |
| ![Glowglass Lens](NyuuTerraria/Content/Skylight/GlowglassLens.png) **Glowglass Lens** | 2 Skylight Bars + 1 Lens + 3 Glowsticks | Night vision and a light around the player. |
| ![Petal Brooch](NyuuTerraria/Content/Skylight/PetalBrooch.png) **Petal Brooch** | 3 Skylight Bars + 2 Daybloom | **+7% magic damage** and faster mana regeneration. |
| ![Nimbus Charm](NyuuTerraria/Content/Skylight/NimbusCharm.png) **Nimbus Charm** | 3 Warden Cores + 8 Cloud | **+12% movement speed** and stronger jumps. |
| ![Starfall Pendant](NyuuTerraria/Content/Skylight/StarfallPendant.png) **Starfall Pendant** | 3 Stormglass Bars + 2 Fallen Stars | **+20 maximum mana**, **+4 percentage points of magic crit**. |
| ![Frostveil Sigil](NyuuTerraria/Content/Skylight/FrostveilSigil.png) **Frostveil Sigil** | 3 Frostveil Bars + 8 Snow Blocks | **+2 defense** and immunity to Chilled. |
| ![Cloudweave Belt](NyuuTerraria/Content/Skylight/CloudweaveBelt.png) **Cloudweave Belt** | 4 Skylight Bars + 12 Cloud + 5 Silk | Enables two extra Sky Trinket slots while equipped in a normal accessory slot. |
| ![Grotto Lantern](NyuuTerraria/Content/AmberGrotto/GrottoLantern.png) **Grotto Lantern** | 4 Verdigris Bars + 6 Glowsticks | Teal personal light and faster mining. |
| ![Amberheart Pendant](NyuuTerraria/Content/AmberGrotto/AmberheartPendant.png) **Amberheart Pendant** | 5 Ember Amber Bars + 2 Chains | **+2 defense** and **+2 life regeneration**. |
| ![Cloud Idol](NyuuTerraria/Content/Skylight/CloudIdol.png) **Cloud Idol** | 30 Cloud + 3 Skylight Bars | Summons Nimbus Warden aboveground or in the sky. |



### Mythril or Orichalcum Anvil

| Result | Ingredients | What it does |
| --- | --- | --- |
| ![Deepheart Pendant](NyuuTerraria/Content/AmberGrotto/DeepheartPendant.png) **Deepheart Pendant** | 1 Amberheart Pendant + 5 Ember Amber Bars + 8 Souls of Night + 8 Souls of Light | Hardmode upgrade with **+5 defense**, **+3 life regeneration**, and **5% damage reduction**. |

### Work Bench, Loom, and hand crafting

| Result | Station | Ingredients | What it does |
| --- | --- | --- | --- |
| ![Moss Slate ×10](NyuuTerraria/Content/AmberGrotto/MossSlate.png) **Moss Slate ×10** | Work Bench | 10 Stone Blocks + 1 Glowing Mushroom | Build or extend an underground Grotto. |
| ![Skylight Chest](NyuuTerraria/Content/Skylight/SkylightChest.png) **Skylight Chest** | Work Bench | 8 Wood of any type + 2 Skylight Bars | A placeable **40-slot** chest. |
| ![Cloudvault](NyuuTerraria/Content/Skylight/Cloudvault.png) **Cloudvault** | Work Bench | 1 Skylight Chest + 4 Stormglass Bars + 4 Frostveil Bars + 10 Cloud | A placeable **160-slot** chest. |
| ![Cloud Satchel](NyuuTerraria/Content/Skylight/CloudSatchel.png) **Cloud Satchel** | Loom | 4 Skylight Bars + 8 Cloud + 6 Silk | Portable access to your existing personal Piggy Bank inventory. |
| ![Cloud Beret](NyuuTerraria/Content/Skylight/CloudBeret.png) **Cloud Beret** | Loom | 5 Silk + 3 Cloud | Head vanity slot. |
| ![Cloudtail Coat](NyuuTerraria/Content/Skylight/CloudtailCoat.png) **Cloudtail Coat** | Loom | 5 Silk + 3 Cloud | Body vanity slot. |
| ![Cloudtail Trousers](NyuuTerraria/Content/Skylight/CloudtailTrousers.png) **Cloudtail Trousers** | Loom | 5 Silk + 3 Cloud | Leg vanity slot. |
| ![Cloudy Bomb](NyuuTerraria/Content/Items/Consumables/CloudyBomb.png) **Cloudy Bomb** | Hand crafting | 1 Bomb + 4 Cloud + 4 Silver Bars | **28-tile blast radius**, damages enemies and clears terrain and walls, including protected Underworld terrain. The blast spares players. |
| ![Sticky Cloudy Bomb](NyuuTerraria/Content/Items/Consumables/StickyCloudyBomb.png) **Sticky Cloudy Bomb** | Hand crafting | 1 Cloudy Bomb + 1 Gel | Sticks to the first surface it hits, then uses the same blast. |

## Using the new features

### Trinket slots and vanity

- The **utility slot** accepts the **Glowglass Lens**. The **sky slot** accepts the **Petal Brooch** or **Nimbus Charm**.
- Equip the **Cloudweave Belt** in a normal accessory slot to activate two more slots. Those slots accept the **Glowglass Lens, Petal Brooch, Nimbus Charm, Starfall Pendant, and Frostveil Sigil**. The Amber Grotto accessories use normal accessory slots.
- Put the **Cloud Beret**, **Cloudtail Coat**, and **Cloudtail Trousers** in vanity slots to wear the outfit over armor. ![Cloudtail outfit](docs/images/cloudtail-outfit.png)
- Select **Petal Bob**, **Storm Tuft**, **Cloudside Ponytail**, **Moonbuns**, or **Lavender Braid** in character creation or at the **Stylist**. ![Petal Bob and Storm Tuft](docs/images/hairstyles.png) ![Cloudside Ponytail, Moonbuns, and Lavender Braid](docs/images/cloudweave-hair.png)

### Storage and reforging

- Right-click a **Cloudvault** to open it. Hover over the chest grid and scroll to reach the additional rows.
- Use the **Cloud Satchel**, then interact with the flying Piggy Bank. It opens the same personal storage as a regular Piggy Bank.
- The **Goblin Tinkerer** can apply accessory modifiers to the wings and magic weapon modifiers to the staves. For example, **Warding** raises the Wings of Democloud from **3 to 7 defense**.

### Steam achievements

Launch tModLoader through Steam. Nyuu Terraria forwards completed **vanilla Terraria** achievements when you enter a world or earn one during play. The mod uses tModLoader’s Steam connection for the handoff.

## Version notes

| Version | Added or changed |
| --- | --- |
| **v0.2.6** | Cleaned up the Amberflare Staff sprite, added individual armor and vanity icons, and moved the Warden set bonus into the armor section. |
| **v0.2.5** | Full player guide. Redrawn Amberflare Staff, Amberheart Pendant, and Deepheart Pendant. |
| **v0.2.4** | Amber Grotto, Amber Urns, Verdigris and Ember Amber, two cave enemies, two pets, tools, weapons, and pendants. |
| **v0.2.3** | Stormglass and Frostveil, Cloudvault, Cloud Satchel, Cloudweave Belt, three hairstyles, and cooler Skylight artwork. |
| **v0.2.2** | Redrawn Skylight items, equipment, boss, and spell sprites. |
| **v0.2.1** | Updated Warden armor and Yuuki’s boots for the current armor set API. |
| **v0.2.0** | Skylight ore, Nimbus Warden, Warden armor, weapons, trinkets, slots, chest, vanity, and two hairstyles. |
| **v0.1.x** | Wings and Staff of Democloud, Yuuki’s boots, Cloudy Bombs, and Steam achievement sync. |
