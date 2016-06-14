using UnityEngine;
using System.Collections;
using UnityEngine.Networking;


public class Player_SyncPosition : NetworkBehaviour
{
    
    [SyncVar]
    private Vector3 syncPos;
    [SerializeField] private Transform playerTransform;
    [SerializeField] private float lerpRate = 15;
    private Vector3 lastPosition;

    public float updateDistanceThreshold = 0.1f;

    void Start()
    {
        lastPosition = playerTransform.position;
    }


    void Update()
    {
        LerpPosition();
    }

    void FixedUpdate()
    {
        SendPosition();
    }

    void LerpPosition()
    {
        //Only happens on other players objects since we ourself are moving smooth already
        if (!isLocalPlayer)
        {
            playerTransform.position = Vector3.Lerp(playerTransform.position, syncPos, Time.deltaTime*lerpRate);
        }
    }

    [Command]
    void CmdSendPositionToserver(Vector3 pos)
    {
        //Client sends its position to server using Command on every movement
        syncPos = pos;
    }

    [ClientCallback]
    void SendPosition()
    {
        if (isLocalPlayer && Vector3.Distance(playerTransform.position, lastPosition) >= updateDistanceThreshold)
        {
            lastPosition = playerTransform.position;
            CmdSendPositionToserver(playerTransform.position);
        }
    }
}
    