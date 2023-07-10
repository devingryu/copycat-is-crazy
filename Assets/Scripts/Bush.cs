using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Bush : MonoBehaviour
{
    Cell standingCell;

    private void Start()
    {
        standingCell = TileManager.Instance.WorldToCell(transform.position);
        standingCell.OnCellAttacked += Bush_OnCellAttacked;
        standingCell.cellObject = CellObject.Bush;
    }

    private void Bush_OnCellAttacked(object sender, Cell.OnCellAttackedArgs e)
    {
        Delete();
    }

    private void Delete()
    {
        Destroy(gameObject);
        standingCell.cellObject = CellObject.Nothing;
        standingCell.OnCellAttacked -= Bush_OnCellAttacked;
    }
}
