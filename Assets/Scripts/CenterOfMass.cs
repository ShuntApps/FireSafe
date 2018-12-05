using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class CenterOfMass : MonoBehaviour {

    [SerializeField] Transform centerPos;
    Rigidbody rgdbody;

    void Start()
    {
        rgdbody = GetComponent<Rigidbody>();
        rgdbody.centerOfMass = centerPos.position;
    }
}
