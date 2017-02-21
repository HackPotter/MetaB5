using System;
using System.Collections.Generic;
using System.Linq;
using System.Reflection;
using System.Text;
using UnityEditor;
using UnityEngine;

public static class TriggerGUILayout
{
    private class TypeCacheEntry
    {
        public Type Type
        {
            get;
            private set;
        }

        public string DisplayName
        {
            get;
            private set;
        }

        public TypeCacheEntry(Type type, string displayName)
        {
            Type = type;
            DisplayName = displayName;
        }

        public override int GetHashCode()
        {
            return Type.GetHashCode();
        }

        public override bool Equals(object obj)
        {
            if (obj == null || !(obj is TypeCacheEntry))
            {
                return false;
            }
            if (this == obj)
            {
                return true;
            }

            if (Type == (obj as TypeCacheEntry).Type)
            {
                return true;
            }
            return false;
        }
    }
    private static Dictionary<Type, Dictionary<string, Type>> _expressionTypeByEvaluationTypeCache = new Dictionary<Type, Dictionary<string, Type>>();
    private static Dictionary<Type, Dictionary<string, ExpressionFieldAttribute>> _expressionPropertyAttributesByExpressionType = new Dictionary<Type, Dictionary<string, ExpressionFieldAttribute>>();

    private static readonly Func<Type, bool> kEventSenderPredicate = (t) => typeof(EventSender).IsAssignableFrom(t) && !typeof(EventFilter).IsAssignableFrom(t) && !t.IsAbstract;
    private static readonly Func<Type, bool> kEventFilterPredicate = (t) => typeof(EventFilter).IsAssignableFrom(t) && !t.IsAbstract;
    private static readonly Func<Type, bool> kEventActionPredicate = (t) => typeof(EventResponder).IsAssignableFrom(t) && !t.IsAbstract;

    private static Dictionary<Func<Type, bool>, Dictionary<string, TypeCacheEntry>> _typeSelectorCache = new Dictionary<Func<Type, bool>, Dictionary<string, TypeCacheEntry>>();

    public static TriggerAttribute GetTriggerAttribute(Type triggerComponentType)
    {
        TriggerAttribute[] triggerAttributes = (TriggerAttribute[])triggerComponentType.GetCustomAttributes(typeof(TriggerAttribute), false);

        if (triggerAttributes.Length > 0)
        {
            return triggerAttributes[0];
        }
        else
        {
            return null;
        }
    }

    // Gotta pass in the extra variables.
    public static void DrawSerializedObject(SerializedObject obj, Type triggerComponentType, Dictionary<string, Variable> variables)
    {
        SerializedProperty property = obj.GetIterator();
        property.NextVisible(true);
        while (property.NextVisible(false))
        {
            if (property.type.StartsWith("PPtr<$"))
            {
                string typeName = property.type.Substring(6, property.type.Length - 7);

                Type t = Assembly.GetAssembly(typeof(Expression)).GetType(typeName);
                if (typeof(Expression).IsAssignableFrom(t))
                {
                    GUILayout.BeginHorizontal();
                    GUILayout.Space(10);
                    GUILayout.BeginVertical();
                    TriggerGUILayout.DrawExpressionSelector(property, triggerComponentType, variables);
                    GUILayout.EndVertical();
                    GUILayout.EndHorizontal();
                    continue;
                }
            }
            GUILayout.BeginHorizontal();
            GUILayout.Space(10);
            EditorGUILayout.PropertyField(property, true, GUILayout.ExpandWidth(true));
            obj.ApplyModifiedProperties();
            GUILayout.EndHorizontal();

            GUILayout.Space(10);
        }
    }

    private static string GetFriendlyPropertyDisplayName(SerializedProperty property)
    {
        string propertyName = property.name;
        propertyName = propertyName.Replace("_", "");
        propertyName = propertyName.Substring(0, 1).ToUpper() + propertyName.Substring(1);

        StringBuilder stringBuilder = new StringBuilder(propertyName.Length * 2);

        for (int i = 0; i < propertyName.Length - 1; i++)
        {
            stringBuilder.Append(propertyName[i]);
            if (!char.IsUpper(propertyName[i]) && char.IsUpper(propertyName[i + 1]))
            {
                stringBuilder.Append(' ');
            }
        }

        stringBuilder.Append(propertyName[propertyName.Length - 1]);

        return stringBuilder.ToString();
    }

