using System.Collections;
using System.Collections.Generic;
using Unity.VisualScripting;
using UnityEngine;

public class VirtualObjectTrigger : MonoBehaviour
{
    public Cargo cargoManager;

    private void OnTriggerEnter(Collider other)
    {
        if (other.gameObject.CompareTag("Wall"))
        {
            cargoManager.isInsideTheWall = false;
            //Debug.Log("Enter");
        }
    }

    private void OnTriggerExit(Collider other)
    {
        if (other.gameObject.CompareTag("Wall"))
        {
            cargoManager.isInsideTheWall = true;
            //Debug.Log("Exit");
        }
    }
}
