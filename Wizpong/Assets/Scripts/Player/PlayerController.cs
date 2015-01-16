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
        racquet.SetMoveDirection(new Vector2(InputManager.Horizontal(pn),
            InputManager.Vertical(racquet.player_number)));

        // finesse
        racquet.SetInputUseFinesse(InputManager.ActionButton(pn));

        // control zone disabling
        if (InputManager.ActionButtonDown(pn)) racquet.EnableControlZone();
        else if (!racquet.ControllingBall() && (InputManager.ActionButtonUp(pn))) racquet.DisableControlZone();

        // lightning
        //if (Input.GetButtonDown("Fire" + pn)) racquet.FireLightning();
    }
	
}
