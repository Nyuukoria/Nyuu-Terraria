# Adding dialogue and quests

The source folder is `NyuuTerraria`. Change its content files, then rebuild the mod in the same tModLoader 1.4.5-dev build used by the other players.

## Lyra's dialogue

Edit `Data/lyra-dialogue.json`. It is a JSON array of strings. Lyra chooses a line when spoken to.

```json
[
  "Hello, {player}. Your profile says {pronouns}; tell me whenever that changes.",
  "I found a piece of embroidered cloth beside the old mirror. Could it belong to the atelier?"
]
```

`{player}` inserts the Terraria character name. `{pronouns}` inserts their optional profile setting. Dialogue does not grant rewards or change progress by itself. Add the corresponding quest separately if a line describes a task. Use normal JSON escaping for quotes and newlines.

## Quest definitions

Edit `Data/quests.json`. Each quest has a permanent, unique `Id`. Keep existing IDs when updating a quest: completed IDs are saved per character.

```json
{
  "Id": "atelier-your-story-1",
  "Title": "A pattern from home",
  "Description": "Bring 6 Moonprism Ore for Lyra's next design.|Gather requests consume their materials on Claim.",
  "Requires": "atelier-3",
  "ModItem": "MoonprismOre",
  "Count": 6,
  "Experience": 150,
  "Reward": "PrismLongcoat",
  "RewardCount": 1
}
```

| Field | Meaning |
| --- | --- |
| `Id` | Unique stable save key. Never reuse an old ID for a different reward. |
| `Title` | Short journal heading. |
| `Description` | Up to two short lines. Separate them with `\|` in prose descriptions; in actual JSON use the plain `|` character, as above. Keep each line around 65 characters or fewer. |
| `Requires` | Optional prerequisite quest ID. Omit it to make a quest available immediately. |
| `ModItem` | Internal name of a Nyuu Terraria item to gather. |
| `Item` | Alternatively, a numeric vanilla Terraria item ID. Wood is `9`; Silk is `225`. Omit `ModItem` when using this field. |
| `Event` | For an exploration or combat objective instead of a gathering request. Omit `Item` and `ModItem` for an event objective. |
| `Count` | Required item quantity or event count; defaults to 1. |
| `Experience` | XP awarded to the player's currently selected profession. |
| `Reward` | Internal name of a Nyuu Terraria reward item. |
| `RewardCount` | Reward quantity; defaults to 1. |

Supported event keys include `visit-reverie`, `visit-hush`, `defeat-enemies`, and `defeat-` followed by a mod NPC's internal name. Examples: `defeat-RootboundKnight`, `defeat-Bellkeeper`, `defeat-PrismMatriarch`, `defeat-LanternStalker`, `defeat-TheMirrorless`, and `defeat-ReefbackGuardian`. Vanilla enemies use their numeric NPC ID after `defeat-`.

Gather requests inspect the main inventory and consume the requested items on Claim. Event counters record progress before a quest is unlocked. Defeat credit goes to nearby players who damaged the enemy. Rewards are claimed individually and cannot be claimed twice. The current authoring format supports one objective and one item reward per quest. New objective kinds, branching conversations, and scripted scenes need corresponding code support.

## Professions

The five profession indices are Vanguard (0), Ranger (1), Arcanist (2), Summoner (3), and Explorer (4). `ChroniclePlayer` saves all five XP totals and skill investments separately; changing the active index does not clear them. Keep these indices stable in future versions. New skill designs belong in `PostUpdateEquips` and the journal's descriptions together.

## Art and names

Item, NPC, tile, and hairstyle source sprites live beside their classes under `Content/Reverie`. Sprites use Terraria's 2-pixel grid and transparent backgrounds. Hair uses 40 × 784 sheets with 14 animation rows; hats use a matching `_Alt` sheet. Lyra and Mira use four 40 × 56 frames stacked vertically. Creature sheets use four equal-height frames; documentation displays a cropped first frame. Clothing uses the existing composite body and leg rig layouts. Do not resize an animation sheet as if it were a single inventory icon.

Player-visible names and tooltips are in `Localization/en-US_Mods.NyuuTerraria.hjson`. Add localization alongside new content.

## Current expedition scope

The Reverie and Hush in v0.4.0 are persistent pocket areas inside the current world. Generation checks for unused sky space and does not replace existing terrain. Changing their generation code affects newly created areas; it does not automatically rebuild an existing expedition. Back up test worlds before experimenting. Full separate-world dimensions and the longer story locations remain future milestones.


## Worldbound story packs — v0.5.0

Create `Data/packs/your-pack.json`. Its root object contains `Quests` and `Scenes` arrays. Quest fields match `Data/quests.json`; use permanent unique IDs and valid reward internal names. Packs load alphabetically. The bundled `worldbound.json` is a complete working example with 14 quests and four scenes.

```json
{
  "Quests": [{"Id":"custom-a", "Title":"A quiet beginning", "Description":"Speak in Worldbound Stories.", "Count":1, "Experience":100, "Reward":"FieldJournal", "RewardCount":1, "Event":"custom-choice"}],
  "Scenes": [{"Id":"custom-scene-a", "Speaker":"Lyra", "Text":"What shall we make?", "Requires":"", "Choices":[{"Text":"A place to rest", "Event":"custom-choice", "Reply":"Let us begin."}]}]
}
```

Scenes support up to two choices and an optional completed-quest `Requires` gate. Choosing records its event once per scene, with a saved discovery ID; repeated clicks cannot grant additional quest credit. This supports prerequisite-based chapters and choice replies, rather than arbitrary branching graphs. Keep existing IDs when editing a published pack. Rebuild the mod and use the same pack on every multiplayer participant.

Worldbound content lives in `Content/Frontiers`. New furniture must use matching tile dimensions and 18-pixel coordinate spacing. Mod development tests belong outside the release source folder. The showcase exporter is published under `tests/` and must never be bundled into the playable mod.
