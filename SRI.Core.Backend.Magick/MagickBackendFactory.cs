namespace SRI.Core.Backend.Magick
{
    public class MagickBackendFactory : BaseBackendFactory
    {
        public override IGraphicsBackend CreateBackend()
        {
            return new MagickGraphicsBackend();
        }
    }
}
