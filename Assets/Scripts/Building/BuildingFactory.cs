using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public static class BuildingFactory 
{
    public static GameObject GetBuilding(string buildingName)
    {
        switch (buildingName)
        {
            case ObjectName.BarracksName:
                var a = DataManager.Instance.Getbuilding(ObjectName.BarracksName).BuildingPrefab;
                return a;     
            case ObjectName.OilProductsName:
                return DataManager.Instance.Getbuilding(ObjectName.OilProductsName).BuildingPrefab;     
            case ObjectName.OilWellName:
                return DataManager.Instance.Getbuilding(ObjectName.OilWellName).BuildingPrefab;       
            case ObjectName.RefineryName:
                return DataManager.Instance.Getbuilding(ObjectName.RefineryName).BuildingPrefab;     
            case ObjectName.SunPanelsName:
                return DataManager.Instance.Getbuilding(ObjectName.SunPanelsName).BuildingPrefab;
            default:
                return null;

        }

    }

}


