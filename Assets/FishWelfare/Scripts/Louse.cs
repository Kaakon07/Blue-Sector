using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Louse : MonoBehaviour
{

    public bool marked = false;
    private Transform parent;
    private Transform boneParent;

private void Start() {
    parent = transform.parent;
    boneParent = parent.parent;
}
    private void Update() {
        //parent.position = boneParent.position;
    }
}
