using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public enum UsableItemName { Can, Needle, Shield }
public enum VehicleName { Turtle, Pirate }
public class ItemCache : MonoBehaviour
{
    public static ItemCache Instance { get; private set; }
    [SerializeField] List<DropItem> dropItemPrefabs;
    [SerializeField] List<UsableItemDataSO> usableItemData;
    [SerializeField] List<AnimatorOverrideController> vehicleAnimList;
    public Balloon balloonPrefab;

    private void Awake()
    {
        if (Instance == null)
            Instance = this;
        else
        {
            if (Instance != this)
                Destroy(gameObject);
        }
    }

    public void SpawnRandomDropItem(Vector3 position)
    {
        int randomIndex = Random.Range(0, dropItemPrefabs.Count);
        Instantiate(dropItemPrefabs[randomIndex], position, Quaternion.identity);
    }

    public UsableItemDataSO GetUsableItemData(UsableItemName name)
    {
        return usableItemData[(int)name];
    }

    public AnimatorOverrideController GetVehicleController(VehicleName name)
    {
        return vehicleAnimList[(int)name];
    }
}
