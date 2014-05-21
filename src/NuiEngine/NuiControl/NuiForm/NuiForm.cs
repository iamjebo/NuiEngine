using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Drawing;
using System.ComponentModel;

using NuiEngine.Core;
using NuiEngine.Utility;
using System.Windows.Forms;
using System.Drawing.Drawing2D;


namespace NuiEngine.NuiControl
{
    /// <summary>
    /// 窗体控件
    /// </summary>
    public class NuiForm:NuiFormBase
    {
        private int m_radius = 5;                                                   // 窗体圆角的半径
        private bool m_canResize = true;                                            // 是否允许窗体改变大小

        private Font m_textFont = new Font("微软雅黑", 10.0f, FontStyle.Bold);      // 绘制窗体标题的字体
        private Color m_textForeColor = Color.FromArgb(250, Color.White);           // 绘制窗体标题的颜色
        private bool m_isTextWithShadow = false;                                    // 是否绘制带有阴影的窗体标题 
        private Color m_textShadowColor = Color.FromArgb(2, Color.Black);           // 标题的阴影颜色
        private int m_textShadowWidth = 4;                                          // 标题阴影的宽度

        private Image m_formFringe =Properties.Resources.form_fringe_bkg;           // 窗体边框图片
        private Image m_formBkg;                                                    // 窗体背景图片
        
        private SysBtnManager m_sysBtnManager;                                      // 系统按钮管理器

        public NuiForm()
        {
            InitializeComponent();
            NuiFormIni();
            m_sysBtnManager = new SysBtnManager(this);
        }

        #region Properties

        [Description("窗体圆角的半径")]
        public int Radius
        {
            get
            {
                return m_radius;
            }
            set
            {
                if (m_radius != value)
                {
                    m_radius = value;
                    this.Invalidate();
                }
            }
        }

        [Description("是否允许窗体改变大小")]
        public bool CanResize
        {
            get
            {
                return m_canResize;
            }
            set
            {
                if (m_canResize != value)
                {
                    m_canResize = value;
                }
            }
        }

        public override Image BackgroundImage
        {
            get
            {
                return m_formBkg;
            }
            set
            {
                if (m_formBkg != value)
                {
                    m_formBkg = value;
                    Invalidate();
                }
            }
        }

        [Description("用于绘制窗体标题的字体")]
        public Font TextFont
        {
            get { return m_textFont; }
            set
            {
                if (m_textFont != value)
                {
                    m_textFont = value;
                }
            }

        }

        [Description("用于绘制窗体标题的颜色")]
        public Color TextForeColor
        {
            get { return m_textForeColor; }
            set
            {
                if (m_textForeColor != value)
                { m_textForeColor = value; }
            }
        }

        [Description("是否绘制带有阴影的窗体标题")]
        public bool TextWithShadow
        {
            get { return m_isTextWithShadow; }
            set
            {
                if (m_isTextWithShadow != value)
                {
                    m_isTextWithShadow = value;
                }
            }
        }

        [Description("如果TextWithShadow属性为True,则使用该属性绘制阴影")]
        public Color TextShadowColor
        {
            get { return m_textShadowColor; }
            set
            {
                if (m_textShadowColor != value)
                {
                    m_textShadowColor = value;
                }
            }
        }

        [Description("如果TextWithShadow属性为True,则使用该属性获取或色泽阴影的宽度")]
        public int TextShadowWidth
        {
            get { return m_textShadowWidth; }
            set
            {
                if (m_textShadowWidth != value)
                {
                    m_textShadowWidth = value;
                }
            }
        }

        [Browsable(false)]
        [Description("返回窗体关闭系统按钮所在的坐标矩形")]
        public Rectangle CloseBoxRect
        {
            get { return SysBtnManager.SysButtonArray[0].LocationRect; }
        }

        [Browsable(false)]
        [Description("返回窗体最大化或者还原系统按钮所在的坐标矩形")]
        public Rectangle MaximiziBoxRect
        {
            get { return SysBtnManager.SysButtonArray[1].LocationRect; }
        }

