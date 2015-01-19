using UnityEngine;
using System.Collections;

public class Help2Page : UIMenuPage 
{
    public Racquet ai_racquet;
    public Transform court_ui;


    public void Start()
    {
        ai_racquet.gameObject.SetActive(true);
        court_ui.gameObject.SetActive(true);
    }
}
