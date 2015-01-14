using UnityEngine;
using System.Collections;

public class PopUp : MonoBehaviour
{

	public void OnEnable()
    {
        UIAudio.Instance.PlayAlert();
    }
}
