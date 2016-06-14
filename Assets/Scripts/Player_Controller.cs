using UnityEngine;
using System.Collections;
using UnityEngine.Networking;

public class Player_Controller : NetworkBehaviour
{

    [SerializeField] Camera PlayerCamera;
    [SerializeField] AudioListener audioListener;


    public float fuelLeft = 15f;
    public float fuelConsumptionRate = 0.2f;
    public float vehicleSpeed = 2f;
    public float vehicleRotationSpeed = 2f;

	// Use this for initialization
	void Start ()
	{
	    PlayerCamera.enabled = false;
	    audioListener.enabled = false;
        if (isLocalPlayer)
        {
            Camera.main.enabled = false;
            PlayerCamera.enabled = true;
            audioListener.enabled = true;
        }
    }
	
	// Update is called once per frame
	void Update () {
	    if (!isLocalPlayer)
	    {
	        return;
	    }
        RotationController();
	    MovementControlller();
	}

    void RotationController()
    {
        if (fuelLeft > 0 && (Input.GetKey("a") || Input.GetKey("d")))
        {
            transform.RotateAround(transform.position, transform.up, Time.deltaTime * vehicleRotationSpeed * (Input.GetKey("d") ? 1 : -1));
            DrainFuel(0.1f);
        }
    }

    void MovementControlller()
    {
        if (fuelLeft > 0 && (Input.GetKey("w") || Input.GetKey("s")))
        {
            
            transform.Translate((Input.GetKey("w") ? 1 : -1) * Vector3.forward * Time.deltaTime * vehicleSpeed);
            DrainFuel(1);   
        }
    }

    void DrainFuel(float multifier)
    {
        fuelLeft -= fuelConsumptionRate * Time.deltaTime * multifier;
    }
}
