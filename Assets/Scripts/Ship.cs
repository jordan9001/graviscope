using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Ship : MonoBehaviour {
    // objects that can be pulled on by grav, and thrust around

    public delegate Vector2 GetGrav(Vector2 pos, float timeoff);
    public static List<GetGrav> heavyobjs = new List<GetGrav>();

    [Range(0.1f, 1000f)]
    public float mass = 1.0f;
    [Range(0.1f, 1000f)]
    public float thrust = 1.0f;

    // this velocity is worldspace
    // doesn't scale or rotate with object
    private Vector2 vel;
    private float thrust_amt = 0f;

    void Start () {
		
	}
	
	void FixedUpdate () {
        Vector2 acc = Vector2.zero;

        Vector2 pos = new Vector2(this.transform.position.x, this.transform.position.y);

        // get acc from gravity
        foreach (GetGrav g in heavyobjs) {
            acc += g(pos, 0.0f);
        }

        // get acc from thrust
        //TODO use fuel

        // a = f/m
        Vector2 tdir = this.transform.TransformDirection(Vector2.up);
        acc += tdir * ((this.thrust * thrust_amt) / this.mass);

        // apply the acc to the velocity for timestep
        // vf = vi + a*t
        this.vel += acc * Time.fixedDeltaTime;


        // apply the vel for timestep
        // pos = vel * time
        pos += this.vel * Time.fixedDeltaTime;
        this.transform.position = pos;

        // check collision
        foreach(DeathArea.GetCrash g in DeathArea.crashlist) {
            if (g(pos)) {
                // die
                this.Death();
            }
        }
	}

    // do a forward thrust
    public void Thrust(float amt) {
        thrust_amt = amt;
    }
    public void StopThrust() {
        thrust_amt = 0f;
    }

    public void Death() {
        // make an explosion the size of the mass
        Debug.Log("Death");
    }

    public Vector2[] GetFuture(int count, float timestep) {
        // run future to get path
        Vector2[] v = new Vector2[count];

        Vector2 pos = new Vector2(this.transform.position.x, this.transform.position.y);
        Vector2 fvel = this.vel;
        Vector2 acc;

        float timeoff = 0.0f;

        v[0] = pos;

        for (int i=1; i<count; i++) {
            // get next position
            acc = Vector2.zero;
            // don't take thrust into consideration, just grav
            foreach (GetGrav g in heavyobjs) {
                acc += g(pos, timeoff);
            }

            fvel += acc * timestep;
            pos += fvel * timestep;

            // set position in array
            v[i] = pos;

            timeoff += timestep;
        }

        return v;
    }
}
