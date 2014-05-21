using System;
using System.Windows.Forms;
using System.Drawing;
using System.Collections.Generic;

namespace NuiEngine.NuiControl
{
    public class PaintTickEventArgs : IDisposable
    {
        private Graphics _graphics;
        private Rectangle _trackRect;
        private IList<float> _tickPosList;

        public PaintTickEventArgs(
            Graphics g, Rectangle trackRect, IList<float> tickPosList)
        {
            _graphics = g;
            _trackRect = trackRect;
            _tickPosList = tickPosList;
        }

        public Graphics Graphics
        {
            get { return _graphics; }
        }

        public Rectangle TrackRect
        {
            get { return _trackRect; }
        }

        public IList<float> TickPosList
        {
            get { return _tickPosList; }
        }

        #region IDisposable 成员

        public virtual void Dispose()
        {
            _graphics = null;
            _tickPosList = null;
        }

        #endregion
    }
}
