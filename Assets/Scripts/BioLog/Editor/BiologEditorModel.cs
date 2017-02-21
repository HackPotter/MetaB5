using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using UnityEditor;

public class BiologEditorModel
{
    public event Action<BiologEntry> SelectedEntryChanged;


    private EditorWindow _window;
    private BiologEntry _selectedEntry;

    public void Repaint()
    {
        _window.Repaint();
    }

    public BiologEditorModel(EditorWindow window, HashSet<string> allTags)
    {
        _window = window;
        AllTags = allTags;
    }

    public HashSet<string> AllTags
    {
        get;
        private set;
    }

    public BiologEntry SelectedEntry
    {
        get
        {
            return _selectedEntry;
        }
        set
        {
            _selectedEntry = value;
            if (SelectedEntryChanged != null)
            {
                SelectedEntryChanged(value);
            }
        }
    }

}
