using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace ImageViewer.ViewModel.Events
{
    public class EventAggreator
    {
        private static EventAggreator _instance;
        public static EventAggreator Instance
        {
            get
            {
                if (_instance == null)
                {
                    _instance = new EventAggreator();
                }
                return _instance;
            }
        }

        private Dictionary<string, List<Delegate>> _eventHandlers = new Dictionary<string, List<Delegate>>();

        public void Subscribe<TEvent>(string eventName,Action<TEvent> handler)
        {
            if (!_eventHandlers.ContainsKey(eventName))
            {
                _eventHandlers[eventName] = new List<Delegate>();
            }
            _eventHandlers[eventName].Add(handler);
        }

        public void Unsubscribe<TEvent>(string eventName, Action<TEvent> handler)
        {
            if (_eventHandlers.ContainsKey(eventName))
            {
                _eventHandlers[eventName].Remove(handler);
            }
        }

        public void Publish<TEvent>(string eventName, TEvent eventData)
        {
            if (_eventHandlers.ContainsKey(eventName))
            {
                foreach (var handler in _eventHandlers[eventName].OfType<Action<TEvent>>())
                {
                    handler(eventData);
                }
            }
        }
    }
}
