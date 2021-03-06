﻿using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Heavy : MonoBehaviour {
    // pulls on light objects in their reach
    public float mass;
    [Range(1f, 600f)]
    public float range;

    public static float G = 0.6f;

    void Start() {
        // registor with the ships
        Ship.heavyobjs.Add(this.GetGrav);
    }


    // gets the gravity accelaration assuming much larger
    // time offset is for moving planets and stuff from future path stuff
    Vector2 GetGrav(Vector2 pos, float timeoff) {
        // g is a scalar amount
        // r is vec from ship to heavy
        // g = -G * (M/(||r||^2))

        float g;
        Vector2 mypos = this.transform.position;
        Vector2 r = mypos - pos;
        float r_sqm = r.SqrMagnitude();

        r.Normalize();
        //g = G * mass / r_sqm;
        // take G into consideration when setting mass
        g = mass / r_sqm;

        return g * r;
    }
}
