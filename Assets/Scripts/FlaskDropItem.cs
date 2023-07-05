using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class FlaskDropItem : MonoBehaviour, IDropItem
{
    public void CollectItem(Player player)
    {
        player.balloonRange = Mathf.Clamp(player.balloonRange + 1, 1, player.balloonRangeMax);
        Delete();
    }

    public void Delete()
    {
        Destroy(gameObject);
    }
}
