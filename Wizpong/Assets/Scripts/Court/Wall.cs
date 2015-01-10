using UnityEngine;
using System.Collections;

public class Wall : MonoBehaviour
{
    public Material neutral, touched, live;
    
    private Color fade_color_i;
    private float fade = 0;
    private const float fade_speed = 0.2f;


    public void SetColorNeutral()
    {
        renderer.material = neutral;
        StopCoroutine("FadeColortoNeutralCoRoutine");
    }
    public void SetColorTouched()
    {
        renderer.material = touched;
        StopCoroutine("FadeColortoNeutralCoRoutine");
    }
    public void SetColorLive(Color c)
    {
        renderer.material = live;
        live.color = c;
        StopCoroutine("FadeColortoNeutralCoRoutine");
    }
    public void FadeColortoNeutral()
    {
        fade_color_i = renderer.material.color;
        fade = 0;
        StartCoroutine("FadeColortoNeutralCoRoutine");
    }

    private IEnumerator FadeColortoNeutralCoRoutine()
    {
        while (true)
        {
            fade += Time.deltaTime * fade_speed;
            fade = Mathf.Min(1, fade);

            Color c = renderer.material.color;
            c = Color.Lerp(fade_color_i, neutral.color, Mathf.Sqrt(fade));
            renderer.material.color = c;

            if (fade >= 1) break;

            yield return null;
        }
    }
    
}