    public static void DrawAddFilterSelector(GameObject parent, Action addedFilterCallback)
    {
        GUILayout.BeginHorizontal();
        GUILayout.FlexibleSpace();
        GUILayout.Label("Add Filter:", GUILayout.ExpandWidth(false));
        Type selectedFilterType = null;
        selectedFilterType = TriggerGUILayout.DrawFilterSelector(selectedFilterType);

        if (selectedFilterType != null)
        {
            int ordinal = GetLastExecutionTime(parent) + 1;
            GameObject filterGameObject = new GameObject(selectedFilterType.Name);
            filterGameObject.transform.parent = parent.transform;
            EventFilter filter = (EventFilter)filterGameObject.AddComponent(selectedFilterType);
            filter.Ordinal = ordinal;
            addedFilterCallback();
        }
        GUILayout.EndHorizontal();
    }

    public static void DrawAddActionSelector(GameObject parent, Action addedActionCallback)
    {
        GUILayout.BeginHorizontal();
        GUILayout.FlexibleSpace();
        GUILayout.Label("Add Action:", GUILayout.ExpandWidth(false));

        Type selectedActionType = null;
        selectedActionType = TriggerGUILayout.DrawActionSelector(selectedActionType);
        if (selectedActionType != null)
        {
            int ordinal = GetLastExecutionTime(parent) + 1;
            GameObject actionGameObject = new GameObject(selectedActionType.Name);
            actionGameObject.transform.parent = parent.gameObject.transform;
            EventResponder responder = (EventResponder)actionGameObject.AddComponent(selectedActionType);
            responder.Ordinal = ordinal;
            List<TriggerActionGroupDescriptor> actionGroupDescriptors = responder.GetTriggerActionGroups();
            foreach (TriggerActionGroupDescriptor descriptor in actionGroupDescriptors)
            {
                TriggerActionGroup.CreateActionGroup(descriptor).transform.parent = responder.gameObject.transform;
            }
            addedActionCallback();
        }
        GUILayout.EndHorizontal();
    }

    private static int GetLastExecutionTime(GameObject parentGameObject)
    {
        int nextVal = -1;

        foreach (Transform transform in parentGameObject.transform)
        {
            EventFilter sibling = transform.GetComponent<EventFilter>();
            if (sibling != null)
            {
                nextVal = Math.Max(nextVal, sibling.Ordinal);
            }

            EventResponder responder = transform.GetComponent<EventResponder>();
            if (responder != null)
            {
                nextVal = Math.Max(nextVal, responder.Ordinal);
            }
        }

        return nextVal;
    }

    private static void MoveOrderableDown(IOrderable orderable)
    {
        GameObject parentGameObject = orderable.gameObject.transform.parent.gameObject;

        int time = orderable.Ordinal;
        int nextTime = time + 1;

        foreach (Transform transform in parentGameObject.transform)
        {
            IOrderable sibling = transform.GetComponent<EventFilter>();
            if (sibling != null && !sibling.Equals(orderable) && sibling.Ordinal == nextTime)
            {
                orderable.Ordinal = nextTime;
                sibling.Ordinal = time;
                return;
            }

            sibling = transform.GetComponent<EventResponder>();
            if (sibling != null && !sibling.Equals(orderable) && sibling.Ordinal == nextTime)
            {
                orderable.Ordinal = nextTime;
                sibling.Ordinal = time;
                return;
            }
        }
    }

