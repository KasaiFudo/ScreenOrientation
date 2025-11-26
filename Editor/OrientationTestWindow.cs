using System;
using System.Linq;
using System.Reflection;
using UnityEditor;
using UnityEditor.SceneManagement;
using UnityEngine;
using Object = UnityEngine.Object;

namespace KasaiFudo.ScreenOrientation.Editor
{
    public class OrientationTestWindow : EditorWindow
    {
        private bool _includeInactive = true;
        private bool _showDebugInfo = false;
        private Vector2 _scrollPosition;
        private string _addKeyInput = "";

        private IOrientationListener[] _cachedComponents;
        private Vector2 _lastGameViewSize;
        private float _lastUpdateTime;
        private const float UPDATE_INTERVAL = 0.5f;

        
        [MenuItem("Tools/Orientation Testing/Open Orientation Test Window")]
        public static void ShowWindow()
        {
            var window = GetWindow<OrientationTestWindow>("Orientation Tester");
            window.minSize = new Vector2(350, 400);
        }

        private void OnGUI()
        {
            var context = GetCurrentContext();
        
            DrawHeader(context);
            DrawMainButtons();
            DrawAdditionKeyEditor();
            DrawSettings();
            DrawComponentsList(context);
            DrawAdditionalTools(context);
            DrawContextInfo(context);
        }

        private void DrawHeader(WorkContext context)
        {
            EditorGUILayout.LabelField("Orientation Testing Tool", EditorStyles.boldLabel);
        
            string contextText = context.IsPrefabMode ? 
                $"Prefab Mode: {context.PrefabName}" : 
                $"Scene Mode: {context.SceneName}";
            
            var style = new GUIStyle(EditorStyles.helpBox);
            style.normal.textColor = context.IsPrefabMode ? Color.cyan : Color.green;
        
            EditorGUILayout.LabelField(contextText, style);
            EditorGUILayout.Space();
        }

        private void DrawMainButtons()
        {
            EditorGUILayout.BeginHorizontal();
            if (GUILayout.Button("Portrait", GUILayout.Height(30)))
            {
                ApplyOrientation(BasicScreenOrientation.Portrait);
            }
            if (GUILayout.Button("Landscape", GUILayout.Height(30)))
            {
                ApplyOrientation(BasicScreenOrientation.Landscape);
            }
            EditorGUILayout.EndHorizontal();

            EditorGUILayout.Space();
        }
        
        private void DrawAdditionKeyEditor()
        {
            EditorGUILayout.LabelField("Addition Data Key", EditorStyles.boldLabel);

            GUI.SetNextControlName("AdditionKeyTextField");
            _addKeyInput = EditorGUILayout.TextField("New Key:", _addKeyInput);

            var e = Event.current;
            bool enter =
                e.type == EventType.KeyUp &&
                e.keyCode == KeyCode.Return &&
                GUI.GetNameOfFocusedControl() == "AdditionKeyTextField";

            EditorGUILayout.BeginHorizontal();

            if (GUILayout.Button("Apply(Enter)") || enter)
            {
                ApplyAdditionKey();
                GUI.FocusControl(null);

                if (enter)
                    Event.current.Use(); // <-- КРИТИЧНО: убираем двойную обработку
            }

            EditorGUILayout.EndHorizontal();
            EditorGUILayout.Space();
        }
        
        private void ApplyAdditionKey()
        {
            if (!string.IsNullOrWhiteSpace(_addKeyInput))
            {
                AdditionDataKey.Key = _addKeyInput;
                Debug.Log($"New AdditionDataKey set: {_addKeyInput}");
            }
            else
            {
                Debug.LogWarning("Key is empty — ignored");
            }
        }

        private void DrawSettings()
        {
            EditorGUILayout.LabelField("Settings", EditorStyles.boldLabel);
            _includeInactive = EditorGUILayout.Toggle("Include Inactive Objects", _includeInactive);
            _showDebugInfo = EditorGUILayout.Toggle("Show Debug Info", _showDebugInfo);
            EditorGUILayout.Space();
        }

