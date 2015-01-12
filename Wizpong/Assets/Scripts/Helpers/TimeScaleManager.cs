using UnityEngine;
using System.Collections;
using System.Collections.Generic;


/// <summary>
/// Manages multiple assignments to Time.timescale
/// </summary>
public class TimeScaleManager : MonoBehaviour 
{
    public float initial_time_scale = 1;

    // time scale multipliers (id, multipliers on this layer (the top float is the used one))
    private static Dictionary<string, Stack<float>> multipliers = new Dictionary<string, Stack<float>>();
    private static float current_time_scale;


    // PUBLIC MODIFIERS

    public void Awake()
    {
        Time.timeScale = initial_time_scale;
        AddMultiplier("original_time_scale", initial_time_scale, false);
    }
    public void Update()
    {
        // insure that time scale is controlled by this manager
        Time.timeScale = current_time_scale;
    }

    public static void AddMultiplier(string id, float multiplier, bool replace)
    {
        if (multipliers.ContainsKey(id))
        {
            if (replace)
            {
                // replace the multiplier at the top of the stack
                Stack<float> s = multipliers[id];
                s.Pop();
                s.Push(multiplier);
            }
            else
            {
                // add onto the top of the current stack of mults for this id
                // only the top mult of the stack multiplies the time scale
                multipliers[id].Push(multiplier);
            }
        }
        else
        {
            // add a new multiplier
            Stack<float> s = new Stack<float>();
            s.Push(multiplier);
            multipliers.Add(id, s);
        }

        UpdateCurrentTimeScale();
    }
    public static void AddMultiplier(string id, float multiplier)
    {
        AddMultiplier(id, multiplier, true);
    }
    public static void RemoveMultiplier(string id, bool only_last_added)
    {
        if (!multipliers.ContainsKey(id)) return;

        if (only_last_added)
        {
            Stack<float> s = multipliers[id];
            if (s.Count > 0)
            {
                s.Pop();
                if (s.Count == 0) multipliers.Remove(id);
            }
        }
        else
        {
            multipliers.Remove(id);
        }

        UpdateCurrentTimeScale();
    }
    public static void RemoveMultiplier(string id)
    {
        RemoveMultiplier(id, false);
    }


    // PRIVATE MODIFIERS

    private static void UpdateCurrentTimeScale()
    {
        current_time_scale = 1;
        foreach (Stack<float> mults in multipliers.Values)
        {
            current_time_scale *= mults.Peek(); // note: there must be an element in mults
        }
        Time.timeScale = current_time_scale;
    }
}