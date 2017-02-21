using UnityEngine;

// Oh god this is awful.
public class MaterialSwap : MonoBehaviour
{
    public bool swapped = false;
    public Material Mat1;
    public Material Mat2;
    
    void Update()
    {
        if (swapped)
        {
            GetComponent<Renderer>().material = Mat2;
        }
        else
        {
            GetComponent<Renderer>().material = Mat1;
        }
    }
}
