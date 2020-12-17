using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;


public class player : MonoBehaviour
{
    [SerializeField] private Wall_Right Wall_Right;//壁との当たり判定(右)
    [SerializeField] private wall_left Wall_left;//壁との当たり判定(左)
    [SerializeField] private In_flont in_flont;//壁との当たり判定(前)
    [SerializeField] private Ground Ground;//地面との衝突判定
    private Player_Audio Audio;//プレイヤー音響
    public Vector3 velocity;// 移動方向
    private Vector3 Jump;//ジャンプ代入
    public float moveSpeed;// 移動速度
    private float walk_time;//歩いた時間
    public Transform Rot;//Transform
    private Rigidbody rd;//Rigidbody
    private Vector3 roteto;//カメラ操作
    private bool Landing;
    public bool Gravitystop, Wonly, wallwalkbool, active;　//補助コード
    private float cameraX; //壁走り時のカメラを斜めにする動作
    private int MoveCamera = 0;
    float z;
    public bool Jump_wall; //壁はしりからの切り替え
    public float Cameraroteto;
    private int Player_wall_rotato;//角度変更の管理
    public float speed;
    private float slidingTimes;//スライディング継続時間
    private Vector3 sldvel; //スライディング用velosity
    private int ThisRotation;
    public float Junptime;


    // Use this for initialization
    void Start()
    {
        rd = GetComponent<Rigidbody>();
        Cursor.visible = false;
        Cursor.lockState = CursorLockMode.Locked;
        Audio = GetComponent<Player_Audio>();

    }



    // Update is called once per frame
    void Update()
    {
        //print(MoveCamera);
        if (Ground.ground) wallwalkbool = false;
        if (Jump_wall) // スクリプトの制御
        {
            Wall_Jump();
        }
        else
        {

            if (Wall_left.Left_Walls || Wall_Right.Right_Walls || in_flont.front_Walls != 0)
            {

                wallwalkbool = true;
                if (Wall_Right.Right_Walls)
                {
                    Rightwalk();
                    ThisRotation = 1;
                    if (MoveCamera == 0)
                    {
                        Camera_Move_wall(20);
                    }
                }


                if (Wall_left.Left_Walls)//左壁なら
                {
                    Leftwalk();
                    ThisRotation = 1;
                    if (MoveCamera == 0)
                    {
                        Camera_Move_wall(20);
                    }
                }

                if (in_flont.front_Walls == 1 || in_flont.front_Walls == 2)
                {
                    frontclimb();
                    ThisRotation = 1;
                    Camera_Move_wall(40);
                }

            }
            else
            {
                //地面の歩行モード
                Gravitystop = true;
                Camera_Move();
                Player_Move();
            }

        }
        /////////////////////////
        if (Wall_Right.Right_Walls || Wall_left.Left_Walls || in_flont.front_Walls == 1)
        {
            var step = speed * Time.deltaTime;
            if (Wall_Right.Right_Walls)
                transform.rotation = Quaternion.RotateTowards(transform.rotation, Quaternion.Euler(Wall_Right.Wall_angle), step);
            if (Wall_left.Left_Walls)
                transform.rotation = Quaternion.RotateTowards(transform.rotation, Quaternion.Euler(Wall_left.Wall_angle), step);
            if (in_flont.front_Walls == 1)
                transform.rotation = Quaternion.RotateTowards(transform.rotation, Quaternion.Euler(in_flont.Wall_angle), step);
        }
        /////////////////////////
        z = Rot.transform.localEulerAngles.z;
        if (MoveCamera == 1)
        {
            if (z <= 15)
            {
                Rot.transform.rotation = Rot.transform.rotation * Quaternion.Euler(0, 0, 1.8f);
            }
            else
            {
                z = 15;
                MoveCamera = 0;
            }
        }

        if (MoveCamera == 2)
        {
            if (z <= 345)
            {
                Rot.transform.rotation = Rot.transform.rotation * Quaternion.Euler(0, 0, -1.8f);
            }
            else
            {
                z = -15;
                MoveCamera = 0;
            }
        }

        ////////////////////////
    }//下の処理の制御スクリプト



