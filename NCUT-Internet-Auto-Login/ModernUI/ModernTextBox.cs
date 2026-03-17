using System;
using System.Drawing;
using System.Drawing.Drawing2D;
using System.Windows.Forms;

namespace NCUT_Internet_Auto_Login.ModernUI
{
    /// <summary>
    /// 現代化風格的文字框控件
    /// </summary>
    public class ModernTextBox : Control
    {
        private TextBox textBox;
        private Label placeholderLabel;
        private string placeholderText = "";
        private Color borderColor = Color.FromArgb(200, 200, 200);
        private Color focusBorderColor = Color.FromArgb(0, 122, 204);
        private bool isFocused = false;
        private int cornerRadius = 6;

        public ModernTextBox()
        {
            this.Size = new Size(200, 35);
            this.BackColor = Color.White;
            this.Font = new Font("Segoe UI", 9F);
            
            // 創建內部 TextBox
            textBox = new TextBox
            {
                BorderStyle = BorderStyle.None,
                Font = this.Font,
                BackColor = this.BackColor,
                ForeColor = Color.FromArgb(220, 220, 220), // 預設深色模式文字
                Location = new Point(10, 8),
                Anchor = AnchorStyles.Top | AnchorStyles.Bottom | AnchorStyles.Left | AnchorStyles.Right
            };
            
            textBox.Width = this.Width - 20;
            textBox.GotFocus += TextBox_GotFocus;
            textBox.LostFocus += TextBox_LostFocus;
            textBox.TextChanged += TextBox_TextChanged;
            
            // 創建 Placeholder Label
            placeholderLabel = new Label
            {
                Text = placeholderText,
                Font = this.Font,
                ForeColor = Color.Gray,
                BackColor = Color.Transparent,
                Location = new Point(10, 8),
                AutoSize = true,
                Cursor = Cursors.IBeam
            };
            placeholderLabel.Click += (s, e) => textBox.Focus();
            
            this.Controls.Add(textBox);
            this.Controls.Add(placeholderLabel);
            placeholderLabel.BringToFront();
            
            this.SetStyle(ControlStyles.UserPaint | 
                         ControlStyles.AllPaintingInWmPaint | 
                         ControlStyles.OptimizedDoubleBuffer, true);
        }

        public override Color BackColor
        {
            get { return base.BackColor; }
            set
            {
                base.BackColor = value;
                if (textBox != null) textBox.BackColor = value;
                this.Invalidate();
            }
        }

        public override Color ForeColor
        {
            get { return base.ForeColor; }
            set
            {
                base.ForeColor = value;
                if (textBox != null) textBox.ForeColor = value;
                this.Invalidate();
            }
        }

        public string PlaceholderText
        {
            get { return placeholderText; }
            set 
            { 
                placeholderText = value;
                placeholderLabel.Text = value;
                UpdatePlaceholder();
            }
        }

        public new string Text
        {
            get { return textBox.Text; }
            set { textBox.Text = value; }
        }

        public bool UseSystemPasswordChar
        {
            get { return textBox.UseSystemPasswordChar; }
            set { textBox.UseSystemPasswordChar = value; }
        }

        public Color BorderColor
        {
            get { return borderColor; }
            set { borderColor = value; this.Invalidate(); }
        }

        public Color FocusBorderColor
        {
            get { return focusBorderColor; }
            set { focusBorderColor = value; }
        }

        public int CornerRadius
        {
            get { return cornerRadius; }
            set { cornerRadius = value; this.Invalidate(); }
        }

        private void TextBox_GotFocus(object sender, EventArgs e)
        {
            isFocused = true;
            this.Invalidate();
        }

        private void TextBox_LostFocus(object sender, EventArgs e)
        {
            isFocused = false;
            this.Invalidate();
        }

        private void TextBox_TextChanged(object sender, EventArgs e)
        {
            UpdatePlaceholder();
        }

        private void UpdatePlaceholder()
        {
            placeholderLabel.Visible = string.IsNullOrEmpty(textBox.Text);
        }

        protected override void OnResize(EventArgs e)
        {
            base.OnResize(e);
            if (textBox != null)
            {
                textBox.Width = this.Width - 20;
            }
        }

        protected override void OnPaint(PaintEventArgs e)
        {
            base.OnPaint(e);
            
            Graphics g = e.Graphics;
            g.SmoothingMode = SmoothingMode.AntiAlias;

            // 選擇邊框顏色
            Color currentBorderColor = isFocused ? focusBorderColor : borderColor;
            
            // 繪製背景
            using (GraphicsPath path = GetRoundedRectPath(this.ClientRectangle, cornerRadius))
            {
                using (SolidBrush brush = new SolidBrush(this.BackColor))
                {
                    g.FillPath(brush, path);
                }
                
                // 繪製邊框
                using (Pen pen = new Pen(currentBorderColor, isFocused ? 2 : 1))
                {
                    g.DrawPath(pen, path);
                }
            }
        }

        private GraphicsPath GetRoundedRectPath(Rectangle rect, int radius)
        {
            GraphicsPath path = new GraphicsPath();
            int diameter = radius * 2;
            
            // 調整矩形以避免邊框被裁切
            rect.Inflate(-1, -1);
            
            path.AddArc(rect.X, rect.Y, diameter, diameter, 180, 90);
            path.AddArc(rect.Right - diameter, rect.Y, diameter, diameter, 270, 90);
            path.AddArc(rect.Right - diameter, rect.Bottom - diameter, diameter, diameter, 0, 90);
            path.AddArc(rect.X, rect.Bottom - diameter, diameter, diameter, 90, 90);
            path.CloseFigure();
            
            return path;
        }
    }
}
