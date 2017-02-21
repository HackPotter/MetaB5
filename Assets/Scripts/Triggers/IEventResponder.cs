
public interface IEventResponder : IOrderable
{
    void OnEvent(ExecutionContext context);

    bool Enabled { get; }
}

