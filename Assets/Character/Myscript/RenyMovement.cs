using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class RenyMovement : MonoBehaviour
{

    Animator anim;
    CharacterController characterController;
    void Start() 
    {
        anim = GetComponent<Animator>();
        characterController = GetComponent<CharacterController>();

        anim.SetBool("iswalking", false);
        anim.SetBool("isdancing", false);
        anim.SetBool("isdef", false);
    }

    void Update()
    {
        

        if (Input.GetKey(KeyCode.W))
        {
            anim.SetBool("iswalking", true);
            anim.SetBool("isdef", false);
            anim.SetBool("isdancing", false);
        }
        else if (Input.GetKey(KeyCode.D))
        {
            anim.SetBool("isdancing", true);
            anim.SetBool("iswalking", false);
            anim.SetBool("isdef", false);
        }
        else if (Input.GetKey(KeyCode.S))
        {
            anim.SetBool("isdef", true);
            anim.SetBool("iswalking", false);
            anim.SetBool("isdancing", false);
        }
    }
}
// 