        [Browsable(false)]
        [Description("返回窗体最小化系统按钮所在的坐标矩形")]
        public Rectangle MinimiziBoxRect 
        {
            get { return SysBtnManager.SysButtonArray[2].LocationRect; }
        }

        internal Rectangle IconRect
        {
            get
            {
                if (base.ShowIcon && base.Icon != null)
                {
                    return new Rectangle(8, 6, SystemInformation.SmallIconSize.Width, SystemInformation.SmallIconSize.Width);
                }
                return Rectangle.Empty;
            }
        }

        internal Rectangle TextRect
        {
            get
            {
                if (base.Text.Length != 0)
                {
                    return new Rectangle(IconRect.Right + 2, 4, Width - (8 + IconRect.Width + 2), TextFont.Height);
                }
                return Rectangle.Empty;
            }
        }

        internal SysBtnManager SysBtnManager
        {
            get
            {
                if (m_sysBtnManager == null)
                {
                    m_sysBtnManager = new SysBtnManager(this);
                }
                return m_sysBtnManager;
            }
        }

        [Browsable(false)]
        [DesignerSerializationVisibility(DesignerSerializationVisibility.Hidden)]
        public new FormBorderStyle FormBorderStyle
        {
            get { return base.FormBorderStyle; }
            set { base.FormBorderStyle = value; }
        }

        #endregion

        #region Overrides

        protected override CreateParams CreateParams
        {
            get
            {
                CreateParams cp = base.CreateParams;
                if (!DesignMode)
                {
                    if (MaximizeBox) { cp.Style |= (int)WindowStyle.WS_MAXIMIZEBOX; }
                    if (MinimizeBox) { cp.Style |= (int)WindowStyle.WS_MINIMIZEBOX; }
                    //cp.ExStyle |= (int)WindowStyle.WS_CLIPCHILDREN;  //防止因窗体控件太多出现闪烁
                    cp.ClassStyle |= (int)ClassStyle.CS_DropSHADOW;  //实现窗体边框阴影效果
                }
                return cp;
            }
        }

        protected override void OnCreateControl()
        {
            base.OnCreateControl();
            Render.SetFormRoundRectRgn(this, Radius);
        }

        protected override void OnLoad(EventArgs e)
        {
            base.OnLoad(e);
            UpdateSystemButtonRect();
        }

        protected override void OnSizeChanged(EventArgs e)
        {
            base.OnSizeChanged(e);
            Render.SetFormRoundRectRgn(this, Radius);
            UpdateSystemButtonRect();
            UpdateMaxButton();
        }

        protected override void WndProc(ref Message m)
        {
            switch (m.Msg)
            {
                case Win32.WM_ERASEBKGND:
                    m.Result = IntPtr.Zero;
                    break;
                case Win32.WM_NCHITTEST:
                    WmNcHitTest(ref m);
                    break;
                default:
                    base.WndProc(ref m);
                    break;
            }
        }

        protected override void OnMouseMove(MouseEventArgs e)
        {
            base.OnMouseMove(e);
            SysBtnManager.ProcessMouseOperate(e.Location, NuiMouseOperate.Move);
        }

        protected override void OnMouseDown(MouseEventArgs e)
        {
            base.OnMouseDown(e);
            if (e.Button == MouseButtons.Left)
            {
                SysBtnManager.ProcessMouseOperate(e.Location, NuiMouseOperate.LeftDown);
            }
        }

        protected override void OnMouseUp(MouseEventArgs e)
        {
            base.OnMouseUp(e);
            if (e.Button == MouseButtons.Left)
            {
                SysBtnManager.ProcessMouseOperate(e.Location, NuiMouseOperate.LeftUp);
            }
        }

        protected override void OnMouseLeave(EventArgs e)
        {
            base.OnMouseLeave(e);
            SysBtnManager.ProcessMouseOperate(Point.Empty, NuiMouseOperate.Leave);
        }

