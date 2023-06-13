using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using UnityEngine.XR.ARFoundation;
using UnityEngine.EventSystems;
using System.IO;

[RequireComponent(typeof(ARRaycastManager))]
public class stone : MonoBehaviour
{
    [SerializeField]
    private Button BarButton;

    [SerializeField]
    private Button BoxButton;

    [SerializeField]
    private Button WallButton;

    [SerializeField]
    private Button TrashButton;

    [SerializeField]
    private Button VRButton;

    [SerializeField]
    private Button HoleButton;

    [SerializeField]
    private Button GBButton;

    [SerializeField]
    private Button SaveButton;

    // [SerializeField]
    // private Button RedoButton;

    // [SerializeField]
    // private Button UndoButton;

    [SerializeField]
    private PlacementObject[] placedObjects;

    private PlacementObject lastSelectedObject;

    private static List<ARRaycastHit> hits = new List<ARRaycastHit>();

    [SerializeField]
    private Color activeColor = Color.red;

    [SerializeField]
    private Color inactiveColor = Color.gray;

    [SerializeField]
    private Camera arCamera;

    private Vector2 touchPosition = default;

    [SerializeField]
    public LayerMask layerMask;

    [SerializeField]
    private Vector3 rotationSpeed = Vector3.zero;

    [SerializeField] 
    private CommandManager commandManager;

    private PlacementObject placedDuck;
    private PlacementObject placedhole;
    private PlacementObject placedball;
    //private Vector2 touchPosition = default;
    //private bool onHold = false;
    //private bool placementPoseIsValid = false;
    //public GameObject placementIndicator;
    private Pose PlacementPose;
    private float initialDistance;
    private Vector3 initialScale;
    Vector3 pos = new Vector3(0f, 10f, 0f);

    float lastTaptime = 0;
    float doubletapthresold = 0.3f;

    private GameObject placedPrefab;

    private ARRaycastManager arRaycastManager;

    // private Stack<IAction> historyStack = new Stack<IAction>();
    // private Stack<IAction> redoHistoryStack = new Stack<IAction>();


    void Awake() 
    {
        arRaycastManager = GetComponent<ARRaycastManager>();

        // set initial prefab
        //ChangePrefabTo("VR");

        BarButton.onClick.AddListener(() => ChangePrefabTo("Wall Bar"));
        BoxButton.onClick.AddListener(() => ChangePrefabTo("Box"));
        WallButton.onClick.AddListener(() => ChangePrefabTo("Wall Unit"));
        TrashButton.onClick.AddListener(() => ChangePrefabTo("trashbin"));
        VRButton.onClick.AddListener(() => ChangePrefabTo("VR"));
        HoleButton.onClick.AddListener(() => ChangePrefabTo("Hole"));
        GBButton.onClick.AddListener(() => ChangePrefabTo("golf ball"));
        SaveButton.onClick.AddListener(() => saveprefab());
        // UndoButton.onClick.AddListener(() => UndoCommand());
    }

    void Start()
    {
        ChangeSelectedObject(placedObjects[0]);
        Debug.Log(placedObjects[0]);
    }

    static public Vector3 posi;
    public Quaternion rot;
    private string filepath;

    void saveprefab()
    {
    string filepath = Path.Combine(Application.persistentDataPath, "vrgolf.csv");

    //find all gameobjects components
    GameObject[] objects = GameObject.FindObjectsOfType<GameObject>();
    Debug.Log(objects);

    //find ground gameobject component
    GameObject vr = GameObject.FindWithTag("Ground");

        using (StreamWriter writer = new StreamWriter(filepath))
        {
            writer.WriteLine("Tag,position.x,position.y,position.z,rotation");

            for (int i = 0; i < objects.Length; i++)
            {
                //Debug.Log(objects.Length);
                posi = objects[i].transform.position - vr.transform.position;
                rot = objects[i].transform.rotation;

                if(objects[i].CompareTag("UndoRedo") && objects[i].name ==("trashbin"))
                {
                    writer.WriteLine("Trashbin," + posi.x + "," + posi.y + "," + posi.z + "," + rot);
                }
                if(objects[i].name ==("Wall Unit"))
                {
                    writer.WriteLine("Wallunit," + posi.x + "," + posi.y + "," + posi.z + "," + rot);
                }
                if(objects[i].name == ("Wall Bar")) 
                {
                    writer.WriteLine("Wall Bar," + posi.x + "," + posi.y + "," + posi.z + "," + rot);
                }
                if(objects[i].name ==("Box"))
                {
                    writer.WriteLine("Box," + posi.x + "," + posi.y + "," + posi.z + "," + rot);
                }
                if(objects[i].CompareTag("Ground"))
                {
                    writer.WriteLine("Ground," + posi.x + "," + posi.y + "," + posi.z + "," + rot);
                }
                if(objects[i].CompareTag("Ball"))
                {
                    writer.WriteLine("Ball," + posi.x + "," + posi.y + "," + posi.z + "," + rot);
                }
                if(objects[i].CompareTag("Hole"))
                {
                    writer.WriteLine("Hole," + posi.x + "," + posi.y + "," + posi.z + "," + rot);
                }
            }
        }
    }

    void ChangePrefabTo(string prefabName)
    {
        if(placedPrefab = Resources.Load<GameObject>($"Prefab/{prefabName}"))
        {
            //Debug.Log("Prefab Loaded");
        }

        if(placedPrefab == null)
        {
            Debug.LogError($"Prefab with name {prefabName} could not be loaded, make sure you check the naming of your prefabs...");
        }
    }

