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
        private readonly MenueLoader mMenueLoader;

        private readonly World mWorld;
        
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
            mMenueLoader = new MenueLoader(mMainScreen, mInputProcessor);

            mMainMenue = new MainMenue(mMenueLoader);
            mWorld = new World();
        }
     
        protected override void Initialize()
        {        
            base.Initialize();
            IsMouseVisible = true;
            mMainScreen.Position = new Rectangle(0, 0, 1024, 768);
            Settings.LoadSettings();

            mInputProcessor.Initialise();
            mInputProcessor.AddKey(new StateChanger(GameState.Starting));

            mMainMenue.Initialise();
            mMainMenue.Position = new Rectangle(120, 20, 256, 64);
            mMenueLoader.LoadMenue(mMainMenue);

            mWorld.Map = new[,]
            {
                {0, 0, 0, 0, 0, 0, 0, 0},
                {0, 0, 0, 0, 0, 0, 0, 0},
                {0, 0, 0, 0, 2, 3, 0, 0},
                {0, 0, 0, 0, 0, 1, 0, 0},
                {0, 0, 0, 0, 0, 0, 0, 3},
                {0, 0, 0, 0, 0, 0, 2, 2},
                {0, 0, 0, 0, 0, 1, 2, 2},
            };
        }

        protected override void LoadContent()
        {
            // Create a new SpriteBatch, which can be used to draw textures.
            mSpriteBatch = new SpriteBatch(GraphicsDevice);
            mMainMenue.LoadContent(Content);
            mWorld.LoadContent(Content);
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
                Camera c = new Camera(mWorld, new Vector2(1, 2));
                c.Position = new Rectangle(0, 0, 512, 768);
                c.Initialise();
                mMainScreen.AddScreen(c);
                mWorld.AddObject(c);

                Camera c2 = new Camera(mWorld, Vector2.Zero);
                c2.Position = new Rectangle(512, 0, 512, 768);
                c2.Initialise();
                mMainScreen.AddScreen(c2);
                mWorld.AddObject(c2);

                mMainMenue.Focus = false;
                mMainScreen.Remove(mMainMenue);

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
