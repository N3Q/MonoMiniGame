using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Graphics;
using Minimal.Input;
using Minimal.Menue;
using Minimal.Screen;
using Minimal.WorldObj;

namespace Minimal
{
    public sealed class Game1 : Game
    {
        private readonly GraphicsDeviceManager mGraphics;
        private SpriteBatch mSpriteBatch;

        private readonly MainScreen mMainScreen;
        private readonly InputProcessor mInputProcessor;
        private readonly MainMenue mMainMenue;
        
        public enum  GameState 
        {
            Init, Starting, Run, Closing
        }

        public static GameState mState = GameState.Init;
        
        public Game1()
        {
            mGraphics = new GraphicsDeviceManager(this);
            Content.RootDirectory = "Content";
            
            mMainScreen = new MainScreen(mGraphics);
            mInputProcessor = new InputProcessor();
            mMainMenue = new MainMenue();
        }
     
        protected override void Initialize()
        {        
            base.Initialize();
            mMainScreen.Position = new Rectangle(0, 0, 1024, 768);
            mGraphics.PreferredBackBufferWidth = 1024;
            mGraphics.PreferredBackBufferHeight = 768;
            IsMouseVisible = true;
            mGraphics.ApplyChanges();
            
            mInputProcessor.Initialise();
            
            mMainMenue.Initialise();
            mMainMenue.Position = new Rectangle(120, 20, 256, 64);
            mMainMenue.Focus = true;
            
            mMainScreen.AddScreen(mMainMenue);
            
            mInputProcessor.AddClick(mMainMenue);
            mInputProcessor.AddPress(mMainMenue);
        }

        protected override void LoadContent()
        {
            // Create a new SpriteBatch, which can be used to draw textures.
            mSpriteBatch = new SpriteBatch(GraphicsDevice);
            mMainMenue.LoadContent(Content);
        }

     
        protected override void UnloadContent()
        {
            
        }
      
        protected override void Update(GameTime gameTime)
        {
            base.Update(gameTime);
            
            if (mState == GameState.Closing) 
                Exit();
            if (mState== GameState.Starting)
            {
                World w = new World();
                w.LoadContent(Content);
                w.Map = new [,]
                {
                    {0, 0, 0, 0},
                    {0, 0, 0, 0},
                    {0, 0, 0, 0},
                    {0, 0, 0, 0},
                    {2, 1, 1, 3}
                };
                
                Camera c = new Camera(w, new Vector2(2, 2));
                c.Position = new Rectangle(0, 0, 1024, 768);
                mMainScreen.AddScreen(c);
                w.AddObject(c);
                
                mState = GameState.Run;
            }
            mInputProcessor.Update(gameTime);
        }

        protected override void Draw(GameTime gameTime)
        {
            base.Draw(gameTime);
            mGraphics.GraphicsDevice.Clear(Color.CornflowerBlue);
            
            mSpriteBatch.Begin();
            mMainScreen.Draw(mSpriteBatch, gameTime);
            mSpriteBatch.End();
            
        }
    }
}
