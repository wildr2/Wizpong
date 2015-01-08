using UnityEngine;
using System.Collections;
using System.Collections.Generic;


public class CamShakeInstance
{
    private float initial_intensity;
    public float Intensity { get; private set; } // shakiness from 0 to 1
    private float max_life_time;
    private float life_time = 0; // time before the shake stops


    /// <summary>
    /// 
    /// </summary>
    /// <param name="initial_intensity"> shakiness from 0 to 1 </param>
    /// <param name="life_time"> (seconds) </param>
    public CamShakeInstance(float initial_intensity, float life_time)
    {
        if (initial_intensity < 0) Debug.LogWarning("camera shake intesity should not be negative");

        this.initial_intensity = initial_intensity;
        Intensity = initial_intensity;

        max_life_time = life_time;
        this.life_time = life_time;
    }

    /// <summary>
    /// Returns whether the duration is over.
    /// </summary>
    /// <returns></returns>
    public bool UpdateDuration()
    {
        life_time -= Time.deltaTime;

        // decrease intensity as duration increases (linear)
        Intensity = (life_time / max_life_time) * initial_intensity;

        return life_time <= 0;
    }

    public bool NoShake()
    {
        return initial_intensity <= 0;
    }

}


public class CameraShake : MonoBehaviour
{
    
    private Camera cam;

    // cam shake
    private LinkedList<CamShakeInstance> shakes;
    public float max_offset_distance = 1f;

    private Vector2 initial_offset = Vector2.zero;
    private Vector2 target_offset = Vector2.zero;
    private Vector2 current_offset = Vector2.zero;

    private float time_to_new_offset = 0;
    private float max_time_to_new_offset = 0.05f;


    // cam focus
    


    // focus pause
    /*
    private System.DateTime end_pause_time; // system time seconds
    private System.DateTime end_focus_reset_time;
    private bool focus_pause = false;
    private System.TimeSpan pause_duration;
    private System.TimeSpan focus_reset_duration = System.TimeSpan.FromMilliseconds(250);
    private float focus_initial_cam_size;
    private float focus_target_cam_size;
    */
    


	public void Start()
    {
        cam = GetComponent<Camera>();
        if (!cam) Debug.LogError("camera component not found");

        shakes = new LinkedList<CamShakeInstance>();
	}

    public void Update()
    {
        //UpdateFocusPause();

        // update time to new offset
        if (time_to_new_offset > 0)
        {
            time_to_new_offset -= Time.deltaTime;
        }
        else
        {
            if (shakes.Count > 0)
                NewTargetOffset();
            else
            {
                if (current_offset != Vector2.zero) ResetCamera();
                return;
            }
        }


        // update offset
        ResetCamera();
        current_offset = Vector2.Lerp(initial_offset, target_offset, 1 - (time_to_new_offset / max_time_to_new_offset));
        
        // move the camera
        transform.position = new Vector3(transform.position.x + current_offset.x,
            transform.position.y + current_offset.y, transform.position.z);


        // update and remove shake instances
        LinkedListNode<CamShakeInstance> node = shakes.First;
        while (node != null)
        {
            LinkedListNode<CamShakeInstance> nextNode = node.Next;
            if (node.Value.UpdateDuration())
            {
                shakes.Remove(node);
                if (shakes.Count == 0)
                {
                    // set a target offset of (0, 0) (bring the camera back)
                    NewTargetOffset(); 
                }
            }
            node = nextNode;
        }

    }

    /*
    private void UpdateFocusPause()
    {
        if (!focus_pause) return;


        if (System.DateTime.Now < end_pause_time)
        {
            // zoom
            double t = 1 - (end_pause_time - System.DateTime.Now).TotalSeconds / pause_duration.TotalSeconds;
            cam.orthographicSize = Mathf.Lerp(focus_initial_cam_size, focus_target_cam_size, (float)t);
        }
        else if (System.DateTime.Now < end_focus_reset_time)
        {
            if (Time.timeScale == 0)
            {
                Time.timeScale = 1;
            }

            // zoom out again
            double t = 1 - (end_focus_reset_time - System.DateTime.Now).TotalSeconds / focus_reset_duration.TotalSeconds;
            cam.orthographicSize = Mathf.Lerp(focus_target_cam_size, focus_initial_cam_size, (float)t);

        }
        else
        {
            focus_pause = false;
            cam.orthographicSize = focus_initial_cam_size;
        }
        // if time < end pause time
            // update zoom in and position change
        // else if time < end zoom reset time
            // if paused (timescale is 0), unpause
            // update zoom and position back to normal
        // else (all done)
            // set zoom_pause to false and exactly reset zoom and position to initial
    }
    */ 

    private void NewTargetOffset()
    {
        // set timer for moving to the offset
        time_to_new_offset = max_time_to_new_offset;

        // get the total amount of shakiness from all sources of camera shake
        float intensity_factor = 0;
        foreach (CamShakeInstance shake in shakes)
        {
            intensity_factor += shake.Intensity;
        }

        // if no shake, target offset is (0, 0)
        if (intensity_factor <= 0)
        {
            target_offset = Vector2.zero;
            initial_offset = target_offset;
            return;
        }


        // prevent the intenssity factor from exceeding 1
        intensity_factor = Mathf.Clamp(intensity_factor, 0, 1);


        // choose a new offset
        initial_offset = target_offset;
        float angle = Random.Range(0, Mathf.PI * 2.0f);
        target_offset.x = Mathf.Cos(angle) * max_offset_distance * intensity_factor;
        target_offset.y = Mathf.Sin(angle) * max_offset_distance * intensity_factor;
    }
    private void ResetCamera()
    {
        transform.position = new Vector3(transform.position.x - current_offset.x,
            transform.position.y - current_offset.y, transform.position.z);

        current_offset = new Vector2(0, 0);
    }


    /// <summary>
    /// 
    /// </summary>
    /// <param name="intensity"> Amount of shakiness to add from 0 to 1 </param>
    public void Shake(CamShakeInstance shake)
    {
        if (!shake.NoShake())
        {
            shakes.AddLast(shake);
            NewTargetOffset();
        }
    }

    public void Focus(Vector2 position)
    {
    }
    public void CancelFocus()
    {
    }

    /*
    /// <summary>
    /// Pause and zoom on a specified position for a specified duration. 
    /// </summary>
    /// <param name="position"></param>
    /// <param name="duration"></param>
    public void FocusPause(Vector2 position, float duration)
    {
        Time.timeScale = 0;

        // only set zoom target and initial if not already zooming
        if (!focus_pause)
        {
            focus_initial_cam_size = GetComponent<Camera>().orthographicSize;
            focus_target_cam_size = focus_initial_cam_size - 1;
        }

        pause_duration = new System.TimeSpan(0, 0, 0, 0, (int)(duration*1000f));
        end_pause_time = System.DateTime.Now + pause_duration;
        end_focus_reset_time = end_pause_time + focus_reset_duration;

        focus_pause = true;
    }
    */
}
