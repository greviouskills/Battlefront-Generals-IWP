using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class CameraMovement : MonoBehaviour
{
    // Start is called before the first frame update
    private Camera cam;
    [SerializeField]
    private float Defaultscrollspeed;
    private float scrollspeed;
    [SerializeField]
    private float zoomspeed;
    [SerializeField]
    private float defaultlimitX,defaultlimitY;

    void Start()
    {
        cam = GetComponent<Camera>();
        AddLimit(transform.position.y);
    }

    // Update is called once per frame
    void Update()
    {
        //AddLimit(cam.fieldOfView);
        
        if (Input.GetKey(KeyCode.W))
        {
            if(transform.position.z < defaultlimitY)
            {
                transform.position = new Vector3(transform.position.x, transform.position.y, transform.position.z + scrollspeed);
            }
        }
        if (Input.GetKey(KeyCode.S))
        {
            if (transform.position.z > -(defaultlimitY))
            {
                transform.position = new Vector3(transform.position.x, transform.position.y, transform.position.z - scrollspeed);
            }
        }
        if (Input.GetKey(KeyCode.D))
        {
            if (transform.position.x < defaultlimitX)
            {
                transform.position = new Vector3(transform.position.x + scrollspeed, transform.position.y, transform.position.z);
            }
        }
        if (Input.GetKey(KeyCode.A))
        {
            if (transform.position.x > -(defaultlimitX))
            {
                transform.position = new Vector3(transform.position.x - scrollspeed, transform.position.y, transform.position.z);
            }
        }
        if (Input.GetKey(KeyCode.Equals))
        {
            if(transform.position.y > 15)
            {
                transform.position = new Vector3(transform.position.x, transform.position.y - (zoomspeed*Time.deltaTime), transform.position.z);
                AddLimit(transform.position.y);
            }
        }
        if (Input.GetKey(KeyCode.Minus))
        {

            if (transform.position.y < 210)
            {
                transform.position = new Vector3(transform.position.x, transform.position.y + (zoomspeed * Time.deltaTime), transform.position.z);
                AddLimit(transform.position.y);
            }
        }

    }

    private void AddLimit(float val)
    {
        defaultlimitY = 680f - 1.283333f * val + 0.002f * Mathf.Pow(val, 2) - 0.000006666667f * Mathf.Pow(val, 3);
        defaultlimitX = 1060f - 13.7f * val + 0.122f * Mathf.Pow(val, 2) - 0.0004f * Mathf.Pow(val, 3);
        float percentage = val / 200;
        scrollspeed = Defaultscrollspeed*percentage;
    }
   
}
