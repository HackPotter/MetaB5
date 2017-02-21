using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace Metablast.UI
{
    public interface IHudView : IScreen
    {
        event Action BiologButtonPressed;
        event Action OptionsButtonPressed;
        event Action ResumeGameButtonPressed;
        event Action ExitGameButtonPressed;

        IContextMessageView ContextMessageView { get; }
        ITransmissionView TransmissionView { get; }
        IObjectiveView ObjectiveFrameView { get; }
    }
}