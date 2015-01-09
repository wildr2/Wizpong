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


    // Audio source groups
    public AudioSource prefab_stunball_stun;
    private static AudioSourceGroup asg_stunball_stun;

    public AudioSource prefab_shock;
    private static AudioSourceGroup asg_shock;

    public AudioSource prefab_stun;
    private static AudioSourceGroup asg_stun;

    public AudioSource prefab_stunball_possess;
    private static AudioSourceGroup asg_stunball_possess;

    public AudioSource prefab_shockball_possess;
    private static AudioSourceGroup asg_shockball_possess;

    public AudioSource prefab_gameball_bump;
    private static AudioSourceGroup asg_gameball_bump;

    public AudioSource prefab_stunball_bump;
    private static AudioSourceGroup asg_stunball_bump;

    public AudioSource prefab_shockball_bump;
    private static AudioSourceGroup asg_shockball_bump;

    
    // Simple audio sources
    public AudioSource source_rewall;
    public AudioSource source_begin_point;
    public AudioSource source_point;
    public AudioSource source_game_over;
    public AudioSource source_live_wall;
    public AudioSource source_possesion_change;



    // Audio sources that are currently playing
    private static List<ActiveAudioSource> active_sources = new List<ActiveAudioSource>();


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
        List<ActiveAudioSource> done_sources = new List<ActiveAudioSource>();
        foreach (ActiveAudioSource s in active_sources)
        {
            // this source should be removed from active sources
            if (s.FinishedPlaying())
            {
                done_sources.Add(s);
                continue;
            }


            // set pitch
            if (!s.timescale_independant)
            {
                // update pitch based on time scale
                s.source.pitch = Mathf.Max(0, (1f + s.pitch_offset) * Time.timeScale);
            }
            else
            {
                s.source.pitch = Mathf.Max(0, (1f + s.pitch_offset));
            }
        }

        // remove no longer playing sources from active_sources
        foreach (ActiveAudioSource source in done_sources)
        {
            active_sources.Remove(source);
        }
    }

    // make sound functions
    public static void PlayStunBallStun(Vector2 position)
    {
        ActiveAudioSource s = asg_stunball_stun.GetAvailableSource();

        s.source.transform.position = position;
        s.Play();

        active_sources.Add(s);
    }
    public static void PlayShock()
    {
        ActiveAudioSource s = asg_shock.GetAvailableSource();

        s.Play();
        s.pitch_offset = 0.4f;
        active_sources.Add(s);
    }
    public static void PlayStun()
    {
        ActiveAudioSource s = asg_stun.GetAvailableSource();

        s.Play();
        active_sources.Add(s);
    }

    public static void PlayStunballPossess(Vector2 position)
    {
        ActiveAudioSource s = asg_stunball_possess.GetAvailableSource();

        s.source.transform.position = position;
        s.Play();

        active_sources.Add(s);
    }
    public static void PlayShockballPossess(Vector2 position)
    {
        ActiveAudioSource s = asg_shockball_possess.GetAvailableSource();

        s.source.transform.position = position;
        //s.pitch_offset = 1;
        s.Play();

        active_sources.Add(s);
    }

    public static void PlayGameBallBump(Vector2 position, float force)
    {
        ActiveAudioSource s = asg_gameball_bump.GetAvailableSource();

        s.source.transform.position = position;
        s.source.volume = force;
        s.pitch_offset = Random.Range(-0.05f, 0.05f);
        s.Play();

        active_sources.Add(s);
    }
    public static void PlayStunBallBump(Vector2 position, float force)
    {
        ActiveAudioSource s = asg_stunball_bump.GetAvailableSource();

        s.source.transform.position = position;
        s.source.volume = force;
        s.pitch_offset = Random.Range(-0.05f, 0.05f);
        s.Play();

        active_sources.Add(s);
    }
    public static void PlayShockBallBump(Vector2 position, float force)
    {
        ActiveAudioSource s = asg_shockball_bump.GetAvailableSource();

        s.source.transform.position = position;
        s.source.volume = force;
        s.pitch_offset = Random.Range(-0.05f, 0.05f);
        s.Play();

        active_sources.Add(s);
    }

    public static void PlayRewall()
    {
        Instance.source_rewall.Play();
    }
    public static void PlayBeginPoint()
    {
        Instance.source_begin_point.Play();
    }
    public static void PlayPoint()
    {
        Instance.source_point.Play();
    }
    public static void PlayGameOver()
    {
        Instance.source_game_over.Play();
    }
    public static void PlayPosessionChange()
    {
        Instance.source_possesion_change.Play();
    }
    public static void PlayLiveWall()
    {
        Instance.source_live_wall.Play();
    }


    // PRIVATE MODIFIERS

    private static void Initialize()
    {
        asg_stunball_stun = new AudioSourceGroup(Instance.prefab_stunball_stun, 2);
        asg_shock = new AudioSourceGroup(Instance.prefab_shock, 6);
        asg_stun = new AudioSourceGroup(Instance.prefab_stun, 2);

        asg_stunball_possess = new AudioSourceGroup(Instance.prefab_stunball_possess, 2);
        asg_shockball_possess = new AudioSourceGroup(Instance.prefab_shockball_possess, 2);

        asg_gameball_bump = new AudioSourceGroup(Instance.prefab_gameball_bump, 4);
        asg_stunball_bump = new AudioSourceGroup(Instance.prefab_stunball_bump, 6);
        asg_shockball_bump = new AudioSourceGroup(Instance.prefab_shockball_bump, 6);

        
    }
}


public class AudioSourceGroup
{
    public AudioSource prefab;
    public int count = 2;
    public AudioSource[] sources;

    public AudioSourceGroup(AudioSource prefab, int count)
    {
        sources = new AudioSource[count];
        for (int i = 0; i < count; ++i)
        {
            sources[i] = (AudioSource)GameObject.Instantiate(prefab);
            sources[i].transform.parent = SoundManager.Instance.transform;
        }
    }
    
    /// <summary>
    /// Return an ActiveAudioSource containing an available audio source from this group.
    /// If all sources are currently playing, the least recently starting audio source is stopped and returned.
    /// </summary>
    /// <returns></returns>
    public ActiveAudioSource GetAvailableSource()
    {
        for (int i = 0; i < count; ++i)
        {
            if (!sources[i].isPlaying)
                return new ActiveAudioSource(sources[i]);
        }

        sources[0].Stop();
        return new ActiveAudioSource(sources[0]);
    }
}
public class ActiveAudioSource
{
    public AudioSource source;
    public float pitch_offset;
    public bool timescale_independant = false;

    public ActiveAudioSource(AudioSource source)
    {
        this.source = source;
    }
    public void Play()
    {
        source.Play();
    }
    public bool FinishedPlaying()
    {
        return (!source.isPlaying);
    }
}