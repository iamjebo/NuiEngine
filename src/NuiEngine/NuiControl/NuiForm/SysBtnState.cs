using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace NuiEngine.NuiControl
{
    /// <summary>
    /// 窗体系统按钮控件状态枚举
    /// </summary>
    internal enum SysBtnState
    {
        /// <summary>
        /// 正常
        /// </summary>
        Normal,

        /// <summary>
        /// 高亮
        /// </summary>
        HighLight,

        /// <summary>
        /// 左键按下
        /// </summary>
        Down,

        /// <summary>
        /// 左键按下状态离开控件
        /// </summary>
        DownLeave
    }
}
