using System;
using System.Drawing;
using System.Drawing.Drawing2D;
using System.Windows.Forms;
using System.Runtime.InteropServices;

namespace NCUT_Internet_Auto_Login.ModernUI
{
    /// <summary>
    /// 現代化風格的 Panel，帶陰影和圓角
    /// </summary>
    public class ModernPanel : Panel
    {
        private int cornerRadius = 10;
        private Color shadowColor = Color.FromArgb(50, 0, 0, 0);
        private int shadowSize = 5;
        private bool drawShadow = true;

        public ModernPanel()
        {
            this.BackColor = Color.White;
            this.SetStyle(ControlStyles.UserPaint | 
                         ControlStyles.AllPaintingInWmPaint | 
                         ControlStyles.OptimizedDoubleBuffer, true);
        }

        public int CornerRadius
        {
            get { return cornerRadius; }
            set { cornerRadius = value; this.Invalidate(); }
        }

        public bool DrawShadow
        {
            get { return drawShadow; }
            set { drawShadow = value; this.Invalidate(); }
        }

        protected override void OnPaint(PaintEventArgs e)
        {
            Graphics g = e.Graphics;
            g.SmoothingMode = SmoothingMode.AntiAlias;

            Rectangle rect = this.ClientRectangle;
            
            // 如果需要繪製陰影，調整矩形大小
            if (drawShadow)
            {
                rect.Inflate(-shadowSize, -shadowSize);
                DrawShadowEffect(g, rect);
            }

            // 繪製圓角矩形背景
            using (GraphicsPath path = GetRoundedRectPath(rect, cornerRadius))
            {
                using (SolidBrush brush = new SolidBrush(this.BackColor))
                {
                    g.FillPath(brush, path);
                }
            }
        }

        private void DrawShadowEffect(Graphics g, Rectangle rect)
        {
            for (int i = shadowSize; i > 0; i--)
            {
                int alpha = (int)(40 * ((float)i / shadowSize));
                Color shadowColor = Color.FromArgb(alpha, 0, 0, 0);
                
                Rectangle shadowRect = rect;
                shadowRect.Inflate(i, i);
                
                using (GraphicsPath path = GetRoundedRectPath(shadowRect, cornerRadius + i))
                {
                    using (Pen pen = new Pen(shadowColor, 1))
                    {
                        g.DrawPath(pen, path);
                    }
                }
            }
        }

        private GraphicsPath GetRoundedRectPath(Rectangle rect, int radius)
        {
            GraphicsPath path = new GraphicsPath();
            int diameter = radius * 2;
            
            path.AddArc(rect.X, rect.Y, diameter, diameter, 180, 90);
            path.AddArc(rect.Right - diameter, rect.Y, diameter, diameter, 270, 90);
            path.AddArc(rect.Right - diameter, rect.Bottom - diameter, diameter, diameter, 0, 90);
            path.AddArc(rect.X, rect.Bottom - diameter, diameter, diameter, 90, 90);
            path.CloseFigure();
            
            return path;
        }
    }

    /// <summary>
    /// 現代化風格的卡片 Panel
    /// </summary>
    public class ModernCard : ModernPanel
    {
        public ModernCard()
        {
            this.CornerRadius = 12;
            this.DrawShadow = true;
            this.BackColor = Color.White;
            this.Padding = new Padding(20);
        }
    }
}
