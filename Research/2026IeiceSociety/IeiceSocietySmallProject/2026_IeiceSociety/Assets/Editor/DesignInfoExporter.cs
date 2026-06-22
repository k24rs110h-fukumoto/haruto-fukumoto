#if UNITY_EDITOR
using System;
using System.Collections.Generic;
using System.Diagnostics;
using System.IO;
using UnityEditor;
using UnityEditor.SceneManagement;
using UnityEngine;
using UnityEngine.SceneManagement;

public class DesignInfoExporter : EditorWindow
{
    private static readonly HashSet<string> nodeKeys = new HashSet<string>();
    private static readonly HashSet<string> edgeKeys = new HashSet<string>();

    private const string CachePath = "Assets/design-info-cache.json";

    [MenuItem("Tools/Design Info/Export/Full Scan")]
    public static void ExportFullScan()
    {
        Export(DesignScanMode.Full);
    }

    [MenuItem("Tools/Design Info/Export/Lightweight Scan")]
    public static void ExportLightweightScan()
    {
        Export(DesignScanMode.Lightweight);
    }

    [MenuItem("Tools/Design Info/Export/Lazy Scene Scan")]
    public static void ExportLazySceneScan()
    {
        Export(DesignScanMode.LazyScene);
    }

    [MenuItem("Tools/Design Info/Export/Differential Scan")]
    public static void ExportDifferentialScan()
    {
        Export(DesignScanMode.Differential);
    }

    public static DesignInfoData Export(DesignScanMode mode)
    {
        nodeKeys.Clear();
        edgeKeys.Clear();

        Stopwatch totalSw = Stopwatch.StartNew();

        DesignInfoData data = new DesignInfoData();
        data.metadata.projectName = Application.productName;
        data.metadata.unityVersion = Application.unityVersion;
        data.metadata.exportedAt = DateTime.Now.ToString("yyyy-MM-dd HH:mm:ss");
        data.metadata.scanMode = mode.ToString();

        if (mode == DesignScanMode.Differential)
        {
            Stopwatch cacheSw = Stopwatch.StartNew();
            bool cacheReusable = TryUseCache(data);
            cacheSw.Stop();
            data.performance.cacheScanMs = cacheSw.ElapsedMilliseconds;

            if (cacheReusable)
            {
                totalSw.Stop();
                data.performance.totalMs = totalSw.ElapsedMilliseconds;
                WriteJson(data, GetOutputPath(mode));
                UnityEngine.Debug.Log("Differential scan used previous cache.");
                return data;
            }
        }

        Stopwatch sw = Stopwatch.StartNew();
        ExportFolders(data);
        sw.Stop();
        data.performance.folderScanMs = sw.ElapsedMilliseconds;

        sw.Restart();
        ExportAssets(data);
        sw.Stop();
        data.performance.assetScanMs = sw.ElapsedMilliseconds;

        sw.Restart();
        ExportScripts(data);
        sw.Stop();
        data.performance.scriptScanMs = sw.ElapsedMilliseconds;

        if (mode == DesignScanMode.Full || mode == DesignScanMode.LazyScene || mode == DesignScanMode.Differential)
        {
            sw.Restart();
            ExportPrefabs(data);
            sw.Stop();
            data.performance.prefabScanMs = sw.ElapsedMilliseconds;
        }

        if (mode == DesignScanMode.Full || mode == DesignScanMode.Differential)
        {
            sw.Restart();
            ExportScenes(data, analyzeSceneContents: true);
            sw.Stop();
            data.performance.sceneScanMs = sw.ElapsedMilliseconds;
        }
        else if (mode == DesignScanMode.LazyScene)
        {
            sw.Restart();
            ExportScenes(data, analyzeSceneContents: false);
            sw.Stop();
            data.performance.sceneScanMs = sw.ElapsedMilliseconds;
        }

        sw.Restart();
        BuildModules(data);
        sw.Stop();
        data.performance.moduleBuildMs = sw.ElapsedMilliseconds;

        UpdateStatistics(data);

        totalSw.Stop();
        data.performance.totalMs = totalSw.ElapsedMilliseconds;

        WriteJson(data, GetOutputPath(mode));
        WriteCache(data);

        UnityEngine.Debug.Log($"Exported: {GetOutputPath(mode)}");
        UnityEngine.Debug.Log($"Mode: {mode}");
        UnityEngine.Debug.Log($"Total: {data.performance.totalMs} ms");
        UnityEngine.Debug.Log($"Assets: {data.statistics.assetCount}, Scripts: {data.statistics.scriptCount}, Nodes: {data.statistics.nodeCount}, Edges: {data.statistics.edgeCount}");

        return data;
    }

