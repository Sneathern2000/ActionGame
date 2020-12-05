using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Player_Audio : MonoBehaviour
{
    [SerializeField] private AudioClip walk1;//足音
    [SerializeField] private AudioClip walk2;//足音
    [SerializeField] private AudioClip janp;//ジャンプ
    [SerializeField] private AudioClip Landing;//着地
    private AudioSource audioSource;//AudioSource
    private bool Sound;
    // Start is called before the first frame update
    void Start()
    {
        audioSource = GetComponent<AudioSource>();
    }

    // Update is called once per frame
    void Update()
    {

    }

    public void Player_Sound()//音響回り
    {
        if (Sound)
        {
            audioSource.PlayOneShot(walk1);
            Sound = false;
        }
        else
        {
            audioSource.PlayOneShot(walk2);
            Sound = true;
        }
    }

    public void Player_Sound_janp()//音響回り
    {
        audioSource.PlayOneShot(janp);
    }

    public void Player_Sound_Landing()//音響回り
    {
        audioSource.PlayOneShot(Landing);
    }

}
