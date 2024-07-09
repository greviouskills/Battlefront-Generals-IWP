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
    private CityManager city;
    [SerializeField]
    private TroopSync troop;
    public bool canmove;
    void Start()
    {
        cam = GetComponent<Camera>();
        AddLimit(transform.position.y);
        StartCoroutine(refresh());
    }

    // Update is called once per frame
    void Update()
    {
        //AddLimit(cam.fieldOfView);
        if (canmove)
        {
            if (Input.GetKey(KeyCode.W))
            {
                if (transform.position.z < 750)
                {
                    transform.position = new Vector3(transform.position.x, transform.position.y, transform.position.z + (scrollspeed * Time.deltaTime));
                }
            }
            if (Input.GetKey(KeyCode.S))
            {
                if (transform.position.z > -(750))
                {
                    transform.position = new Vector3(transform.position.x, transform.position.y, transform.position.z - (scrollspeed * Time.deltaTime));
                }
            }
            if (Input.GetKey(KeyCode.D))
            {
                if (transform.position.x < 750)
                {
                    transform.position = new Vector3(transform.position.x + (scrollspeed * Time.deltaTime), transform.position.y, transform.position.z);
                }
            }
            if (Input.GetKey(KeyCode.A))
            {
                if (transform.position.x > -(750))
                {
                    transform.position = new Vector3(transform.position.x - (scrollspeed * Time.deltaTime), transform.position.y, transform.position.z);
                }
            }
            if (Input.GetKey(KeyCode.Equals))
            {
                if (transform.position.y > 7)
                {
                    transform.position = new Vector3(transform.position.x, transform.position.y - (zoomspeed * Time.deltaTime), transform.position.z);
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
        

    }

    private void AddLimit(float val)
    {
        float percentage = val / 200;
        scrollspeed = Defaultscrollspeed*percentage;
    }
   
    public void ViewCity(string name)
    {
        GameObject target = city.Getcity(name).gameObject;
        transform.position = new Vector3(target.transform.position.x, 40, target.transform.position.z);
        AddLimit(transform.position.y);
    }
    public IEnumerator refresh()
    {
        troop.CameraCheck(transform.position, transform.position.y * 1.5f);
        yield return new WaitForSeconds(0.3f);
        StartCoroutine(refresh());
    }
}
