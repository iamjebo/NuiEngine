using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Windows.Forms;
using System.Drawing;
using System.ComponentModel;
using NuiEngine.Core;
using NuiEngine.Utility;

namespace NuiEngine.NuiControl
{
    /// <summary>
    /// 玻璃按钮控件
    /// </summary>
    public class NuiGlassButton:PictureBox, IButtonControl
    {
        #region  Fileds

        private DialogResult m_DialogResult;
        private bool m_isDefault = false;
        private bool m_holdingSpace = false;

        private NuiControlState m_state = NuiControlState.Normal;
        private Font m_defaultFont = new Font("微软雅黑", 9);

        private Image m_glassHotImg = Properties.Resources.glassbtn_hot;
        private Image m_glassDownImg = Properties.Resources.glassbtn_down;

        private ToolTip m_toolTip = new ToolTip();

        #endregion

        #region Constructor

        public NuiGlassButton(): base()
        {
            this.BackColor = Color.Transparent;
            this.Size = new Size(75, 23);
            this.BorderStyle = BorderStyle.None;
            this.Font = m_defaultFont;
        }

        #endregion

        #region IButtonControl Members

        public DialogResult DialogResult
        {
            get
            {
                return m_DialogResult;
            }
            set
            {
                if (Enum.IsDefined(typeof(DialogResult), value))
                {
                    m_DialogResult = value;
                }
            }
        }

        public void NotifyDefault(bool value)
        {
            if (m_isDefault != value)
            {
                m_isDefault = value;
            }
        }

        public void PerformClick()
        {
            base.OnClick(EventArgs.Empty);
        }

        #endregion

        #region  Properties

        [Browsable(true)]
        [DesignerSerializationVisibility(DesignerSerializationVisibility.Visible)]
        [Category("Appearance")]
        [Description("The text associated with the control.")]
        public override string Text
        {
            get
            {
                return base.Text;
            }
            set
            {
                base.Text = value;
            }
        }

        [Browsable(true)]
        [DesignerSerializationVisibility(DesignerSerializationVisibility.Visible)]
        [Category("Appearance")]
        [Description("The font used to display text in the control.")]
        public override Font Font
        {
            get
            {
                return base.Font;
            }
            set
            {
                base.Font = value;
            }
        }

        [Description("当鼠标放在控件可见处的提示文本")]
        public string ToolTipText { get; set; }

        #endregion

        #region Description Changes
        [Description("Controls how the ImageButton will handle image placement and control sizing.")]
        public new PictureBoxSizeMode SizeMode { get { return base.SizeMode; } set { base.SizeMode = value; } }

        [Description("Controls what type of border the ImageButton should have.")]
        public new BorderStyle BorderStyle { get { return base.BorderStyle; } set { base.BorderStyle = value; } }
        #endregion

        #region Hiding

        [Browsable(false)]
        [DesignerSerializationVisibility(DesignerSerializationVisibility.Hidden)]
        public new ImageLayout BackgroundImageLayout { get { return base.BackgroundImageLayout; } set { base.BackgroundImageLayout = value; } }

        [Browsable(false)]
        [DesignerSerializationVisibility(DesignerSerializationVisibility.Hidden)]
        public new Image BackgroundImage { get { return base.BackgroundImage; } set { base.BackgroundImage = value; } }

        [Browsable(false)]
        [DesignerSerializationVisibility(DesignerSerializationVisibility.Hidden)]
        public new String ImageLocation { get { return base.ImageLocation; } set { base.ImageLocation = value; } }

        [Browsable(false)]
        [DesignerSerializationVisibility(DesignerSerializationVisibility.Hidden)]
        public new Image ErrorImage { get { return base.ErrorImage; } set { base.ErrorImage = value; } }

        [Browsable(false)]
        [DesignerSerializationVisibility(DesignerSerializationVisibility.Hidden)]
        public new Image InitialImage { get { return base.InitialImage; } set { base.InitialImage = value; } }

        [Browsable(false)]
        [DesignerSerializationVisibility(DesignerSerializationVisibility.Hidden)]
        public new bool WaitOnLoad { get { return base.WaitOnLoad; } set { base.WaitOnLoad = value; } }
        #endregion

        #region override

        protected override void OnMouseEnter(EventArgs e)
        {
            //show tool tip 
            if (ToolTipText != string.Empty)
            {
                HideToolTip();
                ShowTooTip(ToolTipText);
            }

            m_state = NuiControlState.Highlight;
            Invalidate();
            base.OnMouseEnter(e);
        }

        protected override void OnMouseLeave(EventArgs e)
        {
            m_state = NuiControlState.Normal;
            Invalidate();
            base.OnMouseLeave(e);
        }

