using System;
using System.Collections.Generic;
using System.Text;
using System.Windows.Forms;
using NuiEngine.NuiControl.Window32;
using NuiEngine.NuiControl.Window32.Struct;
using System.Drawing;
using NuiEngine.NuiControl.Window32.Const;
using System.Drawing.Drawing2D;
using System.ComponentModel;

namespace NuiEngine.NuiControl
{
    /// <summary>
    /// 滑动条控件
    /// </summary>
    [ToolboxBitmap(typeof(TrackBar))]
    public class NuiTrackBar : TrackBar
    {
        #region Fields

        private bool _bPainting = false;
        private TrackBarColorTable _colorTable;

        #endregion

        #region Constructors

        public NuiTrackBar(): base()
        {
            SetStyle(ControlStyles.UserPaint, true);
            SetStyle(ControlStyles.AllPaintingInWmPaint, true);
            SetStyle(ControlStyles.OptimizedDoubleBuffer, true);
            SetStyle(ControlStyles.ResizeRedraw, true);
            SetStyle(ControlStyles.SupportsTransparentBackColor, true);
            UpdateStyles();

            this.BackColor = Color.Transparent;
        }

        #endregion

        #region Properties

        [Browsable(false)]
        public TrackBarColorTable ColorTable
        {
            get 
            {
                if (_colorTable == null)
                {
                    _colorTable = new TrackBarColorTable();
                }
                return _colorTable;
            }
            set
            {
                _colorTable = value;
                base.Invalidate();
            }
        }

        [Description("滑动条开始点颜色")]
        public Color TrackBeginColor { get; set; }

        [Description("滑动条终点颜色")]
        public Color TrackEndColor { get; set; }

        [Description("滑动条边框颜色")]
        public Color TrackBorderColor { get; set; }

        #endregion

        #region Protected Methods

        protected virtual void OnRenderBackground(PaintEventArgs e)
        {
            base.OnPaintBackground(e);
        }

        protected virtual void OnRenderTick(PaintTickEventArgs e)
        {
            Graphics g = e.Graphics;
            Rectangle trackRect = e.TrackRect;
            bool bHorizontal = base.Orientation == Orientation.Horizontal;
            int posFirst = 0;
            int posSecond = 0;
            bool bTickBoth = base.TickStyle == TickStyle.Both;

            if (bHorizontal)
            {
                switch (base.TickStyle)
                {
                    case TickStyle.TopLeft:
                        posFirst = trackRect.Top - 15;
                        break;
                    case TickStyle.BottomRight:
                        posFirst = trackRect.Bottom + 13;
                        break;
                    case TickStyle.Both:
                        posFirst = trackRect.Top - 15;
                        posSecond = trackRect.Bottom + 13;
                        break;
                }
            }
            else
            {
                switch (base.TickStyle)
                {
                    case TickStyle.TopLeft:
                        posFirst = trackRect.Left - 15;
                        break;
                    case TickStyle.BottomRight:
                        posFirst = trackRect.Right + 13;
                        break;
                    case TickStyle.Both:
                        posFirst = trackRect.Left - 15;
                        posSecond = trackRect.Right + 13;
                        break;
                }
            }

            Pen lightPen = new Pen(ColorTable.TickLight);
            Pen darkPen = new Pen(ColorTable.TickDark);

            if (bHorizontal)
            {
                foreach (int tickPos in e.TickPosList)
                {
                    g.DrawLine(
                        lightPen, new Point(tickPos, posFirst),
                        new Point(tickPos, posFirst + 2));
                    g.DrawLine(
                        darkPen, new Point(tickPos + 1, posFirst),
                        new Point(tickPos + 1, posFirst + 2));
                    if (bTickBoth)
                    {
                        g.DrawLine(
                            lightPen, new Point(tickPos, posSecond),
                            new Point(tickPos, posSecond + 2));
                        g.DrawLine(
                            darkPen, new Point(tickPos + 1, posSecond),
                            new Point(tickPos + 1, posSecond + 2));
                    }
                }
            }
            else
            {
                foreach (int tickPos in e.TickPosList)
                {
                    g.DrawLine(
                        lightPen, new Point(posFirst + 2, tickPos), 
                        new Point(posFirst, tickPos));
                    g.DrawLine(
                        darkPen, new Point(posFirst, tickPos + 1),
                        new Point(posFirst + 2, tickPos + 1));

                    if (bTickBoth)
                    {
                        g.DrawLine(
                            lightPen, new Point(posSecond + 2, tickPos),
                            new Point(posSecond, tickPos));
                        g.DrawLine(
                            darkPen, new Point(posSecond, tickPos + 1),
                            new Point(posSecond + 2, tickPos + 1));
                    }
                }
            }

            lightPen.Dispose();
            darkPen.Dispose();
        }

