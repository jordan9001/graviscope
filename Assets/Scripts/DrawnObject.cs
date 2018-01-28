using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class DrawnObject : MonoBehaviour {
    // takes in a series of points and turns it into a oscilliscope drawn object
    // we will have glow increase with z location value
    // https://www.youtube.com/watch?v=_E6NbLucJ2Y
    // we will have jitter from shader
    // we will dim based on motion with a camera shader

    public Vector3[] points;
    public bool isCircle;
    public Material mat;
    public float width = 1f;
    public bool loop = true;
    public bool worldspace = false;

    private LineRenderer rend;
    private const float widthadj = 0.09f;


    void Start () {
        // if it is a circle, generate the circle
        if (this.isCircle && this.points.Length > 2) {
            // get radius from first point
            float radius = points[0].magnitude;
            float step = 2 * Mathf.PI / points.Length;
            float r = 0;
            for (int i=0; i<points.Length; i++) {
                points[i] = new Vector3(Mathf.Cos(r), Mathf.Sin(r), 0f) * radius;
                r += step;
            }
        }
        rend = gameObject.AddComponent<LineRenderer>();
        rend.shadowCastingMode = UnityEngine.Rendering.ShadowCastingMode.Off;
        rend.receiveShadows = false;
        rend.material = mat;
        rend.useWorldSpace = this.worldspace;
        rend.loop = this.loop;
        rend.numCornerVertices = 0;
        rend.numCapVertices = 0;
        //rend.motionVectorGenerationMode = MotionVectorGenerationMode.Camera;
        rend.motionVectorGenerationMode = MotionVectorGenerationMode.ForceNoMotion;
        rend.positionCount = points.Length;
        rend.SetPositions(points);
        rend.widthMultiplier = this.width * widthadj;

        // add yourself as a zoomer
        CameraScript.zoomers.Add(UpdateZoom);
    }

    void OnDrawGizmos() {
        Gizmos.color = Color.green;

        if (this.isCircle && this.points.Length > 2) {
            float radius = points[0].magnitude;
            Gizmos.DrawWireSphere(this.transform.position, radius);
        } else {
            for (int i = 1; i < points.Length; i++) {
                Gizmos.DrawLine(this.transform.TransformPoint(points[i - 1]), this.transform.TransformPoint(points[i]));
            }
            if (points.Length > 2) {
                Gizmos.DrawLine(this.transform.TransformPoint(points[points.Length - 1]), this.transform.TransformPoint(points[0]));
            }
        }
    }

    public void UpdateZoom(float camsize) {
        rend.widthMultiplier = camsize * (this.width * widthadj);
    }

    public void UpdatePositions() {
        rend.SetPositions(points);
    }
}
