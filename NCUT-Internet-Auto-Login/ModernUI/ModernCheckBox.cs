using System;
using System.Drawing;
using System.Windows.Forms;
using System.Runtime.InteropServices;

namespace NCUT_Internet_Auto_Login.ModernUI
{
    /// <summary>
    /// �{�N�ƭ��檺 CheckBox
    /// </summary>
    public class ModernCheckBox : CheckBox
    {
        private Color checkedColor = Color.FromArgb(0, 122, 204);
        private Color uncheckedColor = Color.FromArgb(200, 200, 200);

        public ModernCheckBox()
        {
            this.FlatStyle = FlatStyle.Flat;
            this.UseVisualStyleBackColor = false;
            this.FlatAppearance.BorderSize = 0;
            this.Font = new Font("Segoe UI", 9F);
            this.ForeColor = Color.FromArgb(220, 220, 220);
            this.AutoSize = true;
            
            this.SetStyle(ControlStyles.UserPaint | 
                         ControlStyles.AllPaintingInWmPaint | 
                         ControlStyles.OptimizedDoubleBuffer, true);
        }

        public Color CheckedColor
        {
            get { return checkedColor; }
            set { checkedColor = value; this.Invalidate(); }
        }

        public Color UncheckedColor
        {
            get { return uncheckedColor; }
            set { uncheckedColor = value; this.Invalidate(); }
        }

        protected override void OnPaint(PaintEventArgs pevent)
        {
            Graphics g = pevent.Graphics;
            g.SmoothingMode = System.Drawing.Drawing2D.SmoothingMode.AntiAlias;

            if (this.Parent != null)
                g.Clear(this.Parent.BackColor);
            else
                g.Clear(this.BackColor);

            // ø�s�֨����
            Rectangle checkBoxRect = new Rectangle(0, (this.Height - 18) / 2, 18, 18);
            
            Color boxColor = this.Checked ? checkedColor : uncheckedColor;
            
            using (SolidBrush brush = new SolidBrush(boxColor))
            {
                g.FillRoundedRectangle(brush, checkBoxRect, 4);
            }

            // �p�G�襤�Aø�s�Ŀ�аO
            if (this.Checked)
            {
                using (Pen pen = new Pen(Color.White, 2))
                {
                    g.DrawLines(pen, new Point[]
                    {
                        new Point(checkBoxRect.X + 4, checkBoxRect.Y + 9),
                        new Point(checkBoxRect.X + 7, checkBoxRect.Y + 12),
                        new Point(checkBoxRect.X + 14, checkBoxRect.Y + 5)
                    });
                }
            }

            // ø�s��r
            Rectangle textRect = new Rectangle(
                checkBoxRect.Right + 8,
                0,
                this.Width - checkBoxRect.Right - 8,
                this.Height
            );

            TextRenderer.DrawText(g, this.Text, this.Font, textRect,
                this.Enabled ? this.ForeColor : Color.Gray,
                TextFormatFlags.Left | TextFormatFlags.VerticalCenter);
        }

        protected override void OnMouseEnter(EventArgs eventargs)
        {
            base.OnMouseEnter(eventargs);
            this.Cursor = Cursors.Hand;
        }
    }

    /// <summary>
    /// Graphics �X�i��k
    /// </summary>
    public static class GraphicsExtensions
    {
        public static void FillRoundedRectangle(this Graphics g, Brush brush, Rectangle rect, int radius)
        {
            using (System.Drawing.Drawing2D.GraphicsPath path = GetRoundedRectPath(rect, radius))
            {
                g.FillPath(brush, path);
            }
        }

        public static void DrawRoundedRectangle(this Graphics g, Pen pen, Rectangle rect, int radius)
        {
            using (System.Drawing.Drawing2D.GraphicsPath path = GetRoundedRectPath(rect, radius))
            {
                g.DrawPath(pen, path);
            }
        }

        private static System.Drawing.Drawing2D.GraphicsPath GetRoundedRectPath(Rectangle rect, int radius)
        {
            System.Drawing.Drawing2D.GraphicsPath path = new System.Drawing.Drawing2D.GraphicsPath();
            int diameter = radius * 2;

            path.AddArc(rect.X, rect.Y, diameter, diameter, 180, 90);
            path.AddArc(rect.Right - diameter, rect.Y, diameter, diameter, 270, 90);
            path.AddArc(rect.Right - diameter, rect.Bottom - diameter, diameter, diameter, 0, 90);
            path.AddArc(rect.X, rect.Bottom - diameter, diameter, diameter, 90, 90);
            path.CloseFigure();

            return path;
        }
    }
}
