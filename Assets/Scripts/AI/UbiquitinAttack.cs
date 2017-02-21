using UnityEngine;

public class UbiquitinAttack : MonoBehaviour
{
    private int hits;
    public GameObject explosion;
    
    void OnCollisionEnter(Collision collision)
    {
        if (collision.gameObject.layer == LayerMask.NameToLayer("Ubiquitin"))
        {
            hits++;


            if (hits == 8)
            {
                Instantiate(explosion, transform.position, Quaternion.identity);
                Destroy(gameObject);
            }
        }

    }

    void attachMolecule()
    {
        if (!GetComponent<HingeJoint>())
        {
            gameObject.AddComponent<HingeJoint>();
            GetComponent<HingeJoint>().axis = new Vector3(1.0f, 1.0f, 1.0f);
        }
    }
}