using UnityEngine;

// What if we want to expose multiple arguments?
//      We can aggregate the arguments into one object...
// But that doesn't allow an action to only use one of them, since the action needs to know the type.
//      We could force DataProviders to implement a set of interfaces for exposing different parameter types and do runtime type checks.
// But that doesn't allow an EventSender to expose multiple parameters of the same type.
//      We could store a collection of arguments parameterized by a string key
// But that uses a contract by naming convention...


// Requirements:

// Able to expose multiple arbitrary objects.
// Must provide an interface that allows a consumer to retrieve any of those objects with meaning behind them (i.e., know the difference between two parameters of the same time)
// But must also provide an interface that allows a consumer to use it based on what it can expose, not just what it does expose
// I don't think there's any way to meet these requirements. There is too much context in terms of which parameters are being utilized.

//      Since different arguments might have the same type but represent different information depending on the context, there's no way to convey the context for a given argument with a type system
//          except to implement a different type for each context. I.e., not just a GameObject argument; a TriggeringGameObject argument vs an EnteringGameObject argument vs ...


// The problem stems from this whole idea of treating an EventSender like a collection instead of exposing each argument individually.

// What if EventSenders just use RequireComponent(DataProvider) and DataProvider has a custom inspector to display usage and whatever?

public abstract class DataProvider : MonoBehaviour
{
    public abstract object GetValue();
}