    private static void MoveOrderableUp(IOrderable orderable)
    {
        GameObject parentGameObject = orderable.gameObject.transform.parent.gameObject;

        int time = orderable.Ordinal;
        int nextTime = time - 1;

        foreach (Transform transform in parentGameObject.transform)
        {
            IOrderable sibling = transform.GetComponent<EventFilter>();
            if (sibling != null && !sibling.Equals(orderable) && sibling.Ordinal == nextTime)
            {
                orderable.Ordinal = nextTime;
                sibling.Ordinal = time;
                return;
            }

            sibling = transform.GetComponent<EventResponder>();
            if (sibling != null && !sibling.Equals(orderable) && sibling.Ordinal == nextTime)
            {
                orderable.Ordinal = nextTime;
                sibling.Ordinal = time;
                return;
            }
        }
    }

    private static IOrderable GetNextOrderable(IOrderable orderable)
    {
        GameObject parentGameObject = orderable.gameObject.transform.parent.gameObject;
        int time = orderable.Ordinal + 1;

        return GetOrderableAtTime(parentGameObject, time);
    }

    private static IOrderable GetPreviousOrderable(IOrderable orderable)
    {
        GameObject parentGameObject = orderable.gameObject.transform.parent.gameObject;
        int time = orderable.Ordinal - 1;

        return GetOrderableAtTime(parentGameObject, time);
    }

    private static IOrderable GetOrderableAtTime(GameObject parentGameObject, int time)
    {
        foreach (Transform transform in parentGameObject.transform)
        {
            EventFilter sibling = transform.GetComponent<EventFilter>();
            if (sibling != null && sibling.Ordinal == time)
            {
                return sibling;
            }

            EventResponder responderSibling = transform.GetComponent<EventResponder>();
            if (responderSibling != null && responderSibling.Ordinal == time)
            {
                return responderSibling;
            }
        }

        return null;
    }

    public static bool DrawCustomEventInspectorBar(bool expanded, GameObject senderGameObject, out EventSender newSender)
    {
        EventSender sender = senderGameObject.GetComponent<EventSender>();
        EditorGUILayout.BeginHorizontal();

        EditorGUILayoutExt.BeginLabelStyle(12, FontStyle.Bold, new Color(0.45f, 0.45f, 0.45f), null);

        expanded = GUILayout.Button(expanded ? "▼" : "►", GUI.skin.label, GUILayout.ExpandWidth(false)) ? !expanded : expanded;
        if (sender != null)
        {
            bool enabled = GUILayout.Toggle(sender.enabled, "", GUILayout.ExpandWidth(false));
            if (enabled != sender.enabled)
            {
                sender.enabled = enabled;
            }
        }
        EditorGUILayoutExt.BeginLabelStyle(null, null, new Color(0.72f, 1f, 0.72f), null);
        if (sender != null)
        {
            expanded = GUILayout.Button(sender.GetType().Name, GUI.skin.label, GUILayout.ExpandHeight(false), GUILayout.ExpandWidth(false)) ? !expanded : expanded;
        }
        else
        {
            expanded = GUILayout.Button("Event: None!", GUI.skin.label, GUILayout.ExpandHeight(false), GUILayout.ExpandWidth(false)) ? !expanded : expanded;
        }
        GUILayout.FlexibleSpace();
        GUILayout.FlexibleSpace();

        EditorGUILayoutExt.EndLabelStyle();
        EditorGUILayoutExt.EndLabelStyle();

        Type eventType = TriggerGUILayout.DrawEventSelector(sender != null ? sender.GetType() : null);
        if (eventType != null)
        {
            if (sender == null || eventType != sender.GetType())
            {
                GameObject.DestroyImmediate(sender);
                sender = (EventSender)senderGameObject.AddComponent(eventType);
            }
        }
        else
        {
            GameObject.DestroyImmediate(sender);
            sender = null;
        }

        EditorGUILayout.EndHorizontal();

        newSender = sender;
        return expanded;
    }

