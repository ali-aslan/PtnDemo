using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[System.Serializable]
public class Building 
{

    public string Name;
    public int Health;
    public int SizeX;
    public int SizeY;
    public bool Production;

    [TextArea]
    public string Description;
    public Sprite Sprite;


    public GameObject BuildingPrefab;

}



