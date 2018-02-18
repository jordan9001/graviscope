using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class CameraScript : MonoBehaviour {
    // Follows a player
    // controls the player
    // Also updats drawnobjects to

    public Ship player;

    [Range(60f, 3000f)]
    public float zoommax = 300f;
    [Range(3f, 30f)]
    public float zoommin = 1f;
    [Range(0.1f, 10f)]
    public float zoomsen = 1f;
    [Range(1f, 20f)]
    public float rotspeed = 6f;

    public delegate void UpdateZoom(float camsize);
    public static List<UpdateZoom> zoomers = new List<UpdateZoom>();

    private Camera cam;
    private Vector3 offset = Vector3.back * 30;
    private float startingsize;

    void Start () {
        cam = this.GetComponent<Camera>();
        startingsize = cam.orthographicSize;
	}
	
	void Update () {
        // follow player
        if (player != null) {
            this.transform.position = player.transform.position + this.offset;

            // get inputs

            // mouse look
            Vector2 look_dir = Input.mousePosition - Camera.main.WorldToScreenPoint(player.transform.position);
            float angle = Mathf.Atan2(look_dir.y, look_dir.x) * Mathf.Rad2Deg - 90f;
            player.transform.rotation = Quaternion.Slerp(player.transform.rotation, Quaternion.AngleAxis(angle, Vector3.forward), Time.deltaTime * rotspeed);

            // mouse click
            // left click
            if (Input.GetMouseButtonDown(0)) {
                // shoot
                player.Fire(WeaponRack.Mode.FIRE_DOWN);
            } else if (Input.GetMouseButtonUp(0)) {
                // left click up
                player.Fire(WeaponRack.Mode.FIRE_UP);
            }

            // middle click
            if (Input.GetMouseButtonDown(2)) {
                player.Fire(WeaponRack.Mode.FIRE_SPEC);
            }

            // right click
            if (Input.GetMouseButtonDown(1)) {
                // thrust
                player.Thrust(1.0f);
            } else if (Input.GetMouseButtonUp(1)) {
                player.StopThrust();
            }
        }

        // zoom
        float scroll = Input.GetAxis("Mouse ScrollWheel");
        if (scroll != 0) {
            cam.orthographicSize -= scroll * zoomsen * cam.orthographicSize;
            if (cam.orthographicSize > zoommax) {
                cam.orthographicSize = zoommax;
            }
            if (cam.orthographicSize < zoommin) {
                cam.orthographicSize = zoommin;
            }

            foreach (UpdateZoom z in zoomers) {
                z(cam.orthographicSize / startingsize);
            }
        }

    }
}
