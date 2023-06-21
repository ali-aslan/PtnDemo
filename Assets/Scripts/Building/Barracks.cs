using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Events;

public class Barracks : BuildingController
{

    public List<Spawnable> Spawnables;
    public Transform SpawnPoint;

    public bool SpawnPointSet { get; set; }

    protected override void OnMouseDown()
    {
        if (BuildingManager.Instance.State == BuildingState.PreBuilding && !CameraManager.Instance.PointerOnUi)
        {
           
            base.OnMouseDown();
            EventManager.BarracksSpawning.Invoke(this);
        }
    }

}
