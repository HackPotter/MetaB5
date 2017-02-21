using Squid;
using UnityEngine;

public class ObjectiveFrameView : IObjectiveView
{
    private IUserObjectives _activeObjectives;

    private Frame _objectiveListFrame;

    public ObjectiveFrameView(Control desktop)
    {
        _objectiveListFrame = desktop.GetControl("ObjectiveListFrameContents") as Frame;
        _objectiveListFrame.Controls.Clear();
        _objectiveListFrame.PerformLayout();

        ActiveObjectives = GameContext.Instance.Player.CurrentObjectives;
    }

    public IUserObjectives ActiveObjectives
    {
        get
        {
            return _activeObjectives;
        }
        set
        {
            _activeObjectives = value;

            _activeObjectives.UserObjectiveAdded += new UserObjectiveAdded(_activeObjectives_UserObjectiveAdded);
            _activeObjectives.UserObjectiveTaskAdded += new UserObjectiveTaskAdded(_activeObjectives_UserObjectiveTaskAdded);
            _activeObjectives.UserObjectiveRemoved += new UserObjectiveRemoved(_activeObjectives_UserObjectiveRemoved);
            //_activeObjectives.UserObjectiveCompleted += new UserObjectiveCompleted(_activeObjectives_UserObjectiveCompleted);
        }
    }

    void _activeObjectives_UserObjectiveTaskAdded(GameplayObjective objective, ObjectiveTask newTask)
    {
        Frame objectiveFrame = _objectiveListFrame.Controls.Find((c) => objective.Equals(c.Tag)) as Frame;
        if (objectiveFrame == null)
        {
            DebugFormatter.LogError(this, "Could not find frame for objective {0}. Failed to update objective interface",objective.Name);
            return;
        }
        Label newTaskLabel = CreateTaskLabel(newTask);
        objectiveFrame.Controls.Add(newTaskLabel);
        objectiveFrame.PerformLayout();
        _objectiveListFrame.PerformLayout();
        newTask.TaskCompleted += (t) => task_TaskCompleted(t, newTaskLabel);
    }

    void _activeObjectives_UserObjectiveCompleted(GameplayObjective objective)
    {
        Control toRemove = _objectiveListFrame.Controls.Find((c) => objective.Equals(c.Tag));
        _objectiveListFrame.Controls.Remove(toRemove);
        _objectiveListFrame.PerformLayout();
    }

    void _activeObjectives_UserObjectiveRemoved(GameplayObjective objective)
    {
        Control toRemove = _objectiveListFrame.Controls.Find((c) => objective.Equals(c.Tag));
        _objectiveListFrame.Controls.Remove(toRemove);
        _objectiveListFrame.PerformLayout();
    }

    void _activeObjectives_UserObjectiveAdded(GameplayObjective objective)
    {
        Control objectiveFrame = CreateObjectiveFrame(objective);
        
        _objectiveListFrame.Controls.Add(objectiveFrame);
        objectiveFrame.PerformLayout();
        _objectiveListFrame.PerformLayout();
    }

    private Control CreateObjectiveFrame(GameplayObjective objective)
    {
        Frame objectiveFrame = new Frame();
        
        objectiveFrame.Tag = objective;
        objectiveFrame.Style = "Button - Transparent";
        objectiveFrame.Margin = new Margin(0, 0, 0, 0);
        objectiveFrame.Padding = new Margin(0, 0, 0, 0);
        objectiveFrame.Dock = DockStyle.Top;
        objectiveFrame.AutoSize = AutoSize.Vertical;
        //objectiveFrame.Tint = Squid.ColorInt.RGB(53,95,73);

        Label nameLabel = new Label();
        nameLabel.NoEvents = true;
        nameLabel.Style = "Label - Objective Widget";
        nameLabel.Dock = DockStyle.Top;
        nameLabel.AutoSize = AutoSize.Vertical;
        nameLabel.Margin = new Margin(2, 3, 0, 6);
        nameLabel.Padding = new Margin(4, 4, 4, 4);
        nameLabel.Text = objective.Name;

        objectiveFrame.Controls.Add(nameLabel);

        foreach (var task in objective.Tasks)
        {
            Label taskLabel = CreateTaskLabel(task);
            objectiveFrame.Controls.Add(taskLabel);

            task.TaskCompleted += (t) => task_TaskCompleted(t,taskLabel);
        }

        return objectiveFrame;
    }

    private Label CreateTaskLabel(ObjectiveTask task)
    {
        Label taskLabel = new Label();
        taskLabel.TextWrap = true;
        taskLabel.Style = "Label - Objective Task Widget";
        taskLabel.NoEvents = true;
        taskLabel.Dock = DockStyle.Top;
        taskLabel.AutoSize = AutoSize.Vertical;
        taskLabel.Margin = new Margin(15, 1, 0, 5);
        taskLabel.Padding = new Margin(4, 4, 4, 4);
        taskLabel.Tag = task;
        taskLabel.Text = task.Name;

        return taskLabel;
    }

    void task_TaskCompleted(ObjectiveTask task, Label tasklabel)
    {
        tasklabel.UseTextColor = true;
        tasklabel.TextColor = ColorInt.RGB(35, 200, 35);
    }
}

