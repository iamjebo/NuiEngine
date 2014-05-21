using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Windows.Forms;
using System.Drawing;
using System.ComponentModel;
using NuiEngine.Utility;

namespace NuiEngine.NuiControl
{
    /// <summary>
    /// 图形按钮
    /// </summary>
    public class NuiImageButton: PictureBox, IButtonControl
    {
        #region  Fileds

        private DialogResult m_DialogResult;
        private Image m_HoverImage;
        private Image m_DownImage;
        private Image m_NormalImage;
        private bool m_hover = false;
        private bool m_down = false;
        private bool m_isDefault = false;
        private bool m_holdingSpace = false;

        private ToolTip m_toolTip = new ToolTip();

        #endregion

        #region Constructor

        public NuiImageButton(): base()
        {
            this.BackColor = Color.Transparent;
            this.Size = new Size(75, 23);
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

        [Category("Appearance")]
        [Description("Image to show when the button is hovered over.")]
        public Image HoverImage
        {
            get { return m_HoverImage; }
            set { m_HoverImage = value; if (m_hover) Image = value; }
        }

        [Category("Appearance")]
        [Description("Image to show when the button is depressed.")]
        public Image DownImage
        {
            get { return m_DownImage; }
            set { m_DownImage = value; if (m_down) Image = value; }
        }

        [Category("Appearance")]
        [Description("Image to show when the button is not in any other state.")]
        public Image NormalImage
        {
            get { return m_NormalImage; }
            set { m_NormalImage = value; if (!(m_hover || m_down)) Image = value; }
        }

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

        [Browsable(true)]
        [DesignerSerializationVisibility(DesignerSerializationVisibility.Visible)]
        [Category("Appearance")]
        public override Color ForeColor
        {
            get
            {
                return base.ForeColor;
            }
            set
            {
                base.ForeColor = value;
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
        public new Image Image { get { return base.Image; } set { base.Image = value; } }

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
            base.OnMouseEnter(e);
        }

        protected override void OnMouseMove(MouseEventArgs e)
        {
            m_hover = true;
            if (m_down)
            {
                if ((m_DownImage != null) && (Image != m_DownImage))
                    Image = m_DownImage;
            }
            else
                if (m_HoverImage != null)
                    Image = m_HoverImage;
                else
                    Image = m_NormalImage;
            base.OnMouseMove(e);
        }

        protected override void OnMouseLeave(EventArgs e)
        {
            m_hover = false;
            Image = m_NormalImage;
            base.OnMouseLeave(e);
        }

        protected override void OnMouseDown(MouseEventArgs e)
        {
            base.Focus();
            OnMouseUp(null);
            m_down = true;
            if (m_DownImage != null)
                Image = m_DownImage;
            base.OnMouseDown(e);
        }

        protected override void OnMouseUp(MouseEventArgs e)
        {
            m_down = false;
            if (m_hover)
            {
                if (m_HoverImage != null)
                    Image = m_HoverImage;
            }
            else
                Image = m_NormalImage;
            base.OnMouseUp(e);
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

        protected override void OnLostFocus(EventArgs e)
        {
            m_holdingSpace = false;
            OnMouseUp(null);
            base.OnLostFocus(e);
        }

        protected override void OnPaint(PaintEventArgs pe)
        {
            base.OnPaint(pe);
            if ((!string.IsNullOrEmpty(Text)) && (pe != null) && (base.Font != null))
            {
                SizeF drawStringSize = pe.Graphics.MeasureString(base.Text, base.Font);
                PointF drawPoint;
                if (base.Image != null)
                {
                    drawPoint = new PointF(
                        base.Image.Width / 2 - drawStringSize.Width / 2,
                        base.Image.Height / 2 - drawStringSize.Height / 2);
                }
                else
                {
                    drawPoint = new PointF(
                        base.Width / 2 - drawStringSize.Width / 2,
                        base.Height / 2 - drawStringSize.Height / 2);
                }

                using (SolidBrush drawBrush = new SolidBrush(base.ForeColor))
                {
                    pe.Graphics.DrawString(base.Text, base.Font, drawBrush, drawPoint);
                }

            }
        }

        protected override void Dispose(bool disposing)
        {
            if (disposing)
            {
                if (m_toolTip != null)
                    m_toolTip.Dispose();
            }
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
