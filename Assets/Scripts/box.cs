using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.XR.ARFoundation;
using UnityEngine.XR.ARSubsystems;


public class box : MonoBehaviour
{
    //private bool isSelected = false;
    private Vector3 previousPosition;
    private Quaternion previousRotation;
    public LayerMask layerMask;
    private GameObject placedDuck;
    private GameObject placedref;
    private ARRaycastManager raycastManager;
    static List<ARRaycastHit> hits = new List<ARRaycastHit>();

    void Start()
    {
        raycastManager = GetComponent<ARRaycastManager>();
    }

    void Update()
    {

        // Check for touch input
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

            // // Raycast from touch position
            // Ray ray = Camera.main.ScreenPointToRay(Input.GetTouch(0).position);
            // RaycastHit hit;
            // if (Physics.Raycast(ray, out hit, Mathf.Infinity, layerMask))
            // {
            //     // Check if building block is hit
            //     if (hit.collider.gameObject.CompareTag("UndoRedo"))//||hit.collider.gameObject.CompareTag("Wallunit")
            //     {
            //         // Select/deselect building block
            //         if (isSelected)
            //         {
            //             DeselectBlock(hit.collider.gameObject);
            //         }
            //         else
            //         {
            //             SelectBlock(hit.collider.gameObject);
            //         }
            //     }
            // }


        }

        // // Translate and rotate selected block
        // if (isSelected)
        // {
        //     // Translate block along xz plane
        //     Vector3 position = transform.position;
        //     position.x = previousPosition.x + Input.GetAxis("Horizontal");
        //     position.z = previousPosition.z + Input.GetAxis("Vertical");
        //     transform.position = position;

        //     // Rotate block around y axis
        //     Quaternion rotation = transform.rotation;
        //     rotation *= Quaternion.Euler(0, Input.GetAxis("Rotation"), 0);
        //     transform.rotation = rotation;
        // }
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

            placedDuck = GameObject.FindWithTag("UndoRedo");
            placedDuck.transform.position = hit.position;

            placedref = GameObject.FindWithTag("Ground");
            placedDuck.transform.position = hit.position;

            // placedref = GameObject.FindWithTag("Ground");
            // placedDuck.transform.position = hit.position;

            // placedref = GameObject.FindWithTag("Ground");
            // placedDuck.transform.position = hit.position;


            // if (placedDuck == null)
            // {
            //     placedDuck = Instantiate(duckPrefab, hit.position, Quaternion.identity);
            // }
            // else 

        }
    }

    // // Select building block
    // void SelectBlock(GameObject block)
    // {
    //     isSelected = true;
    //     previousPosition = block.transform.position;
    //     previousRotation = block.transform.rotation;
    //     block.GetComponent<Renderer>().material.color = Color.yellow;
    // }

    // // Deselect building block
    // void DeselectBlock(GameObject block)
    // {
    //     // Check for intersections
    //     if (!CheckForIntersections(block))
    //     {
    //         isSelected = false;
    //         block.GetComponent<Renderer>().material.color = Color.white;
    //     }
    //     else
    //     {
    //         // Restore previous position and rotation
    //         block.transform.position = previousPosition;
    //         block.transform.rotation = previousRotation;
    //     }
    // }

    // // Check for intersections between building blocks
    // bool CheckForIntersections(GameObject block)
    // {
    //     Collider[] colliders = Physics.OverlapBox(block.transform.position, block.transform.localScale / 2);
    //     foreach (Collider collider in colliders)
    //     {
    //         if (collider.gameObject != block && collider.gameObject.CompareTag("Wallunit") && collider.gameObject.CompareTag("UndoRedo"))
    //         {
    //             return true;
    //         }
    //     }
    //     return false;
    // }

    // // Delete building block
    // void DeleteBlock(GameObject block)
    // {
    //     Destroy(block);
    // }
}
