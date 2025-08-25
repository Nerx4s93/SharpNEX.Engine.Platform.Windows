using SharpDX.Direct2D1;
using SharpDX.WIC;
using Bitmap = SharpDX.Direct2D1.Bitmap;
using PixelFormat = SharpDX.WIC.PixelFormat;

namespace SharpNEX.Engine.Platform.Windows.Rendering.DirectX2D1;

internal class DirectX2D1Texture : ITexture
{
    private readonly Bitmap _image;

    public int Width => (int)_image.Size.Width;
    public int Height => (int)_image.Size.Height;

    public DirectX2D1Texture(RenderTarget renderTarget, string path)
    {
        var imagingFactory = new ImagingFactory();

        var bitmapDecoder = new BitmapDecoder(imagingFactory, path, DecodeOptions.CacheOnLoad);
        var frame = bitmapDecoder.GetFrame(0);

        var converter = new FormatConverter(imagingFactory);
        converter.Initialize(frame, PixelFormat.Format32bppPBGRA);

        _image = Bitmap.FromWicBitmap(renderTarget, converter);
    }

    public Bitmap GetImage() => _image;

    public void Dispose()
    {
        _image.Dispose();
    }
}