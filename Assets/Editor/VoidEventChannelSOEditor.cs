using UnityEngine;
using UnityEditor;
using System.Collections.Generic;
using System.Reflection;
using UnityEngine.SceneManagement;

/// <summary>
/// VoidEventChannelSO 클래스에 대한 커스텀 에디터입니다.
/// 인스펙터에 "Find References in Current Scene" 버튼을 추가합니다.
/// </summary>
[CustomEditor(typeof(VoidEventChannelSO))]
public class VoidEventChannelSOEditor : Editor
{
    public override void OnInspectorGUI()
    {
        // 기본 인스펙터 UI를 먼저 그립니다.
        base.OnInspectorGUI();

        // 에디터가 플레이 중이 아닐 때만 버튼을 표시합니다.
        if (!Application.isPlaying)
        {
            EditorGUILayout.Space(10); // 보기 좋게 여백을 추가합니다.

            // "현재 씬에서 참조 찾기" 버튼
            if (GUILayout.Button("Find References in Current Scene"))
            {
                // `target`은 현재 인스펙터에서 보고 있는 VoidEventChannelSO 에셋입니다.
                FindReferencesInCurrentScene(target as ScriptableObject);
            }
        }
    }

    /// <summary>
    /// 현재 열려있는 씬에서 특정 ScriptableObject를 참조하는 모든 게임 오브젝트를 찾습니다.
    /// </summary>
    /// <param name="assetToFind">찾고자 하는 ScriptableObject 에셋</param>
    private void FindReferencesInCurrentScene(ScriptableObject assetToFind)
    {
        if (assetToFind == null) return;

        var activeScene = SceneManager.GetActiveScene();
        // 씬이 유효하지 않으면 실행하지 않습니다.
        if (!activeScene.IsValid())
        {
            Debug.LogWarning("Cannot find references in an invalid scene.");
            return;
        }

        var rootGameObjects = activeScene.GetRootGameObjects();
        var foundObjects = new List<GameObject>();

        foreach (var go in rootGameObjects)
        {
            // 각 최상위 오브젝트와 그 모든 자식 오브젝트를 검사합니다.
            FindReferencesRecursively(go, assetToFind, foundObjects);
        }

        // 검색 결과를 콘솔에 출력합니다.
        Debug.Log($"--- References to '{assetToFind.name}' found in scene '{activeScene.name}': {foundObjects.Count} ---", assetToFind);
        if (foundObjects.Count > 0)
        {
            foreach (var obj in foundObjects)
            {
                // 콘솔 로그를 클릭하면 하이어라키에서 해당 게임 오브젝트가 선택(ping)됩니다.
                Debug.Log($"Found in GameObject: '{obj.name}'", obj);
            }
        }
        else
        {
            Debug.Log("No references found in the current scene's hierarchy.");
        }
    }

    /// <summary>
    /// 특정 게임 오브젝트와 그 자식들에서 참조를 검색하는 재귀 함수입니다.
    /// </summary>
    private void FindReferencesRecursively(GameObject go, ScriptableObject assetToFind, List<GameObject> foundObjects)
    {
        // 게임 오브젝트에 연결된 모든 컴포넌트를 가져옵니다.
        Component[] components = go.GetComponents<Component>();

        foreach (var component in components)
        {
            // 스크립트가 누락된 경우 등 컴포넌트가 null일 수 있으므로 건너뜁니다.
            if (component == null) continue;

            // 리플렉션을 사용하여 컴포넌트의 모든 필드(public, private, protected 등)를 가져옵니다.
            FieldInfo[] fields = component.GetType().GetFields(BindingFlags.Public | BindingFlags.NonPublic | BindingFlags.Instance);

            foreach (var field in fields)
            {
                // 필드의 값을 가져와서 찾고자 하는 에셋과 직접 비교합니다.
                if (field.GetValue(component) == assetToFind)
                {
                    // 아직 리스트에 추가되지 않은 게임 오브젝트만 추가합니다.
                    if (!foundObjects.Contains(go))
                    {
                        foundObjects.Add(go);
                    }
                    // 이 게임 오브젝트에서는 참조를 찾았으므로 더 이상 필드를 검사할 필요가 없습니다.
                    // 바로 다음 자식 오브젝트 검사로 넘어갑니다.
                    goto NextObject;
                }
            }
        }

    NextObject:
        // 모든 자식 오브젝트에 대해 재귀적으로 이 함수를 호출합니다.
        foreach (Transform child in go.transform)
        {
            FindReferencesRecursively(child.gameObject, assetToFind, foundObjects);
        }
    }
}