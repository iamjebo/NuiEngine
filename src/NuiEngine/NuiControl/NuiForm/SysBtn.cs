using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Drawing;

namespace NuiEngine.NuiControl
{
    //鼠标左键按钮事件委托
    public delegate void MouseDownEventHandler();

    /// <summary>
    /// 窗体三个系统按钮
    /// </summary>
    internal class SysBtn
    {
        /// <summary>
        /// 按钮当前状态
        /// </summary>
        public SysBtnState State { get; set; }

        /// <summary>
        /// 按钮坐标矩形
        /// </summary>
        public Rectangle LocationRect { get; set; }

        /// <summary>
        /// 按钮正常状态图片
        /// </summary>
        public Image NormalImg { get; set; }

        /// <summary>
        /// 按钮高亮状态图片
        /// </summary>
        public Image HighLightImg { get; set; }

        /// <summary>
        /// 按钮按下状态图片
        /// </summary>
        public Image DownImg { get; set; }

        /// <summary>
        /// 按钮提示文本
        /// </summary>
        public string ToolTip { get; set; }

        /// <summary>
        /// 鼠标左键按下事件
        /// </summary>
        public event MouseDownEventHandler OnMouseDownEvent;

        /// <summary>
        /// 响应按钮左键按下事件
        /// </summary>
        public void OnMouseDown()
        {
            if (OnMouseDownEvent != null)
            {
                OnMouseDownEvent();
            }
        }
    }
}
