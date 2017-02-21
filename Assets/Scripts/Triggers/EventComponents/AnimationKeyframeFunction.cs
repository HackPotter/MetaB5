using System.Collections.Generic;
using UnityEngine;

public delegate void AnimationChannelGroupMemberTriggered(string tag, string group, GameObject sender);
public delegate void AnimationChannelTriggered(string tag, GameObject sender);

[AddComponentMenu("Metablast/Triggers/Components/Animation Keyframe Function")]
[RequireComponent(typeof(Animation))]
public class AnimationKeyframeFunction : MonoBehaviour
{
    private static Dictionary<string, List<AnimationKeyframeFunction>> _keyframeFunctionTagGroups = new Dictionary<string, List<AnimationKeyframeFunction>>();
    private static Dictionary<string, AnimationChannelGroupMemberTriggered> _groupEventTable = new Dictionary<string, AnimationChannelGroupMemberTriggered>();

    public static void RegisterGroupEventHandler(string tag, AnimationChannelGroupMemberTriggered callback)
    {
        if (!_groupEventTable.ContainsKey(tag))
        {
            _groupEventTable.Add(tag, callback);
        }
        else
        {
            _groupEventTable[tag] += callback;
        }
    }

    public static void DeregisterGroupEventHandler(string tag, AnimationChannelGroupMemberTriggered callback)
    {
        if (!_groupEventTable.ContainsKey(tag))
        {
            DebugFormatter.LogError(null, "Could not deregister animation keyframe function group handler. Tag {0} not found", tag);
            return;
        }

        _groupEventTable[tag] -= callback;
    }

#pragma warning disable 0067, 0649
    [SerializeField]
    private bool _enableGrouping;
    [SerializeField]
    private string _groupName;
#pragma warning restore 0067, 0649

    public event AnimationChannelTriggered OnAnimationEventTriggered;

    void OnEnable()
    {
        if (_enableGrouping)
        {
            if (!_keyframeFunctionTagGroups.ContainsKey(_groupName))
            {
                _keyframeFunctionTagGroups.Add(_groupName, new List<AnimationKeyframeFunction>());
            }
            _keyframeFunctionTagGroups[_groupName].Add(this);
        }
    }

    void OnDisable()
    {
        if (_enableGrouping)
        {
            _keyframeFunctionTagGroups[_groupName].Remove(this);
        }
    }

    public void TriggerEvent(string tag)
    {
        if (OnAnimationEventTriggered != null)
        {
            OnAnimationEventTriggered(tag, this.gameObject);
        }

        if (_enableGrouping)
        {
            AnimationChannelGroupMemberTriggered handlerList = _groupEventTable[_groupName];
            if (handlerList != null)
            {
                handlerList(tag, _groupName, this.gameObject);
            }
        }
    }
}

