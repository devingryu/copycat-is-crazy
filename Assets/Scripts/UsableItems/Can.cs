using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Can : UsableItem
{
    protected override bool CanUse(Player player)
    {
        return player.HasTurtle();
    }

    protected override void Effect(Player player)
    {
        player.UpgradeTurtle();
    }
}
