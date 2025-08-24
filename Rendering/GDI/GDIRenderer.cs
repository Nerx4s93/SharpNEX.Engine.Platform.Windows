using System.Drawing;
using System.Runtime.InteropServices;

namespace SharpNEX.Engine.Platform.Windows.Rendering.GDI
{
    internal class GDIRenderer : IWinRenderer
    {
        private IntPtr _hwnd;
        private Graphics? _formGraphics;
        private Graphics? _bufferGraphics;

        private Bitmap? _buffer;

        public void Init(IntPtr hwnd, int width, int height)
        {
            _hwnd = hwnd;

            _buffer = new Bitmap(width, height);

            _formGraphics = Graphics.FromHwnd(hwnd);
            _bufferGraphics = Graphics.FromImage(_buffer);
        }

        public void BeginFrame()
        {
            Clear(255, 255, 255, 255);
        }

        public void EndFrame()
        {
            _formGraphics = Graphics.FromHwnd(_hwnd);

            GetClientRect(_hwnd, out Rect rect);

            var width = rect.Right - rect.Left;
            var height = rect.Bottom - rect.Top;

            var destRect = new Rectangle(0, 0, width, height);

            _formGraphics!.DrawImage(_buffer!, destRect);
        }


        public void Clear(int r, int g, int b, int a)
        { 
            var color = Color.FromArgb(a, r, g, b);
            _bufferGraphics!.Clear(color);
        }

        public ITexture CreateTexture(string path)
        {
            return new GDITexture(path);
        }

        public ITexture CreateTexture(int width, int height, byte[] data)
        {
            return new GDITexture(width, height, data);
        }

        public void DrawTexture(ITexture texture, float x, float y, float width, float height)
        {
            if (texture is not GDITexture gdiTexture)
            {
                throw new ArgumentException("Текстура должна быть типа GDITexture", nameof(texture));
            }

            var image = gdiTexture.GetImage();
            var destRect = new RectangleF(x, y, width, height);

            _bufferGraphics!.DrawImage(image, destRect);
        }

        #region Import

        [DllImport("user32.dll")]
        private static extern bool GetClientRect(IntPtr hWnd, out Rect lpRect);

        [StructLayout(LayoutKind.Sequential)]
        private struct Rect
        {
            public int Left;
            public int Top;
            public int Right;
            public int Bottom;
        }

        #endregion
    }
}