    private static string GetOutputPath(DesignScanMode mode)
    {
        switch (mode)
        {
            case DesignScanMode.Full:
                return "Assets/design-info-full.json";
            case DesignScanMode.Lightweight:
                return "Assets/design-info-lightweight.json";
            case DesignScanMode.LazyScene:
                return "Assets/design-info-lazy-scene.json";
            case DesignScanMode.Differential:
                return "Assets/design-info-differential.json";
            default:
                return "Assets/design-info.json";
        }
    }

    private static void WriteJson(DesignInfoData data, string path)
    {
        Stopwatch sw = Stopwatch.StartNew();
        File.WriteAllText(path, JsonUtility.ToJson(data, true));
        sw.Stop();

        data.performance.jsonWriteMs = sw.ElapsedMilliseconds;

        AssetDatabase.Refresh();
    }

    private static bool IsOutputJson(string path)
    {
        string fileName = Path.GetFileName(path);
        return fileName.StartsWith("design-info") || fileName.StartsWith("evaluation");
    }

    private static void AddNode(DesignInfoData data, string id, string label, string type, string path)
    {
        if (string.IsNullOrEmpty(id)) return;
        if (nodeKeys.Contains(id)) return;

        nodeKeys.Add(id);

        data.nodes.Add(new DesignNode
        {
            id = id,
            label = label,
            type = type,
            path = path
        });
    }

    private static void AddEdge(DesignInfoData data, string source, string target, string type, string sourcePath, string targetPath)
    {
        if (string.IsNullOrEmpty(source) || string.IsNullOrEmpty(target)) return;

        string key = source + "|" + target + "|" + type + "|" + sourcePath;

        if (edgeKeys.Contains(key)) return;

        edgeKeys.Add(key);

        data.edges.Add(new DesignEdge
        {
            source = source,
            target = target,
            type = type,
            sourcePath = sourcePath,
            targetPath = targetPath
        });
    }

    private static void ExportFolders(DesignInfoData data)
    {
        Queue<string> queue = new Queue<string>();
        queue.Enqueue("Assets");

        while (queue.Count > 0)
        {
            string folder = queue.Dequeue();

            if (folder != "Assets")
            {
                data.folders.Add(new FolderData { path = folder });
                AddNode(data, folder, Path.GetFileName(folder), "Folder", folder);
            }

            foreach (string child in AssetDatabase.GetSubFolders(folder))
            {
                queue.Enqueue(child);
            }
        }
    }

    private static void ExportAssets(DesignInfoData data)
    {
        foreach (string guid in AssetDatabase.FindAssets(""))
        {
            string path = AssetDatabase.GUIDToAssetPath(guid);

            if (!path.StartsWith("Assets/")) continue;
            if (AssetDatabase.IsValidFolder(path)) continue;
            if (IsOutputJson(path)) continue;

            UnityEngine.Object asset = AssetDatabase.LoadAssetAtPath<UnityEngine.Object>(path);
            if (asset == null) continue;

            string name = Path.GetFileNameWithoutExtension(path);
            string type = GetAssetType(path, asset);

            FileInfo fileInfo = new FileInfo(path);

            AssetData assetData = new AssetData
            {
                guid = guid,
                name = name,
                path = path,
                type = type,
                extension = Path.GetExtension(path),
                fileSize = fileInfo.Exists ? fileInfo.Length : 0,
                lastWriteTime = fileInfo.Exists ? fileInfo.LastWriteTimeUtc.ToString("o") : ""
            };

            data.assets.Add(assetData);

            AddNode(data, name, name, type, path);

            string folderPath = Path.GetDirectoryName(path);
            if (!string.IsNullOrEmpty(folderPath))
            {
                folderPath = folderPath.Replace("\\", "/");
                AddEdge(data, folderPath, name, "contains", folderPath, path);
            }
        }
    }

