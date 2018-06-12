using System.Collections.Generic;
using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Input;

namespace Minimal.Input
{
    public sealed class InputProcessor
    {

        private MouseState mOld;
        
        private readonly List<IClickable> mClicks = new List<IClickable>();
        private readonly List<IPressable> mPressables = new List<IPressable>();

        public void Initialise()
        {
            mOld = Mouse.GetState();
        } 
        
        public void Update(GameTime time)
        {
            MouseState state = Mouse.GetState();
            
            if (mOld.LeftButton != state.LeftButton && mOld.LeftButton == ButtonState.Pressed)
                foreach (var c in mClicks)
                {
                    if (c.Focus) c.Click(state.Position);
                }
            
            if (mOld.LeftButton != state.LeftButton && mOld.LeftButton == ButtonState.Released)
                foreach (var c in mPressables)
                {
                    if (c.Focus) c.Press(state.Position);
                }
            
            mOld = state;
        }

        public void AddClick(IClickable c)
        {
            mClicks.Add(c);
        }
        
        public void AddPress(IPressable c)
        {
            mPressables.Add(c);
        }
    }

    public interface IInput
    {
        bool Focus { get; set; }
    }

    public enum AbstractKeys
    {
        Select, Walk
    }
        
    public interface IClickable : IInput
    {
        void Click(Point click);
    }

    public interface IPressable : IInput
    {
        void Press(Point p);
    }
}