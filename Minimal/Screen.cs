using System.Collections.Generic;
using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Graphics;

namespace Minimal.Screen
{
    public interface IScreen
    {
        Rectangle Position { get; set; }
        void Draw(SpriteBatch spriteBatch, GameTime gameTime);
    }

    public class MainScreen : IScreen
    {
        private readonly List<IScreen> _mScreens = new List<IScreen>();

        public Rectangle Position { get; set; } 

        public void AddScreen(IScreen s)
        {
            _mScreens.Add(s);
        }

        public void Remove(IScreen s)
        {
            _mScreens.Remove(s);
        }

        public void Draw(SpriteBatch b, GameTime time)
        {
            foreach (var s in _mScreens)
            {
                if (Position.Intersects(s.Position)) s.Draw(b,time);
            }
        }
    }
}
