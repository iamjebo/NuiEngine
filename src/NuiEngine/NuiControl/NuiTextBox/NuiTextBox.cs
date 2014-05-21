using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Windows.Forms;
using System.Drawing;
using NuiEngine.Utility;
using System.Drawing.Drawing2D;
using System.ComponentModel;

namespace NuiEngine.NuiControl
{
    /// <summary>
    /// 文本框控件
    /// </summary>
    public class NuiTextBox:TextBox
    {
        #region Field

        private NuiControlState m_state = NuiControlState.Normal;
        private Font m_defaultFont = new Font("微软雅黑", 9);

        //当Text属性为空时编辑框内出现的提示文本
        private string _emptyTextTip;
        private Color _emptyTextTipColor = Color.DarkGray;

        #endregion

        #region Constructor

        public NuiTextBox()
        {
            SetStyles();
            this.Font = m_defaultFont;
            this.BorderStyle = BorderStyle.FixedSingle;
        }

        #endregion

        #region Properites

        [Description("当Text属性为空时编辑框内出现的提示文本")]
        public String EmptyTextTip
        {
            get { return _emptyTextTip; }
            set
            {
                if (_emptyTextTip != value)
                {
                    _emptyTextTip = value;
                    base.Invalidate();
                }
            }
        }

        [Description("获取或设置EmptyTextTip的颜色")]
        public Color EmptyTextTipColor
        {
            get { return _emptyTextTipColor; }
            set
            {
                if (_emptyTextTipColor != value)
                {
                    _emptyTextTipColor = value;
                    base.Invalidate();
                }
            }
        }

        #endregion

        #region Override

        protected override void OnMouseEnter(EventArgs e)
        {
            m_state = NuiControlState.Highlight;
            this.Invalidate();
            base.OnMouseEnter(e);
        }

        protected override void OnMouseLeave(EventArgs e)
        {
            if (m_state == NuiControlState.Highlight && Focused)
            {
                m_state = NuiControlState.Focus;
            }
            else if (m_state == NuiControlState.Focus)
            {
                m_state = NuiControlState.Focus;
            }
            else
            {
                m_state = NuiControlState.Normal;
            }
            this.Invalidate();
            base.OnMouseLeave(e);
        }

        protected override void OnMouseDown(MouseEventArgs mevent)
        {
            if (mevent.Button == MouseButtons.Left)
            {
                m_state = NuiControlState.Highlight;
            }
            this.Invalidate();
            base.OnMouseDown(mevent);
        }

        protected override void OnMouseUp(MouseEventArgs mevent)
        {
            if (mevent.Button == MouseButtons.Left)
            {
                if (ClientRectangle.Contains(mevent.Location))
                {
                    m_state = NuiControlState.Highlight;
                }
                else
                {
                    m_state = NuiControlState.Focus;
                }
            }
            this.Invalidate();
            base.OnMouseUp(mevent);
        }

        protected override void OnLostFocus(EventArgs e)
        {
            m_state = NuiControlState.Normal;
            this.Invalidate();
            base.OnLostFocus(e);
        }

        protected override void OnEnabledChanged(EventArgs e)
        {
            if (Enabled)
            {
                m_state = NuiControlState.Normal;
            }
            else
            {
                m_state = NuiControlState.Disabled;
            }
            this.Invalidate();
            base.OnEnabledChanged(e);
        }

        protected override void WndProc(ref Message m)
        {//TextBox是由系统进程绘制，重载OnPaint方法将不起作用

            base.WndProc(ref m);
            if (m.Msg == Win32.WM_PAINT || m.Msg == Win32.WM_CTLCOLOREDIT)
            {
                WmPaint(ref m);
            }
        }

        protected override void Dispose(bool disposing)
        {
            if (disposing)
            {
                if (m_defaultFont != null)
                {
                    m_defaultFont.Dispose();
                }
            }

            m_defaultFont = null;
            base.Dispose(disposing);
        }

        #endregion

        #region Private

