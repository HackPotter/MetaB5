using System.Collections.Generic;

public class TestNewFilter : EventResponder
{
    public bool DoGroupOne;
    public string Message;

    private static readonly TriggerActionGroupDescriptor _testActionGroup1 = new TriggerActionGroupDescriptor("Test1", "This is just a test");
    private static readonly TriggerActionGroupDescriptor _testActionGroup2 = new TriggerActionGroupDescriptor("Test2", "This is just a test");
    public override List<TriggerActionGroupDescriptor> GetTriggerActionGroups()
    {
        return new List<TriggerActionGroupDescriptor>() { _testActionGroup1, _testActionGroup2 };
    }

    public override void OnEvent(ExecutionContext context)
    {
        DebugFormatter.Log(this, Message);
        if (DoGroupOne)
        {
            TriggerActionGroup(_testActionGroup1, context);
        }
        else
        {
            TriggerActionGroup(_testActionGroup2, context);
        }
    }
}

