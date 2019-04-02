using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class AnimatorIK : MonoBehaviour
{
    public Transform model;

   //public Animation ra;

    public Transform rht,lht, rft, lft;
    public Transform rHandIdle, lHandIdle, rFootIdle, lFootIdle;

    public Transform rHandHover, lHandHover;
    public Transform rFootHover, lFootHover;

    private void Awake()
    {
        model.position = new Vector3(0, 1.2f, 0);
        //ra = null;
        target = null;
        
    }
    // Start is called before the first frame update
    void Start()
    {
        rht.position = rHandIdle.position;
        rht.rotation = rHandIdle.rotation;

        lht.position = lHandIdle.position;
        lht.rotation = lHandIdle.rotation;

        lft.position = lFootIdle.position;
        lft.rotation = lFootIdle.rotation;

        rft.position = rFootIdle.position;
        rft.rotation = rFootIdle.rotation;
    }

    public void Hover()
    {
        RunAnimation(rht, rHandHover);
        RunAnimation(lht, lHandHover);
        RunAnimation(rft, rFootHover);
        RunAnimation(lft, lFootHover);
    }
    public void Flying()
    {
        RunAnimation(rht, rHandFlying);
        RunAnimation(lht, lhandFlying);
    }

    Transform target;
    Vector3 targetDir;
    public Transform rHandFlying, lhandFlying;
    public float speed = .001F;
    void Update()
    {
        //if (ra)
        //{
        //    ra.target.position = Vector3.Lerp(ra.startPos,ra.endPos,)
        //}
        // Debug.Log("wtf");

  

        if (Input.GetKeyDown(KeyCode.Alpha1))
        {
            Debug.Log("run animation");
            RunAnimation(rht, rHandHover);
            RunAnimation(lht, lHandHover);
            RunAnimation(rft, rFootHover);
            RunAnimation(lft, lFootHover);
        }

        if (Input.GetKeyDown(KeyCode.Alpha0))
        {
            Debug.Log("run animation");
            RunAnimation(rht,rHandFlying);
            RunAnimation(lht, lhandFlying);
        }
       // if (Input.GetKeyDown(KeyCode.Alpha1))
            if(Input.GetKeyDown(KeyCode.D))
        {
            
            Debug.Log("run animation");
            RunAnimation(rht, rHandIdle);
            RunAnimation(lht, lHandIdle);
        }

        //  Debug.Log(target);

        if (target)
        {
            // Distance moved = time * speed.
           // float distCovered = (Time.time - animationStartTime) * speed;

            // Fraction of journey completed = current distance divided by total distance.
           // float fracJourney = distCovered / targetDir.normalized.magnitude;

            // Set our position as a fraction of the distance between the markers.
           // rht.position = Vector3.Lerp(targetStartPos, target.position, fracJourney);
            rht.position = Vector3.Lerp(rht.position, target.position, Time.deltaTime * 10);
            rht.rotation = Quaternion.Lerp(rht.rotation, target.rotation, Time.deltaTime * 10)
;
            if (Vector3.Distance(target.position,rht.position) < .01)
            {
                rht.rotation = target.rotation;
                rht.position = target.position;
                target = null;
            }
        }
        var removeAnimations = new List<Animation>();
        if (runningAnimations != null)
        {
            foreach (var animation in runningAnimations)
            {
              //  if (animation.targetTransform)
                //{
                    animation.IKtarget.position = Vector3.Lerp(animation.IKtarget.position, animation.targetTransform.position, Time.deltaTime * 10);
                animation.IKtarget.rotation = Quaternion.Lerp(animation.IKtarget.rotation, animation.targetTransform.rotation, Time.deltaTime * 10);
                    if (Vector3.Distance(animation.IKtarget.position, animation.targetTransform.position) < .01)
                    {
                    //  animation.targetTransform = null;
                    // runningAnimations.Remove(animation);
                    removeAnimations.Add(animation);
                        target = null;
                    }
              //  }
            }

            foreach ( var animation in removeAnimations )
            {
                runningAnimations.Remove(animation);
            }
        }
    }

    List<Animation> runningAnimations;
    struct Animation
    {
        public Transform targetTransform;
        public Transform IKtarget;
    }

    public void RunAnimation(Transform IKtarget, Transform endPos)
    {
        if (runningAnimations == null)
            runningAnimations = new List<Animation>();

        var newAnimation = new Animation();
        newAnimation.targetTransform = endPos;
        newAnimation.IKtarget = IKtarget;

        runningAnimations.Add(newAnimation);


        //if(endPos == null)
        //{
        //    Debug.LogWarning("target pos is null");
        //}
       // //this.target = endPos;
        //animationStartTime = Time.time;
       // targetDir = endPos.position - rht.position;
       // targetDir = rht.position - endPos.position;
      //  targetStartPos = rht.position;
    }

    float animationStartTime;
    Vector3 targetStartPos;
    //public void RunAnimation(Animation animationToRun)
    //{
    //    ra = animationToRun;
    //    ra.running = true;
    //    animationStartTime = Time.deltaTime;
    //}
}
