using System;

namespace AltinnDesktopTool.Utils.PubSub
{
    public class PubSubEventArgs<T> : EventArgs
    {
        public T Item { get; set; }

        public PubSubEventArgs(T item)
        {
            Item = item;
        }
    }
}