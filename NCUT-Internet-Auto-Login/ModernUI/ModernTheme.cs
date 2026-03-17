using System;
using System.Drawing;

namespace NCUT_Internet_Auto_Login.ModernUI
{
    /// <summary>
    /// 現代化 UI 主題管理器
    /// </summary>
    public static class ModernTheme
    {
        // 主要顏色
        public static class Primary
        {
            public static Color Main = Color.FromArgb(0, 122, 204);
            public static Color Light = Color.FromArgb(41, 151, 225);
            public static Color Dark = Color.FromArgb(0, 102, 184);
        }

        // 次要顏色
        public static class Secondary
        {
            public static Color Main = Color.FromArgb(92, 184, 92);
            public static Color Light = Color.FromArgb(102, 204, 102);
            public static Color Dark = Color.FromArgb(72, 164, 72);
        }

        // 錯誤/危險顏色
        public static class Danger
        {
            public static Color Main = Color.FromArgb(232, 17, 35);
            public static Color Light = Color.FromArgb(242, 47, 65);
            public static Color Dark = Color.FromArgb(212, 0, 25);
        }

        // 背景顏色
        public static class Background
        {
            public static Color Light = Color.FromArgb(250, 250, 250);
            public static Color Paper = Color.White;
            public static Color Default = Color.FromArgb(245, 245, 245);
            public static Color Dark = Color.FromArgb(45, 45, 48);
            public static Color DarkPaper = Color.FromArgb(30, 30, 30);
        }

        // 文字顏色
        public static class Text
        {
            public static Color Primary = Color.FromArgb(50, 50, 50);
            public static Color Secondary = Color.FromArgb(100, 100, 100);
            public static Color Disabled = Color.FromArgb(180, 180, 180);
            public static Color Light = Color.White;
        }

        // 邊框顏色
        public static class Border
        {
            public static Color Light = Color.FromArgb(230, 230, 230);
            public static Color Main = Color.FromArgb(200, 200, 200);
            public static Color Dark = Color.FromArgb(150, 150, 150);
        }

        // 陰影顏色
        public static class Shadow
        {
            public static Color Light = Color.FromArgb(30, 0, 0, 0);
            public static Color Medium = Color.FromArgb(50, 0, 0, 0);
            public static Color Dark = Color.FromArgb(80, 0, 0, 0);
        }

        // 狀態顏色
        public static class Status
        {
            public static Color Success = Color.FromArgb(92, 184, 92);
            public static Color Warning = Color.FromArgb(240, 173, 78);
            public static Color Error = Color.FromArgb(232, 17, 35);
            public static Color Info = Color.FromArgb(91, 192, 222);
        }

        // 圓角半徑
        public static class BorderRadius
        {
            public const int Small = 4;
            public const int Medium = 8;
            public const int Large = 12;
            public const int ExtraLarge = 16;
        }

        // 間距
        public static class Spacing
        {
            public const int XSmall = 4;
            public const int Small = 8;
            public const int Medium = 16;
            public const int Large = 24;
            public const int XLarge = 32;
        }

        // 字體
        public static class Fonts
        {
            public static Font Title = new Font("Segoe UI", 18F, FontStyle.Bold);
            public static Font Subtitle = new Font("Segoe UI Semibold", 14F);
            public static Font Heading = new Font("Segoe UI Semibold", 12F);
            public static Font Body = new Font("Segoe UI", 9F);
            public static Font Small = new Font("Segoe UI", 8F);
            public static Font Caption = new Font("Segoe UI", 7F);
        }
    }
}
