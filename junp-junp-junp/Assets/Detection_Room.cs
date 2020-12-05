using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Detection_Room : MonoBehaviour
{
    [SerializeField] private bool In_or_Out;
    public bool DetectionIn;
    public bool DetectionOut;
    private float time = 5f;
    // Start is called before the first frame update
    void Start()
    {
        
    }

    // Update is called once per frame
    void Update()
    {
        if (DetectionIn || DetectionOut)
        {
            time -= Time.deltaTime;
            if(time <= 0)
            {
                time = 5;
                DetectionIn = false;
                DetectionOut = true;
            }
        }
    }


    void OnTriggerEnter(Collider other)
    {
        if (In_or_Out)
        {
            DetectionIn = true;
        }
        else
        {
            DetectionOut = true;
        }
    }
}
