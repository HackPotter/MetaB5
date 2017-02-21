using UnityEngine;

public class CannonTool : MonoBehaviour
{
    public GameObject projectile;
    public LayerMask mask;

    private Ray ray;
    private RaycastHit hit;
    private LineRenderer drawline;
    private bool rocketlauncher = true;
    private bool laser = false;
    private bool stick;
    private bool destroytether = false;
    private Vector3 curPosition;
    private Vector3 lineDest;


    void Start()
    {
        drawline = GetComponent<LineRenderer>();
    }


    void Update()
    {

        curPosition = transform.position;

        if (Input.GetKeyDown("1"))
        {
            //switches tool to rocket
            rocketlauncher = true;
            laser = false;
            stick = false;
            drawline.SetWidth(0, 0);
        }

        if (Input.GetKeyDown("2"))
        {
            //switches tool to grapple
            laser = true;
            rocketlauncher = false;
            stick = false;
            drawline.SetWidth(0, 0);
        }

        if (Input.GetKeyDown(KeyCode.F) && rocketlauncher)
            Use();

        if (Input.GetKeyDown(KeyCode.F) && laser)
        {
            //code that sets up grapple direction and target check
            destroytether = false;
            stick = false;
            lineDest = transform.forward * 30 + curPosition;
            drawline.SetPosition(0, curPosition);
            drawline.SetPosition(1, lineDest);
            drawline.SetWidth(0.5f, 0.5f);
            ray.origin = curPosition;
            ray.direction = transform.forward;
            if (!stick)
            {
                if (Physics.Raycast(ray, out hit, 30, mask))
                {
                    if (hit.collider.gameObject.GetComponent<BaseShootable>() != null)
                    {
                        stick = true;
                        //grapple sticks to object when object has BaseShootable
                    }
                }
                else
                {
                    destroytether = true;
                }
            }
        }

        if (stick)
        {
            //code to move object towards you
            hit.collider.GetComponent<Rigidbody>().constraints = RigidbodyConstraints.None;
            drawline.SetPosition(0, curPosition);
            drawline.SetPosition(1, hit.collider.gameObject.transform.position);
            if ((curPosition - hit.collider.gameObject.transform.position).magnitude > 3)
            {
                hit.collider.GetComponent<Rigidbody>().AddForce(curPosition - hit.collider.transform.position);
            }
            if ((Input.GetKey(KeyCode.G) && laser) | (curPosition - hit.collider.gameObject.transform.position).magnitude > 30)
            {
                //code to destroy tether connection
                stick = false;
                destroytether = true;
                lineDest = hit.collider.gameObject.transform.position;
                hit.collider.GetComponent<Rigidbody>().constraints = RigidbodyConstraints.FreezeAll; //comment out this line if you want the draggables to keep their momentum when released

            }
        }

        if (destroytether)
        {
            //code to destroy tether if instructed to or you get to far away from object
            lineDest = lineDest - 3 * (lineDest - curPosition).normalized;
            drawline.SetPosition(0, curPosition);
            drawline.SetPosition(1, lineDest);
            if ((lineDest - curPosition).magnitude < 2)
            {
                drawline.SetWidth(0, 0);
                destroytether = false;
            }
        }
    }


    void Use()
    {
        GameObject newProjectile;
        newProjectile = Instantiate(projectile, transform.position + transform.forward, transform.rotation) as GameObject;
        newProjectile.gameObject.name = "newProjectile";
    }


    void OnGUI()
    {
        if (rocketlauncher)
        {
            GUI.Label(new Rect(Screen.width * 0.4f, Screen.height * .05f, 250, 200), "Primary Weapon : Rocket Launcher");
        }
        if (laser)
        {
            GUI.Label(new Rect(Screen.width * 0.4f, Screen.height * .05f, 300, 200), "Primary Weapon : Pulse Grapple");
        }
    }
}


