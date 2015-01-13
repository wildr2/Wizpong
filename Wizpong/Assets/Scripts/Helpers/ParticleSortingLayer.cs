using UnityEngine;
using System.Collections;

public class ParticleSortingLayer : MonoBehaviour
{
    public string layer = "default";
    public int order = 0;

    public void Start()
    {
        // Set the sorting layer of the particle system.
        particleSystem.renderer.sortingLayerName = layer;
        particleSystem.renderer.sortingOrder = order;
    }


}
