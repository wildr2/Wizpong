using UnityEngine;
using System.Collections;

public class CourtEffects : MonoBehaviour
{
    // Court color
    public SpriteRenderer court_background;
    private Color court_color_default;
    private Color court_color_flash = Color.black;
    private float court_flash_timer = 0, court_flash_timer_max = 1.2f;

    // Confetti
    public ParticleSystem confetti;


    // PUBLIC MODIFIERS

    public void Awake()
    {
        court_color_default = court_background.color;
    }

    public void FlashCourtColor(Color color)
    {
        //court_flash_timer = 0;
        court_color_flash = color;
        StartCoroutine("UpdateFlashCourt");
    }
    public void EnableConfetti(Color color)
    {
        confetti.renderer.material.SetColor("_EmisColor", color);
        confetti.Play();
    }


    // PRIVATE MODIFIERS

    private IEnumerator UpdateFlashCourt()
    {
        float t = 0;
        while (t < 1)
        {
            t += Time.unscaledDeltaTime;
            if (t >= 1)
            {
                court_background.color = court_color_default;
                break;
            }

            court_background.color = Color.Lerp(court_color_flash, court_color_default, Mathf.Pow(t, 4));
            yield return null;
        }
    }
}
