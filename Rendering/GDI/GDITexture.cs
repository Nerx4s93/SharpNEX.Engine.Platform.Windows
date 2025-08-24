using System.Drawing;
using System.Drawing.Imaging;
using System.Runtime.InteropServices;

namespace SharpNEX.Engine.Platform.Windows.Rendering.GDI;

public class GDITexture : ITexture
{
    private readonly Image _image;

    public int Width => _image.Width;
    public int Height => _image.Height;

    public GDITexture(string path)
    {
        _image = Image.FromFile(path);
    }

    public GDITexture(int width, int height, byte[] data)
    {
        var bmp = new Bitmap(width, height, PixelFormat.Format32bppArgb);

        var bmpData = bmp.LockBits(
            new Rectangle(0, 0, width, height),
            ImageLockMode.WriteOnly,
            PixelFormat.Format32bppArgb);

        try
        {
            Marshal.Copy(data, 0, bmpData.Scan0, data.Length);
        }
        finally
        {
            bmp.UnlockBits(bmpData);
        }

        _image = bmp;
    }

    public GDITexture(Image existingImage)
    {
        _image = existingImage;
    }

    public Image GetImage() => _image;

    public void Dispose()
    {
        _image.Dispose();
    }
}