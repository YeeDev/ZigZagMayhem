using System.Collections.Generic;
using UnityEngine;

public class BulletPooler : MonoBehaviour
{
    [SerializeField] int objectLimit = 0;
    [SerializeField] int startingObjects = 0;
    [SerializeField] bool createsMoreObjects = false;
    [SerializeField] Transform parent = null;
    [SerializeField] GameObject objectType = null;

    Queue<GameObject> objectQueue = new Queue<GameObject>();

    private void Awake() { CreateInitialObjects(); }

    private void CreateInitialObjects()
    {
        for (int i = 0; i < startingObjects; i++)
        {
            if (objectQueue.Count >= objectLimit) { break; }

            CreateNewObject();
        }
    }

    private GameObject CreateNewObject()
    {
        GameObject poolObject = Instantiate(objectType);
        Bullet bullet = poolObject.GetComponent<Bullet>();

        bullet.SetPooler = this;
        poolObject.SetActive(false);

        if (parent != null) { poolObject.transform.parent = parent; }

        return poolObject;
    }

    public GameObject GetObject()
    {
        if (createsMoreObjects && objectQueue.Count <= 0) { return CreateNewObject(); }
        else if(objectQueue.Count <= 0) { return null; }

        return objectQueue.Dequeue();
    }

    public void EnqueueObject(GameObject objectToEnqueue) { objectQueue.Enqueue(objectToEnqueue); }
}