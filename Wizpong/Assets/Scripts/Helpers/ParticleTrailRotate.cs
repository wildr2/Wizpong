using UnityEngine;
using System.Collections;


[RequireComponent(typeof(ParticleSystem))]
public class ParticleTrailRotate : MonoBehaviour
{
    private ParticleSystem ps;
    public Rigidbody2D rigidbody_ref;


    public void Awake()
    {
        ps = GetComponent<ParticleSystem>();
        //if (!ps) Debug.LogError("ParticleSystem not found");
    }

    public void Update()
    {
        ps.startRotation = -Mathf.Atan2(rigidbody_ref.velocity.y, rigidbody_ref.velocity.x);
    }
}
