using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class CameraFollow : MonoBehaviour
{
    public Transform playerTransform;
    public Vector3 offset;
    public float smoothSpeed = 0.98f;

    private void LateUpdate(){
        Vector3 desiredPosition = playerTransform.position + offset;
        Vector3 smoothedPosition = Vector3.Lerp(transform.position, desiredPosition, smoothSpeed * Time.deltaTime);
        transform.position = smoothedPosition;
    }
}


// using System.Collections;
// using System.Collections.Generic;
// using UnityEngine;

// public class CameraFollow : MonoBehaviour
// {
//     public Transform playerTransform;
//     public Vector3 offset;

//     private void LateUpdate()
//     {
//         transform.position = playerTransform.position + offset;
//     }
// }