    public static bool DrawCustomFilterInspectorBar(bool expanded, EventFilter filter, out EventFilter newFilter)
    {
        EditorGUILayout.BeginHorizontal();

        EditorGUILayoutExt.BeginLabelStyle(12, FontStyle.Bold, new Color(0.45f, 0.45f, 0.45f), null);

        expanded = GUILayout.Button(expanded ? "▼" : "►", GUI.skin.label, GUILayout.ExpandWidth(false)) ? !expanded : expanded;
        bool newEnabled = GUILayout.Toggle(filter.enabled, "", GUILayout.ExpandWidth(false));
        if (newEnabled != filter.enabled)
        {
            filter.enabled = newEnabled;
        }
        EditorGUILayoutExt.BeginLabelStyle(null, null, new Color(1f, 1f, 0.72f), null);
        expanded = GUILayout.Button(filter.GetType().Name, GUI.skin.label, GUILayout.ExpandHeight(false), GUILayout.ExpandWidth(false)) ? !expanded : expanded;
        GUILayout.FlexibleSpace();

        EditorGUILayoutExt.EndLabelStyle();
        EditorGUILayoutExt.EndLabelStyle();

        if (GUILayout.Button("▲", GUILayout.ExpandWidth(false)))
        {
            MoveOrderableUp(filter);
        }

        if (GUILayout.Button("▼", GUILayout.ExpandWidth(false)))
        {
            MoveOrderableDown(filter);
        }

        Type eventType = TriggerGUILayout.DrawFilterSelector(filter.GetType());
        if (eventType != null)
        {
            if (eventType != filter.GetType())
            {
                GameObject filterGameObject = filter.gameObject;
                int executionTime = filter.Ordinal;
                GameObject.DestroyImmediate(filter);
                filter = (EventFilter)filterGameObject.AddComponent(eventType);
                filter.Ordinal = executionTime;
                filterGameObject.name = filter.GetType().Name;
            }
        }
        else
        {
            IOrderable next;
            int time = filter.Ordinal + 1;
            while ((next = GetOrderableAtTime(filter.gameObject.transform.parent.gameObject, time)) != null)
            {
                next.Ordinal -= 1;
                time++;
            }
            GameObject.DestroyImmediate(filter.gameObject);
            filter = null;
        }

        EditorGUILayout.EndHorizontal();

        newFilter = filter;
        return expanded;
    }

    public static bool DrawCustomActionInspectorBar(bool expanded, EventResponder responder, out EventResponder newResponder)
    {
        //Event.current.type == EventType.
        EditorGUILayout.BeginHorizontal();

        EditorGUILayoutExt.BeginLabelStyle(12, FontStyle.Bold, new Color(0.45f, 0.45f, 0.45f), null);
        expanded = GUILayout.Button(expanded ? "▼" : "►", GUI.skin.label, GUILayout.ExpandWidth(false)) ? !expanded : expanded;
        bool newEnabled = GUILayout.Toggle(responder.enabled, "", GUILayout.ExpandWidth(false));

        if (newEnabled != responder.enabled)
        {
            responder.enabled = newEnabled;
        }
        EditorGUILayoutExt.BeginLabelStyle(null, null, new Color(1f, 0.72f, 0.72f), null);

        expanded = GUILayout.Button(responder.GetType().Name, GUI.skin.label, GUILayout.ExpandHeight(false), GUILayout.ExpandWidth(false)) ? !expanded : expanded;
        GUILayout.FlexibleSpace();

        EditorGUILayoutExt.EndLabelStyle();
        EditorGUILayoutExt.EndLabelStyle();

        if (GUILayout.Button("▲", GUILayout.ExpandWidth(false)))
        {
            MoveOrderableUp(responder);
        }

        if (GUILayout.Button("▼", GUILayout.ExpandWidth(false)))
        {
            MoveOrderableDown(responder);
        }

        Type eventType = TriggerGUILayout.DrawActionSelector(responder.GetType());
        if (eventType != null)
        {
            if (eventType != responder.GetType())
            {
                GameObject responderGameObject = responder.gameObject;
                int executionTime = responder.Ordinal;
                GameObject.DestroyImmediate(responder);
                responder = (EventResponder)responderGameObject.AddComponent(eventType);
                responder.Ordinal = executionTime;
                responderGameObject.name = responder.GetType().Name;
            }
        }
        else
        {
            IOrderable next;
            int time = responder.Ordinal + 1;
            while ((next = GetOrderableAtTime(responder.gameObject.transform.parent.gameObject, time)) != null)
            {
                next.Ordinal -= 1;
                time++;
            }
            GameObject.DestroyImmediate(responder.gameObject);
            responder = null;
        }

        EditorGUILayout.EndHorizontal();

        newResponder = responder;
        return expanded;
    }