    private static void ExportScripts(DesignInfoData data)
    {
        foreach (string guid in AssetDatabase.FindAssets("t:MonoScript"))
        {
            string path = AssetDatabase.GUIDToAssetPath(guid);

            if (!path.StartsWith("Assets/")) continue;
            if (IsOutputJson(path)) continue;
            if (!path.EndsWith(".cs")) continue;

            MonoScript monoScript = AssetDatabase.LoadAssetAtPath<MonoScript>(path);
            if (monoScript == null) continue;

            Type type = monoScript.GetClass();

            ScriptData script = new ScriptData
            {
                guid = guid,
                name = Path.GetFileNameWithoutExtension(path),
                path = path,
                className = type != null ? type.Name : "",
                namespaceName = type != null ? type.Namespace : "",
                baseClass = type != null && type.BaseType != null ? type.BaseType.Name : "",
                classResolved = type != null
            };

            if (type != null)
            {
                foreach (Type interfaceType in type.GetInterfaces())
                {
                    script.interfaces.Add(interfaceType.Name);
                    AddEdge(data, type.Name, interfaceType.Name, "implements", path, interfaceType.AssemblyQualifiedName);
                }

                if (type.BaseType != null)
                {
                    AddEdge(data, type.Name, type.BaseType.Name, "inherits", path, type.BaseType.AssemblyQualifiedName);
                }
            }

            ExtractScriptTextRelations(data, script, path);

            data.scripts.Add(script);
        }
    }

    private static void ExtractScriptTextRelations(DesignInfoData data, ScriptData script, string path)
    {
        string text;

        try
        {
            text = File.ReadAllText(path);
        }
        catch
        {
            return;
        }

        string[] lines = text.Split('\n');
        bool previousLineSerializeField = false;

        foreach (string rawLine in lines)
        {
            string line = rawLine.Trim();

            if (line.StartsWith("//")) continue;

            if (line.StartsWith("using "))
            {
                string usingName = line.Replace("using ", "").Replace(";", "").Trim();

                if (!string.IsNullOrEmpty(usingName))
                {
                    script.usings.Add(usingName);
                }
            }

            if (line.StartsWith("[SerializeField]"))
            {
                previousLineSerializeField = true;

                string inlineType = TryExtractFieldType(line);

                if (!string.IsNullOrEmpty(inlineType))
                {
                    script.serializeFields.Add(inlineType);
                    AddEdge(data, script.name, inlineType, "serialize_field", path, inlineType);
                    previousLineSerializeField = false;
                }

                continue;
            }

            if (previousLineSerializeField)
            {
                string fieldType = TryExtractFieldType(line);

                if (!string.IsNullOrEmpty(fieldType))
                {
                    script.serializeFields.Add(fieldType);
                    AddEdge(data, script.name, fieldType, "serialize_field", path, fieldType);
                }

                previousLineSerializeField = false;
            }

            AddGenericRelation(data, script, path, line, "GetComponent", "get_component");
            AddGenericRelation(data, script, path, line, "AddComponent", "add_component");
            AddGenericRelation(data, script, path, line, "FindObjectOfType", "find_object");
            AddGenericRelation(data, script, path, line, "Instantiate", "instantiate");
            AddGenericRelation(data, script, path, line, "Resources.Load", "resources_load");
        }
    }

    private static void AddGenericRelation(
        DesignInfoData data,
        ScriptData script,
        string path,
        string line,
        string methodName,
        string relationType)
    {
        string targetType = TryExtractGenericType(line, methodName);

        if (string.IsNullOrEmpty(targetType)) return;

        script.methodDependencies.Add(relationType + ":" + targetType);
        AddEdge(data, script.name, targetType, relationType, path, targetType);
    }

    private static string TryExtractFieldType(string line)
    {
        line = line.Replace("[SerializeField]", "").Trim();
        line = line.Replace("private ", "");
        line = line.Replace("public ", "");
        line = line.Replace("protected ", "");
        line = line.Replace("readonly ", "");
        line = line.Replace("static ", "");

        if (!line.Contains(";")) return "";

        string[] parts = line.Split(new[] { ' ', '\t' }, StringSplitOptions.RemoveEmptyEntries);

        if (parts.Length < 2) return "";

        return parts[0].Trim();
    }

    private static string TryExtractGenericType(string line, string methodName)
    {
        string key = methodName + "<";
        int start = line.IndexOf(key, StringComparison.Ordinal);

        if (start < 0) return "";

        start += key.Length;

        int end = line.IndexOf(">", start, StringComparison.Ordinal);

        if (end < 0) return "";

        return line.Substring(start, end - start).Trim();
    }

    private static void ExportPrefabs(DesignInfoData data)
    {
        foreach (string guid in AssetDatabase.FindAssets("t:Prefab"))
        {
            string path = AssetDatabase.GUIDToAssetPath(guid);

            if (!path.StartsWith("Assets/")) continue;
            if (IsOutputJson(path)) continue;

            GameObject prefab = AssetDatabase.LoadAssetAtPath<GameObject>(path);
            if (prefab == null) continue;

            PrefabData prefabData = new PrefabData
            {
                guid = guid,
                name = Path.GetFileNameWithoutExtension(path),
                path = path
            };

            AddNode(data, prefabData.name, prefabData.name, "Prefab", path);

            ExportGameObjectRecursive(data, prefab.transform, prefabData.objects, prefabData.name, path);

            data.prefabs.Add(prefabData);
        }
    }

