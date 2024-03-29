﻿namespace ScalableRelativeImage
{
    /// <summary>
    /// Warning thrown during analysis and render.
    /// </summary>
    public record ExecutionWarning
    {
        public string ID;
        public string Message;
        public ExecutionWarning(string ID, string Message)
        {
            this.ID = ID;
            this.Message = Message;
        }
    }
}