    private static ExpressionFieldAttribute GetExpressionFieldAttribute(Type expressionContainingType, SerializedProperty expressionProperty)
    {
        if (!_expressionPropertyAttributesByExpressionType.ContainsKey(expressionContainingType))
        {
            _expressionPropertyAttributesByExpressionType.Add(expressionContainingType, new Dictionary<string, ExpressionFieldAttribute>());
        }

        var propertyCache = _expressionPropertyAttributesByExpressionType[expressionContainingType];
        if (!propertyCache.ContainsKey(expressionProperty.name))
        {
            FieldInfo fieldInfo = expressionContainingType.GetField(expressionProperty.name, BindingFlags.Instance | BindingFlags.NonPublic | BindingFlags.Public | BindingFlags.FlattenHierarchy);
            ExpressionFieldAttribute fieldAttribute = null;
            if (fieldInfo != null)
            {
                ExpressionFieldAttribute[] typeAttributes = (ExpressionFieldAttribute[])fieldInfo.GetCustomAttributes(typeof(ExpressionFieldAttribute), false);
                if (typeAttributes.Length != 0)
                {
                    fieldAttribute = typeAttributes[0];
                }
            }
            propertyCache.Add(expressionProperty.name, fieldAttribute);
        }

        return propertyCache[expressionProperty.name];
    }

    private static Dictionary<string, Type> GetValidExpressionTypes(Type requiredExpressionValueType)
    {
        if (!_expressionTypeByEvaluationTypeCache.ContainsKey(requiredExpressionValueType))
        {
            Dictionary<string, Type> expressionTypes = Assembly.GetAssembly(typeof(Expression)).GetTypes().Where(
            (t) =>
            {
                if (!typeof(Expression).IsAssignableFrom(t) || t.IsAbstract)
                {
                    return false;
                }

                ExpressionAttribute[] expressionAttributes = (ExpressionAttribute[])t.GetCustomAttributes(typeof(ExpressionAttribute), false);
                if (expressionAttributes.Length != 0)
                {
                    ExpressionAttribute expressionAttribute = expressionAttributes[0];
                    if (expressionAttribute.IsDynamicType)
                    {
                        return true;
                    }
                    else
                    {
                        return requiredExpressionValueType.IsAssignableFrom(expressionAttribute.EvaluationType);
                    }
                }
                else
                {
                    return requiredExpressionValueType.IsAssignableFrom(typeof(object));
                }
            }).ToDictionary((t) => t.Name);
            expressionTypes.Add("-", null);
            _expressionTypeByEvaluationTypeCache.Add(requiredExpressionValueType, expressionTypes);
        }

        return _expressionTypeByEvaluationTypeCache[requiredExpressionValueType];
    }

