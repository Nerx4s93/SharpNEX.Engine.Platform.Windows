using SharpDX;
using SharpDX.Direct2D1;
using SharpDX.Mathematics.Interop;
using SharpNEX.Engine.Platform.Windows.Rendering.DirectX2D1.Exception;

namespace SharpNEX.Engine.Platform.Windows.Rendering.DirectX2D1;

public class DirectX2D1Renderer : IRenderer
{
    private static WindowRenderTarget? _renderTarget;

    public void Init(IntPtr hwnd, int width, int height)
    {
        var factory = new Factory();
        var renderProps = new RenderTargetProperties(
            new PixelFormat(SharpDX.DXGI.Format.B8G8R8A8_UNorm, AlphaMode.Premultiplied)
        );
        var hwndRenderTargetProps = new HwndRenderTargetProperties()
        {
            Hwnd = hwnd,
            PixelSize = new Size2(width, height),
            PresentOptions = PresentOptions.None
        };

        _renderTarget = new WindowRenderTarget(factory, renderProps, hwndRenderTargetProps);
    }

    public void BeginFrame()
    {
        _renderTarget!.BeginDraw();
    }

    public void EndFrame()
    {
        _renderTarget!.EndDraw();
    }

    public void Clear(int r, int g, int b, int a)
    {
        _renderTarget!.Clear(new RawColor4(1.0f, 1.0f, 1.0f, 1.0f));
    }

    public ITexture CreateTexture(string path)
    {
        if (_renderTarget == null)
        {
            throw new RenderTargetNotCreatedException();
        }

        return new DirectX2D1Texture(_renderTarget, path);
    }

    public ITexture CreateTexture(int width, int height, byte[] data)
    {
        throw new NotImplementedException();
    }

    public void DrawTexture(ITexture texture, float x, float y, float width, float height)
    {
        var bitmap = ((DirectX2D1Texture)texture).GetImage();

        var transformMatrix = _renderTarget!.Transform;

        var position = new Vector(x, y);
        var size = new Vector(width, height);
        var center = new Vector(x, y);
        const float angle = 0f;

        var combinedMatrix = MatrixBuilder.Build(position, size, center, angle);

        _renderTarget.Transform = combinedMatrix;

        _renderTarget.DrawBitmap(bitmap, 1.0f, BitmapInterpolationMode.Linear);

        _renderTarget.Transform = transformMatrix;
    }
}