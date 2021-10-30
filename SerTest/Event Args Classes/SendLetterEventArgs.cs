using System;
using LetterSpace;

namespace VariantB
{
    public class SendLetterEventArgs : EventArgs
    {
        public Letter Letter { get; set; }
    }

    public delegate void SendLetterHandler(object source, SendLetterEventArgs arg);
}
