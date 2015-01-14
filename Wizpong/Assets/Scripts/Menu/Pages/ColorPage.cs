using UnityEngine;
using System.Collections;
using UnityEngine.UI;

public class ColorPage : UIMenuPage 
{
    public Button bttn_prefab, random_bttn_prefab;
    public GridLayoutGroup grid;
    public PlayerSetupPage player_page;
    
    private int choosing_player_number = 1;


    public void Awake()
    {
        CreateColorGrid();
    }

    public void ButtonColor(int color_id)
    {
        if (choosing_player_number < 1 || choosing_player_number > 1) Debug.LogWarning("player number should be 1 or 2");
        GameSettings.Instance.player_color_ID[choosing_player_number - 1] = color_id;

        player_page.ResetColorPreviewAndButton(choosing_player_number,
            GameSettings.Instance.player_colors[color_id], GameSettings.Instance.player_color_names[color_id]);

        TransitionOut();
        if (player_page != null) player_page.TransitionIn();
    }
    public void SetChoosingPlayerNumber(int number)
    {
        choosing_player_number = number;
    }
    

    
    private void CreateColorGrid()
    {
        // create the random color button
        Button b = (Button)Instantiate(random_bttn_prefab);
        b.transform.SetParent(grid.transform);
        b.transform.localScale = new Vector3(1, 1, 1); // fix problem with scale resizing...
        b.onClick.AddListener(() => ButtonColor(0));
        

        // create color buttons
        int n = GameSettings.Instance.player_colors.Length;
        for (int id = 1; id < n; ++id)
        {
            b = (Button)Instantiate(bttn_prefab);
            b.transform.SetParent(grid.transform);
            b.transform.localScale = new Vector3(1, 1, 1); // fix problem with scale resizing...

            b.GetComponent<UITransitionFade>().offset = (float)id / n;

            int param = id;
            b.onClick.AddListener(() => ButtonColor(param));

            Image image = b.GetComponent<Image>();
            image.color = GameSettings.Instance.player_colors[id];
        }
    }
}
