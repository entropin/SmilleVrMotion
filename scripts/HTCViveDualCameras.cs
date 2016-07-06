using UnityEngine;
using System.Collections;

public class HTCViveDualCameras : MonoBehaviour
{

    public Transform ThirdPersonController;
    public Transform HMDCamera;
    public Transform VRRigg;
    public Transform VRRiggMover;

    public bool isFirstPerson = true;
    public bool isCameraFading = false;

    //Camera vars
    public float height = 1.6f;
    public float distans = 3.0f;
    public float speed = 0.3f;


    public float zoomSpeed = 0.3f;
    public float minDistance = 0.0f;

    public float maxLeavay;
    public float maxRotationalLeavay;

    // Use this for initialization
    void Start()
    {

    }

    // Update is called once per frame
    void Update()
    {

    }

    void LateUpdate()
    {
        //We do some math to the lookAt point to add a min distance, and  min hight
        Vector3 extraHeight = new Vector3(0.0f, height, 0.0f);
        Vector3 extraDistance = new Vector3(0.0f, 0.0f, -distans);


        Vector3 targetDiff = VRRiggMover.position - HMDCamera.position;
        Vector3 targetPosition = ThirdPersonController.position + targetDiff;


        /*
         We change only the y acsess of the vrRiggs rotation, for that we use a Quaternion.
         A Quaternion is only rotation, a Quaternion can be multiplied på a vector to change the vectors rotaion in the world
        */
        Quaternion rotation = Quaternion.Euler(0.0f, ThirdPersonController.eulerAngles.y, 0.0f);

        //If we are in first personmode and dont have 
        if (isFirstPerson && VRRigg.parent == null)
        {
            VRRiggMover.position = (ThirdPersonController.position) + rotation * new Vector3(0.0f, 0.0f, -0.1f);
            var newRotation = Quaternion.LookRotation(ThirdPersonController.position - VRRiggMover.position).eulerAngles;
            newRotation.x = 0;
            newRotation.z = 0;
            VRRiggMover.rotation = Quaternion.Euler(newRotation);
        }

        //Do not active this script if in first persion mode
        if (isFirstPerson && !isCameraFading)
            return;


        Vector3 aV = ThirdPersonController.position;
        Vector3 bV = VRRiggMover.position;
        aV.y = 0f;
        bV.y = 0f;
        float dist = Vector3.Distance(aV, bV);

        // Debug.Log(dist);


        if (isFirstPerson && isCameraFading)
        {
            //Zoom variable is active lets zoom in

            //Animate in
            //transform.LookAt (new Vector3 (lookAt.position.x, transform.position.y, lookAt.position.z));
            VRRiggMover.position = Vector3.Lerp(VRRiggMover.position, ThirdPersonController.position + rotation * new Vector3(0, 0, 0), zoomSpeed);

            //Lets turn it of when the vrRigg hitts the spot
            if (dist <= 0.05f && isCameraFading)
            {
                isCameraFading = false;
                Debug.Log("Stoping ZoomIn");
                VRRigg.SetParent(null, true);

                //Snap to ground§
                //vrRigg.position = new Vector3(cameraRiggMover.position.x, lookAt.position.y, cameraRiggMover.position.z);

            }

        }
        else if (dist < (distans - 0.05f) && isCameraFading)  //If the distans is to close we animate out smootly
        {
            VRRigg.SetParent(VRRiggMover.transform, true);
            //Animate in
            //transform.LookAt (new Vector3 (lookAt.position.x, transform.position.y, lookAt.position.z));
            VRRiggMover.position = Vector3.Lerp(VRRiggMover.position, (ThirdPersonController.position + extraHeight) + rotation * extraDistance, zoomSpeed);
            //Lets turn it of when the vrRigg hitts the spot
            if (dist >= (distans - 0.1f))
            {
                isCameraFading = false;
                Debug.Log("Stoping ZoomOut");
            }
        }
        else
        {

            VRRigg.SetParent(VRRiggMover.transform, true);
            //Vector3.Lerp(currentPoss, wantedPoss, speed)
            VRRiggMover.position = Vector3.Lerp(VRRiggMover.position, (ThirdPersonController.position + extraHeight) + rotation * extraDistance, speed);

            var newRotation = Quaternion.LookRotation(ThirdPersonController.position - VRRiggMover.position).eulerAngles;
            newRotation.x = 0;
            newRotation.z = 0;

            VRRiggMover.rotation = Quaternion.Lerp(VRRiggMover.rotation, Quaternion.Euler(newRotation), speed);
        }

    }
}