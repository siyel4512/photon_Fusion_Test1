using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using Fusion;

public class Windmill2 : NetworkBehaviour
{
    private Animator anim;
    private int start_id;
    private bool isPlaying = false;
    [Networked] public bool S { get; set; }

    // Start is called before the first frame update
    void Start()
    {
        print("Windmill2 Start() 실행");
        anim = GetComponent<Animator>();
        start_id = Animator.StringToHash("Start");
        anim.speed = 0f;
    }

    public override void Render()
    {
        anim.SetBool(start_id, true);

        if (Input.GetKeyDown(KeyCode.R))
        {
            if (isPlaying)
            {
                isPlaying = false;
                print("애니메이션 중지");
                //anim.SetBool(start_id, isPlaying);
                anim.speed = 0f;
            }
            else
            {
                isPlaying = true;
                print("애니메이션 재생");
                //anim.SetBool(start_id, isPlaying);
                anim.speed = 1f;
            }
        }
    }
}
