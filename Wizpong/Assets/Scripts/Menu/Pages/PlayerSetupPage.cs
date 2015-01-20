using UnityEngine;
using System.Collections;
using System.Collections.Generic;
using UnityEngine.UI;

public class PlayerSetupPage : UIMenuPage 
{
    public Text[] bttn_control_type_text;
    public Text[] bttn_color_text;
    public InputField[] name_input;
    public Image[] racquet_previews;
    public ColorPage color_page;

    public VerticalLayoutGroup input_device_column;
    public Text input_device_text_prefab;
    private Dictionary<PlayerInputDevice, Text> device_texts;


    public UIMenuPage same_colors_pop_up, same_input_pop_up, begin_pop_up;
    public FadeScreenPage fadescreen_page;


    new public void Start()
    {
        for (int pn = 1; pn < name_input.Length; ++pn)
        {
            ResetButtonControlType(pn, GameSettings.Instance.ai_controlled[pn - 1]);
            ResetInputFieldPlayerName(pn, GameSettings.Instance.player_name[pn - 1]);
            ResetColorPreviewAndButton(pn, GameSettings.Instance.GetPlayerColor(pn, true),
                GameSettings.Instance.player_color_names[GameSettings.Instance.player_color_ID[pn - 1]]);
        }

        base.Start();
    }

    public void ButtonControlType(int player_number)
    {
        GameSettings.Instance.ai_controlled[player_number - 1] = ! GameSettings.Instance.ai_controlled[player_number - 1];
        ResetButtonControlType(player_number, GameSettings.Instance.ai_controlled[player_number - 1]);
    }
    public void ButtonChangeColor(int player_number)
    {
        TransitionOut();
        color_page.SetChoosingPlayerNumber(player_number);
        color_page.player_page = this;
        color_page.TransitionIn();
    }
    public void InputFieldEnterPlayerName(int player_number)
    {
        GameSettings.Instance.player_name[player_number - 1] = name_input[player_number - 1].text;
    }
    public void ButtonBegin()
    {
        // same colors (not including random)
        if (GameSettings.Instance.PlayerSameColors())
            same_colors_pop_up.TransitionIn();

        // same input in a human only match
        else if (!(GameSettings.Instance.ai_controlled[0] || GameSettings.Instance.ai_controlled[1])
            && InputManager.GetChosenInputDevice(1) == InputManager.GetChosenInputDevice(2))
            same_input_pop_up.TransitionIn();

        // all set
        else
            begin_pop_up.TransitionIn();
    }
    public void ButtonBeginYes()
    {
        fadescreen_page.TransitionIn();
        fadescreen_page.on_transitioned_in = () => Application.LoadLevel("Game");

        TransitionOut();
    }
    public void ButtonBeginTutorial()
    {
        // settings
        GameSettings.Instance.ai_controlled[1] = true;
        GameSettings.Instance.ai_controlled[0] = false;
        GameSettings.Instance.player_color_ID[1] = 0; // random color

        fadescreen_page.TransitionIn();
        fadescreen_page.on_transitioned_in = () => Application.LoadLevel("Tutorial");

        TransitionOut();
    }

    public void ResetColorPreviewAndButton(int player_number, Color color, string color_name)
    {
        racquet_previews[player_number - 1].color = color;
        bttn_color_text[player_number - 1].text = color_name;
    }


    protected override void OnStartTransitionIn()
    {
        StartCoroutine("UpdateInputDeviceChoice");
        CreateInputDeviceList();

        base.OnStartTransitionIn();
    }
    protected override void OnStartTransitionOut()
    {
        StopCoroutine("UpdateInputDeviceChoice");
        base.OnStartTransitionOut();
    }
    protected override void OnFinishTransitionOut()
    {
        DestroyInputDeviceList();
        base.OnFinishTransitionOut();
    }

    private void CreateInputDeviceList()
    {
        // create the input device choice list
        device_texts = new Dictionary<PlayerInputDevice, Text>();
        for (int i = 0; i < InputManager.NumberOfDevicesAvailable(); ++i)
        {
            Text text = (Text)Instantiate(input_device_text_prefab);
            text.text = ((PlayerInputDevice)i).ToString();
            text.transform.parent = input_device_column.transform;
            device_texts.Add(((PlayerInputDevice)i), text);
        }
    }
    private void DestroyInputDeviceList()
    {
        // delete the input device choice list
        int n = input_device_column.transform.childCount;
        for (int i = 0; i < n; ++i)
        {
            Destroy(input_device_column.transform.GetChild(i).gameObject);
        }
    }
    private IEnumerator UpdateInputDeviceChoice()
    {
        while (true)
        {

            for (int i = 0; i < InputManager.NumberOfDevicesAvailable(); ++i)
            {
                PlayerInputDevice device = (PlayerInputDevice)i;
                if (InputManager.HorizontalDevice(device) < 0)
                {
                    // set text
                    device_texts[InputManager.GetChosenInputDevice(1)].text = InputManager.GetChosenInputDevice(1).ToString();
                    device_texts[device].text = "< " + device.ToString();

                    // set input device
                    InputManager.SetPlayerInputDevice(1, device);
                }
                else if (InputManager.HorizontalDevice(device) > 0)
                {
                    // set text
                    device_texts[InputManager.GetChosenInputDevice(2)].text = InputManager.GetChosenInputDevice(2).ToString();
                    device_texts[device].text = device.ToString() + " >";

                    // set input device
                    InputManager.SetPlayerInputDevice(2, device);
                }
            }


            yield return null;
        }
    }

    private void ResetButtonControlType(int player_number, bool ai_controlled)
    {
        bttn_control_type_text[player_number - 1].text = (ai_controlled ? "AI" : "Human");
    }
    private void ResetInputFieldPlayerName(int player_number, string name)
    {
        name_input[player_number - 1].placeholder.GetComponent<Text>().text = name;
    }
    
}
