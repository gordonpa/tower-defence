#if UNITY_EDITOR
using UnityEditor;
using UnityEngine;
using System.IO;

public static class ModelIconExporter
{
    [MenuItem("Assets/Export Model Thumbnail", false, 1000)]
    static void ExportSelectedModelIcon()
    {
        GameObject selected = Selection.activeObject as GameObject;
        if (selected == null)
        {
            EditorUtility.DisplayDialog("提示", "请先选中一个模型预制体！", "OK");
            return;
        }

        GameObject root = new GameObject("__IconRenderRoot");

        // 1. 实例化并强制激活
        GameObject model = (GameObject)Object.Instantiate(selected, root.transform);
        model.name = selected.name;
        SetActiveRecursive(model, true);      // 确保全部激活

        // 2. 计算包围盒并居中缩放
        Bounds bounds = CalculateBounds(model);
        if (bounds.size == Vector3.zero)
        {
            EditorUtility.DisplayDialog("提示", "模型没有可用的 Renderer！", "OK");
            Object.DestroyImmediate(root);
            return;
        }

        float maxSize = Mathf.Max(bounds.size.x, bounds.size.y, bounds.size.z);
        float scale = 1.8f / maxSize;
        model.transform.localScale = Vector3.one * scale;
        model.transform.position = -bounds.center * scale;

        // 3. 创建相机
        Camera cam = new GameObject("__IconCamera").AddComponent<Camera>();
        cam.transform.SetParent(root.transform, false);
        cam.transform.position = new Vector3(0, 0, -5);
        cam.transform.LookAt(Vector3.zero);
        cam.orthographic = true;
        cam.orthographicSize = 1;
        cam.clearFlags = CameraClearFlags.SolidColor;
        cam.backgroundColor = new Color(220f / 255f, 235f / 255f, 255f / 255f, 1f);
        cam.nearClipPlane = 0.01f;
        cam.farClipPlane = 20f;

        // 4. 渲染目标
        RenderTexture rt = RenderTexture.GetTemporary(256, 256, 24, RenderTextureFormat.ARGB32);
        cam.targetTexture = rt;
        cam.Render();

        // 5. 读取像素
        RenderTexture prevRT = RenderTexture.active;
        RenderTexture.active = rt;
        Texture2D output = new Texture2D(256, 256, TextureFormat.ARGB32, false);
        output.ReadPixels(new Rect(0, 0, 256, 256), 0, 0);
        output.Apply();

        // 6. 清理
        RenderTexture.active = prevRT;
        RenderTexture.ReleaseTemporary(rt);
        Object.DestroyImmediate(root);

        // 7. 保存
        byte[] png = output.EncodeToPNG();
        Object.DestroyImmediate(output);

        string folder = Path.Combine(Application.dataPath, "../ExportedIcons/");
        if (!Directory.Exists(folder)) Directory.CreateDirectory(folder);

        string path = Path.Combine(folder, $"{selected.name}_Icon.png");
        File.WriteAllBytes(path, png);

        AssetDatabase.Refresh();
        Debug.Log($"已导出图标：{path}");
    }

    private static Bounds CalculateBounds(GameObject go)
    {
        Bounds b = new Bounds(go.transform.position, Vector3.zero);
        foreach (Renderer r in go.GetComponentsInChildren<Renderer>(true))
        {
            if (r != null) b.Encapsulate(r.bounds);
        }
        return b;
    }

    private static void SetActiveRecursive(GameObject go, bool active)
    {
        go.SetActive(active);
        foreach (Transform child in go.transform)
            SetActiveRecursive(child.gameObject, active);
    }
}
#endif