using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Windmill2 : MonoBehaviour
{
    private Animator anim;
    private int start_id;

    // Start is called before the first frame update
    void Start()
    {
        anim = GetComponent<Animator>();
        start_id = Animator.StringToHash("Start");
    }

    // Update is called once per frame
    void Update()
    {
        if (Input.GetKeyDown(KeyCode.R))
        {
            print("�ִϸ��̼� ���");
            anim.SetBool(start_id, true);
        }
    }
}
