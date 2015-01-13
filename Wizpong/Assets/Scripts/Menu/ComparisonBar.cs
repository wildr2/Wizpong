using UnityEngine;
using System.Collections;
using UnityEngine.UI;

public class ComparisonBar : MonoBehaviour 
{
    public float width = 500;
    public RectTransform left_bar, right_bar;
    public Text left_text, right_text;

    public void Start()
    {
        SetWidths(0.5f);
    }
    public void Set(string left_text, string right_text, float left_pct)
    {
        // set text
        this.left_text.text = left_text;
        this.right_text.text = right_text;

        // set widths
        SetWidths(left_pct);
    }

    public void SetColors(Color left_color, Color right_color)
    {
        Image left_image = left_bar.GetComponent<Image>();
        Image right_image = right_bar.GetComponent<Image>();

        left_image.color = left_color;
        right_image.color = right_color;
    }

    private void SetWidths(float left_pct)
    {
        left_bar.sizeDelta = new Vector2((1 - left_pct) * -width, 20);
        right_bar.sizeDelta = new Vector2(left_pct * -width, 20);
    }
}
