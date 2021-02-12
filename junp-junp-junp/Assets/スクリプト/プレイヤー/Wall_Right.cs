using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Wall_Right : MonoBehaviour
{
    [SerializeField] private player player;
    [SerializeField] private Ground Ground;
    [SerializeField] private In_flont In_Flont;
    [SerializeField] private wall_left wall_left;
    private float maxDistance = 0.45f; //Rayの長さ
    public Vector3 Wall_angle;//向く角度
    public bool Right_Walls;//壁走りON
    public GameObject OBJ2;//法線参照用クローンOBJ
    public bool Access_right;//一定時間のアクセス権禁止
    public float Access_time; //カウント   

    // Start is called before the first frame update
    void Update()
    {
        Ray ray = new Ray(transform.position, transform.forward);
        RaycastHit hit;
        Debug.DrawRay(ray.origin, ray.direction * maxDistance, Color.red);
        if (Physics.Raycast(ray, out hit, maxDistance))
        {
            
            if (hit.collider.gameObject.tag == "Junp_wall" && !Ground.ground && player.moveSpeed >= 6 && hit.collider.gameObject.GetInstanceID() != player.BlockID_Run && !wall_left.Left_Walls && In_Flont.front_Walls ==0)
            {
                print("okkkkkkkkkk");
                /////////////////////////角度変更////////////////////////////////
                Quaternion rot = Quaternion.FromToRotation(transform.forward, hit.normal);
                
                GameObject cloneBox = Instantiate(OBJ2, hit.point, rot,hit.transform);
                
                cloneBox.transform.rotation = rot * transform.rotation;

                Wall_angle = cloneBox.GetComponent<Rigidbody>().transform.eulerAngles ;
                Wall_angle.y += 90;
                Wall_angle.z = 0;
                Wall_angle.x = 0;
                //Vector3 rot2 = cloneBox.transform.localEulerAngles;
                //cloneBox.transform.localEulerAngles = rot2;
                //rot2.x = -90*Mathf.Deg2Rad;
                //cloneBox.transform.rotation = rot2;
                /////////////////////////角度変更////////////////////////////////
                Right_Walls = true;

                player.BlockID_Retention = hit.collider.gameObject.GetInstanceID();//壁のID保持
            }
            if (player.moveSpeed == 0 || Ground.ground || hit.collider.gameObject.tag != "Junp_wall" || player.Jump_wall)
            {
                //player.Cameraroteto = player.Rot.localEulerAngles.y;
                Right_Walls = false;
                player.Cameraroteto = player.Rot.eulerAngles.y;
                player.BlockID_Run = player.BlockID_Retention;
            }
            if (player.Jump_wall)
            {
                Access_right = false;
                Access_time = 1;
            }

        }
        else
        {
            Right_Walls = false;
        }

        if (!Access_right)
        {
            if (Access_time >= 0)
            {
                Access_time -= Time.deltaTime;
            }
            else
            {
                Access_right = true;
            }
           
        }
    }
}
