using System.Collections.Generic;
using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Input;

namespace Minimal.Input
{
    public class InputProcessor
    {

        private MouseState mOld;
        
        private List<IClickable> mClicks = new List<IClickable>();
        private List<IPressable> mPressables = new List<IPressable>();

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
                    if (c.Focus) c.click(state.Position);
                }
            
            if (mOld.LeftButton != state.LeftButton && mOld.LeftButton == ButtonState.Released)
                foreach (var c in mPressables)
                {
                    if (c.Focus) c.press(state.Position);
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
        void click(Point click);
    }

    public interface IPressable : IInput
    {
        void press(Point p);
    }
}