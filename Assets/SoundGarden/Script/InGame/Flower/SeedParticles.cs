using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class SeedParticles : MonoBehaviour
{
    [SerializeField] private GameObject idleParticle;
    [SerializeField] private GameObject chargeParticle;
    [SerializeField] private GameObject explosionParticle;

    public void TurnAllOff()
    {
        idleParticle.SetActive(false);
        chargeParticle.SetActive(false);
        explosionParticle.SetActive(false);
    }

    public void SetIdleParticle(bool isOn)
    {
        idleParticle.SetActive(isOn);
    }

    public void SetChargeParticle(bool isOn)
    {
        chargeParticle.SetActive(isOn);
    }

    public void SetExplosionParticle(bool isOn)
    {
        explosionParticle.SetActive(isOn);
    }
}
