using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Pool;
using System.Linq;

public class ObjectPool : MonoBehaviour
{

    public static List<ObjectPoolInfo> ObjectPools = new List<ObjectPoolInfo>();

    private static GameObject poolObjectsEmptyHolder;
    //private static GameObject gameObjectEmptyHolder;

    //string poolType;

    private void Awake()
    {
        SetUpEmptyPoolHolder();

    }

    private void SetUpEmptyPoolHolder()
    {
        poolObjectsEmptyHolder = new GameObject("Pooled Objects");
        //gameObjectEmptyHolder = new GameObject("Pooled Game Objects");
        //gameObjectEmptyHolder.transform.SetParent(poolObjectsEmptyHolder.transform);
    }

    public static GameObject SpawnObject(GameObject objectToSpawn, Vector3 spawnPosition, Quaternion spawnRotation)
    {

        //Look for an object of the same name in the list and get it
        ObjectPoolInfo pool = ObjectPools.Find(p => p.lookUpString == objectToSpawn.name);

        //ObjectPoolInfo pool = null;
        //foreach(ObjectPoolInfo poolObject in ObjectPools)
        //{
        //    if (poolObject.lookUpString == objectToSpawn.name)
        //    {
        //        pool = poolObject;
        //    }
        //}

        //If the object does not exist then create one and add it to the pool
        if (pool == null)
        {

            pool = new ObjectPoolInfo() { lookUpString = objectToSpawn.name };
            ObjectPools.Add(pool);
        }

        //Look for and get a inactive gameobject
        GameObject spawnableObject = pool.InactiveGameObject.FirstOrDefault();

        //GameObject spawnableObject = null;
        //foreach(GameObject obj in pool.InactiveGameObject)
        //{
        //    if(obj != null)
        //    {
        //        spawnableObject = obj;
        //        break;
        //    }
        //}


        //If fail to get the object then make a new one
        if (spawnableObject == null)
        {
            spawnableObject = Instantiate(objectToSpawn, spawnPosition, spawnRotation);
            spawnableObject.transform.SetParent(poolObjectsEmptyHolder.transform);
        }
        else
        {
            //If got object then enable it
            //spawnableObject.transform.position = spawnPosition;
            //spawnableObject.transform.rotation = spawnRotation;
            spawnableObject.transform.SetPositionAndRotation(spawnPosition, spawnRotation);
            pool.InactiveGameObject.Remove(spawnableObject);
            spawnableObject.SetActive(true);
        }

        return spawnableObject;
    }

    public static void ReturnObjectToPool(GameObject obj)
    {
        //exclude the " (clone)" part out of the pass-in GameObject's name
        string goName = obj.name.Substring(0, obj.name.Length - 7);

        //ObjectPoolInfo pool = null;
        //foreach (ObjectPoolInfo poolObject in ObjectPools)
        //{
        //    if (poolObject.lookUpString == goName)
        //    {
        //        pool = poolObject;
        //    }
        //}

        //Look for the gameobject of the same name
        ObjectPoolInfo pool = ObjectPools.Find(p => p.lookUpString == goName);

        if (pool == null)
        {
            Debug.LogWarning("Trying to release a unpooled object: " + obj.name);
        }
        else
        {
            obj.SetActive(false);
            pool.InactiveGameObject.Add(obj);
        }
    }

    //private static GameObject SetParentObject(string poolType)
    //{
    //    switch (poolType)
    //    {
    //        case "GameObject":
    //            return gameObjectEmptyHolder;
    //        case "None":
    //            return null;
    //        default:
    //            return null;
    //    }
    //}
}

public class ObjectPoolInfo
{
    public string lookUpString;
    public List<GameObject> InactiveGameObject = new List<GameObject>();
}

