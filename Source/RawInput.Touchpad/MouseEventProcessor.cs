using System;
using System.Collections.Generic;
using System.Linq;
using System.Runtime.InteropServices;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Input;

namespace RawInput.Touchpad
{
    internal class MouseEventProcessor
    {
        [DllImport("user32.dll")]
        private static extern int SetCursorPos(int x, int y);
        public void MoveCursor(int x, int y)
        {
            SetCursorPos(x, y);
            return;
        }
    }

}
