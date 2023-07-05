using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class RollerDropItem : MonoBehaviour, IDropItem
{
    public void CollectItem(Player player)
    {
        player.speed = Mathf.Clamp(player.speed + 1, 1, player.speedMax);
        Delete();
    }

    public void Delete()
    {
        Destroy(gameObject);
    }
}
