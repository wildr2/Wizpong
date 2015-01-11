using UnityEngine;
using System.Collections;
using UnityEngine.UI;

[RequireComponent(typeof(Image))]
public class UITransitionFade : UITransition
{
    private Image image;
    private float alpha_initial;
    public float offset = 0;

    public void Awake()
    {
        image = GetComponent<Image>();
        alpha_initial = image.color.a;
    }
    public override void UpdateTransition(float transition, bool going_in)
    {
        float t = 1 - Mathf.Pow(1 - transition, 4);

        // offset
        if (going_in)
        {
            t = Mathf.Max(0, (t - offset) / (1 - offset));
        }
        else
        {
            t = 1 - Mathf.Max(0, ((1 - t) - offset) / (1 - offset));
        }

        // set alpha
        Color c = image.color;
        c.a = alpha_initial * t;
        image.color = c;
    }

}
