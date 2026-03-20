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
        private bool isOutline = false;
        private Color outlineColor = Color.Empty;

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

        /// <summary>
        /// When true the button renders as an outline / ghost button
        /// (transparent fill + coloured border) instead of a solid fill.
        /// </summary>
        public bool IsOutline
        {
            get { return isOutline; }
            set { isOutline = value; this.Invalidate(); }
        }

        /// <summary>
        /// Border / text colour used in outline mode.
        /// Falls back to <see cref="NormalColor"/> when not set.
        /// </summary>
        public Color OutlineColor
        {
            get { return outlineColor.IsEmpty ? normalColor : outlineColor; }
            set { outlineColor = value; this.Invalidate(); }
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

            if (isOutline)
            {
                // ── Outline / ghost style ─────────────────────────────
                Color disabledColor = Color.FromArgb(70, 70, 70);
                Color borderColor;
                Color textColor;

                if (!this.Enabled)
                {
                    borderColor = disabledColor;
                    textColor   = disabledColor;
                }
                else if (isPressed)
                {
                    borderColor = pressedColor;
                    textColor   = pressedColor;
                }
                else
                {
                    borderColor = OutlineColor;
                    textColor   = OutlineColor;
                }

                // Inset by 1 px so the pen stroke is not clipped at the edges.
                Rectangle borderRect = Rectangle.Inflate(this.ClientRectangle, -1, -1);

                // Fill with parent background so the button looks transparent.
                Color bgFill = this.Parent?.BackColor ?? Color.FromArgb(30, 30, 30);
                using (GraphicsPath bgPath = GetRoundedRectPath(this.ClientRectangle, cornerRadius))
                using (SolidBrush bgBrush = new SolidBrush(bgFill))
                {
                    g.FillPath(bgBrush, bgPath);
                }

                // Semi-transparent tint on hover / press for subtle interaction feedback.
                const int TintAlphaHovered = 30;
                const int TintAlphaPressed = 60;
                if (this.Enabled && (isHovered || isPressed))
                {
                    int alpha = isPressed ? TintAlphaPressed : TintAlphaHovered;
                    using (GraphicsPath tintPath = GetRoundedRectPath(borderRect, cornerRadius))
                    using (SolidBrush tintBrush = new SolidBrush(Color.FromArgb(alpha, OutlineColor)))
                    {
                        g.FillPath(tintBrush, tintPath);
                    }
                }

                // Draw border.
                using (GraphicsPath borderPath = GetRoundedRectPath(borderRect, cornerRadius))
                using (Pen pen = new Pen(borderColor, 2f))
                {
                    g.DrawPath(pen, borderPath);
                }

                // Draw text.
                TextRenderer.DrawText(g, this.Text, this.Font, this.ClientRectangle,
                    textColor, TextFormatFlags.HorizontalCenter | TextFormatFlags.VerticalCenter);
            }
            else
            {
                // ── Solid fill style (original) ───────────────────────
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

                using (GraphicsPath path = GetRoundedRectPath(this.ClientRectangle, cornerRadius))
                using (SolidBrush brush = new SolidBrush(bgColor))
                {
                    g.FillPath(brush, path);
                }

                TextRenderer.DrawText(g, this.Text, this.Font, this.ClientRectangle,
                    this.ForeColor, TextFormatFlags.HorizontalCenter | TextFormatFlags.VerticalCenter);
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
}
