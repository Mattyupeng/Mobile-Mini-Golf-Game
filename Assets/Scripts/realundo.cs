using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class realundo : MonoBehaviour
{
    private Dictionary<GameObject, List<Vector3>> positionDict = new Dictionary<GameObject, List<Vector3>>();
    private Dictionary<GameObject, List<Quaternion>> rotationDict = new Dictionary<GameObject, List<Quaternion>>();
    private Dictionary<GameObject, int> currentStateIndexDict = new Dictionary<GameObject, int>();

    void Start()
    {
        // Initialize the dictionaries for each game object
        GameObject[] gameObjects = GameObject.FindGameObjectsWithTag("UndoRedo");
        foreach (GameObject gameObject in gameObjects)
        {
            positionDict[gameObject] = new List<Vector3>();
            rotationDict[gameObject] = new List<Quaternion>();
            currentStateIndexDict[gameObject] = 0;

            // Add the initial state of the game object to the lists
            positionDict[gameObject].Add(gameObject.transform.position);
            rotationDict[gameObject].Add(gameObject.transform.rotation);
        }
    }

    void Update()
    {
        // Update the dictionaries with the current state of each game object
        foreach (KeyValuePair<GameObject, List<Vector3>> kvp in positionDict)
        {
            GameObject gameObject = kvp.Key;
            List<Vector3> positionList = kvp.Value;
            List<Quaternion> rotationList = rotationDict[gameObject];

            positionList.Add(gameObject.transform.position);
            rotationList.Add(gameObject.transform.rotation);

            // Update the current state index for this game object
            currentStateIndexDict[gameObject] = positionList.Count - 1;
        }
    }

    public void Undo()
    {
        // Undo the last action for each game object
        foreach (KeyValuePair<GameObject, List<Vector3>> kvp in positionDict)
        {
            GameObject gameObject = kvp.Key;
            List<Vector3> positionList = kvp.Value;
            List<Quaternion> rotationList = rotationDict[gameObject];
            int currentStateIndex = currentStateIndexDict[gameObject];

            if (currentStateIndex > 0)
            {
                // Retrieve the previous state of the game object from the lists
                currentStateIndex--;
                gameObject.transform.position = positionList[currentStateIndex];
                gameObject.transform.rotation = rotationList[currentStateIndex];

                // Update the current state index for this game object
                currentStateIndexDict[gameObject] = currentStateIndex;
            }
        }
    }

    public void Redo()
    {
        // Redo the last undone action for each game object
        foreach (KeyValuePair<GameObject, List<Vector3>> kvp in positionDict)
        {
            GameObject gameObject = kvp.Key;
            List<Vector3> positionList = kvp.Value;
            List<Quaternion> rotationList = rotationDict[gameObject];
            int currentStateIndex = currentStateIndexDict[gameObject];

            if (currentStateIndex < positionList.Count - 1)
            {
                // Retrieve the next state of the game object from the lists
                currentStateIndex++;
                gameObject.transform.position = positionList[currentStateIndex];
                gameObject.transform.rotation = rotationList[currentStateIndex];

                // Update the current state index for this game object
                currentStateIndexDict[gameObject] = currentStateIndex;
            }
        }
    }
}
