using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PlayerJump : MonoBehaviour
{
    public float jumpMax = 4f;
    public float jumpForce = 30f;

    //To be changed after new model done
    public Vector3[] feetPoints;
    public float groundDistance = 0.5f;

    private float jumpHeld = 0;
    private float jumpMinimum = 1;

    private PlayerAttack playerAttack;
    private Rigidbody rb;
    public bool HasJumped { get; private set; } = false;
    private void Start()
	{
		rb = GetComponent<Rigidbody>();
        playerAttack = GetComponent<PlayerAttack>();
	}
	void Update()
    {
        //TODO: think about how roll, dodge, and attack affected by jump in terms of physics
        //if (HasJumped)
        //    playerMovement.CanMove = false;
        GroundCheck();

        if (Input.GetKey(KeyCode.Space) && !HasJumped &&
            (playerAttack.GetAttackStatus() != WeaponStatus.ATTACK))
        {
            jumpHeld += Time.deltaTime;
        }
        else if (Input.GetKeyUp(KeyCode.Space) && !HasJumped &&
            (playerAttack.GetAttackStatus() != WeaponStatus.ATTACK))
        {
            HasJumped = true;
            if (jumpHeld < jumpMinimum)
                Jump();
            else
                HoldJump(Mathf.Clamp(jumpHeld, jumpMinimum, jumpMax));
            jumpHeld = 0;
        }
        else
            jumpHeld = 0;
    }
    private void HoldJump(float force)
	{
        rb.AddForce(Vector3.up * (force * jumpForce), ForceMode.Impulse);
    }
    private void Jump()
	{
        rb.AddForce(Vector3.up * jumpForce, ForceMode.Impulse);
    }
    private void GroundCheck()
	{
        for (int i = 0; i < feetPoints.Length; i++)
        {
            Vector3 playerFeet = transform.position + feetPoints[i];
            if (Physics.Raycast(playerFeet, Vector3.down, groundDistance))
			{
                HasJumped = false;
                break;
            }
            else
                HasJumped = true;
        }
	}
	private void OnDrawGizmosSelected()
	{
        for (int i = 0; i < feetPoints.Length; i++)
        {
            Vector3 playerFeet = transform.position + feetPoints[i];
            Gizmos.DrawLine(playerFeet, playerFeet + (Vector3.down * groundDistance));
        }
    }
}