        private void SetStyles()
        {
            // TextBox由系统绘制，不能设置 ControlStyles.UserPaint样式
            SetStyle(ControlStyles.AllPaintingInWmPaint, true);
            SetStyle(ControlStyles.OptimizedDoubleBuffer, true);
            SetStyle(ControlStyles.ResizeRedraw, true);
            SetStyle(ControlStyles.SupportsTransparentBackColor, true);
            UpdateStyles();
        }

        private void WmPaint(ref Message m)
        {
            Graphics g = Graphics.FromHwnd(base.Handle);
            g.SmoothingMode = SmoothingMode.AntiAlias;

            if (!Enabled)
            {
                m_state = NuiControlState.Disabled;
            }

            switch (m_state)
            {
                case NuiControlState.Normal:
                    DrawNormalTextBox(g);
                    break;
                case NuiControlState.Highlight:
                    DrawHighLightTextBox(g);
                    break;
                case NuiControlState.Focus:
                    DrawFocusTextBox(g);
                    break;
                case NuiControlState.Disabled:
                    DrawDisabledTextBox(g);
                    break;
                default:
                    break;
            }

            if (Text.Length == 0 && !string.IsNullOrEmpty(EmptyTextTip) && !Focused)
            {
                TextRenderer.DrawText(g, EmptyTextTip, Font, ClientRectangle, EmptyTextTipColor,GetTextFormatFlags(TextAlign, RightToLeft == RightToLeft.Yes));
            }
        }

        private void DrawNormalTextBox(Graphics g)
        {
            using (Pen borderPen = new Pen(Color.LightGray))
            {
                g.DrawRectangle(
                    borderPen,
                    new Rectangle(
                        ClientRectangle.X,
                        ClientRectangle.Y,
                        ClientRectangle.Width - 1,
                        ClientRectangle.Height - 1));
            }
        }

        private void DrawHighLightTextBox(Graphics g)
        {
            using (Pen highLightPen = new Pen(NuiConfig.QQHighLightColor))
            {
                Rectangle drawRect = new Rectangle(
                        ClientRectangle.X,
                        ClientRectangle.Y,
                        ClientRectangle.Width - 1,
                        ClientRectangle.Height - 1);

                g.DrawRectangle(highLightPen, drawRect);

                //InnerRect
                drawRect.Inflate(-1, -1);
                highLightPen.Color = NuiConfig.QQHighLightInnerColor;
                g.DrawRectangle(highLightPen, drawRect);
            }
        }

        private void DrawFocusTextBox(Graphics g)
        {
            using (Pen focusedBorderPen = new Pen(NuiConfig.QQHighLightInnerColor))
            {
                g.DrawRectangle(
                    focusedBorderPen,
                    new Rectangle(
                        ClientRectangle.X,
                        ClientRectangle.Y,
                        ClientRectangle.Width - 1,
                        ClientRectangle.Height - 1));
            }
        }

        private void DrawDisabledTextBox(Graphics g)
        {
            using (Pen disabledPen = new Pen(SystemColors.ControlDark))
            {
                g.DrawRectangle(disabledPen,
                    new Rectangle(
                        ClientRectangle.X,
                        ClientRectangle.Y,
                        ClientRectangle.Width - 1,
                        ClientRectangle.Height - 1));
            }
        }

        private static TextFormatFlags GetTextFormatFlags(HorizontalAlignment alignment, bool rightToleft)
        {
            TextFormatFlags flags = TextFormatFlags.WordBreak |
                TextFormatFlags.SingleLine;
            if (rightToleft)
            {
                flags |= TextFormatFlags.RightToLeft | TextFormatFlags.Right;
            }

            switch (alignment)
            {
                case HorizontalAlignment.Center:
                    flags |= TextFormatFlags.VerticalCenter | TextFormatFlags.HorizontalCenter;
                    break;
                case HorizontalAlignment.Left:
                    flags |= TextFormatFlags.VerticalCenter | TextFormatFlags.Left;
                    break;
                case HorizontalAlignment.Right:
                    flags |= TextFormatFlags.VerticalCenter | TextFormatFlags.Right;
                    break;
            }
            return flags;
        }

        #endregion
    }
}
