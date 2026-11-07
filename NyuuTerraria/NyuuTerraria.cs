using System;
using System.IO;
using Terraria;
using Terraria.ID;
using Terraria.ModLoader;
using NyuuTerraria.Content.Reverie;

namespace NyuuTerraria;
public class NyuuTerraria : Mod
{
    public override void HandlePacket(BinaryReader reader, int whoAmI)
    {
        byte kind=reader.ReadByte();
        if(kind==0 && Main.netMode==NetmodeID.Server)
        {
            int realm=reader.ReadByte();
            Player player=Main.player[whoAmI];
            if(realm<2 && player.active && !player.dead && player.HeldItem.ModItem is RealmKey key && key.Realm==realm)
                ModContent.GetInstance<RealmWorld>().Travel(player,realm);
        }
        else if(kind==1 && Main.netMode==NetmodeID.MultiplayerClient)
        {
            string key=reader.ReadString(); int xp=reader.ReadInt32();
            var data=Main.LocalPlayer.GetModPlayer<ChroniclePlayer>();
            data.Record("defeat-enemies"); data.Record(key); data.AddExperience(xp);
        }
        else if(kind==3 && Main.netMode==NetmodeID.MultiplayerClient)
        {
            int spell=reader.ReadByte(),amount=reader.ReadInt32();
            if(spell==global::NyuuTerraria.Content.Spellcraft.Spells.Ward)
                Main.LocalPlayer.GetModPlayer<global::NyuuTerraria.Content.Spellcraft.SpellcraftPlayer>().GainExperience(spell,Math.Clamp(amount,0,4));
        }
        else if(kind==2)
        {
            int playerId=reader.ReadByte();
            if(playerId<0 || playerId>=Main.maxPlayers) return;
            if(Main.netMode==NetmodeID.Server && playerId!=whoAmI) return;
            var data=Main.player[playerId].GetModPlayer<ChroniclePlayer>();
            data.Profession=Math.Clamp((int)reader.ReadByte(),0,4);
            for(int i=0;i<5;i++) data.Skills[i]=Math.Clamp((int)reader.ReadByte(),0,50);
            if(Main.netMode==NetmodeID.Server) data.SyncProfession(-1,whoAmI);
        }
    }
}
