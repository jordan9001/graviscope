﻿using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Wep_Nuke : MonoBehaviour, IWeapon {

    [Range(0.03f, 15f)]
    public float reload_time = 0.45f;
    public float shot_offset = 3.0f;
    public string wepname;
    public GameObject missle_fab;

	void Awake () {
        // join the WeaponRack
        WeaponRack.all_weapons.Add(this);
        Debug.Log("Added nuke to weapons");
        // TODO initialize pool
	}

    public int Fire(Vector2 loc, Vector2 vel, Vector2 dir, WeaponRack.Mode mode) {
        switch (mode) {
            case WeaponRack.Mode.FIRE_DOWN:
                break;
            default:
                return 0;
        }

        // launch missle from pool
        // TODO

        GameObject m = Instantiate(missle_fab);
        m.transform.position = loc + (dir * shot_offset);
        float angle = Mathf.Atan2(dir.y, dir.x) * Mathf.Rad2Deg;
        m.transform.rotation = Quaternion.AngleAxis(angle, Vector3.forward);
        Ship s = m.GetComponent<Ship>();
        s.vel = vel + (dir * s.thrust); // should actually include mass, but whatever

        return 1;
    }

    public int ModeAmmo(WeaponRack.Mode mode) {
        switch (mode) {
            case WeaponRack.Mode.FIRE_DOWN:
                return 1;
        }
        return 0;
    }

    public float GetReloadTime() {
        return reload_time;
    }

    public string GetName() {
        return wepname;
    }
}
