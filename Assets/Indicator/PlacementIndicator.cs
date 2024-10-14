using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.XR.ARFoundation;
using UnityEngine.XR.ARSubsystems;

public class PlacementIndicator : MonoBehaviour {
    [SerializeField]
    private ARRaycastManager rayManager;
    private GameObject visual;

    void Start() {
        if (rayManager == null) {
            rayManager = FindObjectOfType<ARRaycastManager>();
        }
        visual = transform.GetChild(0).gameObject;
        visual.SetActive(false);
    }

    void Update() {
        List<ARRaycastHit> hits = new List<ARRaycastHit>();
        rayManager.Raycast(new Vector2(Screen.width / 2, Screen.height / 2), hits, TrackableType.PlaneWithinPolygon);

        if (hits.Count > 0) {
            transform.position = hits[0].pose.position;
            transform.rotation = hits[0].pose.rotation;

            if (!visual.activeInHierarchy) {
                visual.SetActive(true);
            }
        }
        else {
            if (visual.activeInHierarchy) {
                visual.SetActive(false);
            }
        }
    }
}