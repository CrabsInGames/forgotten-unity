using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.InputSystem;

public class PlayerMovement : MonoBehaviour
{
    public static PlayerMovement active;
    public CharacterController characterController;
    PlayerControls inputActions;

    [Header("Movement")]
    [SerializeField] float walkSpeed;
    [SerializeField] Vector2 lookSpeed;
    Vector2 walkInput { get => inputActions.Player.Walk.ReadValue<Vector2>(); }
    Vector2 lookInput { get => inputActions.Player.Look.ReadValue<Vector2>(); }
    
    [Header("First Person Camera")]
    [SerializeField] Camera FPCamera;
    float VCamT;
    [SerializeField] Transform[] cameraExtremes;

    [Header("pick ups")]
    [SerializeField] Transform holdPositioner;
    Rigidbody holdBody;
    bool HoldBodyAssigned { get => holdBody; }
    [SerializeField] float pickupMaxDistance;
    [SerializeField] LayerMask pickupLayerMask;
    int pickupLayer;
    int holdLayer;


    private void Awake()
    {
        inputActions = new PlayerControls();
        inputActions.Player.PickUp.started += ctx => StartHold();
        inputActions.Player.PickUp.canceled += ctx => EndHold();
    }
    private void OnEnable()
    {
        active = this;
        inputActions.Enable();
    }
    private void OnDisable()
    {
        inputActions.Disable();
    }

    void Start()
    {
        if (!characterController)
            characterController = GetComponent<CharacterController>();
        if (!FPCamera)
            FPCamera = GetComponentInChildren<Camera>();
        VCamT = 0.5f;

        pickupLayer = LayerMask.NameToLayer("PickUps");
        holdLayer = LayerMask.NameToLayer("Hold");
    }

    private void FixedUpdate()
    {
        switch (HoldBodyAssigned)
        {
            case true:
                MoveHoldBody();
                break;
        }
    }
    void MoveHoldBody()
    {
        holdBody.MovePosition(holdPositioner.position);
        //holdBody.MoveRotation(holdPositioner.rotation);
    }

    void Update()
    {
        SideRotation();
        CamVRotation();
        Walk();
    }
    void SideRotation()
    {
        float rotate = lookInput.x * lookSpeed.x * Time.deltaTime;
        transform.Rotate(0, rotate, 0);
    }
    void CamVRotation()
    {
        VCamT += lookInput.y * lookSpeed.y * Time.deltaTime;
        VCamT = Mathf.Clamp01(VCamT);
        FPCamera.transform.rotation = Quaternion.Lerp(cameraExtremes[0].rotation, cameraExtremes[1].rotation, VCamT);
    }
    void Walk()
    {
        Vector3 walkLocal = new Vector3(walkInput.x, 0, walkInput.y) * walkSpeed;
        Vector3 walkWorld = transform.TransformDirection(walkLocal);
        characterController.Move(walkWorld * Time.deltaTime);
    }

    void StartHold()
    {
        Ray ray = FPCamera.ViewportPointToRay(Vector2.one * 0.5f);
        bool hit = Physics.Raycast(ray, out RaycastHit hitInfo, pickupMaxDistance, pickupLayerMask);
        switch (hit)
        {
            case true:
                holdBody = hitInfo.rigidbody;
                Debug.Log("now holding " + holdBody);
                switch (HoldBodyAssigned)
                {
                    case true:
                        holdBody.gameObject.layer = holdLayer;
                        holdBody.isKinematic = false;
                        holdBody.useGravity = false;
                        holdPositioner.position = holdBody.position;
                        holdPositioner.rotation = holdBody.rotation;
                        break;
                }
                break;
            case false:
                Debug.Log("nothing to grab");
                break;
        }
    }
    public void EndHold()
    {
        switch (HoldBodyAssigned)
        {
            case true:
                holdBody.gameObject.layer = pickupLayer;
                holdBody.useGravity = true;
                Debug.Log(holdBody + " released");
                holdBody = null;
                break;
        }
    }

    private void OnDrawGizmos()
    {
        Vector3 downOffset = Vector3.up * characterController.radius;
        Gizmos.DrawWireSphere(transform.position + downOffset, characterController.radius);
        Vector3 upOffset = Vector3.up * (characterController.height - characterController.radius);
        Gizmos.DrawWireSphere(transform.position + upOffset, characterController.radius);
    }
}