        protected override void OnPaint(PaintEventArgs e)
        {
            e.Graphics.SmoothingMode = SmoothingMode.AntiAlias;
            e.Graphics.InterpolationMode = InterpolationMode.HighQualityBicubic;

            //draw BackgroundImage
            if (BackgroundImage != null)
            {
                switch (BackgroundImageLayout)
                {
                    case ImageLayout.Stretch:
                    case ImageLayout.Zoom:
                        e.Graphics.DrawImage(
                            m_formBkg,
                            ClientRectangle,
                            new Rectangle(0, 0, m_formBkg.Width, m_formBkg.Height),
                            GraphicsUnit.Pixel);
                        break;
                    case ImageLayout.Center:
                    case ImageLayout.None:
                    case ImageLayout.Tile:
                        e.Graphics.DrawImage(
                            m_formBkg,
                            ClientRectangle,
                            ClientRectangle,
                            GraphicsUnit.Pixel);
                        break;
                }
            }

            //draw system buttons
            SysBtnManager.DrawSystemButtons(e.Graphics);

            //draw fringe
            Render.DrawFormFringe(this, e.Graphics, m_formFringe, Radius);

            //draw icon
            if (Icon != null && ShowIcon)
            {
                e.Graphics.DrawIcon(Icon, IconRect);
            }

            //draw text
            if (Text.Length != 0)
            {
                if (TextWithShadow)
                {
                    using (Image textImg = Render.GetStringImgWithShadowEffect(Text, TextFont, TextForeColor, TextShadowColor, TextShadowWidth))
                    {
                        e.Graphics.DrawImage(textImg,TextRect.Location);
                    }
                }
                else
                {
                    TextRenderer.DrawText(
                    e.Graphics,
                    Text, TextFont,
                    TextRect,
                    TextForeColor,
                    TextFormatFlags.SingleLine | TextFormatFlags.EndEllipsis);
                }
            }

        }

        protected override void Dispose(bool disposing)
        {
            if (disposing)
            {
                if (m_sysBtnManager != null)
                {
                    m_sysBtnManager.Dispose();
                    m_sysBtnManager = null;

                    m_formFringe.Dispose();
                    m_formFringe = null;

                    m_textFont.Dispose();
                    m_textFont = null;

                    if (m_formBkg != null)
                    {
                        m_formBkg.Dispose();
                        m_formBkg = null;
                    }
                }
            }
            base.Dispose(disposing);
        }

        #endregion

        #region Private Methods

        private void InitializeComponent()
        {
            this.SuspendLayout();
            // 
            // NuiForm
            // 
            this.ClientSize = new System.Drawing.Size(684, 402);
            this.FormBorderStyle = System.Windows.Forms.FormBorderStyle.None;
            this.Name = " NuiForm";
            this.ResumeLayout(false);
        }

