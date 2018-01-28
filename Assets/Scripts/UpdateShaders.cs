using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class UpdateShaders : MonoBehaviour {
    public List<Material> jitterers = new List<Material>();
    //public start_jitter

    private List<float> originaljitter = new List<float>();
    private List<float> originalZjitter = new List<float>();

	void Start () {
        CameraScript.zoomers.Add(UpdateZoom);

        for (int i=0; i<jitterers.Count; i++) {
            originaljitter.Add(jitterers[i].GetFloat("_Jitter"));
            originalZjitter.Add(jitterers[i].GetFloat("_ZJitter"));
        }
    }

    void OnApplicationQuit() {
        for (int i = 0; i < jitterers.Count; i++) {
            jitterers[i].SetFloat("_Jitter", originaljitter[i]);
            jitterers[i].SetFloat("_ZJitter", originalZjitter[i]);
        }
    }

    public void UpdateZoom(float camsize) {
        for (int i = 0; i < jitterers.Count; i++) {
            jitterers[i].SetFloat("_Jitter", originaljitter[i] * camsize);
            jitterers[i].SetFloat("_ZJitter", originalZjitter[i] * camsize);
        }
    }
}
