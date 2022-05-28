using System.Collections;
using System.Collections.Generic;
using UnityEngine;

/*******
*   This script is attached to objects that can be picked up or moved by the player. It's only real purpose currently is to keep a reference to the original
*   position of the object.
*******/
public class MoveableObjectScript : MonoBehaviour
{
    private Vector3 _originalPosition;
    public Vector3 OriginalPosition { get { return _originalPosition; } }

    private Quaternion _originalRotation;
    public Quaternion OriginalRotation { get { return _originalRotation; } }

    // Start is called before the first frame update
    void Start()
    {
        _originalPosition = transform.position;
        _originalRotation = transform.rotation;
    }

    public void ResetPositionRotation() {
        transform.position = _originalPosition;
        transform.rotation = _originalRotation;
    }
}
