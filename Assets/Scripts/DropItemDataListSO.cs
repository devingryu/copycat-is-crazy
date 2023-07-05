using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[CreateAssetMenu(fileName = "DropItemDataListSO", menuName = "ScriptableObject/DropItemDataListSO")]
public class DropItemDataListSO : ScriptableObject
{
    public List<DropItemDataSO> list;
}