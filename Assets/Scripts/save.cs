using UnityEngine;

public class save : MonoBehaviour
{
    public void saveit()
    {
        Debug.Log(Application.persistentDataPath);
    }
}