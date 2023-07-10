using System.Collections;
using System.Collections.Generic;
using Unity.PlasticSCM.Editor.WebApi;
using UnityEngine;

public class DropItem : MonoBehaviour
{
    public Stat stat;
    Cell currentCell;

    private void Awake()
    {
        currentCell = TileManager.Instance.WorldToCell(transform.position);
        currentCell.cellObject = CellObject.Breakable;
        currentCell.OnCellAttacked += DropItem_OnCellAttacked;
    }

    private void DropItem_OnCellAttacked(object sender, Cell.OnCellAttackedArgs e)
    {
        Delete();
    }

    public void Delete()
    {
        currentCell.cellObject = CellObject.Nothing;
        currentCell.OnCellAttacked -= DropItem_OnCellAttacked;
        Destroy(gameObject);
    }
}
