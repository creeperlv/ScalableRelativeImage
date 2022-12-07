using SRI.Core.Backend;
using SRI.Core.Backend.Magick;
using SRI.Core.Backend.SystemDrawing;

namespace SRI.Core.Core
{
    public class BackendFactory:BaseBackendFactory
    {
        public static BackendDefinition UsingBackend = BackendDefinition.SystemDrawing;
        //public static readonly BackendFactory Instance = new BackendFactory();
        
        public override IGraphicsBackend CreateBackend()
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
