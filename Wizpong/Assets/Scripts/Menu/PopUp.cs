using UnityEngine;
using System.Collections;

public class PopUp : MonoBehaviour
{
    private UIAudio audio;

    public void Awake()
    {
        audio = Object.FindObjectOfType<UIAudio>();
        if (audio == null) Debug.LogError("Missing UIAudio");
    }

	public void OnEnable()
    {
        audio.PlayAlert();
    }
}
