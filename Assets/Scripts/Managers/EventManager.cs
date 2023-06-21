using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Events;

public static class EventManager 
{
    public static UnityEvent<Vector3, IHealth> AttackToObject = new UnityEvent<Vector3, IHealth>();

    public static UnityEvent<Vector3> TargerDestroyed = new UnityEvent<Vector3>();
    public static UnityEvent<GameObject> MissileHit = new UnityEvent<GameObject>();


    public static UnityEvent<BuildingController> BuildingInformation = new UnityEvent<BuildingController>();
    public static UnityEvent<SoldierController> SoldierInformation = new UnityEvent<SoldierController>();

    public static UnityEvent<Barracks> BarracksSpawning = new UnityEvent<Barracks>();

 

}
