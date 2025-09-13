using System.Collections.Generic;
using UnityEngine;

public class GenericObjectPool<T> : MonoBehaviour where T : Component
{
    [SerializeField]
    protected T prefab;
    protected Queue<T> objectsQueue = new Queue<T>();

    protected T Get()
    {
        if (objectsQueue.Count == 0)
            AddObjects(10);
        return objectsQueue.Dequeue();
    }

    protected void ReturnToPool(T objectToReturn)
    {
        objectToReturn.gameObject.SetActive(false);
        objectToReturn.transform.position = Vector3.zero;
        objectToReturn.transform.rotation = Quaternion.identity;
        objectsQueue.Enqueue(objectToReturn);
    }

    protected virtual void AddObjects(int count)
    {
        for (int i = 0; i < count; i++)
        {
            var newObject = Instantiate(
                original: prefab,
                parent: transform);
            newObject.transform.position = Vector3.zero;
            newObject.transform.rotation = Quaternion.identity;
            newObject.gameObject.SetActive(false);
            objectsQueue.Enqueue(newObject);
        }
    }

    protected IEnumerable<T> Each()
    {
        while (objectsQueue.Count > 0)
            yield return objectsQueue.Dequeue();
    }

}