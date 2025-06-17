using UnityEngine;

namespace _2._Scripts.Events
{
    [CreateAssetMenu(fileName = "TwoIntegerEvent", menuName = "Scriptable Objects/Events/Two Integer Event", order = 0)]
    public class TwoIntegerEvent : TwoGenericEvent<int, int>
    {
    }
}