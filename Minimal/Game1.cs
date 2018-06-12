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
        private readonly GraphicsDeviceManager _mGraphics;
        private SpriteBatch _mSpriteBatch;

        private readonly MainScreen _mainScreen;
        private readonly InputProcessor _inputProcessor;
        private readonly MainMenue _mainMenue;
        
        public enum  GameState 
        {
            Init, Starting, Run, Closing
        }

        public static GameState State = GameState.Init;
        
        public Game1()
        {
            _mGraphics = new GraphicsDeviceManager(this);
            Content.RootDirectory = "Content";
            
            _mainScreen = new MainScreen();
            _inputProcessor = new InputProcessor();
            _mainMenue = new MainMenue();
        }
     
        protected override void Initialize()
        {        
            base.Initialize();
            _mainScreen.Position = new Rectangle(0, 0, 1024, 768);
            _mGraphics.PreferredBackBufferWidth = 1024;
            _mGraphics.PreferredBackBufferHeight = 768;
            IsMouseVisible = true;
            _mGraphics.ApplyChanges();
            
            _inputProcessor.Initialise();
            
            _mainMenue.Initialise();
            _mainMenue.Position = new Rectangle(120, 20, 256, 64);
            _mainMenue.Focus = true;
            
            _mainScreen.AddScreen(_mainMenue);
            
            _inputProcessor.AddClick(_mainMenue);
            _inputProcessor.AddPress(_mainMenue);
        }

        protected override void LoadContent()
        {
            // Create a new SpriteBatch, which can be used to draw textures.
            _mSpriteBatch = new SpriteBatch(GraphicsDevice);
            _mainMenue.LoadContent(Content);
        }

     
        protected override void UnloadContent()
        {
            
        }
      
        protected override void Update(GameTime gameTime)
        {
            base.Update(gameTime);
            
            if (State == GameState.Closing) 
                Exit();
            if (State== GameState.Starting)
            {
                World w = new World();
                w.LoadContent(Content);
                w.map = new int[,]
                {
                    {0, 0, 0, 0},
                    {0, 0, 0, 0},
                    {0, 0, 0, 0},
                    {0, 0, 0, 0},
                    {2, 1, 1, 3}
                };
                
                Camera c = new Camera(w, new Vector2(2, 2));
                c.Position = new Rectangle(0, 0, 1024, 768);
                _mainScreen.AddScreen(c);
                w.AddObject(c);
                
                State = GameState.Run;
            }
            _inputProcessor.Update(gameTime);
        }

        protected override void Draw(GameTime gameTime)
        {
            base.Draw(gameTime);
            _mGraphics.GraphicsDevice.Clear(Color.CornflowerBlue);
            
            _mSpriteBatch.Begin();
            _mainScreen.Draw(_mSpriteBatch, gameTime);
            _mSpriteBatch.End();
            
        }
    }
}
