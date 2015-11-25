using UnityEngine;
using System.Collections;
using System.Collections.Generic;

public class Pool {

    private List<GameObject> pool;
    private Poolable initial;

    public Pool(Poolable init) {
        pool = new List<GameObject>();
        initial = init;
    }

    public GameObject getInactive()
    {
        GameObject retour = null;
        foreach (GameObject obj in pool)
        {
            if(!obj.activeInHierarchy)
            {
                retour = obj;
                obj.SetActive(true);
                break;
            }
        }
        if(retour == null)
        {
            retour = initial.copy();
            pool.Add(retour);
            retour.SetActive(true);
        }
        return retour;
    }

}
