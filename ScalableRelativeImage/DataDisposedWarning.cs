namespace ScalableRelativeImage
{
    public record DataDisposedWarning : ExecutionWarning
    {
        public DataDisposedWarning(string Key, string Value) : base("SRI003", $"Data \"{Value}\"(\"{Key}\") has been disposed.") { }
    }
}
