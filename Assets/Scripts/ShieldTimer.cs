using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class ShieldTimer : MonoBehaviour
{
    public float timer = 5;
    Player player;

    private void Start()
    {
        player = GetComponentInParent<Player>();
    }

    private void Update()
    {
        timer -= Time.deltaTime;
        if(timer < 0)
        {
            player.IsShield = false;
            Destroy(gameObject);
        }
    }
}
