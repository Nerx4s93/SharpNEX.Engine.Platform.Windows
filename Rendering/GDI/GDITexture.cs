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