    public static void DrawExpressionSelector(SerializedProperty expressionProperty, Type expressionContainingType, Dictionary<string, Variable> variables)
    {
        ExpressionFieldAttribute fieldAttribute = GetExpressionFieldAttribute(expressionContainingType, expressionProperty);

        // Defaults for display name and required type if attribute isn't present.
        Type requiredExpressionValueType = fieldAttribute != null ? fieldAttribute.ExpressionType : typeof(object);
        string displayName = fieldAttribute != null ? fieldAttribute.DisplayName : expressionProperty.name;

        if (requiredExpressionValueType == typeof(Variable))
        {
            // Special case. Show all variables:
            UnityEngine.Object propertyValue = expressionProperty.objectReferenceValue;
            if (propertyValue == null || !(propertyValue is VariableLiteralExpression))
            {
                expressionProperty.objectReferenceValue = ScriptableObject.CreateInstance<VariableLiteralExpression>();
            }
            VariableLiteralExpression variableExpression = (VariableLiteralExpression)expressionProperty.objectReferenceValue;

            GUILayout.BeginHorizontal();
            GUILayout.Label(GetFriendlyPropertyDisplayName(expressionProperty), GUILayout.ExpandWidth(false));
            GUILayout.FlexibleSpace();
            Variable variableValue = VariableSelector(variableExpression.Value as Variable, typeof(object), variables, false);
            GUILayout.EndHorizontal();
            variableExpression.VariableValue = variableValue;
            return;
        }

        // Get collection of all expression types in the project.
        // Filter the expression types returned to those that match the field required type.

        Dictionary<string, Type> expressionTypes = GetValidExpressionTypes(requiredExpressionValueType);

        // Get a string array of expression type names to display
        string[] typeArray = expressionTypes.Keys.ToArray();

        // Find the current value of the expression property.
        UnityEngine.Object currentValue = expressionProperty.objectReferenceValue;

        // Find the index of the selection in the typeArray to show the correct type in the drop down
        string currentSelection = currentValue == null ? "-" : currentValue.GetType().Name;
        int currentSelectionIndex = Array.IndexOf(typeArray, currentSelection);

        // Show the drop down selector
        GUILayout.BeginHorizontal();
        GUILayout.Label(displayName, GUILayout.ExpandWidth(false));
        GUILayout.FlexibleSpace();
        int newSelectionIndex = EditorGUILayout.Popup(currentSelectionIndex, expressionTypes.Keys.ToArray());


        // If the new index isn't what we had before, find the new expression type in the dictionary by type name and assign a new instance of it.
        if (currentSelectionIndex != newSelectionIndex)
        {
            Type newType = expressionTypes[typeArray[newSelectionIndex]];
            if (newType != null)
            {
                expressionProperty.objectReferenceValue = ScriptableObject.CreateInstance(newType);
                expressionProperty.serializedObject.ApplyModifiedProperties();
                currentValue = expressionProperty.objectReferenceValue;
            }
        }

        // Now show the fields of the assigned expression object (if not null)
        if (currentValue != null)
        {
            SerializedObject serializedObject = new SerializedObject(currentValue);
            // If it's a VariableExpression, then we need to show the custom VariableSelector.
            if (currentValue.GetType() == typeof(VariableExpression))
            {
                DrawVariableSelector(serializedObject, requiredExpressionValueType, variables);
            }
            else
            {
                // Otherwise, just show the properties like normal.
                SerializedProperty property = serializedObject.GetIterator();
                property.NextVisible(true);
                while (property.NextVisible(false))
                {
                    GUILayout.BeginHorizontal();
                    GUILayout.Label(GetFriendlyPropertyDisplayName(property));
                    EditorGUILayout.PropertyField(property, GUIContent.none, true);
                    serializedObject.ApplyModifiedProperties();
                    GUILayout.EndHorizontal();
                }
            }
        }
        GUILayout.EndHorizontal();
    }

    public static Variable VariableSelector(Variable current, Type variableType, Dictionary<string, Variable> availableVariables, bool allowDynamic = true)
    {
        if (variableType == null)
        {
            Debug.Log("Variable Type is Null!");
        }
        List<KeyValuePair<string, Variable>> validVariables = availableVariables.Where(
            (v) =>
            {
                return variableType.IsAssignableFrom(v.Value.VariableType) && (allowDynamic || !(v.Value is DynamicVariable));
            }).ToList();

        validVariables.Insert(0, new KeyValuePair<string, Variable>("-", null));

        string[] guiOptions = validVariables.ConvertAll<string>((kvp) => kvp.Key).ToArray();

        int currentIndex = validVariables.IndexOf(validVariables.Find((kvp) => kvp.Value == current));
        int newIndex = EditorGUILayout.Popup(currentIndex, guiOptions);

        return newIndex <= 0 || newIndex > guiOptions.Length - 1 ? null : availableVariables[guiOptions[newIndex]];
    }

