using UnityEngine;

public class Shootable : BaseShootable
{
    private Vector3 curPosition;
    private Quaternion curRotation;
    private GameObject curExplosion;

    public GameObject explosionType;

    void Update()
    {
        curPosition = transform.position;
        curRotation = transform.rotation;
    }

    public override void OnShot()
    {
        GameObject.Destroy(this.gameObject);
        explosionEffect();
    }

    void explosionEffect()
    {
        curExplosion = Instantiate(explosionType, curPosition, curRotation) as GameObject;
        curExplosion.GetComponent<ParticleSystem>().Play();
        Destroy(curExplosion.gameObject, curExplosion.GetComponent<ParticleSystem>().main.duration);

    }
}