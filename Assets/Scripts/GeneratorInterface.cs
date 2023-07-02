using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public interface GeneratorInterface {
    int[,] GenerateMapData(int width, int height, Factors factors);
}