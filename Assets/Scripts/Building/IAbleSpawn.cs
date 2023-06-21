using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public interface IAbleSpawn  
{
    public List<Spawnable> Spawnables { get; set; }
    public Transform SpawnPoint { get; set; }
    public bool SpawnPointSet { get; set; }

}
