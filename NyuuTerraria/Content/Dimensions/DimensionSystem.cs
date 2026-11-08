using System;
using System.Collections.Generic;
using System.IO;
using System.Reflection;
using System.Threading.Tasks;
using Microsoft.Xna.Framework;
using Terraria;
using Terraria.ID;
using Terraria.Utilities;
using Terraria.IO;
using Terraria.ModLoader;
using Terraria.ModLoader.IO;
using Terraria.GameContent.Generation;
using Terraria.WorldBuilding;

namespace NyuuTerraria.Content.Dimensions;

/// <summary>
/// A destination that lives in its own world file, like the Nether: nothing of it is built in the
/// player's home world. Each home world gets its own copy, stored beside the world list in
/// Worlds/NyuuDimensions/&lt;home world id&gt;/&lt;Id&gt;.wld, so it never shows up in world select.
/// </summary>
public abstract class Dimension
{
    /// <summary>File name and save key. Never change it once released.</summary>
    public abstract string Id { get; }
    public abstract string DisplayName { get; }
    public virtual int Width => 1600;
    public virtual int Height => 900;

    /// <summary>The RealmWorld realm the whole dimension counts as (biome, sky, spawns), or -1.</summary>
    public virtual int Realm => -1;

    /// <summary>The FrontierWorld area the whole dimension counts as, or -1.</summary>
    public virtual int Area => -1;

    /// <summary>Builds the whole world. Runs once, the first time anyone from that home world enters.</summary>
    public abstract void Generate(GenerationProgress progress);
}

/// <summary>
/// Switches the single-player game between the home world and dimension worlds.
///
/// Going in:  remember where the player stood, save and quit the home world through vanilla's
///            own SaveAndQuit, then load the dimension (creating it on the first visit).
/// Coming out: save and quit the dimension, load the home world, put the player back where
///            they left.
/// Multiplayer keeps the older in-world pocket destinations: a server can only run one world.
/// </summary>
public class DimensionSystem : ModSystem
{
    internal static readonly Dimension[] All = { new ReverieDimension() };

    /// <summary>Set while a dimension is being generated, so world gen runs only its passes.</summary>
    internal static Dimension Generating;

    private static bool switching;
    private static Progression carried;
    private static string cachedPath;
    private static Dimension cachedDimension;

    internal static string Root => Path.Combine(Main.WorldPath, "NyuuDimensions");

    internal static string PathFor(Guid home, Dimension d) => Path.Combine(Root, home.ToString("N"), d.Id + ".wld");

    /// <summary>The dimension the loaded world is, or null in a normal world.</summary>
    internal static Dimension Current
    {
        get
        {
            string path = Main.ActiveWorldFileData?.Path;
            if (path == cachedPath) return cachedDimension;
            cachedPath = path;
            cachedDimension = null;
            if (path != null && Path.GetFullPath(path).StartsWith(Path.GetFullPath(Root), StringComparison.OrdinalIgnoreCase))
            {
                string id = Path.GetFileNameWithoutExtension(path);
                foreach (Dimension d in All)
                    if (d.Id == id) cachedDimension = d;
            }
            return cachedDimension;
        }
    }

    internal static Dimension ForRealm(int realm)
    {
        foreach (Dimension d in All)
            if (d.Realm == realm) return d;
        return null;
    }

    public override void OnWorldLoad()
    {
        switching = false;
        cachedPath = null;
        if (Current != null && carried != null)
            carried.Apply();
    }

    public override void OnWorldUnload()
    {
        cachedPath = null;
        cachedDimension = null;
    }

    // ---------------------------------------------------------------- travel

    internal static bool CanTravel(Player player)
    {
        if (switching || Main.netMode != NetmodeID.SinglePlayer) return false;
        if (player.dead || Main.ActiveWorldFileData == null) return false;
        foreach (NPC n in Main.npc)
            if (n.active && n.boss) return false;
        return true;
    }

