using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class HandController : MonoBehaviour
{
    public float moveSpeed = 1f;
    public Transform equipPoint;

    // Update is called once per frame
    void Update()
    {
        if (Input.GetKey(KeyCode.W))
        {
            transform.Translate(transform.up * Time.deltaTime * moveSpeed);
        }
        else if (Input.GetKey(KeyCode.S))
        {
            transform.Translate(-transform.up * Time.deltaTime * moveSpeed);
        }
    }
}
