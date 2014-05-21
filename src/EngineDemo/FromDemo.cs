using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.Linq;
using System.Text;
using System.Windows.Forms;
using NuiEngine.NuiControl;
using System.Drawing.Drawing2D;

namespace EngineDemo
{
    public partial class FromDemo : NuiForm
    {
        private bool m_go = false;

        public FromDemo()
        {
            InitializeComponent();
        }

        protected override CreateParams CreateParams
        {
            get
            {
                CreateParams cp = base.CreateParams;
                if (!DesignMode)
                {
                    cp.ExStyle |= (int)WindowStyle.WS_CLIPCHILDREN;
                }
                return cp;
            }
        }

        protected override void OnPaint(PaintEventArgs e)
        {
            base.OnPaint(e);
            DrawFromAlphaMainPart(this, e.Graphics);
        }

        // 绘制窗体主体部分白色透明层
        private void DrawFromAlphaMainPart(Form form, Graphics g)
        {
            Color[] colors = 
            {
                Color.FromArgb(5, Color.White),
                Color.FromArgb(30, Color.White),
                Color.FromArgb(165, Color.White),
                Color.FromArgb(170, Color.White),
                Color.FromArgb(30, Color.White),
                Color.FromArgb(5, Color.White)
            };

            float[] pos = 
            {
                0.0f,
                0.04f,
                0.10f,
                0.95f,
                0.98f,
                1.0f      
            };

            ColorBlend colorBlend = new ColorBlend(6);
            colorBlend.Colors = colors;
            colorBlend.Positions = pos;

            RectangleF destRect = new RectangleF(0, 0, form.Width, form.Height);
            using (LinearGradientBrush lBrush = new LinearGradientBrush(destRect, colors[0], colors[5], LinearGradientMode.Vertical))
            {
                lBrush.InterpolationColors = colorBlend;
                g.FillRectangle(lBrush, destRect);
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

        private void FromDemo_Load(object sender, EventArgs e)
        {
            this.Left = (Screen.PrimaryScreen.WorkingArea.Width - Width) / 2;
            this.Top = (Screen.PrimaryScreen.WorkingArea.Height - Height) / 2;

            marqueeprogressBarEx1.MarqueeStart();
            timerMarquee.Enabled = true;

            animateprogressBarEx1.Value = 100;
            animateprogressBarEx1.StartAnimation();
        }

        private void nuiBtnSure_Click(object sender, EventArgs e)
        {
            NuiMessageBox.Show(this, "修改密码成功", "提示", NuiMessageBoxIcon.OK, NuiMessageBoxButtons.OK);
        }

        private void nuiBtnCancel_Click(object sender, EventArgs e)
        {
            NuiMessageBox.Show(this, "你确定要提出程序？", "提示", NuiMessageBoxIcon.Question, NuiMessageBoxButtons.OKCancel);
        }

        private void nuiBtnDelete_Click(object sender, EventArgs e)
        {
            NuiMessageBox.Show(this, "应用程序出现未知异常，导致删除出错", "提示", NuiMessageBoxIcon.Error, NuiMessageBoxButtons.RetryCancel);
        }

        private void nuiBtnSearch_Click(object sender, EventArgs e)
        {
            NuiMessageBox.Show(this, "这里是信息提示文本", "提示", NuiMessageBoxIcon.Information, NuiMessageBoxButtons.OKCancel);
        }

        // 图形按钮--播放按钮--切换
        private void nuiImageBtnPlay_Click(object sender, EventArgs e)
        {
            // 播放
            if (nuiImageBtnPlay.Tag.ToString() == "1")
            {
                nuiImageBtnPlay.NormalImage = Properties.Resources.MiniPlayerBtnPlayTPause_05;
                nuiImageBtnPlay.HoverImage = Properties.Resources.MiniPlayerBtnPlayTPause_06;
                nuiImageBtnPlay.DownImage = Properties.Resources.MiniPlayerBtnPlayTPause_07;
                nuiImageBtnPlay.ToolTipText = "暂停";
                nuiImageBtnPlay.Tag = 2;
                nuiImageBtnPlay.Invalidate();
            }
            else if (nuiImageBtnPlay.Tag.ToString() == "2")
            {
                nuiImageBtnPlay.NormalImage = Properties.Resources.MiniPlayerBtnPlayTPause_01;
                nuiImageBtnPlay.HoverImage = Properties.Resources.MiniPlayerBtnPlayTPause_02;
                nuiImageBtnPlay.DownImage = Properties.Resources.MiniPlayerBtnPlayTPause_03;
                nuiImageBtnPlay.ToolTipText = "播放";
                nuiImageBtnPlay.Tag = 1;
                nuiImageBtnPlay.Invalidate();
            }
        }

        private void nuiImageBtnPlay_MouseDoubleClick(object sender, MouseEventArgs e)
        {
            // 播放
            if (nuiImageBtnPlay.Tag.ToString() == "1")
            {
                nuiImageBtnPlay.NormalImage = Properties.Resources.MiniPlayerBtnPlayTPause_05;
                nuiImageBtnPlay.HoverImage = Properties.Resources.MiniPlayerBtnPlayTPause_06;
                nuiImageBtnPlay.DownImage = Properties.Resources.MiniPlayerBtnPlayTPause_07;
                nuiImageBtnPlay.ToolTipText = "暂停";
                nuiImageBtnPlay.Tag = 2;
                nuiImageBtnPlay.Invalidate();
            }
            else if (nuiImageBtnPlay.Tag.ToString() == "2")
            {
                nuiImageBtnPlay.NormalImage = Properties.Resources.MiniPlayerBtnPlayTPause_01;
                nuiImageBtnPlay.HoverImage = Properties.Resources.MiniPlayerBtnPlayTPause_02;
                nuiImageBtnPlay.DownImage = Properties.Resources.MiniPlayerBtnPlayTPause_03;
                nuiImageBtnPlay.ToolTipText = "播放";
                nuiImageBtnPlay.Tag = 1;
                nuiImageBtnPlay.Invalidate();
            }
        }


        private void nuiTrackBarTest_Scroll(object sender, EventArgs e)
        {
            int rate = nuiTrackBarTest.Value;
            string text = nuiTrackBarTest.Value.ToString() + "%";

            goldProgressBarEx1.Value = rate;
            goldProgressBarEx1.Text = text;

            biggreenprogressBarEx1.Value = rate;
            biggreenprogressBarEx1.Text = text;

            greenprogressBarEx1.Value = rate;
            greenprogressBarEx1.Text = text;

            blueprogressBarEx1.Value = rate;
            blueprogressBarEx1.Text = text;

            dualProgressBar1.MasterValue = rate;
            if (rate < 25) { return; }
            if (rate > 98)
            {
                dualProgressBar1.Value = 100;
            }
            else
            {
                dualProgressBar1.Value = dualProgressBar1.MasterValue - 25;
            }

        }

        // 开始
        private void nuiBtnStart_Click(object sender, EventArgs e)
        {
            nuiBtnStart.Enabled = false;
            nuiBtnPause.Enabled = true;
            nuiBtnStop.Enabled = true;

            // marquee
            marqueeprogressBarEx1.MarqueeSpeed = 10;
            marqueeprogressBarEx1.MarqueePercentage = 25;
            marqueeprogressBarEx1.MarqueeStep = 2;

            marqueeprogressBarEx1.MarqueeStart();
            timerMarquee.Enabled = true;

            // 动画进度条
            animateprogressBarEx1.Value = 100;
            animateprogressBarEx1.StartAnimation();

            // 双进度条
            m_go = true;
            dualProgressBar1.Value = 0;
            dualProgressBar1.MasterValue = 0;
            dualProgressBar1.Maximum = 2000;
            dualProgressBar1.MasterMaximum = 10000;
            for (int i = 0; i < 5; i++)
            {
                if (!m_go) { break; }
                dualProgressBar1.Value = 0;
                for (int j = 0; j < 2000; j++)
                {
                    if (!m_go) { break; }
                    dualProgressBar1.Value = j;
                    dualProgressBar1.MasterValue++;
                    Application.DoEvents();
                }
            }
        }

        // 暂停
        private void nuiBtnPause_Click(object sender, EventArgs e)
        {
            nuiBtnStart.Enabled = true;
            nuiBtnPause.Enabled = false;
            nuiBtnStop.Enabled = true;

            marqueeprogressBarEx1.MarqueePause();
            timerMarquee.Enabled = false;
        }
        
        // 停止
        private void nuiBtnStop_Click(object sender, EventArgs e)
        {
            nuiBtnStart.Enabled = true;
            nuiBtnPause.Enabled = false;
            nuiBtnStop.Enabled = false;

            marqueeprogressBarEx1.MarqueeStop();
            timerMarquee.Enabled = false;
            marqueeprogressBarEx1.MasterValue = 36;

            animateprogressBarEx1.Value = 36;
            animateprogressBarEx1.StopAnimation();
        }

        private void timerMarquee_Tick(object sender, EventArgs e)
        {
            Random r = new Random(DateTime.Now.Millisecond);
            int newval = marqueeprogressBarEx1.MasterValue + r.Next(0, 10);
            if (newval > marqueeprogressBarEx1.MasterMaximum)
            {
                marqueeprogressBarEx1.MasterValue = 0;
            }
            else
            {
                marqueeprogressBarEx1.MasterValue = newval;
            }
        }
    }
}
