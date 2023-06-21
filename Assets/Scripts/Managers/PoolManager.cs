using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PoolManager : BaseSingleton<PoolManager>
{
    [SerializeField] private List<PoolSpecs> PoolList;




    public void FillPoolStart()
    {
        for (int i = 0; i < PoolList.Count; i++)
            FillPool(PoolList[i]);
    }
    private void FillPool(PoolSpecs poolInfo)
    {
        for (int i = 0; i < poolInfo.HowMany; i++)
        {
            var poolObj = Instantiate(poolInfo.Prefab, poolInfo.Bucket);
            poolObj.SetActive(false);
            poolInfo.Pool.Add(poolObj);
        }
    }

    public GameObject GetPoolObject(PoolObjectType poolObjectType)
    {
        var selected = PoolList.Find(self => self.Type == poolObjectType);
        var pool = selected.Pool;
        GameObject ins = null;

        if (pool.Count <= 0)
            ins = Instantiate(selected.Prefab, selected.Bucket);

        for (int i = 0; i < pool.Count; i++)
        {
            if (!pool[i].activeInHierarchy)
            {
                ins = pool[i];
                pool.Remove(ins);
                break;
            }

        }

        return ins;
    }

    public void ReturnObject(GameObject obj, PoolObjectType type)
    {
        obj.SetActive(false);

        PoolSpecs selected = PoolList.Find(self => self.Type == (PoolObjectType)type);
        var pool = selected.Pool;

        if (!pool.Contains(obj))
        {
            pool.Add(obj);
        }

    }



}


public enum PoolObjectType
{
    CloseRange=0,       //Close range Ammo 
    SoldierOne = 1,     // Rocket Launcher 
    SoldierTwo = 2,     // Patrol
    SoldierThree = 3,   // Mortar
    SoldierFour = 4,    // Special Forces

    Rocket = 5,
    Mortar= 6

}

[System.Serializable]
public class PoolSpecs
{
    public PoolObjectType Type;
    public int HowMany = 0;
    public GameObject Prefab;
    public Transform Bucket;

    public List<GameObject> Pool = new List<GameObject>();
}