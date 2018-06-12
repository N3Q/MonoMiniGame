using System;
using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Graphics;

namespace Minimal
{
    public static class SpriteBatchExtension
    {
        public static void FillRectangle(this SpriteBatch b, Rectangle rect, Color c)
        {
            Texture2D t = new Texture2D(b.GraphicsDevice, 1, 1);
            t.SetData(new[]{c});
            b.Draw(t, rect, c);
        }
    }

    public static class Vector2Extensions
    {
        public static Vector2 Add(this Vector2 u, Vector2 v)
        {
            return new Vector2(u.X + v.X, u.Y + v.Y);
        }
        
        public static Vector2 Mult(this Vector2 u, float skal)
        {
            return new Vector2(u.X * skal, u.Y *skal);
        }
    }

    public static class IntExt
    {
        public static int[,] Cut(this int[,] h, Rectangle r)
        {
            
            var n = new int[r.Height, r.Width];
            var i = 0;
            var j = 0;
            for (var y = r.Y; y < r.Y + r.Height && y < h.GetLength(0); y++)
            {
                for (var x = r.X; x < r.X + r.Width && x < h.GetLength(0); x++)
                {
                    n[j, i] = h[y, x];
                    i++;
                }
                i = 0; j++;
            }
            return n;
        }
    }
}