using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace Metablast.UI
{
    public interface IBiologView : IScreen
    {
        event Action ExitButtonPressed;

        bool IsShowing { get; }

        void SetTint(float tint);
    }
}