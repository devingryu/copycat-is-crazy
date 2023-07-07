using System;
using System.Collections;
using System.Collections.Generic;
using UnityEditor;
using UnityEngine;

public enum CellObject { Nothing, Box, Balloon, Wall, DropItem, }
public class Cell
{
    public event EventHandler<OnCellAttackedArgs> OnCellAttacked;

    public class OnCellAttackedArgs : EventArgs
    {
        public Vector3Int position;
    }

    public Vector3Int cellPos;
    public Vector3 worldPos;
    public CellObject cellObject = CellObject.Nothing;
    public void Attack()
    {
        OnCellAttacked?.Invoke(this, new OnCellAttackedArgs { position = cellPos });
    }
}
