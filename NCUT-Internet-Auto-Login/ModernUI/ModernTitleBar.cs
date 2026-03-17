using System;
using System.Drawing;
using System.Drawing.Drawing2D;
using System.Windows.Forms;
using System.Runtime.InteropServices;

namespace NCUT_Internet_Auto_Login.ModernUI
{
    /// <summary>
    /// 現代化風格的自訂標題欄
    /// </summary>
    public class ModernTitleBar : Panel
    {
        private Label lblTitle;
        private Button btnClose;
        private Button btnMinimize;
        private Button btnMaximize;
        private Form parentForm;

        [DllImport("user32.dll")]
        private static extern bool ReleaseCapture();

        [DllImport("user32.dll")]
        private static extern int SendMessage(IntPtr hWnd, int Msg, int wParam, int lParam);

        private const int WM_NCLBUTTONDOWN = 0xA1;
        private const int HT_CAPTION = 0x2;

        public ModernTitleBar()
        {
            this.Height = 40;
            this.BackColor = Color.FromArgb(45, 45, 48);
            this.Dock = DockStyle.Top;

            InitializeComponents();
        }

        private void InitializeComponents()
        {
            // 標題文字
            lblTitle = new Label
            {
                Text = "NCUT Internet Auto Login V2",
                Font = new Font("Segoe UI", 10F, FontStyle.Regular),
                ForeColor = Color.White,
                Location = new Point(15, 10),
                AutoSize = true
            };
            lblTitle.MouseDown += TitleBar_MouseDown;

            // 關閉按鈕
            btnClose = CreateTitleBarButton("?", Color.FromArgb(232, 17, 35));
            btnClose.Click += (s, e) => 
            {
                if (parentForm != null)
                {
                    parentForm.Close();
                }
            };

            // 最大化按鈕
            btnMaximize = CreateTitleBarButton("□", Color.FromArgb(60, 60, 63));
            btnMaximize.Click += (s, e) => 
            {
                if (parentForm != null)
                {
                    if (parentForm.WindowState == FormWindowState.Maximized)
                    {
                        parentForm.WindowState = FormWindowState.Normal;
                        btnMaximize.Text = "□";
                    }
                    else
                    {
                        parentForm.WindowState = FormWindowState.Maximized;
                        btnMaximize.Text = "?";
                    }
                }
            };

            // 最小化按鈕
            btnMinimize = CreateTitleBarButton("─", Color.FromArgb(60, 60, 63));
            btnMinimize.Click += (s, e) => 
            {
                if (parentForm != null)
                {
                    parentForm.WindowState = FormWindowState.Minimized;
                }
            };

            this.Controls.Add(lblTitle);
            this.Controls.Add(btnClose);
            this.Controls.Add(btnMaximize);
            this.Controls.Add(btnMinimize);

            this.MouseDown += TitleBar_MouseDown;
        }

        private Button CreateTitleBarButton(string text, Color hoverColor)
        {
            Button btn = new Button
            {
                Text = text,
                Font = new Font("Segoe UI", 10F),
                ForeColor = Color.White,
                BackColor = Color.Transparent,
                FlatStyle = FlatStyle.Flat,
                Size = new Size(46, 40),
                Cursor = Cursors.Hand,
                TabStop = false
            };

            btn.FlatAppearance.BorderSize = 0;
            btn.FlatAppearance.MouseOverBackColor = hoverColor;
            btn.FlatAppearance.MouseDownBackColor = Color.FromArgb(
                Math.Max(0, hoverColor.R - 20),
                Math.Max(0, hoverColor.G - 20),
                Math.Max(0, hoverColor.B - 20)
            );

            return btn;
        }

        public void SetParentForm(Form form)
        {
            parentForm = form;
            PositionButtons();
        }

        private void PositionButtons()
        {
            if (parentForm == null) return;

            btnClose.Location = new Point(this.Width - 46, 0);
            btnMaximize.Location = new Point(this.Width - 92, 0);
            btnMinimize.Location = new Point(this.Width - 138, 0);

            btnClose.Anchor = AnchorStyles.Top | AnchorStyles.Right;
            btnMaximize.Anchor = AnchorStyles.Top | AnchorStyles.Right;
            btnMinimize.Anchor = AnchorStyles.Top | AnchorStyles.Right;
        }

        protected override void OnResize(EventArgs eventargs)
        {
            base.OnResize(eventargs);
            PositionButtons();
        }

        private void TitleBar_MouseDown(object sender, MouseEventArgs e)
        {
            if (e.Button == MouseButtons.Left && parentForm != null)
            {
                ReleaseCapture();
                SendMessage(parentForm.Handle, WM_NCLBUTTONDOWN, HT_CAPTION, 0);
            }
        }

        public string Title
        {
            get { return lblTitle.Text; }
            set { lblTitle.Text = value; }
        }
    }
}