        private void NuiFormIni()
        {
            this.MaximumSize = Screen.PrimaryScreen.WorkingArea.Size;
            SetStyles();
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

        private void WmNcHitTest(ref Message m)  //调整窗体大小
        {
            int wparam = m.LParam.ToInt32();
            Point mouseLocation = new Point(MethodHelper.LOWORD(wparam), MethodHelper.HIWORD(wparam));
            mouseLocation = PointToClient(mouseLocation);

            if (WindowState != FormWindowState.Maximized)
            {
                if (CanResize == true)
                {
                    if (mouseLocation.X < 5 && mouseLocation.Y < 5)
                    {
                        m.Result = new IntPtr(Win32.HTTOPLEFT);
                        return;
                    }

                    if (mouseLocation.X > Width - 5 && mouseLocation.Y < 5)
                    {
                        m.Result = new IntPtr(Win32.HTTOPRIGHT);
                        return;
                    }

                    if (mouseLocation.X < 5 && mouseLocation.Y > Height - 5)
                    {
                        m.Result = new IntPtr(Win32.HTBOTTOMLEFT);
                        return;
                    }

                    if (mouseLocation.X > Width - 5 && mouseLocation.Y > Height - 5)
                    {
                        m.Result = new IntPtr(Win32.HTBOTTOMRIGHT);
                        return;
                    }

                    if (mouseLocation.Y < 3)
                    {
                        m.Result = new IntPtr(Win32.HTTOP);
                        return;
                    }

                    if (mouseLocation.Y > Height - 3)
                    {
                        m.Result = new IntPtr(Win32.HTBOTTOM);
                        return;
                    }

                    if (mouseLocation.X < 3)
                    {
                        m.Result = new IntPtr(Win32.HTLEFT);
                        return;
                    }

                    if (mouseLocation.X > Width - 3)
                    {
                        m.Result = new IntPtr(Win32.HTRIGHT);
                        return;
                    }
                }
            }
            m.Result = new IntPtr(Win32.HTCLIENT);
        }

        private void UpdateMaxButton()
        {
            bool isMax = WindowState == FormWindowState.Maximized;
            if (isMax)
            {
                SysBtnManager.SysButtonArray[1].NormalImg = Properties.Resources.sys_restore_normal;
                SysBtnManager.SysButtonArray[1].HighLightImg = Properties.Resources.sys_restore_highlight;
                SysBtnManager.SysButtonArray[1].DownImg = Properties.Resources.sys_restore_down;
                SysBtnManager.SysButtonArray[1].ToolTip = "还原";
            }
            else
            {
                SysBtnManager.SysButtonArray[1].NormalImg = Properties.Resources.sys_max_normal;
                SysBtnManager.SysButtonArray[1].HighLightImg = Properties.Resources.sys_max_highlight;
                SysBtnManager.SysButtonArray[1].DownImg = Properties.Resources.sys_max_down;
                SysBtnManager.SysButtonArray[1].ToolTip = "最大化";
            }
        }

        protected void UpdateSystemButtonRect()
        {
            bool isShowMaxButton = MaximizeBox;
            bool isShowMinButton = MinimizeBox;
            Rectangle closeRect = new Rectangle(
                    Width - SysBtnManager.SysButtonArray[0].NormalImg.Width,
                    -1,
                    SysBtnManager.SysButtonArray[0].NormalImg.Width,
                    SysBtnManager.SysButtonArray[0].NormalImg.Height);

            //update close button location rect.
            SysBtnManager.SysButtonArray[0].LocationRect = closeRect;

            //Max
            if (isShowMaxButton)
            {
                SysBtnManager.SysButtonArray[1].LocationRect = new Rectangle(
                    closeRect.X - SysBtnManager.SysButtonArray[1].NormalImg.Width,
                    -1,
                    SysBtnManager.SysButtonArray[1].NormalImg.Width,
                    SysBtnManager.SysButtonArray[1].NormalImg.Height);
            }
            else
            {
                SysBtnManager.SysButtonArray[1].LocationRect = Rectangle.Empty;
            }

            //Min
            if (!isShowMinButton)
            {
                SysBtnManager.SysButtonArray[2].LocationRect = Rectangle.Empty;
                return;
            }
            if (isShowMaxButton)
            {
                SysBtnManager.SysButtonArray[2].LocationRect = new Rectangle(
                    SysBtnManager.SysButtonArray[1].LocationRect.X - SysBtnManager.SysButtonArray[2].NormalImg.Width,
                    -1,
                    SysBtnManager.SysButtonArray[2].NormalImg.Width,
                    SysBtnManager.SysButtonArray[2].NormalImg.Height);
            }
            else
            {
                SysBtnManager.SysButtonArray[2].LocationRect = new Rectangle(
                   closeRect.X - SysBtnManager.SysButtonArray[2].NormalImg.Width,
                   -1,
                   SysBtnManager.SysButtonArray[2].NormalImg.Width,
                   SysBtnManager.SysButtonArray[2].NormalImg.Height);
            }
        }

        #endregion
    }
}
