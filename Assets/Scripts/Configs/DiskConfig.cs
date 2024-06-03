using System.Security.Cryptography.X509Certificates;
using UnityEditor.Experimental.GraphView;
using UnityEngine;

[CreateAssetMenu(fileName = "Data", menuName = "ScriptableObjects/DiskConfig", order = 1)]
public class DiskConfig : ScriptableObject
{
    public float throwPower;
    public float borderSize;
    public float stopThreshHold;
    public LayerMask layer;
    public Color Color;
}
