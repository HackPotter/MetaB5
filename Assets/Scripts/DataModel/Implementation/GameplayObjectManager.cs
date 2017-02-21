using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

public class GameplayObjectCollection : Dictionary<string, IGameplayObject> { }

public class GameplayObjectManager : IGameplayObjectManager
{
    private Dictionary<Type, GameplayObjectCollection> _gameplayObjectsByType = new Dictionary<Type, GameplayObjectCollection>();

    public T CreateGameplayObject<T>(string identifier) where T : class, IGameplayObject, new()
    {
        GameplayObjectCollection gameplayObjects;
        if (!_gameplayObjectsByType.TryGetValue(typeof(T), out gameplayObjects))
        {
            gameplayObjects = new GameplayObjectCollection();
            _gameplayObjectsByType.Add(typeof(T), gameplayObjects);
        }


        if (!gameplayObjects.ContainsKey(identifier))
        {
            T gameplayObject = new T();
            gameplayObjects.Add(identifier, gameplayObject);
            return gameplayObject;
        }

        DebugFormatter.LogError(this, "Attempting to add gameplay object, but a gameplay object of type {0} with identifier {1} already exists.", typeof(T), identifier);
        return null;
    }

    public T GetGameplayObject<T>(string identifier) where T : class, IGameplayObject
    {
        GameplayObjectCollection gameplayObjects;
        if (!_gameplayObjectsByType.TryGetValue(typeof(T), out gameplayObjects))
        {
            DebugFormatter.LogError(this, "Attempting to retrieve gameplay object of type {0} with identifier {1}, but no gameplay objects of given type exist.", typeof(T), identifier);
            return null;
        }

        IGameplayObject gameplayObject;
        if (!gameplayObjects.TryGetValue(identifier, out gameplayObject))
        {
            DebugFormatter.LogError(this, "Attempting to retrieve gameplay object of type {0} with identifier {1}, but no gameplay object with the given identifier exists.", typeof(T), identifier);
            return null;
        }

        return (T)gameplayObject;
    }

    public IEnumerable<T> GetAll<T>() where T : class, IGameplayObject
    {
        GameplayObjectCollection gameplayObjects;
        if (!_gameplayObjectsByType.TryGetValue(typeof(T), out gameplayObjects))
        {
            DebugFormatter.LogError(this, "Attempting to retrieve gameplay objects of type {0}, but no gameplay objects of given type exist.", typeof(T));
            yield break;
        }

        foreach (var gameplayObject in gameplayObjects.Values)
        {
            yield return (T)gameplayObject;
        }
    }
}

