using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Windows.Forms;

namespace NuiEngine.NuiControl
{
    /// <summary>
    /// 拥有ToolTip属性的Form基类
    /// </summary>
    public class NuiFormBase:Form
    {
        private ToolTip m_toolTip;

        public NuiFormBase():base()
        {
            m_toolTip = new ToolTip();
        }

        /// <summary>
        /// 获取窗体ToolTip控件
        /// </summary>
        public ToolTip ToolTip
        {
            get { return m_toolTip; }
        }

        protected override void Dispose(bool disposing)
        {
            base.Dispose(disposing);
            if (disposing)
            {
                m_toolTip.Dispose();
            }
            m_toolTip = null;
        }

        private void InitializeComponent()
        {
            this.SuspendLayout();
            // 
            // NuiFromBase
            // 
            this.ClientSize = new System.Drawing.Size(636, 372);
            this.FormBorderStyle = System.Windows.Forms.FormBorderStyle.None;
            this.Name = "NuiFromBase";
            this.ResumeLayout(false);
        }
    }
}
