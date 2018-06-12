using System.Collections.Generic;
using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Content;
using Microsoft.Xna.Framework.Graphics;
using Minimal.Screen;

namespace Minimal.WorldObj
{
    public sealed class World
    {
        private int[,] mMap;
        
        public int Width { get; private set; }
        public int Height { get; private set; }
        
        public int[,] Map
        {
            get { return mMap; }
            set
            {
                mMap = value;
                Width = value.GetLength(0);
                Height = value.GetLength(1);
            } 
        }

        public readonly Dictionary<int, Texture2D> mTexture2Ds = new Dictionary<int, Texture2D>();
        private readonly List<Objects> mObjects = new List<Objects>();
        
        public void LoadContent(ContentManager content)
        {
            const string pre = "Platformer/Tiles/";
            string[] tiles = {"grassCenter", "grass", "grassCliffLeft", "grassCliffRight"};
            for (var i = 0; i < tiles.Length; i++)
            {
                mTexture2Ds.Add(i+1, content.Load<Texture2D>(pre + tiles[i]));
            }
        }

        public void AddObject(Objects o)
        {
            mObjects.Add(o);
        }

        public Objects GetObjects(Point p)
        {
            return mObjects[0];
        }

    }
    
    public abstract class Objects
    {
        protected readonly World mWorld;
        protected Vector2 mCoordinates;

        public Objects(World w, Vector2 coordinates)
        {
            mWorld = w;
            mCoordinates = coordinates;
        }
    }

    internal sealed class Camera : Objects, IScreen
    {
        private Rectangle mView;
        
        public float Zoom { get; set; }
        
        public Rectangle Position { get; set; }

        public int TileSize { get; set; } = 50;

        public Camera(World w, Vector2 coord) : base(w, coord)
        {
            
        }

        public void Initialise()
        {
            mView = new Rectangle(mCoordinates.ToPoint(), Position.Size.ToVector2().Mult(1f / TileSize).ToPoint());
            if (mView.Width > mWorld.Width) mView.Width = (int) (mWorld.Width - mCoordinates.X);
            if (mView.Height > mWorld.Height) mView.Height = (int) (mWorld.Height - mCoordinates.Y);
        }

        public void Draw(SpriteBatch spriteBatch, GameTime gameTime)
        {
           // spriteBatch.FillRectangle(Position, Color.Coral);
            var map = mWorld.Map.Cut(mView);
            for (var y = 0; y < map.GetLength(0); y++)
            {
                for (var x = 0; x < map.GetLength(1); x++)
                {
                    if (map[y,x] == 0) continue;
                    var text = new Rectangle(x * TileSize, y * TileSize, TileSize, TileSize);
                    text.Location = text.Location.ToVector2().Add(Position.Location.ToVector2()).ToPoint();
                    spriteBatch.Draw(mWorld.mTexture2Ds[map[y,x]], text, Color.White);
                }
            }
        }
    }
}