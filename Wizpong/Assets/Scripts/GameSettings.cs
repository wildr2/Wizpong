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
                    Initialize();
                }
            }
            return _instance;
        }
    }


    // player info
    public static bool[] ai_controlled = { false, false };
    public static string[] player_name = { "Player 1", "Player 2" };
    public static int[] player_color_ID = { 0, 0 };

	// match info
    public static int court;
    public static int match_type;
    public static bool music_on = true;

    // constant data  (perhaps load from file in future)
    public static Color[] player_colors; // set in initialize
    public static string[] player_color_names = { "random color", "red", "pink", "purple", "blue",
                                                  "teal", "green", "lime", "yellow", "orange" };
    private static string[] hex_colors = { "ffffff", "ff0000", "ff00c6", "8949ff", "4c6eff",
                            "4cc2ff", "09ff49", "a5ff09", "f8ff38", "ff9a38" };


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

    private static void Initialize()
    {
        // colors
        InitializeColorsFromHex();
    }
    private static void InitializeColorsFromHex()
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
    private static void SetUnchosenPlayerColors()
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

    public static Color GetPlayerColor(int player_number)
    {
        // set colors for players with random color selected
        if (player_color_ID[player_number - 1] == 0)
            SetUnchosenPlayerColors();

        return player_colors[player_color_ID[player_number - 1]];
    }
    /// <summary>
    /// Returns whether the players have the same player color selected (random color doesn't count).
    /// </summary>
    /// <returns></returns>
    public static bool PlayerSameColors()
    {
        return player_color_ID[0] == player_color_ID[1] && player_color_ID[0] != 0;
    }    
}
