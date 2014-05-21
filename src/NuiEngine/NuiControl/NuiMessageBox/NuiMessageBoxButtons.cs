using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace NuiEngine.NuiControl
{
    /// <summary>
    /// 弹出对话框按钮枚举
    /// </summary>
    public enum NuiMessageBoxButtons
    {
        /// <summary>
        /// 消息框包含“确定”按钮
        /// </summary>
        OK,
        /// <summary>
        /// 消息框包含“确定”与“取消”按钮
        /// </summary>
        OKCancel,
        /// <summary>
        /// 消息框包含“重试”与“取消”按钮
        /// </summary>
        RetryCancel
    }
}
