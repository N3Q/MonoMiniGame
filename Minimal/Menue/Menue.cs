﻿using System;
using System.Collections.Generic;
using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Content;
using Microsoft.Xna.Framework.Graphics;
using Minimal.Input;
using Minimal.Screen;

namespace Minimal.Menue
{
    public sealed class MainMenue : IScreen, IClickable, IPressable
    {
        private bool mFocus;
        private Rectangle mPos;
        
        public Rectangle Position
        {
            get { return mPos; }
            set
            {
                mPos = value;
                
                var curX = value.X;
                var curY = value.Y;
                var curW = value.Width;
                var skip = value.Height / mContent.Count;
                var curH = skip - (mContent.Count * 2);
                
                for (int i = 0; i < mContent.Count; i++)
                {
                    Rectangle rect = new Rectangle(curX, curY, curW, curH);
                    Button b = mContent[i];
                    b.Position = rect;
                    curY += skip;
                }
            } 
        }
        
        public bool Focus
        {
            get { return mFocus; } 
            set
            {
                mFocus = value;
                foreach (var button in mContent)
                {
                    button.Focus = value;
                }
            }
        }

        private SpriteFont mFont;

        private readonly List<Button> mContent = new List<Button>();
        
        public void Initialise()
        {
            var nGame = new Button
            {
                Text = "New Game"
            };
            nGame.SetListener(new StateChanger(Game1.GameState.Starting));
            mContent.Add(nGame);
            var mid = new Button
            {
                Text = "Mid Button"
            };
            mid.SetListener(new Hello("Clicked"));
            mContent.Add(mid);
            var other = new Button
            {
                Text = "Close"
            };
            other.SetListener(new StateChanger(Game1.GameState.Closing));
            mContent.Add(other);
            
            foreach (var button in mContent)
            {
                button.TextColor = Color.Azure;
                button.Background = Color.Bisque;
                button.Font = mFont;
            }
        }

        public void LoadContent(ContentManager content)
        {
            mFont = content.Load<SpriteFont>("Font");
        }

        public void Draw(SpriteBatch spriteBatch, GameTime gameTime)
        {
            spriteBatch.FillRectangle(Position, Color.Azure);
            foreach (var button in mContent)
            {
                button.Draw(spriteBatch, gameTime);
            }
        }

        public void Click(Point click)
        {
            if (!Position.Contains(click)) return;
            foreach (var button in mContent)
            {
                button.Click(click);
            }
        }
        
        public void Press(Point click)
        {
            if (!Position.Contains(click)) return;
            foreach (var button in mContent)
            {
                button.Press(click);
            }
        }
    }

    public class Button : IScreen, IClickable, IPressable
    {
        private IActionListener mListener;
        
        public Rectangle Position { get; set; }
        public bool Focus { get; set; }
        
        public String Text { private get; set;  }
        public SpriteFont Font { private get; set; }
        public Color Background { private get; set; }
        public Color TextColor { private get; set; }

        public void SetListener(IActionListener l)
        {
            mListener = l;
        } 
        
        public void Draw(SpriteBatch spriteBatch, GameTime gameTime)
        {
            spriteBatch.FillRectangle(Position, Background);
            var text = Font.MeasureString(Text).Mult(-0.5f);
            spriteBatch.DrawString(Font, Text, Position.Center.ToVector2().Add(text), TextColor);
        }
        
        public void Click(Point click)
        {
            if (Position.Contains(click))
            {
                mListener?.ActionPerformed();
                Background = Color.Aqua;
            }
        }

        public void Press(Point p)
        {
            if (Position.Contains(p)) Background = Color.Chartreuse; 
        }
    }

    public interface IActionListener
    {
        void ActionPerformed();
    }

    public class Hello : IActionListener
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

    public class StateChanger : IActionListener
    {
        private Game1.GameState s;

        public StateChanger(Game1.GameState s)
        {
            this.s = s;
        }
        
        public void ActionPerformed()
        {
            Game1.mState = s;
        }
    }
}