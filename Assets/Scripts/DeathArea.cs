using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class DeathArea : MonoBehaviour {

    public delegate bool GetCrash(Vector2 pos);
    public static List<GetCrash> crashlist = new List<GetCrash>();

    [Range(1f, 600f)]
    public float radius;

    private float r_sqr;

	void Start () {
        // add to the list
        crashlist.Add(this.IsCrash);

        r_sqr = radius * radius;
	}

    void OnDrawGizmos() {
        Gizmos.color = Color.red;
        Gizmos.DrawWireSphere(this.transform.position, radius);
    }

    public bool IsCrash(Vector2 pos) {
        Vector2 mypos = this.transform.position;

        if ((mypos - pos).SqrMagnitude() <= r_sqr) {
            return true;
        }
        return false;
    }
}
