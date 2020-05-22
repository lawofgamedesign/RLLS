using System;
using System.Collections;
using System.Collections.Generic;

public class EventManager
{
    //create a singleton event manager
    static private EventManager instance;
    static public EventManager Instance
    {
        get
        {
            if (instance == null) { instance = new EventManager(); }

            return instance;
        }
    }


    //dictionary of handlers, indexed by the event that triggers the handlers
    private Dictionary<Type, Event.Handler> registeredHandlers = new Dictionary<Type, Event.Handler>();


    //Objects call this function to subscribe to events of a given type.
    //handler = The function to be called when the event occurs.
    //T = The type of the event.
    public void Register<T>(Event.Handler handler) where T : Event
    {
        Type type = typeof(T);

        /*
         * 
         * If there's already an event of that type, add this handler to the list of handlers for that event.
         * If not, add that event to the dictionary, with this handler as the first item in the list.
         * Note that there's no explicit list; in C# delegates are both references to a function and a list of functions.
         * 
         */

        if (registeredHandlers.ContainsKey(type))
        {
            registeredHandlers[type] += handler;
        } else
        {
            registeredHandlers[type] = handler; 
        }
    }


    //Objects call this to unsubscribe to events of a given type.
    public void Unregister<T>(Event.Handler handler) where T : Event
    {
        Type type = typeof(T);

        Event.Handler handlers;

        if (registeredHandlers.TryGetValue(type, out handlers))
        {
            handlers -= handler;

            if (handlers == null)
            {
                registeredHandlers.Remove(type);
            } else
            {
                registeredHandlers[type] = handlers;
            }
        }
    }


    //Objects call this to publish an event.
    public void Fire(Event e)
    {
        Type type = e.GetType();

        Event.Handler handlers;

        if (registeredHandlers.TryGetValue(type, out handlers))
        {
            handlers(e);
        }
    }
}
