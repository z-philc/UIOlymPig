using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[System.Serializable]
public class ObjectPoolItem
{
    public int amountToPool;
    public GameObject objectToPool;
    public bool shouldExpand = true;
}

public class ObjectPooler : MonoBehaviour
{
    public static ObjectPooler instance;
    public List<ObjectPoolItem> itemsToPool;
    public List<GameObject> lsPooledObjects;

    private void Awake()
    {
        instance = this;
    }
    // Use this for initialization
    void Start()
    {
        lsPooledObjects = new List<GameObject>();
        foreach (ObjectPoolItem item in itemsToPool)
        {
            for (int i = 0; i < item.amountToPool; i++)
            {
                GameObject obj = (GameObject)Instantiate(item.objectToPool);
                obj.transform.SetParent(transform);
                obj.SetActive(false);
                lsPooledObjects.Add(obj);
            }
        }

        Debug.LogError(lsPooledObjects[0].activeInHierarchy);
    }

    public GameObject GetPooledObject(string tag)
    {
        for (int i = 0; i < lsPooledObjects.Count; i++)
        {
            if (!lsPooledObjects[i].activeInHierarchy && lsPooledObjects[i].tag == tag)
            {
                return lsPooledObjects[i];
            }
        }
        foreach (ObjectPoolItem item in itemsToPool)
        {
            if (item.objectToPool.tag == tag)
            {
                if (item.shouldExpand)
                {
                    GameObject obj = (GameObject)Instantiate(item.objectToPool);
                    obj.SetActive(false);
                    obj.transform.SetParent(transform);
                    lsPooledObjects.Add(obj);
                    return obj;
                }
            }
        }
        return null;
    }

}
