using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class TrailLine : MonoBehaviour {
    public int length = 27;
    public Material mat;

    private DrawnObject path;
    private Vector3[] points;

    void Start () {
        points = new Vector3[length];

        // set depth on the path
        for (int i = 0; i < length; i++) {
            points[i].x = transform.position.x;
            points[i].y = transform.position.y;
            points[i].z = (i * (1f / length));
        }

        path = gameObject.AddComponent<DrawnObject>();
        path.isCircle = false;
        path.mat = mat;
        path.width = 1f;
        path.points = points;
        path.loop = false;
        path.worldspace = true;
    }
	
	void Update () {
        // ugh I don't want to move all the points every time
        // ideally, this would be a circular buffer with a index to it's start
        // but the line renderer can't deal with that

        for (int i = points.Length-1; i >= 1; i--) {
            points[i].x = points[i - 1].x;
            points[i].y = points[i - 1].y;
        }
        points[0].x = transform.position.x;
        points[0].y = transform.position.y;

        path.UpdatePositions();
    }
}
