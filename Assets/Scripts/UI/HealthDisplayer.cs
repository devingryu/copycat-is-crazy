using System.Collections;
using System.Collections.Generic;
using Unity.VisualScripting;
using UnityEngine;

public class HealthDisplayer : MonoBehaviour
{
    [SerializeField] Player player;
    [SerializeField] RectTransform heartImageTemplate;
    float offsetX = 55;
    List<RectTransform> heartTransforms = new List<RectTransform>();

    private void Awake()
    {
        heartImageTemplate.gameObject.SetActive(false);
    }

    private void Start()
    {
        player.OnDamage += TargetPlayer_OnDamage;
        Init();
    }

    private void TargetPlayer_OnDamage(object sender, System.EventArgs e)
    {
        int amount = player.Health;
        if(amount < heartTransforms.Count)
        {
            // heart destroy
            while(amount != heartTransforms.Count)
            {
                RectTransform target = heartTransforms[heartTransforms.Count - 1];
                heartTransforms.RemoveAt(heartTransforms.Count - 1);
                Destroy(target.gameObject);
            }
        }
        else
        {
            //heart increase
            while(amount != heartTransforms.Count)
            {
                RectTransform spawnedHeart = Instantiate(heartImageTemplate, transform);
                heartTransforms.Add(spawnedHeart);
                spawnedHeart.anchoredPosition = new Vector2(offsetX * heartTransforms.Count - 1, 0);
                spawnedHeart.gameObject.SetActive(true);
            }
        }
    }

    private void Init()
    {
        int amount = player.Health;
        for(int i = 0;i<amount;++i)
        {
            RectTransform spawnedHeart = Instantiate(heartImageTemplate, transform);
            heartTransforms.Add(spawnedHeart);
            spawnedHeart.anchoredPosition = new Vector2(offsetX * i, 0);
            spawnedHeart.gameObject.SetActive(true);
        }
    }
}
