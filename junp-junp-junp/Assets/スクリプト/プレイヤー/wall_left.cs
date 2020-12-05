using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class wall_left : MonoBehaviour
{
    [SerializeField] private player player;
    [SerializeField] private Ground Ground;
    private float maxDistance = 0.45f;//Rayの長さ
    public Vector3 Wall_angle;//向く角度
    public bool Left_Walls;//壁走りON
    public GameObject OBJ2;//法線参照用クローンOBJ
    public bool Access_left;//一定時間のアクセス権禁止
    private float Access_time; //カウント  

    // Start is called before the first frame update
    void Update()
    {
        Ray ray = new Ray(transform.position, transform.forward);
        RaycastHit hit;
        Debug.DrawRay(ray.origin, ray.direction * maxDistance, Color.red);
        if (Physics.Raycast(ray, out hit, maxDistance))
        {
            if (hit.collider.gameObject.tag == "wall" && !Ground.ground && player.moveSpeed >= 6 && Access_left)
            {
                /////////////////////////角度変更////////////////////////////////
                Quaternion rot = Quaternion.FromToRotation(transform.forward, hit.normal);

                GameObject cloneBox = Instantiate(OBJ2, hit.point, rot, hit.transform);

                cloneBox.transform.rotation = rot * transform.rotation;

                Wall_angle = cloneBox.GetComponent<Rigidbody>().transform.eulerAngles;
                Wall_angle.y -= 90;
                Wall_angle.z = 0;
                Wall_angle.x = 0;
                /////////////////////////角度変更////////////////////////////////
                Left_Walls = true;
            }
            if (player.moveSpeed == 0 || Ground.ground || hit.collider.gameObject.tag != "wall" || player.Jump_wall)
            {
                player.Cameraroteto = player.Rot.eulerAngles.y;
                Left_Walls = false;
            }

        }
        else
        {
            Left_Walls = false;
        }

        if (!Access_left)
        {
            if (Access_time >= 0)
            {
                Access_time -= Time.deltaTime;
            }
            else
            {
                Access_left = true;
            }
        }
    }
}