        private void DrawComponentsList(WorkContext context)
        {
            var components = GetComponents(context);
        
            string countText = context.IsPrefabMode ? 
                $"Found in Prefab: {components.Length}" : 
                $"Found in Scene: {components.Length}";
            
            EditorGUILayout.LabelField(countText, EditorStyles.helpBox);

            if (_showDebugInfo && components.Length > 0)
            {
                EditorGUILayout.LabelField("Components List:", EditorStyles.boldLabel);
                _scrollPosition = EditorGUILayout.BeginScrollView(_scrollPosition, GUILayout.Height(200));

                foreach (var component in components)
                {
                    DrawComponentItem(component, context);
                }

                EditorGUILayout.EndScrollView();
            }

            EditorGUILayout.Space();
        }

        private void DrawComponentItem(IOrientationListener component, WorkContext context)
        {
            if (component == null) return;
            
            var mono = component as MonoBehaviour;

            EditorGUILayout.BeginHorizontal();
        
            var icon = mono.gameObject.activeInHierarchy ? "✓" : "✗";
            EditorGUILayout.LabelField(icon, GUILayout.Width(20));
        
            string displayName = context.IsPrefabMode ? GetPrefabPath(mono.transform) : mono.name;

            if (GUILayout.Button(displayName, EditorStyles.linkLabel))
            {
                Selection.activeGameObject = mono.gameObject;
                EditorGUIUtility.PingObject(mono.gameObject);
            }
        
            EditorGUILayout.LabelField($"({component.GetType().Name})", EditorStyles.miniLabel);

            EditorGUILayout.EndHorizontal();
        }

        private void DrawAdditionalTools(WorkContext context)
        {
            EditorGUILayout.LabelField("Additional Tools", EditorStyles.boldLabel);

            EditorGUILayout.BeginHorizontal();
        
            if (GUILayout.Button("Select All"))
            {
                var components = GetComponents(context);
                if (components.Length > 0)
                {
                    Selection.objects = components.Select(c => (c as MonoBehaviour)?.gameObject).ToArray<Object>();
                }
            }
        
            EditorGUILayout.EndHorizontal();
        
            if (context.IsPrefabMode)
            {
                EditorGUILayout.Space();
                if (GUILayout.Button("Save Prefab Changes", GUILayout.Height(25)))
                {
                    SavePrefabChanges();
                }
            }

            EditorGUILayout.Space();
        }

        private void DrawContextInfo(WorkContext context)
        {
            var gameViewSize = GetGameViewSize();
            var currentOrientation = gameViewSize.x > gameViewSize.y ? "Landscape" : "Portrait";
        
            EditorGUILayout.HelpBox($"Game View: {gameViewSize.x:F0}x{gameViewSize.y:F0} ({currentOrientation})", 
                MessageType.Info);

            if (context.IsPrefabMode)
            {
                EditorGUILayout.HelpBox("Working in Prefab Mode - changes will affect the prefab asset", 
                    MessageType.Warning);
            }
        }

        private WorkContext GetCurrentContext()
        {
            var prefabStage = PrefabStageUtility.GetCurrentPrefabStage();
        
            if (prefabStage != null)
            {
                return new WorkContext
                {
                    IsPrefabMode = true,
                    PrefabStage = prefabStage,
                    PrefabName = prefabStage.prefabContentsRoot.name,
                    SceneName = null
                };
            }
            else
            {
                var activeScene = EditorSceneManager.GetActiveScene();
                return new WorkContext
                {
                    IsPrefabMode = false,
                    PrefabStage = null,
                    PrefabName = null,
                    SceneName = activeScene.name
                };
            }
        }

        private IOrientationListener[] GetComponents(WorkContext context)
        {
            if (_cachedComponents != null && Time.realtimeSinceStartup - _lastUpdateTime < UPDATE_INTERVAL)
            {
                return _cachedComponents;
            }

            if (context.IsPrefabMode)
            {
                _cachedComponents = GetPrefabComponents(context);
            }
            else
            {
                _cachedComponents = GetSceneComponents();
            }

            _lastUpdateTime = Time.realtimeSinceStartup;
            return _cachedComponents;
        }

