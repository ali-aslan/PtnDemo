using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.EventSystems;

public class BuildingController : GridObject, IHealth
{
    public int Health{ get; set; }

    private int SizeX;
    private int SizeY;

    public virtual void Start()
    {
       var specs = DataManager.Instance.Getbuilding(ObjectName);
       Health = specs.Health;
       SizeX = specs.SizeX;
       SizeY = specs.SizeY;
       SpriteRenderer = GetComponentInChildren<SpriteRenderer>();
       SpriteRenderer.sprite = specs.Sprite;
    }

    private void OnMouseOver() //for attack soldier to building
    {
        if (BuildingManager.Instance.State == BuildingState.PreBuilding && !CameraManager.Instance.PointerOnUi)
        {
            if (Input.GetMouseButtonDown(1))
            {
                EventManager.AttackToObject.Invoke(transform.position, this);
            }
        }
    }

    protected override void OnMouseDown() // for inform update information 
    {

        if (BuildingManager.Instance.State == BuildingState.PreBuilding && !CameraManager.Instance.PointerOnUi)
        {
            if (Input.GetMouseButtonDown(0))
            {
                base.OnMouseDown();
                TroopManager.Instance.SoldierDeselect();
                string name = this.name.Remove(this.name.Length - 7);
                UIManager.Instance.UpdateDataBuilding(name, this.Health);
            }
        }
  
    }

    public virtual void TakeDamage(int damage)
    {
        Health = Health - damage;
        if (Health <= 0)
        {
            ToDisappear();
            return;
        }

        StartCoroutine(TakeDamageInterval());

    }
    public virtual void ToDisappear()
    {
        Vector3 transformPos = transform.position;
        for (int x = 0; x < SizeX; x++)
            for (int y = 0; y < SizeY; y++)
            {
                Tile tile = GridManager.Instance.GetTile(x + (int)transformPos.x, y + (int)transformPos.y);
                tile.SetEmpty(true);

            }

        EventManager.TargerDestroyed.Invoke(gameObject.transform.position);
        Destroy(gameObject);
    }

    IEnumerator TakeDamageInterval()
    {
        var color = SpriteRenderer.color;
        var orjColorBlue = color.b;
        var orjColorGreen = color.g;
        color.b = 0f;
        color.g = 0f;
        SpriteRenderer.color = color;
        yield return new WaitForSeconds(0.3f);
        color.b = orjColorBlue;
        color.g = orjColorGreen;
        SpriteRenderer.color = color;
    }


}
