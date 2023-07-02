using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UIElements;

public class UIController : MonoBehaviour
{
    public Button generateButton;
    public Label seedLabel;
    private MapGenerator mapGenerator;

    private void Awake()
    {
        mapGenerator = GameObject.Find("MapGenerator").GetComponent<MapGenerator>();
        
        var root = GetComponent<UIDocument>().rootVisualElement;
        generateButton = root.Q<Button>("GenerateButton");
        generateButton.clicked += GenerateButtonPressed;
        seedLabel = root.Q<Label>("SeedLabel");
    }

    private void GenerateButtonPressed() {
        mapGenerator.GenerateMap();
        seedLabel.text = "current seed: " + mapGenerator.seed;
    }
}
