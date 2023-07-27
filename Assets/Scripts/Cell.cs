using System;
using UnityEngine;

[Flags]
public enum CellObject 
{ 
    Nothing = 0, 
    Box = 1, 
    Bush = 4, 
    Balloon = 8, 
    Spike = 16,
    DropItem = 32,
    Wall = 64,
    Unbreakable = 128,
}
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

    public Spike spike = null;
}
