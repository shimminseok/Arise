using System.Collections.Generic;
using System.Linq;
using UnityEngine;

public class BuildingManager : SceneOnlySingleton<BuildingManager>
{
    private readonly List<TowerController> _placedTowers = new();

    public void RegisterTower(TowerController tower)
    {
        if (!_placedTowers.Contains(tower))
            _placedTowers.Add(tower);
    }

    public List<BuildingSaveData> GetAllBuildingData()
    {
        return _placedTowers
            .Where(t => t != null)
            .Select(t => t.GetSaveData())
            .ToList();
    }

    public void RebuildFromData(List<BuildingSaveData> dataList)
    {
        foreach (var data in dataList)
        {
            var towerObj = ObjectPoolManager.Instance.GetObject(data.TowerId);
            towerObj.transform.position = data.Position;

            if (towerObj.TryGetComponent(out TowerController tower))
            {
                // tower.UpgradeLevel = data.UpgradeLevel;
                tower.OnSpawnFromPool();
                tower.OnBuildComplete();
                RegisterTower(tower);
            }
        }
    }
}