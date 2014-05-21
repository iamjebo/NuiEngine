using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Windows.Forms;
using System.Drawing;
using System.Drawing.Drawing2D;
using System.ComponentModel;

namespace EngineDemo
{
    /// <summary>
    /// 分割直线控件
    /// </summary>
    public class SpiltLineText : Panel
    {
        private Image m_lineImg = EngineDemo.Properties.Resources.split_line;
        private string m_text;

        public SpiltLineText()
        {
            this.BackColor = Color.Transparent;
            this.Size = new Size(100, 28);
        }

        [Browsable(true)]
        public override string Text
        {
            get
            {
                return m_text;
            }
            set
            {
                m_text = value;
            }
        }

        protected override void OnPaint(PaintEventArgs e)
        {
            base.OnPaint(e);
            Graphics g = e.Graphics;
            g.SmoothingMode = SmoothingMode.AntiAlias;
            g.InterpolationMode = InterpolationMode.HighQualityBilinear;
            g.PageUnit = GraphicsUnit.Pixel;

            if (!string.IsNullOrEmpty(Text))
            {
                // 计算文本矩形与直线图片矩形
                StringFormat format = new StringFormat();
                format.FormatFlags = StringFormatFlags.MeasureTrailingSpaces;
                SizeF textSize = g.MeasureString(this.Text, this.Font, 500, format);
                int offset = 3;
                Rectangle textRect = new Rectangle(
                                        offset,
                                        offset,
                                        (int)textSize.Width + offset,
                                        (int)((int)textSize.Height + offset));
                Rectangle imageRect = new Rectangle(
                                        textRect.Right,
                                        (textRect.Top + textRect.Height / 2 - m_lineImg.Height/2),
                                        ClientSize.Width - (offset + textRect.Width),
                                        m_lineImg.Height);

                // 绘制文本与图片
                TextRenderer.DrawText(
                      g,
                      this.Text,
                      this.Font,
                      textRect,
                      this.ForeColor);
                e.Graphics.DrawImage(m_lineImg, imageRect, new Rectangle(m_lineImg.Width / 2, 0, m_lineImg.Width / 2, m_lineImg.Height), GraphicsUnit.Pixel);
            }
        }
    }
}
