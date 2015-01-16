using UnityEngine;
using System.Collections;


public enum PlayerInputDevice { Keyboard, Joystick1L, Joystick1R, Joystick2L, Joystick2R }

public static class InputManager
{
    private static PlayerInputDevice[] input_device_mapping;
    private const int max_players = 2;


    static InputManager()
    {
        input_device_mapping = new PlayerInputDevice[max_players];
        SetPlayerInputDevice(1, PlayerInputDevice.Keyboard);
        SetPlayerInputDevice(2, PlayerInputDevice.Joystick1R);
    }

    public static void SetPlayerInputDevice(int player_number, PlayerInputDevice device)
    {
        CheckInputPlayerNumber(player_number);

        input_device_mapping[player_number - 1] = (PlayerInputDevice)device;
    }
    public static int NumberOfDevicesAvailable()
    {
        return 1 + Input.GetJoystickNames().Length * 2;
    }
    public static PlayerInputDevice GetChosenInputDevice(int player_number)
    {
        return input_device_mapping[player_number - 1];
    }

    public static float Horizontal(int player_number)
    {
        CheckInputPlayerNumber(player_number);
        switch (input_device_mapping[player_number - 1])
        {
            case PlayerInputDevice.Keyboard: return Input.GetAxis("KB X");
            case PlayerInputDevice.Joystick1L: return Input.GetAxis("Joystick1L X");
            case PlayerInputDevice.Joystick1R: return Input.GetAxis("Joystick1R X");
            case PlayerInputDevice.Joystick2L: return Input.GetAxis("Joystick2L X");
            case PlayerInputDevice.Joystick2R: return Input.GetAxis("Joystick2R X");
        }
        return 0;
    }
    public static float HorizontalDevice(PlayerInputDevice device)
    {
        switch (device)
        {
            case PlayerInputDevice.Keyboard: return Input.GetAxis("KB X");
            case PlayerInputDevice.Joystick1L: return Input.GetAxis("Joystick1L X");
            case PlayerInputDevice.Joystick1R: return Input.GetAxis("Joystick1R X");
            case PlayerInputDevice.Joystick2L: return Input.GetAxis("Joystick2L X");
            case PlayerInputDevice.Joystick2R: return Input.GetAxis("Joystick2R X");
        }
        return 0;
    }
    public static float Vertical(int player_number)
    {
        CheckInputPlayerNumber(player_number);
        switch (input_device_mapping[player_number - 1])
        {
            case PlayerInputDevice.Keyboard: return Input.GetAxis("KB Y");
            case PlayerInputDevice.Joystick1L: return Input.GetAxis("Joystick1L Y");
            case PlayerInputDevice.Joystick1R: return Input.GetAxis("Joystick1R Y");
            case PlayerInputDevice.Joystick2L: return Input.GetAxis("Joystick2L Y");
            case PlayerInputDevice.Joystick2R: return Input.GetAxis("Joystick2R Y");
        }
        return 0;
    }
    public static float VerticalDevice(PlayerInputDevice device)
    {
        switch (device)
        {
            case PlayerInputDevice.Keyboard: return Input.GetAxis("KB Y");
            case PlayerInputDevice.Joystick1L: return Input.GetAxis("Joystick1L Y");
            case PlayerInputDevice.Joystick1R: return Input.GetAxis("Joystick1R Y");
            case PlayerInputDevice.Joystick2L: return Input.GetAxis("Joystick2L Y");
            case PlayerInputDevice.Joystick2R: return Input.GetAxis("Joystick2R Y");
        }
        return 0;
    }
    public static bool ActionButtonDown(int player_number)
    {
        CheckInputPlayerNumber(player_number);
        switch (input_device_mapping[player_number - 1])
        {
            case PlayerInputDevice.Keyboard: return Input.GetButtonDown("KB Action");
            case PlayerInputDevice.Joystick1L: return Input.GetButtonDown("Joystick1L Action");
            case PlayerInputDevice.Joystick1R: return Input.GetButtonDown("Joystick1R Action");
            case PlayerInputDevice.Joystick2L: return Input.GetButtonDown("Joystick2L Action");
            case PlayerInputDevice.Joystick2R: return Input.GetButtonDown("Joystick2R Action");
        }
        return false;
    }
    public static bool ActionButtonUp(int player_number)
    {
        CheckInputPlayerNumber(player_number);
        switch (input_device_mapping[player_number - 1])
        {
            case PlayerInputDevice.Keyboard: return Input.GetButtonUp("KB Action");
            case PlayerInputDevice.Joystick1L: return Input.GetButtonUp("Joystick1L Action");
            case PlayerInputDevice.Joystick1R: return Input.GetButtonUp("Joystick1R Action");
            case PlayerInputDevice.Joystick2L: return Input.GetButtonUp("Joystick2L Action");
            case PlayerInputDevice.Joystick2R: return Input.GetButtonUp("Joystick2R Action");
        }
        return false;
    }
    public static bool ActionButton(int player_number)
    {
        CheckInputPlayerNumber(player_number);
        switch (input_device_mapping[player_number - 1])
        {
            case PlayerInputDevice.Keyboard: return Input.GetButton("KB Action");
            case PlayerInputDevice.Joystick1L: return Input.GetButton("Joystick1L Action");
            case PlayerInputDevice.Joystick1R: return Input.GetButton("Joystick1R Action");
            case PlayerInputDevice.Joystick2L: return Input.GetButton("Joystick2L Action");
            case PlayerInputDevice.Joystick2R: return Input.GetButton("Joystick2R Action");
        }
        return false;
    }


    private static void CheckInputPlayerNumber(int player_number)
    {
        if (player_number < 0 || player_number > max_players)
        {
            Debug.LogError("Player Number must be a number 1 through " + max_players);
        }
    }
}

