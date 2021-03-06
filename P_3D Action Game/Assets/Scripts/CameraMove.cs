using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class CameraMove : MonoBehaviour
{

    [SerializeField]
    private Transform cameraArm;

    [SerializeField]
    private Transform target;

    public float followSpeed;
    public float Sensitivity;

    void Update()
    {
        LookAround();
        followCam();
    }

    private void LookAround()
    {
        Vector2 mouseDelta = new Vector2(Input.GetAxis("Mouse X"), Input.GetAxis("Mouse Y")) * Sensitivity;
        Vector3 camAngle = cameraArm.rotation.eulerAngles;
        float x = camAngle.x - mouseDelta.y;
        
        if(x < 180f)
        {
            x = Mathf.Clamp(x, -1f, 70f);
        }
        else
        {
            x = Mathf.Clamp(x, 325f, 361f);
        }

        cameraArm.rotation = Quaternion.Euler(x, camAngle.y + mouseDelta.x, camAngle.z);
    }

    private void followCam()
    {
        transform.position = Vector3.MoveTowards(transform.position, target.position, followSpeed * Time.deltaTime);

        //finalDir = transform.TransformPoint(dirNormarized * maxDistance);

        //RaycastHit hit;
        //if (Physics.Linecast(transform.position, finalDir, out hit))
        //{
        //    finalDistance = Mathf.Clamp(hit.distance, minDistance, maxDistance);
        //}
        //else
        //{
        //    finalDistance = maxDistance;
        //}

        //realCamera.localPosition = Vector3.Lerp(realCamera.localPosition, dirNormarized * finalDistance, Time.deltaTime * smoothness);
    }

















    // first ?õ?
    //public Transform target;
    //public float followSpeed = 10f;
    //public float sensitivity = 100f;
    //public float clampAngle = 70f;

    //private float rotX;
    //private float rotY;

    //public Transform realCamera;
    //public Vector3 dirNormarized;
    //public Vector3 finalDir;
    //public float minDistance;
    //public float maxDistance;
    //public float finalDistance;
    //public float smoothness = 10f;
    //void Start()
    //{
    //    rotX = transform.localRotation.eulerAngles.x;
    //    rotY = transform.localRotation.eulerAngles.y;

    //    dirNormarized = realCamera.localPosition.normalized;
    //    finalDistance = realCamera.localPosition.magnitude;
    //    Debug.Log(finalDistance);
    //}

    //void Update()
    //{
    //    rotX += -(Input.GetAxis("Mouse Y")) * sensitivity * Time.deltaTime;    
    //    rotY += Input.GetAxis("Mouse X") * sensitivity * Time.deltaTime;

    //    rotX = Mathf.Clamp(rotX, -clampAngle, clampAngle);
    //    Quaternion rot = Quaternion.Euler(rotX, rotY, 0);
    //    transform.rotation = rot;
    //}

    //void LateUpdate()
    //{
    //    transform.position = Vector3.MoveTowards(transform.position, target.position, followSpeed * Time.deltaTime);

    //    finalDir = transform.TransformPoint(dirNormarized * maxDistance);

    //    RaycastHit hit;
    //    if (Physics.Linecast(transform.position, finalDir, out hit))
    //    {
    //        finalDistance = Mathf.Clamp(hit.distance, minDistance, maxDistance);
    //    }
    //    else
    //    {
    //        finalDistance = maxDistance;
    //    }

    //    realCamera.localPosition = Vector3.Lerp(realCamera.localPosition, dirNormarized * finalDistance, Time.deltaTime * smoothness);
    //}



}
