using System.Collections.Generic;

namespace AltinnDesktopTool.Utils.PubSub
{

    public delegate void PubSubEventHandler<T>(object sender, PubSubEventArgs<T> args);

    public static class PubSub<T>
    {
        private static Dictionary<string, PubSubEventHandler<T>> _events =
                new Dictionary<string, PubSubEventHandler<T>>();

        public static void AddEvent(string name, PubSubEventHandler<T> handler)
        {
            if (!_events.ContainsKey(name))
                _events.Add(name, handler);
        }
        public static void RaiseEvent(string name, object sender, PubSubEventArgs<T> args)
        {
            if (_events.ContainsKey(name) && _events[name] != null)
                _events[name](sender, args);
        }
        public static void RegisterEvent(string name, PubSubEventHandler<T> handler)
        {
            if (_events.ContainsKey(name))
                _events[name] += handler;
        }
    }
}
