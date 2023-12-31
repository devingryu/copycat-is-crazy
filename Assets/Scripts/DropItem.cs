using UnityEngine;

public class DropItem : MonoBehaviour
{
    public Stat stat;
    Cell currentCell;

    private void Awake()
    {
        currentCell = TileManager.Instance.WorldToCell(transform.position);
        currentCell.cellObject |= CellObject.DropItem;
        currentCell.OnCellAttacked += DropItem_OnCellAttacked;
    }

    private void DropItem_OnCellAttacked(object sender, Cell.OnCellAttackedArgs e)
    {
        Delete();
    }

    public void Delete()
    {
        currentCell.cellObject &= (~CellObject.DropItem);
        currentCell.OnCellAttacked -= DropItem_OnCellAttacked;
        Destroy(gameObject);
    }
}
