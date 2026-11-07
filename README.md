<a id="top"></a>

<div align="center">
  <img src="NyuuTerraria/icon.png" alt="Nyuu Terraria icon" width="64" height="64">

  <h1>Nyuu Terraria</h1>

  <p><strong>Cloud-born magic. Crystal caverns. Stories beyond the mirror.</strong></p>
  <p>An early-game adventure that grows with you: explore, craft, meet Lyra, and make your character your own.</p>

  <p>
    <code>v0.4.0 · Chronicle Expeditions</code>
    &nbsp;
    <code>tModLoader 1.4.5-dev</code>
  </p>

  <p>
    <a href="https://github.com/Nyuukoria/Nyuu-Terraria/raw/refs/heads/main/downloads/NyuuTerraria.tmod"><strong>Download the mod</strong></a>
    &nbsp; · &nbsp;
    <a href="downloads/NyuuTerraria-Source.zip">Source ZIP</a>
    &nbsp; · &nbsp;
    <a href="#getting-started">Start playing</a>
    &nbsp; · &nbsp;
    <a href="#roadmap">Roadmap</a>
  </p>

  <p>
    <img src="NyuuTerraria/Content/Reverie/RootboundKnight.png" alt="Rootbound Knight" height="96">
    &nbsp;&nbsp;
    <img src="NyuuTerraria/Content/Reverie/PrismMatriarch.png" alt="Prism Matriarch" height="112">
    &nbsp;&nbsp;
    <img src="NyuuTerraria/Content/Reverie/Bellkeeper.png" alt="The Bellkeeper" height="96">
  </p>

  <p><kbd>J</kbd> Quests &amp; professions &nbsp; · &nbsp; <kbd>V</kbd> Backpack</p>
</div>

| Explore somewhere new | Make your own build | Bring something home |
| :---: | :---: | :---: |
| **7 ores · 2 cave biomes**<br>Reverie & Hush pocket expeditions | **5 professions · 12 quests**<br>Pride customization & 8 long hairstyles | **6 bosses · 3 minibosses**<br>Pets, equipment & backpacks up to 100 slots |

## Find what you need

