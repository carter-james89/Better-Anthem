using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class MechController : MonoBehaviour
{
    Rigidbody rb;
    public Transform model;

    public GameObject footRockets;

    public enum CamMode { ThirdPerson, Turret }
    public CamMode camMode;

    //TODO left trigger cam switch
    //material on building

    // Start is called before the first frame update
    void Start()
    {
        status = Status.Standing;
        rb = GetComponent<Rigidbody>();
        footRockets.SetActive(false);
    }


    bool transitioning = false;
    Vector3 targetEuler = Vector3.zero;
    Vector3 cameraTarget;
    Vector3 modelTarget;
    float transitionTime;
    public Transform navBall;

    public Camera turretCam;

    public GameObject turret;
    void Update()
    {

        var leftTrigger = Input.GetAxis("Left Trigger");
        if (leftTrigger > 0 & camMode != CamMode.Turret)
        {
            camMode = CamMode.Turret;
            turretCam.enabled = true;
            cam.enabled = false;
        }
        else if (leftTrigger == 0 & camMode != CamMode.ThirdPerson)
        {
            camMode = CamMode.ThirdPerson;
            turretCam.enabled = false;
            cam.enabled = true;
        }
        // if (Input.GetKeyDown(KeyCode.Space))
        if (Input.GetKeyDown(KeyCode.Joystick1Button0) & (status == Status.Jumping || status == Status.Standing))
        {
            Debug.Log("a button");
            // FindObjectOf.Type<AnimatorIK;
            Jump();
        }

        turret.transform.rotation = transform.rotation;
        navBall.rotation = Quaternion.LookRotation(Vector3.up, Vector3.forward);


        leftStickY = Input.GetAxis("Vertical");
        rightStickX = Input.GetAxis("Right Horizontal");
        rightStickY = Input.GetAxis("Right Vertical");
        var leftStickX = Input.GetAxis("Horizontal");

        //    if(left)

        if (camMode == CamMode.ThirdPerson)
        {
            //Pitch
            transform.Rotate(Vector3.right, rightStickY * Time.deltaTime * -30);
            // transform.Rotate(Vector3.forward, rightStickX * Time.deltaTime * -rotSpeed);
            //  transform.Rotate(transform.forward, rightStickX * -Time.deltaTime * 50);
            //Roll
            transform.RotateAround(transform.forward, rightStickX * -Time.deltaTime * 5);
            //Yaw
            if (leftStickY < .9f)
                transform.Rotate(Vector3.up, leftStickX * Time.deltaTime * 30);

            if (Input.GetKeyDown(KeyCode.JoystickButton5) & status == Status.Flying)
            {
                boost = true;
            }


            //Move the camera with the stick
            var targetCamRot = Vector3.zero;
            if (rightStickY > 0)
            {
                targetCamRot = new Vector3(-15, 0, 0);
            }
            else if (rightStickY < 0)
            {
                targetCamRot = new Vector3(15, 0, 0);
            }
            // var targetEuler = Vector3.Lerp(Vector3.zero, targetCamRot, Math.Abs(rightStickY));
            //camAnchor.transform.localEulerAngles = Vector3.Lerp(camAnchor.transform.localEulerAngles, targetEuler, Time.deltaTime * 10);

            camAnchor.localRotation = Quaternion.Lerp(camAnchor.localRotation, Quaternion.Euler(targetEuler), Time.deltaTime / 2);

            camAnchor.transform.localEulerAngles = Vector3.Lerp(Vector3.zero, targetCamRot, Math.Abs(rightStickY));

            var hoverRot = transform.rotation;
            var flyingRot = Quaternion.LookRotation(-transform.up, transform.forward);
            if (leftStickY > 0)
            {
                flyingRot = Quaternion.LookRotation(-transform.up, transform.forward);
            }
            else if (leftStickY < 0)
            {
                flyingRot = Quaternion.LookRotation(transform.up, -transform.forward);
            }
            var setRot = Quaternion.Lerp(hoverRot, flyingRot, Math.Abs(leftStickY));
            model.transform.rotation = Quaternion.Lerp(model.rotation, setRot, Time.deltaTime * 10);



            if (status != Status.Standing && status != Status.Jumping)
                if (Math.Abs(leftStickY) < .2f & status != Status.Hovering & Vector3.Angle(model.up, transform.up) < 20)
                {
                    status = Status.Hovering;
                }
                else if (Math.Abs(leftStickY) >= .2f & status != Status.Flying)
                {
                    status = Status.Flying;
                }
        }

        model.transform.position = Vector3.Lerp(model.transform.position, transform.position, Time.deltaTime * 100);

        var camPos = Vector3.Lerp(startCamPos, endCamPos, leftStickY);
        cam.transform.localPosition = camPos;
    }


    private void Awake()
    {
        startCamPos = cam.transform.localPosition;
        endCamPos = cam.transform.localPosition - new Vector3(0, 0, 3);
    }
    bool boost;
    public Camera cam;
    Vector3 startCamPos, endCamPos;
    public Transform camAnchor;
    public TMPro.TextMeshProUGUI velText;
    private void FixedUpdate()
    {
        velText.text = rb.velocity.magnitude.ToString();
        switch (status)
        {
            case (Status.Jumping):
                break;
            case (Status.Hovering):
                var hoverAngle = Vector3.Angle(model.up, Vector3.up);

                if (hoverAngle < 20 & !footRockets.activeInHierarchy)
                {
                    footRockets.SetActive(true);
                }
                else if (hoverAngle > 20 & footRockets.activeInHierarchy)
                {
                    footRockets.SetActive(false);
                }
                if (hoverAngle < 20 & rb.velocity.y <= 0)
                {
                    float yVel1 = rb.velocity.y + Physics.gravity.y;
                    //Debug.Log("up velocity " + -yVel1);
                    rb.AddForce(new Vector3(0, Math.Abs(yVel1), 0), ForceMode.Acceleration);
                }

                if (leftStickY == 0)
                {
                    var tempEuler = transform.rotation;
                    tempEuler.x = Mathf.Lerp(tempEuler.x, 0, Time.deltaTime * 10);
                    tempEuler.z = Mathf.Lerp(tempEuler.z, 0, Time.deltaTime * 10);
                    transform.rotation = tempEuler;
                }

                break;
            case (Status.Flying):
                if (!footRockets.activeInHierarchy)
                {
                    footRockets.SetActive(true);
                }

               // if (Vector3.Angle(model.up, transform.up) > 20)
             //   {
                    rb.AddForce(Vector3.down * 1000, ForceMode.Force);
               // }

                if (leftStickY == 0)
                {
                    var tempEuler = transform.rotation;
                    tempEuler.x = Mathf.Lerp(tempEuler.x, 0, Time.deltaTime);
                    tempEuler.z = Mathf.Lerp(tempEuler.z, 0, Time.deltaTime);
                    transform.rotation = tempEuler;
                }

                //Boost
                if (boost)
                {
                    Debug.Log("boost");
                    rb.AddForce(model.up * 20000, ForceMode.Acceleration);
                    boost = false;
                }



                //if(leftStickY > 0)
                //{
                float yVel = rb.velocity.y + Physics.gravity.y;
                var throttle = Mathf.Lerp(yVel, 90000, Math.Abs(leftStickY));
                rb.AddForce(model.up * throttle, ForceMode.Force);
                //}
                //else
                //{
                //    rb.velocity = Vector3.zero;
                //}


                if (Input.GetKey(KeyCode.Joystick1Button4))
                {
                    // rb.AddForce(0, -50, 0, ForceMode.Force);
                    rb.AddForce(model.up * 200);
                }
                //else if (Input.GetKey(KeyCode.Joystick1Button5))
                //{
                //    //rb.AddForce(0, 50, 0, ForceMode.Force);
                //    rb.AddForce(model.up * 200);
                //    footRockets.SetActive(true);
                //}
                break;
        }

    }
    float jumpTime = 0;
    float jumpLength = 5;

    enum Status { Standing, Hovering, Jumping, HoverForward, Flying }
    Status status = Status.Standing;
    private float leftStickY;
    private float rightStickX;
    private float rightStickY;

    void Jump()
    {
        Debug.Log("Jump 0");
        Debug.Log(status.ToString());
        if (status == Status.Standing)
        {
            Debug.Log("Jump");
            GetComponent<Rigidbody>().AddForce(Vector3.up * 500, ForceMode.Acceleration);
            status = Status.Jumping;
        }
        else if (status == Status.Jumping)
        {
            status = Status.Hovering;
            // FindObjectOfType<AnimatorIK>().Hover();
            Debug.Log("Hovering");
        }

    }
}
//if (Input.GetKeyDown(KeyCode.Space))
//{
//    transitioning = true;
//    // transform.localEulerAngles -= new Vector3(90, 0, 0);
//    //  model.localEulerAngles = new Vector3(90, 0, 0);

//    modelTarget = new Vector3(90, 0, 0);
//    var eulerAngles = transform.eulerAngles;
//    cameraTarget = eulerAngles - new Vector3(90, 0, 0);
//    transitionTime = 0;
//}

//if (transitioning)
//{
//    transform.rotation = Quaternion.Lerp(Quaternion.Euler(transform.eulerAngles), Quaternion.Euler(cameraTarget), Time.deltaTime);
//    transitionTime += Time.deltaTime;

//    if (transitionTime > 2)
//    {
//        transitioning = false;
//    }
//    // model.localEulerAngles = Vector3.Lerp(model.localEulerAngles, modelTarget, Time.deltaTime);

//    //if(Vector3.Angle(model.localEulerAngles, modelTarget) < 1)
//    //{
//    //    model.eulerAngles = modelTarget;
//    //    transitioning = false;
//    //}
//}