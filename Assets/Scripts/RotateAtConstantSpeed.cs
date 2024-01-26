using UnityEngine;

public class RotateAtConstantSpeed : MonoBehaviour
{
    [SerializeField] float rotationPerFrame = 1f;

    void FixedUpdate()
    {
        transform.Rotate(Vector3.up * rotationPerFrame);
    }
}
