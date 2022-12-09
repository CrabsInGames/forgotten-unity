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
    [SerializeField] float VCamT;
    [SerializeField] Transform[] cameraExtremes;
    //[SerializeField] Light light;

    [Header("pick ups")]
    [SerializeField] float smoothTime = 0.1f;
    public Transform holdPositioner;
    Rigidbody holdBody;
    bool isHolding;
    [SerializeField] float pickupMaxDistance;
    [SerializeField] LayerMask pickupLayerMask;
    int pickupLayer;
    int holdLayer;

    [Header("UI")]
    public GameObject grabUI;

    private void Awake()
    {
        inputActions = new PlayerControls();
        inputActions.Player.PickUp.started += ctx => StartHold();
        inputActions.Player.PickUp.canceled += ctx => EndHold(false);
        inputActions.Player.PauseMenu.started += ctx => OpenMenu();
        inputActions.Player.Journal.started += ctx => OpenJournal();
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
        switch (isHolding && (bool)holdBody)
        {
            case true:
                MoveHoldBody();
                break;
        }
    }
    void MoveHoldBody()
    {
        Vector3 refVelocity = Vector3.zero;
        holdBody.MovePosition(Vector3.SmoothDamp(holdBody.transform.position, holdPositioner.position, ref refVelocity, smoothTime));        
    }

    void Update()
    {
        SideRotation();
        CamVRotation();
        Walk();
        if (!isHolding)
            RayCheck();
    }
    void RayCheck()
    {
        Ray ray = FPCamera.ViewportPointToRay(Vector2.one * 0.5f);
        bool hit = Physics.Raycast(ray, out RaycastHit hitInfo, pickupMaxDistance, pickupLayerMask);
        switch (hit)
        {
            case true:
                //light.transform.LookAt(hitInfo.point);
                holdBody = hitInfo.rigidbody;
                switch ((bool)holdBody)
                {
                    case true:
                        Debug.Log("looking at " + holdBody);
                        grabUI.SetActive(true);
                        switch (hitInfo.collider.CompareTag("Note"))
                        {
                            case true:
                                Debug.Log(hitInfo.collider.name + " note found");
                                grabUI.SetActive(true);
                                break;
                        }
                        return;
                    case false:
                        holdBody = null;
                        break;
                }
                break;
            case false:
                holdBody = null;
                break;
        }
        grabUI.SetActive(false);
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
        Vector3 walkLocal = new Vector3(walkInput.x, -transform.position.y, walkInput.y) * walkSpeed;
        Vector3 walkWorld = transform.TransformDirection(walkLocal);
        characterController.Move(walkWorld * Time.deltaTime);
    }

    void StartHold()
    {
        switch ((bool)holdBody)
        {
            case false:
                Debug.Log("nothing to grab");
                break;
            case true:
                switch (holdBody.gameObject.tag)
                {
                    case "Cube":
                        holdBody.gameObject.layer = holdLayer;
                        holdBody.useGravity = false;
                        holdBody.isKinematic = true;
                        //holdPositioner.position = holdBody.position;
                        //holdPositioner.rotation = holdBody.rotation;
                        isHolding = true;
                        break;
                    case "Note":
                        NotePickup note = holdBody.GetComponent<NotePickup>();
                        note.PickUp();
                        MenuManager.active.OpenMenu(1);
                        break;
                    case "Hand":
                        ClockGrabber grabber = holdBody.GetComponent<ClockGrabber>();
                        grabber.StartGrab();
                        isHolding = true;
                        break;
                }
                grabUI.SetActive(false);
                break;
        }
    }
    public void EndHold(bool makeKinematic)
    {
        switch (isHolding && (bool)holdBody)
        {
            case true:
                switch (holdBody.CompareTag("Hand"))
                {
                    case true:
                        ClockGrabber grabber = holdBody.GetComponent<ClockGrabber>();
                        grabber.EndGrab();
                        break;
                    case false:
                        holdBody.gameObject.layer = pickupLayer;
                        holdBody.useGravity = true;
                        holdBody.isKinematic = makeKinematic;
                        break;
                }
                Debug.Log(holdBody + " released");
                holdBody = null;
                isHolding = false;
                break;
        }
    }

    void OpenMenu()
    {
        if (MenuManager.active)
            MenuManager.active.CloseOrOpen();
    }
    void OpenJournal()
    {
        if (MenuManager.active)
            MenuManager.active.OpenMenu(1);
    }

    private void OnDrawGizmos()
    {
        Vector3 downOffset = Vector3.up * characterController.radius;
        Gizmos.DrawWireSphere(transform.position + downOffset, characterController.radius);
        Vector3 upOffset = Vector3.up * (characterController.height - characterController.radius);
        Gizmos.DrawWireSphere(transform.position + upOffset, characterController.radius);

        switch (isHolding)
        { 
            case true:
                Gizmos.color = Color.red;
                break;
            case false:
                Gizmos.color = Color.blue;
                break;
        }
        Gizmos.DrawWireSphere(holdPositioner.position, 0.1f);
    }
}
