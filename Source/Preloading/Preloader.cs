using NineSolsAPI.Utils;
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
        private Dictionary<string, List<IPreloadTarget>> simpleConsumers = new();

        public void AddPreload(string scene, string path, IPreloadTarget preloadTarget) {
            if(!simpleConsumers.TryGetValue(path, out var consumers)) {
                consumers = [];
                simpleConsumers[path] = consumers;
            }
            consumers.Add(preloadTarget);
        }
        private void PreloadAssets() {
            var bundle = AssemblyUtils.GetEmbeddedAssetBundle("BossChallengeMod.Resources.Bundles.bcmbundle.bundle");

            if(bundle == null) {
                Log.Error($"BCM bundle not loaded");
                return;
            }

            var assets = bundle.LoadAllAssets<GameObject>();

            foreach (var asset in assets) {
                var assetInstance = GameObject.Instantiate(asset);
                assetInstance.hideFlags = HideFlags.HideAndDontSave;

                if (!simpleConsumers.TryGetValue(asset.name, out var consumers)) {
                    continue;
                }


                foreach (var consumer in consumers) {
                    consumer.Set(assetInstance);
                }
            }
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

        public IEnumerator PreloadOld() {
            yield return PreloadScenes();
        }

        public void PreloadLatest() {
            PreloadAssets();
        }
    }
}
