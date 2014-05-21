using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Drawing;

using NuiEngine.Utility;
using System.Windows.Forms;

namespace NuiEngine.NuiControl
{
    /// <summary>
    /// 窗体三个系统按钮管理器
    /// </summary>
    internal class SysBtnManager:IDisposable
    {

        private NuiFormBase m_owner;
        private bool m_isMouseDown;
        private SysBtn[] m_sysBtns = new SysBtn[3];   // 0.Close 1.Max 2 Min
       
        /// <summary>
        /// 系统按钮数组
        /// </summary>
        public SysBtn[] SysButtonArray
        {
            get
            {
                return m_sysBtns;
            }
        }

        /// <summary>
        /// 按钮状态索引器
        /// </summary>
        public SysBtnState this[int index]
        {
            get
            {
                return SysButtonArray[index].State;
            }
            set
            {
                if (SysButtonArray[index].State != value)
                {
                    SysButtonArray[index].State = value;
                    if (m_owner != null)
                    {
                        Invalidate(SysButtonArray[index].LocationRect);
                    }
                }
            }
        }

        public SysBtnManager(NuiFormBase owner)
        {
            this.m_owner = owner;
            IniSystemButtons();
        }

        public void ProcessMouseOperate(Point mousePoint, NuiMouseOperate operate)
        {
            switch (operate)
            {
                case NuiMouseOperate.Move:
                    ProcessMouseMove(mousePoint);
                    break;
                case NuiMouseOperate.LeftDown:
                    ProcessMouseDown(mousePoint);
                    break;
                case NuiMouseOperate.LeftUp:
                    ProcessMouseUP(mousePoint);
                    break;
                case NuiMouseOperate.Leave:
                    ProcessMouseLeave();
                    break;
                default:
                    break;
            }
        }

        public void DrawSystemButtons(Graphics g)
        {
            for (int index = 0; index < SysButtonArray.Length; index++)
            {
                //当窗体没有此系统按钮时，不进行绘制
                if (SysButtonArray[index].LocationRect == Rectangle.Empty)
                {
                    continue;
                }

                switch (this[index])
                {
                    case SysBtnState.DownLeave:
                    case SysBtnState.Normal:
                        g.DrawImage(
                            SysButtonArray[index].NormalImg,
                            SysButtonArray[index].LocationRect,
                            new Rectangle(0, 0, SysButtonArray[index].NormalImg.Width, SysButtonArray[index].NormalImg.Height),
                            GraphicsUnit.Pixel);
                        break;
                    case SysBtnState.HighLight:
                        g.DrawImage(
                            SysButtonArray[index].HighLightImg,
                            SysButtonArray[index].LocationRect,
                            new Rectangle(0, 0, SysButtonArray[index].HighLightImg.Width, SysButtonArray[index].HighLightImg.Height),
                            GraphicsUnit.Pixel);
                        break;
                    case SysBtnState.Down:
                        g.DrawImage(
                            SysButtonArray[index].DownImg,
                            SysButtonArray[index].LocationRect,
                            new Rectangle(0, 0, SysButtonArray[index].DownImg.Width, SysButtonArray[index].DownImg.Height),
                            GraphicsUnit.Pixel);
                        break;
                    default:
                        break;
                }
            }
        }

        private void ProcessMouseMove(Point mousePoint)
        {
            string toolTip = string.Empty;
            bool hide = true;

            int index = SearchPointInRects(mousePoint);
            if (index != -1)
            {
                hide = false;  //显示提示文本
                if (!m_isMouseDown)
                {
                    if (this[index] != SysBtnState.HighLight)
                    {
                        toolTip = SysButtonArray[index].ToolTip;
                    }
                    this[index] = SysBtnState.HighLight;
                }
                else
                {
                    if (this[index] == SysBtnState.DownLeave)
                    {
                        this[index] = SysBtnState.Down;
                    }
                }

                //其他按钮的状态为 Normal
                for (int i = 0; i < SysButtonArray.Length; i++)
                {
                    if (i != index)
                    {
                        this[i] = SysBtnState.Normal;
                    }
                }
            }
            else
            {
                for (int i = 0; i < SysButtonArray.Length; i++)
                {
                    if (!m_isMouseDown)
                    {
                        this[i] = SysBtnState.Normal;
                    }
                    else
                    {
                        if (this[i] == SysBtnState.Down)
                        {
                            this[i] = SysBtnState.DownLeave;
                        }
                    }
                }
            }

            if (toolTip != string.Empty)
            {
                HideToolTip();
                ShowTooTip(toolTip);
            }

            if (hide)
            {
                HideToolTip();
            }

        }

        private void ProcessMouseDown(Point mousePoint)
        {

            int index = SearchPointInRects(mousePoint);
            if (index != -1)
            {
                m_isMouseDown = true;
                this[index] = SysBtnState.Down;
            }
            else
            {
                MethodHelper.MoveWindow(m_owner);
            }
        }

        private void ProcessMouseUP(Point mousePoint)
        {
            m_isMouseDown = false;
            int index = SearchPointInRects(mousePoint);
            if (index != -1)
            {
                if (this[index] == SysBtnState.Down)
                {
                    this[index] = SysBtnState.Normal;

                    //handle event at there
                    SysButtonArray[index].OnMouseDown();
                }
            }
            else
            {
                ProcessMouseLeave();
            }
        }

