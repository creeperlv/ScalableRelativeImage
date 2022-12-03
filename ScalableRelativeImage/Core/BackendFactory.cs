namespace SRI.Core.Core
{
    public class BackendFactory
    {
        public static BackendDefinition UsingBackend = BackendDefinition.Magick;
        public static readonly BackendFactory Instance = new BackendFactory();
        public IGraphicsBackend CreateBackend()
        {
            switch (UsingBackend)
            {
                case BackendDefinition.SystemDrawing:
                    return new SystemGraphicsBackend();
                    break;
                case BackendDefinition.Magick:
                    return new MagickGraphicsBackend();
                    break;
                default:
                    break;
            }
            return null;
        }
    }
}
