using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class GameManager : Singleton<GameManager>
{
    private SeedObjectPool _seedObjectPool;
    public SeedObjectPool seedObjectPool { get { return _seedObjectPool; } }

    private void Awake()
    {
        CreateSeedObjPool();
    }

    private void CreateSeedObjPool()
    {
        if(_seedObjectPool != null)
        {
            Destroy(_seedObjectPool.gameObject);
            _seedObjectPool = null;
        }

        GameObject seedPoolObject = new GameObject("SeedContainer");
        seedPoolObject.transform.SetParent(this.transform);

        _seedObjectPool = seedPoolObject.AddComponent<SeedObjectPool>();
    }
}
