using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Tool : MonoBehaviour
{
    private bool isEnter = false;
    private bool isEquip = false;
    private Player player;
    private Rigidbody rigid;

    // Start is called before the first frame update
    void Start()
    {
        rigid = GetComponent<Rigidbody>();
    }

    // Update is called once per frame
    void Update()
    {
        if (isEnter && Input.GetKeyDown(KeyCode.Space) && !isEquip)
        {
            Equip();
        }
    }

    private void OnTriggerEnter(Collider other)
    {
        if (other.CompareTag("Player") && !isEquip)
        {
            isEnter = true;
            player = other.transform.parent.GetComponent<Player>();
        }
    }

    private void OnTriggerExit(Collider other)
    {
        if (other.CompareTag("Player"))
        {
            isEnter = false;
            player = null;
        }
    }

    private void Equip()
    {
        rigid.useGravity = false;
        rigid.isKinematic = true;
        transform.SetParent(player.equipPoint);
        transform.localPosition = Vector3.zero;
        transform.localRotation = new Quaternion(0, 0, 0, 0);

        isEquip = true;
    }
}
