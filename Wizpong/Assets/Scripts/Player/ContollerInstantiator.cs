using UnityEngine;
using System.Collections;

public class ContollerInstantiator : MonoBehaviour 
{
    public Racquet racquet;
    public MatchManager match;

    public void Awake()
    {
        if (GameSettings.ai_controlled[racquet.player_number - 1])
        {
            AIController c = gameObject.AddComponent<AIController>();
            c.Instantiate(racquet, match);
        }
        else
        {
            PlayerController c = gameObject.AddComponent<PlayerController>();
            c.Instantiate(racquet);
        }
    }

}
