using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class InstantiateCommand : IAction
{
    private GameObject toSpawnGameObject;
    private Vector3 positionToSpawn;
    //private Vector3 rotationtoSpawn;

    private GameObject spawnedGameObject;

    public InstantiateCommand(GameObject toSpawnGameObject, Vector3 positionToSpawn)
    {
        this.toSpawnGameObject = toSpawnGameObject;
        this.positionToSpawn = positionToSpawn;
        //this.rotationtoSpawn = rotationtoSpawn;
    }

    public void ExecuteCommand()
    {
        spawnedGameObject = GameObject.Instantiate(toSpawnGameObject, positionToSpawn, Quaternion.identity);
    }

    public void UndoCommand()
    {
        GameObject.Destroy(spawnedGameObject);
    }

}