    void Camera_Move()//視点操作＆視点可動域制限
    {
        float X_Rotation = Input.GetAxis("Mouse X");
        float Y_Rotation = Input.GetAxis("Mouse Y");
        Rot.transform.Rotate(-Y_Rotation, X_Rotation, 0);
        Vector3 Y = Rot.transform.localEulerAngles;
        if (Y.x >= 180)
            Y.x -= 360;
        Rot.transform.localEulerAngles = new Vector3(Mathf.Clamp(Y.x, -50, 75), 0, 0);
        roteto = new Vector3(0, X_Rotation * 2, 0);
        rd.transform.eulerAngles += roteto;
    }



    void Camera_Move_wall(float angle)//視点操作＆視点可動域制限
    {
        float X_Rotation = Input.GetAxis("Mouse X");
        float Y_Rotation = Input.GetAxis("Mouse Y");
        Rot.transform.Rotate(-Y_Rotation, X_Rotation, 0);
        Vector3 Y = Rot.transform.localEulerAngles;
        if (Y.x >= 180)
            Y.x -= 360;
        if (Y.y >= 180)
            Y.y -= 360;
        Rot.transform.localEulerAngles = new Vector3(Mathf.Clamp(Y.x, -angle, angle), Mathf.Clamp(Y.y, -20, angle), z);
        roteto = new Vector3(0, X_Rotation * 2, 0);
        Rot.transform.eulerAngles += roteto;
    }

