using System;

namespace Granges.MikPicture.Core
{
    public class ResizeEventArgs : EventArgs
    {
        public string Filename { get; set; }

        public ResizeEventArgs(string filename)
        {
            Filename = filename;
        }
    }
}