    internal static void Enter(Dimension d)
    {
        Player player = Main.LocalPlayer;
        if (!CanTravel(player) || Current != null) return;

        var data = player.GetModPlayer<DimensionPlayer>();
        data.HomePath = Main.ActiveWorldFileData.Path;
        data.HomeId = Main.ActiveWorldFileData.UniqueId.ToString();
        data.ReturnPosition = player.position;
        data.PendingReturn = true;

        carried = Progression.Capture();
        int gameMode = Main.ActiveWorldFileData.GameMode;
        string path = PathFor(Main.ActiveWorldFileData.UniqueId, d);
        switching = true;
        Main.NewText("The " + d.DisplayName + " opens around you...", new Color(211, 170, 255));
        WorldGen.SaveAndQuit(() => Open(d, path, gameMode));
    }

    internal static void ReturnHome()
    {
        Player player = Main.LocalPlayer;
        if (!CanTravel(player) || Current == null) return;

        string home = player.GetModPlayer<DimensionPlayer>().HomePath;
        switching = true;
        WorldGen.SaveAndQuit(() =>
        {
            if (!string.IsNullOrEmpty(home) && File.Exists(home))
            {
                WorldFileData data = WorldFile.GetAllMetadata(home, false);
                if (data != null && data.IsValid)
                {
                    data.SetAsActive();
                    WorldGen.playWorld();
                    return;
                }
            }
            // Home world was moved or deleted: the safest place to leave the player is the menu.
            switching = false;
            Main.menuMode = 0;
        });
    }

    private static void Open(Dimension d, string path, int gameMode)
    {
        try
        {
            Directory.CreateDirectory(Path.GetDirectoryName(path));
            if (File.Exists(path))
            {
                WorldFileData existing = WorldFile.GetAllMetadata(path, false);
                if (existing != null && existing.IsValid)
                {
                    existing.SetAsActive();
                    WorldGen.playWorld();
                    return;
                }
            }
            Create(d, path, gameMode);
        }
        catch (Exception e)
        {
            ModContent.GetInstance<DimensionSystem>().Mod.Logger.Error("Could not open dimension " + d.Id, e);
            switching = false;
            Generating = null;
            Main.menuMode = 0;
        }
    }

    private static readonly FieldInfo PathField = typeof(FileData).GetField("_path", BindingFlags.Instance | BindingFlags.NonPublic);

    /// <summary>
    /// Makes the dimension's world file. This is our own small "subworld" pipeline: vanilla world
    /// generation always runs its Reset step first, which assumes a normal-size world and crashes on
    /// a small dimension, and it would also run every other mod's passes. Here only the dimension's
    /// own generator runs, behind the normal world-loading screen, and the result is saved like any world.
    /// </summary>
    private static void Create(Dimension d, string path, int gameMode)
    {
        WorldFileData data = NewMetadata(d, path, gameMode);
        data.SetAsActive();
        Main.worldName = d.DisplayName;

        var progress = new GenerationProgress { TotalWeight = 1 };
        Main.menuMode = 888;
        Main.MenuUI.SetState(new DimensionLoading(progress, d.DisplayName));

        Task.Run(() =>
        {
            bool ok = Build(d, data, progress);
            Main.QueueMainThreadAction(() =>
            {
                WorldFileData made = ok ? WorldFile.GetAllMetadata(path, false) : null;
                if (made != null && made.IsValid)
                {
                    made.SetAsActive();
                    WorldGen.playWorld();
                }
                else
                {
                    switching = false;
                    Main.menuMode = 0;
                }
            });
        });
    }

    internal static WorldFileData NewMetadata(Dimension d, string path, int gameMode)
    {
        WorldFileData data = WorldFile.CreateMetadata(d.DisplayName, false, gameMode);
        PathField.SetValue(data, path);
        data.SetWorldSize(d.Width, d.Height);
        // Vanilla's Reset step would pick this; without it the dimension would share the home world's
        // id, and with it the player's bed spawn and map.
        data.WorldId = new Random().Next(1, int.MaxValue);
        return data;
    }

