using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class ObjectPoolManager : SingletonMono<ObjectPoolManager>
{
    public List<GameObject> objectPools = new();

    public GameObject GetObject(string name, GameObject prefab)
    {
        GameObject objectPoolItem = objectPools.Find(_ => _.name.Contains(name) && !_.activeInHierarchy);
        if (objectPoolItem == null)
        {
            GameObject newObj = Instantiate(prefab, transform.position, Quaternion.identity);
            newObj.name = name;
            objectPools.Add(newObj);
            return newObj;
        }
        objectPoolItem.SetActive(true);
        return objectPoolItem;
    }

    public void ResetItem()
    {
        objectPools.ForEach(obj => { 
            obj.SetActive(false);
            obj.transform.position = transform.position;
        });
    }
}
