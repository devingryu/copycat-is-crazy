using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public enum DropItemId { Flask, }

public class ItemCache : MonoBehaviour
{
    public static ItemCache Instance { get; private set; }
    DropItemDataListSO dropItemList;
    [HideInInspector] public UsableItemDataListSO usableItemList;



    private void Awake()
    {
        Instance = this;
        dropItemList = Resources.Load<DropItemDataListSO>(typeof(DropItemDataListSO).Name);
        usableItemList = Resources.Load<UsableItemDataListSO>(typeof(UsableItemDataListSO).Name);
    }

    public void SpawnRandomDropItem(Vector3 position)
    {
        int randomIndex = Random.Range(0, dropItemList.list.Count);
        Instantiate(dropItemList.list[randomIndex].prefab, position, Quaternion.identity);
    }
}