    /// <summary>Generates and saves a dimension into the active world data. Runs off the main thread.</summary>
    internal static bool Build(Dimension d, WorldFileData data, GenerationProgress progress)
    {
        try
        {
            Generating = d;
            WorldGen.generatingWorld = true;
            WorldGen.isGeneratingOrLoadingWorld = true;
            WorldGen.generatingWorldOnThisThread = true;
            Main.rand = new UnifiedRandom(data.Seed);
            Main.maxTilesX = d.Width;
            Main.maxTilesY = d.Height;
            WorldGen.setWorldSize();
            WorldGen.clearWorld();
            progress.Start(1);
            d.Generate(progress);
            progress.End();
        }
        catch (Exception e)
        {
            ModContent.GetInstance<DimensionSystem>().Mod.Logger.Error("Generating dimension " + d.Id + " failed", e);
            return false;
        }
        finally
        {
            Generating = null;
            WorldGen.generatingWorld = false;
            WorldGen.isGeneratingOrLoadingWorld = false;
            WorldGen.generatingWorldOnThisThread = false;
        }
        WorldFile.SaveNewWorld();
        return true;
    }

    /// <summary>
    /// Boss and hardmode progress from the home world, so gates inside a dimension match where
    /// the player actually is in the game. Copied in; never written back.
    /// </summary>
    private sealed class Progression
    {
        private bool hardMode, slime, eye, evil, skeletron, bee, mech, plantera, golem, cultist, moonLord;

        internal static Progression Capture() => new()
        {
            hardMode = Main.hardMode, slime = NPC.downedSlimeKing, eye = NPC.downedBoss1, evil = NPC.downedBoss2,
            skeletron = NPC.downedBoss3, bee = NPC.downedQueenBee, mech = NPC.downedMechBossAny,
            plantera = NPC.downedPlantBoss, golem = NPC.downedGolemBoss, cultist = NPC.downedAncientCultist,
            moonLord = NPC.downedMoonlord
        };

        internal void Apply()
        {
            Main.hardMode |= hardMode; NPC.downedSlimeKing |= slime; NPC.downedBoss1 |= eye; NPC.downedBoss2 |= evil;
            NPC.downedBoss3 |= skeletron; NPC.downedQueenBee |= bee; NPC.downedMechBossAny |= mech;
            NPC.downedPlantBoss |= plantera; NPC.downedGolemBoss |= golem; NPC.downedAncientCultist |= cultist;
            NPC.downedMoonlord |= moonLord;
        }
    }
}

/// <summary>Where to send the player when they come home. Saved with the character.</summary>
public class DimensionPlayer : ModPlayer
{
    internal string HomePath = "";
    internal string HomeId = "";
    internal Vector2 ReturnPosition;
    internal bool PendingReturn;

    public override void OnEnterWorld()
    {
        if (!PendingReturn || DimensionSystem.Current != null || Main.netMode != NetmodeID.SinglePlayer) return;
        if (Main.ActiveWorldFileData?.UniqueId.ToString() != HomeId) return;
        PendingReturn = false;

        Vector2 spot = ReturnPosition;
        if (!WorldGen.InWorld((int)spot.X / 16, (int)spot.Y / 16, 20) || Collision.SolidCollision(spot, Player.width, Player.height))
            return; // keep the normal spawn point rather than drop the player into a wall
        Player.Teleport(spot, TeleportationStyleID.MagicConch);
        Player.velocity = Vector2.Zero;
        Player.fallStart = (int)(spot.Y / 16);
    }

    public override void SaveData(TagCompound tag)
    {
        tag["dimHome"] = HomePath;
        tag["dimHomeId"] = HomeId;
        tag["dimReturn"] = ReturnPosition;
        tag["dimPending"] = PendingReturn;
    }

    public override void LoadData(TagCompound tag)
    {
        HomePath = tag.GetString("dimHome");
        HomeId = tag.GetString("dimHomeId");
        ReturnPosition = tag.Get<Vector2>("dimReturn");
        PendingReturn = tag.GetBool("dimPending");
    }
}
