using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Factors : MonoBehaviour
{
    [Range(0, 1f)]
    public double iceFactor;
    [Range(0, 1f)]
    public double landFactor;
    [Range(1, 10)]
    public int initialPlains;
    [Range(0, 0.1f)]
    public double plainsPiecemeal;
    [Range(0, 1f)]
    public double oceanFactor;
    [Range(0, 0.15f)]
    public double snowFactor;
    [Range(0, 0.2f)]
    public double tundraFactor;
    [Range(0, 1f)]
    public double desertFactor;
    [Range(0, 0.1f)]
    public double desertPiecemeal;
    [Range(0, 20)]
    public int maxDesertSize;
    [Range(0, 0.1f)]
    public double oasisFactor;
    [Range(0, 1f)]
    public double grasslandFactor;
    [Range(0, 0.1f)]
    public double grasslandPiecemeal;
    [Range(0, 20)]
    public int maxGrasslandSize;
    [Range(0, 0.1f)]
    public double marshFactor;
    [Range(0, 1f)]
    public double woodsFactor;
    [Range(0, 1f)]
    public double rainforestFactor;
    [Range(0, 0.1f)]
    public double woodsPiecemeal;
    [Range(0, 20)]
    public int maxWoodsSize;
    [Range(0, 0.5f)]
    public double hillsFactor;
    [Range(0, 0.1f)]
    public double mountainFactor;
    [Range(0.5f, 1f)]
    public double mountainPiecemeal;
    [Range(0, 10)]
    public int maxMoutainSize;
}
