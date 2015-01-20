using UnityEngine;
using System.Collections;

public class ContollerInstantiator : MonoBehaviour 
{
    public Racquet racquet;
    private MatchManager match;

    public void Awake()
    {
        if (GameSettings.Instance.ai_controlled[racquet.player_number - 1])
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

    public void Start()
    {
        match = Object.FindObjectOfType<MatchManager>();
        if (match == null) Debug.LogError("MatchManager missing.");
    }

}
