using UnityEngine;

public class Tile : MonoBehaviour
{
    public bool IsEmpty; 

    [SerializeField] private SpriteRenderer spriteRenderer;

    private Color defColor;

    #region a* pathfinding
    
    public int x { get; set; }
    public int y { get; set; }
    public int gCost { get; set; }
    public int hCost { get; set; }
    public int fCost { get; set; }

    public Tile cameFromTile;

    public void CalculateFCost()
    {
        fCost = hCost + gCost;
    }
    
    #endregion
    
    private void Start()
    {
        IsEmpty = true;
    }
    
    public void SetEmpty(bool status)
    {
        IsEmpty = status;
    }

    public void Init(int x, int y, Color defaultColor)
    {
        defColor = defaultColor;
        gameObject.name = $"Tile {x} x {y}";
        spriteRenderer.color = defaultColor;
        this.x = x;
        this.y = y;
    }

    public void HighlightTile()
    {
        spriteRenderer.color = Color.green;
    }

    public void DehighlightTile()
    {
        spriteRenderer.color = defColor;
    }




}