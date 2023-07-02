using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEditor;

[CustomEditor(typeof(MapGenerator))]
public class MapGeneratorEditor : Editor {
    public override void OnInspectorGUI() {
        base.DrawDefaultInspector();
        if (GUILayout.Button("GenerateMap")) {
            ((MapGenerator) target).GenerateMap();
        }
        if (GUILayout.Button("CleanTileMap")) {
            ((MapGenerator) target).CleanTileMap();
        }
    }
}
