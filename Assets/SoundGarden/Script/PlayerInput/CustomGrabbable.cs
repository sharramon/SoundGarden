using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using Oculus.Interaction;

public class CustomGrabbable : Grabbable
{
    public void ForceUngrab()
    {
        // Ensure we simulate unselect events for all current grab points
        List<PointerEvent> currentGrabPoints = GetCurrentGrabPoints();
        foreach (var point in currentGrabPoints)
        {
            PointerEvent unselectEvent = new PointerEvent(point.Identifier, PointerEventType.Unselect, point.Pose, point.Data);
            ProcessPointerEvent(unselectEvent);
        }
    }

    // Method to get the current grab points
    private List<PointerEvent> GetCurrentGrabPoints()
    {
        List<PointerEvent> grabPoints = new List<PointerEvent>();
        for (int i = 0; i < _selectingPointIds.Count; i++)
        {
            grabPoints.Add(new PointerEvent(_selectingPointIds[i], PointerEventType.Select, _selectingPoints[i], null));
        }
        return grabPoints;
    }
}
