using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class MechController : MonoBehaviour
{
    Rigidbody rb;
    public Transform model;

    public GameObject footRockets;

    // Start is called before the first frame update
    void Start()
    {
        rb = GetComponent<Rigidbody>();
        footRockets.SetActive(false);
    }
    bool transitioning = false;
    Vector3 targetEuler = Vector3.zero;
    Vector3 cameraTarget;
    Vector3 modelTarget;
    float transitionTime;
    public Transform navBall;

    public GameObject turret;
    void Update()
    {
        // if (Input.GetKeyDown(KeyCode.Space))
        if (Input.GetKeyDown(KeyCode.Joystick1Button0))
        {
            // FindObjectOf.Type<AnimatorIK;
              Jump();

           
        }

        turret.transform.rotation = transform.rotation;
        navBall.rotation = Quaternion.LookRotation(Vector3.up, Vector3.forward);
        if (Input.GetKeyDown(KeyCode.Space))
        {
            transitioning = true;
           // transform.localEulerAngles -= new Vector3(90, 0, 0);
          //  model.localEulerAngles = new Vector3(90, 0, 0);

            modelTarget = new Vector3(90, 0, 0);
            var eulerAngles = transform.eulerAngles;
            cameraTarget = eulerAngles - new Vector3(90, 0, 0);
            transitionTime = 0;
        }

        if (transitioning)
        {
            transform.rotation = Quaternion.Lerp(Quaternion.Euler(transform.eulerAngles), Quaternion.Euler(cameraTarget), Time.deltaTime);
            transitionTime += Time.deltaTime;

            if(transitionTime > 2)
            {
                transitioning = false;
            }
           // model.localEulerAngles = Vector3.Lerp(model.localEulerAngles, modelTarget, Time.deltaTime);

            //if(Vector3.Angle(model.localEulerAngles, modelTarget) < 1)
            //{
            //    model.eulerAngles = modelTarget;
            //    transitioning = false;
            //}
        }

        //  Debug.Log(Vector2.Angle(Vector3.up, transform.up));

        // Debug.Log(Input.GetAxis("Vertical"));

        model.localEulerAngles = Vector3.Lerp(model.localEulerAngles, targetEuler, Time.deltaTime * 10);

        leftStickY = Input.GetAxis("Vertical");

        rightStickX = Input.GetAxis("Right Horizontal");

         rightStickY = Input.GetAxis("Right Vertical");

        transform.Rotate(Vector3.right, rightStickY * Time.deltaTime * -30);

        var leftStickX = Input.GetAxis("Horizontal");
        var rotSpeed = 30;
        if (status == Status.Flying)
            rotSpeed = 80;
        // transform.Rotate(Vector3.forward, rightStickX * Time.deltaTime * -rotSpeed);
      //  transform.Rotate(transform.forward, rightStickX * -Time.deltaTime * 50);
         transform.RotateAround(transform.forward, rightStickX * -Time.deltaTime);

        // Debug.Log(rightStickX);



        if (Mathf.Abs(leftStickY) < .8f)
            transform.Rotate(Vector3.up, leftStickX * Time.deltaTime * 30);

        if (status == Status.Hovering || status == Status.HoverForward || status == Status.Flying)
        {
            if (leftStickY > 0 & leftStickY < .66f & status != Status.HoverForward)
            {
                status = Status.HoverForward;
                // model.localEulerAngles = new Vector3(20, 0, 0);
                targetEuler = new Vector3(20, 0, 0);
                Debug.Log("HoverForward");
            }
            else if (leftStickY > .66f & status != Status.Flying)
            {
                Debug.Log("Flying");
                status = Status.Flying;
                FindObjectOfType<AnimatorIK>().Flying();
                footRockets.SetActive(true);
                //  model.localEulerAngles = new Vector3(89, 0, 0);
                targetEuler = new Vector3(89, 0, 0);
            }
            else if (leftStickY == 0 & status != Status.Hovering)
            {

                Debug.Log("hover");
                status = Status.Hovering;
                FindObjectOfType<AnimatorIK>().Hover();
                // model.localEulerAngles = new Vector3(0, 0, 0);
                targetEuler = new Vector3(0, 0, 0);
            }
        }

        if (Math.Round(rightStickX) == 0 & Math.Round(rightStickY) == 0 &
            Math.Round(leftStickX) == 0 & Math.Round(leftStickY) == 0)
        {

            var tempEuler = transform.rotation;
            tempEuler.x = Mathf.Lerp(tempEuler.x, 0, Time.deltaTime * 5);
            tempEuler.z = Mathf.Lerp(tempEuler.z, 0, Time.deltaTime * 5);
            transform.rotation = tempEuler;
        }

       //model.transform.position = transform.position;
       // model.transform.rotation = transform.rotation;// Quaternion.Lerp(model.transform.rotation, transform.rotation, Time.deltaTime);

    }

    private void FixedUpdate()
    {
        switch (status)
        {
            case (Status.Hovering):
                float yVel = rb.velocity.y + Physics.gravity.y;
                //  rb.AddForce(Vector3.up * 9.8f);

                var hoverAngle = Vector3.Angle(transform.up, Vector3.up);

                if(hoverAngle < 20 & !footRockets.activeInHierarchy)
                {
                    footRockets.SetActive(true);
                }
                else if(hoverAngle > 20 & footRockets.activeInHierarchy)
                {
                    footRockets.SetActive(false);
                }
                if (hoverAngle < 20)
                {
                    rb.AddForce(0, -yVel, 0, ForceMode.Force);
                }
                else
                {
                   // var tempEuler = transform.rotation;
                   // tempEuler.x =  Mathf.Lerp(tempEuler.x, 0, Time.deltaTime);
                  //  tempEuler.z =  Mathf.Lerp(tempEuler.z, 0, Time.deltaTime);
                    // transform.rotation = tempEuler;
                    rb.AddForce(0, -80, 0, ForceMode.Force);
                }

                Debug.Log(rightStickX);

              


                if (Input.GetKey(KeyCode.Joystick1Button4))
                {
                   // rb.AddForce(0, -50, 0, ForceMode.Force);
                    rb.AddForce(model.up * 200);
                }
                else if (Input.GetKey(KeyCode.Joystick1Button5))
                {
                    //rb.AddForce(0, 50, 0, ForceMode.Force);
                    rb.AddForce(model.up * 200);
                    footRockets.SetActive(true);
                }

                break;
            case (Status.HoverForward):
                rb.AddForce(transform.forward * 200);
                yVel = rb.velocity.y + Physics.gravity.y;
                //  rb.AddForce(Vector3.up * 9.8f);
                rb.AddForce(0, -yVel, 0, ForceMode.Force);

                break;
            case (Status.Flying):
                rb.AddForce(transform.forward * 400);
                //  yVel = rb.velocity.y + Physics.gravity.y;
                //  rb.AddForce(Vector3.up * 9.8f);
                //  rb.AddForce(0, -yVel, 0, ForceMode.Force);

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
        if (status == Status.Standing)
        {
            Debug.Log("Jump");
            GetComponent<Rigidbody>().AddForce(Vector3.up * 750);
            status = Status.Jumping;
        }
        else if (status == Status.Jumping)
        {
            status = Status.Hovering;
            FindObjectOfType<AnimatorIK>().Hover();
            Debug.Log("Hovering");
        }

    }
}
