using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEditor;
[CustomEditor(typeof(RoomManage))]
public class RoomManagerEditorScript : Editor
{
    public override void OnInspectorGUI() //like update called every frame
    {
        DrawDefaultInspector();
        EditorGUILayout.HelpBox("This script is responsible for creating and joining Room", MessageType.Info);
        RoomManage roomManager = (RoomManage) target;

        if(GUILayout.Button("Join Cafe Room"))
        {
            roomManager.OnEnterRoomBtnClicked_Cafe();
        }

     
    }
}
