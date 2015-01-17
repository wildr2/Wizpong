using UnityEngine;
using System.Collections;

public class FireballTrailSegment : MonoBehaviour
{
    private Fireball parent;

    public void Initialize(Fireball parent)
    {
        this.parent = parent;
        StartCoroutine("DeactivateAfterWait");
    }
    public void OnTriggerEnter2D(Collider2D collider)
    {
        // VS Racquet physical
        if (collider.CompareTag("Racquet"))
        {
            Racquet r = collider.GetComponent<Racquet>();

            // only stun racquets that are not currently controlling the fireball (have it in the control zone)
            if (r != null && !(parent.ControllingRacquet() != null && parent.ControllingRacquet().ControlledBall() == parent))
            {
                // stun the racquet and move them away from the fire
                r.Stun(0.2f);
                Vector2 v = transform.position - r.transform.position;
                r.transform.Translate(-v * 0.5f);
            }
        }
    }

    private IEnumerator DeactivateAfterWait()
    {
        yield return new WaitForSeconds(3);

        gameObject.SetActive(false);
    }
}
