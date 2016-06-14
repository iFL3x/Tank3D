using UnityEngine;
using System.Collections;
using UnityEngine.Networking;

public class Player_SyncRotation : NetworkBehaviour
{

    [SyncVar] private Quaternion syncPlayerRotation;
    [SyncVar] private Quaternion syncCamRotation;

    [SerializeField] private Transform playerTransform;
    [SerializeField] private Transform camTransform;
    [SerializeField] private float lerpRate = 15;

    private Quaternion lastPlayerRot;
    private Quaternion lastCamRot;
    public float rotationUpdateThreshhold = 0.1f;

    void Update()
    {
        LerpRotations();
    }

    void FixedUpdate()
    {
        SendRotations();
    }

    void LerpRotations()
    {
        if (!isLocalPlayer)
        {
            playerTransform.rotation = Quaternion.Lerp(playerTransform.rotation, syncPlayerRotation, Time.deltaTime * lerpRate);
            camTransform.rotation = Quaternion.Lerp(camTransform.rotation, syncCamRotation, Time.deltaTime * lerpRate);
        }
    }

    [Command]
    void CmdSendRotationsToServer(Quaternion playerRot, Quaternion camRot)
    {
        syncPlayerRotation = playerRot;
        syncCamRotation = camRot;
    }

    [ClientCallback]
    void SendRotations()
    {
        if (isLocalPlayer && (Quaternion.Angle(playerTransform.rotation, lastPlayerRot) > rotationUpdateThreshhold || Quaternion.Angle(camTransform.rotation, lastCamRot) > rotationUpdateThreshhold))
        {
            CmdSendRotationsToServer(playerTransform.rotation, camTransform.rotation);
            lastPlayerRot = playerTransform.rotation;
            lastCamRot = camTransform.rotation;
        }
    }
}















