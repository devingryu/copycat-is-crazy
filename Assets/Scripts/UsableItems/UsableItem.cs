using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public abstract class UsableItem
{
    int amount = 0;

    public void Use(Player player)
    {
        if (amount == 0 || !CanUse(player))
            return;
        --amount;
        Effect(player);
    }

    public void SetAmount(int amount)
    {
        this.amount = amount;
    }

    public int GetAmount()
    { 
        return amount; 
    }

    protected abstract bool CanUse(Player player);
    protected abstract void Effect(Player player);
    public abstract Sprite GetSprite();
}