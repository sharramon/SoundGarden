using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class SeedObjectPool : GenericObjectPool<Seed>
{
    public static SeedObjectPool instance { get; private set; }
    // Start is called before the first frame update
    private void Awake()
    {
        instance = this;
    }
    void Start()
    {
        prefab = ResourceManager.instance.resourceScriptable.seed;
    }

    public Seed Spawn(Transform _targetTransform, Vector3 offset)
    {
        Seed b = Get();
        b.transform.position = _targetTransform.position + offset;
        Utils.LookAtInXZPlane(b.transform, PlayerInputHandler.instance.mainCamera.transform);

        b.gameObject.SetActive(true);
        return b;
    }
    public Seed Spawn()
    {
        Seed b = Get();
        b.gameObject.SetActive(true);
        return b;
    }
    public void Return(Seed _seed)
    {
        ReturnToPool(_seed);

    }
}
