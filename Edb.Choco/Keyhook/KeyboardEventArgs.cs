using System;

namespace Edb.Environment.Keyhook
{
    using System.Windows.Forms;

    public class KeyboardEventArgs : EventArgs
    {
        public EState State { get; private set; }

        public EKeyType Type { get; private set; }

        public Keys Key { get; private set; }

        public bool Handled { get; set; }

        public KeyboardEventArgs(EState state, EKeyType type, Keys keys)
        {
            State = state;
            Type = type;
            Key = keys;
        }
    }
}
