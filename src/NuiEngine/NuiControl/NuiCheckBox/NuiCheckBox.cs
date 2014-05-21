using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Windows.Forms;
using System.Drawing;
using NuiEngine.Core;
using System.ComponentModel;
using System.Drawing.Drawing2D;
using NuiEngine.Utility;

namespace NuiEngine.NuiControl
{
    /// <summary>
    /// 复选框控件
    /// </summary>
    public class NuiCheckBox:CheckBox
    {
        #region Field

        private NuiControlState  m_state = NuiControlState.Normal;
        private Image m_checkImg = Properties.Resources.checkbox_check;
        private Font m_defaultFont = new Font("微软雅黑", 9);

        private static readonly ContentAlignment RightAlignment = ContentAlignment.TopRight | ContentAlignment.BottomRight | ContentAlignment.MiddleRight;
        private static readonly ContentAlignment LeftAlignment = ContentAlignment.TopLeft | ContentAlignment.BottomLeft | ContentAlignment.MiddleLeft;

        #endregion

        #region Constructor

        public NuiCheckBox()
        {
            SetStyles();
            this.BackColor = Color.Transparent;
            this.Font = m_defaultFont;
        }

        #endregion

        #region Properites

        [Description("获取QQCheckBox左边正方形的宽度")]
        protected virtual int CheckRectWidth
        {
            get { return 13; }
        }

        #endregion

        #region Override

        protected override void OnMouseEnter(EventArgs e)
        {
            m_state = NuiControlState.Highlight;
            base.OnMouseEnter(e);
        }

        protected override void OnMouseLeave(EventArgs e)
        {
            m_state = NuiControlState.Normal;
            base.OnMouseLeave(e);
        }

