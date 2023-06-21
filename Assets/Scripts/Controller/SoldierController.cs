using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class SoldierController : Spawnable, IHealth, IDamage
{
    public int Health { get; set; }
    public int AttackPow { get; set; }
    public float AttackRate { get; set; }
    public float Range { get; set; }

    public Vector3 TargetPos { get; set; }
    public Tile OnTile { get; set; }

    private List<Tile> Path;
    private bool IsMoving;
    private bool Attacking;

    public void OnEnable()
    {
        var specs = DataManager.Instance.GetSoldier(ObjectName);

        SpriteRenderer = GetComponentInChildren<SpriteRenderer>();
        Health = specs.Health;
        AttackPow = specs.AttackPow;
        AttackRate = specs.AttackSpeed;
        SpriteRenderer.sprite = specs.Sprite;
        Range = specs.Range;
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

    protected override void OnMouseDown() // GridObject  virtual void
    {
        if(BuildingManager.Instance.State == BuildingState.PreBuilding && !CameraManager.Instance.PointerOnUi)
        {
            base.OnMouseDown();
            EventManager.SoldierInformation.Invoke(this);
        }
    }

    private void OnMouseOver() //Soldier attack soldier
    {
        if (BuildingManager.Instance.State == BuildingState.PreBuilding && !CameraManager.Instance.PointerOnUi)
        {
            if (Input.GetMouseButtonDown(1)) // right click
                EventManager.AttackToObject.Invoke(transform.position, this);
        }
    }

    public void Attack(Vector3 attackingPos, IHealth attackingUnit)
    {
        TargetPos = attackingPos;
       
     
        if(Vector3.Distance(attackingPos, transform.position) >= Range)// if soldier to away from target get closest attack pos only for ranged soldier
        {
            attackingPos = new Vector3(attackingPos.x - Range/2, attackingPos.y - Range/2); // i know its wrong but i dont have time for this 

            var movingTile = GridManager.Instance.GetClosestTile(attackingPos);
            Move(movingTile);
            Attacking = true;
            StartCoroutine(StartAttacking(attackingUnit));

        }
        else //soldier near from target start attack only for ranged soldier
        {
            Attacking = true;
            StartCoroutine(StartAttacking(attackingUnit));
        }
    }

    public void StopAttack() => Attacking = false;
    public void GiveDamage(int damage, IHealth target) => target.TakeDamage(damage);



    public virtual void ToDisappear()
    {
        OnTile.SetEmpty(true);
        PoolManager.Instance.ReturnObject(gameObject, PoolObjectType);
    }

    public void Move(Tile targetTile)
    {
        if (IsMoving) return;
        Path = GridManager.Instance.FindPath(OnTile.x, OnTile.y, targetTile.x, targetTile.y);
        if (Path == null) return;
        IsMoving = true;

        foreach (var item in Path)
        {
            item.HighlightTile();
        }

        StartCoroutine(StartMoving());

        
    }

    IEnumerator StartMoving()
    {
        OnTile.SetEmpty(true);
        OnTile = Path[Path.Count - 1];
        OnTile.SetEmpty(false);
        var cellOffset = GridManager.Instance.CellSize / 2f;
        while (Path.Count > 0)
        {
            var moveDuration = 0.25f;
            for (float t = 0; t < moveDuration; t += Time.deltaTime)
            {
                transform.position = Vector3.Lerp(transform.position, Path[0].transform.position - new Vector3(cellOffset, cellOffset, 0f), t / moveDuration);
                Path[(int)t].DehighlightTile();
                yield return null;
            }
            Path.RemoveAt(0);
            yield return null;
        }

        IsMoving = false;
    }

    IEnumerator StartAttacking(IHealth attackingUnit)
    {
        while (IsMoving)
        {
            yield return null;
        }
        while (attackingUnit as Object && Attacking && !IsMoving)
        {
            if (Range == 0)
                GiveDamage(AttackPow, attackingUnit);
            else
                TroopManager.Instance.RangedAmmo(AttackPow, attackingUnit,AmmoObjectType,transform.position,TargetPos);
            if (attackingUnit.Health <= 0)
            {
                Attacking = false;
                yield break;
            }

            yield return new WaitForSeconds(AttackRate);
        }
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
