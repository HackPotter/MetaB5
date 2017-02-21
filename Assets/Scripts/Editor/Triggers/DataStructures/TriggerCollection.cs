using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using UnityEngine;

public class TriggerCollection : IEnumerable<Trigger>
{
    private EventEditorContext _context;
    private OverviewEditorContext _overviewContext;
    private TriggerCollectionNode _rootNode;
    private List<Trigger> _triggers;

    public TriggerCollectionNode TriggerCollectionRoot
    {
        get { return _rootNode; }
    }

    public TriggerCollection(EventEditorContext context, OverviewEditorContext overviewContext, TriggerRoot root)
    {
        _rootNode = new TriggerCollectionNode(context, overviewContext, root.gameObject);
        _triggers = new List<Trigger>();
        _context = context;
        _overviewContext = overviewContext;
        Initialize();
    }

    public void Initialize()
    {
        _rootNode = new TriggerCollectionNode(_context, _overviewContext, _rootNode.GameObject);
        _triggers.Clear();
        PopulateTriggerList(_rootNode.GameObject.transform, _triggers);
        BuildTree(_rootNode);
    }

    private void PopulateTriggerList(Transform node, List<Trigger> triggers)
    {
        foreach (Transform child in node.gameObject.transform)
        {
            Trigger trigger = child.GetComponent<Trigger>();
            if (trigger)
            {
                triggers.Add(trigger);
            }

            PopulateTriggerList(child, triggers);
        }
    }

    private void BuildTree(TriggerCollectionNode node)
    {
        foreach (Transform child in node.GameObject.transform)
        {
            var childNode = new TriggerCollectionNode(_context, _overviewContext, child.gameObject);
            node.AddChild(childNode);

            if (child.gameObject.GetComponent<Trigger>())
            {
                continue;
            }
            BuildTree(childNode);
        }
        /*
        foreach (Trigger trigger in triggers)
        {
            Insert(trigger);
        }*/
    }

    private void Insert(Trigger trigger)
    {
        // Build stack of trigger's lineage up to (and excluding) the Event root.
        Stack<GameObject> triggerParents = new Stack<GameObject>();
        Transform current = trigger.transform.parent.transform;
        while (current != null && current != _rootNode.GameObject.transform)
        {
            triggerParents.Push(current.gameObject);
            current = current.parent;
        }

        // Pop the oldest ancestor and traverse tree down lineage adding new nodes as necessary.
        GameObject currentGameObject;
        TriggerCollectionNode currentNode = _rootNode;
        while (triggerParents.Count > 0)
        {
            currentGameObject = triggerParents.Pop();
            TriggerCollectionNode node = currentNode.FirstOrDefault((otherNode) => otherNode.GameObject == currentGameObject);
            if (node != null)
            {
                currentNode = node;
            }
            else
            {
                TriggerCollectionNode newNode = new TriggerCollectionNode(_context, _overviewContext, currentGameObject);
                currentNode.AddChild(newNode);
                currentNode = newNode;
            }
        }

        currentNode.AddChild(new TriggerCollectionNode(_context, _overviewContext, trigger.gameObject));
    }

    public IEnumerator<Trigger> GetEnumerator()
    {
        foreach (Trigger trigger in _triggers)
        {
            yield return trigger;
        }
    }

    System.Collections.IEnumerator System.Collections.IEnumerable.GetEnumerator()
    {
        return GetEnumerator();
    }
}

