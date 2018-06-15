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

    public sealed class MainScreen : IScreen
    {
        private readonly List<IScreen> mScreens = new List<IScreen>();
        private Rectangle mPosition;
        private readonly GraphicsDeviceManager mGraphics;
        private bool mFullscreen;

        public MainScreen(GraphicsDeviceManager graphics)
        {
            mGraphics = graphics;
            mPosition = new Rectangle(0, 0, 1, 1);
        }

        public bool Fullscreen {
            get { return mFullscreen; }
            set {
                mGraphics.ToggleFullScreen();
                mFullscreen = value;
            }
        }

        public Rectangle Position
        {
            get { return mPosition; }
            set
            {
                // Adjusting Screen Resolution
                mGraphics.PreferredBackBufferWidth = value.Width;
                mGraphics.PreferredBackBufferHeight = value.Height;
                mGraphics.ApplyChanges();

                var chgX = value.Width / mPosition.Width;
                var chgY = value.Height / mPosition.Height;

                // Adjusting subscreens.
                foreach (var scr in mScreens)
                {
                    var p = scr.Position;
                    scr.Position = new Rectangle(p.X * chgX, p.Y * chgY, p.Width * chgX, p.Height *chgY);
                }

                mPosition = value;
            }
        }
        
        public void AddScreen(IScreen s)
        {
            mScreens.Add(s);
        }

        public void Remove(IScreen s)
        {
            mScreens.Remove(s);
        }

        public void Draw(SpriteBatch b, GameTime time)
        {
            foreach (var s in mScreens)
            {
                if (Position.Intersects(s.Position)) s.Draw(b,time);
            }
        }
    }
}