        protected virtual void OnRenderTrack(PaintEventArgs e)
        {
            Graphics g = e.Graphics;
            Rectangle rect = e.ClipRectangle;
            bool horizontal = base.Orientation == Orientation.Horizontal;
            float mode = horizontal ? 0f : 270f;

            if (horizontal)
            {
                rect.Inflate(0, 1);
            }
            else
            {
                 rect.Inflate(1, 0);
            }

            SmoothingModeGraphics sg = new SmoothingModeGraphics(g);

            using (GraphicsPath path = GraphicsPathHelper.CreatePath(
                rect, 4, RoundStyle.All, true))
            {
                using (LinearGradientBrush brush = new LinearGradientBrush(
                    rect, TrackBeginColor, TrackEndColor, mode))
                {
                    g.FillPath(brush, path);
                }

                using (Pen pen = new Pen(TrackBorderColor))
                {
                    g.DrawPath(pen, path);
                }
            }

            rect.Inflate(-1, -1);
            using (GraphicsPath path = GraphicsPathHelper.CreatePath(
                rect, 4, RoundStyle.All, true))
            {
                using (Pen pen = new Pen(ColorTable.TrackInnerBorder))
                {
                    g.DrawPath(pen, path);
                }
            }

            sg.Dispose();
        }

        protected virtual void OnRenderThumb(PaintThumbEventArgs e)
        {
            Graphics g = e.Graphics;
            Rectangle rect = e.ClipRectangle;
            ControlState state = e.ControlState;
            ThumbArrowDirection direction = ThumbArrowDirection.None;
            Color begin = ColorTable.ThumbBackNormal;
            Color end = ColorTable.TrackInnerBorder;
            Color border = ColorTable.ThumbBorderNormal;
            float mode =
               base.Orientation == Orientation.Horizontal ?
               90f : 0f;

            switch (base.Orientation)
            {
                case Orientation.Horizontal:
                    switch (base.TickStyle)
                    {
                        case TickStyle.None:
                        case TickStyle.BottomRight:
                            direction = ThumbArrowDirection.Down;
                            break;
                        case TickStyle.TopLeft:
                            direction = ThumbArrowDirection.Up;
                            break;
                        case TickStyle.Both:
                            direction = ThumbArrowDirection.None;
                            break;
                    }
                    break;
                case Orientation.Vertical:
                    switch (base.TickStyle)
                    {
                        case TickStyle.TopLeft:
                            direction = ThumbArrowDirection.Left;
                            break;
                        case TickStyle.None:
                        case TickStyle.BottomRight:
                            direction = ThumbArrowDirection.Right;
                            break;
                        case TickStyle.Both:
                            direction = ThumbArrowDirection.None;
                            break;
                    }
                    break;
            }

            switch (state)
            {
                case ControlState.Hover:
                    begin = ColorTable.ThumbBackHover;
                    border = ColorTable.ThumbBorderHover;
                    break;
            }

            using (SmoothingModeGraphics sg = new SmoothingModeGraphics(g))
            {
                using (GraphicsPath path =
                    GraphicsPathHelper.CreateTrackBarThumbPath(
                    rect, direction))
                {
                    using (LinearGradientBrush brush = new LinearGradientBrush(
                        rect, begin, end, mode))
                    {
                        Blend blend = new Blend();
                        blend.Positions = new float[] { 0, .2f, .5f, .8f, 1f };
                        blend.Factors = new float[] { 1f, .7f, 0, .7f, 1f };
                        brush.Blend = blend;

                        g.FillPath(brush, path);
                    }
                    using (Pen pen = new Pen(border))
                    {
                        g.DrawPath(pen, path);
                    }
                }

                rect.Inflate(-1, -1);
                using (GraphicsPath path =
                   GraphicsPathHelper.CreateTrackBarThumbPath(
                   rect, direction))
                {
                    using (Pen pen = new Pen(ColorTable.TrackInnerBorder))
                    {
                        g.DrawPath(pen, path);
                    }
                }
            }
        }

