using System;
using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Graphics;
using Terraria;
using Terraria.GameContent;
using Terraria.ModLoader;
using NyuuTerraria.Content.Reverie;

namespace NyuuTerraria.Content.Spellcraft;

internal static class SpellbookView
{
    private static void Text(string value,Vector2 position,Color color,float size=.72f)=>Utils.DrawBorderString(Main.spriteBatch,value,position,color,size);
    internal static void Draw(Rectangle panel)
    {
        var data=Main.LocalPlayer.GetModPlayer<SpellcraftPlayer>();int x=panel.X+18,y=panel.Y+93;
        Text("Choose a learned spell. Hold right-click with the wand for Ward.",new Vector2(x,y),Color.White,.67f);
        for(int i=0;i<Spells.Count;i++)
        {
            int top=y+26+i*39;bool learned=data.Learned[i];Color color=learned ? Spells.Colors[i] : Color.Gray;
            Texture2D texture=ModContent.Request<Texture2D>("NyuuTerraria/Content/Spellcraft/"+Spells.Icons[i]).Value;
            Main.spriteBatch.Draw(texture,new Vector2(x+13,top+15),null,learned ? Color.White : Color.Gray,0,texture.Size()*.5f,Math.Min(26f/texture.Width,26f/texture.Height),SpriteEffects.None,0);
            Text(Spells.Names[i],new Vector2(x+34,top),color,.72f);
            Text(learned ? $"Mastery {data.EffectiveLevel(i)}/{data.LevelCap}" : "Learn its tome to unlock",new Vector2(x+34,top+17),color,.61f);
            int level=data.EffectiveLevel(i),from=Spells.ExperienceForLevel(level),to=Spells.ExperienceForLevel(level+1);
            float progress=level>=data.LevelCap ? 1 : Math.Clamp((data.Experience[i]-from)/(float)(to-from),0,1);
            Main.spriteBatch.Draw(TextureAssets.MagicPixel.Value,new Rectangle(x+231,top+19,170,5),new Color(54,45,70));
            Main.spriteBatch.Draw(TextureAssets.MagicPixel.Value,new Rectangle(x+231,top+19,(int)(170*progress),5),color);
            Text(learned ? (level>=data.LevelCap ? "Tier mastered" : $"XP {data.Experience[i]}/{to}") : "Locked",new Vector2(x+231,top),color,.59f);
            if(i==Spells.Ward) Text("Hold RMB",new Vector2(x+437,top+6),color,.7f);
            else if(ChronicleInterface.Button(!learned ? "Locked" : data.SelectedSpell==i ? "Selected" : "Select",new Rectangle(x+427,top+1,125,30)))
                data.Select(i);
        }
        Text("K opens Spellbook. Books are learned once per character.",new Vector2(x,panel.Bottom-47),new Color(204,189,227),.66f);
        Text("Power caps at mastery 5 before Hardmode, then 10. Progress stays saved.",new Vector2(x,panel.Bottom-26),new Color(169,205,225),.6f);
    }
}
