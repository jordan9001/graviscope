using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class FutureLine : MonoBehaviour {

    public int length = 42;
    public Material mat;
    public Ship ship;
    [Range(1f, 20f)]
    public float timestep_sc = 1.8f; // if this is not 1 it will have error, which looks cool

    private DrawnObject path;
    [HideInInspector]
    private Vector3[] points;

	void Start () {
        points = new Vector3[length];

        // set depth on the path
        for (int i=0; i<length; i++) {
            points[i].x = 0;
            points[i].y = 0;
            points[i].z = (i * (1f/length));
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
        Vector2[] future;

        future = ship.GetFuture(length, timestep_sc * Time.fixedDeltaTime);

        for (int i=0; i<length; i++) {
            points[i].x = future[i].x;
            points[i].y = future[i].y;
        }
        path.UpdatePositions();
	}
}
