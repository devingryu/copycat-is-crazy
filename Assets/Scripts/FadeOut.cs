using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class FadeOut : MonoBehaviour
{
    [SerializeField] Image panel;
    float time = 0f;
    float F_time = 0.3f;

    public void FadeOUT()
    {
        if (panel == null) return;
        panel.enabled = true;
        panel.color = new Color(panel.color.r, panel.color.g, panel.color.b, 0);
        StartCoroutine(Fadeout());
    }
    IEnumerator Fadeout()
    {
        time = 0f;
        Color alpha = panel.color;
        while (alpha.a < 1f)
        {
            time += Time.deltaTime / F_time;
            alpha.a = Mathf.Lerp(0, 1, time);
            panel.color = alpha;
            yield return null;
        }
    }
}
