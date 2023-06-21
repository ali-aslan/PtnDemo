using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class TroopManager : BaseSingleton<TroopManager>
{
    private SoldierController orderTaker;
    private bool haveOrderTaker;

    public Dictionary<Vector3, List<GameObject>> TargetToMissile = new Dictionary<Vector3, List<GameObject>>();

    public void OnEnable()
    {
        EventManager.AttackToObject.AddListener(SoldierAttack);
        EventManager.SoldierInformation.AddListener(SoldierInformation);
        EventManager.TargerDestroyed.AddListener(TargerDestroyed);
        EventManager.MissileHit.AddListener(MissileHit);

    }

    private void SoldierInformation(SoldierController soldiercontroller)
    {
        orderTaker = soldiercontroller;
        haveOrderTaker = true;
        BuildingManager.Instance.DeselectBuilding();
        UIManager.Instance.UpdateDataTroop(soldiercontroller.ObjectName, soldiercontroller.Health);
    }

    public void SoldierDeselect()
    {
        haveOrderTaker = false;
        orderTaker = null;
    }


    private void SoldierAttack(Vector3 targetPoint, IHealth targetObject)
    {
        if (haveOrderTaker)
            orderTaker.Attack(targetPoint, targetObject);
    }


    private void Update()
    {
        if (haveOrderTaker && Input.GetMouseButtonDown(1))
        {
            var intXPos = Mathf.RoundToInt(CameraManager.Instance.mouseCursorPos.x);
            var intYPos = Mathf.RoundToInt(CameraManager.Instance.mouseCursorPos.y);

            var tile = GridManager.Instance.GetTile(intXPos, intYPos);
            if (tile.IsEmpty)
            {
                orderTaker.StopAttack();
                orderTaker.Move(tile);
            }
        }
    }

    public void RangedAmmo(int attackPow, IHealth targetHealth, PoolObjectType type, Vector3 launchPos,Vector3 targetPos)
    {
        GameObject RangedObject = PoolManager.Instance.GetPoolObject(type);
        AmmoController RangedScript = RangedObject.GetComponent<AmmoController>();
        RangedScript.AttackPow = attackPow;
        RangedScript.TargetIhealth = targetHealth;
        RangedScript.TargetPos = targetPos;


        RangedObject.transform.position = launchPos;
        RangedObject.SetActive(true);

        if (!TargetToMissile.ContainsKey(targetPos)) //listing all missile 
        {
            TargetToMissile.Add(targetPos,new List<GameObject> {RangedObject});
        }
        else
        {
            TargetToMissile[targetPos].Add(RangedObject);
        }
       
    }

    private void TargerDestroyed(Vector3 targetPos)
    {
        //if(TargetToMissile.ContainsKey(targetPos))
        //{
        //    foreach (GameObject GOMissile in TargetToMissile[targetPos])
        //    {
        //        PoolManager.Instance.ReturnObject(GOMissile, GOMissile.GetComponent<AmmoController>().AmmoObjectType);
        //    }
        //}
        TargetToMissile.Remove(targetPos);
    }

    public void MissileHit(GameObject missile)
    {

        foreach (var item in TargetToMissile.Values)
        {
            if (item.Contains(missile))
                item.Remove(missile);
        }

    }

    private void OnDisable()
    {
        EventManager.AttackToObject.RemoveListener(SoldierAttack);
        EventManager.SoldierInformation.RemoveListener(SoldierInformation);
        EventManager.TargerDestroyed.RemoveListener(TargerDestroyed);
        EventManager.MissileHit.RemoveListener(MissileHit);
    }

}
