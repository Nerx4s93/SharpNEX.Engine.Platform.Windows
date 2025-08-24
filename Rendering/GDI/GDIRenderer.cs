using System.Drawing;
using System.Runtime.InteropServices;

namespace SharpNEX.Engine.Platform.Windows.Rendering.GDI
{
    internal class GDIRenderer : IWinRenderer
    {
        private Graphics? _formGraphics;
        private Graphics? _bufferGraphics;

        private Bitmap? _buffer;

        public void Init(IntPtr hwnd, int width, int height)
        {
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
            var hwnd = _formGraphics!.GetHdc();
            var rect = new Rectangle();
            GetClientRect(hwnd, ref rect);
            _formGraphics.ReleaseHdc(hwnd);

            _formGraphics.DrawImage(_buffer!, rect);
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
        public static extern bool GetClientRect(IntPtr hWnd, ref Rectangle rect);

        #endregion
    }
}
