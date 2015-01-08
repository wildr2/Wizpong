using UnityEngine;
using System.Collections;

public class PlayerController : MonoBehaviour 
{
    public Racquet racquet;

    public void Instantiate(Racquet racquet)
    {
        this.racquet = racquet;
    }
    public void Update()
    {
        int pn = racquet.player_number;


        // move
        racquet.SetMoveDirection(new Vector2(Input.GetAxisRaw("Horizontal" + pn),
            Input.GetAxisRaw("Vertical" + pn)));

        // finesse
        racquet.SetInputUseFinesse(Input.GetButton("Finesse" + pn));

        // control zone disabling
        if (Input.GetButtonDown("Disable" + pn)) racquet.ShrinkControlZone();

        // lightning
        if (Input.GetButtonDown("Fire" + pn)) racquet.FireLightning();
    }
	
}
