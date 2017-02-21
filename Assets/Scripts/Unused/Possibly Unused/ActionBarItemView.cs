using Squid;

public class ActionBarItemView : Control
{
    private ActionBarItem _item;

    private ImageControl _imageIcon;
    private Button _button;

    public ActionBarItemView(ActionBarItem item)
    {
        _item = item;

        _imageIcon = new ImageControl();
        _imageIcon.Dock = DockStyle.Fill;
        _imageIcon.NoEvents = true;

        _button = new Button();
        _button.Dock = DockStyle.Fill;
        _button.Style = "";

        NoEvents = true;

        Elements.Add(_imageIcon);
        Elements.Add(_button);

        Tooltip = _item.Tool.ToolName;
    }
}

