using System;
using System.Drawing;
using System.Drawing.Drawing2D;
using System.Windows.Forms;

namespace NCUT_Internet_Auto_Login.ModernUI
{
    /// <summary>
    /// �{�N�ƭ��檺���s����
    /// </summary>
    public class ModernButton : Button
    {
        private Color hoverColor;
        private Color normalColor;
        private Color pressedColor;
        private bool isHovered = false;
        private bool isPressed = false;
        private int cornerRadius = 8;

        public ModernButton()
        {
            // �]�m�w�]�˦�
            this.FlatStyle = FlatStyle.Flat;
            this.FlatAppearance.BorderSize = 0;
            this.BackColor = Color.FromArgb(0, 122, 204);
            this.ForeColor = Color.White;
            this.Font = new Font("Segoe UI", 9F, FontStyle.Regular);
            this.Cursor = Cursors.Hand;
            this.Size = new Size(100, 35);
            
            normalColor = Color.FromArgb(0, 122, 204);
            hoverColor = Color.FromArgb(0, 102, 184);
            pressedColor = Color.FromArgb(0, 92, 164);
            
            // �]�m���w�ĥH��ְ{�{
            this.SetStyle(ControlStyles.UserPaint | 
                         ControlStyles.AllPaintingInWmPaint | 
                         ControlStyles.OptimizedDoubleBuffer, true);
        }

        public int CornerRadius
        {
            get { return cornerRadius; }
            set { cornerRadius = value; this.Invalidate(); }
        }

        public Color NormalColor
        {
            get { return normalColor; }
            set { normalColor = value; this.Invalidate(); }
        }

        public Color HoverColor
        {
            get { return hoverColor; }
            set { hoverColor = value; }
        }

        public Color PressedColor
        {
            get { return pressedColor; }
            set { pressedColor = value; }
        }

        protected override void OnMouseEnter(EventArgs e)
        {
            base.OnMouseEnter(e);
            isHovered = true;
            this.Invalidate();
        }

        protected override void OnMouseLeave(EventArgs e)
        {
            base.OnMouseLeave(e);
            isHovered = false;
            this.Invalidate();
        }

        protected override void OnMouseDown(MouseEventArgs mevent)
        {
            base.OnMouseDown(mevent);
            isPressed = true;
            this.Invalidate();
        }

        protected override void OnMouseUp(MouseEventArgs mevent)
        {
            base.OnMouseUp(mevent);
            isPressed = false;
            this.Invalidate();
        }

        protected override void OnPaint(PaintEventArgs pevent)
        {
            Graphics g = pevent.Graphics;
            g.SmoothingMode = SmoothingMode.AntiAlias;

            // ��ܭI���C��
            Color bgColor = normalColor;
            if (!this.Enabled)
            {
                bgColor = Color.FromArgb(200, 200, 200);
            }
            else if (isPressed)
            {
                bgColor = pressedColor;
            }
            else if (isHovered)
            {
                bgColor = hoverColor;
            }

            // ø�s�ꨤ�x�έI��
            using (GraphicsPath path = GetRoundedRectPath(this.ClientRectangle, cornerRadius))
            {
                using (SolidBrush brush = new SolidBrush(bgColor))
                {
                    g.FillPath(brush, path);
                }
            }

            // ø�s��r
            TextRenderer.DrawText(g, this.Text, this.Font, this.ClientRectangle, 
                this.ForeColor, TextFormatFlags.HorizontalCenter | TextFormatFlags.VerticalCenter);
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
}
