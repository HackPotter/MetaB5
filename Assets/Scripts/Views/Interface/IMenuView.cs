using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

public interface IMenuView
{
    event Action PlayButtonPressed;
    event Action LoginButtonPressed;
    event Action ExitButtonPressed;
}

