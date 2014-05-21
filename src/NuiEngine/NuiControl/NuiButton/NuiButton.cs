using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Windows.Forms;
using System.Drawing;
using System.Drawing.Drawing2D;
using NuiEngine.Core;

namespace NuiEngine.NuiControl
{
    /// <summary>
    /// 普通按钮控件
    /// </summary>
    public class NuiButton : Button
    {
        #region Field

        private NuiControlState m_state = NuiControlState.Normal;
        private Font m_defaultFont = new Font("微软雅黑", 9);

        private Image m_normalImg = Properties.Resources.btn_normal;
        private Image m_hightImg = Properties.Resources.btn_highlight;
        private Image m_focusImg = Properties.Resources.btn_focus;
        private Image m_downImg = Properties.Resources.btn_down;

        #endregion

        #region Constructor

        public NuiButton()
        {
            SetStyles();
            this.Font = m_defaultFont;
            this.Size = new Size(68, 23);
        }

        #endregion

        #region Properites

        private int ImageWidth
        {
            get
            {
                if (Image == null)
                {
                    return 16;
                }
                else
                {
                    return Image.Width;
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
                m_state = NuiControlState.Down;
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

        protected override void OnPaint(PaintEventArgs pevent)
        {
            base.OnPaint(pevent);
            Graphics g = pevent.Graphics;
            g.SmoothingMode = SmoothingMode.AntiAlias;
            g.InterpolationMode = InterpolationMode.HighQualityBilinear;

            Rectangle imageRect, textRect;
            CalculateRect(out imageRect, out textRect);

            if (!Enabled)
            {
                m_state = NuiControlState.Disabled;
            }
            switch (m_state)
            {
                case NuiControlState.Normal:

                    Render.DrawImageWithNineRect(
                        g, m_normalImg,
                        ClientRectangle,
                        new Rectangle(0, 0, m_normalImg.Width, m_normalImg.Height));
                    break;
                case NuiControlState.Highlight:

                    Render.DrawImageWithNineRect(
                        g, m_hightImg,
                        ClientRectangle,
                        new Rectangle(0, 0, m_hightImg.Width, m_hightImg.Height));
                    break;
                case NuiControlState.Focus:

                    Render.DrawImageWithNineRect(
                        g, m_focusImg,
                        ClientRectangle,
                        new Rectangle(0, 0, m_focusImg.Width, m_focusImg.Height));
                    break;
                case NuiControlState.Down:
                    Render.DrawImageWithNineRect(
                       g, m_downImg,
                       ClientRectangle,
                       new Rectangle(0, 0, m_downImg.Width, m_downImg.Height));
                    break;
                case NuiControlState.Disabled:
                    DrawDisabledButton(g);
                    break;
                default:
                    break;
            }

            if (Image != null)
            {
                g.DrawImage(Image, imageRect, 0, 0, Image.Width, Image.Height, GraphicsUnit.Pixel);
            }

            Color textColor = Enabled ? ForeColor : SystemColors.GrayText;
            TextRenderer.DrawText(
                  g,
                  Text,
                  Font,
                  textRect,
                  textColor,
                  GetTextFormatFlags(TextAlign, RightToLeft == RightToLeft.Yes));

        }

        protected override void Dispose(bool disposing)
        {
            if (disposing)
            {
                if (m_normalImg != null) { m_normalImg.Dispose(); }
                if (m_hightImg != null) { m_hightImg.Dispose(); }
                if (m_downImg != null) { m_downImg.Dispose(); }
                if (m_focusImg != null) { m_focusImg.Dispose(); }
                if (m_defaultFont != null) { m_defaultFont.Dispose(); }
            }

            m_normalImg = null;
            m_hightImg = null;
            m_focusImg = null;
            m_downImg = null;
            m_defaultFont = null;
            base.Dispose(disposing);
        }

        #endregion

        #region Private

        private void SetStyles()
        {
            SetStyle(ControlStyles.UserPaint, true);
            SetStyle(ControlStyles.AllPaintingInWmPaint, true);
            SetStyle(ControlStyles.OptimizedDoubleBuffer, true);
            SetStyle(ControlStyles.ResizeRedraw, true);
            SetStyle(ControlStyles.SupportsTransparentBackColor, true);
            UpdateStyles();
        }

        private void CalculateRect(out Rectangle imageRect, out Rectangle textRect)
        {
            imageRect = Rectangle.Empty;
            textRect = Rectangle.Empty;
            if (Image == null)
            {
                textRect = new Rectangle(
                   3,
                   0,
                   Width - 6,
                   Height);
                return;
            }
            switch (TextImageRelation)
            {
                case TextImageRelation.Overlay:
                    imageRect = new Rectangle(
                        3,
                        (Height - ImageWidth) / 2,
                        ImageWidth,
                        ImageWidth);
                    textRect = new Rectangle(
                        3,
                        0,
                        Width - 6,
                        Height);
                    break;
                case TextImageRelation.ImageAboveText:
                    imageRect = new Rectangle(
                        (Width - ImageWidth) / 2,
                        3,
                        ImageWidth,
                        ImageWidth);
                    textRect = new Rectangle(
                        3,
                        imageRect.Bottom,
                        Width - 6,
                        Height - imageRect.Bottom - 2);
                    break;
                case TextImageRelation.ImageBeforeText:
                    imageRect = new Rectangle(
                        3,
                        (Height - ImageWidth) / 2,
                        ImageWidth,
                        ImageWidth);
                    textRect = new Rectangle(
                        imageRect.Right + 3,
                        0,
                        Width - imageRect.Right - 6,
                        Height);
                    break;
                case TextImageRelation.TextAboveImage:
                    imageRect = new Rectangle(
                        (Width - ImageWidth) / 2,
                        Height - ImageWidth - 3,
                        ImageWidth,
                        ImageWidth);
                    textRect = new Rectangle(
                        0,
                        3,
                        Width,
                        Height - imageRect.Y - 3);
                    break;
                case TextImageRelation.TextBeforeImage:
                    imageRect = new Rectangle(
                        Width - ImageWidth - 6,
                        (Height - ImageWidth) / 2,
                        ImageWidth,
                        ImageWidth);
                    textRect = new Rectangle(
                        3,
                        0,
                        imageRect.X - 3,
                        Height);
                    break;
            }

            if (RightToLeft == RightToLeft.Yes)
            {
                imageRect.X = Width - imageRect.Right;
                textRect.X = Width - textRect.Right;
            }
        }

        private void DrawDisabledButton(Graphics g)
        {
            int radius = 4;
            //此处让其宽度减1，让其由Normal态平滑自然的过渡到Disabled态，保持按钮高度一致。
            using (GraphicsPath borderPath = Render.CreateRoundPath(new Rectangle(ClientRectangle.X, ClientRectangle.Y, ClientRectangle.Width, ClientRectangle.Height -1), radius))
            {
                using (Pen disalbedPen = new Pen(Color.FromArgb(156, 165, 177)))
                {
                    g.DrawPath(disalbedPen, borderPath);
                }

                //背景层渐变,向内缩小1个像素
                Rectangle backRect = new Rectangle(ClientRectangle.X + 1, ClientRectangle.Y + 1, ClientRectangle.Width - 2, ClientRectangle.Height - 2 -1);
                using (GraphicsPath innerPath = Render.CreateRoundPath(backRect, radius))
                {
                    using (LinearGradientBrush lBrush = new LinearGradientBrush(backRect, Color.FromArgb(247, 252, 254), Color.FromArgb(230, 240, 243), LinearGradientMode.Vertical))
                    {
                        g.FillPath(lBrush, innerPath);
                    }
                }
            }
        }

        internal static TextFormatFlags GetTextFormatFlags(ContentAlignment alignment, bool rightToleft)
        {
            TextFormatFlags flags = TextFormatFlags.WordBreak |
                TextFormatFlags.SingleLine;
            if (rightToleft)
            {
                flags |= TextFormatFlags.RightToLeft | TextFormatFlags.Right;
            }

            switch (alignment)
            {
                case ContentAlignment.BottomCenter:
                    flags |= TextFormatFlags.Bottom | TextFormatFlags.HorizontalCenter;
                    break;
                case ContentAlignment.BottomLeft:
                    flags |= TextFormatFlags.Bottom | TextFormatFlags.Left;
                    break;
                case ContentAlignment.BottomRight:
                    flags |= TextFormatFlags.Bottom | TextFormatFlags.Right;
                    break;
                case ContentAlignment.MiddleCenter:
                    flags |= TextFormatFlags.HorizontalCenter |
                        TextFormatFlags.VerticalCenter;
                    break;
                case ContentAlignment.MiddleLeft:
                    flags |= TextFormatFlags.VerticalCenter | TextFormatFlags.Left;
                    break;
                case ContentAlignment.MiddleRight:
                    flags |= TextFormatFlags.VerticalCenter | TextFormatFlags.Right;
                    break;
                case ContentAlignment.TopCenter:
                    flags |= TextFormatFlags.Top | TextFormatFlags.HorizontalCenter;
                    break;
                case ContentAlignment.TopLeft:
                    flags |= TextFormatFlags.Top | TextFormatFlags.Left;
                    break;
                case ContentAlignment.TopRight:
                    flags |= TextFormatFlags.Top | TextFormatFlags.Right;
                    break;
            }
            return flags;
        }

        #endregion
    }
}
