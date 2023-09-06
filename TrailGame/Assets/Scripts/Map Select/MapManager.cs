using System;
using System.Collections;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using GameData;
using UnityEngine;
using UnityEngine.AddressableAssets;

public class MapManager : MonoBehaviour
{
    [SerializeField] string mapLabel;
    TaskManager tm = new TaskManager();

    [SerializeField] private List<MapData> _maps = new List<MapData>();
    public ReadOnlyCollection<MapData> Maps { get { return _maps.AsReadOnly(); } }

    public void Init()
    {
        LoadMapsAddressables(() => { Debug.Log("Finished Loading"); });
    }

    public void LoadMapsAddressables(Action callback)
    {

        var op = Addressables.LoadResourceLocationsAsync(mapLabel);

        op.Completed += ophandle =>
        {
            var result = ophandle.Result;

            var leftToLoad = ophandle.Result.Count;
            foreach (var readLocation in ophandle.Result)
            {
                var loaderOp = Addressables.LoadAssetAsync<MapData>(readLocation);
                loaderOp.Completed += opHandle =>
                {
                    if (!_maps.Contains(loaderOp.Result))
                    {
                        _maps.Add(loaderOp.Result);
                    }

                    leftToLoad--;
                    if (leftToLoad == 0)
                    {
                        callback?.Invoke();
                    }
                };
            }
        };
    }

    void Update()
    {
        tm.Update();
    }
}

public struct MapInfo
{

}
