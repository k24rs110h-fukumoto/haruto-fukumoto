#if UNITY_EDITOR
using System;
using System.IO;
using UnityEditor;
using UnityEngine;

public class DesignInfoEvaluator : EditorWindow
{
    [MenuItem("Tools/Design Info/Evaluate/Run All Scan Comparison")]
    public static void RunAllScanComparison()
    {
        DesignInfoData full = DesignInfoExporter.Export(DesignScanMode.Full);
        DesignInfoData lightweight = DesignInfoExporter.Export(DesignScanMode.Lightweight);
        DesignInfoData lazyScene = DesignInfoExporter.Export(DesignScanMode.LazyScene);
        DesignInfoData differential = DesignInfoExporter.Export(DesignScanMode.Differential);

        EvaluationData evaluation = new EvaluationData
        {
            projectName = Application.productName,
            evaluatedAt = DateTime.Now.ToString("yyyy-MM-dd HH:mm:ss")
        };

        evaluation.results.Add(BuildResult(full, full));
        evaluation.results.Add(BuildResult(lightweight, full));
        evaluation.results.Add(BuildResult(lazyScene, full));
        evaluation.results.Add(BuildResult(differential, full));

        string outputPath = "Assets/evaluation.json";
        File.WriteAllText(outputPath, JsonUtility.ToJson(evaluation, true));

        AssetDatabase.Refresh();

        UnityEngine.Debug.Log("Evaluation exported: Assets/evaluation.json");
    }

    private static ScanEvaluationResult BuildResult(DesignInfoData target, DesignInfoData full)
    {
        double nodeCompleteness = full.statistics.nodeCount > 0
            ? Math.Round((double)target.statistics.nodeCount / full.statistics.nodeCount * 100.0, 2)
            : 0;

        double edgeCompleteness = full.statistics.edgeCount > 0
            ? Math.Round((double)target.statistics.edgeCount / full.statistics.edgeCount * 100.0, 2)
            : 0;

        double graphCompleteness = Math.Round((nodeCompleteness + edgeCompleteness) / 2.0, 2);

        return new ScanEvaluationResult
        {
            scanMode = target.metadata.scanMode,
            totalMs = target.performance.totalMs,

            folderCount = target.statistics.folderCount,
            assetCount = target.statistics.assetCount,
            scriptCount = target.statistics.scriptCount,
            prefabCount = target.statistics.prefabCount,
            sceneCount = target.statistics.sceneCount,
            moduleCount = target.statistics.moduleCount,
            nodeCount = target.statistics.nodeCount,
            edgeCount = target.statistics.edgeCount,

            changedAssetCount = target.statistics.changedAssetCount,
            reusedAssetCount = target.statistics.reusedAssetCount,

            nodeCompletenessPercent = nodeCompleteness,
            edgeCompletenessPercent = edgeCompleteness,
            graphCompletenessPercent = graphCompleteness
        };
    }
}
#endif