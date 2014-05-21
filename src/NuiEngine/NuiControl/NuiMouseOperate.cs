using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace NuiEngine.NuiControl
{
    /// <summary>
    /// 鼠标操作状态枚举
    /// </summary>
    internal enum NuiMouseOperate
    {
        /// <summary>
        /// 鼠标移动
        /// </summary>
        Move,

        /// <summary>
        /// 左键按下
        /// </summary>
        LeftDown,

        /// <summary>
        /// 左键弹起
        /// </summary>
        LeftUp,

        /// <summary>
        /// 鼠标离开控件可见区域
        /// </summary>
        Leave
    }
}
