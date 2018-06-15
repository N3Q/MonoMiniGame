using System.Collections.Generic;
using System.Linq;
using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Input;

namespace Minimal.Input
{
    public sealed class Settings
    {
        public static Dictionary<Keys, AbstractKeys> Bindings { get; private set; } = new Dictionary<Keys, AbstractKeys>();

        public static void LoadSettings()
        {
            Bindings.Add(Keys.Space, AbstractKeys.Jump);
            Bindings.Add(Keys.Left, AbstractKeys.Left);
            Bindings.Add(Keys.Right, AbstractKeys.Right);
        }
    }

    public sealed class InputProcessor
    {

        private MouseState mOld;
        private KeyboardState mOldKey;
        
        private readonly List<IClickable> mClicks = new List<IClickable>();
        private readonly List<IPressable> mPressables = new List<IPressable>();
        private readonly List<IKey> mKeys = new List<IKey>();

        public void Initialise()
        {
            mOld = Mouse.GetState();
            mOldKey = Keyboard.GetState();
        } 
        
        public void Update(GameTime time)
        {
            MouseState state = Mouse.GetState();
            KeyboardState keys = Keyboard.GetState();

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

            var akeys = new List<AbstractKeys>();
            foreach (var k in Settings.Bindings.Keys)
            {
                if (mOldKey.GetPressedKeys().Contains(k) && !keys.GetPressedKeys().Contains(k)) akeys.Add(Settings.Bindings[k]);
            }

            if (akeys.Count > 0)
                foreach (var k in mKeys)
                {
                    k.KeyPressed(akeys.ToArray());
                }

            mOldKey = keys;
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

        public void AddKey(IKey k)
        {
            mKeys.Add(k);
        }
    }

    public interface IInput
    {
        bool Focus { get; set; }
    }

    public enum AbstractKeys
    {
        Jump, Right, Left
    }
        
    public interface IClickable : IInput
    {
        void Click(Point click);
    }

    public interface IPressable : IInput
    {
        void Press(Point p);
    }

    public interface IKey : IInput
    {
        void KeyPressed(AbstractKeys[] keys);
    }
}