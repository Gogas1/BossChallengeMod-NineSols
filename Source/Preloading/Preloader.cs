﻿using NineSolsAPI.Utils;
using System;
using System.Collections;
using System.Collections.Generic;
using System.IO;
using System.Text;
using UnityEngine;
using UnityEngine.SceneManagement;

namespace BossChallengeMod.Preloading {
    public class Preloader {

        Dictionary<string, Dictionary<string, GameObject>> preloads = new();

        private Dictionary<string, Dictionary<string, List<IPreloadTarget>>> preloadConsumers = new(); 

        public void AddPreload(string scene, string path, IPreloadTarget preloadTarget) {
            if (!preloadConsumers.TryGetValue(scene, out var scenePreloadsConsumers)) {
                scenePreloadsConsumers = [];
                preloadConsumers[scene] = scenePreloadsConsumers;
            }

            if (!preloadConsumers[scene].TryGetValue(path, out var consumers)) {
                consumers = [];
                preloadConsumers[scene][path] = consumers;
            }

            consumers.Add(preloadTarget);
        }

        private IEnumerator PreloadScenes() {
            var bundle = AssemblyUtils.GetEmbeddedAssetBundle("BossChallengeMod.Resources.Bundles.items.bundle");
            if (bundle == null) {
                Log.Error($"Items bundle not loaded");
                yield break;
            }

            try {
                List<AsyncOperation> ops = [];

                var add = (string scenePath, Scene scene) => {
                    try {
                        var rootObjects = scene.GetRootGameObjects();
                        var sceneName = Path.GetFileNameWithoutExtension(scenePath);
                        if (!preloadConsumers.TryGetValue(sceneName, out var scenePreloadsConsumers)) {
                            scenePreloadsConsumers = new();
                        }

                        if (!preloads.TryGetValue(sceneName, out var scenePreloads)) {
                            scenePreloads = [];
                            preloads[sceneName] = scenePreloads;
                        }
                        foreach (var obj in rootObjects) {
                            try {
                                Log.Info($"Preloading {obj.name}");
                                if(scenePreloads.ContainsKey(obj.name) || scenePreloadsConsumers.ContainsKey(obj.name)) {
                                    scenePreloads[obj.name] = obj;
                                    scenePreloadsConsumers[obj.name].ForEach(c => c.Set(obj));
                                    Log.Info($"No consumers for {obj.name} preload");
                                }

                                RCGLifeCycle.DontDestroyForever(obj);
                                AutoAttributeManager.AutoReference(obj);
                                AutoAttributeManager.AutoReferenceAllChildren(obj);
                            } catch (Exception ex) {
                                Log.Error($"{ex.Message}, {ex.StackTrace}");
                            }
                        }
                    } catch (Exception ex) {
                        Log.Error($"{ex.Message}, {ex.StackTrace}");
                    }

                };

                foreach (var scenePath in bundle.GetAllScenePaths()) {
                    var alreadyLoaded = SceneManager.GetSceneByPath(scenePath);
                    if (alreadyLoaded.isLoaded) yield return SceneManager.UnloadSceneAsync(alreadyLoaded);

                    var op = SceneManager.LoadSceneAsync(scenePath, LoadSceneMode.Additive);
                    if (op == null) continue;
                    op.completed += _ => add(scenePath, SceneManager.GetSceneByPath(scenePath));
                    ops.Add(op);
                }


                foreach (var op in ops) yield return op;

            } finally {
                bundle.Unload(false);
            }

        }

        public IEnumerator Preload() {
            yield return PreloadScenes();
            
        }
    }
}
