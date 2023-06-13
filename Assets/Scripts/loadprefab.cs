using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class loadprefab : MonoBehaviour
{

    //public Transform spawnPostion;
    public GameObject prefab;
 
    public void InstantiateCaller()
    {
        Instantiate(prefab);
    }

}
