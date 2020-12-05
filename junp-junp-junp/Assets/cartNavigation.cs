using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class cartNavigation : MonoBehaviour
{
    private int carryCount;
    [SerializeField] private player_Detection player_Detection;
    public int Count = 1;
    [SerializeField] GameObject[] carryGameobject;
    private bool ReturnPath = false;
    // Start is called before the first frame update
    void Start()
    {
        carryCount = carryGameobject.Length;
    }

    // Update is called once per frame
    void Update()
    {
            if (player_Detection.Play)
            {
                if (this.transform.position == carryGameobject[Count].transform.position)
                {
                    if (ReturnPath)
                    {
                        Count--;
                        if (Count < 0)
                        {
                            Count = 1;
                            ReturnPath = false;
                        }
                    }
                    else
                    {
                        Count++;
                        if (Count >= carryCount)
                        {
                            Count = carryCount - 2;
                            ReturnPath = true;
                        }
                    }
                }
                this.transform.position = Vector3.MoveTowards(this.transform.position, carryGameobject[Count].transform.position, Time.deltaTime * 1);
            }
            else
            {
                this.transform.position = Vector3.MoveTowards(this.transform.position, carryGameobject[0].transform.position, Time.deltaTime * 1);
                Count = 1;
            }
            
    }

}