        protected override void WndProc(ref Message m)
        {
            switch (m.Msg)
            {
                case WM.WM_PAINT:
                    if (!_bPainting)
                    {
                        _bPainting = true;

                        PAINTSTRUCT ps = new PAINTSTRUCT();

                        NativeMethods.BeginPaint(m.HWnd, ref ps);
                        DrawTrackBar(m.HWnd);
                        NativeMethods.ValidateRect(m.HWnd, ref ps.rcPaint);
                        NativeMethods.EndPaint(m.HWnd, ref ps);

                        _bPainting = false;
                        m.Result = Result.TRUE;
                    }
                    else
                    {
                        base.WndProc(ref m);
                    }
                    break;
                default:
                    base.WndProc(ref m);
                    break;
            }
        }

        #endregion

        #region Helper Methods

        private bool ThumbHovering(RECT thumbRect)
        {
            RECT windowRect = new RECT();
            Point point = new Point();

            NativeMethods.GetWindowRect(base.Handle, ref windowRect);
            NativeMethods.OffsetRect(
                ref thumbRect, windowRect.Left, windowRect.Top);
            NativeMethods.GetCursorPos(ref point);
            if (NativeMethods.PtInRect(ref thumbRect, point))
            {
                return true;
            }
            return false;
        }

        private void DrawTrackBar(IntPtr hWnd)
        {
            ControlState state = ControlState.Normal;
            bool horizontal = base.Orientation == Orientation.Horizontal;
            ImageDc tempDc = new ImageDc(base.Width, base.Height);
            RECT trackRect = new RECT();
            RECT thumbRect = new RECT();

            Graphics g = Graphics.FromHdc(tempDc.Hdc);

            NativeMethods.SendMessage(
                hWnd, TBM.TBM_GETCHANNELRECT, 0, ref trackRect);
            NativeMethods.SendMessage(
                hWnd, TBM.TBM_GETTHUMBRECT, 0, ref thumbRect);

            Rectangle trackRectangle = horizontal ?
                trackRect.Rect :
                Rectangle.FromLTRB(
                trackRect.Top, trackRect.Left,
                trackRect.Bottom, trackRect.Right);

            if (ThumbHovering(thumbRect))
            {
                if (Helper.LeftKeyPressed())
                {
                    state = ControlState.Pressed;
                }
                else
                {
                    state = ControlState.Hover;
                }
            }

            using (PaintEventArgs pe = new PaintEventArgs(
                g, ClientRectangle))
            {
                OnRenderBackground(pe);
            }

            int ticks = NativeMethods.SendMessage(
                hWnd, TBM.TBM_GETNUMTICS, 0, 0);

            if (ticks > 0)
            {
                List<float> tickPosList = new List<float>(ticks);

                int thumbOffset = horizontal ? 
                    thumbRect.Rect.Width : thumbRect.Rect.Height;
                int trackWidth = trackRect.Right - trackRect.Left;
                float tickSpace = (trackWidth - thumbOffset) / (float)(ticks - 1);
                float offset = trackRect.Left + thumbOffset / 2f;

                for(int pos = 0; pos < ticks; pos ++)
                {
                    tickPosList.Add(offset + tickSpace * pos);
                }

                using (PaintTickEventArgs pte = new PaintTickEventArgs(
                    g, trackRectangle, tickPosList))
                {
                    OnRenderTick(pte);
                }
            }

            using (PaintEventArgs pe = new PaintEventArgs(
                g, trackRectangle))
            {
                OnRenderTrack(pe);
            }

            using (PaintThumbEventArgs pe = new PaintThumbEventArgs(
                g, thumbRect.Rect, state))
            {
                OnRenderThumb(pe);
            }

            g.Dispose();
            IntPtr hDC = NativeMethods.GetDC(hWnd);
            NativeMethods.BitBlt(
                hDC, 0, 0, base.Width, base.Height, 
                tempDc.Hdc, 0, 0, 0xCC0020);
            NativeMethods.ReleaseDC(hWnd, hDC);
            tempDc.Dispose();
        }

        #endregion
    }
}
