using UnityEngine;
using System.Collections;
using UnityEngine.EventSystems;

/// MouseLook rotates the transform based on the mouse delta.
/// Minimum and Maximum values can be used to constrain the possible rotation

/// To make an FPS style character:
/// - Create a capsule.
/// - Add the MouseLook script to the capsule.
///   -> Set the mouse look to use LookX. (You want to only turn character but not tilt it)
/// - Add FPSInputController script to the capsule
///   -> A CharacterMotor and a CharacterController component will be automatically added.

/// - Create a camera. Make the camera a child of the capsule. Reset it's transform.
/// - Add a MouseLook script to the camera.
///   -> Set the mouse look to use LookY. (You want the camera to tilt up and down like a head. The character already turns.)
[AddComponentMenu("Camera-Control/Mouse Look")]
public class MouseLook : MonoBehaviour {

	public enum RotationAxes { MouseXAndY = 0, MouseX = 1, MouseY = 2 }
	public RotationAxes axes = RotationAxes.MouseXAndY;
	public float sensitivityX = 15F;
	public float sensitivityY = 15F;

	public float minimumX = -360F;
	public float maximumX = 360F;

	public float minimumY = -60F;
	public float maximumY = 60F;

	float rotationY = 0F;

    bool cursorVisable = false;
    float lastCursorToggle = 0;

	void Update ()
	{
        if (GameState.Instance.PauseLevel != PauseLevel.Unpaused)
        {
            Cursor.lockState = CursorLockMode.None;
            return;
        }

        if (/*!EventSystem.current.IsPointerOverGameObject() && */ (Input.GetMouseButton(1) || !cursorVisable))
        {
            Cursor.lockState = CursorLockMode.Locked;
            CheckForCursorToggle();
            if (axes == RotationAxes.MouseXAndY)
            {
                float rotationX = transform.localEulerAngles.y + Input.GetAxis("Horizontal") * sensitivityX;

                rotationY += Input.GetAxis("Vertical") * sensitivityY + 2 * transform.localEulerAngles.y;
                rotationY = Mathf.Clamp(rotationY, minimumY, maximumY);

                transform.localEulerAngles = new Vector3(-rotationY, rotationX, 0);
            }
            else if (axes == RotationAxes.MouseX)
            {
                transform.Rotate(0, Input.GetAxis("Horizontal") * sensitivityX, 0);
            }
            else
            {
                rotationY += Input.GetAxis("Vertical") * sensitivityY;
                rotationY = Mathf.Clamp(rotationY, minimumY, maximumY);

                transform.localEulerAngles = new Vector3(-rotationY, transform.localEulerAngles.y, transform.localEulerAngles.z);
            }
        }
        else
        {

            CheckForCursorToggle();

            if (axes == RotationAxes.MouseXAndY || axes == RotationAxes.MouseY)
            {
                rotationY = Mathf.Lerp(rotationY, 0, 0.15f);
                rotationY = Mathf.Clamp(rotationY, minimumY, maximumY);

                //transform.localEulerAngles = new Vector3(-rotationY, transform.localEulerAngles.y, 0);
            }
        }

        /*
		if (axes == RotationAxes.MouseXAndY)
		{
			float rotationX = transform.localEulerAngles.y + Input.GetAxis("Horizontal") * sensitivityX;
			
			rotationY += Input.GetAxis("Vertical") * sensitivityY;
			rotationY = Mathf.Clamp (rotationY, minimumY, maximumY);
			
			transform.localEulerAngles = new Vector3(-rotationY, rotationX, 0);
		}
		else if (axes == RotationAxes.MouseX)
		{
			transform.Rotate(0, Input.GetAxis("Horizontal") * sensitivityX, 0);
		}
		else
		{
			rotationY += Input.GetAxis("Vertical") * sensitivityY;
			rotationY = Mathf.Clamp (rotationY, minimumY, maximumY);
			
			transform.localEulerAngles = new Vector3(-rotationY, transform.localEulerAngles.y, 0);
		}*/
	}

    private float ClampAngle(float ang, float min, float max) //This function is never used anywhere...
    {
        if (ang > 180) ang = ang - 360;
        ang = Mathf.Clamp(ang, min, max);
        if (ang < 0) ang = 360 + ang;
        return ang;
    }
	
	void Start ()
	{
		// Make the rigid body not change rotation
		if (GetComponent<Rigidbody>())
			GetComponent<Rigidbody>().freezeRotation = true;
	}

    private void CheckForCursorToggle()
    {

        if (Input.GetKey(KeyCode.Escape) && (Time.fixedTime - lastCursorToggle) > 1)
        {
            cursorVisable = !cursorVisable;
        }

        if (cursorVisable)
        {
            Cursor.lockState = CursorLockMode.None;
        }
        else
        {
            Cursor.lockState = CursorLockMode.Locked;
        }
    }
}