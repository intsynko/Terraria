using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public delegate void Void(float a);

public class PlayerMovement : MonoBehaviour, IEntity {

    public float Hp = 100;
    private float fullHp;
    public event Void UpdateHp;
	public CharacterController2D controller;
    public Animator animator;

	public float runSpeed = 40f;

	float horizontalMove = 0f;
	bool jump = false;
    

    private void Start()
    {
        fullHp = Hp;
    }

    void Update () {

		horizontalMove = Input.GetAxisRaw("Horizontal") * runSpeed;

        animator.SetFloat("Speed", Mathf.Abs(horizontalMove));

		if (Input.GetButtonDown("Jump")&& !jump)
		{
			jump = true;
            animator.SetBool("IsJamping", true);

		}
        


	}

    public void OnLanding() {
        animator.SetBool("IsJamping", false);
    }

	void FixedUpdate ()
	{
		// Move our character
		controller.Move(horizontalMove * Time.fixedDeltaTime, false, jump);
		jump = false;
	}


    public void Damage(float damage)
    {
        Hp -= damage;
        UpdateHp(Hp / fullHp);
    }
}
