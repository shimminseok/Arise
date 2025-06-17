using UnityEngine;

public class TurretInstallTracker : MonoBehaviour
{
    public static TurretInstallTracker Instance { get; private set; }
    public bool HasInstalledTurret { get; private set; }

    private void Awake()
    {
        Instance = this;
    }

    public void OnTurretInstalled()
    {
        HasInstalledTurret = true;
    }
}