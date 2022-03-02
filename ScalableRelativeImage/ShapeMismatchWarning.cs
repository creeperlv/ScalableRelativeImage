using System;

namespace ScalableRelativeImage
{
    public record ShapeMismatchWarning : ExecutionWarning
    {
        public ShapeMismatchWarning(object receieved, Type Target) : base("SRI004", $"Shape {receieved.GetType().Name} does not match required shape \"{Target.Name}\"") { }
    }
}
