// Converted from UnityScript to C# at http://www.M2H.nl/files/js_to_c.php - by Mike Hergaarden
// Do test the code! You usually need to change a few small bits.

using UnityEngine;
using System.Collections;

[AddComponentMenu("Camera-Control/Smooth Follow 2")]
public class SmoothFollow2 : MonoBehaviour
{
    /*
    This camera smoothes out rotation around the y-axis and height.
    Horizontal Distance to the target is always fixed.

    There are many different ways to smooth the rotation but doing it this way gives you a lot of control over how the camera behaves.
    For the smoothed values of rotation and position, we calculate their wanted value.
    Then we smooth it using the Lerp function.
    Then we apply the smoothed values to the transform's position and rotation.
    */

    // The target we are following
    public Transform target;
    // the height we want the camera to be above the target
    public float height = 0.5f;
    // The distance in the x-z plane to the target
    public float distance = 1.0f;
    //The amount by which we want to dampen the height
    public float heightDamping = 1.0f;
    //The amount by which we want to dampen the rotation
    public float rotationDamping = 1.0f;
    public bool smoothRotation = true;

    private RaycastHit hit;
    private GameObject prevHitColliderGO;

    // Place the script in the Camera-Control group in the component menu


    void Start()
    {
        // Make the rigid body not change rotation 
        if (GetComponent<Rigidbody>())
            GetComponent<Rigidbody>().freezeRotation = true;
    }

    void LateUpdate()
    {
        //Early on, if we don't have a target!
        if (!target)
        {
            return;
        }

        //Transforms position from local to world space
        Vector3 wantedPosition = target.TransformPoint(0, height, -distance);
        transform.position = Vector3.Lerp(transform.position, wantedPosition, Time.deltaTime * heightDamping);

        if (smoothRotation)
        {
            transform.rotation = Quaternion.Lerp(transform.rotation, target.rotation, Time.deltaTime * rotationDamping);
        }

        //checkLineOfSight();

        //We want to see the target at all times!
        transform.LookAt(target);
    }

    private void checkLineOfSight()
    {
        Vector3 trueTargetPosition = target.transform.position - new Vector3(0, -height, 0);
        // Cast the line to check if view is blocked:
        if (Physics.Linecast(trueTargetPosition, transform.position, out hit))
            if (!hit.collider.isTrigger)
            {
                if (!prevHitColliderGO)
                {
                    hit.collider.gameObject.SetActive(false);
                }
                else if (hit.collider.gameObject != prevHitColliderGO)
                {
                    prevHitColliderGO.SetActive(true);
                    hit.collider.gameObject.SetActive(false);
                }

                prevHitColliderGO = hit.collider.gameObject;
            }

        /*  
            if(!hit.collider.isTrigger)
            {
                // If so, shorten distance so camera is in front of object:
                FIXME_VAR_TYPE tempDistance= Vector3.Distance (trueTargetPosition, hit.point) - 0.28f; 
                // Finally, rePOSITION the CAMERA:
                position = target.position - (target.rotation * Vector3.forward * tempDistance + Vector3(0,-height,0)); 
                transform.position = position;
            } 
        }*/
    }
}