using System.Collections.Generic;

public interface IGameplayObjectManager
{
    T CreateGameplayObject<T>(string identifier) where T : class, IGameplayObject, new();
    T GetGameplayObject<T>(string identifier) where T : class, IGameplayObject;

    IEnumerable<T> GetAll<T>() where T : class, IGameplayObject;

}