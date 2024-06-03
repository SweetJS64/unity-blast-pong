using UnityEngine;
using UnityEngine.Serialization;

public class DiskSpawner : MonoBehaviour
{
    [SerializeField] private GameObject diskPrefab;
    [SerializeField] private DiskConfig redDiskConfig;
    [SerializeField] private DiskConfig blueDiskConfig;

    private int _alternation = 1;

    private void OnMouseDown()
    {
        var diskGO = Spawn(diskPrefab);
        var diskController = diskGO.GetComponent<DiskController>();
        var diskConfig = _alternation > 0 ? redDiskConfig : blueDiskConfig;
        diskController.Init(diskConfig.throwPower, diskConfig.borderSize, diskConfig.stopThreshHold);
        diskGO.layer = ToLayer(diskConfig.layer);
        diskGO.GetComponent<Renderer>().material.color = diskConfig.Color;
    }

    private GameObject Spawn(GameObject disk)
    {
        var diskTransform = transform;
        var diskGO = Instantiate(disk, diskTransform.position, diskTransform.rotation, diskTransform.parent);
        _alternation *= -1;
        return diskGO;
    }

    public static int ToLayer(int bitmask)
    {
        int result = bitmask > 0 ? 0 : 31;
        while (bitmask > 1)
        {
            bitmask >>= 1;
            result++;
        }

        return result;
    }
}