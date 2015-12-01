using UnityEngine;
using System.Collections;
using System.Collections.Generic;

public class Grid  {

    private Dictionary<Vector3, bool> table;

    public Grid()
    {
        table = new Dictionary<Vector3, bool>();
    }

	public Vector3 GetCellCenter(Vector3 pos)
    {
        return new Vector3(Mathf.FloorToInt(pos.x) + 0.5f, 0, Mathf.FloorToInt(pos.z) + 0.5f);
    }

    public bool IsFree(Vector3 p)
    {
        if(table.ContainsKey(p))
            return table[p];
        return true;
    }

    public void Take(Vector3 p)
    {
            table[p] = false;
    }

    public void Free(Vector3 p)
    {
        table[p] = true;
    }
}
