using Meta.XR.MRUtilityKit;
using System;
using System.Collections;
using System.Collections.Generic;
using UnityEditor;
using UnityEngine;

public class SeedCollider : MonoBehaviour
{
    [SerializeField] private Seed seed;
    private void OnCollisionEnter(Collision collision)
    {
        Debug.Log($"Seed has collided with {collision.gameObject.name}");

        if(seed.isGrowable == true)
        {
            if(collision.transform.parent.gameObject.GetComponent<MRUKAnchor>() != null)
            {
                Debug.Log($"Seed has collided with room object, contact length is {collision.contacts.Length}");
                if (collision.contacts.Length > 0)
                {
                    seed.isGrowable = false;
                    seed.rigidBody.velocity = Vector3.zero;
                    seed.rigidBody.angularVelocity = Vector3.zero;
                    seed.rigidBody.isKinematic = true;

                    ContactPoint contact = collision.contacts[0];

                    seed.GrowFlower(contact.point, contact.normal);
                }
            }
        }
    }
}
