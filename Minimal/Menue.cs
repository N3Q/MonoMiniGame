using System;
using System.Collections.Generic;
using Minimal.WorldObj;
using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Content;
using Microsoft.Xna.Framework.Graphics;
using Minimal.Input;
using Minimal.Screen;

namespace Minimal.Menue
{
    public class MainMenue : IScreen, IClickable, IPressable
    {
        private bool _mFocus;
        private Rectangle _mPos;
        
        public Rectangle Position {
            get => _mPos;
            set
            {
                _mPos = value;
                
                var curX = value.X;
                var curY = value.Y;
                var curW = value.Width;
                var skip = value.Height / content.Count;
                var curH = skip - (content.Count * 2);
                
                for (int i = 0; i < content.Count; i++)
                {
                    Rectangle rect = new Rectangle(curX, curY, curW, curH);
                    Button b = content[i];
                    b.Position = rect;
                    curY += skip;
                }
            } 
        }
        
        public bool Focus
        {
            get => _mFocus;
            set
            {
                _mFocus = value;
                foreach (var button in content)
                {
                    button.Focus = value;
                }
            }
        }

        private SpriteFont font;

        private readonly List<Button> content = new List<Button>();
        
        public void Initialise()
        {
            var nGame = new Button
            {
                Text = "New Game"
            };
            nGame.SetListener(new StateChanger(Game1.GameState.Starting));
            content.Add(nGame);
            var mid = new Button
            {
                Text = "Mid Button"
            };
            mid.SetListener(new Hello("Clicked"));
            content.Add(mid);
            var other = new Button
            {
                Text = "Close"
            };
            other.SetListener(new StateChanger(Game1.GameState.Closing));
            content.Add(other);
            
            foreach (var button in content)
            {
                button.TextColor = Color.Azure;
                button.Background = Color.Bisque;
                button.Font = font;
            }
        }

        public void LoadContent(ContentManager content)
        {
            font = content.Load<SpriteFont>("Font");
        }

        public void Draw(SpriteBatch spriteBatch, GameTime gameTime)
        {
            spriteBatch.FillRectangle(Position, Color.Azure);
            foreach (var button in content)
            {
                button.Draw(spriteBatch, gameTime);
            }
        }

        public void click(Point click)
        {
            if (!Position.Contains(click)) return;
            foreach (var button in content)
            {
                button.click(click);
            }
        }
        
        public void press(Point click)
        {
            if (!Position.Contains(click)) return;
            foreach (var button in content)
            {
                button.press(click);
            }
        }
    }

    public class Button : IScreen, IClickable, IPressable
    {
        private ActionListener _listener;
        
        public Rectangle Position { get; set; }
        public bool Focus { get; set; }
        
        public String Text { private get; set;  }
        public SpriteFont Font { private get; set; }
        public Color Background { private get; set; }
        public Color TextColor { private get; set; }

        public void SetListener(ActionListener l)
        {
            _listener = l;
        } 
        
        public void Draw(SpriteBatch spriteBatch, GameTime gameTime)
        {
            spriteBatch.FillRectangle(Position, Background);
            var text = Font.MeasureString(Text).Mult(-0.5f);
            spriteBatch.DrawString(Font, Text, Position.Center.ToVector2().Add(text), TextColor);
        }
        
        public void click(Point click)
        {
            if (Position.Contains(click))
            {
                _listener?.ActionPerformed();
                Background = Color.Aqua;
            }
        }

        public void press(Point p)
        {
            if (Position.Contains(p)) Background = Color.Chartreuse; 
        }
    }

    public interface ActionListener
    {
        void ActionPerformed();
    }

    public class Hello : ActionListener
    {
        private String s;
        public Hello(String s)
        {
            this.s = s;
        }
        public void ActionPerformed()
        {
            Console.WriteLine(s);
        }
    }

    public class StateChanger : ActionListener
    {
        private Game1.GameState s;

        public StateChanger(Game1.GameState s)
        {
            this.s = s;
        }
        
        public void ActionPerformed()
        {
            Game1.State = s;
        }
    }
}