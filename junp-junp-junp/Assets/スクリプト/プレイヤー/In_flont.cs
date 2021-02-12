

using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class In_flont : MonoBehaviour
{
    [SerializeField] private player player;
    [SerializeField] private Ground Ground;
    [SerializeField] private Wall_Right Wall_Right;//壁との当たり判定(右)
    [SerializeField] private wall_left Wall_left;//壁との当たり判定(左)
    public Vector3 Wall_angle;//向く角度
    public GameObject OBJ2;//法線参照用クローンOBJ
    public float maxDistance = 0.25f;//Rayの長さ
    public bool Access_right;//一定時間のアクセス権禁止
    public bool Colider_Exit; //登り切ったかの判定
    public int front_Walls;//壁登りON
    private float Count;

    //// Start is called before the first frame update
    void Update()
    {
        if (Ground.ground) Access_right = true;
        //////////////////Rayでプレイヤーの角度補正かつ壁の衝突判定
        Ray ray = new Ray(transform.position, transform.forward);
        RaycastHit hit;
        Debug.DrawRay(ray.origin, ray.direction * maxDistance, Color.red);
        if (Physics.Raycast(ray, out hit, maxDistance))
        {

            if (hit.collider.gameObject.tag == "Junp_wall" && hit.collider.gameObject.GetInstanceID() != player.BlockID_Run && !Ground.ground && !Wall_left.Left_Walls && !Wall_Right.Right_Walls)
            {
                Count = 0.3f;
                /////////////////////////角度変更////////////////////////////////
                Quaternion rot = Quaternion.FromToRotation(transform.forward, hit.normal);

                GameObject cloneBox = Instantiate(OBJ2, hit.point, rot, hit.transform);

                cloneBox.transform.rotation = rot * transform.rotation;

                Wall_angle = cloneBox.GetComponent<Rigidbody>().transform.eulerAngles;
                Wall_angle.y += 180;
                Wall_angle.z = 0;
                Wall_angle.x = 0;
                /////////////////////////角度変更////////////////////////////////
                front_Walls = 1;//判定

                player.BlockID_Retention = hit.collider.gameObject.GetInstanceID();//壁のID保持
            }

            if (Ground.ground)// 
            {
                //player.Cameraroteto = player.Rot.localEulerAngles.y;
                front_Walls = 0;
                player.Cameraroteto = player.Rot.eulerAngles.y;
                player.BlockID_Run = player.BlockID_Retention;
            }
           
        }
        else
        {
            if (front_Walls == 1)
            {
                front_Walls = 2;
                player.Cameraroteto = player.Rot.eulerAngles.y;
                player.BlockID_Run = player.BlockID_Retention;
            }     
        }

        if(front_Walls == 3)
        {
            Count -= Time.deltaTime;

            if(Count <= 0)
            {
                front_Walls = 0;
            }
        }
        
    }
}

