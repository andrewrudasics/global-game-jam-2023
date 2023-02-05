using UnityEditor;
using UnityEngine;

public class DebugMenu : MonoBehaviour
{
    // Add a menu item named "Do Something" to MyMenu in the menu bar.
    [MenuItem("Debug / Perform Rectangular Attack")]
    static void DoSomething()
    {
        AbilityManager.Instance.PerformRectangularAttack(0, new Vector2(0, 0), 1, 2, new Vector2(1, 0), 10);
    }
}