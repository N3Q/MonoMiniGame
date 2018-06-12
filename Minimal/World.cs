using System.Collections.Generic;
using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Content;
using Microsoft.Xna.Framework.Graphics;
using Minimal.Screen;

namespace Minimal.WorldObj
{
    public class World
    {
        private int[,] _mMap;
        
        public int Width { get; private set; }
        public int Height { get; private set; }
        
        public int[,] map
        {
            get => _mMap;
            set
            {
                _mMap = value;
                Width = value.GetLength(0);
                Height = value.GetLength(1);
            } 
        }

        public readonly Dictionary<int, Texture2D> Texture2Ds = new Dictionary<int, Texture2D>();
        
        private readonly List<Objects> _mObjects = new List<Objects>();
        
        public void LoadContent(ContentManager content)
        {
            const string pre = "Platformer/Tiles/";
            string[] tiles = {"grassCenter", "grass", "grassCliffLeft", "grassCliffRight"};
            for (var i = 0; i < tiles.Length; i++)
            {
                Texture2Ds.Add(i+1, content.Load<Texture2D>(pre + tiles[i]));
            }
        }

        public void AddObject(Objects o)
        {
            _mObjects.Add(o);
        }

    }
    
    public abstract class Objects
    {
        protected readonly World World;
        protected Vector2 Coordinates;

        public Objects(World w, Vector2 coordinates)
        {
            World = w;
            Coordinates = coordinates;
        }
    }

    public class Camera : Objects, IScreen
    {
        private Rectangle _mView;
        
        public float Zoom { get; set; }
        
        public Rectangle Position { get; set; }

        public Camera(World w, Vector2 coord) : base(w, coord)
        {
            _mView = new Rectangle(Coordinates.ToPoint(), Position.Size.ToVector2().Mult(1/30f).ToPoint());
        }

        public void Draw(SpriteBatch spriteBatch, GameTime gameTime)
        {
            var map = World.map.Cut(_mView);
            for (var y = 0; y < _mView.Height; y++)
            {
                for (var x = 0; x < _mView.Width; x++)
                {
                    var text = new Rectangle(x, y, 30, 30);
                    spriteBatch.Draw(World.Texture2Ds[map[y,x]], text, Color.White);
                }
            }
        }
    }
}