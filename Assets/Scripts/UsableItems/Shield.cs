using System.Collections;
using System.Collections.Generic;
using Unity.Properties;
using UnityEngine;

public class Shield : UsableItem
{
    protected override bool CanUse(Player player)
    {
        return !player.IsShield && !player.IsTrapped;
    }

    protected override void Effect(Player player)
    {
        Debug.Log(player.gameObject.name + " is using Shield!");
        player.IsShield = true;
    }
}
