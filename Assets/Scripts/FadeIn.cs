using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class FadeIn : MonoBehaviour
{
    [SerializeField] Image panel;
    float time = 0f;
    float F_time = 1f;

    private void Awake()
    {
        panel.enabled = false;
    }

    public void FadeIN()
    {
        if (panel == null) return;
        panel.enabled = true;
        StartCoroutine(Fadein());
    }
    IEnumerator Fadein()
    {
        yield return new WaitForSeconds(0.2f);
        time = 0f;
        Color alpha = panel.color;
        while(alpha.a > 0f)
        {
            time += Time.deltaTime / F_time;
            alpha.a = Mathf.Lerp(1, 0, time);
            panel.color = alpha;
            yield return null;
        }
        panel.enabled = false;
    }
}
