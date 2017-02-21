using System;
using System.Linq;
using System.Collections.Generic;
using System.Reflection;
using UnityEditor;

public class EventEditorContext
{
    private TriggerRoot _triggerRoot;
    private TriggerCollection _triggers;
    private GlobalSymbolTable _symbolTable;

    private OverviewEditorContext _overviewContext;
    
    private EditorWindow _window;

    public bool Repaint
    {
        get;
        set;
    }

    public Trigger SelectedTrigger
    {
        get;
        private set;
    }

    public TriggerRoot TriggerRoot
    {
        get { return _triggerRoot; }
    }

    public TriggerCollection Triggers
    {
        get { return _triggers; }
    }

    public GlobalSymbolTable GlobalSymbolTable
    {
        get { return _symbolTable; }
    }

    public EventEditorContext(EditorWindow window, OverviewEditorContext overviewContext, TriggerRoot triggerRoot, List<Trigger> triggers, GlobalSymbolTable symbolTable)
    {
        _triggerRoot = triggerRoot;
        _overviewContext = overviewContext;
        _window = window;
        _triggers = new TriggerCollection(this, _overviewContext, _triggerRoot);
        _symbolTable = symbolTable;
    }

    public void SelectTrigger(Trigger trigger)
    {
        SelectedTrigger = trigger;
        _triggers = new TriggerCollection(this, _overviewContext, _triggerRoot);
        _triggers.Initialize();
    }

    public void Refresh()
    {
        _window.Repaint();
        _triggers = new TriggerCollection(this, _overviewContext, _triggerRoot);
        _triggers.Initialize();
    }
}

