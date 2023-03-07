using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using Fusion;

public class Windmill2 : NetworkBehaviour
{
    private Animator anim;
    private int start_id;
    private bool isPlaying = false;

    // Start is called before the first frame update
    void Start()
    {
        print("Windmill2 Start() ½ÇÇà");
        anim = GetComponent<Animator>();
        start_id = Animator.StringToHash("Start");
        anim.speed = 0f;
    }

    public override void Render()
    {
        anim.SetBool(start_id, true);

        if (Input.GetKeyDown(KeyCode.R))
        {
            RPC_PlayAnimaion();
        }
    }

    [Rpc]
    public void RPC_PlayAnimaion()
    {
        // pause animation
        if (isPlaying)
        {
            isPlaying = false;
            anim.speed = 0f;
        }
        // play animation
        else
        {
            isPlaying = true;
            anim.speed = 1f;
        }
    }
}
