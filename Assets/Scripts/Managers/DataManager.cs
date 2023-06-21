using System.Collections;
using System.Collections.Generic;
using System.Linq;
using UnityEngine;
using UnityEngine.Events;

[System.Serializable]
public class TroopList
{
    public List<Troop> Troops;
}

[System.Serializable]
public class BuildingList
{
    public List<Building> Buildings;
}

public class DataManager : BaseSingleton<DataManager>
{
    public TroopList troopList;
    public BuildingList buildingList;

    public List<GameObject> BuildingPrefabs;

    private void Start()
    {
        List<Sprite>  BuildingSprites = Resources.LoadAll<Sprite>("BuildingSprites").ToList();
        List<Sprite> TroopSprites = Resources.LoadAll<Sprite>("TroopSprites").ToList();        
        
        BuildingPrefabs = Resources.LoadAll<GameObject>("BuildingPrefabs").ToList();
        //List<Sprite> TroopPrefabs = Resources.LoadAll<Sprite>("TroopPrefabs").ToList();

        TextAsset TroopData = (TextAsset)Resources.Load("TroopsData");
        TextAsset BuildingData = (TextAsset)Resources.Load("BuildingsData");

        troopList = JsonUtility.FromJson<TroopList>(TroopData.text);
        buildingList = JsonUtility.FromJson<BuildingList>(BuildingData.text);

        //load sprites
        foreach (var item in TroopSprites)
        {
            if (troopList.Troops.Find(sel => sel.Name == item.name) == null)
                continue;

            var Item = troopList.Troops.Find(sel => sel.Name == item.name);
            Item.Sprite = item;
        }

        foreach (var item in BuildingSprites)
        {
            if (buildingList.Buildings.Find(sel => sel.Name == item.name) == null)
                continue;

            var Item = buildingList.Buildings.Find(sel => sel.Name == item.name);
            Item.Sprite = item;
        }

        foreach (var item in BuildingPrefabs)
        {
            if (buildingList.Buildings.Find(sel => sel.Name == item.name) == null)
                continue;

            var Item = buildingList.Buildings.Find(sel => sel.Name == item.name);
            Item.BuildingPrefab = item;
        }
        PoolManager.Instance.FillPoolStart();

    } 
    
  

    public Building Getbuilding(string buildingName)
    {
        return buildingList.Buildings.Find(sel => sel.Name == buildingName);
    }

    public Troop GetSoldier(string troopName)
    {
        return troopList.Troops.Find(sel => sel.Name == troopName);
    }




}


