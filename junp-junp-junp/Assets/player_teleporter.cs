using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class player_teleporter : MonoBehaviour
{
    private GameObject player;
    [SerializeField] GameObject teleport_position;
    // Start is called before the first frame update
    void Start()
    {
        player = GameObject.FindWithTag("Player");
    }

    // Update is called once per frame
    void Update()
    {
        if(player.transform.position.y < -20)
        {
            player.transform.position = teleport_position.transform.position;
        }
    }
}