| Start your adventure | Explore & fight | Craft & customize |
| --- | --- | --- |
| [Install & multiplayer](#install-and-play)<br>[First steps](#getting-started)<br>[What's new in v0.4.0](#chronicle-expeditions) | [Material locations](#where-to-find-materials)<br>[Cave bosses](#boss-progression)<br>[Expedition encounters](#expedition-bosses-and-minibosses) | [Backpacks](#backpacks)<br>[All crafting recipes](#complete-crafting-guide)<br>[Expedition recipes](#every-expedition-crafting-recipe) |
| [Quests & professions](#journal-stories-and-professions)<br>[Lyra, hairstyles & outfits](#lyra-pride-hairstyles-and-clothing) | [Amber Grotto](#amber-grotto)<br>[Hollowglass Reach](#hollowglass-reach) | [Trinkets, storage & reforging](#using-the-new-features)<br>[Build & contribute](#building-from-source) |

[Need help?](#quick-fixes) · [Roadmap](#roadmap) · [Version history](#version-notes)

## Install and play

1. [Download `NyuuTerraria.tmod`](https://github.com/Nyuukoria/Nyuu-Terraria/raw/refs/heads/main/downloads/NyuuTerraria.tmod).
2. Place it in `Documents\My Games\Terraria\tModLoader-dev\Mods`.
3. Open **Workshop → Manage Mods**, enable **Nyuu Terraria**, then reload mods.

**Playing with friends?** Use the same tModLoader dev build and mod file on the server and every client.

| Your world | How to access the new content |
| --- | --- |
| **New world** | Natural ore veins, Amber Grottos, and Hollowglass ruins appear when you create a world. Explore the Underground and Cavern layers. |
| **Existing world** | Use the alternate bar recipes and craft biome blocks to build underground areas. The [Amber Grotto](#amber-grotto) and [Hollowglass Reach](#hollowglass-reach) sections give the block counts and recipes. |

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

<details>
<summary><strong>View the v0.4.0 sprite gallery</strong> — equipment, encounters, wildlife & hairstyles</summary>

![Chronicle expedition sprites](docs/images/chronicle-expeditions-preview.png?v=roadmap-040)

</details>

### Fixes included

<details>
<summary><strong>Backpack, inventory & interface fixes</strong></summary>

- The backpack remains in its existing saved slot, but is now displayed under **Equipment**, alongside pets and mounts. Click the Equipment tab in the inventory, then use the **Backpack** slot below those slots.
- Backpack clicks no longer use a vanilla bank-container action. This fixes the missing-container error that interrupted the panel and made slots flicker.
- Accessory/trinket wheel scrolling follows the expected direction. Custom bar sprites have a larger, consistently cropped inventory footprint.
- Backpack and Chronicle panels use Terraria's scaled UI coordinates once, keeping drawing and clicks aligned when you change UI scale.

</details>

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

<details>
<summary><strong>Profession bonuses, skill points & XP progression</strong></summary>

| Profession | Initial skill effect per invested point |
| --- | --- |
| Vanguard | +0.5% melee damage; +1 defense for every five points |
| Ranger | +0.5% ranged damage |
| Arcanist | +0.5% magic damage and +1 mana regeneration bonus |
| Summoner | +0.5% summon damage |
| Explorer | +0.4% movement speed and +0.3% mining speed, with the mining bonus capped at 15% |

Levels currently cap at **50 per profession**. Total XP required for level L is **50 × L²**. Quests, hostile enemy defeats, and first expedition visits award XP to the selected profession. Moving through the expeditions as Explorer also awards a small amount of exploration XP. These are the first passive skills; active abilities and branching trees remain planned.

</details>

Your future dialogue and unique quests can be added through [the content authoring guide](docs/CONTENT-AUTHORING.md). Dialogue and quest definitions are kept separately from the journal code.

### Lyra, pride, hairstyles, and clothing

![Lyra](NyuuTerraria/Content/Reverie/Lyra_Head.png) **Lyra, the Prism Tailor**, is a trans woman town NPC who can arrive when a suitable empty house is available. Talk to her for **Stories & quests**, or choose **Long hairstyles** repeatedly to cycle through all eight new styles for free. They are also available during character creation.

<details>
<summary><strong>Preview all eight long hairstyles</strong></summary>

| Style | Preview | Style | Preview |
| --- | --- | --- | --- |
| **Waistlength Waves** | ![Waistlength Waves](docs/images/hairstyles/WaistlengthWaves.png) | **Twin Ribbon Braids** | ![Twin Ribbon Braids](docs/images/hairstyles/TwinRibbonBraids.png) |
| **Silken Curtain** | ![Silken Curtain](docs/images/hairstyles/SilkenCurtain.png) | **Moonlit Curls** | ![Moonlit Curls](docs/images/hairstyles/MoonlitCurls.png) |
| **Trailing Ponytail** | ![Trailing Ponytail](docs/images/hairstyles/TrailingPonytail.png) | **Cascading Layers** | ![Cascading Layers](docs/images/hairstyles/CascadingLayers.png) |
| **Long Side Braid** | ![Long Side Braid](docs/images/hairstyles/LongSideBraid.png) | **Halfup Ribbon** | ![Halfup Ribbon](docs/images/hairstyles/HalfupRibbon.png) |

</details>

The **Pride Ribbon** gives cyan and pink sparkles when visible. **Prism Longcoat** and **Prism Skirt** go in body and leg **vanity** slots. All styles and clothes are available to every character.

Place a **Prism Wardrobe** and right-click it to open **J → Wardrobe**. Save up to **three appearance presets** containing hair, colors, and body appearance. Stand near that wardrobe to restore a preset. These presets do not duplicate or replace inventory/armor items. Choose pronouns in the same tab, or use **/nyuuprofile she/her** (any short custom value is accepted). Lyra's authored dialogue can use your character name and pronouns.

### Expedition materials and inhabitants

<details>
<summary><strong>Where to find materials, enemies & peaceful wildlife</strong></summary>

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

</details>

### Expedition bosses and minibosses

<details>
<summary><strong>Summons, health, rewards & fight tips</strong></summary>

Health below is the normal base value before difficulty/player adjustments. The original bosses also gained health: **Nimbus Warden 5,600**, **Prism Sentinel 7,200**, and **Mourning Bell 10,400**.

| Encounter | Where / how to summon | Base HP | Rewards and fight advice |
| --- | --- | --- | --- |
| ![Reefback Guardian](NyuuTerraria/Content/Reverie/ReefbackGuardian.png) **Reefback Guardian — miniboss** | Use **Reef Shell** in the Reverie | 3,200 | 10–18 Moonprism Ore and 8–16 Coral. Use the platforms to change height as it prepares its charge. |
| ![Rootbound Knight](NyuuTerraria/Content/Reverie/RootboundKnight_Head_Boss.png) **Rootbound Knight** | Use **Rootbound Seal** in the Reverie | 10,000 | 5–8 Prism Wing Scales, 12–18 Echo Fragments, 18–30 Moonprism Ore. Its defense falls from 32 to 8 after the hovering phase. Dodge the charge, then attack during its recovery. |
| ![Lantern Stalker](NyuuTerraria/Content/Reverie/LanternStalker.png) **Lantern Stalker — miniboss** | Rare natural Hush spawn | 4,200 | Archive Lantern and 1–3 Echo Fragments. It is translucent while stalking, then becomes visible and speeds up before its charge. Keep space around yourself. |
| ![The Mirrorless](NyuuTerraria/Content/Reverie/TheMirrorless.png) **The Mirrorless — miniboss** | Rare natural Hush spawn in **Hardmode** | 6,500 | 1–3 Echo Fragments; 1-in-3 chance for Echo Scythe. Circle around its hovering path and move aside for aimed shots. |
| ![Bellkeeper](NyuuTerraria/Content/Reverie/Bellkeeper_Head_Boss.png) **The Bellkeeper** | Use **Cracked Bell Sigil** in the Hush, **Hardmode** | 19,000 | Echo Scythe, 12–18 Echo Fragments, 18–30 Moonprism Ore. Stay away from the center of each ring, move through a gap, and save room to dodge its charge. |
| ![Prism Matriarch](NyuuTerraria/Content/Reverie/PrismMatriarch_Head_Boss.png) **Prism Matriarch** | Use **Prism Cocoon** in the Reverie, **Hardmode** | 24,000 | 12–18 Prism Wing Scales, Pride Moth Charm, and Prism Moth Staff. Alternate between platform levels to avoid rings and aimed five-shot fans. |

**Preparation:** bring healing potions, useful accessories, and a mobility option. The expedition areas already have three central platform levels. A piercing staff, Moonprism Bow, or a summon can keep damage going while you dodge. Below half health, the ring patterns and charges become faster.

**Hush jumpscares:** while exploring the Hush, a brief Lantern Stalker apparition and sound can occur after you have been inside for a short time. There is a cooldown between scares. It does not deal damage or lock movement. Toggle **Hush Jumpscares** in the mod's client settings.

</details>

### Every expedition crafting recipe

<details>
<summary><strong>Open all 24 expedition recipes</strong></summary>

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

</details>

## Boss progression

These are the original cave and cloud encounters. For the Reverie and Hush fights, see [expedition bosses and minibosses](#expedition-bosses-and-minibosses).

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

<details>
<summary><strong>Hollowglass equipment, summons & pet recipes</strong></summary>

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

</details>

## Where to find materials

<details>
<summary><strong>Open the complete material location guide</strong></summary>

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

</details>

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

Choose a station below to open its recipe book. For Moonprism gear, pride items, portals, and memorials, use the [expedition recipe book](#every-expedition-crafting-recipe).

[Bars and Furnace recipes](#furnace) · [Weapons and tools](#anvil-weapons-and-tools) · [Armor](#anvil-armor) · [Accessories](#anvil-accessories-trinkets-and-boss-summon) · [Hardmode upgrade](#mythril-or-orichalcum-anvil) · [Storage, vanity, and bombs](#work-bench-loom-and-hand-crafting)

Backpack recipes are in [Backpacks](#backpacks). Hollowglass equipment and summons are in [Reach recipes](#reach-recipes).

All recipes below produce **one item** unless a quantity is shown. “Anvil” means an **Iron or Lead Anvil**. Weapon values are base stats before reforges and player bonuses.

### Furnace

<details>
<summary><strong>Ore-to-bar recipes & alternate crafting routes</strong></summary>

| Result | Ingredients |
| --- | --- |
| ![Skylight Bar](NyuuTerraria/Content/Skylight/SkylightBar.png) **Skylight Bar** | **3 Skylight Ore** or **2 Iron or Lead Bars + 3 Cloud** |
| ![Stormglass Bar](NyuuTerraria/Content/Skylight/StormglassBar.png) **Stormglass Bar** | **3 Stormglass Ore** or **1 Skylight Bar + 1 Demonite Bar + 2 Cloud** |
| ![Frostveil Bar](NyuuTerraria/Content/Skylight/FrostveilBar.png) **Frostveil Bar** | **3 Frostveil Ore** or **1 Skylight Bar + 6 Ice Blocks** |
| ![Verdigris Bar](NyuuTerraria/Content/AmberGrotto/VerdigrisBar.png) **Verdigris Bar** | **3 Verdigris Ore** or **1 Iron or Lead Bar + 1 Copper Bar + 2 Glowing Mushrooms** |
| ![Ember Amber Bar](NyuuTerraria/Content/AmberGrotto/EmberAmberBar.png) **Ember Amber Bar** | **3 Ember Amber Ore** or **1 Amber + 1 Gold or Platinum Bar** |
| ![Amber Urn](NyuuTerraria/Content/AmberGrotto/AmberUrn.png) **Amber Urn** | **5 Clay Blocks + 1 Ember Amber Ore** |

</details>

### Anvil weapons and tools

<details>
<summary><strong>Staves, weapons & exploration tools</strong></summary>

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

</details>

### Anvil armor

<details>
<summary><strong>Armor pieces, defense & set bonuses</strong></summary>

| Result | Ingredients | What it does |
| --- | --- | --- |
| ![Yuuki’s boots](NyuuTerraria/Content/Items/Armor/YuukisAntiFallDamageBoots.png) **Yuuki’s Anti Fall Damage Boots** | 15 Demonite Bars + 10 Shadow Scales + 10 Cloud | Leg armor with **6 defense**, **+5 percentage points of crit**, fall damage immunity, and a cloud trail. Can replace the greaves in a Shadow armor set. |
| ![Warden Crown](NyuuTerraria/Content/Skylight/WardenCrown.png) **Warden Crown** | 5 Skylight Bars + 2 Warden Cores | **4 defense**, **+20 maximum mana**. Head armor slot. |
| ![Warden Mantle](NyuuTerraria/Content/Skylight/WardenMantle.png) **Warden Mantle** | 8 Skylight Bars + 2 Warden Cores | **6 defense**, **+6% magic damage**. Body armor slot. |
| ![Warden Greaves](NyuuTerraria/Content/Skylight/WardenGreaves.png) **Warden Greaves** | 6 Skylight Bars + 2 Warden Cores | **5 defense**, **+8% movement speed**. Leg armor slot. |

The **full Warden set** gives **15 total defense**, **12% reduced mana cost**, and **+6 percentage points of magic crit**, alongside each piece’s individual effect.

</details>

### Anvil accessories, trinkets, and boss summon

<details>
<summary><strong>Accessories, trinkets & Cloud Idol</strong></summary>

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

</details>

### Mythril or Orichalcum Anvil

<details>
<summary><strong>Hardmode equipment upgrade</strong></summary>

| Result | Ingredients | What it does |
| --- | --- | --- |
| ![Deepheart Pendant](NyuuTerraria/Content/AmberGrotto/DeepheartPendant.png) **Deepheart Pendant** | 1 Amberheart Pendant + 5 Ember Amber Bars + 8 Souls of Night + 8 Souls of Light | Hardmode upgrade with **+5 defense**, **+3 life regeneration**, and **5% damage reduction**. |

</details>

### Work Bench, Loom, and hand crafting

<details>
<summary><strong>Storage, clothes, furniture & bombs</strong></summary>

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

</details>

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

<details>
<summary><strong>Build validation & current testing limits</strong></summary>

**Validation:** compilation, packaged texture-path checks, clean sprite alpha, persistent profession/quest/backpack data, no-container backpack clicks, Equipment-tab visibility, both realm creation and reloads, and the four summon encounters' attack loops were checked. Full rendered client UI, multiplayer playthroughs, and gameplay balance still need in-game verification. [Validation details](docs/validation/0.4.0.md).

</details>

## Quick fixes

| What you see | What to check |
| --- | --- |
| **The mod fails to load after a tModLoader update** | Confirm you are using **1.4.5-dev**. Development builds can change their mod APIs. Include the error text when reporting a loading problem. |
| **A backpack upgrade is missing** | Empty all backpack storage, including overflow slots, and put the preceding backpack in your inventory. Each upgrade also needs its listed material and an anvil. |
| **V does not open a backpack** | Equip the pack in the dedicated **Backpack** slot. Check the **Open Backpack** keybind under Controls. |
| **A boss summon cannot be used** | Cloud Idol requires the surface or sky. Glassbound Seal and Resonating Bell require active Hollowglass Reach, with neither Reach boss already present. |
| **New ores or ruins are missing from an old world** | Use alternate bar recipes and craft biome blocks, or create a new world. |
| **Some Cloudvault slots are hidden** | Hover over the chest inventory and scroll to reach the lower rows. |
| **A recipe is missing** | Check the exact station and named ingredients. Anvil means Iron or Lead. Deepheart requires a Mythril or Orichalcum Anvil. |
| **A sprite or held item looks wrong** | Include the item name and a screenshot so the sprite, placement, and animation frame can be checked. |

<a id="ideas-for-future-updates"></a>

## Roadmap

**Where I want to take Nyuu Terraria next.** More places worth exploring, more ways to build a character, and stories that make the world feel alive.

The first chapter is already playable in [v0.4.0](#chronicle-expeditions). The ideas below are for later updates; names, scope, and release order can change as the mod grows.

| Direction | What comes next |
| --- | --- |
| **Polish the foundations** | Refine backpack controls, inventory layouts, multiplayer behavior, worn sprites, boss pacing, and progression rewards. |
| **Build out the adventure** | Expand the Reverie and Hush into separate-world destinations, give Lyra a longer story, and fill the world with discoveries and side quests. |
| **Grow into the late game** | Add class equipment, deeper skills, biome upgrades, new bosses, and challenges that carry progression beyond Moon Lord. |

### Trans pride and character expression

<details>
<summary><strong>Atelier stories, pride collections & more personal style</strong></summary>

- **Lyra's atelier restoration:** rebuild her workshop through a story quest line, find lost patterns, and unlock embroidered outfits, furniture, and accessories.
- **More pride cosmetics:** cyan, pink, and white capes, hair ribbons, clips, dyes, banners, lanterns, and matching pet accessories.
- **A bigger Prism Wardrobe:** named appearance presets, outfit combinations, favorite looks, and better previews before applying a style.
- **Personal touches:** more character-profile options, dialogue that respects those choices, and vanity versions of favorite combat equipment.
- **Spell and companion styles:** collectible color themes for weapon trails, lantern light, and pet effects.

</details>

### New dimensions and scenarios

<details>
<summary><strong>Beyond the mirror — strange, beautiful & unsettling places</strong></summary>

| Place | Ideas to explore |
| --- | --- |
| **The Reverie** | A full dimension with floating flower islands, crystal orchards, moonlit lakes, seasonal gardens, and an abandoned observatory. Restore paths between islands and uncover the mirror's history. |
| **The Hush** | A separate horror chapter spanning an abandoned mining town, a forgotten archive, and a manor with changing corridors. Follow misleading lanterns, solve environmental puzzles, and piece together the fate of missing explorers. |
| **Tidepool Hollows** | Coastal caves with pearl deposits, hidden fishing pools, shell furniture, flooded shrines, and reef encounters. |
| **Overgrown Citadel** | A ruined fortress overtaken by roots and crystal growth. Open sealed gardens, recover relics, and reveal new paths as bosses fall. |
| **Cinderwood Depths** | Ash-covered forests and abandoned forges near the Underworld, with ember creatures, heat-themed equipment, and buried workshops. |
| **Starglass Expanse** | A late-game destination of shattered celestial ruins, unusual gravity, rare building materials, and the final observatory story. |

For the Hush, I want more atmosphere between encounters: distant footsteps, lights that move out of sight, false rooms, and occasional jumpscares. Scare settings would remain adjustable.

</details>

### Bosses, minibosses, and enemies

<details>
<summary><strong>New encounters & deeper fights for familiar bosses</strong></summary>

- **Expanded existing bosses:** more distinct phases, readable attack cues, difficulty tuning, trophies, masks, and equipment that reflects each boss's theme.
- **The Hollow Astronomer:** an observatory boss with rotating constellations, marked gravity shifts, and celestial weapon rewards.
- **The Unwritten:** a late-game Hush encounter that turns remembered locations into changing arenas and closes the archive's story.
- **The Drowned Choir:** a Tidepool boss whose attacks move with waves, echoes, and currents.
- **The Pale Gardener:** a Reverie guardian surrounded by shifting flower hazards and fragile crystal blooms.
- **New minibosses:** an archive caretaker, a reef ambusher, a haunted armor patrol, and a creature hiding inside abandoned mine machinery.
- **More everyday threats:** reef skitterers, vinebound armor, lantern mimics, ash burrowers, glass serpents, paper spirits, and starfallen constructs.
- **Boss rematches:** optional stronger variants with new patterns and cosmetic rewards.

</details>

### Quest journal and NPC stories

<details>
<summary><strong>Longer stories, discoveries & quests worth returning for</strong></summary>

- Expand the journal with **tracked objectives, map hints, chapter pages, and clearer reward previews**.
- Add **village noticeboards** for exploration, gathering, fishing, crafting, and encounter contracts.
- Build **Lyra's atelier story**, the Hush missing-explorer mystery, and a Hollowglass research quest line.
- Support **custom dialogue and unique quest packs**, with branching conversations and choices that change later scenes.
- Improve **party quest tracking** so friends can follow the same story while keeping individual rewards and saved progress.
- Add **world discoveries** such as letters, journals, hidden shrines, and small stories attached to ruins.
- Include rewards such as **pets, hairstyles, furniture patterns, housing decorations, and access to new routes**.

</details>

### Levels, skills, and professions

<details>
<summary><strong>Skill trees, active abilities & mastery rewards</strong></summary>

Every profession will continue to keep its own progress when switching. The next step is to turn the current passive ranks into more interesting build choices.

| Profession | Possible future skills |
| --- | --- |
| **Vanguard** | Timed guards, short defensive abilities, melee combos, and shield specialization. |
| **Ranger** | Precision shots, ammunition recovery, traps, and bow or thrown-weapon paths. |
| **Arcanist** | Element combinations, spell modifiers, mana-control skills, and charged casts. |
| **Summoner** | Companion commands, minion roles, support effects, and creature specialization. |
| **Explorer** | Treasure clues, mining techniques, fishing bonuses, gathering skills, and navigation tools. |

Other ideas include **mastery cosmetics**, milestone unlocks, saved build presets, and profession-specific crafting patterns. Skill resets and switching should stay easy to use.

</details>

### Peaceful creatures and pets

<details>
<summary><strong>Gardens, companions & a world with quieter moments</strong></summary>

- Give **Dew Foxes, Glass Deer, Mothcats, and Mooncap Sprites** more idle behavior, feeding interactions, and favorite habitats.
- Add **Tidepool Turtles**, lantern fireflies, garden birds, tiny crystal snails, and other biome-specific wildlife.
- Introduce **pet beds, perches, bowls, small habitats, and collectible color variants**.
- Add more companions through **exploration, friendship quests, and rare discoveries**.
- Build a **wildlife journal** with observation goals and rewards for discovering peaceful creatures.
- Let suitable gardens and decorated homes attract different visitors.

</details>

### Graves and memorials

<details>
<summary><strong>Memorial gardens, keepsakes & more grave styles</strong></summary>

- Expand memorials with **biome-specific shapes, more materials, decorative symbols, and flower arrangements**.
- Add **death-grave variants** that match the area where a character fell.
- Introduce an optional **keepsake retrieval quest** with a marker for the last death location.
- Add **pet memorials**, candle stands, cemetery fences, benches, garden bells, and hanging lanterns.
- Explore a **cemetery keeper NPC** with small stories and restoration requests.

</details>

### More long feminine hairstyles and clothes

<details>
<summary><strong>Longer cuts, intricate braids & coordinated outfits</strong></summary>

More style ideas include **hime cuts, waterfall braids, fishtail braids, double Dutch braids, long twin ponytails, soft wolf cuts, side ponytails, long locs, extra-long straight hair, and loose buns with trailing strands**.

Each style should have matching movement frames, a version for wearing under hats, and clean color support. Outfit ideas include **embroidered dresses, long skirts, layered coats, stockings, boots, oversized sweaters, floral accessories, and matching hair ribbons**.

I'd also like more themed collections: **atelier casualwear, moonlit formalwear, coastal outfits, cozy winter clothes, and gothic Hush clothing**.

</details>

### Other progression ideas

<details>
<summary><strong>Equipment, building, storage & adventures across every stage</strong></summary>

| Stage | Ideas for later |
| --- | --- |
| **Early game** | More ore-based bows, boomerangs, summons, and tools; hidden starter ruins; fishing rewards; small NPC requests; useful exploration trinkets. |
| **Middle game** | Class armor with active skills, biome upgrades after major bosses, weapon evolution paths, new crafting stations, and a relic forge that combines boss materials. |
| **Late game** | Celestial dungeons, multi-phase boss rematches, legendary equipment, unusual mounts, and challenges beyond Moon Lord. |
| **Building & collecting** | More furniture sets, decorative plants, paintings, music boxes, rare vases, collectible relics, and small structures to restore. |
| **Everyday improvements** | Storage search, sorting, clearer crafting hints, favorite quest tracking, better equipment previews, and easier loadout management. |
| **World events** | Traveling caravans, strange weather, garden festivals, haunted nights, and roaming encounter chains. |

</details>

## Version notes

<details>
<summary><strong>Release history — v0.1.x through v0.4.0</strong></summary>

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

</details>


---

<p align="center">
  <a href="#top">Back to top</a> &nbsp; · &nbsp;
  <a href="#find-what-you-need">Player guide</a> &nbsp; · &nbsp;
  <a href="docs/CONTENT-AUTHORING.md">Add your own stories</a>
</p>
