using System;
using System.Collections;
using System.Collections.Generic;
using System.Runtime.CompilerServices;
using UnityEditor;
using UnityEngine;
using UnityEngine.UIElements;
using Cursor = UnityEngine.Cursor;

public class DavidController : MonoBehaviour
{
    [SerializeField] private float speed;
    [SerializeField] private Animator davidAnimator;
    private static readonly int Speed = Animator.StringToHash("Speed");
    [SerializeField] private Camera m_camera;
    [SerializeField] private float rotationSpeed = 45;
    [SerializeField] private float stamina;
    [SerializeField] private float maxStamina;
    public float staminaRate;
    private bool canRun;
    private bool mustRecover;
    private bool canRecover;



    private void Start()
    {
        maxStamina = 5;
        stamina = maxStamina;
        staminaRate = 1;
        mustRecover = false;
        canRecover = true;

    }
    private void IncreaseStamina()
    {
            stamina += staminaRate * Time.deltaTime; 
    }

    private void OnApplicationFocus(bool hasFocus)
    {
        Cursor.visible = !hasFocus;
        Cursor.lockState = hasFocus ? CursorLockMode.None : CursorLockMode.Confined;
    }

    private void RunningCooldown()
    {
        if(mustRecover == true)
        {
            canRecover = false;
            Invoke("Recover", 4.5f);
        }
    }
    private void Recover()
    {
        canRecover = true;
        mustRecover = false;
    }

    void Update()
    {
        // Stamina 

        RunningCooldown();


        if (stamina < maxStamina && canRecover == true)
        {
            IncreaseStamina();
        }
        if (stamina <= 1)
        {
            mustRecover = true;
            canRun = false;

        } else
        {
            canRun = true;
        }


        Move(GetMovementInput());
        Rotate(GetRotationInput());

        // Animations

        // Running
        if (Input.GetKey(KeyCode.LeftShift) && Input.GetAxis("Vertical") > 0 && canRun == true)
        {
            davidAnimator.SetFloat(Speed, 2f);
            speed = 1.8f;
            stamina -= (staminaRate * 2) * Time.deltaTime;

        } else
        {
            speed = 0.8f;
        }
        // Running Cooldown 
        

        // Backwards
        if (Input.GetAxis("Vertical") < 0)
        {
            davidAnimator.SetFloat(Speed, -0.1f);
        } else
        {
           
        }
        
        // Crouching
        if (Input.GetKey(KeyCode.LeftControl)) {

            davidAnimator.SetBool("isCrouched", true);
            speed = 0;
        } else
        {
            davidAnimator.SetBool("isCrouched", false);
        }


    }

    private void Rotate(Vector2 p_scrollDelta)
    {
        transform.Rotate(Vector3.up, p_scrollDelta.x * rotationSpeed * Time.deltaTime, Space.Self);
    }

    private Vector2 GetRotationInput()
    {
        var l_mouseX = Input.GetAxis("Mouse X");
        var l_mouseY = Input.GetAxis("Mouse Y");
        return new Vector2(l_mouseX, l_mouseY);
    }

    private Vector3 GetMovementInput()
    {
        var l_horizontal = Input.GetAxis("Horizontal");
        var l_vertical = Input.GetAxis("Vertical");
        return new Vector3(l_horizontal, 0, l_vertical).normalized;
        
    }

    public void Move(Vector3 p_inputMovement)
    {
        var transform1 = transform;
        transform1.position += (p_inputMovement.z * transform1.forward + p_inputMovement.x * transform1.right) *
                               (speed * Time.deltaTime);
        davidAnimator.SetFloat(Speed, p_inputMovement.magnitude);
    }

}