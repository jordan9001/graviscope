using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public interface IWeapon {
    // loc is where it starts from
    // vel is inherited velocity
    // dir is direction to shoot
    // mode is button up, down or special
    // retuns ammo used
    int Fire(Vector2 loc, Vector2 vel, Vector2 dir, WeaponRack.Mode mode);

    // returns ammo needed for a certain mode
    int ModeAmmo(WeaponRack.Mode mode);

    float GetReloadTime();

    string GetName();
}

public class WeaponRack : MonoBehaviour {

    public enum Mode { FIRE_UP, FIRE_DOWN, FIRE_SPEC };

    public static List<IWeapon> all_weapons = new List<IWeapon>();

    [HideInInspector]
    public List<int> weapon_ammo;
    [HideInInspector]
    public int selected = 0;

    private float reloaded = 0.0f;

    void Start() {
        if (all_weapons.Count == 0) {
            Debug.Log("Error! Zero count all_weapons");
        }

        weapon_ammo = new List<int>(all_weapons.Count);
        for (int i = 0; i < all_weapons.Count; i++) {
            weapon_ammo.Add(6);
        }
    }

    public void AddAmmo(int amt, string name) {
        for (int i = 0; i < all_weapons.Count; i++) {
            if (all_weapons[i].GetName() == name) {
                weapon_ammo[i] += amt;
                return;
            }
        }
        Debug.LogFormat("Tried to add ammo to {0}, but could not find weapon", name);
    }

    public bool Fire(Vector2 loc, Vector2 vel, Vector2 dir, WeaponRack.Mode mode) {
        if (all_weapons[selected].ModeAmmo(mode) > weapon_ammo[selected]) {
            return false;
        }

        if (mode == WeaponRack.Mode.FIRE_DOWN) {
            if (Time.fixedTime < reloaded) {
                return false;
            } else {
                // set time when reloaded
                reloaded = Time.fixedTime + all_weapons[selected].GetReloadTime();
            }
        }

        weapon_ammo[selected] -= all_weapons[selected].Fire(loc, vel, dir, mode);

        return true;
    }
}
