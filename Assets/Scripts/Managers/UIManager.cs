using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using DG.Tweening;

public class UIManager : BaseSingleton<UIManager> // Singleton  
{
    [SerializeField] public GameObject InformationArea;
    [SerializeField] public GameObject ProductionArea;
    [SerializeField] public Text Header;
    [SerializeField] public Text Description;
    [SerializeField] public Text Health;
    [SerializeField] public Image Image;
    [SerializeField] public GameObject UnitChoose;

    private List<Spawnable> Troop;
    private Barracks SpawnBarracks;

    private bool productionHide=false;
    private bool InformationHide=false;


    //For Update UI data  

    public void UpdateDataBuilding(string name,int health)
    {
        InformationArea.SetActive(true);
        var build = DataManager.Instance.Getbuilding(name);

        Header.text = build.Name;
        Description.text = build.Description;
        Image.sprite = build.Sprite;

        Health.text = "Health:" + health.ToString();

        if (build.Production)
            UnitChoose.SetActive(true);
        else
            UnitChoose.SetActive(false);
    }
    public void UpdateDataTroop(string name, int health)
    {
        var troop = DataManager.Instance.GetSoldier(name);
        Header.text = troop.Name;
        Description.text = troop.Description;
        Image.sprite = troop.Sprite;

        Health.text = "Health:" + health.ToString();
        UnitChoose.SetActive(false);
        InformationArea.SetActive(true);
    }


    //For spawn 
    public void SetSpawnData(List<Spawnable> troop,Barracks spawnBarracks)
    {
       this.SpawnBarracks = spawnBarracks;
       this.Troop = troop;
    }

    public void SpawnUnit(int unitName)
    {
        
        if (!SpawnBarracks) return;

        Vector3 spawnPoint = SpawnBarracks.SpawnPoint.position; //orj spawn point 
        Tile spawnTile = GridManager.Instance.GetClosestTile(SpawnBarracks.SpriteRenderer.transform.position);  
        Tile targetTile = GridManager.Instance.GetClosestTile(spawnPoint);

        if (!spawnTile || !spawnTile.IsEmpty) return; //if spawn tile is fill or null dont do anything 

        GameObject SoldierObject = PoolManager.Instance.GetPoolObject((PoolObjectType)unitName); // take one soldier from pool
        SoldierController SoldierScript = SoldierObject.GetComponent<SoldierController>();

        //UIManager.Instance.UpdateDataTroop(SoldierScript.ObjectName, SoldierScript.Health);

        SoldierScript.transform.position = spawnTile.transform.position - new Vector3(0.5f, 0.5f, 0f);
        SoldierScript.OnTile = spawnTile;
        SoldierObject.SetActive(true);

        if (!targetTile || !targetTile.IsEmpty) return;
        SoldierScript.Move(targetTile);    //Send to unit spawning area 
        targetTile.SetEmpty(false);        //make that tile filled
    }

    public void HideProduction()
    {
        if(productionHide)
        ProductionArea.transform.DOLocalMoveX(-520f, 2.5f);//-520 orj //-760
        else
        ProductionArea.transform.DOLocalMoveX(-760f, 2.5f);//-520 orj //-760

        productionHide = !productionHide;

    }

    public void HideInformation()
    {
        if (InformationHide)
            InformationArea.transform.DOLocalMoveX(527.5f, 2.5f);//527.5 orj //752.5
        else
            InformationArea.transform.DOLocalMoveX(752.5f, 2.5f);//-520 orj //-760
         
        InformationHide = !InformationHide;

    }

    public void Close() => Application.Quit();





}
