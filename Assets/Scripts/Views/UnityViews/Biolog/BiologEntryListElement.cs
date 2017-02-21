using System;
using UnityEngine;
using UnityEngine.Events;
using UnityEngine.UI;

[Serializable]
public class BiologEntryButtonPressed : UnityEvent<BiologEntryListElement>
{
}

public class BiologEntryListElement : MonoBehaviour
{
#pragma warning disable 0067, 0649
    [SerializeField]
    private BiologEntryButtonPressed _buttonPressed;
    [SerializeField]
    private Button _button;
    [SerializeField]
    private Text _elementLabel;
    [SerializeField]
    private BiologEntry _entry;
#pragma warning restore 0067, 0649

    public BiologEntry Entry
    {
        get { return _entry; }
        set
        {
            _entry = value;
            _elementLabel.text = _entry.EntryName;
        }
    }

    public BiologEntryButtonPressed ButtonPressed
    {
        get { return _buttonPressed; }
    }

    public Button EntryButton
    {
        get { return _button; }
    }

    void OnEnable()
    {
        _button.onClick.AddListener(onClickHandler);
    }

    void OnDisable()
    {
        _button.onClick.RemoveListener(onClickHandler);
    }

    void onClickHandler()
    {
        _buttonPressed.Invoke(this);
    }
}

