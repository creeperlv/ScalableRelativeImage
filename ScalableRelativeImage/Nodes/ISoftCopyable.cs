namespace ScalableRelativeImage.Nodes
{
    public interface ISoftCopyable
    {
        ISoftCopyable SoftCopy();
    }
    public interface ISoftCopyable<T>
    {
        T SoftCopy();
    }
}