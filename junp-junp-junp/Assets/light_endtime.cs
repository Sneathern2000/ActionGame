using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class light_endtime : MonoBehaviour
{
    [SerializeField] private Detection_Room Detection_RoomIn;
    [SerializeField] private Detection_Room Detection_RoomOut;
    private float time =0;
    private Light thislight;
    private bool On;
    // Start is called before the first frame update
    void Start()
    {
        thislight = this.GetComponent<Light>();
    }

    // Update is called once per frame
    void Update()
    {
        if (Detection_RoomIn.DetectionIn) On = true;
        if (Detection_RoomOut.DetectionOut) On = false;

        if (On)
        {

            if (time <= 0)
            {
                if (thislight.intensity == 0)
                {
                    thislight.intensity = 6;
                    time = Random.Range(0.1f, 5);
                }
                else
                {
                    thislight.intensity = 0;
                    time = Random.Range(0.1f, 7);
                }
            }
            time -= Time.deltaTime;
        }
        else
        {
            thislight.intensity = 0;
        }


    }
}
