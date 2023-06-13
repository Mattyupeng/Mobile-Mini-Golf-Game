using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class limitloadprefab : MonoBehaviour
{

    //public Transform spawnPostion;
    public GameObject prefab;
    private GameObject placedprefab;
 
    public void InstantiateCaller()
    {
        if(placedprefab = null){
            placedprefab = Instantiate(prefab);
        }

    }

}
