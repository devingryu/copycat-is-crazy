using System;
using System.Collections;
using System.Collections.Generic;
using UnityEditor;
using UnityEngine;

public class Cell
{
    public event EventHandler<OnCellAttackedArgs> OnCellAttacked;

    public class OnCellAttackedArgs : EventArgs
    {
        public Vector3Int position;
    }

    public Vector3Int cellPos;
    public Vector3 worldPos;
    public bool objectOnCell;
    bool isAttacked;
    public bool IsAttacked
    {
        get { return isAttacked; }
        set
        {

            if(value)
            {
                OnCellAttacked?.Invoke(this, new OnCellAttackedArgs { position = cellPos });
                Collider2D[] colliderArray = Physics2D.OverlapPointAll(worldPos);

                foreach(Collider2D collider in colliderArray)
                {
                    if(collider.TryGetComponent<Box>(out Box box))
                        box.Delete();
                    else if(collider.TryGetComponent<Balloon>(out Balloon balloon))
                        balloon.Explode();
                    else if(collider.TryGetComponent<IDropItem>(out IDropItem dropItem))
                        dropItem.Delete();
                }


            }
            else
            {

            }
        }
    }
}
