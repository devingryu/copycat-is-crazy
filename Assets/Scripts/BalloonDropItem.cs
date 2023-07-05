using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class BalloonDropItem : MonoBehaviour, IDropItem
{
    public void CollectItem(Player player)
    {
        player.balloonNumberMax++;
        player.balloonNumber++;
        Delete();
    }
    public void Delete()
    {
        Destroy(gameObject);
    }
}
