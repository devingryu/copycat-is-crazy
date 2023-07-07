using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Wall : MonoBehaviour
{
    void Start()
    {
        TileManager.Instance.WorldToCell(transform.position).cellObject = CellObject.Wall;
    }
}
