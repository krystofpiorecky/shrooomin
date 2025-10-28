using UnityEngine;
using Mirror;
using System.Diagnostics;

[RequireComponent(typeof(Rigidbody))]
public class PlayerMovement : NetworkBehaviour
{
  public float moveSpeed = 5f;
  public float rotationSpeed = 10f;
  public Animator animator;
  public Transform cameraTransform;

  private Rigidbody rb;
  private Vector3 moveDirection;

  private float yaw;
  private float pitch;
  private readonly float minPitch = -89f;
  private readonly float maxPitch = 89f;
  private readonly float mouseSensitivity = 2f;

  [SyncVar(hook = nameof(OnAnimationMovementSpeedChanged))]
  private float animationMovementSpeed;

  public GameObject[] bodyParts;

  void Start()
  {
    rb = GetComponent<Rigidbody>();
    rb.constraints = RigidbodyConstraints.FreezeRotation;

    if (isLocalPlayer) {
      Camera.main.GetComponent<SmoothFollowCamera>().target = cameraTransform;
      foreach (var bp in bodyParts)
        bp.SetActive(false);
    }
  }

  void Update()
  {
    if (!isLocalPlayer) return;

    if (Input.GetKeyDown(KeyCode.Escape))
    {
      Cursor.lockState = CursorLockMode.None;
      Cursor.visible = true;
    }
    else if (Input.GetMouseButtonDown(0))
    {
      Cursor.lockState = CursorLockMode.Locked;
      Cursor.visible = false;
    }

    if (Cursor.visible) return;

    HandleCamera();

    float h = Input.GetAxis("Horizontal");
    float v = Input.GetAxis("Vertical");
    Vector3 input = new Vector3(h, 0, v).normalized;

    Vector3 camForward = cameraTransform.forward;
    camForward.y = 0;
    Vector3 camRight = cameraTransform.right;
    camRight.y = 0;
    moveDirection = (camForward * input.z + camRight * input.x).normalized;
  }

  [Command]
  void CmdSetAnimationMovementSpeed(float value) =>
    animationMovementSpeed = value;

  void OnAnimationMovementSpeedChanged(float oldValue, float value) =>
    animator.SetFloat("Speed", value);

  void HandleCamera()
  {
    float mouseX = Input.GetAxis("Mouse X") * mouseSensitivity;
    float mouseY = Input.GetAxis("Mouse Y") * mouseSensitivity;

    yaw += mouseX;
    pitch -= mouseY;
    pitch = Mathf.Clamp(pitch, minPitch, maxPitch);

    transform.rotation = Quaternion.Euler(0f, yaw, 0f);
    cameraTransform.localRotation = Quaternion.Euler(pitch, 0f, 0f);
  }

  void FixedUpdate()
  {
    if (!isLocalPlayer) return;

    if (moveDirection.sqrMagnitude > 0.01f)
    {
      Quaternion targetRotation = Quaternion.LookRotation(moveDirection);
      transform.rotation = Quaternion.Slerp(transform.rotation, targetRotation, rotationSpeed * Time.fixedDeltaTime);
      rb.MovePosition(rb.position + moveDirection * moveSpeed * Time.fixedDeltaTime);
      CmdSetAnimationMovementSpeed(1);
    }
    else
    {
      CmdSetAnimationMovementSpeed(0);
    }
  }
}
