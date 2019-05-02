﻿using System.Collections;
using System.Collections.Generic;
using UnityEngine;

//Rope Script taken from https://www.habrador.com/tutorials/rope/3-another-simplified-rope/ 
//If we have a stiff rope, such as a metal wire, then we need a simplified solution
//this is also an accurate solution because a metal wire is not swinging as much as a rope made of a lighter material
public class RopeController : MonoBehaviour
{
    //Objects that will interact with the rope
    public Transform whatTheRopeIsConnectedTo;
    public Transform whatIsHangingFromTheRope;

    //Line renderer used to display the rope
    private LineRenderer lineRenderer;

    //A list with all rope sections
    public List<Vector3> allRopeSections = new List<Vector3>();

    void Start()
    {
        //Init the line renderer we use to display the rope
        lineRenderer = GetComponent<LineRenderer>();
    }

    void Update()
    {
        //Display the rope with a line renderer
        DisplayRope();
    }


    //Display the rope with a line renderer
    private void DisplayRope()
    {
        //Update the list with rope sections by approximating the rope with a bezier curve
        //A Bezier curve needs 4 control points
        Vector3 A = whatTheRopeIsConnectedTo.position;
        Vector3 D = whatIsHangingFromTheRope.position;

        //Upper control point
        //To get a little curve at the top than at the bottom
        Vector3 B = A + whatTheRopeIsConnectedTo.up * (-(A - D).magnitude * 0.1f);
        //B = A;

        //Lower control point
        Vector3 C = D + whatIsHangingFromTheRope.up * ((A - D).magnitude * 0.5f);

        //Get the positions
        BezierCurve.GetBezierCurve(A, B, C, D, allRopeSections);


        //An array with all rope section positions
        Vector3[] positions = new Vector3[allRopeSections.Count];

        for (int i = 0; i < allRopeSections.Count; i++)
        {
            positions[i] = allRopeSections[i];
        }

        //Add the positions to the line renderer
        lineRenderer.positionCount = positions.Length;

        lineRenderer.SetPositions(positions);
    }
}