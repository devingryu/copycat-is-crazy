using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class ShieldTimer : MonoBehaviour
{
    const float shieldTime = 5;
    public float timer;
    Player player;

    private void Awake()
    {
        timer = shieldTime;
        player = GetComponentInParent<Player>();
        gameObject.SetActive(false);
    }

    private void Update()
    {
        timer -= Time.deltaTime;
        if(timer < 0)
        {
            player.IsShield = false;
            gameObject.SetActive(false);
        }
    }

    private void OnEnable()
    {
        timer = shieldTime;
    }
}