        protected override void OnMouseDown(MouseEventArgs e)
        {
            if (e.Button == MouseButtons.Left)
                m_state = NuiControlState.Down;
            Invalidate();
            base.OnMouseDown(e);
        }

        protected override void OnMouseUp(MouseEventArgs e)
        {
            if (e.Button == MouseButtons.Left)
            {
                if (ClientRectangle.Contains(e.Location))
                    m_state = NuiControlState.Highlight;
                else
                    m_state = NuiControlState.Normal;
            }
            Invalidate();
            base.OnMouseUp(e);
        }

        protected override void OnLostFocus(EventArgs e)
        {

            m_state = NuiControlState.Normal;
            Invalidate();
            m_holdingSpace = false;
            base.OnLostFocus(e);
        }

        protected override void OnPaint(PaintEventArgs pe)
        {
            Rectangle imageRect, textRect;
            CalculateRect(out imageRect, out textRect);
            switch (m_state)
            {
                case NuiControlState.Highlight:
                    Render.DrawImageWithNineRect(
                         pe.Graphics,
                       m_glassHotImg,
                        ClientRectangle,
                        new Rectangle(0, 0, m_glassDownImg.Width, m_glassDownImg.Height));
                    break;
                case NuiControlState.Down:
                    Render.DrawImageWithNineRect(
                         pe.Graphics,
                        m_glassDownImg,
                        ClientRectangle,
                        new Rectangle(0, 0, m_glassDownImg.Width, m_glassDownImg.Height));
                    break;
                default:
                    break;
            }

            if (Image != null)
            {
                pe.Graphics.DrawImage(
                    Image,
                    imageRect,
                    new Rectangle(0, 0, Image.Width, Image.Height),
                    GraphicsUnit.Pixel);
            }

            if (Text.Trim().Length != 0)
            {
                TextRenderer.DrawText(
                    pe.Graphics,
                    Text,
                    Font,
                    textRect,
                    SystemColors.ControlText);
            }
        }

        public override bool PreProcessMessage(ref Message msg)
        {
            if (msg.Msg == Win32.WM_KEYUP)
            {
                if (m_holdingSpace)
                {
                    if ((int)msg.WParam == (int)Keys.Space)
                    {
                        OnMouseUp(null);
                        PerformClick();
                    }
                    else if ((int)msg.WParam == (int)Keys.Escape
                        || (int)msg.WParam == (int)Keys.Tab)
                    {
                        m_holdingSpace = false;
                        OnMouseUp(null);
                    }
                }
                return true;
            }
            else if (msg.Msg == Win32.WM_KEYDOWN)
            {
                if ((int)msg.WParam == (int)Keys.Space)
                {
                    m_holdingSpace = true;
                    OnMouseDown(null);
                }
                else if ((int)msg.WParam == (int)Keys.Enter)
                {
                    PerformClick();
                }
                return true;
            }
            else
                return base.PreProcessMessage(ref msg);
        }

        protected override void Dispose(bool disposing)
        {
            if (disposing)
            {
                if (m_defaultFont != null)
                    m_defaultFont.Dispose();
                if (m_glassDownImg != null)
                    m_glassDownImg.Dispose();
                if (m_glassHotImg != null)
                    m_glassHotImg.Dispose();
                if (m_toolTip != null)
                    m_toolTip.Dispose();
            }
            m_defaultFont = null;
            m_glassDownImg = null;
            m_glassHotImg = null;
            m_toolTip = null;
            base.Dispose(disposing);
        }

        protected override void OnTextChanged(EventArgs e)
        {
            Refresh();
            base.OnTextChanged(e);
        }

        #endregion

        #region Private

        private void CalculateRect(out Rectangle imageRect, out Rectangle textRect)
        {
            imageRect = Rectangle.Empty;
            textRect = Rectangle.Empty;
            if (Image == null && !string.IsNullOrEmpty(Text))
            {
                textRect = new Rectangle(3, 0, Width - 6, Height);
            }
            else if (Image != null && string.IsNullOrEmpty(Text))
            {
                imageRect = new Rectangle((Width - Image.Width) / 2, (Height - Image.Height) / 2, Image.Width, Image.Height);
            }
            else if (Image != null && !string.IsNullOrEmpty(Text))
            {
                imageRect = new Rectangle(4, (Height - Image.Height) / 2, Image.Width, Image.Height);
                textRect = new Rectangle(imageRect.Right + 1, 0, Width - 4 * 2 - imageRect.Width - 1, Height);
            }
        }

        private void ShowTooTip(string toolTipText)
        {
            m_toolTip.Active = true;
            m_toolTip.SetToolTip(this, toolTipText);
        }

        private void HideToolTip()
        {
            m_toolTip.Active = false;
        }

        #endregion
    }
}
