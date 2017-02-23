using UnityEngine;

public class CameraChange : MonoBehaviour {
    private bool main;
    public Camera ship;
    public Camera pro;

    void Awake() {
        main = true;
    }

    public void SetShipCam() {
        main = true;
        ship.GetComponent<Camera>().enabled = true;
        pro.GetComponent<Camera>().enabled = false;
    }

    public void SetProCam() {
        main = false;
        ship.GetComponent<Camera>().enabled = false;
        pro.GetComponent<Camera>().enabled = true;
    }

    bool IsShipCamActive() {
        return main;
    }

    void SwitchCamera() {
        main = !main;

        if (main) {
            //set to main
            ship.GetComponent<Camera>().enabled = true;
            pro.GetComponent<Camera>().enabled = false;
            print("main");
        }
        else {
            //set to other
            ship.GetComponent<Camera>().enabled = false;
            pro.GetComponent<Camera>().enabled = true;
            print("other");
        }
    }
}