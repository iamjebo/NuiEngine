using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Drawing;
using NuiEngine.Utility;

namespace NuiEngine.NuiControl
{
    /// <summary>
    /// 实现仿QQ效果控件内部使用颜色表配置
    /// </summary>
    internal class NuiConfig
    {
        /// <summary>
        /// QQ按钮边框颜色
        /// </summary>
        public static Color QQBorderColor = Color.LightBlue;  //LightBlue = Color.FromArgb(173, 216, 230)

        /// <summary>
        /// QQ按钮高亮颜色
        /// </summary>
        public static Color QQHighLightColor = MethodHelper.GetColor(QQBorderColor, 255, -63, -11, 23);   //Color.FromArgb(110, 205, 253)
        
        /// <summary>
        /// QQ按钮内部高亮颜色
        /// </summary>
        public static Color QQHighLightInnerColor = MethodHelper.GetColor(QQBorderColor, 255, -100, -44, 1);   //Color.FromArgb(73, 172, 231);
    }
}
