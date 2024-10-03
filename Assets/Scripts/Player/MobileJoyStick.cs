using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.EventSystems;

public class MobileJoyStick : MonoBehaviour, IPointerDownHandler, IPointerUpHandler
{
    public int verticalAxis = 0;
    public int horizontalAxis = 0;

    [Header("How much to move joystick to register movement")]
    [SerializeField] float XDeadZone;
    [SerializeField] float YDeadZone;

    [Header("Joystick restrictions")] 

    [SerializeField] float minClampX,maxClampX;
    [SerializeField] float minClampY,maxClampY;


    bool isPressed = false;
    [Header("Joystick middle")]
    [SerializeField] Transform joystickStick;
    Vector3 defaultJoystickPos;
    int touchIndex = -1;

    void Start()
    {
        defaultJoystickPos = joystickStick.transform.position;
        YDeadZone = Screen.height * YDeadZone;
        XDeadZone = Screen.height * XDeadZone;
    }

    public void OnPointerDown(PointerEventData eventData)
    {
       // Debug.Log("Pressed Joystick");        
        isPressed = true;        
        
    }

    void Update()
    {
      //  Debug.Log("Joystick ind:" + touchIndex + " Len: " +Input.touchCount);

        if(isPressed)
        {
            foreach(Touch t in Input.touches)
            {
                
                float distance = Vector2.Distance(joystickStick.position,t.position) / Screen.height;
                Debug.Log(distance);
                if(distance< 0.3f)
                {
                    
                    touchIndex = t.fingerId;
                    Debug.Log(touchIndex);
                    break;
                }
            }
            if(touchIndex >= Input.touchCount)
            {
                touchIndex = Input.touchCount -1;
            }
            joystickStick.position = Input.GetTouch(touchIndex).position;

            
            joystickStick.position = new Vector2(Mathf.Clamp(joystickStick.position.x,Screen.width * minClampX,Screen.width * maxClampX), Mathf.Clamp(joystickStick.position.y,Screen.height * minClampY,Screen.height * maxClampY));
           // Debug.Log(joystickStick.position.x/Screen.width + "  ---  " + joystickStick.position.y / Screen.height );
            //100-300
            JoystickMove();
        }

    }

    public void OnPointerUp(PointerEventData eventData)
    {
        

        isPressed = false;
        joystickStick.position = defaultJoystickPos;
        horizontalAxis = 0;
        verticalAxis = 0;
    }

    // Set different vertical and horizontal axis depending on where the joystick position is.
    void JoystickMove()
    {

        if(joystickStick.position.x > defaultJoystickPos.x + XDeadZone)
        {
            verticalAxis = 1;
        }
        else if(joystickStick.position.x< defaultJoystickPos.x - XDeadZone)
        {
            verticalAxis = -1;
        }
        else
        {
            verticalAxis =0;
        }

        if(joystickStick.position.y > defaultJoystickPos.y + YDeadZone)
        {
            horizontalAxis = 1;
        }
        else if(joystickStick.position.y< defaultJoystickPos.y - YDeadZone)
        {
            horizontalAxis = -1;
        }
        else
        {
            horizontalAxis = 0;
        }
        
    }
}
