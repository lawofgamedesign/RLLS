using UnityEngine;

public class RotateObject : MonoBehaviour
{
    private Rigidbody rb;
    private float rotateSpeed = 10.0f;
    private Rigidbody rotatedObject;

    private void Start()
    {
        rb = GetComponent<Rigidbody>();
        rotatedObject = GameObject.Find("Player 1").GetComponent<Rigidbody>();
    }

    private void Update()
    {
        if (Input.GetKey(KeyCode.I))
        {
            Quaternion rotationChange = Quaternion.AngleAxis(rb.rotation.y + rotateSpeed * Time.deltaTime, Vector3.right);
            rotatedObject.MovePosition(rotationChange * (rotatedObject.position - rb.position) + rb.position);
            Vector3 outwardDir = (rotatedObject.position - rb.position).normalized;
            rotatedObject.MoveRotation(Quaternion.LookRotation(outwardDir, rotatedObject.transform.localRotation * Vector3.up));
        }
        if (Input.GetKey(KeyCode.J))
        {
            Quaternion rotationChange = Quaternion.AngleAxis(rb.rotation.x - rotateSpeed * Time.deltaTime, Vector3.up);
            rotatedObject.MovePosition(rotationChange * (rotatedObject.position - rb.position) + rb.position);
            Vector3 outwardDir = (rotatedObject.position - rb.position).normalized;
            Vector3 crossDir = Vector3.Cross(outwardDir, rotatedObject.transform.up);
            Vector3 upDir = Vector3.Cross(outwardDir, crossDir);
            rotatedObject.MoveRotation(Quaternion.LookRotation(outwardDir, rotatedObject.transform.up));
        }
    }


    private Quaternion CalculateWristAngle()
    {
        Quaternion wristRotation = rotatedObject.rotation;
        Vector3 outwardDir = (rotatedObject.position - rb.position).normalized;

        return Quaternion.identity;

    }
}
