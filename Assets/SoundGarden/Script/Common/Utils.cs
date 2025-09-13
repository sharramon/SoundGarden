using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Utils : MonoBehaviour
{
    public static void ClearAllAnimationTriggers(Animator animator)
    {
        foreach (var parameter in animator.parameters)
        {
            if (parameter.type == AnimatorControllerParameterType.Trigger)
            {
                animator.ResetTrigger(parameter.name);
            }
        }
    }

    /// <summary>
    /// use is look at true / false in case the z forward of object actually faces away
    /// </summary>
    public static void LookAtInXZPlane(Transform mainTransform, Transform targetTransform, bool isLookAt = true)
    {
        Vector3 targetPos = new Vector3(targetTransform.position.x, mainTransform.position.y, targetTransform.position.z);
        mainTransform.LookAt(targetPos, Vector3.up);

        if (!isLookAt)
        {
            // Rotate 180 degrees around the Y axis to look away if isLookAt is false
            mainTransform.eulerAngles = new Vector3(mainTransform.eulerAngles.x, mainTransform.eulerAngles.y + 180, mainTransform.eulerAngles.z);
        }
    }
}