    // Get rid of this and use VariableSelector
    public static void DrawVariableSelector(SerializedObject variableExpression, Type variableType, Dictionary<string, Variable> variables)
    {
        // Get all valid assignments: find variables that are assignable to the expression object and then convert it to a string list.
        List<string> validVariables = variables.Where(
            (v) =>
            {
                if (v.Value == null || variableType == null)
                {
                    return false;
                }
                return variableType.IsAssignableFrom(v.Value.VariableType);
            }).ToList().ConvertAll((v) => v.Key);
        validVariables.Insert(0, "-");
        string[] validVariablesArray = validVariables.ToArray();

        VariableExpression expression = variableExpression.targetObject as VariableExpression;
        int currentIndex = string.IsNullOrEmpty(expression.VariableIdentifier) ? 0 : Array.IndexOf(validVariablesArray, expression.VariableIdentifier);
        currentIndex = currentIndex < 0 ? 0 : currentIndex;
        int newIndex = EditorGUILayout.Popup(currentIndex, validVariablesArray);

        if (newIndex != currentIndex)
        {
            variableExpression.FindProperty("_variableIdentifier").stringValue = validVariablesArray[newIndex];
            variableExpression.ApplyModifiedProperties();
        }
    }

    public static Type DrawEventSelector(Type currentType)
    {
        return DrawTypeSelector(currentType, kEventSenderPredicate);
    }

    public static Type DrawFilterSelector(Type currentType)
    {
        return DrawTypeSelector(currentType, kEventFilterPredicate);
    }

    public static Type DrawActionSelector(Type currentType)
    {
        return DrawTypeSelector(currentType, kEventActionPredicate);
    }

    private static TypeCacheEntry GetTypeCacheEntryFromType(Type t)
    {
        TriggerAttribute[] attributes = (TriggerAttribute[])t.GetCustomAttributes(typeof(TriggerAttribute), false);
        if (attributes.Length == 0)
        {
            return new TypeCacheEntry(t, t.Name);
        }
        else
        {
            if (!string.IsNullOrEmpty(attributes[0].DisplayPath))
            {
                return new TypeCacheEntry(t, attributes[0].DisplayPath + "/" + t.Name);
            }
            else
            {
                return new TypeCacheEntry(t, t.Name);
            }
        }
    }

    private static Type DrawTypeSelector(Type currentType, Func<Type, bool> filterPredicate)
    {
        if (!_typeSelectorCache.ContainsKey(filterPredicate))
        {
            _typeSelectorCache.Add(filterPredicate, Assembly.GetAssembly(typeof(EventSender)).GetTypes().Where(filterPredicate).Select<Type, TypeCacheEntry>((t) => GetTypeCacheEntryFromType(t)).ToDictionary((t) => t.DisplayName));
        }

        Dictionary<string, TypeCacheEntry> eventTypes = _typeSelectorCache[filterPredicate];
        List<string> eventTypeKeysList = eventTypes.Keys.ToList();

        eventTypeKeysList.Sort();

        if (currentType == null)
        {
            eventTypeKeysList.Add("-");
        }
        else
        {
            eventTypeKeysList.Add("");
            eventTypeKeysList.Add("Delete");
        }

        string[] eventTypeKeys = eventTypeKeysList.ToArray();

        // Get current selection
        int currentIndex = currentType != null ? Array.IndexOf(eventTypeKeys, GetTypeCacheEntryFromType(currentType).DisplayName) : Array.IndexOf(eventTypeKeys, "-");


        int newIndex = EditorGUILayout.Popup(currentIndex, eventTypeKeys);
        /*
        if (newIndex != currentIndex)
        {
            Debug.Log("Current Index: " + currentIndex + "; " + (currentType != null ? currentType.Name : "null type"));
            Debug.Log("New Index: " + newIndex + "New Type: " + (newIndex < eventTypeKeys.Length ? eventTypeKeys[newIndex] : "out of range"));
            if (newIndex < eventTypeKeys.Length)
            {
                Debug.Log("New Type in cache? " + eventTypes.ContainsKey(eventTypeKeys[newIndex]));
            }
        }*/

        if (newIndex >= eventTypes.Count || newIndex < 0)
        {
            //Debug.Log("New Index...: " + newIndex);
            return null;
        }

        //return currentType;

        return eventTypes[eventTypeKeys[newIndex]].Type;
    }
}