        private IOrientationListener[] GetPrefabComponents(WorkContext context)
        {
            if (context.PrefabStage?.prefabContentsRoot == null)
                return Array.Empty<IOrientationListener>();

            return context.PrefabStage.prefabContentsRoot
                .GetComponentsInChildren<IOrientationListener>(_includeInactive);
        }

        private IOrientationListener[] GetSceneComponents()
        {
            var obj = FindObjectsByType<MonoBehaviour>(_includeInactive ? FindObjectsInactive.Include : FindObjectsInactive.Exclude, FindObjectsSortMode.None);
            
            return obj.OfType<IOrientationListener>().ToArray();
        }

        private void ApplyOrientation(BasicScreenOrientation orientation)
        {
            var context = GetCurrentContext();
            var components = GetComponents(context);
            string contextName = context.IsPrefabMode ? "prefab" : "scene";

            if (components.Length == 0)
            {
                EditorUtility.DisplayDialog("No Components", 
                    $"No OrientationAware components found in the current {contextName}!", 
                    "OK");
                return;
            }
        
            Undo.RecordObjects(components.Select(c => (c as MonoBehaviour)?.transform ).ToArray<Object>(), 
                $"Apply {orientation} Orientation");

            foreach (var component in components)
            {
                var mono = component as MonoBehaviour;
                try
                {
                    if(orientation == BasicScreenOrientation.Portrait)
                        component.ApplyPortraitData();
                    else
                        component.ApplyLandscapeData();
                    
                    EditorUtility.SetDirty(mono);
                }
                catch (Exception e)
                {
                    Debug.LogError($"Error applying orientation to {GetFullPath(mono?.transform)}: {e.Message}");
                }
            }

            if (context.IsPrefabMode)
            {
                EditorUtility.SetDirty(context.PrefabStage.prefabContentsRoot);
            }

            Debug.Log($"Applied {orientation} to {components.Length} components in {contextName}");
        
            _cachedComponents = null;
            Repaint();
        }

        private void SavePrefabChanges()
        {
            var prefabStage = PrefabStageUtility.GetCurrentPrefabStage();
            if (prefabStage != null)
            {
                EditorSceneManager.MarkSceneDirty(prefabStage.scene);
                AssetDatabase.SaveAssets();
                Debug.Log($"Saved prefab: {prefabStage.prefabContentsRoot.name}");
            }
        }

        private string GetPrefabPath(Transform transform)
        {
            var path = transform.name;
            var parent = transform.parent;
        
            while (parent != null)
            {
                path = parent.name + "/" + path;
                parent = parent.parent;
            }
        
            return path;
        }

        private string GetFullPath(Transform transform)
        {
            var context = GetCurrentContext();
            if (context.IsPrefabMode)
            {
                return GetPrefabPath(transform);
            }
            else
            {
                return transform.name; // В сцене можно использовать просто имя
            }
        }

        private Vector2 GetGameViewSize()
        {
            var asm = typeof(UnityEditor.Editor).Assembly;

            var gameViewType = asm.GetType("UnityEditor.GameView");
            if (gameViewType == null)
                return new Vector2(1920, 1080);

            // Находим открытое окно GameView НЕ создавая его
            var gameView = Resources.FindObjectsOfTypeAll(gameViewType)
                .FirstOrDefault() as EditorWindow;

            // Если GameView не открыт — возвращаем разумное значение
            if (gameView == null)
                return new Vector2(1920, 1080);

            return new Vector2(gameView.position.width, gameView.position.height);
        }
    
        private void OnFocus()
        {
            _cachedComponents = null;
        }

        private void OnHierarchyChange()
        {
            _cachedComponents = null;
            Repaint();
        }
    }


    public struct WorkContext
    {
        public bool IsPrefabMode;
        public PrefabStage PrefabStage;
        public string PrefabName;
        public string SceneName;
    }
}