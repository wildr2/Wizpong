using UnityEngine;
using System.Collections;

public class Ball : MonoBehaviour 
{
    protected Racquet controlling_racquet; 


    public void Start()
    {
        Initialize();

        // BUG FIX - bug in which the collider will not move with the ball when the ball is parented to an empty gameobject...
        collider2D.enabled = false;
        collider2D.enabled = true;
    }
    public virtual void Initialize()
    {
        controlling_racquet = null;
    }
    
    public virtual void TakeControl(Racquet racquet)
    {
        controlling_racquet = racquet;
    }
}
