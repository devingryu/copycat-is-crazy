using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Wall : MonoBehaviour
{
    [SerializeField] bool isBreakable;
    Cell standingCell;
    private void Start()
    {
        if(isBreakable)
        {
            standingCell = TileManager.Instance.WorldToCell(transform.position);
            standingCell.cellObject |= CellObject.Wall;
            standingCell.OnCellAttacked += BreakableWall_OnCellAttacked;
        }
        else
        {
            TileManager.Instance.WorldToCell(transform.position).cellObject |= (CellObject.Unbreakable | CellObject.Wall);

        }
    }

    private void BreakableWall_OnCellAttacked(object sender, Cell.OnCellAttackedArgs e)
    {
        Delete();
    }

    private void Delete()
    {
        Destroy(gameObject);
        standingCell.OnCellAttacked -= BreakableWall_OnCellAttacked;
        standingCell.cellObject &= (~(CellObject.Wall | CellObject.Unbreakable));
    }
}
