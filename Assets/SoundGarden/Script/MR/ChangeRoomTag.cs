using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using Meta.XR.MRUtilityKit;

public class ChangeRoomTag : MonoBehaviour
{
    [SerializeField] private string RoomTag = "Room";
    public void ChangeAllTags()
    {
        // Find all objects in the scene with the MRUKAnchor component
        MRUKAnchor[] anchors = FindObjectsOfType<MRUKAnchor>();

        Debug.Log($"number of anchors found is {anchors.Length}");

        foreach (MRUKAnchor anchor in anchors)
        {
            // Change the tag of the object with the MRUKAnchor component
            ChangeTagRecursively(anchor.gameObject, RoomTag);
        }
    }

    private void ChangeTagRecursively(GameObject obj, string tag)
    {
        // Change the tag of the current object
        obj.tag = tag;

        // Recursively change the tags of all children
        foreach (Transform child in obj.transform)
        {
            ChangeTagRecursively(child.gameObject, tag);
        }
    }
}
