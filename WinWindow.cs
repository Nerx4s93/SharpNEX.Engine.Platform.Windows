using System.Runtime.InteropServices;

namespace SharpNEX.Engine.Platform.Windows
{
    internal class WinWindow : IWindow
    {
        public string Title { get; set; }
        public int Width { get; }
        public int Height { get; }

        private IntPtr _hwnd;

        public WinWindow(string title, int width, int height)
        {
            Title = title;
            Width = width;
            Height = height;

            CreateWindow();
        }

        private void CreateWindow()
        {
            WNDCLASS wc = new WNDCLASS
            {
                lpfnWndProc = WndProcCallback,
                lpszClassName = "SharpNEXWindowClass",
                hInstance = Marshal.GetHINSTANCE(typeof(WinWindow).Module)
            };
            RegisterClass(ref wc);

            _hwnd = CreateWindowEx(
                0,
                wc.lpszClassName,
                Title,
                0xCF0000,
                100, 100, Width, Height,
                IntPtr.Zero,
                IntPtr.Zero,
                wc.hInstance,
                IntPtr.Zero
            );

            ShowWindow(_hwnd, 1);
            UpdateWindow(_hwnd);

            while (GetMessage(out var msg, IntPtr.Zero, 0, 0))
            {
                TranslateMessage(ref msg);
                DispatchMessage(ref msg);
            }
        }

        private IntPtr WndProcCallback(IntPtr hwnd, uint msg, IntPtr wParam, IntPtr lParam)
        {
            const uint WM_DESTROY = 0x0002;
            switch (msg)
            {
                case WM_DESTROY:
                    PostQuitMessage(0);
                    break;
            }
            return DefWindowProc(hwnd, msg, wParam, lParam);
        }


        #region WinAPI

        [StructLayout(LayoutKind.Sequential)]
        private struct WNDCLASS
        {
            public uint style;
            public WndProc lpfnWndProc;
            public int cbClsExtra;
            public int cbWndExtra;
            public IntPtr hInstance;
            public IntPtr hIcon;
            public IntPtr hCursor;
            public IntPtr hbrBackground;
            public string lpszMenuName;
            public string lpszClassName;
        }

        [StructLayout(LayoutKind.Sequential)]
        private struct MSG
        {
            public IntPtr hwnd;
            public uint message;
            public IntPtr wParam;
            public IntPtr lParam;
            public uint time;
            public Point pt;
        }

        [StructLayout(LayoutKind.Sequential)]
        private struct Point { public int x, y; }

        private delegate IntPtr WndProc(IntPtr hwnd, uint msg, IntPtr wParam, IntPtr lParam);

        [DllImport("user32.dll", SetLastError = true)]
        private static extern ushort RegisterClass([In] ref WNDCLASS lpWndClass);

        [DllImport("user32.dll", SetLastError = true)]
        private static extern IntPtr CreateWindowEx(
            uint dwExStyle,
            string lpClassName,
            string lpWindowName,
            uint dwStyle,
            int x, int y,
            int nWidth, int nHeight,
            IntPtr hWndParent,
            IntPtr hMenu,
            IntPtr hInstance,
            IntPtr lpParam
        );

        [DllImport("user32.dll")]
        private static extern bool ShowWindow(IntPtr hWnd, int nCmdShow);

        [DllImport("user32.dll")]
        private static extern bool UpdateWindow(IntPtr hWnd);

        [DllImport("user32.dll")]
        private static extern bool GetMessage(out MSG lpMsg, IntPtr hWnd, uint wMsgFilterMin, uint wMsgFilterMax);

        [DllImport("user32.dll")]
        private static extern bool TranslateMessage([In] ref MSG lpMsg);

        [DllImport("user32.dll")]
        private static extern IntPtr DispatchMessage([In] ref MSG lpmsg);

        [DllImport("user32.dll")]
        private static extern void PostQuitMessage(int nExitCode);

        [DllImport("user32.dll")]
        private static extern IntPtr DefWindowProc(IntPtr hWnd, uint msg, IntPtr wParam, IntPtr lParam);

        #endregion
    }
}
