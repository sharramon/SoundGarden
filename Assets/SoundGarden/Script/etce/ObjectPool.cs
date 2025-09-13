using System.Collections.Generic;
using Unity.Mathematics;
using UnityEngine;

public class ObjectPool : Singleton<ObjectPool>
{
    public GameObject seedPrefab;
    public int poolSize = 10;

    private List<GameObject> pool;

    void Start()
    {
        pool = new List<GameObject>();

        for (int i = 0; i < poolSize; i++)
        {
            GameObject obj = Instantiate(seedPrefab);
            obj.SetActive(false);
            pool.Add(obj);
        }
    }

    public GameObject GetObject(Vector3 _pos)
    {
        foreach (GameObject obj in pool)
        {
            if (!obj.activeInHierarchy)
            {
                obj.transform.position = _pos;
                obj.transform.rotation = quaternion.identity;
                obj.SetActive(true);
                return obj;
            }
        }

        GameObject newObj = Instantiate(seedPrefab);
         newObj = Instantiate(seedPrefab);
         
         newObj.transform.position = _pos;
         newObj.transform.rotation = quaternion.identity;
         
        pool.Add(newObj);
        return newObj;
    }

    public void ReturnObject(GameObject obj)
    {
        obj.SetActive(false);
    }
}