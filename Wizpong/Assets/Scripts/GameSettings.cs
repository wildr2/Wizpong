using UnityEngine;
using System.Collections;


/// <summary>
/// Singleton containing settings (set in menus) for gameplay
/// </summary>
public class GameSettings : MonoBehaviour
{
    private static GameSettings _instance;
    public static GameSettings Instance
    {
        get
        {
            if (_instance == null)
            {
                _instance = GameObject.FindObjectOfType<GameSettings>();

                if (_instance == null) Debug.LogError("Missing GameSettings");
                else
                {
                    DontDestroyOnLoad(_instance);
                    _instance.Initialize();
                }
            }
            return _instance;
        }
    }

    // Audio volumes
    public float volume_fx = 1, volume_music = 1;

    // Player info
    public bool[] ai_controlled = { false, false };
    public string[] player_name = { "Player 1", "Player 2" };
    public int[] player_color_ID = { 0, 0 };

	// Match info
    public int match_type;
    public bool music_on = true;

    // Constant data  (perhaps load from file in future)
    [System.NonSerialized]
    public Color[] player_colors; // set in initialize
    public string[] player_color_names = { "random color", "red", "pink", "purple", "blue",
           "teal", "green", "lime", "yellow", "orange", "maroon", "navy", "indigo" };
    private string[] hex_colors = { "ffffff", "ff0000", "ff00c6", "8949ff", "4c6eff",
           "4cc2ff", "09ff49", "a5ff09", "f8ff38", "ff9a38", "6f1a4a", "11116c", "2c1ccf" };


    // PUBLIC MODIFIERS

    public void Awake()
    {
        // if this is the first instance, make this the singleton
        if (_instance == null)
        {
            _instance = this;
            DontDestroyOnLoad(_instance);
        }
        else
        {
            // destroy other instances that are not the already existing singleton
            if (this != _instance)
                Destroy(this.gameObject);
        }

        Initialize();
    }


    // PRIVATE MODIFIERS

    private void Initialize()
    {
        // colors
        InitializeColorsFromHex();
    }
    private void InitializeColorsFromHex()
    {
        player_colors = new Color[hex_colors.Length];
        for (int i = 0; i < hex_colors.Length; ++i)
        {
            player_colors[i] = GeneralHelpers.HexToColor(hex_colors[i]);
        }
    }

    /// <summary>
    /// If any player has selected random color, this will choose a color for them, insuring that no players
    /// have the same color
    /// </summary>
    private void SetUnchosenPlayerColors()
    {
        for (int i = 0; i < 2; ++i)
        {
            if (player_color_ID[i] == 0)
            {
                player_color_ID[i] = Random.Range(1, player_colors.Length - 1);
                if (PlayerSameColors()) ++player_color_ID[i];
                if (player_color_ID[i] == 0) ++player_color_ID[i]; // can't random to random
            }
        }
    }


    // PUBLIC ACCESSORS

    /// <summary>
    /// If can be random is true and "random color" is currently selected, white is returned and
    /// the color is not changed.
    /// </summary>
    /// <param name="player_number"></param>
    /// <param name="can_be_random"></param>
    /// <returns></returns>
    public Color GetPlayerColor(int player_number, bool can_be_random)
    {
        GameSettings s = Instance;

        // set colors for players with random color selected
        if (s.player_color_ID[player_number - 1] == 0 && !can_be_random)
            s.SetUnchosenPlayerColors();

        return s.player_colors[s.player_color_ID[player_number - 1]];
    }
    public Color GetPlayerColor(int player_number)
    {
        return GetPlayerColor(player_number, false);
    }
    /// <summary>
    /// Returns whether the players have the same player color selected (random color doesn't count).
    /// </summary>
    /// <returns></returns>
    public bool PlayerSameColors()
    {
        GameSettings s = Instance;
        return s.player_color_ID[0] == s.player_color_ID[1] && s.player_color_ID[0] != 0;
    }    
}
