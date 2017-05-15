using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

public class EventElementCollection<TKey, TEventArg>
{
    private Dictionary<TKey, EventElement<TEventArg>> _eventElements = new Dictionary<TKey, EventElement<TEventArg>>();

    public EventElement<TEventArg> this[TKey key]
    {
        get
        {
            EventElement<TEventArg> eventElement;
            if (!_eventElements.TryGetValue(key, out eventElement))
            {
                eventElement = new EventElement<TEventArg>();
                _eventElements.Add(key, eventElement);
            }
            return eventElement;
        }
        set
        {
            _eventElements[key] = value;
        }
    }
}

public class EventElementCollection<TKey, TEventArg1, TEventArg2>
{
    private Dictionary<TKey, EventElement<TEventArg1, TEventArg2>> _eventElements = new Dictionary<TKey, EventElement<TEventArg1, TEventArg2>>();

    public EventElement<TEventArg1, TEventArg2> this[TKey key]
    {
        get
        {
            EventElement<TEventArg1, TEventArg2> eventElement;
            if (!_eventElements.TryGetValue(key, out eventElement))
            {
                eventElement = new EventElement<TEventArg1, TEventArg2>();
                _eventElements.Add(key, eventElement);
            }
            return eventElement;
        }
        set
        {
            _eventElements[key] = value;
        }
    }
}

public class EventElementCollection<TKey, TEventArg1, TEventArg2, TEventArg3>
{
    private Dictionary<TKey, EventElement<TEventArg1, TEventArg2, TEventArg3>> _eventElements = new Dictionary<TKey, EventElement<TEventArg1, TEventArg2, TEventArg3>>();

    public EventElement<TEventArg1, TEventArg2, TEventArg3> this[TKey key]
    {
        get
        {
            EventElement<TEventArg1, TEventArg2, TEventArg3> eventElement;
            if (!_eventElements.TryGetValue(key, out eventElement))
            {
                eventElement = new EventElement<TEventArg1, TEventArg2, TEventArg3>();
                _eventElements.Add(key, eventElement);
            }
            return eventElement;
        }
        set
        {
            _eventElements[key] = value;
        }
    }
}
