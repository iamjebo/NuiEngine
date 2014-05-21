using System;
using System.Collections.Generic;
using System.Text;
using System.Drawing;

namespace NuiEngine.NuiControl
{
    public class TrackBarColorTable
    {
        private static readonly Color _trackInnerBorder = Color.FromArgb(200, 250, 250, 250);
        private static readonly Color _thumbBackNormal = Color.FromArgb(200, 193, 227, 247);
        private static readonly Color _thumbBackHover = Color.FromArgb(200, 50, 162, 228);
        private static readonly Color _thumbBackPressed = Color.FromArgb(200, 50, 162, 228);
        private static readonly Color _thumbBorderNormal = Color.FromArgb(103, 165, 216);
        private static readonly Color _thumbBorderHover = Color.FromArgb(70, 146, 207);
        private static readonly Color _tickLight = Color.FromArgb(233, 238, 238);
        private static readonly Color _tickDark = Color.FromArgb(197, 197, 197);

        public TrackBarColorTable() { }

        

        public virtual Color TrackInnerBorder
        {
            get { return _trackInnerBorder; }
        }

        public virtual Color ThumbBackNormal
        {
            get { return _thumbBackNormal; }
        }

        public virtual Color ThumbBackHover
        {
            get { return _thumbBackHover; }
        }

        public virtual Color ThumbBackPressed
        {
            get { return _thumbBackPressed; }
        }

        public virtual Color ThumbBorderNormal
        {
            get { return _thumbBorderNormal; }
        }

        public virtual Color ThumbBorderHover
        {
            get { return _thumbBorderHover; }
        }

        public virtual Color TickLight
        {
            get { return _tickLight; }
        }

        public virtual Color TickDark
        {
            get { return _tickDark; }
        }
    }
}
