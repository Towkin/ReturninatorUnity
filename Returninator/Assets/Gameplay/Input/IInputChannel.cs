using UnityEngine;
using System.Collections;

namespace Returninator.Gameplay
{
    public interface IInputChannel
    {
        InputChange GetInputChange();
    }
}