using UnityEngine;

public class ControlOptionsManager
{
    public enum ControlType
    {
        Touch,
        Tilt
    }


    private static ControlType currentControlType = ControlType.Touch; // Default control type

    public static ControlType GetControlType()
    {
        return currentControlType;
    }

    public static void SetControlType(int controlType)
    {
        currentControlType = (ControlType)controlType;
    }
    
}
