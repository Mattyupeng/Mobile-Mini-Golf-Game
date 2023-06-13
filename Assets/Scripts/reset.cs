using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class reset : MonoBehaviour
{

    public void OnResetButtonClicked()
    {
        GameObject[] objects = GameObject.FindGameObjectsWithTag("UndoRedo");
        for (int i = 0; i < objects.Length; i++)
        {
            Destroy(objects[i]);
        }

        GameObject ball = GameObject.FindWithTag("Ball");
        Destroy(ball);

        GameObject hole = GameObject.FindWithTag("Hole");
        Destroy(hole);

        GameObject ground = GameObject.FindWithTag("Ground");
        Destroy(ground);

    }
}
