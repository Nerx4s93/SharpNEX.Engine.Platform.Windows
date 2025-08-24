namespace SharpNEX.Engine.Platform.Windows.Rendering.GDI
{
    internal class GDIRenderer : IWinRenderer
    {
        public void BeginFrame()
        {
            throw new NotImplementedException();
        }

        public void EndFrame()
        {
            throw new NotImplementedException();
        }

        public void Clear(float r, float g, float b, float a)
        {
            throw new NotImplementedException();
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
            throw new NotImplementedException();
        }
    }
}
