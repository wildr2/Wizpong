using UnityEngine;
using System.Collections;
using System.Collections.Generic;


public enum TransitionState { In, Out, GoingIn, GoingOut }

public class MenuPage : MonoBehaviour 
{
    // screen size to use for gui measurements (use EnableGUIScale to make resolution independant)
    public static Rect default_screen = new Rect(0, 0, 1920, 1080);

    // transitions
    protected float transition_seconds = 0.2f;
    protected float transition = 0; // 0 is out, 1 is in
    protected TransitionState tran_state { get; private set; }
    private System.Action on_in, on_out;

    // user control
    private bool has_control = true;

    // keyboard control
    private KeyboardControlNode kb_head_node = null;
    private KeyboardControlNode kb_current_node;
    private Dictionary<string, KeyboardControlNode> kb_node_by_name;

    private Stack<KeyboardControlNode> kb_last_nodes = new Stack<KeyboardControlNode>();
    private bool keyboard_nodes_setup = false;

    



    // PUBLIC MODIFIERS
    public void Awake()
    {
        tran_state = TransitionState.GoingIn;
    }
    public void Update()
    {
        UpdateTransitions();
        UpdateKeyboardControl();
    }

    public void TransitionIn(System.Action on_in)
    {
        this.on_in = on_in;
        gameObject.SetActive(true);
        tran_state = TransitionState.GoingIn;

        kb_current_node = kb_head_node;
        has_control = true;
    }
    public void TransitionOut(System.Action on_out)
    {
        this.on_out = on_out;
        tran_state = TransitionState.GoingOut;
        has_control = false;
    }


    // PRIVATE MODIFIERS
    private void UpdateTransitions()
    {
        if (tran_state == TransitionState.GoingIn)
        {
            transition += Time.deltaTime / transition_seconds;
            if (transition >= 1)
            {
                if (on_in != null) on_in();
                transition = 1;
                tran_state = TransitionState.In;
            }
        }
        else if (tran_state == TransitionState.GoingOut)
        {
            transition -= Time.deltaTime / transition_seconds;
            if (transition <= 0)
            {
                if (on_out != null) on_out();
                transition = 0;
                tran_state = TransitionState.Out;
                gameObject.SetActive(false);
            }
        }
    }
    private void UpdateKeyboardControl()
    {
        if (kb_current_node == null || !has_control) return;

        if (Input.GetKeyDown(KeyCode.DownArrow))
        {
            if (kb_current_node.down != null) kb_current_node = kb_current_node.down;
            //GUI.FocusControl(kb_current_node.name);
            Debug.Log("menu down to " + kb_current_node.name);
        }
        if (Input.GetKeyDown(KeyCode.UpArrow))
        {
            if (kb_current_node.up != null) kb_current_node = kb_current_node.up;
            //GUI.FocusControl(kb_current_node.name);
            Debug.Log("menu up to " + kb_current_node.name);
        }

        
    }


    // PROTECTED MODIFIERS
    protected void EnableGUIScale()
    {
        float v_ratio = Screen.height / default_screen.height;
        float h_ratio = Screen.width / default_screen.width;
        float ratio = Mathf.Min(v_ratio, h_ratio);
        
        GUI.matrix = Matrix4x4.TRS(new Vector3(AspectUtility.xOffset, AspectUtility.yOffset, 0), Quaternion.identity, new Vector3(ratio, ratio, 1));
    }
    protected void GiveControlToPopups(MenuPage[] pop_ups)
    {
        GUI.enabled = true;
        foreach (MenuPage pu in pop_ups)
        {
            if (pu.gameObject.activeInHierarchy && !pu.IsGoingOut())
                GUI.enabled = false;
        }

        has_control = GUI.enabled;
    }

    protected void NextVerticalKeyboardControl(string name)
    {
        if (keyboard_nodes_setup) return;

        KeyboardControlNode next = new KeyboardControlNode();
        next.name = name;
        GUI.SetNextControlName(name);

        // first node (head node)
        if (kb_last_nodes.Count == 0)
        {
            kb_last_nodes.Push(next);

            if (kb_head_node == null)
            {
                kb_head_node = next;
                kb_current_node = kb_head_node;
            }

            // initialize the node dictionary
            kb_node_by_name = new Dictionary<string, KeyboardControlNode>();
        }

        // another node
        else
        {
            KeyboardControlNode last = kb_last_nodes.Pop();
            last.down = next;

            next.up = last;
            next.left = last.left;
            next.right = last.right;

            kb_last_nodes.Push(next);
       }

        // add to node dictionary
        kb_node_by_name.Add(name, next);
    }
    protected void EndKeyboardControlGroup()
    {
    }
    protected void DebugLogKBNodes()
    {
        KeyboardControlNode n = kb_head_node;
        while (n != null)
        {
            Debug.Log((n.left != null ? n.left.name : "---") + n.name + (n.right != null ? n.right.name : "---"));
            n = n.down;
        }
    }
    protected void EndKeyboardControlSetup()
    {
        keyboard_nodes_setup = true;

        if (kb_current_node != null && has_control) GUI.FocusControl(kb_current_node.name);
    }

    protected void SetKeyBoardFocus(string name)
    {
        if (keyboard_nodes_setup) kb_current_node = kb_node_by_name[name];
    }


    // PRIVATE / PROTECTED ACCESSORS
    protected bool KBControlPressed(string name)
    {
        return kb_current_node != null && kb_current_node.name == name && Input.GetKeyDown(KeyCode.Return);
    }
    protected bool LastControlHover(string name)
    {
        return Event.current.type == EventType.Repaint && has_control && !IsGoingOut() &&
            GUILayoutUtility.GetLastRect().Contains(Event.current.mousePosition);

        
    }


    // PUBLIC ACCESSORS
    public bool IsIn()
    {
        return tran_state == TransitionState.In;
    }
    public bool IsOut()
    {
        return tran_state == TransitionState.Out;
    }
    public bool IsGoingIn()
    {
        return tran_state == TransitionState.GoingIn;
    }
    public bool IsGoingOut()
    {
        return tran_state == TransitionState.GoingOut;
    }
    public float TransitionPow()
    {
        return IsGoingIn() ? -Mathf.Pow((transition-1), 2) + 1
            : IsGoingOut() ? Mathf.Pow(transition, 2)
            : transition;
    }


    // HELPERS
}


class KeyboardControlNode
{
    public string name = "";
    public KeyboardControlNode left = null, right = null, up = null, down = null;
}