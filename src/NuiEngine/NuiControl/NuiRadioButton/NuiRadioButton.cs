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
    /// 单选框按钮
    /// </summary>
    public class NuiRadioButton : RadioButton
    {
        #region Field

        private NuiControlState m_state = NuiControlState.Normal;
        private Image m_dotImg = Properties.Resources.radiobox_dot;
        private Font m_defaultFont = new Font("微软雅黑", 9);

        private static readonly ContentAlignment RightAlignment = ContentAlignment.TopRight | ContentAlignment.BottomRight | ContentAlignment.MiddleRight;
        private static readonly ContentAlignment LeftAlignment = ContentAlignment.TopLeft | ContentAlignment.BottomLeft | ContentAlignment.MiddleLeft;

        #endregion

        #region Constructor

        public NuiRadioButton()
        {
            SetStyles();
            this.BackColor = Color.Transparent;
            this.Font = m_defaultFont;
        }

        #endregion

        #region Properites

        [Description("获取QQRadioButton左边正方形的宽度")]
        protected virtual int CheckRectWidth
        {
            get { return 12; }
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

            Rectangle circleRect, textRect;
            CalculateRect(out circleRect, out textRect);

            if (Enabled == false)
            {
                m_state = NuiControlState.Disabled;
            }

            switch (m_state)
            {
                case NuiControlState.Highlight:
                case NuiControlState.Down:
                    DrawHighLightCircle(g, circleRect);
                    break;
                case NuiControlState.Disabled:
                    DrawDisabledCircle(g, circleRect);
                    break;
                default:
                    DrawNormalCircle(g, circleRect);
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
                if (m_dotImg != null)
                {
                    m_dotImg.Dispose();
                }
                if (m_defaultFont != null)
                {
                    m_defaultFont.Dispose();
                }
            }

            m_dotImg = null;
            m_defaultFont = null;
            base.Dispose(disposing);
        }

        #endregion

        #region Private

        private void DrawNormalCircle(Graphics g, Rectangle circleRect)
        {
            g.FillEllipse(Brushes.White, circleRect);
            using (Pen borderPen = new Pen(NuiConfig.QQBorderColor))
            {
                g.DrawEllipse(borderPen, circleRect);
            }
            if (Checked)
            {
                circleRect.Inflate(-2, -2);
                g.DrawImage(
                    m_dotImg,
                    new Rectangle(circleRect.X + 1, circleRect.Y + 1, circleRect.Width - 1, circleRect.Height - 1),
                    0,
                    0,
                    m_dotImg.Width,
                    m_dotImg.Height,
                    GraphicsUnit.Pixel);
            }
        }

        private void DrawHighLightCircle(Graphics g, Rectangle circleRect)
        {
            DrawNormalCircle(g, circleRect);
            using (Pen p = new Pen(NuiConfig.QQHighLightInnerColor))
            {
                g.DrawEllipse(p, circleRect);

                circleRect.Inflate(1, 1);
                p.Color = NuiConfig.QQHighLightColor;

                g.DrawEllipse(p, circleRect);
            }
        }

        private void DrawDisabledCircle(Graphics g, Rectangle circleRect)
        {
            g.DrawEllipse(SystemPens.ControlDark, circleRect);
            if (Checked)
            {
                circleRect.Inflate(-2, -2);
                g.DrawImage(
                    m_dotImg,
                    new Rectangle(circleRect.X + 1, circleRect.Y + 1, circleRect.Width - 1, circleRect.Height - 1),
                    0,
                    0,
                    m_dotImg.Width,
                    m_dotImg.Height,
                    GraphicsUnit.Pixel);
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

        private void CalculateRect(out Rectangle circleRect, out Rectangle textRect)
        {
            circleRect = new Rectangle(
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
                        circleRect.Y = 2;
                        break;
                    case ContentAlignment.MiddleRight:
                    case ContentAlignment.MiddleLeft:
                        circleRect.Y = (Height - CheckRectWidth) / 2;
                        break;
                    case ContentAlignment.BottomRight:
                    case ContentAlignment.BottomLeft:
                        circleRect.Y = Height - CheckRectWidth - 2;
                        break;
                }

                circleRect.X = 1;

                textRect = new Rectangle(
                    circleRect.Right + 2,
                    0,
                    Width - circleRect.Right - 4,
                    Height);
            }
            else if ((bCheckAlignRight && !bRightToLeft)
                || (bCheckAlignLeft && bRightToLeft))
            {
                switch (CheckAlign)
                {
                    case ContentAlignment.TopLeft:
                    case ContentAlignment.TopRight:
                        circleRect.Y = 2;
                        break;
                    case ContentAlignment.MiddleLeft:
                    case ContentAlignment.MiddleRight:
                        circleRect.Y = (Height - CheckRectWidth) / 2;
                        break;
                    case ContentAlignment.BottomLeft:
                    case ContentAlignment.BottomRight:
                        circleRect.Y = Height - CheckRectWidth - 2;
                        break;
                }

                circleRect.X = Width - CheckRectWidth - 1;

                textRect = new Rectangle(
                    2, 0, Width - CheckRectWidth - 6, Height);
            }
            else
            {
                switch (CheckAlign)
                {
                    case ContentAlignment.TopCenter:
                        circleRect.Y = 2;
                        textRect.Y = circleRect.Bottom + 2;
                        textRect.Height = Height - CheckRectWidth - 6;
                        break;
                    case ContentAlignment.MiddleCenter:
                        circleRect.Y = (Height - CheckRectWidth) / 2;
                        textRect.Y = 0;
                        textRect.Height = Height;
                        break;
                    case ContentAlignment.BottomCenter:
                        circleRect.Y = Height - CheckRectWidth - 2;
                        textRect.Y = 0;
                        textRect.Height = Height - CheckRectWidth - 6;
                        break;
                }

                circleRect.X = (Width - CheckRectWidth) / 2;

                textRect.X = 2;
                textRect.Width = Width - 4;
            }
        }

        #endregion
    }
}