    void ChangeSelectedObject(PlacementObject selected)
    {
        foreach (PlacementObject current in placedObjects)
        {   
            MeshRenderer meshRenderer = current.GetComponent<MeshRenderer>();
            if(selected != current) 
            {
                current.Selected = false;
                meshRenderer.material.color = inactiveColor;
            }
            else 
            {
                current.Selected = true;
                meshRenderer.material.color = activeColor;  
            }

        }
    }

    void Update()
    {
        if(placedPrefab == null)
        {
            return;
        }

        //select object with one finger touch
        if (Input.touchCount == 1)
        {
            //get touch
            Touch touch = Input.GetTouch(0);

            touchPosition = touch.position;

            if (touch.phase == TouchPhase.Began)
            {
                if(EventSystem.current.IsPointerOverGameObject())
                    return;
                
                Ray ray = arCamera.ScreenPointToRay(touchPosition);
                RaycastHit hitObject;
                if(Physics.Raycast(ray, out hitObject))
                {
                    lastSelectedObject = hitObject.transform.GetComponent<PlacementObject>();

                    if(Time.time-lastTaptime <= doubletapthresold)
                    {
                        Debug.Log("double tapping");
                        lastTaptime = 0;
                        Destroy(lastSelectedObject);
                    }
                    else if(lastSelectedObject != null)
                    {
                        PlacementObject[] allOtherObjects = FindObjectsOfType<PlacementObject>();
                        foreach(PlacementObject placementObject in allOtherObjects)
                        {
                            placementObject.Selected = placementObject == lastSelectedObject;
                            ChangeSelectedObject(lastSelectedObject);
                        }
                    }
                    else
                    {
                        lastTaptime = Time.time;
                    }
                }
            }

            if(touch.phase == TouchPhase.Ended)
            {
                lastSelectedObject.Selected = false;
            }

        }

        //Instantiating the object from touchposition method
        if(arRaycastManager.Raycast(touchPosition, hits, UnityEngine.XR.ARSubsystems.TrackableType.PlaneWithinPolygon))
        {
            if(EventSystem.current.IsPointerOverGameObject())
                return;
            //Debug.Log("not over UI");

            var hitPose = hits[0].pose;

            if(lastSelectedObject == null && placedPrefab.tag == ("UndoRedo"))
            {
                lastSelectedObject = Instantiate(placedPrefab, hitPose.position, hitPose.rotation).GetComponent<PlacementObject>();
            }
            if(lastSelectedObject == null && placedPrefab.tag == ("Wallunit"))
            {
                lastSelectedObject = Instantiate(placedPrefab, hitPose.position, hitPose.rotation).GetComponent<PlacementObject>();
            }
            if(lastSelectedObject == null && placedPrefab.tag == ("Ground"))
            {
                if(placedDuck == null)
                {
                    placedDuck = Instantiate(placedPrefab, hitPose.position, hitPose.rotation).GetComponent<PlacementObject>();
                }
                else placedDuck.transform.position = hitPose.position;
            }
            if(lastSelectedObject == null && placedPrefab.tag == ("Hole"))
            {
                if(placedhole == null)
                {
                    placedhole = Instantiate(placedPrefab, hitPose.position, hitPose.rotation).GetComponent<PlacementObject>();
                }
                else placedhole.transform.position = hitPose.position;
            }
            if(lastSelectedObject == null && placedPrefab.tag == ("Ball"))
            {
                if(placedball == null)
                {
                    placedball = Instantiate(placedPrefab, hitPose.position, hitPose.rotation).GetComponent<PlacementObject>();
                }
                else placedball.transform.position = hitPose.position;
            }        
            else
            {
                if(lastSelectedObject.Selected)
                {
                    lastSelectedObject.transform.position = hitPose.position;
                    //lastSelectedObject.transform.rotation = hitPose.rotation;
                    //lastSelectedObject.transform.SetPositionAndRotation(hitPose.position, lastSelectedObject.rotation);
                }
            }  
        }

        if(Input.touchCount == 2)
        {
            var touchZero = Input.GetTouch(0);
            var touchOne = Input.GetTouch(1);

            if(EventSystem.current.IsPointerOverGameObject())
                return;
            //Debug.Log("not over UI");

            // if any one of touchzero or touchOne is cancelled or maybe ended then do nothing
            if(touchZero.phase == TouchPhase.Ended || touchZero.phase == TouchPhase.Canceled ||
                touchOne.phase == TouchPhase.Ended || touchOne.phase == TouchPhase.Canceled)
            {
                return; // basically do nothing
            }

            if(touchZero.phase == TouchPhase.Stationary || touchOne.phase == TouchPhase.Stationary)
            {
                //if the object is onhold by two finger, start rotation
                lastSelectedObject.transform.Rotate(rotationSpeed * Time.deltaTime, Space.Self);
            }

            if(touchZero.phase == TouchPhase.Began || touchOne.phase == TouchPhase.Began)
            {
                initialDistance = Vector2.Distance(touchZero.position, touchOne.position);
                initialScale = lastSelectedObject.transform.localScale;
                //Debug.Log("Initial Disatance: " + initialDistance + "GameObject Name: "
                    //+ lastSelectedObject.name); // Just to check in console
                
            }
            else // if touch is moved
            {
               var currentDistance = Vector2.Distance(touchZero.position, touchOne.position);

                //if accidentally touched or pinch movement is very very small
                if (Mathf.Approximately(initialDistance, 0))
                {
                    return; // do nothing if it can be ignored where inital distance is very close to zero
                }

               var factor = currentDistance / initialDistance;
                lastSelectedObject.transform.localScale = initialScale * factor; // scale multiplied by the factor we calculated
                
            }
        }

    }

    //static List<ARRaycastHit> hits = new List<ARRaycastHit>();
}