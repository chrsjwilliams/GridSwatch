using System;
using System.Collections;
using System.Collections.Generic;
using System.Linq;
using UnityEngine;
using UnityEngine.Purchasing;

public class StoreIconProvider
{
    public static Dictionary<string, Texture2D> Icons
    {
        get;
        private set;
    } = new();

    private static int TargetIconCount;

    public delegate void LoadCompleteAction();

    public static event LoadCompleteAction OnLoadComplete;

    public static void Initialize(ProductCollection products)
    {
        if (Icons.Count != 0)
        {
            Debug.Log("StoreIconProvider has already been initialized");
            return;
        }
        
        Debug.Log($"Loaidng store icons for {products.all.Length} products");
        TargetIconCount = products.all.Length;
        foreach (Product product in products.all)
        {
            Debug.Log($"Loading store icon at path StoreIcons/{product.definition.id}");
            ResourceRequest operation = Resources.LoadAsync<Texture2D>($"StoreIcons/{product.definition.id}");
            operation.completed += HandleLoadIcon;
        }
        
    }

    public static Texture2D GetIcon(string id)
    {
        if (Icons.Count == 0)
        {
            Debug.LogError("Called StoreIconProvider.GetIcon() before initializing! This is not a supported operation.");
            throw new InvalidOperationException(
                "StoreIconProvider.GetIcon() cannot be called before calling StoreIconProvider.Initialize()");
        }
        else
        {
            Icons.TryGetValue(id, out Texture2D icon);
            return icon;
        }
        
    }

    private static void HandleLoadIcon(AsyncOperation operation)
    {
        ResourceRequest request = operation as ResourceRequest;
        if (request.asset == null)
        {
            // Subtract from total because something failed to load
            TargetIconCount--;
            return;
        }
        
        Debug.Log($"Successfully loaded {request.asset.name}");
        Icons.Add(request.asset.name, request.asset as Texture2D);

        if (Icons.Count == TargetIconCount)
        {
            OnLoadComplete?.Invoke();
        }
    }
}