    private static void ExportScenes(DesignInfoData data, bool analyzeSceneContents)
    {
        string currentScenePath = SceneManager.GetActiveScene().path;

        foreach (string guid in AssetDatabase.FindAssets("t:Scene"))
        {
            string path = AssetDatabase.GUIDToAssetPath(guid);

            if (!path.StartsWith("Assets/")) continue;
            if (IsOutputJson(path)) continue;

            SceneData sceneData = new SceneData
            {
                guid = guid,
                name = Path.GetFileNameWithoutExtension(path),
                path = path,
                analyzed = analyzeSceneContents
            };

            AddNode(data, sceneData.name, sceneData.name, "Scene", path);

            if (analyzeSceneContents)
            {
                try
                {
                    Scene scene = EditorSceneManager.OpenScene(path, OpenSceneMode.Single);

                    foreach (GameObject root in scene.GetRootGameObjects())
                    {
                        ExportGameObjectRecursive(data, root.transform, sceneData.rootObjects, sceneData.name, path);
                    }
                }
                catch (Exception e)
                {
                    UnityEngine.Debug.LogWarning($"Scene skipped: {path}\n{e.Message}");
                    sceneData.analyzed = false;
                }
            }

            data.scenes.Add(sceneData);
        }

        if (!string.IsNullOrEmpty(currentScenePath) && File.Exists(currentScenePath))
        {
            EditorSceneManager.OpenScene(currentScenePath, OpenSceneMode.Single);
        }
    }

    private static void ExportGameObjectRecursive(
        DesignInfoData data,
        Transform transform,
        List<GameObjectData> list,
        string ownerName,
        string ownerPath)
    {
        GameObject obj = transform.gameObject;

        GameObjectData objectData = new GameObjectData
        {
            name = obj.name,
            path = GetHierarchyPath(transform),
            active = obj.activeSelf
        };

        AddNode(data, objectData.path, obj.name, "GameObject", ownerPath);
        AddEdge(data, ownerName, objectData.path, "has_game_object", ownerPath, objectData.path);

        foreach (Component component in obj.GetComponents<Component>())
        {
            if (component == null) continue;

            string componentName = component.GetType().Name;
            objectData.components.Add(componentName);

            AddNode(data, componentName, componentName, "Component", component.GetType().AssemblyQualifiedName);
            AddEdge(data, objectData.path, componentName, "has_component", ownerPath, component.GetType().AssemblyQualifiedName);
        }

        foreach (Transform child in transform)
        {
            ExportGameObjectRecursive(data, child, objectData.children, objectData.path, ownerPath);
        }

        list.Add(objectData);
    }

    private static string GetHierarchyPath(Transform transform)
    {
        string path = transform.name;
        Transform current = transform.parent;

        while (current != null)
        {
            path = current.name + "/" + path;
            current = current.parent;
        }

        return path;
    }

    private static void BuildModules(DesignInfoData data)
    {
        Dictionary<string, ModuleData> moduleMap = new Dictionary<string, ModuleData>();
        Dictionary<string, string> assetToModulePath = new Dictionary<string, string>();

        foreach (AssetData asset in data.assets)
        {
            string folderPath = Path.GetDirectoryName(asset.path);

            if (string.IsNullOrEmpty(folderPath)) continue;

            folderPath = folderPath.Replace("\\", "/");

            if (!moduleMap.ContainsKey(folderPath))
            {
                moduleMap[folderPath] = new ModuleData
                {
                    name = Path.GetFileName(folderPath),
                    path = folderPath
                };
            }

            moduleMap[folderPath].assets.Add(asset.name);

            if (!assetToModulePath.ContainsKey(asset.name))
            {
                assetToModulePath.Add(asset.name, folderPath);
            }
        }

        foreach (DesignEdge edge in data.edges)
        {
            if (!assetToModulePath.ContainsKey(edge.source)) continue;
            if (!assetToModulePath.ContainsKey(edge.target)) continue;

            string sourceModule = assetToModulePath[edge.source];
            string targetModule = assetToModulePath[edge.target];

            if (sourceModule == targetModule)
            {
                moduleMap[sourceModule].internalEdges.Add(edge);
            }
        }

        data.modules = new List<ModuleData>(moduleMap.Values);
    }

