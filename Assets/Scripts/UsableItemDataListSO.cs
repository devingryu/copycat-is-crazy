using System.Collections;
using System.Collections.Generic;
using UnityEngine;
[CreateAssetMenu(fileName = "UsableItemDataListSO", menuName = "ScriptableObject/UsableItemDataListSO")]
public class UsableItemDataListSO : ScriptableObject
{
    public List<UsableItemDataSO> list;
}