        protected override void OnMouseDown(MouseEventArgs mevent)
        {
            if (mevent.Button == MouseButtons.Left)
            {
                m_state = NuiControlState.Down;
            }
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
                    m_state = NuiControlState.Normal;
                }
            }
            base.OnMouseUp(mevent);
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
            base.OnEnabledChanged(e);
        }

        protected override void OnPaint(PaintEventArgs pevent)
        {
            base.OnPaint(pevent);
            base.OnPaintBackground(pevent);

            Graphics g = pevent.Graphics;
            g.SmoothingMode = SmoothingMode.AntiAlias;
            g.InterpolationMode = InterpolationMode.HighQualityBicubic;

            Rectangle checkRect, textRect;
            CalculateRect(out checkRect, out textRect);

            if (Enabled == false)
            {
                m_state = NuiControlState.Disabled;
            }

            switch (m_state)
            {
                case NuiControlState.Highlight:
                case NuiControlState.Down:
                    DrawHighLightCheckRect(g, checkRect);
                    break;
                case NuiControlState.Disabled:
                    DrawDisabledCheckRect(g, checkRect);
                    break;
                default:
                    DrawNormalCheckRect(g, checkRect);
                    break;
            }

            Color textColor = (Enabled == true) ? ForeColor : SystemColors.GrayText;
            TextRenderer.DrawText(
                g,
                Text,
                Font,
                textRect,
                textColor,
                MethodHelper.GetTextFormatFlags(TextAlign, RightToLeft == RightToLeft.Yes));
        }

        protected override void Dispose(bool disposing)
        {
            if (disposing)
            {
                if (m_checkImg != null)
                {
                    m_checkImg.Dispose();
                }
                if (m_defaultFont != null)
                {
                    m_defaultFont.Dispose();
                }
            }

            m_checkImg = null;
            m_defaultFont = null;
            base.Dispose(disposing);
        }

        #endregion

        #region Private

        private void DrawNormalCheckRect(Graphics g, Rectangle checkRect)
        {
            g.FillRectangle(Brushes.White, checkRect);
            using (Pen borderPen = new Pen(NuiConfig.QQBorderColor))
            {
                g.DrawRectangle(borderPen, checkRect);
            }
            if (Checked)
            {
                g.DrawImage(m_checkImg,checkRect, 0,0,m_checkImg.Width, m_checkImg.Height,GraphicsUnit.Pixel);
            }
        }

        private void DrawHighLightCheckRect(Graphics g, Rectangle checkRect)
        {
            DrawNormalCheckRect(g, checkRect);
            using (Pen p = new Pen(NuiConfig.QQHighLightInnerColor))
            {
                g.DrawRectangle(p, checkRect);

                checkRect.Inflate(1, 1);
                p.Color = NuiConfig.QQHighLightColor;

                g.DrawRectangle(p, checkRect);
            }


        }

        private void DrawDisabledCheckRect(Graphics g, Rectangle checkRect)
        {
            g.DrawRectangle(SystemPens.ControlDark, checkRect);
            if (Checked)
            {
                g.DrawImage(m_checkImg,checkRect,0,0, m_checkImg.Width,m_checkImg.Height,GraphicsUnit.Pixel);
            }
        }

        private void SetStyles()
        {
            SetStyle(ControlStyles.UserPaint, true);
            SetStyle(ControlStyles.AllPaintingInWmPaint, true);
            SetStyle(ControlStyles.OptimizedDoubleBuffer, true);
            SetStyle(ControlStyles.ResizeRedraw, true);
            SetStyle(ControlStyles.SupportsTransparentBackColor, true);
            UpdateStyles();
        }

        private void CalculateRect(out Rectangle checkRect, out Rectangle textRect)
        {
            checkRect = new Rectangle(
                0, 0, CheckRectWidth, CheckRectWidth);
            textRect = Rectangle.Empty;
            bool bCheckAlignLeft = (int)(LeftAlignment & CheckAlign) != 0;
            bool bCheckAlignRight = (int)(RightAlignment & CheckAlign) != 0;
            bool bRightToLeft = (RightToLeft == RightToLeft.Yes);

            if ((bCheckAlignLeft && !bRightToLeft) ||
                (bCheckAlignRight && bRightToLeft))
            {
                switch (CheckAlign)
                {
                    case ContentAlignment.TopRight:
                    case ContentAlignment.TopLeft:
                        checkRect.Y = 2;
                        break;
                    case ContentAlignment.MiddleRight:
                    case ContentAlignment.MiddleLeft:
                        checkRect.Y = (Height - CheckRectWidth) / 2;
                        break;
                    case ContentAlignment.BottomRight:
                    case ContentAlignment.BottomLeft:
                        checkRect.Y = Height - CheckRectWidth - 2;
                        break;
                }

                checkRect.X = 1;

                textRect = new Rectangle(
                    checkRect.Right + 2,
                    0,
                    Width - checkRect.Right - 4,
                    Height);
            }
            else if ((bCheckAlignRight && !bRightToLeft)
                || (bCheckAlignLeft && bRightToLeft))
            {
                switch (CheckAlign)
                {
                    case ContentAlignment.TopLeft:
                    case ContentAlignment.TopRight:
                        checkRect.Y = 2;
                        break;
                    case ContentAlignment.MiddleLeft:
                    case ContentAlignment.MiddleRight:
                        checkRect.Y = (Height - CheckRectWidth) / 2;
                        break;
                    case ContentAlignment.BottomLeft:
                    case ContentAlignment.BottomRight:
                        checkRect.Y = Height - CheckRectWidth - 2;
                        break;
                }

                checkRect.X = Width - CheckRectWidth - 1;

                textRect = new Rectangle(
                    2, 0, Width - CheckRectWidth - 6, Height);
            }
            else
            {
                switch (CheckAlign)
                {
                    case ContentAlignment.TopCenter:
                        checkRect.Y = 2;
                        textRect.Y = checkRect.Bottom + 2;
                        textRect.Height = Height - CheckRectWidth - 6;
                        break;
                    case ContentAlignment.MiddleCenter:
                        checkRect.Y = (Height - CheckRectWidth) / 2;
                        textRect.Y = 0;
                        textRect.Height = Height;
                        break;
                    case ContentAlignment.BottomCenter:
                        checkRect.Y = Height - CheckRectWidth - 2;
                        textRect.Y = 0;
                        textRect.Height = Height - CheckRectWidth - 6;
                        break;
                }

                checkRect.X = (Width - CheckRectWidth) / 2;

                textRect.X = 2;
                textRect.Width = Width - 4;
            }
        }

        #endregion
    }
}
