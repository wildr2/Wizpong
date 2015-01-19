using UnityEngine;
using System.Collections;
using UnityEngine.UI;

public class Help1Page : UIMenuPage 
{
    public Gameball gameball;
    public Racquet racquet;

    public Text gameball_label;
    public Text racquet_label;

    private Vector2 gameball_label_offset;
    private Vector2 racquet_label_offset;


    public void Start()
    {
        gameball_label_offset = gameball_label.transform.position - gameball.transform.position;
        racquet_label_offset = racquet_label.transform.position - racquet.transform.position;
    }
    public void Update()
    {
        gameball_label.transform.position = (Vector2)gameball.transform.position + gameball_label_offset;
        racquet_label.transform.position = (Vector2)racquet.transform.position + racquet_label_offset;
    }
}
