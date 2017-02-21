using UnityEngine;

public class ProjectileMovement : MonoBehaviour
{

    public float speed;
    public LayerMask mask;
    public GameObject explosionType;

    private GameObject curExplosion;
    private RaycastHit hit;
    private Ray ray;
    private Vector3 curPosition;
    private Vector3 nextPosition;


    void Start()
    {
    }

    void FixedUpdate()
    {
        //code to move the projectile and check for collision of shootable object
        curPosition = transform.position;
        transform.position = curPosition + transform.forward * speed * Time.fixedDeltaTime;
        nextPosition = transform.position;
        ray.origin = curPosition;
        ray.direction = (nextPosition - curPosition).normalized;
        if (Physics.Raycast(ray, out hit, 2 * (nextPosition - curPosition).magnitude, mask))
        {
            OnProjectileCollision();
            if (hit.collider.gameObject.GetComponent<BaseShootable>() != null)
            {
                hit.collider.gameObject.GetComponent<BaseShootable>().OnShot();
            }
            else
            {
                explosionEffect();
            }
        }
    }

    void OnProjectileCollision()
    {
        GameObject.Destroy(this.gameObject);
    }


    void explosionEffect()
    {
        curExplosion = Instantiate(explosionType, curPosition, transform.rotation) as GameObject;
        curExplosion.GetComponent<ParticleSystem>().Play();
        Destroy(curExplosion.gameObject, curExplosion.GetComponent<ParticleSystem>().duration);
    }
}