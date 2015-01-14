using UnityEngine;
using System.Collections;
using System.Collections.Generic;

public class PooledObject : MonoBehaviour
{
    private LinkedList<PooledObject> parent_list;

    public void OnDisable()
    {
        // finished objects are put to the end of their ObjectPool list
        parent_list.Remove(this);
        parent_list.AddLast(this);
    }

    public void SetParentListReference(LinkedList<PooledObject> list)
    {
        parent_list = list;
    }
}
