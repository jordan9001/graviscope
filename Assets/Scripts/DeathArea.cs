using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class DeathArea : MonoBehaviour {

    public delegate bool GetCrash(Vector2 pos);
    public static List<GetCrash> crashlist = new List<GetCrash>();

    [Range(1f, 600f)]
    public float radius;
    public bool invert = false;

    private float r_sqr;

	void Start () {
        // add to the list
        crashlist.Add(this.IsCrash);

        r_sqr = radius * radius;
	}

    void OnDrawGizmos() {
        Gizmos.color = Color.red;
        if (invert) {
            Gizmos.color = Color.white;
        }
        Gizmos.DrawWireSphere(this.transform.position, radius);
    }

    public void Resize(float rad) {
        this.radius = rad;
        this.r_sqr = rad * rad;
    }

    private void OnDestroy() {
        crashlist.Remove(this.IsCrash);
    }

    public bool IsCrash(Vector2 pos) {
        Vector2 mypos = this.transform.position;

        if (pos == mypos) {
            // TODO
            // this may be a dumb way to check for our own object's death area?
            return false;
        }

        if (invert) {
            if ((mypos - pos).SqrMagnitude() > r_sqr) {
                return true;
            }
        } else {
            if ((mypos - pos).SqrMagnitude() < r_sqr) {
                return true;
            }
        }
        return false;
    }
}
