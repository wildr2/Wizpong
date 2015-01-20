using UnityEngine;
using System.Collections;
using UnityEngine.UI;

public class Help1Page : UIMenuPage 
{
    public Gameball gameball;
    public Racquet racquet;

    public Text gameball_label;
    public Text racquet_label;

    private MatchManager match;

    private Vector2 gameball_label_offset;
    private Vector2 racquet_label_offset;


    public void Awake()
    {
        match = Object.FindObjectOfType<MatchManager>();
        if (match == null) Debug.LogError("MatchManager missing.");        
    }
    new public void Start()
    {
        // labels
        gameball_label_offset = gameball_label.transform.position - gameball.transform.position;
        racquet_label_offset = racquet_label.transform.position - racquet.transform.position;

        base.Start();
    }
    public void Update()
    {
        gameball_label.transform.position = (Vector2)gameball.transform.position + gameball_label_offset;
        racquet_label.transform.position = (Vector2)racquet.transform.position + racquet_label_offset;
    }

    protected override void OnStartTransitionIn()
    {
        // customize the match
        match.SetMatchClockEnabled(false);
        match.SetRewallingEnabled(false);
        match.SetInterPointWaitEnabled(false);
        match.SetScoringEnabled(false);
        match.BeginMatch();

        base.OnStartTransitionIn();
    }
}
