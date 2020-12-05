using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Ground : MonoBehaviour
{
    public bool ground;
    [SerializeField]private GameObject Player;
    void OnTriggerStay(Collider other)
    {
        if (other.gameObject)
        {
            ground = true;
        }
        if(other.gameObject.tag == "Movefloor")
        {
           // Player.transform.parent = other.gameObject.transform;
        }
    }

    void OnTriggerExit(Collider other)
    {
        if (other.gameObject)
        {
            ground = false;
        }

    }
}
