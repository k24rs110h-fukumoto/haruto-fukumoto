#if UNITY_EDITOR
using System;
using System.Collections.Generic;

public enum DesignScanMode
{
    Full,
    Lightweight,
    LazyScene,
    Differential
}

[Serializable]
public class DesignInfoData
{
    public MetadataData metadata = new MetadataData();
    public PerformanceData performance = new PerformanceData();
    public StatisticsData statistics = new StatisticsData();

    public List<FolderData> folders = new List<FolderData>();
    public List<AssetData> assets = new List<AssetData>();
    public List<ScriptData> scripts = new List<ScriptData>();
    public List<PrefabData> prefabs = new List<PrefabData>();
    public List<SceneData> scenes = new List<SceneData>();
    public List<ModuleData> modules = new List<ModuleData>();

    public List<DesignNode> nodes = new List<DesignNode>();
    public List<DesignEdge> edges = new List<DesignEdge>();
}

[Serializable]
public class MetadataData
{
    public string projectName;
    public string unityVersion;
    public string exportedAt;
    public string scanMode;
}

[Serializable]
public class PerformanceData
{
    public long folderScanMs;
    public long assetScanMs;
    public long scriptScanMs;
    public long prefabScanMs;
    public long sceneScanMs;
    public long moduleBuildMs;
    public long cacheScanMs;
    public long jsonWriteMs;
    public long totalMs;
}

[Serializable]
public class StatisticsData
{
    public int folderCount;
    public int assetCount;
    public int scriptCount;
    public int prefabCount;
    public int sceneCount;
    public int moduleCount;
    public int nodeCount;
    public int edgeCount;
    public int changedAssetCount;
    public int reusedAssetCount;
}

[Serializable]
public class FolderData
{
    public string path;
}

[Serializable]
public class AssetData
{
    public string guid;
    public string name;
    public string path;
    public string type;
    public string extension;
    public long fileSize;
    public string lastWriteTime;
}

[Serializable]
public class ScriptData
{
    public string guid;
    public string name;
    public string path;
    public string className;
    public string namespaceName;
    public string baseClass;
    public bool classResolved;

    public List<string> interfaces = new List<string>();
    public List<string> usings = new List<string>();
    public List<string> serializeFields = new List<string>();
    public List<string> methodDependencies = new List<string>();
}

[Serializable]
public class PrefabData
{
    public string guid;
    public string name;
    public string path;
    public List<GameObjectData> objects = new List<GameObjectData>();
}

[Serializable]
public class SceneData
{
    public string guid;
    public string name;
    public string path;
    public bool analyzed;
    public List<GameObjectData> rootObjects = new List<GameObjectData>();
}

[Serializable]
public class GameObjectData
{
    public string name;
    public string path;
    public bool active;
    public List<string> components = new List<string>();
    public List<GameObjectData> children = new List<GameObjectData>();
}

[Serializable]
public class ModuleData
{
    public string name;
    public string path;
    public List<string> assets = new List<string>();
    public List<DesignEdge> internalEdges = new List<DesignEdge>();
}

[Serializable]
public class DesignNode
{
    public string id;
    public string label;
    public string type;
    public string path;
}

[Serializable]
public class DesignEdge
{
    public string source;
    public string target;
    public string type;
    public string sourcePath;
    public string targetPath;
}

[Serializable]
public class CacheData
{
    public List<CacheAssetEntry> assets = new List<CacheAssetEntry>();
}

[Serializable]
public class CacheAssetEntry
{
    public string path;
    public string guid;
    public long fileSize;
    public string lastWriteTime;
}

[Serializable]
public class EvaluationData
{
    public string projectName;
    public string evaluatedAt;
    public List<ScanEvaluationResult> results = new List<ScanEvaluationResult>();
}

[Serializable]
public class ScanEvaluationResult
{
    public string scanMode;
    public long totalMs;

    public int folderCount;
    public int assetCount;
    public int scriptCount;
    public int prefabCount;
    public int sceneCount;
    public int moduleCount;
    public int nodeCount;
    public int edgeCount;

    public int changedAssetCount;
    public int reusedAssetCount;

    public double nodeCompletenessPercent;
    public double edgeCompletenessPercent;
    public double graphCompletenessPercent;
}
#endif