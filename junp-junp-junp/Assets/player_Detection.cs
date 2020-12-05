using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class player_Detection : MonoBehaviour
{
    [SerializeField] cartNavigation cartNavigation;
    public bool Play;
    [SerializeField] private GameObject light;
    private Light lt;
    private bool insity;
    // Start is called before the first frame update
    void Start()
    {
        lt = light.GetComponent<Light>();
    }

    // Update is called once per frame
    void Update()
    {
        if (Play)
        {
            if(lt.intensity <= 13 && insity == false)
            {
                lt.intensity += Time.deltaTime * 6;
                
            }
            else
            {
                insity = true;
            }
            if(lt.intensity != 0 && insity == true)
            {
                lt.intensity -= Time.deltaTime * 6;
            }
            else
            {
                insity = false;
            }
        }
        else
        {
            if (lt.intensity <= 13)
            {
                lt.intensity += Time.deltaTime * 6;

            }
        }

        if(cartNavigation.Count == 7)
        {
            lt.color = Color.green;

        }
        else
        {
            lt.color = Color.red;
        }
    }    
    void OnTriggerStay(Collider other)
    {
        if(other.gameObject.tag == "Player")Play = true;
    }

    void OnTriggerExit(Collider other)
    {
        Play = false;
    }

}

