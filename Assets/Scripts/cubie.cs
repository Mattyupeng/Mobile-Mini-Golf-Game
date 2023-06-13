using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.XR.ARFoundation;
using UnityEngine.XR.ARSubsystems;

public class cubie : MonoBehaviour
{
    [SerializeField] private GameObject duckPrefab;
    //[SerializeField] private GameObject holePrefab;
    //[SerializeField] private GameObject ballPrefab;
    private GameObject placedDuck;
    //private GameObject placedhole;
    //private GameObject placedball;
    private ARRaycastManager raycastManager;
    static List<ARRaycastHit> hits = new List<ARRaycastHit>();

    void Start()
    {
        raycastManager = GetComponent<ARRaycastManager>();
    }

    void Update()
    {
        // only try placing object if one-finger touch
        if (Input.touchCount == 1)
        {
            //get touch
            Touch touch = Input.GetTouch(0);

            // only try placing if the touch is a tap
            // (as opposed to a drag/flick)
            if (touch.phase == TouchPhase.Ended)
            {
                TryPlaceDuck(touch);
            }
        }
    }

    // helper function to place duck
    void TryPlaceDuck(Touch touch)
    {
        // get touch position (2D, on screen)
        Vector2 touchPos = touch.position;

        // raycasts to worldspace (3D)
        // and check if the 3D space is on the ground
        if (raycastManager
            .Raycast(touchPos,
            hits,
            TrackableType.PlaneWithinPolygon))
        {
            
            var hit = hits[0].pose;
            
            if (placedDuck == null)
            {
                placedDuck = Instantiate(duckPrefab, hit.position, Quaternion.identity);
            }
            else placedDuck.transform.position = hit.position;

            // if (placedhole == null)
            // {
            //     placedhole = Instantiate(holePrefab, hit.position, Quaternion.identity);
            // }
            // else placedhole.transform.position = hit.position;

            // if (placedball == null)
            // {
            //     placedball = Instantiate(ballPrefab, hit.position, Quaternion.identity);
            // }
            // else placedball.transform.position = hit.position;
        }
    }
}