    void Player_Move()//プレイヤー移動操作
    {

        if (ThisRotation == 1)//壁走りあとカメラの向きをplayerObjと同期する
        {
            transform.rotation = Quaternion.Euler(0, Cameraroteto, 0);
            MoveCamera = 0;
            ThisRotation = 2;
        }
        //壁走りした後地面に着地したら早さを8にする
        if (ThisRotation == 2 && Ground.ground)
        {
            moveSpeed = 8;
            ThisRotation = 0;
        }
        //////////////////////////////////////重力
        if (!Ground.ground)
        {
            rd.velocity -= new Vector3(0, 10 * Time.deltaTime, 0);
        }

        ///////////////////////////////////////移動
        velocity = Vector3.zero;
        if (Input.GetKeyDown(KeyCode.LeftControl) && moveSpeed >= 7) slidingTimes = 9;

        float angleDir = transform.eulerAngles.y * (Mathf.PI / 180.0f);
        Vector3 dir1 = new Vector3(Mathf.Sin(angleDir), 0, Mathf.Cos(angleDir));
        Vector3 dir2 = new Vector3(-Mathf.Cos(angleDir), 0, Mathf.Sin(angleDir));
        if (Input.GetKey(KeyCode.LeftControl))//スライディング処理
        {
            this.transform.localScale = new Vector3(1, 0.6f, 1);
            
            if (slidingTimes >= 2)
            {
                sldvel = Vector3.zero;
                sldvel.z += 1;
                sldvel = gameObject.transform.rotation * sldvel.normalized * slidingTimes * Time.deltaTime * 1.5f;
                slidingTimes -= Time.deltaTime * 7;
                transform.position += sldvel;
            }
            else
            {
                if (Input.GetKey(KeyCode.W))
                {
                    transform.position += dir1 * moveSpeed * Time.deltaTime;
                }
                if (Input.GetKey(KeyCode.A))
                {
                    transform.position += dir2 * moveSpeed * Time.deltaTime;
                }
                if (Input.GetKey(KeyCode.D))
                {
                    transform.position += -dir2 * moveSpeed * Time.deltaTime;
                }
                if (Input.GetKey(KeyCode.S))
                {
                    transform.position += -dir1 * moveSpeed * Time.deltaTime;
                }
            }
            // rd.velocity = new Vector3(sldvel.x, velocity.y, sldvel.z);
            // transform.position += sldvel;
        }
        else
        {
            this.transform.localScale = new Vector3(1, 1, 1);
            if (Input.GetKey(KeyCode.W))
            {
                transform.position += dir1 * moveSpeed * Time.deltaTime;
            }
            if (Input.GetKey(KeyCode.A))
            {
                transform.position += dir2 * moveSpeed * Time.deltaTime;
            }
            if (Input.GetKey(KeyCode.D))
            {
                transform.position += -dir2 * moveSpeed * Time.deltaTime;
            }
            if (Input.GetKey(KeyCode.S))
            {
                transform.position += -dir1 * moveSpeed * Time.deltaTime;
            }

        }
        //////////////////////////////////////
        if (Input.GetKey(KeyCode.A) || Input.GetKey(KeyCode.S) || Input.GetKey(KeyCode.D))
        { Wonly = false; }
        else { Wonly = true; }


        if (Input.GetKey(KeyCode.W) || Input.GetKey(KeyCode.A) || Input.GetKey(KeyCode.S) || Input.GetKey(KeyCode.D)) //動いたら...
        {
            ////////////////走り
            if(Input.GetKey(KeyCode.LeftControl) && moveSpeed >= 3  && Input.GetKey(KeyCode.W))
            {
                moveSpeed -= 5 * Time.deltaTime * 8;
                if (moveSpeed < 3) moveSpeed = 3;
            }
            else
            {
                if (Input.GetKey(KeyCode.LeftShift) && moveSpeed <= 8 && Ground.ground && Wonly && Input.GetKey(KeyCode.W) && !Input.GetKey(KeyCode.LeftControl))
                {
                    moveSpeed += Time.deltaTime * 10;
                    if (moveSpeed > 8) moveSpeed = 8;
                }
                if (!Input.GetKey(KeyCode.LeftShift) && moveSpeed >= 5 || !Input.GetKey(KeyCode.W) && !Input.GetKey(KeyCode.LeftControl))
                {
                    moveSpeed -= 5 * Time.deltaTime * 8;
                    if (moveSpeed < 5) moveSpeed = 5;
                }
            }
            //////////////////

            if (Input.GetKeyDown(KeyCode.Space) || !Ground.ground || slidingTimes >= 2)//spaceキー押してないなら
            { walk_time = 0; }
            else
            {
                //歩いている時間を計測（走ると倍）
                walk_time += Time.deltaTime * moveSpeed / 5;
            }
            if (!Ground.ground && moveSpeed < 0)//壁走り解除後のスピード補正
                moveSpeed = 5;
        }
        else
        {
            moveSpeed = 5;
            walk_time = 0;
        }
        //ジャンプ
        Jump = Vector3.zero;
        if (Input.GetKeyDown(KeyCode.Space) && Ground.ground)
        {
            Jump.y += 5;
            Audio.Player_Sound_janp();
        }
        if (Ground.ground)
        {
            if (Landing)
            {
                Audio.Player_Sound_Landing();
                Landing = false;
            }

        }
        else
        {
            Landing = true;

        }

        if (Jump.magnitude > 0)
        {
            rd.velocity += Jump;
        }
        if (walk_time > 0.5)//歩行時間が一定を超えたら
        {
            walk_time = 0;
            Audio.Player_Sound();
        }
    }



