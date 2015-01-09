using UnityEngine;
using System.Collections;
using System.Collections.Generic;


public class SoundManager : MonoBehaviour 
{
    private static SoundManager _instance;
    public static SoundManager Instance
    {
        get
        {
            if (_instance == null)
            {
                _instance = GameObject.FindObjectOfType<SoundManager>();

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

    // Audio sources
    public AudioSource stun_ball_stun_prefab;
    private static int stun_ball_stun_n = 2;
    private static AudioSource[] stun_ball_stun_sources;

    // audio sources that are currently playing
    private static List<AudioSource> active_sources = new List<AudioSource>();


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
    public void Update()
    {
        List<AudioSource> done_sources = new List<AudioSource>();
        foreach (AudioSource source in active_sources)
        {
            // this source should be removed from active sources
            if (!source.isPlaying)
            {
                done_sources.Add(source);
                continue;
            }

            // update pitch based on time scale
            source.pitch = Time.timeScale;   // could improve with initial pitch infomation...
        }

        // remove no longer playing sources from active_sources
        foreach (AudioSource source in done_sources)
        {
            active_sources.Remove(source);
        }
    }

    // make sound functions
    public static void PlayStunBallStun(Vector2 position)
    {
        for (int i = 0; i < stun_ball_stun_n; ++i)
        {
            if (!stun_ball_stun_sources[i].isPlaying)
            {
                stun_ball_stun_sources[i].transform.position = position;
                stun_ball_stun_sources[i].Play();
                active_sources.Add(stun_ball_stun_sources[i]);
                break;
            }
        }
    }


    // PRIVATE MODIFIERS

    private static void Initialize()
    {
        // stun ball stun
        stun_ball_stun_sources = new AudioSource[stun_ball_stun_n];
        for (int i = 0; i < stun_ball_stun_n; ++i)
        {
            stun_ball_stun_sources[i] = (AudioSource)Instantiate(Instance.stun_ball_stun_prefab);
            stun_ball_stun_sources[i].transform.parent = Instance.transform;
        }
            
    }
}
