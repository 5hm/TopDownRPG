using System.Collections;
using System.Collections.Generic;
using UnityEditor;
using UnityEngine;
using UnityEngine.UI;

public class PlayerAction : MonoBehaviour
{
    public GameManager gameManager;
    public float speed;
    float h;
    float v;
    bool is_HorizonMove;
    Vector3 dirVec;
    GameObject scanEvents;

    Animator anim;
    Rigidbody2D rigid;

    //Mobile Key Var
    int up_Value;
    int down_Value;
    int left_Value;
    int right_Value;
    bool up_Down;
    bool down_Down;
    bool left_Down;
    bool right_Down;
    bool up_Up;
    bool down_Up;
    bool left_Up;
    bool right_Up;

    // Start is called before the first frame update
    void Awake()
    {
        rigid = GetComponent<Rigidbody2D>();
        anim = GetComponent<Animator>();
    }

    // Update is called once per frame
    void Update()
    {
        //move value , 스캔 시 이동제한
        //PC + Mobile
        h = gameManager.is_Action ? 0 : Input.GetAxisRaw("Horizontal") + right_Value + left_Value;
        v = gameManager.is_Action ? 0 : Input.GetAxisRaw("Vertical") + up_Value + down_Value;

        //Check Button Down & Up, 스캔 시 이동제한
        //PC + Mobile
        bool h_Down = gameManager.is_Action ? false : Input.GetButtonDown("Horizontal") || right_Down || left_Down;
        bool v_Down = gameManager.is_Action ? false : Input.GetButtonDown("Vertical") || up_Down || down_Down;
        bool h_Up = gameManager.is_Action ? false : Input.GetButtonUp("Horizontal") || right_Up || left_Up;
        bool v_Up = gameManager.is_Action ? false : Input.GetButtonUp("Vertical") || up_Up || down_Up;

        
        //대각선 이동 방지    이해 잘 안됨
        if (h_Down)
        {
            is_HorizonMove = true;
        }
        else if(v_Down)
        {
            is_HorizonMove = false;
        }
        else if (h_Up || v_Up)
        {
            is_HorizonMove = h != 0;
        }

        //Animation
        if(anim.GetInteger("HAxisRaw") != h)
        {
            anim.SetBool("is_Change", true);
            anim.SetInteger("HAxisRaw", (int)h);
        }
        else if (anim.GetInteger("VAxisRaw") != v)
        {
            anim.SetBool("is_Change", true);
            anim.SetInteger("VAxisRaw", (int)v);
        }
        else
        {
            anim.SetBool("is_Change", false);
        }

        //Direction
        if (v_Down && v == 1)
            dirVec = Vector3.up;

        else if (v_Down && v == -1)
            dirVec = Vector3.down;

        else if (h_Down && h == -1)
            dirVec = Vector3.left;

        else if (h_Down && h == 1)
            dirVec = Vector3.right;

        //Scan Events
        if (Input.GetButtonDown("Jump") && scanEvents != null)
        {
            gameManager.Action(scanEvents);
        }

        //Mobile var Init
        up_Down = false;
        down_Down = false;
        left_Down = false;
        right_Down = false;
        up_Up = false;
        down_Up = false;
        left_Up = false;
        right_Up = false;

    }

    void FixedUpdate()
    {
        //대각선 이동 방지
        Vector2 moveVec = is_HorizonMove ? new Vector2(h, 0) : new Vector2(0, v);
        rigid.velocity = moveVec * speed;

        //Ray
        Debug.DrawRay(rigid.position, dirVec * 0.7f, new Color(0, 1, 0));
        RaycastHit2D rayHit = Physics2D.Raycast(rigid.position, dirVec, 0.7f, LayerMask.GetMask("Events")); 

        //콜리더 값이 있으면 = Events
        if(rayHit.collider != null)
        {
            scanEvents = rayHit.collider.gameObject;
        }
        else
        {
            scanEvents = null;
        }
    }

    public void ButtonDown(string type)
    {
        switch (type) {
            case "U":
                up_Value = 1;
                up_Down = true;
                break;
            case "D":
                down_Value = -1;
                down_Down = true;
                break;
            case "L":
                left_Value = -1;
                left_Down = true;
                break;
            case "R":
                right_Value = 1;
                right_Down = true;
                break;
            case "A":
                if(scanEvents != null)
                {
                    gameManager.Action(scanEvents);
                }
                break;
            case "C":
                gameManager.SubMenuActive();
                break;
        }
    }

    public void ButtonUp(string type)
    {
        switch (type)
        {
            case "U":
                up_Value = 0;
                up_Up = true;
                break;
            case "D":
                down_Value = 0;
                down_Up = true;
                break;
            case "L":
                left_Value = 0;
                left_Up = true;
                break;
            case "R":
                right_Value = 0;
                right_Up = true;
                break;
        }
    }
}