    private static void UpdateStatistics(DesignInfoData data)
    {
        data.statistics.folderCount = data.folders.Count;
        data.statistics.assetCount = data.assets.Count;
        data.statistics.scriptCount = data.scripts.Count;
        data.statistics.prefabCount = data.prefabs.Count;
        data.statistics.sceneCount = data.scenes.Count;
        data.statistics.moduleCount = data.modules.Count;
        data.statistics.nodeCount = data.nodes.Count;
        data.statistics.edgeCount = data.edges.Count;
    }

    private static bool TryUseCache(DesignInfoData data)
    {
        if (!File.Exists(CachePath)) return false;
        if (!File.Exists("Assets/design-info-full.json")) return false;

        CacheData cache = JsonUtility.FromJson<CacheData>(File.ReadAllText(CachePath));
        List<AssetData> currentAssets = CollectAssetMetadataOnly();

        int changed = 0;
        int reused = 0;

        foreach (AssetData current in currentAssets)
        {
            CacheAssetEntry old = cache.assets.Find(x => x.path == current.path);

            if (old == null)
            {
                changed++;
                continue;
            }

            if (old.fileSize != current.fileSize || old.lastWriteTime != current.lastWriteTime)
            {
                changed++;
            }
            else
            {
                reused++;
            }
        }

        if (changed > 0)
        {
            data.statistics.changedAssetCount = changed;
            data.statistics.reusedAssetCount = reused;
            return false;
        }

        DesignInfoData previous = JsonUtility.FromJson<DesignInfoData>(File.ReadAllText("Assets/design-info-full.json"));

        data.metadata = previous.metadata;
        data.metadata.exportedAt = DateTime.Now.ToString("yyyy-MM-dd HH:mm:ss");
        data.metadata.scanMode = DesignScanMode.Differential.ToString();

        data.folders = previous.folders;
        data.assets = previous.assets;
        data.scripts = previous.scripts;
        data.prefabs = previous.prefabs;
        data.scenes = previous.scenes;
        data.modules = previous.modules;
        data.nodes = previous.nodes;
        data.edges = previous.edges;

        data.statistics = previous.statistics;
        data.statistics.changedAssetCount = changed;
        data.statistics.reusedAssetCount = reused;

        return true;
    }

    private static List<AssetData> CollectAssetMetadataOnly()
    {
        List<AssetData> result = new List<AssetData>();

        foreach (string guid in AssetDatabase.FindAssets(""))
        {
            string path = AssetDatabase.GUIDToAssetPath(guid);

            if (!path.StartsWith("Assets/")) continue;
            if (AssetDatabase.IsValidFolder(path)) continue;
            if (IsOutputJson(path)) continue;

            FileInfo fileInfo = new FileInfo(path);

            result.Add(new AssetData
            {
                guid = guid,
                name = Path.GetFileNameWithoutExtension(path),
                path = path,
                extension = Path.GetExtension(path),
                fileSize = fileInfo.Exists ? fileInfo.Length : 0,
                lastWriteTime = fileInfo.Exists ? fileInfo.LastWriteTimeUtc.ToString("o") : ""
            });
        }

        return result;
    }

    private static void WriteCache(DesignInfoData data)
    {
        CacheData cache = new CacheData();

        foreach (AssetData asset in data.assets)
        {
            cache.assets.Add(new CacheAssetEntry
            {
                path = asset.path,
                guid = asset.guid,
                fileSize = asset.fileSize,
                lastWriteTime = asset.lastWriteTime
            });
        }

        File.WriteAllText(CachePath, JsonUtility.ToJson(cache, true));
    }

    private static string GetAssetType(string path, UnityEngine.Object asset)
    {
        string extension = Path.GetExtension(path).ToLower();

        if (extension == ".cs") return "Script";
        if (extension == ".prefab") return "Prefab";
        if (extension == ".unity") return "Scene";
        if (extension == ".asset") return "ScriptableObject";
        if (extension == ".mat") return "Material";
        if (extension == ".png" || extension == ".jpg" || extension == ".jpeg") return "Texture";
        if (extension == ".anim" || extension == ".controller") return "Animation";
        if (extension == ".wav" || extension == ".mp3" || extension == ".ogg") return "Audio";
        if (extension == ".inputactions") return "InputActionAsset";
        if (extension == ".asmdef") return "AssemblyDefinitionAsset";
        if (extension == ".json" || extension == ".txt" || extension == ".md") return "TextAsset";

        return asset.GetType().Name;
    }
}
#endif