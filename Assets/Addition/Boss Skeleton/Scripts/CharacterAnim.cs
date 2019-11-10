using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class CharacterAnim : MonoBehaviour
{
    public static Animator anim;
    void Start()
    {
        anim = GetComponent<Animator>();
    }

    void Update()
    {
        switch (CharacterLogic.CurrentAnimation)
        {
            case CharacterLogic.allAnimations.Staying:
                anim.SetBool("IsRunning", false);
                break;
            case CharacterLogic.allAnimations.Running:
                anim.SetBool("IsRunning", true);
                break;
            case CharacterLogic.allAnimations.Attack:
                anim.SetTrigger("DoAttack");
                anim.SetBool("IsRunning", false);
                break;
            case CharacterLogic.allAnimations.Dying:
                anim.SetTrigger("IsDead");
                break;
            case CharacterLogic.allAnimations.GetHit:
                anim.SetTrigger("GetHit");
                anim.SetBool("IsRunning", false);
                break;
        }
    }
}
