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
            var n = new int[r.Width, r.Height];
            var i = 0;
            var j = 0;
            for (var x = r.X; x < r.Width; x++)
            {
                for (var y = r.Y; y < r.Height; y++)
                {
                    n[i, j] = h[x, y];
                    i++;
                }
                j++;
            }
            return n;
        }
    }
}