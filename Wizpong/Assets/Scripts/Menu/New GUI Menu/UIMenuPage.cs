using UnityEngine;
using System.Collections;
using System.Collections.Generic;
using UnityEngine.UI;


public class UIMenuPage : MonoBehaviour 
{
    public float seconds = 0.5f;
    public bool transition_out_on_start = false;

    public System.Action on_transitioned_in;
    public System.Action on_transitioned_out;

    private bool going_in = false, going_out = false;
    private float transition = 0;
    private UITransition[] transition_objects;


    // PUBLIC MODIFIERS

    public void Start()
    {
        if (transition_out_on_start)
        {
            transition = 1;
            TransitionOut();
        }
        else TransitionIn();
    }

    public void TransitionIn()
    {
        if (!going_in)
        {
            gameObject.SetActive(true);
            transform.SetAsLastSibling(); // make the topmost page (to disable interaction with lower pages)
            UpdateTransitioningObjectList();

            going_out = false;
            StopCoroutine("UpdateTransitionOut");

            going_in = true;
            StartCoroutine("UpdateTransitionIn");
        }
    }
    public void TransitionOut()
    {
        if (!going_out)
        {
            UpdateTransitioningObjectList();
            transform.SetAsFirstSibling(); // make the bottommost page (to disable interaction with lower pages)

            going_in = false;
            StopCoroutine("UpdateTransitionIn");

            going_out = true;
            StartCoroutine("UpdateTransitionOut");
        }
    }


    // PRIVATE MODIFIERS

    private void UpdateTransitioningObjectList()
    {
        transition_objects = GetComponentsInChildren<UITransition>();
    }

    private IEnumerator UpdateTransitionIn()
    {
        while (true)
        {
            transition += Time.unscaledDeltaTime / seconds;
            transition = Mathf.Min(transition, 1);

            foreach (UITransition obj in transition_objects)
            {
                obj.UpdateTransition(transition, going_in);
            }

            // transition finished
            if (transition >= 1)
            {
                going_in = false;
                if (on_transitioned_in != null) on_transitioned_in();
                break;
            }

            yield return null;
        }
    }
    private IEnumerator UpdateTransitionOut()
    {
        while (true)
        {
            transition -= Time.unscaledDeltaTime / seconds;
            transition = Mathf.Max(transition, 0);

            foreach (UITransition obj in transition_objects)
            {
                obj.UpdateTransition(transition, going_in);
            }

            // transition finished
            if (transition <= 0)
            {
                going_out = false;
                gameObject.SetActive(false);
                if (on_transitioned_out != null)
                {
                    on_transitioned_out();
                }
                break;
            }

            yield return null;
        }
    }
    

    // PUBLIC ACCESSORS

    public bool IsGoingOut()
    {
        return going_out;
    }
    public bool IsGoingIn()
    {
        return going_in;
    }
	public float Transition()
    {
        return transition;
    }
}
