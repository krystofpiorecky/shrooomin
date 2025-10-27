using UnityEngine;

public class SmoothFollowCamera : MonoBehaviour
{
  public Transform target;

  private readonly float positionLerpSpeed = 30f;
  private readonly float rotationLerpSpeed = 30f;

  void LateUpdate()
  {
    if (target == null) return;

    transform.position = Vector3.Lerp(transform.position, target.position, positionLerpSpeed * Time.deltaTime);
    transform.rotation = Quaternion.Slerp(transform.rotation, target.rotation, rotationLerpSpeed * Time.deltaTime);
  }
}
