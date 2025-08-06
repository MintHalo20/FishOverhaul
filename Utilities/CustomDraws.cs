using System;
using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Graphics;
using Terraria;
using Terraria.GameContent;
using Terraria.ID;

namespace YourMod.Utilities
{
    public static class ItemDrawHelper
    {
        public static bool DrawFloatingItemWithPulse(
            Item item,
            SpriteBatch spriteBatch,
            ref float rotation,
            ref float scale,
            int whoAmI
        )
        {
            Texture2D texture = TextureAssets.Item[item.type].Value;
            Vector2 position = item.Center - Main.screenPosition;
            Rectangle frame = texture.Frame();
            Vector2 origin = frame.Size() / 2f;

            // Bobbing animation (like treasure bags)
            float time = Main.GlobalTimeWrappedHourly * 2f;
            float offsetY = (float)Math.Sin(time) * 2f;
            position.Y += offsetY;

            // Lighting color at the item's tile position
            Color color = Lighting.GetColor(
                (int)(item.position.X + item.width * 0.5f) / 16,
                (int)(item.position.Y + item.height * 0.5f) / 16
            );

            // Draw main texture
            spriteBatch.Draw(
                texture,
                position,
                frame,
                color,
                rotation,
                origin,
                scale,
                SpriteEffects.None,
                0f
            );

            // Optional pulse effect
            if (ItemID.Sets.ItemIconPulse[item.type])
            {
                float pulseScale = 1f + (float)Math.Sin(time) * 0.05f;
                spriteBatch.Draw(
                    texture,
                    position,
                    frame,
                    Color.White * 0.6f,
                    rotation,
                    origin,
                    scale * pulseScale,
                    SpriteEffects.None,
                    0f
                );
            }

            return false; // Prevents default drawing
        }
    }
}
