using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class BuildingGhost : MonoBehaviour
{
    public MeshRenderer[] meshRenderers;
    public Material validMaterial;
    public Material invalidMaterial;

    private void Awake()
    {
        meshRenderers = GetComponentsInChildren<MeshRenderer>();
    }

    public void SetValid(bool isValid)
    {
        foreach (var r in meshRenderers)
            r.material = isValid ? validMaterial : invalidMaterial;
    }

    public void SetPosition(Vector3 worldPos)
    {
        transform.position = worldPos;
    }

    public void SetMaterialColor(bool canBuilding)
    {
        foreach (var r in meshRenderers)
        {
            r.material.color = canBuilding ? new Color(0, 1, 0, 0.5f) : new Color(1, 0, 0, 0.5f);
        }
    }
}