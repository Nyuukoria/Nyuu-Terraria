using System;
using System.Collections.Generic;
using System.Reflection;
using Terraria;
using Terraria.Achievements;
using Terraria.ModLoader;
using Terraria.Social;

namespace NyuuTerraria.Content;

public class SteamAchievementSync : ModSystem
{
    private readonly HashSet<string> submitted = new(StringComparer.Ordinal);
    private AchievementManager manager;
    private Action<string> sendCommand;

    public override void PostSetupContent()
    {
        if (Main.dedServ || SocialAPI.Mode != SocialMode.Steam || Main.Achievements == null)
            return;

        const BindingFlags flags = BindingFlags.Static | BindingFlags.NonPublic;
        Type client = typeof(Main).Assembly.GetType("Terraria.ModLoader.Engine.TerrariaSteamClient");
        MethodInfo send = client?.GetMethod("SendCmd", flags, null, new[] { typeof(string) }, null);
        if (send == null || client.GetField("serverPipe", flags)?.GetValue(null) == null)
        {
            Mod.Logger.Warn("Terraria's Steam achievement connection is unavailable.");
            return;
        }

        sendCommand = send.CreateDelegate<Action<string>>();
        manager = Main.Achievements;
        manager.OnAchievementCompleted += Submit;
    }

    public override void OnWorldLoad()
    {
        if (manager == null)
            return;

        foreach (Achievement achievement in manager.CreateAchievementsList())
            Submit(achievement);
    }

    private void Submit(Achievement achievement)
    {
        if (!achievement.IsCompleted || achievement.ModAchievement != null ||
            !submitted.Add(achievement.Name))
            return;

        try
        {
            sendCommand("grant:" + achievement.Name);
        }
        catch (Exception ex)
        {
            submitted.Remove(achievement.Name);
            Mod.Logger.Warn($"Couldn't forward achievement {achievement.Name} to Steam: {ex.Message}");
        }
    }

    public override void Unload()
    {
        if (manager != null)
            manager.OnAchievementCompleted -= Submit;

        manager = null;
        sendCommand = null;
        submitted.Clear();
    }
}
