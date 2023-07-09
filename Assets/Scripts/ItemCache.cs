using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class ItemCache : MonoBehaviour
{
    public static ItemCache Instance { get; private set; }
    [SerializeField] List<DropItem> dropItemPrefabs;
    public ShieldTimer shieldTimerPrefab;
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
}
