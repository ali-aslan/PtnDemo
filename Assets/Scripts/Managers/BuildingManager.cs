using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Events;

public class BuildingManager : BaseSingleton<BuildingManager>
{
    [SerializeField] private SpriteRenderer BuildingSpriteRenderer;
    private Building currentbuilding;
    public BuildingState State { get; set; }

    private Color frameColor;

    private Transform spawnPoint;
    private bool spawnPointSelection;


    public void Start()
    {
        State = BuildingState.PreBuilding;
        EventManager.BarracksSpawning.AddListener(BarracksSelected);

    }

    public void BuildBuilding(string buildingName)
    {
        var build = DataManager.Instance.Getbuilding(buildingName);

        if (currentbuilding == null || currentbuilding.Name != buildingName)
        {
            currentbuilding = build;
            State = BuildingState.OnBuilding;
            BuildingSpriteRenderer.sprite = build.Sprite;
            BuildingSpriteRenderer.transform.localScale = new Vector3(build.SizeX, build.SizeY, 1);
        }
        else if (currentbuilding.Name == buildingName)
            DeselectBuilding();

        UIManager.Instance.UpdateDataBuilding(buildingName,build.Health);


    }

    public void DeselectBuilding()
    {
        State = BuildingState.PreBuilding;
        spawnPointSelection = false;
        BuildingSpriteRenderer.sprite = null;
        currentbuilding = null;
        UIManager.Instance.InformationArea.SetActive(false);

    }

    private void Update()
    {
        if (State == BuildingState.OnBuilding)
        {
            int posX = Mathf.RoundToInt(CameraManager.Instance.mouseCursorPos.x);
            int posY = Mathf.RoundToInt(CameraManager.Instance.mouseCursorPos.y);

            BuildingSpriteRenderer.transform.position = new Vector2(posX + currentbuilding.SizeX / 2f - 0.5f, posY + currentbuilding.SizeY / 2f - 0.5f);

            if (!TileControl(posX, posY, currentbuilding))
            {
                frameColor = Color.red;
                frameColor.a = 0.35f;
                BuildingSpriteRenderer.color = frameColor;
            }  
            else
            {
                frameColor = Color.green;
                frameColor.a = 0.35f;
                BuildingSpriteRenderer.color = frameColor;

                if (Input.GetMouseButtonDown(0) && CameraManager.Instance.dragging && !CameraManager.Instance.PointerOnUi)
                {
                    BuildObject(posX, posY);
                }

            }

       

        }

        //for spawn point change
        if(spawnPointSelection && Input.GetMouseButtonDown(1))
        {
            var intXPos = Mathf.RoundToInt(CameraManager.Instance.mouseCursorPos.x);
            var intYPos = Mathf.RoundToInt(CameraManager.Instance.mouseCursorPos.y);

            var tile = GridManager.Instance.GetTile(intXPos, intYPos);
            if (tile && tile.IsEmpty)
            {
                spawnPoint.position = tile.transform.position;
                spawnPoint.gameObject.SetActive(true);
            }

        }

    }
    private void BarracksSelected(Barracks selectedBarracks)
    {
        spawnPoint = selectedBarracks.SpawnPoint;
        spawnPointSelection = true;

        TroopManager.Instance.SoldierDeselect();
        UIManager.Instance.SetSpawnData(selectedBarracks.Spawnables,selectedBarracks);
    }

    private void BuildObject(int posX, int posY)
    {
        GameObject obj = BuildingFactory.GetBuilding(currentbuilding.Name);
        Instantiate(obj, new Vector2(posX, posY), Quaternion.identity);

        for (int x = 0; x < currentbuilding.SizeX; x++)
            for (int y = 0; y < currentbuilding.SizeY; y++)
            {
                Tile tile = GridManager.Instance.GetTile(posX + x, posY + y);
                tile.SetEmpty(false);
            }

        DeselectBuilding();

    }

    public void OnDisable()
    {
        EventManager.BarracksSpawning.RemoveListener(BarracksSelected);
    }


    private bool TileControl(int posX, int posY, Building build) // we are checking grids if the field is full return false
    {
        for (int x = 0; x < build.SizeX; x++)
            for (int y = 0; y < build.SizeY; y++)
            {
                var tile = GridManager.Instance.GetTile(posX + x, posY + y);
                if (!tile)
                    return false;

                if (tile.IsEmpty)
                    continue;
                else
                    return false;
            }

        return true;
    }

}
public enum BuildingState
{
    PreBuilding = 0,
    OnBuilding = 1,
    PostBuilding = 2,
}
