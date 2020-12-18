using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class player : MonoBehaviour
{
    public GameObject Rot;
    private Vector3 roteto;
    private Rigidbody rd;
    private float speed = 7;
    // Start is called before the first frame update
    void Start()
    {
        rd = GetComponent<Rigidbody>();
        Cursor.visible = false;
        Cursor.lockState = CursorLockMode.Locked;
    }

    // Update is called once per frame
    void Update()
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

        Vector3 xzMove = (transform.forward * Input.GetAxis("Vertical") * speed) + (transform.right * Input.GetAxis("Horizontal") * speed);
        rd.velocity = new Vector3(xzMove.x, rd.velocity.y, xzMove.z);
    }

    void FixedUpdate()
    {

    }
}
