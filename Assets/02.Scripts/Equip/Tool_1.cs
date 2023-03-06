using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Tool_1 : MonoBehaviour
{
    private bool isEnter = false;
    private HandController hand;
    private Rigidbody rigid;

    // Start is called before the first frame update
    void Start()
    {
        rigid = GetComponent<Rigidbody>();    
    }

    // Update is called once per frame
    void Update()
    {
        if (isEnter && Input.GetKeyDown(KeyCode.Space))
        {
            print("Equip This tool");
            Equip();
        }
    }

    private void OnTriggerEnter(Collider other)
    {
        Debug.Log($"name : {other.name} / tag : {other.tag}");

        if (other.CompareTag("Player"))
        {
            Debug.Log($"플레이어 이름 : {other.transform.name}");
            isEnter = true;
            hand = other.GetComponent<HandController>();
        }
    }

    private void OnTriggerExit(Collider other)
    {
        if (other.CompareTag("Player"))
        {
            isEnter = false;
            hand = null; 
        }
    }

    private void Equip()
    {
        rigid.useGravity = false;
        rigid.isKinematic = true;
        transform.SetParent(hand.equipPoint);
        transform.localPosition = Vector3.zero;
        transform.localRotation = new Quaternion(0, 0, 0, 0);
    }
}