        private void ProcessMouseLeave()
        {
            for (int i = 0; i < SysButtonArray.Length; i++)
            {
                if (this[i] == SysBtnState.Down)
                {
                    this[i] = SysBtnState.DownLeave;
                }
                else
                { //所有按钮的状态为 Normal
                    this[i] = SysBtnState.Normal;
                }
            }
        }

        private void Invalidate(Rectangle rect)
        {
            m_owner.Invalidate(rect);
        }

        private void ShowTooTip(string toolTipText)
        {
            if (m_owner != null)
            {
                m_owner.ToolTip.Active = true;
                m_owner.ToolTip.SetToolTip(m_owner, toolTipText);
            }
        }

        private void HideToolTip()
        {
            if (m_owner != null)
            {
                m_owner.ToolTip.Active = false;
            }
        }

        /// <summary>
        /// 判断鼠标点是否在系统按钮矩形内
        /// </summary>
        /// <param name="mousePoint">鼠标坐标点</param>
        /// <returns>如果存在，返回系统按钮索引，否则返回 -1</returns>
        private int SearchPointInRects(Point mousePoint)
        {
            bool isFind = false;
            int index = 0;
            foreach (SysBtn button in SysButtonArray)
            {
                if (button.LocationRect != Rectangle.Empty && button.LocationRect.Contains(mousePoint))
                {
                    isFind = true;
                    break;
                }
                index++;
            }
            if (isFind)
            {
                return index;
            }
            else
            {
                return -1;
            }
        }

        private void IniSystemButtons()
        {
            bool isShowMaxButton = m_owner.MaximizeBox;
            bool isShowMinButton = m_owner.MinimizeBox;

            //Colse
            SysBtn closeBtn = new SysBtn();
            SysButtonArray[0] = closeBtn;
            closeBtn.ToolTip = "关闭";
            closeBtn.NormalImg = Properties.Resources.sys_close_normal;
            closeBtn.HighLightImg = Properties.Resources.sys_close_highlight;
            closeBtn.DownImg = Properties.Resources.sys_close_down;
            closeBtn.LocationRect = new Rectangle(
                m_owner.Width - closeBtn.NormalImg.Width,
                -1,
                closeBtn.NormalImg.Width,
                closeBtn.NormalImg.Height);
            //注册事件
            closeBtn.OnMouseDownEvent += new MouseDownEventHandler(this.CloseButtonEvent);


            //Max
            SysBtn MaxBtn = new SysBtn();
            SysButtonArray[1] = MaxBtn;
            MaxBtn.ToolTip = "最大化";
            if (isShowMaxButton)
            {
                MaxBtn.NormalImg = Properties.Resources.sys_max_normal;
                MaxBtn.HighLightImg = Properties.Resources.sys_max_highlight;
                MaxBtn.DownImg = Properties.Resources.sys_max_down;
                MaxBtn.OnMouseDownEvent += new MouseDownEventHandler(this.MaxButtonEvent);
                MaxBtn.LocationRect = new Rectangle(
                    closeBtn.LocationRect.X - MaxBtn.NormalImg.Width,
                    -1,
                    MaxBtn.NormalImg.Width,
                    MaxBtn.NormalImg.Height);
            }
            else
            {
                MaxBtn.LocationRect = Rectangle.Empty;
            }

            //Min
            SysBtn minBtn = new SysBtn();
            SysButtonArray[2] = minBtn;
            minBtn.ToolTip = "最小化";
            if (!isShowMinButton)
            {
                minBtn.LocationRect = Rectangle.Empty;
                return;
            }
            minBtn.NormalImg = Properties.Resources.sys_min_normal;
            minBtn.HighLightImg = Properties.Resources.sys_min_highlight;
            minBtn.DownImg = Properties.Resources.sys_min_down;
            minBtn.OnMouseDownEvent += new MouseDownEventHandler(this.MinButtonEvent);
            if (isShowMaxButton)
            {
                minBtn.LocationRect = new Rectangle(
                    MaxBtn.LocationRect.X - minBtn.NormalImg.Width,
                    -1,
                    minBtn.NormalImg.Width,
                    minBtn.NormalImg.Height);
            }
            else
            {
                minBtn.LocationRect = new Rectangle(
                   closeBtn.LocationRect.X - minBtn.NormalImg.Width,
                   -1,
                   minBtn.NormalImg.Width,
                   minBtn.NormalImg.Height);
            }
        }

        private void CloseButtonEvent()
        {
            m_owner.Close();
        }

        private void MaxButtonEvent()
        {
            bool isMax = m_owner.WindowState == FormWindowState.Maximized;
            if (isMax)
            {
                m_owner.WindowState = FormWindowState.Normal;
            }
            else
            {
                m_owner.WindowState = FormWindowState.Maximized;
            }
        }

        private void MinButtonEvent()
        {
            m_owner.WindowState = FormWindowState.Minimized;
        }

        public void Dispose()
        {
            m_owner = null;
        }
    }
}
