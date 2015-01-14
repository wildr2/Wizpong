using UnityEngine;
using System.Collections;
using System.Collections.Generic;

public class ObjectPool : MonoBehaviour
{
    private static ObjectPool _instance;
    public static ObjectPool Instance
    {
        get
        {
            if (_instance == null)
            {
                _instance = PooledObject.FindObjectOfType<ObjectPool>();

                if (_instance == null) Debug.LogError("Missing ObjectPool");
            }
            return _instance;
        }
    }

    private Dictionary<string, LinkedList<PooledObject>> objects = new Dictionary<string, LinkedList<PooledObject>>();


    public void Awake()
    {
        _instance = this;
    }

    public void RequestObjects(PooledObject prefab, int count, bool stack)
    {
        if (prefab == null)
        {
            Debug.LogError("Attempt to request pooling objects based on a null prefab.");
            return;
        }


        // get / make the list of specified objects
        LinkedList<PooledObject> list;

        if (objects.ContainsKey(prefab.name))
        {
            list = objects[prefab.name];
        }
        else
        {
            list = new LinkedList<PooledObject>();
            objects.Add(prefab.name, list);
        }

        // Instantiate objects as specified
        int initial_count = list.Count;
        while (list.Count < count || (stack && list.Count < initial_count + count))
        {
            PooledObject obj = (PooledObject)Instantiate(prefab);
            obj.transform.parent = transform;
            list.AddFirst(obj);
            obj.SetParentListReference(list);
            obj.gameObject.SetActive(false);
        }
    }
    public PooledObject GetPooledObject(PooledObject prefab, bool active_only)
    {
        // find the list of objects specified
        if (!objects.ContainsKey(prefab.name))
        {
            Debug.LogError("There are no objects of name " + prefab.name + " in the ObjectPool");
            return null;
        }
        
        LinkedList<PooledObject> list = objects[prefab.name];
        if (list.Count == 0)
        {
            Debug.Log("No objects of name " + prefab.name + " have been added to their list in the ObjectPool.");
            return null;
        }


        // find an object not in use (not active)
        // any non active object will have been put at the back of the list
        PooledObject obj = list.Last.Value;
        if (!obj.gameObject.activeInHierarchy || !active_only)
        {
            // object is available or we can take it anyway (not active_only)
            list.Remove(obj);
            list.AddFirst(obj);
            obj.gameObject.SetActive(true);
            return obj;
        }
        else return null;
    }
}