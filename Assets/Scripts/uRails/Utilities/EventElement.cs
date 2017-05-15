

public delegate void EventDelegate<T>(T eventData);
public delegate void EventDelegate<T1, T2>(T1 arg1, T2 arg2);
public delegate void EventDelegate<T1, T2, T3>(T1 arg1, T2 arg2, T3 arg3);

public struct EventElement<T>
{
    private event EventDelegate<T> eventdelegate;

    public void Dispatch(T eventData)
    {
        if (eventdelegate != null)
        {
            eventdelegate(eventData);
        }
    }

    public static EventElement<T> operator +(EventElement<T> kElement, EventDelegate<T> kDelegate)
    {
        kElement.eventdelegate += kDelegate;
        return kElement;
    }

    public static EventElement<T> operator -(EventElement<T> kElement, EventDelegate<T> kDelegate)
    {
        kElement.eventdelegate -= kDelegate;
        return kElement;
    }
}

public struct EventElement<T1, T2>
{
    private event EventDelegate<T1, T2> eventdelegate;

    public void Dispatch(T1 arg1, T2 arg2)
    {
        if (eventdelegate != null)
        {
            eventdelegate(arg1, arg2);
        }
    }

    public static EventElement<T1, T2> operator +(EventElement<T1, T2> kElement, EventDelegate<T1, T2> kDelegate)
    {
        kElement.eventdelegate += kDelegate;
        return kElement;
    }

    public static EventElement<T1, T2> operator -(EventElement<T1, T2> kElement, EventDelegate<T1, T2> kDelegate)
    {
        kElement.eventdelegate -= kDelegate;
        return kElement;
    }
}

public struct EventElement<T1, T2, T3>
{
    private event EventDelegate<T1, T2, T3> eventdelegate;

    public void Dispatch(T1 arg1, T2 arg2, T3 arg3)
    {
        if (eventdelegate != null)
        {
            eventdelegate(arg1, arg2, arg3);
        }
    }

    public static EventElement<T1, T2, T3> operator +(EventElement<T1, T2, T3> kElement, EventDelegate<T1, T2, T3> kDelegate)
    {
        kElement.eventdelegate += kDelegate;
        return kElement;
    }

    public static EventElement<T1, T2, T3> operator -(EventElement<T1, T2, T3> kElement, EventDelegate<T1, T2, T3> kDelegate)
    {
        kElement.eventdelegate -= kDelegate;
        return kElement;
    }
}