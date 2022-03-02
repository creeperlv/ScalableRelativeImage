using ScalableRelativeImage.Nodes;

namespace ScalableRelativeImage
{
    public record ShapeDisposedWarning : ExecutionWarning
    {
        public ShapeDisposedWarning(INode node) : base("SRI005", $"Shape \"{node.GetType().Name}\" has been disposed.") { }
    }
}
