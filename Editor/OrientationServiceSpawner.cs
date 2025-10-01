using UnityEditor;
using UnityEngine;

namespace KasaiFudo.ScreenOrientation.Editor
{
    public static class OrientationServiceSpawner
    {
        [MenuItem("GameObject/Screen Orientation/Add update service", false, 10)]
        public static void AddManager(MenuCommand menuCommand)
        {
            var prefab = AssetDatabase.LoadAssetAtPath<GameObject>(
                "Packages/com.kasaifudo.screenorientation/Runtime/Prefabs/OrientationUpdateService.prefab"
            );

            if (prefab != null)
            {
                var instance = Object.Instantiate(prefab);
                GameObjectUtility.SetParentAndAlign(instance, menuCommand.context as GameObject);
                Undo.RegisterCreatedObjectUndo(instance, "Create " + instance.name);
                Selection.activeObject = instance;
            }
            else
            {
                Debug.LogError("ScreenOrientationSystem prefab not found!");
            }
        }
    }
}
