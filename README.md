# Nyuu Terraria

**Explore forgotten caverns and portal expeditions, meet Lyra, build your character, and challenge new bosses.**

Nyuu Terraria adds **seven ores**, **two underground biomes**, **two portal pocket expeditions**, **six bosses**, **three minibosses**, quests, five professions, pets, pride customization, armor, weapons, long hairstyles, extra trinket slots, and backpacks with up to **100 extra inventory slots**.

<p align="center">
  <img src="NyuuTerraria/Content/Backpacks/DemoniteBackpack.png" alt="Demonite Backpack" height="48">
  &nbsp;&nbsp;
  <img src="NyuuTerraria/Content/Skylight/NimbusWarden.png" alt="Nimbus Warden" height="88">
  &nbsp;&nbsp;
  <img src="NyuuTerraria/Content/Hollowglass/PrismSentinel.png" alt="Prism Sentinel" height="88">
  &nbsp;&nbsp;
  <img src="NyuuTerraria/Content/Hollowglass/MourningBell.png" alt="Mourning Bell" height="88">
</p>

**Current version — v0.4.0** · **Requires tModLoader 1.4.5-dev**

[**Download the mod**](https://github.com/Nyuukoria/Nyuu-Terraria/raw/refs/heads/main/downloads/NyuuTerraria.tmod) · [Browse the source](NyuuTerraria/) · [Download source ZIP](downloads/NyuuTerraria-Source.zip) · [Version notes](#version-notes)

## Find what you need

| I want to… | Go here |
| --- | --- |
| Install the mod or play with friends | [Install and play](#install-and-play) |
| Try the new quests, professions, portals, or Lyra | [Chronicle expeditions](#chronicle-expeditions) |
| Know what to make first | [Getting started](#getting-started) |
| Equip a backpack and use V | [Backpacks](#backpacks) |
| Find ores and recipe ingredients | [Where to find materials](#where-to-find-materials) |
| Explore caves and find pets | [Amber Grotto](#amber-grotto) · [Hollowglass Reach](#hollowglass-reach) |
| Summon a boss or learn its attacks | [Boss progression](#boss-progression) |
| Look up a recipe or item effect | [Complete crafting guide](#complete-crafting-guide) · [Reach recipes](#reach-recipes) |
| Use trinket slots, storage, or hairstyles | [Using the new features](#using-the-new-features) |
| See possible future additions | [Ideas by game stage](#ideas-for-future-updates) |
| Build or contribute to the mod | [Building from source](#building-from-source) |

## Install and play

1. [Download `NyuuTerraria.tmod`](https://github.com/Nyuukoria/Nyuu-Terraria/raw/refs/heads/main/downloads/NyuuTerraria.tmod).
2. Place it in `Documents\My Games\Terraria\tModLoader-dev\Mods`.
3. Open **Workshop → Manage Mods**, enable **Nyuu Terraria**, then reload mods.

**Playing with friends?** Use the same tModLoader dev build and mod file on the server and every client.

| Your world | How to access the new content |
| --- | --- |
| **New world** | Natural ore veins, Amber Grottos, and Hollowglass ruins generate with the world. Explore the Underground and Cavern layers. |
| **Existing world, including Meow** | Use the alternate bar recipes and craft biome blocks to build underground areas. The [Amber Grotto](#amber-grotto) and [Hollowglass Reach](#hollowglass-reach) sections give the block counts and recipes. |

## Getting started

1. **Make Skylight Bars.** Smelt 3 Skylight Ore per bar, or use 2 Iron or Lead Bars + 3 Cloud at a Furnace.
2. **Pick an early upgrade.** Dawnpetal Staff gives a first magic weapon. Glowglass Lens helps you see underground. A Copper Backpack adds 20 storage slots.
3. **Choose a cave route.** Amber Grotto supplies mining gear, regeneration accessories, and two pet drops. Hollowglass Reach supplies crystal weapons and a two-boss progression.
4. **Prepare for a boss.** Nimbus Warden opens Warden armor and Stormweaver gear. Prism Sentinel opens Mourning Bell, whose drops craft Hollowchime Staff and a bell pet.
5. **Upgrade in Hardmode.** Combine Amberheart Pendant with Souls of Light, Souls of Night, and Ember Amber Bars to make Deepheart Pendant.

Follow the ingredient and crafting-station tables below for exact recipes.


<a id="chronicle-expeditions"></a>

## v0.4.0 — Chronicle expeditions

This is the first playable chapter of the roadmap: **Lyra**, **12 quests**, **five switchable professions**, **eight long hairstyles**, new equipment, **three bosses and three minibosses**, and two portal expeditions.

![Chronicle expedition sprites](docs/images/chronicle-expeditions-preview.png)

### Fixes included

- The backpack remains in its existing saved slot, but is now displayed under **Equipment**, alongside pets and mounts. Click the Equipment tab in the inventory, then use the **Backpack** slot below those slots.
- Backpack clicks no longer use a vanilla bank-container action. This fixes the missing-container error that interrupted the panel and made slots flicker.
- Accessory/trinket wheel scrolling follows the expected direction. Custom bar sprites have a larger, consistently cropped inventory footprint.
- Backpack and Chronicle panels use Terraria's scaled UI coordinates once, keeping drawing and clicks aligned when you change UI scale.

### Your first expedition

1. Press **J** to open **Nyuu Chronicle**. A crafted Chronicle Journal also opens it.
2. Make a **Garden Mirror** at a Work Bench. Use it to enter **the Reverie**.
3. Mine **Moonprism Ore** under the orchard floor and smelt it at a Furnace. Look for the expedition chest near the landing area.
4. Follow Lyra's atelier requests in the journal, craft equipment, and challenge the **Reefback Guardian** or **Rootbound Knight**.
5. Craft a **Cracked Bell Portal** to enter **the Hush**. Bring healing items and a lantern. Its enemies provide Echo Fragments.
6. In Hardmode, challenge **the Bellkeeper**, **the Mirrorless**, and **Prism Matriarch**.

**Portal behavior:** use either key again while inside either expedition to return. Keys are reusable and cannot be used during a boss fight. In this release, these are **persistent pocket areas in unused sky space within your current world**, rather than separate world files. They have biome detection, enemies, resources, structures, loot, and combat platforms. The full separate-world dimensions, changing manor, mining town, and observatory remain future work.

Both areas are created only on their first visit and saved with the world. Their chests do not refill. Existing terrain and builds are checked before an area is placed; if no empty space fits, travel is refused without replacing terrain. Existing worlds can use this feature.

### Journal, stories, and professions

**J → Quests** shows objectives, prerequisite quests, progress, and rewards. Press **Claim** when ready. Gathering requests consume the listed inventory materials; exploration and combat requests consume nothing. Each reward can be claimed once per character. Combat credit goes to nearby players who helped damage that enemy; peaceful wildlife and statue enemies do not award combat XP.

The first 12 requests cover Lyra's atelier, the crystal orchard, the Rootbound Knight, entering the Hush, the Lantern Stalker, Bellkeeper, Matriarch, an enemy-clearance contract, and the Reefback challenge. Your completed requests and counters persist between worlds.

**J → Professions:** switch freely between the five professions. **Every profession saves its own XP, level, and invested skill points.** Switching does not erase any progress. Only your active profession grants its bonuses. Each level provides one point; use **+ Skill** to invest it or **Reset active skills** to redistribute that branch for free.

| Profession | Initial skill effect per invested point |
| --- | --- |
| Vanguard | +0.5% melee damage; +1 defense for every five points |
| Ranger | +0.5% ranged damage |
| Arcanist | +0.5% magic damage and +1 mana regeneration bonus |
| Summoner | +0.5% summon damage |
| Explorer | +0.4% movement speed and +0.3% mining speed, with the mining bonus capped at 15% |

Levels currently cap at **50 per profession**. Total XP required for level L is **50 × L²**. Quests, hostile enemy defeats, and first expedition visits award XP to the selected profession. Moving through the expeditions as Explorer also awards a small amount of exploration XP. These are the first passive skills; active abilities and branching trees remain planned.

Your future dialogue and unique quests can be added through [the content authoring guide](docs/CONTENT-AUTHORING.md). Dialogue and quest definitions are kept separately from the journal code.

### Lyra, pride, hairstyles, and clothing

![Lyra](NyuuTerraria/Content/Reverie/Lyra_Head.png) **Lyra, the Prism Tailor**, is a trans woman town NPC who can arrive when a suitable empty house is available. Talk to her for **Stories & quests**, or choose **Long hairstyles** repeatedly to cycle through all eight new styles for free. They are also available during character creation.

| Style | Preview | Style | Preview |
| --- | --- | --- | --- |
| **Waistlength Waves** | ![Waistlength Waves](docs/images/hairstyles/WaistlengthWaves.png) | **Twin Ribbon Braids** | ![Twin Ribbon Braids](docs/images/hairstyles/TwinRibbonBraids.png) |
| **Silken Curtain** | ![Silken Curtain](docs/images/hairstyles/SilkenCurtain.png) | **Moonlit Curls** | ![Moonlit Curls](docs/images/hairstyles/MoonlitCurls.png) |
| **Trailing Ponytail** | ![Trailing Ponytail](docs/images/hairstyles/TrailingPonytail.png) | **Cascading Layers** | ![Cascading Layers](docs/images/hairstyles/CascadingLayers.png) |
| **Long Side Braid** | ![Long Side Braid](docs/images/hairstyles/LongSideBraid.png) | **Halfup Ribbon** | ![Halfup Ribbon](docs/images/hairstyles/HalfupRibbon.png) |

The **Pride Ribbon** gives cyan and pink sparkles when visible. **Prism Longcoat** and **Prism Skirt** go in body and leg **vanity** slots. All styles and clothes are available to every character.

Place a **Prism Wardrobe** and right-click it to open **J → Wardrobe**. Save up to **three appearance presets** containing hair, colors, and body appearance. Stand near that wardrobe to restore a preset. These presets do not duplicate or replace inventory/armor items. Choose pronouns in the same tab, or use **/nyuuprofile she/her** (any short custom value is accepted). Lyra's authored dialogue can use your character name and pronouns.

### Expedition materials and inhabitants

| Material or creature | Where to find it / what it does |
| --- | --- |
| ![Moonprism Ore](NyuuTerraria/Content/Reverie/MoonprismOre.png) **Moonprism Ore** | Mine crystal clusters beneath the Reverie's orchard floor. Glass Serpents can drop 1–3, and the first expedition chest contains 18. |
| ![Moonprism Bar](NyuuTerraria/Content/Reverie/MoonprismBar.png) **Moonprism Bar** | Smelt 3 Moonprism Ore at a Furnace. Used for early expedition equipment and the Hush portal. |
| ![Echo Fragment](NyuuTerraria/Content/Reverie/EchoFragment.png) **Echo Fragment** | Hush chest and hostile Hush inhabitants. The Rootbound Knight and Bellkeeper also drop 12–18. |
| ![Prism Wing Scale](NyuuTerraria/Content/Reverie/PrismWingScale.png) **Prism Wing Scale** | Rootbound Knight drops 5–8; Prism Matriarch drops 12–18. Used for the Matriarch summon. |
| ![Glass Serpent](NyuuTerraria/Content/Reverie/GlassSerpent.png) **Glass Serpent** | Hostile Reverie creature: 180 base HP, 30 contact damage. |
| ![Paper Wraith](NyuuTerraria/Content/Reverie/PaperWraith.png) **Paper Wraith** | Hostile Hush creature: 280 base HP, 30 contact damage; periodically fires an aimed echo. |
| ![Dew Fox](NyuuTerraria/Content/Reverie/DewFox.png) **Dew Fox**, ![Glass Deer](NyuuTerraria/Content/Reverie/GlassDeer.png) **Glass Deer** | Peaceful Reverie wildlife. |
| ![Mothcat](NyuuTerraria/Content/Reverie/Mothcat.png) **Mothcat**, ![Mooncap Sprite](NyuuTerraria/Content/Reverie/MooncapSprite.png) **Mooncap Sprite** | More peaceful Reverie wildlife. Feeding and garden interactions are future additions. |

The Reverie chest also contains a **Dew Berry Charm**, five Healing Potions, and a **Prism Cocoon**. The Hush chest contains **18 Echo Fragments**, an **Archive Lantern**, five Healing Potions, and a **Cracked Bell Sigil**. Hardmode summons still require Hardmode even when found early.

### Expedition bosses and minibosses

Health below is the normal base value before difficulty/player adjustments. The original bosses also gained health: **Nimbus Warden 5,600**, **Prism Sentinel 7,200**, and **Mourning Bell 10,400**.

| Encounter | Where / how to summon | Base HP | Rewards and fight advice |
| --- | --- | --- | --- |
| ![Reefback Guardian](NyuuTerraria/Content/Reverie/ReefbackGuardian.png) **Reefback Guardian — miniboss** | Use **Reef Shell** in the Reverie | 3,200 | 10–18 Moonprism Ore and 8–16 Coral. Use the platforms to change height as it prepares its charge. |
| ![Rootbound Knight](NyuuTerraria/Content/Reverie/RootboundKnight_Head_Boss.png) **Rootbound Knight** | Use **Rootbound Seal** in the Reverie | 10,000 | 5–8 Prism Wing Scales, 12–18 Echo Fragments, 18–30 Moonprism Ore. Its defense falls from 32 to 8 after the hovering phase. Dodge the charge, then attack during its recovery. |
| ![Lantern Stalker](NyuuTerraria/Content/Reverie/LanternStalker.png) **Lantern Stalker — miniboss** | Rare natural Hush spawn | 4,200 | Archive Lantern and 1–3 Echo Fragments. It is translucent while stalking, then becomes visible and speeds up before its charge. Keep space around yourself. |
| ![The Mirrorless](NyuuTerraria/Content/Reverie/TheMirrorless.png) **The Mirrorless — miniboss** | Rare natural Hush spawn in **Hardmode** | 6,500 | 1–3 Echo Fragments; 1-in-3 chance for Echo Scythe. Circle around its hovering path and move aside for aimed shots. |
| ![Bellkeeper](NyuuTerraria/Content/Reverie/Bellkeeper_Head_Boss.png) **The Bellkeeper** | Use **Cracked Bell Sigil** in the Hush, **Hardmode** | 19,000 | Echo Scythe, 12–18 Echo Fragments, 18–30 Moonprism Ore. Stay away from the center of each ring, move through a gap, and save room to dodge its charge. |
| ![Prism Matriarch](NyuuTerraria/Content/Reverie/PrismMatriarch_Head_Boss.png) **Prism Matriarch** | Use **Prism Cocoon** in the Reverie, **Hardmode** | 24,000 | 12–18 Prism Wing Scales, Pride Moth Charm, and Prism Moth Staff. Alternate between platform levels to avoid rings and aimed five-shot fans. |

**Preparation:** bring healing potions, useful accessories, and a mobility option. The generated areas already have three central platform levels. A piercing staff, Moonprism Bow, or a summon can keep damage going while you dodge. Below half health, the ring patterns and charges become faster.

**Hush jumpscares:** while exploring the Hush, a brief Lantern Stalker apparition and sound can occur after you have been inside for a short time. There is a cooldown between scares. It does not deal damage or lock movement. Toggle **Hush Jumpscares** in the mod's client settings.

### Every expedition crafting recipe

“Anvil” means an Iron or Lead Anvil. Unless stated otherwise, recipes produce one item.

| Item | Station | Ingredients | Use |
| --- | --- | --- | --- |
| ![Journal](NyuuTerraria/Content/Reverie/ChronicleJournal.png) **Chronicle Journal** | Work Bench | 8 Wood + 2 Silk | Open quests, professions, and presets; J works without carrying it. |
| ![Mirror](NyuuTerraria/Content/Reverie/GardenMirror.png) **Garden Mirror** | Work Bench | 20 Glass + 5 Fallen Stars + 8 Silk | Reusable Reverie travel key and realm exit. |
| ![Portal](NyuuTerraria/Content/Reverie/CrackedBellPortal.png) **Cracked Bell Portal** | Anvil | 6 Moonprism Bars + 20 Obsidian + 10 Bone | Reusable Hush travel key and realm exit. |
| ![Bar](NyuuTerraria/Content/Reverie/MoonprismBar.png) **Moonprism Bar** | Furnace | 3 Moonprism Ore | Equipment material. |
| ![Wardrobe](NyuuTerraria/Content/Reverie/GardenMirror.png) **Prism Wardrobe** | Work Bench | 25 Wood + 10 Glass + 5 Silk | Place and right-click for appearance presets. |
| ![Ribbon](NyuuTerraria/Content/Reverie/PrideRibbon.png) **Pride Ribbon** | Loom | 3 Silk + 1 Fallen Star | Wearable pride sparkles. |
| ![Coat](NyuuTerraria/Content/Reverie/PrismLongcoat.png) **Prism Longcoat** | Loom | 10 Silk + 2 Moonprism Bars | Body vanity clothing. |
| ![Skirt](NyuuTerraria/Content/Reverie/PrismSkirt.png) **Prism Skirt** | Loom | 10 Silk + 2 Moonprism Bars | Leg vanity clothing. |
| ![Staff](NyuuTerraria/Content/Reverie/PrismMothStaff.png) **Prism Moth Staff** | Anvil | 10 Moonprism Bars + 4 Fallen Stars + 12 Wood | 42 base magic damage, 5 mana, use time 12; piercing moth bolts and a colored trail. |
| ![Bow](NyuuTerraria/Content/Reverie/MoonprismBow.png) **Moonprism Bow** | Anvil | 7 Moonprism Bars + 12 Wood | 27 base ranged damage; consumes arrows and turns them into piercing bolts. |
| ![Boomerang](NyuuTerraria/Content/Reverie/CoralBoomerang.png) **Coral Boomerang** | Work Bench | 8 Coral + 15 Wood | 22 base melee damage; returns after flying out or hitting terrain. |
| ![Scepter](NyuuTerraria/Content/Reverie/MooncapScepter.png) **Mooncap Scepter** | Anvil | 5 Moonprism Bars + 15 Glowing Mushrooms + 10 Wood | 19 base summon damage; one minion slot per Mooncap. |
| ![Scythe](NyuuTerraria/Content/Reverie/EchoScythe.png) **Echo Scythe** | Anvil | 12 Echo Fragments + 8 Moonprism Bars | 56 base melee damage; sends out a spinning spectral blade. |
| ![Lantern](NyuuTerraria/Content/Reverie/ArchiveLantern.png) **Archive Lantern** | Anvil | 4 Echo Fragments + 5 Torches + 3 Iron or Lead Bars | Accessory granting night vision and light. |
| ![Fox charm](NyuuTerraria/Content/Reverie/DewBerryCharm.png) **Dew Berry Charm** | Work Bench | 3 Moonprism Bars + 3 Daybloom | Dew Fox pet. |
| ![Moth charm](NyuuTerraria/Content/Reverie/PrideMothCharm.png) **Pride Moth Charm** | Loom | 8 Silk + 4 Moonprism Bars | Glowing Pride Moth pet. |
| ![Shell](NyuuTerraria/Content/Reverie/ReefShell.png) **Reef Shell** | Work Bench | 10 Coral + 3 Seashells | Reefback Guardian summon. |
| ![Seal](NyuuTerraria/Content/Reverie/RootboundSeal.png) **Rootbound Seal** | Anvil | 5 Moonprism Bars + 3 Vines | Rootbound Knight summon. |
| ![Sigil](NyuuTerraria/Content/Reverie/CrackedBellSigil.png) **Cracked Bell Sigil** | Anvil | 8 Echo Fragments + 10 Obsidian | Bellkeeper summon; use in Hardmode. |
| ![Cocoon](NyuuTerraria/Content/Reverie/PrismCocoon.png) **Prism Cocoon** | Anvil | 6 Prism Wing Scales + 5 Souls of Light | Prism Matriarch summon; use in Hardmode. |
| ![Memorial](NyuuTerraria/Content/Reverie/PrismMemorial.png) **Prism Memorial** | Work Bench | 20 Stone Blocks + 2 Moonprism Bars + 2 Daybloom | Engravable decorative crystal memorial. |
| ![Copper memorial](NyuuTerraria/Content/Reverie/CopperMemorial.png) **Copper Memorial** | Work Bench | 4 Copper Bars + 15 Stone Blocks | Engravable memorial with a plaque. |
| ![Obsidian memorial](NyuuTerraria/Content/Reverie/ObsidianMemorial.png) **Obsidian Memorial** | Work Bench | 15 Obsidian + 2 Daybloom | Engravable flower-covered memorial. |
| ![Vase](NyuuTerraria/Content/Reverie/ReverieVase.png) **Reverie Vase** | Furnace | 5 Clay Blocks + 1 Moonprism Ore | Decorative furniture; also placed in the expeditions. |

Right-click a memorial to read/edit its inscription using the sign interface. These are crafted decorations; automatic death-grave variants and retrieval quests remain planned.

**Validation:** compilation, packaged texture-path checks, clean sprite alpha, persistent profession/quest/backpack data, no-container backpack clicks, Equipment-tab visibility, both realm generators and reloads, and the four summon encounters' attack loops were checked. Full rendered client UI, multiplayer playthroughs, and gameplay balance still need in-game verification. [Validation details](docs/validation/0.4.0.md).


## Boss progression

| Boss | Where to summon | Summon item | Main rewards |
| --- | --- | --- | --- |
| ![Nimbus Warden](NyuuTerraria/Content/Skylight/NimbusWarden_Head_Boss.png) **Nimbus Warden** | Aboveground or in the sky | **Cloud Idol** — 30 Cloud + 3 Skylight Bars at an anvil | Warden Cores for armor, Stormweaver Staff, and Nimbus Charm. [Fight guide](#summoning-and-fighting-nimbus-warden) |
| ![Prism Sentinel](NyuuTerraria/Content/Hollowglass/PrismSentinel_Head_Boss.png) **Prism Sentinel** | Hollowglass Reach | **Glassbound Seal** — 4 Hollowglass Bars + 2 Fallen Stars at an anvil, or find one in a Reach chest | Prism Hearts for Resonance Charm and Resonating Bell. [Fight guide](#prism-sentinel) |
| ![Mourning Bell](NyuuTerraria/Content/Hollowglass/MourningBell_Head_Boss.png) **Mourning Bell** | Hollowglass Reach | **Resonating Bell** — 3 Prism Hearts + 5 Hollowglass Bars + 10 Obsidian at an anvil | Resonant Clappers for Hollowchime Staff and Pocket Chime. [Fight guide](#mourning-bell) |

**Hollowglass route:** Prism Sentinel → Prism Hearts → Resonating Bell → Mourning Bell. Nimbus Warden has its own crafting route. All three are pre-Hardmode encounters.

## Backpacks

Open the inventory's **Equipment** tab (the tab containing pets and mounts). Equip one backpack in the dedicated **Backpack** slot below those slots, then press **V** to open or close its window. You can change the binding under Controls. Use the arrow buttons to change pages when the pack has more than 50 slots.

| Backpack | Extra slots | Full recipe at an Iron or Lead Anvil |
| --- | --- | --- |
| ![Copper Backpack](NyuuTerraria/Content/Backpacks/CopperBackpack.png) **Copper** | **20** | 12 Copper Bars + any chest |
| ![Iron Backpack](NyuuTerraria/Content/Backpacks/IronBackpack.png) **Iron** | **30** | 12 Iron Bars + 1 Copper Backpack |
| ![Gold Backpack](NyuuTerraria/Content/Backpacks/GoldBackpack.png) **Gold** | **40** | 12 Gold Bars + 1 Iron Backpack |
| ![Emerald Backpack](NyuuTerraria/Content/Backpacks/EmeraldBackpack.png) **Emerald** | **60** | 8 Emeralds + 1 Gold Backpack |
| ![Obsidian Backpack](NyuuTerraria/Content/Backpacks/ObsidianBackpack.png) **Obsidian** | **80** | 30 Obsidian + 1 Emerald Backpack |
| ![Demonite Backpack](NyuuTerraria/Content/Backpacks/DemoniteBackpack.png) **Demonite** | **100** | 20 Demonite Bars + 1 Obsidian Backpack |

**Upgrade order — Copper → Iron → Gold → Emerald → Obsidian → Demonite.** Each upgrade consumes the previous backpack. “Any chest” includes vanilla placeable chests and the mod’s Skylight Chest, Cloudvault, and Reach Chest.

**Using it:** equip the pack, press **V**, and move items into its slots. Contents belong to your character and carry over between worlds. **Every backpack storage slot must be empty before crafting an upgrade**, including overflow slots. Empty it into your inventory or a [Cloudvault](#work-bench-loom-and-hand-crafting), move the old backpack from its equipment slot into your inventory, craft the next tier, and equip the result.

Switching to a smaller pack or removing it still leaves over-capacity items available to withdraw with an empty cursor. Those items also need to be withdrawn before an upgrade becomes available.

## Hollowglass Reach

![Prism Sentinel](NyuuTerraria/Content/Hollowglass/PrismSentinel.png) ![Mourning Bell](NyuuTerraria/Content/Hollowglass/MourningBell.png)

Explore the **Cavern layer** of a new world for chambers of dark **Glass Slate**, turquoise lanterns, and crystal ore. Ruin chests contain **12–18 Hollowglass Ore**, **1 Glassbound Seal**, **2 Spelunker Potions**, and **3 Healing Potions**.

For an existing world, make **10 Glass Slate** from **10 Stone Blocks + 2 Glass** at a Furnace. Place **100 nearby Glass Slate or Hollowglass ore blocks** underground to activate Hollowglass Reach. Use Glass Slate floors for its enemies to spawn on.

| Find | Where and what it gives |
| --- | --- |
| ![Hollowglass Ore](NyuuTerraria/Content/Hollowglass/HollowglassOre.png) **Hollowglass Ore** | Mine veins in the Reach or search its chests and vases. Smelt **3 Ore** per bar at a Furnace. |
| ![Glass Crawler](NyuuTerraria/Content/Hollowglass/GlassCrawler.png) **Glass Crawler** | Spawns on Glass Slate. **82 base HP, 22 contact damage, 8 defense**. Base **1-in-2** chance to drop **1–3 Hollowglass Ore**. |
| ![Glasswing Moth](NyuuTerraria/Content/Hollowglass/GlasswingMoth.png) **Glasswing Moth** | Spawns on Glass Slate. **65 base HP, 20 contact damage, 5 defense**. Base **1-in-2** chance to drop **1–2 Hollowglass Ore**. |
| ![Glass Vase](NyuuTerraria/Content/Hollowglass/GlassVase.png) **Glass Vase** | Break for one of four equally likely drops: **2 Healing Potions**, **3 Hollowglass Ore**, **1 Spelunker Potion**, or **10 Silver Coins**. The vase is consumed. |

### Prism Sentinel

Craft **Glassbound Seal** at an anvil with **4 Hollowglass Bars + 2 Fallen Stars**, or find one in a natural Reach chest. Use it while standing in **Hollowglass Reach** to summon the Sentinel.

It fires three-shot crystal fans while hovering, then pauses and glows amber before charging. Run across a platform during the volleys, and change height when it stops to prepare the charge. Its defense falls during the charge phase, giving you an opening to attack. Below half health its volleys and charges get faster.

The Sentinel has **7,200 base HP**, **28 contact damage**, and **16 defense while hovering**, falling to **5 during the charge phase**. It drops **7–11 Prism Hearts** and **12–20 Hollowglass Ore**. Use the hearts for a Resonance Charm or the second boss summon.

### Mourning Bell

At an anvil, combine **3 Prism Hearts + 5 Hollowglass Bars + 10 Obsidian** into a **Resonating Bell**. Use it in **Hollowglass Reach**.

The Bell sways before releasing rings of crystal shots, then fires aimed bolts. Keep enough space to see gaps between the ring shots and move through a gap while keeping your distance from the center. Change direction for the aimed bolts. Below half health the attack cycle speeds up.

It has **10,400 base HP**, **33 contact damage**, and **12 defense**. It drops **7–11 Resonant Clappers** and **12–20 Hollowglass Ore**. Clappers craft the Hollowchime Staff and the Little Glass Bell pet.

### Reach recipes

| Result | Station | Ingredients | What it does |
| --- | --- | --- | --- |
| ![Glass Slate](NyuuTerraria/Content/Hollowglass/GlassSlate.png) **Glass Slate ×10** | Furnace | 10 Stone Blocks + 2 Glass | Biome building blocks. |
| ![Hollowglass Bar](NyuuTerraria/Content/Hollowglass/HollowglassBar.png) **Hollowglass Bar** | Furnace | 3 Hollowglass Ore, or 4 Glass + 1 Silver or Tungsten Bar + 1 Amethyst | Reach crafting material. |
| **Glass Vase** | Furnace | 4 Glass + 1 Hollowglass Ore | A breakable supply vase. |
| ![Reach Lantern](NyuuTerraria/Content/Hollowglass/ReachLantern.png) **Reach Lantern** | Work Bench | 1 Hollowglass Bar + 2 Torches | Placeable turquoise cave light. |
| ![Reach Chest](NyuuTerraria/Content/Hollowglass/ReachChest.png) **Reach Chest** | Work Bench | 8 Glass Slate + 2 Hollowglass Bars | A 40-slot chest. Crafted chests start empty. |
| ![Glassbound Seal](NyuuTerraria/Content/Hollowglass/GlassboundSeal.png) **Glassbound Seal** | Anvil | 4 Hollowglass Bars + 2 Fallen Stars | Summons Prism Sentinel in the Reach. |
| ![Resonating Bell](NyuuTerraria/Content/Hollowglass/ResonatingBell.png) **Resonating Bell** | Anvil | 3 Prism Hearts + 5 Hollowglass Bars + 10 Obsidian | Summons Mourning Bell in the Reach. |
| ![Glassedge Blade](NyuuTerraria/Content/Hollowglass/GlassedgeBlade.png) **Glassedge Blade** | Anvil | 8 Hollowglass Bars + 2 Emeralds | **31 melee damage** and a piercing glass blade projectile. |
| ![Resonance Charm](NyuuTerraria/Content/Hollowglass/ResonanceCharm.png) **Resonance Charm** | Anvil | 4 Prism Hearts + 4 Hollowglass Bars | **+2 defense**, **+20 maximum mana**, **+4 percentage points of magic crit**. Uses a normal accessory slot. |
| ![Hollowchime Staff](NyuuTerraria/Content/Hollowglass/HollowchimeStaff.png) **Hollowchime Staff** | Anvil | 5 Resonant Clappers + 10 Hollowglass Bars + 3 Fallen Stars | **56 magic damage**, **6 mana per cast**. Its gently homing shots can pierce two enemies. |
| ![Little Glass Bell](NyuuTerraria/Content/Hollowglass/LittleGlassBell.png) **Pocket Chime** | Anvil | 2 Resonant Clappers + 3 Hollowglass Bars | Use to summon a glowing Little Glass Bell vanity pet. |

Both summons are consumed on use and require the Reach biome. The second summon uses a first-boss drop. Both bosses are pre-Hardmode encounters.

## Where to find materials

| Material | Where to look |
| --- | --- |
| ![Glass Slate](NyuuTerraria/Content/Hollowglass/GlassSlate.png) **Glass Slate** | Mine dark turquoise stone in Hollowglass Reach, or craft 10 from 10 Stone Blocks + 2 Glass at a Furnace. |
| ![Hollowglass Ore](NyuuTerraria/Content/Hollowglass/HollowglassOre.png) **Hollowglass Ore** | Hollowglass Reach veins, enemies, chests, and vases. See the [Reach recipes](#reach-recipes) for its alternate bar recipe. |
| ![Skylight Ore](NyuuTerraria/Content/Skylight/SkylightOre.png) **Skylight Ore** | Stone in the Underground and upper Cavern layers of new worlds. |
| ![Stormglass Ore](NyuuTerraria/Content/Skylight/StormglassOre.png) **Stormglass Ore** | Cavern stone in new worlds, deeper than the first Skylight veins. |
| ![Frostveil Ore](NyuuTerraria/Content/Skylight/FrostveilOre.png) **Frostveil Ore** | Underground snow and ice in new worlds. |
| ![Verdigris Ore](NyuuTerraria/Content/AmberGrotto/VerdigrisOre.png) **Verdigris Ore** | Moss Slate pockets in Amber Grottos. Mossback Beetles can also drop it, and it can be found in Amber Urns. |
| ![Ember Amber Ore](NyuuTerraria/Content/AmberGrotto/EmberAmberOre.png) **Ember Amber Ore** | Amber Grottos. The amber veins give off a faint glow. Lantern Moths can also drop it. |
| ![Moss Slate](NyuuTerraria/Content/AmberGrotto/MossSlate.png) **Moss Slate** | Mine the green-gray stone around an Amber Grotto, or craft it at a Work Bench. |
| ![Warden Core](NyuuTerraria/Content/Skylight/WardenCore.png) **Warden Core** | Defeat Nimbus Warden for **9–14 cores**. The staff, armor pieces, and charm all use them. |
| ![Cloud](docs/images/materials/Cloud.png)<br>**Cloud** | Mine the fluffy blocks on Floating Islands. |
| ![Wood](docs/images/materials/Wood.png) ![Gel](docs/images/materials/Gel.png) ![Bomb](docs/images/materials/Bomb.png)<br>**Wood, Gel, and Bombs** | Chop trees for Wood and fight Slimes for Gel. Look for Bombs in pots and chests, or buy them from the Demolitionist. |
| ![Copper Bar](docs/images/materials/CopperBar.png) ![Iron Bar](docs/images/materials/IronBar.png) ![Lead Bar](docs/images/materials/LeadBar.png) ![Silver Bar](docs/images/materials/SilverBar.png) ![Gold Bar](docs/images/materials/GoldBar.png) ![Platinum Bar](docs/images/materials/PlatinumBar.png)<br>**Copper, Iron, Lead, Silver, Gold, and Platinum Bars** | Mine and smelt the corresponding ores at a Furnace. Recipes that list alternatives accept either named bar. The alternate Verdigris recipe specifically requires **Copper**, and Cloudy Bombs specifically require **Silver**. |
| ![Demonite Bar](docs/images/materials/DemoniteBar.png) ![Shadow Scale](docs/images/materials/ShadowScale.png)<br>**Demonite Bars and Shadow Scales** | Smelt Demonite Ore into bars at a Furnace. Eater of Worlds is a source of Demonite Ore and Shadow Scales. |
| ![Ice Block](docs/images/materials/IceBlock.png) ![Snow Block](docs/images/materials/SnowBlock.png)<br>**Ice Blocks and Snow Blocks** | Mine in the Snow biome. The Frostveil Bar recipe takes **Ice Blocks**, and the Frostveil Sigil takes **Snow Blocks**. |
| ![Glowing Mushroom](docs/images/materials/GlowingMushroom.png) ![Amber](docs/images/materials/Amber.png) ![Clay Block](docs/images/materials/ClayBlock.png)<br>**Glowing Mushrooms, Amber, and Clay** | Look underground for Glowing Mushroom biomes. Mine Amber in the Underground Desert, or process Silt, Slush, or Desert Fossil in an Extractinator. Clay occurs in reddish deposits. [Amber sources](https://terraria.wiki.gg/wiki/Amber) |
| ![Daybloom](docs/images/materials/Daybloom.png) ![Fallen Star](docs/images/materials/FallenStar.png) ![Lens](docs/images/materials/Lens.png) ![Glowstick](docs/images/materials/Glowstick.png)<br>**Daybloom, Fallen Stars, Lenses, and Glowsticks** | Gather Daybloom from surface grass, pick up Fallen Stars at night, fight Demon Eyes for Lenses, and look for Glowsticks in pots or buy them from the Merchant at night. |
| ![Leather](docs/images/materials/Leather.png) ![Emerald](docs/images/materials/Emerald.png) ![Obsidian](docs/images/materials/Obsidian.png)<br>**Leather, Emeralds, and Obsidian** | This mod adds a Work Bench recipe for Leather using **8 Gel + 1 Silk**. Find Emeralds underground. Obsidian forms where water meets lava. |
| ![Silk](docs/images/materials/Silk.png) ![Chain](docs/images/materials/Chain.png)<br>**Silk and Chains** | Make Silk from Cobwebs at a Loom. Make Chains from Iron or Lead Bars at an anvil. |
| ![Soulof Light](docs/images/materials/SoulofLight.png) ![Soulof Night](docs/images/materials/SoulofNight.png)<br>**Souls of Light and Night** | In Hardmode, fight enemies in the Cavern layer of the Hallow for Light and the Corruption or Crimson for Night. |

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

The Warden has **5,600 base HP**, **26 base contact damage**, and **9 defense**. It drops **9–14 Warden Cores** per kill before any difficulty-specific adjustments. The three armor pieces use six cores in total. The staff and charm use another seven.

## Complete crafting guide

[Bars and Furnace recipes](#furnace) · [Weapons and tools](#anvil-weapons-and-tools) · [Armor](#anvil-armor) · [Accessories](#anvil-accessories-trinkets-and-boss-summon) · [Hardmode upgrade](#mythril-or-orichalcum-anvil) · [Storage, vanity, and bombs](#work-bench-loom-and-hand-crafting)

Backpack recipes are in [Backpacks](#backpacks). Hollowglass equipment and summons are in [Reach recipes](#reach-recipes).

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
| ![Yuuki’s boots](NyuuTerraria/Content/Items/Armor/YuukisAntiFallDamageBoots.png) **Yuuki’s Anti Fall Damage Boots** | 15 Demonite Bars + 10 Shadow Scales + 10 Cloud | Leg armor with **6 defense**, **+5 percentage points of crit**, fall damage immunity, and a cloud trail. Can replace the greaves in a Shadow armor set. |
| ![Warden Crown](NyuuTerraria/Content/Skylight/WardenCrown.png) **Warden Crown** | 5 Skylight Bars + 2 Warden Cores | **4 defense**, **+20 maximum mana**. Head armor slot. |
| ![Warden Mantle](NyuuTerraria/Content/Skylight/WardenMantle.png) **Warden Mantle** | 8 Skylight Bars + 2 Warden Cores | **6 defense**, **+6% magic damage**. Body armor slot. |
| ![Warden Greaves](NyuuTerraria/Content/Skylight/WardenGreaves.png) **Warden Greaves** | 6 Skylight Bars + 2 Warden Cores | **5 defense**, **+8% movement speed**. Leg armor slot. |

The **full Warden set** gives **15 total defense**, **12% reduced mana cost**, and **+6 percentage points of magic crit**, alongside each piece’s individual effect.

### Anvil accessories, trinkets, and boss summon

| Result | Ingredients | What it does |
| --- | --- | --- |
| ![Wings of Democloud](NyuuTerraria/Content/Items/Accessories/WingsOfDemocloud.png) **Wings of Democloud** | 20 Cloud + 10 Demonite Bars | Hold Jump for about one second of powered flight, then glide. **+3 base defense**. Accessory slot. Accessory reforges apply. |
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

## Building from source

Copy the [NyuuTerraria source folder](NyuuTerraria/) into `Documents\My Games\Terraria\tModLoader-dev\ModSources`. Open **Develop Mods** in tModLoader and select **Build + Reload**.

The `Content` folders contain the item, enemy, biome, and storage code alongside their sprites. The `Localization` folder contains item names and tooltips.

## Quick fixes

| What you see | What to check |
| --- | --- |
| **The mod fails to load after a tModLoader update** | Confirm you are using **1.4.5-dev**. Development builds can change their mod APIs. Include the error text when reporting a loading problem. |
| **A backpack upgrade is missing** | Empty all backpack storage, including overflow slots, and put the preceding backpack in your inventory. Each upgrade also needs its listed material and an anvil. |
| **V does not open a backpack** | Equip the pack in the dedicated **Backpack** slot. Check the **Open Backpack** keybind under Controls. |
| **A boss summon cannot be used** | Cloud Idol requires the surface or sky. Glassbound Seal and Resonating Bell require active Hollowglass Reach, with neither Reach boss already present. |
| **New ores or ruins are missing from an old world** | Use alternate bar recipes and craft biome blocks, or generate a new world. |
| **Some Cloudvault slots are hidden** | Hover over the chest inventory and scroll to reach the lower rows. |
| **A recipe is missing** | Check the exact station and named ingredients. Anvil means Iron or Lead. Deepheart requires a Mythril or Orichalcum Anvil. |
| **A sprite or held item looks wrong** | Include the item name and a screenshot so the sprite, placement, and animation frame can be checked. |

## Ideas for future updates

The roadmap below describes the longer-term designs. **v0.4.0 has started these systems:** Lyra, pride ribbon and moth, appearance presets, eight long hairstyles, a journal with 12 quests, persistent professions, crafted memorials, wildlife, encounters, and two pocket expeditions. See [the playable chapter](#chronicle-expeditions) for current behavior and recipes. Separate-world dimensions, full story locations, deeper skill trees, and the later encounters below remain future work.

### Trans pride and character expression

| Planned addition | Idea |
| --- | --- |
| **Prism Wardrobe** | Expand the existing three appearance presets with saved outfit combinations and more wardrobe options. |
| **Trans pride collection** | Expand the existing ribbon, clothing, and moth pet with cyan, pink, and white capes, clips, dyes, banners, and furniture. |
| **Lyra, the Prism Tailor** | Continue the trans tailor's initial dialogue and journal requests with an authored atelier restoration, new story chapters, clothes, and vanity accessories. |
| **Character profile** | Optional name and pronoun settings for modded dialogue. Clothing and hairstyle choices remain available to every character. |

### New dimensions and scenarios

| Planned area | Setting and things to do |
| --- | --- |
| **The Reverie** | A new dimension reached through a restored garden mirror. Explore floating flower islands, moonlit lakes, crystal orchards, and ruined observatories. Find new ores, build with luminous furniture, meet peaceful creatures, and follow the Prism Tailor's story. |
| **The Hush** | A separate horror scenario reached through a cracked bell portal. Explore an abandoned mining town, an underground archive, and a manor whose corridors change between visits. Follow unreliable lantern signals, investigate missing explorers, and collect clues that reveal the area's history. |
| **Tidepool Hollows** | An early-game coastal cave biome with fishing treasures, pearl gear, shell furniture, and a reef miniboss. |
| **Overgrown Citadel** | A Hardmode ruin with tangled gardens, corrupted crystal enemies, relic crafting, and a boss guarding the entrance to the Reverie's deeper areas. |

The Reverie would focus on discovery, magic, and building. The Hush would have its own horror story, encounters, and rewards.

### Bosses, minibosses, and enemies

| Encounter | Stage and location | Fight idea and rewards |
| --- | --- | --- |
| **Reefback Guardian — miniboss** | Early game, Tidepool Hollows | A shell-armored creature that leaves openings after charging. Drops pearls for early ranged gear and water accessories. |
| **The Rootbound Knight — boss** | Late pre-Hardmode, Overgrown Citadel outskirts | Switches between shield defense and sweeping vine attacks. Drops materials for melee and summoning sets. |
| **Lantern Stalker — miniboss** | The Hush | Follows false lantern trails and retreats into dark passages. Drops a lantern that reveals hidden writing. |
| **The Bellkeeper — boss** | Hardmode, The Hush mining town | Uses directional sound waves, falling chains, and a bell that players can interrupt. Drops echo weapons and sound-themed accessories. |
| **The Mirrorless — miniboss** | The Hush manor | A reflection that repeats your recent movements with a delay. Rewards a short-range repositioning accessory. |
| **Prism Matriarch — boss** | Mid-to-late Hardmode, The Reverie | A giant moth with shifting wing patterns and colored projectile paths. Drops flight gear, mage equipment, and decorative wings. |
| **The Hollow Astronomer — boss** | Late game, ruined observatory | Rotates constellations around the arena and changes gravity during marked phases. Drops celestial weapons and crafting materials. |
| **The Unwritten — final dimension boss** | Post-Moon Lord, The Hush archive | Changes the arena between remembered locations. Rewards final-tier gear and unlocks the last chapter of the dimension story. |
| **More regular enemies** | Across the new areas | Reef skitterers, vinebound armor, glass serpents, paper wraiths, lantern mimics, and starfallen constructs, each with its own drops and behavior. |

### Quest journal and NPC stories

- **A proper quest journal** with tracked objectives, map hints, quest chains, and a clear reward preview.
- **Village noticeboards** for gathering, fishing, crafting, exploration, and boss contracts.
- **NPC story quests**, including Lyra's atelier restoration, missing explorers in The Hush, and a researcher studying Hollowglass.
- **Multiplayer quest credit** for players helping with the same objective, with individual reward claims.
- **Rewards beyond equipment**, such as hairstyles, furniture recipes, pets, housing decorations, and access to new areas.

### Levels, skills, and professions

The initial five professions already keep their own XP, levels, and skill investment when you switch. The future system will expand their passive ranks into branching choices, active abilities, and milestone unlocks.

| Skill branch | Example choices |
| --- | --- |
| **Vanguard** | Melee timing bonuses, guard skills, and short defensive abilities. |
| **Ranger** | Bow and thrown-weapon bonuses, ammunition recovery, and precision shots. |
| **Arcanist** | Mana control, spell effects, and elemental combinations. |
| **Summoner** | Companion commands, minion specialization, and support effects. |
| **Explorer** | Mining, fishing, gathering, navigation, and treasure detection. |

The current journal supports free switching and skill resets. Future stations can add more build options; saved progress for every profession will remain part of the system.

### Peaceful creatures and pets

| Creature | Idea |
| --- | --- |
| **Dew Fox** | A tiny forest creature that visits gardens and follows dropped berries. |
| **Glass Deer** | A rare glowing animal in crystal orchards, with a photo or observation quest. |
| **Mothcat** | A sleepy creature that rests near lanterns and can become a pet. |
| **Tidepool Turtle** | A coastal critter with shell-color variants and a small aquarium habitat. |
| **Mooncap Sprites** | Friendly mushroom creatures that gather around suitable player-built gardens. |

Additional plans include feeding interactions, pet beds, idle animations, and a bestiary section for peaceful wildlife.

### Graves and memorials

- **Grave styles by material and biome**, including copper plaques, obsidian markers, crystal headstones, and flower-covered memorials.
- **Engravable memorial stones** with a player name, a short message, and decorative symbols.
- **Pet memorials** for builds and gardens.
- **A grave-recovery quest option** that marks your last death location and rewards retrieving a keepsake.
- **Cemetery furniture**, such as fences, candles, wilted flowers, benches, and lanterns.

### More long feminine hairstyles and clothes

The first eight long styles are available in v0.4.0. Future additions will expand the selection with more lengths, bangs, curls, braids, and ribbon arrangements.

Each style would include its worn-under-a-hat version and animation frames. Matching clothes could include long skirts, fitted coats, embroidered dresses, boots, stockings, and mix-and-match hair ribbons.

### Other progression ideas

| Stage | Additional ideas |
| --- | --- |
| **Early game** | Starter bows, boomerangs, and summon weapons for existing ores; exploration tools; fishing rewards; small NPC requests. |
| **Middle game** | Corrupted versions of custom biomes, armor with active skills, and a relic forge that combines boss materials into new weapon effects. |
| **Late game** | A celestial ruin dungeon, boss rematches with new phases, legendary weapon upgrades, and unusual mounts. |


## Version notes

| Version | Added or changed |
| --- | --- |
| **v0.4.0** | Backpack flicker and Equipment-tab placement fixes, corrected accessory scrolling, larger bar icons; Lyra, pride customization, eight long hairstyles, 12 quests, five persistent professions, Reverie/Hush pocket expeditions with optional scares, three bosses, three minibosses, wildlife, pets, Moonprism, weapons, and memorials. |
| **v0.3.1** | Sequential backpack recipes with an empty-storage requirement, a Glass Slate guide icon, and new Warden, Lens, Cloudtail, and matching bomb artwork. |
| **v0.3.0** | Six backpacks, a dedicated Backpack slot and V-key storage window, Hollowglass Reach, two bosses, ruin loot, equipment, a pet, and exterior sprite cleanup. |
| **v0.2.6** | Cleaned up the Amberflare Staff sprite, added individual armor and vanity icons, and moved the Warden set bonus into the armor section. |
| **v0.2.5** | Full player guide. Redrawn Amberflare Staff, Amberheart Pendant, and Deepheart Pendant. |
| **v0.2.4** | Amber Grotto, Amber Urns, Verdigris and Ember Amber, two cave enemies, two pets, tools, weapons, and pendants. |
| **v0.2.3** | Stormglass and Frostveil, Cloudvault, Cloud Satchel, Cloudweave Belt, three hairstyles, and cooler Skylight artwork. |
| **v0.2.2** | Redrawn Skylight items, equipment, boss, and spell sprites. |
| **v0.2.1** | Updated Warden armor and Yuuki’s boots for the current armor set API. |
| **v0.2.0** | Skylight ore, Nimbus Warden, Warden armor, weapons, trinkets, slots, chest, vanity, and two hairstyles. |
| **v0.1.x** | Wings and Staff of Democloud, Yuuki’s boots, Cloudy Bombs, and Steam achievement sync. |

