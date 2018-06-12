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
                mGraphics.PreferredBackBufferWidth = value.Width;
                mGraphics.PreferredBackBufferHeight = value.Height;
                mGraphics.ApplyChanges();
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
