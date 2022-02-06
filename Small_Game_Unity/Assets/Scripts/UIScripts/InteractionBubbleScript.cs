using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class InteractionBubbleScript : MonoBehaviour
{
    [SerializeField]
    private GameObject targetObject; // reference to the object this bubble is hovering above

    public void SetTargetObject(GameObject targetObject) {
        this.targetObject = targetObject;
    }

    public void ClearTargetObject() {
        this.targetObject = null;
    }

    public GameObject GetTargetObject() {
        return this.targetObject;
    }
}
