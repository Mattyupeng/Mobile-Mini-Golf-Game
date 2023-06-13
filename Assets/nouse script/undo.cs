using UnityEngine;
using System.Collections;
using System.Collections.Generic;
 
public class undo: MonoBehaviour 
{
 
    public class setting {
        public GameObject Obj;
        public Vector3 Pos;
        public Quaternion Rot;
        public bool Deleted;
         
        public void Restore(){
            Obj.transform.position = Pos;
            Obj.transform.rotation = Rot;
            Obj.SetActive (Deleted);
        }
        public setting(GameObject g){
            Obj = g;
            Pos = g.transform.position;
            Rot = g.transform.rotation;
            Deleted = g.activeSelf;
             
        }
    }
     
    public List<setting> UndoList;

    public void AddUndo (GameObject g){
        setting s = new setting (g);
        UndoList.Add (s);
    }

    public void Undo (){
        if (UndoList.Count > 0) {
            UndoList[UndoList.Count-1].Restore();
            UndoList.RemoveAt(UndoList.Count-1);
        }
    }
 
    void Start () {
        UndoList = new List<setting> ();
    }
     
 
}