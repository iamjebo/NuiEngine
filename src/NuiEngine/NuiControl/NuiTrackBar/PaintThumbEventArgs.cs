using System;
using System.Drawing;
using System.Windows.Forms;

namespace NuiEngine.NuiControl
{
    public class PaintThumbEventArgs : PaintEventArgs
    {
        private ControlState _controlState;

        public ControlState ControlState
        {
            get { return _controlState; }
        }

        public PaintThumbEventArgs(
            Graphics g, Rectangle clipRect, ControlState state)
            : base(g, clipRect)
        {
            _controlState = state;
        }
    }
}
