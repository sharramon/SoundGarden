using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class FlowerMicTrigger : MonoBehaviour
{
    private Seed seed;

    private void OnTriggerEnter(Collider other)
    {
        Debug.Log($"collider triggered by object {other.gameObject.name}");

        if (other.CompareTag("Seed"))
        {
            Debug.Log("Seed entered the mic trigger.");

            other.transform.parent.gameObject.TryGetComponent<Seed>(out seed);

            if(seed == null || seed.isGrowable == true)
            {
                Debug.LogError($"there is no seed in seed trigger object {other.gameObject.name}");
                return;
            }

            seed.SetMic(this.transform);
        }
    }
}
