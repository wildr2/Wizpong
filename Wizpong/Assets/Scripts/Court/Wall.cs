using UnityEngine;
using System.Collections;

public class Wall : MonoBehaviour
{
    public Material neutral, touched, live;
    
    public void SetColorNeutral()
    {
        renderer.material = neutral;
    }
    public void SetColorTouched()
    {
        renderer.material = touched;
    }
    public void SetColorLive(Color c)
    {
        renderer.material = live;
        live.color = c;
    }
    
}
