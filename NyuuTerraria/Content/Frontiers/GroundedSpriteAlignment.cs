using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Graphics;
using Terraria;
using Terraria.GameContent;
using Terraria.ModLoader;
namespace NyuuTerraria.Content.Frontiers;
public class GroundedSpriteAlignment : GlobalNPC
{
    public override bool PreDraw(NPC npc,SpriteBatch batch,Vector2 screenPos,Color drawColor)
    {
        if(npc.ModNPC?.Mod!=Mod||LivingAnimation.Rigs.ContainsKey(npc.ModNPC.Name)||npc.noGravity||!npc.townNPC&&!CreatureAnimationSetup.Names.Contains(npc.ModNPC.Name))return true;
        var texture=TextureAssets.Npc[npc.type].Value;var frame=npc.frame;
        if(frame.Width<=0||frame.Height<=0)frame=new Rectangle(0,0,texture.Width,texture.Height/Main.npcFrameCount[npc.type]);
        batch.Draw(texture,npc.Bottom-screenPos,frame,drawColor,0,new Vector2(frame.Width/2f,frame.Height),npc.scale,npc.spriteDirection==1?SpriteEffects.FlipHorizontally:SpriteEffects.None,0);
        return false;
    }
}
