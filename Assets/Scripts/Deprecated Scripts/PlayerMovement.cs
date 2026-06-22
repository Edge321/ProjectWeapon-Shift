using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PlayerMovement : MonoBehaviour
{
    public float walkSpeed = 0.3f;
    public float runSpeed = 0.5f;

    private float canRun;
    private float xMovement;
    private float zMovement;

    public static bool CanMove { get; set; } = true;

    private Animator animator;
    private void Awake()
    {
        animator = GetComponent<Animator>();
    }
    private void Update()
    {
        xMovement = Input.GetAxis("Horizontal");
        zMovement = Input.GetAxis("Vertical");
        canRun = Input.GetAxis("Shift");

        if (canRun > 0 && CanMove)
                WalkOrRun(runSpeed);
        else if (CanMove)
                WalkOrRun(walkSpeed);
        else
            //Set to 0 to prevent unwanted animation transitions
            animator.SetFloat("Speed", 0); 
        
    }
    /// <summary>
    /// Character walks or run depending on if shift key is pressed
    /// </summary>
    private void WalkOrRun(float speed)
    {
        float xSpeed = xMovement * speed * Time.deltaTime;
        float zSpeed = zMovement * speed * Time.deltaTime;
        transform.Translate(xSpeed, 0, zSpeed);
    }
}