    void Leftwalk()
    {

        ////////////////////////////初期設定(一度のみ)//////////////////////////////////
        if (Gravitystop)
        {
            z = 0;
            MoveCamera = 2;
            cameraX = Rot.rotation.x;
            rd.isKinematic = true;
            Gravitystop = false;
            rd.isKinematic = false;
            moveSpeed = 10;
        }
        ////////////////////////////初期設定(一度のみ)//////////////////////////////////

        if (Input.GetKeyDown(KeyCode.Space))//space押されてないなら
        {
            Cameraroteto = Rot.eulerAngles.y;
            Jump_wall = true;
            moveSpeed = 5;
        }
        else
        {
            ///////////////////////////プレイヤーの動き/////////////////////////////////////
            velocity = Vector3.zero;
            if (Input.GetKey(KeyCode.W) && Input.GetKey(KeyCode.LeftShift))
            {
                velocity.z += 1;
                moveSpeed -= Time.deltaTime * 8;

            }
            else
            {
                moveSpeed = 0;
            }
            if (Input.GetKey(KeyCode.D))
                velocity.x += 1;

            velocity = gameObject.transform.rotation * velocity.normalized * 7 * Time.deltaTime;

            if (velocity.magnitude > 0) //動いたら...
            {
                transform.position += velocity; //移動操作をプレイヤーに代入
            }
            ///////////////////////////プレイヤーの動き/////////////////////////////////////

            ///////////////////////////走るサウンド/////////////////////////////////////////
            walk_time += Time.deltaTime * 2;
            if (walk_time > 0.5)//歩行時間が一定を超えたら
            {
                walk_time = 0;
                Audio.Player_Sound();
            }
            ///////////////////////////走るサウンド/////////////////////////////////////////

            ///////////////////////////壁走りの上昇＆下降///////////////////////////////////
            Jump = Vector3.zero;
            if (moveSpeed > 5)
            {
                Jump.y += 1;
                Jump = gameObject.transform.rotation * Jump.normalized * Time.deltaTime;
            }
            if (moveSpeed < 5 && moveSpeed > 4)
            {
                Jump.y += 1;
                Jump = gameObject.transform.rotation * Jump.normalized * Time.deltaTime / 2;
            }
            if (moveSpeed < 4 && moveSpeed > 3)
            {
                Jump.y += 1;
                Jump = gameObject.transform.rotation * Jump.normalized * Time.deltaTime / 3;
            }
            if (moveSpeed < 3 && moveSpeed > 2)
            {
                Jump.y -= 1;
                Jump = gameObject.transform.rotation * Jump.normalized * Time.deltaTime / 5;
            }
            if (moveSpeed < 2)
            {
                Jump.y -= 1;
                Jump = gameObject.transform.rotation * Jump.normalized * Time.deltaTime / 7;
            }
            if (Jump.magnitude > 0) //動いたら...
            {
                transform.position += Jump; //移動操作をプレイヤーに代入
            }
            ///////////////////////////壁走りの上昇＆下降///////////////////////////////////
        }



        if (moveSpeed < 0)
        {
            moveSpeed = 0;
        }
    }//壁移動左



    void Rightwalk()
    {

        ////////////////////////////初期設定(一度のみ)//////////////////////////////////
        if (Gravitystop)
        {

            //Rot.transform.Rotate(new Vector3(0, 0, 15));
            //transform.rotation = Quaternion.Euler(Wall_Right.Wall_angle);
            z = 0;
            MoveCamera = 1;
            cameraX = Rot.rotation.x;
            rd.isKinematic = true;
            Gravitystop = false;
            rd.isKinematic = false;
            moveSpeed = 10;
        }
        ////////////////////////////初期設定(一度のみ)//////////////////////////////////
        if (Input.GetKeyDown(KeyCode.Space))//space押されてないなら
        {
            Cameraroteto = Rot.eulerAngles.y;
            Jump_wall = true;
            moveSpeed = 5;
            //rd.AddForce(Rot.transform.forward * 30000);
        }
        else
        {
            ///////////////////////////プレイヤーの動き/////////////////////////////////////
            velocity = Vector3.zero;
            if (Input.GetKey(KeyCode.W) && Input.GetKey(KeyCode.LeftShift))
            {
                velocity.z += 1;
                moveSpeed -= Time.deltaTime * 8;

            }
            else
            {
                moveSpeed = 0;
            }
            if (Input.GetKey(KeyCode.A))
                velocity.x -= 1;

            velocity = gameObject.transform.rotation * velocity.normalized * 7 * Time.deltaTime;

            if (velocity.magnitude > 0) //動いたら...
            {
                transform.position += velocity; //移動操作をプレイヤーに代入
            }
            ///////////////////////////プレイヤーの動き/////////////////////////////////////

            ///////////////////////////走るサウンド/////////////////////////////////////////
            walk_time += Time.deltaTime * 2;
            if (walk_time > 0.5)//歩行時間が一定を超えたら
            {
                walk_time = 0;
                Audio.Player_Sound();
            }
            ///////////////////////////走るサウンド/////////////////////////////////////////

            ///////////////////////////壁走りの上昇＆下降///////////////////////////////////
            Jump = Vector3.zero;
            if (moveSpeed > 5)
            {
                Jump.y += 1;
                Jump = gameObject.transform.rotation * Jump.normalized * Time.deltaTime;
            }
            if (moveSpeed < 5 && moveSpeed > 4)
            {
                Jump.y += 1;
                Jump = gameObject.transform.rotation * Jump.normalized * Time.deltaTime / 2;
            }
            if (moveSpeed < 4 && moveSpeed > 3)
            {
                Jump.y += 1;
                Jump = gameObject.transform.rotation * Jump.normalized * Time.deltaTime / 3;
            }
            if (moveSpeed < 3 && moveSpeed > 2)
            {
                Jump.y -= 1;
                Jump = gameObject.transform.rotation * Jump.normalized * Time.deltaTime / 5;
            }
            if (moveSpeed < 2)
            {
                Jump.y -= 1;
                Jump = gameObject.transform.rotation * Jump.normalized * Time.deltaTime / 7;
            }
            if (Jump.magnitude > 0) //動いたら...
            {
                transform.position += Jump; //移動操作をプレイヤーに代入
            }
            ///////////////////////////壁走りの上昇＆下降///////////////////////////////////
        }



        if (moveSpeed < 0)
        {
            moveSpeed = 0;
        }
    }//壁移動右



