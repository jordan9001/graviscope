using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Explosion : MonoBehaviour {
    public int point_count = 30;
    public float grow_time = 1.0f;
    public float start_size = 1.0f;
    public float end_size = 8.0f;
    public float fade_time = 0.6f;
    public Material mat;

    private Vector3[] points;
    private DrawnObject path;
    private DeathArea darea;

    private float time = 0f;

    void Start () {
        points = new Vector3[point_count];

        path = gameObject.AddComponent<DrawnObject>();
        path.isCircle = false;
        path.mat = mat;
        path.width = 1f;
        path.points = points;
        path.loop = false;
        path.worldspace = true;

        darea = gameObject.AddComponent<DeathArea>();
        darea.radius = start_size;

        Random_Cloud();
    }
	
	void Update () {
        time += Time.deltaTime;
        Random_Cloud();
	}

    void Random_Cloud() {
        float maxrad;
        float fade;

        if (time < grow_time) {
            fade = 0f;
            maxrad = start_size + ((end_size - start_size) * (time / grow_time));
        } else if (time < (grow_time + fade_time)) {
            maxrad = end_size;
            fade = ((time - grow_time) / fade_time);
        } else {
            Destroy(gameObject);
            return;
        }

        for (int i = 0; i < point_count; i++) {
            points[i] = this.transform.TransformPoint(Random.insideUnitSphere * maxrad);
            points[i].z = fade;
        }

        path.UpdatePositions();

        darea.Resize(maxrad);
    }
}
