using UnityEngine;

[RequireComponent(typeof(Rigidbody))]
public class PlayerMovement : MonoBehaviour
{
  public float moveSpeed = 5f;
  public float rotationSpeed = 10f;
  public Animator animator;
  public Transform cameraTransform;

  private Rigidbody rb;
  private Vector3 moveDirection;

  private float yaw;
  private float pitch;
  private readonly float minPitch = -30f;
  private readonly float maxPitch = 70f;
  private readonly float mouseSensitivity = 2f;

  void Start()
  {
    rb = GetComponent<Rigidbody>();
    rb.constraints = RigidbodyConstraints.FreezeRotation;
  }

  void Update()
  {
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

    float speedPercent = input.magnitude;
    animator.SetFloat("Speed", speedPercent, 0.1f, Time.deltaTime);
  }

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
    if (moveDirection.sqrMagnitude > 0.01f)
    {
      Quaternion targetRotation = Quaternion.LookRotation(moveDirection);
      transform.rotation = Quaternion.Slerp(transform.rotation, targetRotation, rotationSpeed * Time.fixedDeltaTime);
      rb.MovePosition(rb.position + moveDirection * moveSpeed * Time.fixedDeltaTime);
    }
  }
}
