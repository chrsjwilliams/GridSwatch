﻿using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Assertions;
using Object = UnityEngine.Object;



 public class GameSceneManager<TTransitionData>
 {
     internal GameObject SceneRoot { get; private set; }

     private readonly Dictionary<Type, GameObject> _scenes = new Dictionary<Type, GameObject>();
     private readonly Stack<Scene<TTransitionData>> _sceneStack = new Stack<Scene<TTransitionData>>();

     public static Action SceneChanged;
     
     public Scene<TTransitionData> CurrentScene
     {
         get
         {
             return _sceneStack.Count != 0 ? _sceneStack.Peek() : null;
         }
     }

     public GameSceneManager(GameObject root, IEnumerable<GameObject> scenePrefabs)
     {
         SceneRoot = root;
         foreach (var prefab in scenePrefabs)
         {
             var scene = prefab.GetComponent<Scene<TTransitionData>>();
             Assert.IsNotNull(scene, "Could not find scene script in prefab used to initialize SceneManager");
             _scenes.Add(scene.GetType(), prefab);
         }
     }

     public void PopScene(TTransitionData data = default(TTransitionData))
     {
         Scene<TTransitionData> previousScene = null;
         Scene<TTransitionData> nextScene = null;

         if (_sceneStack.Count != 0)
         {
             previousScene = _sceneStack.Peek();
             _sceneStack.Pop();
         }

         if (_sceneStack.Count != 0)
         {
             nextScene = _sceneStack.Peek();
         }

         if (nextScene != null)
         {
             nextScene._OnEnter(data);
         }

         if (previousScene != null)
         {
             previousScene._OnExit();
             Object.Destroy(previousScene.Root);
         }
     }

     public void PushScene<T>(TTransitionData data = default(TTransitionData), bool prevSceneActive = false) where T : Scene<TTransitionData>
     {
         var previousScene = CurrentScene;
         var nextScene = GetScene<T>();

         _sceneStack.Push(nextScene);
         nextScene._OnEnter(data);

         if (previousScene != null)
         {
             previousScene._OnExit();
             previousScene.Root.SetActive(prevSceneActive);
         }
     }

     public void Swap<T>(TTransitionData data = default(TTransitionData)) where T : Scene<TTransitionData>
     {
         Scene<TTransitionData> previousScene = null;
         if (_sceneStack.Count > 0)
         {

             previousScene = _sceneStack.Peek();
             _sceneStack.Pop();

         }

         var nextScene = GetScene<T>();
         _sceneStack.Push(nextScene);
         nextScene._OnEnter(data);

         if (previousScene != null)
         {
             previousScene._OnExit();
             Object.Destroy(previousScene.Root);
         }
     }

     private T GetScene<T>() where T : Scene<TTransitionData>
     {
         GameObject prefab;
         _scenes.TryGetValue(typeof(T), out prefab);
         Assert.IsNotNull(prefab, "Could not find scene prefab for scene type: " + typeof(T).Name);

         var sceneObject = Object.Instantiate(prefab);
         sceneObject.name = typeof(T).Name;
         sceneObject.transform.SetParent(SceneRoot.transform, false);
         return sceneObject.GetComponent<T>();
     }
 }



