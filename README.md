<div align="center">
<img src="NyuuTerraria/icon.png" width="64" alt="Nyuu Terraria">

# Nyuu Terraria

**Magic, discoveries, and stories worth bringing home.**

`v0.5.1 · Repair pass` &nbsp; `tModLoader 1.4.5-dev`

[**Download mod**](downloads/NyuuTerraria.tmod) · [**Showcase save**](downloads/NyuuTerraria-Showcase.zip) · [**Source ZIP**](downloads/NyuuTerraria-Source.zip)

</div>

## Find your way

| Play | Explore | Follow development |
| --- | --- | --- |
| [Install and controls](#start-playing)<br>[Complete player guide](docs/PLAYER-GUIDE.md)<br>[All 145 items and recipes](docs/ITEMS.md)<br>[Backpack upgrades](docs/PLAYER-GUIDE.md#backpacks) | [Material locations](docs/PLAYER-GUIDE.md#where-to-find-materials)<br>[Boss summons and strategies](docs/WORLDBOUND.md#encounters-and-preparation)<br>[Six expedition destinations](docs/WORLDBOUND.md#six-destinations-one-persistent-world)<br>[Quests, skills and wildlife](docs/WORLDBOUND.md) | [Latest fixes](#latest-fixes)<br>[Testing status](docs/validation/0.5.1.md)<br>[Collapsible roadmap](#roadmap)<br>[Clickable release history](#version-notes)<br>[Contribute dialogue and quests](docs/CONTENT-AUTHORING.md) |

## Start playing

1. Download the mod above and place it in `Documents/My Games/Terraria/tModLoader-dev/Mods`.
2. Enable **Nyuu Terraria** in Manage Mods, then reload. Use the matching 1.4.5-dev build on every player's machine.
3. Start with your own backed-up world, or import the separate [Nyuu Showcase 051](docs/SHOWCASE.md) to try everything.
4. Open the Chronicle and complete **Lyra: a place to begin** with 8 Wood. Ready quests now have **Claim** buttons directly in Requests; materials can come from your inventory or backpack.

| Key | Action |
| --- | --- |
| **L** | Chronicle: quests, professions, wardrobe, stories and wildlife |
| **K** | Spellbook and casting modes |
| **V** | Equipped backpack storage; arrows change its page |
| **G** | Learned profession ability |
| **Escape** | Close Chronicle |

Existing custom key assignments are preserved. Change bindings in Settings → Controls → Mod Controls.

## Latest fixes

| Reported problem | v0.5.1 change | Details |
| --- | --- | --- |
| Backpack stops drawing partway through | Fixed vanilla's 40-slot navigation limit; 50 visible slots per page | [Release and tests](docs/releases/v0.5.1.md) |
| Backpack on cursor stays on armor tab | Apply Equipment-page selection during inventory drawing, after vanilla selection | [Backpacks](docs/PLAYER-GUIDE.md#backpacks) |
| Requests cannot be claimed where tracked | Direct Claim button, captured UI clicks, inventory/backpack hand-ins, duplicate-claim protection | [Quest instructions](docs/WORLDBOUND.md#stories-discoveries-and-rewards) |
| NPC walks backward or floats | Correct original artwork orientation and feet anchors; separate ground and flying movement | [Visual repair notes](docs/releases/v0.5.1.md) |
| Worn clothing and props sit incorrectly | Correct seven worn rigs and furniture baselines while retaining the established artwork | [Validation](docs/validation/0.5.1.md) |
| Expeditions are empty scaffold boxes | Native trees, themed terrain, water, gardens, ruins, shelves and starglass islands; no forest scaffold ledges | [Fresh showcase](docs/SHOWCASE.md) |

**Existing built expeditions stay intact.** The revised landscapes appear in newly created destinations and in the fresh showcase. Import the new showcase beside your old save; do not replace a world you care about.

## Choose your next adventure

| Progress | Try this | Read more |
| --- | --- | --- |
| Early game | Democloud equipment, Skylight, Amber Grotto, sequential backpack upgrades, Lyra's opening quests | [Survival and crafting guide](docs/PLAYER-GUIDE.md) |
| Growing your build | Elemental Aetherloom modes, spell mastery, five persistent professions and talent branches | [Magic and talents](docs/WORLDBOUND.md) |
| Exploration | Hollowglass caves, Reverie/Hush and four Worldbound pocket expeditions | [Destinations, resources and encounters](docs/WORLDBOUND.md) |
| Collecting and expression | Long hairstyles, pride cosmetics, outfits, pets, wildlife observations and memorial gardens | [Character and home](docs/PLAYER-GUIDE.md#lyra-pride-hairstyles-and-clothing) |

![Worldbound collection](docs/images/worldbound-preview.png)

## Roadmap

| Status | Latest completed work | Version |
| --- | --- | --- |
| 🟢 Added | Backpack rendering and Equipment routing, direct quest claims, original-sprite alignment, worn-rig repairs and revised expedition generation | [v0.5.1](docs/releases/v0.5.1.md) · [Test report](docs/validation/0.5.1.md) |

### Shipped in Worldbound Chapters

| Status | Playable addition | Version / guide |
| --- | --- | --- |
| 🟢 Added | Element reactions, Ritual Focus, infusions, Celestial Loom and four spell palettes | [v0.5.0](docs/releases/v0.5.0.md) · [Magic](docs/WORLDBOUND.md#magic-workshop) |
| 🟢 Added | Tidepool, Citadel, Cinderwood and Starglass pocket expeditions | [v0.5.0](docs/releases/v0.5.0.md) · [Travel](docs/WORLDBOUND.md#six-destinations-one-persistent-world) |
| 🟢 Added | Pale Gardener, Unwritten, two minibosses, four threats and two rematches | [v0.5.0](docs/releases/v0.5.0.md) · [Encounters](docs/WORLDBOUND.md#encounters-and-preparation) |
| 🟢 Added | 14 additional quests, four dialogue scenes, pinned tracking, noticeboards and profile labels | [v0.5.0](docs/releases/v0.5.0.md) · [Stories](docs/WORLDBOUND.md#stories-discoveries-and-rewards) |
| 🟢 Added | Five saved talent branches, free resets, active abilities and mastery rewards | [v0.5.0](docs/releases/v0.5.0.md) · [Talents](docs/WORLDBOUND.md#professions-and-talents) |
| 🟢 Added | Four peaceful creatures, observation reward, feeding bowls, beds and rest behavior | [v0.5.0](docs/releases/v0.5.0.md) · [Wildlife](docs/WORLDBOUND.md#wildlife-home-and-memorials) |
| 🟢 Added | Mira, three memorials, four outfits, furnishings, festival and convergence | [v0.5.0](docs/releases/v0.5.0.md) · [Home and events](docs/WORLDBOUND.md#events) |

Remaining yellow rows refer to **deeper follow-ups**, such as full-size dimensions, linked rituals, expanded patterns, larger quest branches and more event types. The playable first wave above is included in the download.


More expressive magic, places worth getting lost in, and stories that grow with your character. This is the plan, not a promise of release dates. Finished features live in the [playable feature index](#already-playable), separate from the future work below.

| Status | Meaning |
| --- | --- |
| 🟢 **Added** | Playable in the linked version. |
| 🟡 **In Progress** | Implementation has started, but the feature is unfinished. |
| ⚪ **Not Started** | Planned work that has not been implemented. |
| 🔵 **Considering** | An idea being explored; scope may change. |
| 🔴 **Dropped** | A canceled idea, kept only when its history is useful. |



| Build your character | Explore & encounter | Shape the world |
| --- | --- | --- |
| [Mage spellcraft](#mage-spellcraft-and-combat)<br>[Pride & expression](#trans-pride-and-character-expression)<br>[Levels & professions](#levels-skills-and-professions)<br>[Long hairstyles & clothes](#more-long-feminine-hairstyles-and-clothes) | [Dimensions & scenarios](#new-dimensions-and-scenarios)<br>[Bosses & enemies](#bosses-minibosses-and-enemies)<br>[Quests & stories](#quest-journal-and-npc-stories) | [Peaceful creatures & pets](#peaceful-creatures-and-pets)<br>[Graves & memorials](#graves-and-memorials)<br>[Other progression](#other-progression-ideas)<br>[Already playable](#already-playable) |

<a id="mage-spellcraft-and-combat"></a>

<details>
<summary><strong>Mage spellcraft and combat</strong></summary>

The first wave is [playable in v0.4.1](docs/PLAYER-GUIDE.md#v041). [Elaina: Wandering Witch](https://steamcommunity.com/sharedfiles/filedetails/?id=2850199955) inspired the direction: magic with a sense of practice, spectacle, and discovery. Nyuu's spells, artwork, and implementation belong to its own world.

| Status | Feature | What comes next |
| --- | --- | --- |
| 🟡 In Progress | Additional element combinations | Mark enemies with one element, then trigger a second for steam bursts, frozen blooms, and storm-charged tides. |
| ⚪ Not Started | More charged casts | Give frost, tide, and arcane spells their own held attacks, distinct release cues, and a reason to choose a quick cast. |
| 🟡 In Progress | Linked ritual networks | Place linked runes for a longer cast with strong visual warnings, material costs, and powerful effects. |
| ⚪ Not Started | Mage progression | Midgame catalysts, class armor, evolving wands, and late-game spell forms with clear boss gates. |
| 🔵 Considering | Witch's broom | A craftable flight tool with upgrades and its own movement identity. |
| 🟡 In Progress | Expanded cauldron discoveries | Brew temporary spell infusions and discover recipes through exploration and NPC stories. |
| 🔵 Considering | Party support | Targeted healing, support circles, and shared ward effects with balanced cooldowns. |
| 🟡 In Progress | Advanced celestial patterns | Sweeping beams, orbiting sigils, constellation storms, and a post-Moon Lord ritual weapon. |
| 🟡 In Progress | More spell and companion styles | Collectible trail palettes, casting-circle patterns, and companion effects that preserve readable attacks. |

</details>

<a id="trans-pride-and-character-expression"></a>

<details>
<summary><strong>Trans pride and character expression</strong></summary>

| Status | Feature | What comes next |
| --- | --- | --- |
| ⚪ Not Started | Lyra's atelier restoration | A longer story about rebuilding her workshop, finding patterns, and unlocking embroidered outfits and furniture. |
| ⚪ Not Started | Pride collections | More trans-pride capes, clips, hair ribbons, dyes, banners, lanterns, and matching pet accessories. |
| ⚪ Not Started | Prism Wardrobe expansion | Named appearance presets, favorite outfits, and previews before applying a style. |
| 🟡 In Progress | Expanded profile-aware dialogue | More character-expression choices, dialogue that respects them, and vanity versions of favorite equipment. |

</details>

<a id="new-dimensions-and-scenarios"></a>

<details>
<summary><strong>New dimensions and scenarios</strong></summary>

The current Reverie and Hush are **pocket expeditions inside your world**. The full destinations below are future expansions.

| Status | Place | Ideas to explore |
| --- | --- | --- |
| ⚪ Not Started | The Reverie — full dimension | Floating flower islands, crystal orchards, moonlit lakes, seasonal gardens, and an abandoned observatory. Restore paths and discover the mirror's history. |
| ⚪ Not Started | The Hush — horror chapter | An abandoned mining town, forgotten archive, and manor with shifting corridors. Investigate missing explorers, misleading lanterns, false rooms, and occasional jumpscares, with adjustable scare settings. |
| 🟡 In Progress | Tidepool: full coastal region | Coastal caves, pearl deposits, fishing pools, shell furniture, flooded shrines, and reef encounters. |
| 🟡 In Progress | Citadel: expanded fortress routes | A ruined fortress overtaken by roots and crystal. Open sealed gardens and reveal routes as bosses fall. |
| 🟡 In Progress | Cinderwood: full forge chapter | Ash-covered forests and buried forges near the Underworld, with ember creatures and abandoned workshops. |
| 🟡 In Progress | Starglass: observatory finale | Celestial ruins, unusual gravity, rare building materials, and the observatory story's final chapter. |

</details>

<a id="bosses-minibosses-and-enemies"></a>

<details>
<summary><strong>Bosses, minibosses, and enemies</strong></summary>

| Status | Encounter | What comes next |
| --- | --- | --- |
| ⚪ Not Started | Existing boss expansion | More distinct phases, readable cues, difficulty tuning, trophies, masks, and themed rewards. |
| 🔵 Considering | The Hollow Astronomer | Rotating constellations, marked gravity shifts, and celestial weapons in the observatory. |
| 🟡 In Progress | The Unwritten: expanded story mechanics | A late-game Hush fight that reshapes remembered locations and closes the archive's story. |
| 🔵 Considering | The Drowned Choir | A Tidepool boss built around waves, echoes, and shifting currents. |
| 🟡 In Progress | The Pale Gardener: expanded encounter | More garden mechanics for the existing Citadel guardian with flower hazards and fragile crystal blooms. |
| 🟡 In Progress | Further minibosses beyond the first two | An archive caretaker, reef ambusher, haunted armor patrol, and a creature inside abandoned mine machinery. |
| 🟡 In Progress | Additional biome enemy families | Reef skitterers, vinebound armor, lantern mimics, ash burrowers, and starfallen constructs. |
| 🟡 In Progress | More rematches and unique patterns | Optional stronger variants with new patterns and cosmetic rewards. |

</details>

<a id="quest-journal-and-npc-stories"></a>

<details>
<summary><strong>Quest journal and NPC stories</strong></summary>

| Status | Feature | What comes next |
| --- | --- | --- |
| 🟡 In Progress | Map hints and chapter navigation | Tracked objectives, map hints, chapter pages, and clearer reward previews. |
| 🟡 In Progress | Further story chapters | Lyra's atelier, the Hush missing-explorer mystery, and Hollowglass research. |
| 🟡 In Progress | Deeper branching dialogue packs | Extend the existing custom-content format with branching choices and scenes, leaving room for your own dialogue and unique quests. |
| ⚪ Not Started | Party story tracking | Follow the same chapter with friends while retaining individual rewards and saved progress. |
| 🟡 In Progress | Expanded noticeboard contract types | Exploration, gathering, fishing, crafting, and encounter contracts. |
| 🟡 In Progress | More letters and ruin discoveries | Letters, hidden shrines, explorer journals, and small stories attached to ruins. |
| 🟡 In Progress | Further cosmetic and route rewards | Hairstyles, pets, furniture patterns, housing decorations, and access to new routes. |

</details>

<a id="levels-skills-and-professions"></a>

<details>
<summary><strong>Levels, skills, and professions</strong></summary>

Switching already preserves each profession's progress. Future skills will build on that system.

| Status | Profession / feature | Possible skills |
| --- | --- | --- |
| 🟡 In Progress | Vanguard: guards and melee combos | Timed guards, defensive abilities, melee combos, and shields. |
| 🟡 In Progress | Ranger: traps and bow paths | Precision shots, ammunition recovery, traps, and bow paths. |
| 🟡 In Progress | Arcanist: advanced spell modifiers | Spell modifiers, mana control, and build choices beyond the new spell-mastery system. |
| 🟡 In Progress | Summoner: companion roles and commands | Companion commands, minion roles, and support effects. |
| 🟡 In Progress | Explorer: fishing and navigation skills | Treasure clues, mining techniques, fishing, gathering, and navigation tools. |
| 🟡 In Progress | More mastery cosmetics and presets | Cosmetics, milestone unlocks, saved skill presets, and profession crafting patterns, with easy resets. |

</details>

<a id="peaceful-creatures-and-pets"></a>

<details>
<summary><strong>Peaceful creatures and pets</strong></summary>

| Status | Feature | What comes next |
| --- | --- | --- |
| 🟡 In Progress | Richer habitat-specific wildlife behavior | Feeding, idle behavior, and favorite habitats for Dew Foxes, Glass Deer, Mothcats, and Mooncap Sprites. |
| 🟡 In Progress | Further peaceful species | Tidepool Turtles, lantern fireflies, garden birds, and tiny crystal snails. |
| 🟡 In Progress | Expanded pet habitats and variants | Beds, perches, bowls, habitats, and collectible color variants. |
| ⚪ Not Started | More companion discoveries | Pets unlocked through friendship quests, exploration, and rare finds. |
| 🟡 In Progress | More wildlife goals and garden visitors | Observation goals, discovery rewards, and gardens that attract visitors. |

</details>

<a id="graves-and-memorials"></a>

<details>
<summary><strong>Graves and memorials</strong></summary>

| Status | Feature | What comes next |
| --- | --- | --- |
| ⚪ Not Started | Memorial collections | More materials, biome shapes, decorative symbols, and flower arrangements. |
| 🟡 In Progress | Expanded death-grave variants | Graves that match the area where a character fell. |
| 🔵 Considering | Keepsake retrieval | An optional last-death marker and a small retrieval quest. |
| 🟡 In Progress | Expanded memorial garden collections | Pet memorials, candle stands, fences, benches, garden bells, and hanging lanterns. |
| 🟡 In Progress | Mira: further restoration chapters | A peaceful NPC with restoration requests and small stories. |

</details>

<a id="more-long-feminine-hairstyles-and-clothes"></a>

<details>
<summary><strong>More long feminine hairstyles and clothes</strong></summary>

| Status | Collection | What comes next |
| --- | --- | --- |
| ⚪ Not Started | Long flowing styles | Hime cuts, waterfall braids, extra-long straight hair, side ponytails, and soft wolf cuts. |
| ⚪ Not Started | Braids and tied styles | Fishtail and double Dutch braids, twin ponytails, long locs, and loose buns with trailing strands. |
| ⚪ Not Started | Animation and hats | Movement frames, clean dye support, and matching versions under headwear for every new style. |
| 🔵 Considering | Atelier wardrobe | Embroidered dresses, long skirts, layered coats, stockings, oversized sweaters, and matching ribbons. |
| 🟡 In Progress | More seasonal and gothic outfits | Moonlit formalwear, coastal clothes, cozy winter sets, and gothic Hush fashion. |

</details>

<a id="other-progression-ideas"></a>

<details>
<summary><strong>Other progression ideas</strong></summary>

| Status | Stage / area | Ideas for later |
| --- | --- | --- |
| ⚪ Not Started | Early game | More ore bows, boomerangs, summons, and tools; starter ruins, fishing rewards, and exploration trinkets. |
| ⚪ Not Started | Middle game | Class armor with active skills, boss-driven biome upgrades, weapon evolution, and a relic forge. |
| 🔵 Considering | Late game | Celestial dungeons, legendary gear, unusual mounts, and challenges beyond Moon Lord. |
| 🟡 In Progress | More furniture and collectibles | Furniture sets, plants, paintings, music boxes, vases, relics, and structures to restore. |
| ⚪ Not Started | Storage and equipment | Storage search, sorting, clearer crafting hints, equipment previews, and easier loadout management. |
| 🟡 In Progress | Further caravans, weather and haunted events | Caravans, strange weather, garden festivals, haunted nights, and roaming encounters. |

</details>

<a id="already-playable"></a>

<details>
<summary><strong>Already playable</strong></summary>

| Status | Feature | Added in | Guide |
| --- | --- | --- | --- |
| 🟢 Added | Aetherloom Wand, six casting modes, five learning tomes, spell mastery, charged Emberbloom, Crescent Ward, and Spellbook | [v0.4.1](docs/PLAYER-GUIDE.md#v041) | [Spellcraft guide](docs/PLAYER-GUIDE.md#spellcraft) |
| 🟢 Added | Lyra, pride ribbon, profile customization, Prism Wardrobe, and eight long hairstyles | [v0.4.0](docs/PLAYER-GUIDE.md#v040) | [Character expression](docs/PLAYER-GUIDE.md#lyra-pride-hairstyles-and-clothing) |
| 🟢 Added | Twelve quests, custom story data, and five switchable professions with saved progress | [v0.4.0](docs/PLAYER-GUIDE.md#v040) | [Journal and professions](docs/PLAYER-GUIDE.md#journal-stories-and-professions) |
| 🟢 Added | Reverie and Hush pocket expeditions, adjustable scares, and persistent realm loot | [v0.4.0](docs/PLAYER-GUIDE.md#v040) | [First expedition](docs/PLAYER-GUIDE.md#chronicle-expeditions) |
| 🟢 Added | Three expedition bosses, three minibosses, peaceful wildlife, pets, and crafted memorials | [v0.4.0](docs/PLAYER-GUIDE.md#v040) | [Expedition encounters](docs/PLAYER-GUIDE.md#expedition-bosses-and-minibosses) |
| 🟢 Added | Sequential backpack upgrades that require empty storage; matching bomb artwork | [v0.3.1](docs/PLAYER-GUIDE.md#v031) | [Backpacks](docs/PLAYER-GUIDE.md#backpacks) |
| 🟢 Added | Six backpack tiers, Equipment slot, V-key storage, Hollowglass Reach and its two bosses | [v0.3.0](docs/PLAYER-GUIDE.md#v030) | [Hollowglass Reach](docs/PLAYER-GUIDE.md#hollowglass-reach) |
| 🟢 Added | Amber Grotto, Verdigris, Ember Amber, cave creatures, pets, tools, and pendants | [v0.2.4](docs/PLAYER-GUIDE.md#v024) | [Amber Grotto](docs/PLAYER-GUIDE.md#amber-grotto) |
| 🟢 Added | Stormglass, Frostveil, Cloudvault storage, Satchel, and Belt | [v0.2.3](docs/PLAYER-GUIDE.md#v023) | [Crafting guide](docs/PLAYER-GUIDE.md#complete-crafting-guide) |
| 🟢 Added | Skylight, Nimbus Warden, armor, trinkets, and vanity | [v0.2.0](docs/PLAYER-GUIDE.md#v020) | [Boss progression](docs/PLAYER-GUIDE.md#boss-progression) |
| 🟢 Added | Democloud wings, staff, boots, and Cloudy Bombs | [v0.1.x](docs/PLAYER-GUIDE.md#v01x) | [Democloud recipes](docs/PLAYER-GUIDE.md#complete-crafting-guide) |

</details>

## Version notes

<details open>
<summary><strong>Release history — v0.1.x through v0.5.0</strong></summary>

| Version | Added or changed |
| --- | --- |
| **[v0.5.1](docs/releases/v0.5.1.md)** | Backpack rendering and cursor tab fixes; direct quest claims and backpack hand-ins; NPC direction and grounding; preserved artwork, corrected worn rigs and props; six revised expedition landscapes; refreshed showcase and guide navigation. |
| **[v0.5.0](docs/releases/v0.5.0.md)** | Worldbound Chapters: six total destinations, reaction magic, rituals, cauldron infusions, Celestial Loom, two bosses and minibosses, talents, 14 more quests, four choice scenes, wildlife care, memorials, outfits, events, visual and interaction fixes, and an all-items showcase save. |
| **[v0.4.1](docs/releases/v0.4.1.md)** | Added Aetherloom Wand, six casting modes, five learning tomes, K-key Spellbook, saved spell mastery, charged Emberbloom, piercing frost, splitting water, chained lightning, healing, Crescent Ward, Ward Sigil, and the status-table roadmap. |
| **[v0.4.0](docs/releases/v0.4.0.md)** | Backpack flicker and Equipment-tab placement fixes, corrected accessory scrolling, larger bar icons; Lyra, pride customization, eight long hairstyles, 12 quests, five persistent professions, Reverie/Hush pocket expeditions with optional scares, three bosses, three minibosses, wildlife, pets, Moonprism, weapons, and memorials. |
| <a id="v031"></a> **[v0.3.1](docs/releases/v0.3.1.md)** | Sequential backpack recipes with an empty-storage requirement, a Glass Slate guide icon, and new Warden, Lens, Cloudtail, and matching bomb artwork. |
| <a id="v030"></a> **[v0.3.0](docs/releases/v0.3.0.md)** | Six backpacks, a dedicated Backpack slot and V-key storage window, Hollowglass Reach, two bosses, ruin loot, equipment, a pet, and exterior sprite cleanup. |
| **[v0.2.6](docs/releases/v0.2.6.md)** | Cleaned up the Amberflare Staff sprite, added individual armor and vanity icons, and moved the Warden set bonus into the armor section. |
| **[v0.2.5](docs/releases/v0.2.5.md)** | Full player guide. Redrawn Amberflare Staff, Amberheart Pendant, and Deepheart Pendant. |
| <a id="v024"></a> **[v0.2.4](docs/releases/v0.2.4.md)** | Amber Grotto, Amber Urns, Verdigris and Ember Amber, two cave enemies, two pets, tools, weapons, and pendants. |
| <a id="v023"></a> **[v0.2.3](docs/releases/v0.2.3.md)** | Stormglass and Frostveil, Cloudvault, Cloud Satchel, Cloudweave Belt, three hairstyles, and cooler Skylight artwork. |
| **[v0.2.2](docs/releases/v0.2.2.md)** | Redrawn Skylight items, equipment, boss, and spell sprites. |
| **[v0.2.1](docs/releases/v0.2.1.md)** | Updated Warden armor and Yuuki’s boots for the current armor set API. |
| <a id="v020"></a> **[v0.2.0](docs/releases/v0.2.0.md)** | Skylight ore, Nimbus Warden, Warden armor, weapons, trinkets, slots, chest, vanity, and two hairstyles. |
| <a id="v01x"></a> **[v0.1.x](docs/releases/v0.1.x.md)** | Wings and Staff of Democloud, Yuuki’s boots, Cloudy Bombs, and Steam achievement sync. |

</details>



[Back to navigation](#find-your-way)
