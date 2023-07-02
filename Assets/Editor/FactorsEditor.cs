using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEditor;

[CustomEditor(typeof(Factors))]
public class FactorsEditor : Editor {
    public override void OnInspectorGUI() {
        base.DrawDefaultInspector();
    }
}