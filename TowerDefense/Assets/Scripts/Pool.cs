using UnityEngine;
using System.Collections;
using System.Collections.Generic;

public class Pool {

    private List<Poolable> pool;
    private Poolable initial;

    public Pool(Poolable init) {
        pool = new List<Poolable>();
        initial = init;
    }

    public GameObject getInactive()
    {
        GameObject retour = null;
        foreach (Poolable obj in pool)
        {
            if(!(obj.gameObject).activeInHierarchy)
            {
                retour = obj.gameObject;
                obj.Reset();
                obj.gameObject.SetActive(true);
                break;
            }
        }
        if(retour == null)
        {
            retour = initial.copy();
            pool.Add(retour.GetComponent<Poolable>());
            retour.SetActive(true);
        }
        return retour;
    }

}