    void frontclimb()
    {

        ////////////////////////////初期設定(一度のみ)//////////////////////////////////
        if (Gravitystop)
        {
            rd.isKinematic = true;
            Gravitystop = false;
            rd.isKinematic = false;
            moveSpeed = 10;
        }
        ////////////////////////////初期設定(一度のみ)//////////////////////////////////

        if (in_flont.front_Walls == 1)
        {

            if (Input.GetKeyDown(KeyCode.Space))//space押されてないなら
            {
                Cameraroteto = Rot.eulerAngles.y;
                Jump_wall = true;
                moveSpeed = 5;
            }
            else
            {
                ///////////////////////////プレイヤーの動き/////////////////////////////////////
                velocity = Vector3.zero;
                if (Input.GetKey(KeyCode.W))
                {
                    velocity.y += 1;
                    moveSpeed -= Time.deltaTime * 5;

                }
                else
                {
                    moveSpeed = 0;
                }
                if (Input.GetKey(KeyCode.D))
                {
                    velocity.z -= 1;
                    moveSpeed = 0;
                }
                velocity = gameObject.transform.rotation * velocity.normalized * 3 * Time.deltaTime;

                if (velocity.magnitude > 0) //動いたら...
                {
                    transform.position += velocity; //移動操作をプレイヤーに代入
                }
                ///////////////////////////プレイヤーの動き/////////////////////////////////////

                ///////////////////////////走るサウンド/////////////////////////////////////////
                walk_time += Time.deltaTime * 2;
                if (walk_time > 0.5)//歩行時間が一定を超えたら
                {
                    walk_time = 0;
                    Audio.Player_Sound();
                }
                ///////////////////////////走るサウンド/////////////////////////////////////////


            }
        }
        else
        {
            if (Junptime <= 0)
            {
                in_flont.front_Walls = 0;
            }
            else
            {
                velocity = Vector3.zero;
                velocity.y += 1;
                velocity = gameObject.transform.rotation * velocity.normalized * 3 * Time.deltaTime;
                transform.position += velocity;
                Junptime -= Time.deltaTime;
            }

        }
        if (moveSpeed < 0)//0以下になった時の補正用
        {
            moveSpeed = 0;
        }
    }//壁登り



    void Wall_Jump()//カメラの角度に変更
    {
        Jump = Vector3.zero;

        Jump.y += 5;
        Jump.x += Rot.transform.forward.x;
        Jump.z += Rot.transform.forward.z;
        Audio.Player_Sound_janp();
        rd.velocity += Jump;
        Jump_wall = false;
    }



    void wall_angle()//壁に衝突時にプレイヤーを壁にそって走るように角度の変更
    {
        var step = speed * Time.deltaTime;
        if (Player_wall_rotato == 1)
            transform.rotation = Quaternion.RotateTowards(transform.rotation, Quaternion.Euler(Wall_Right.Wall_angle), step);
        if (Player_wall_rotato == 2)
            transform.rotation = Quaternion.RotateTowards(transform.rotation, Quaternion.Euler(Wall_left.Wall_angle), step);

    }



}