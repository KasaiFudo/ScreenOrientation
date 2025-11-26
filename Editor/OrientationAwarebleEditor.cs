using System.Reflection;
using UnityEditor;
using UnityEngine;

namespace KasaiFudo.ScreenOrientation.Editor
{
    [CustomEditor(typeof(OrientationAwareble<>), true)]
    public class OrientationAwarebleEditor : UnityEditor.Editor
    {
        public override void OnInspectorGUI()
        {
            base.OnInspectorGUI();
        
            if (GUILayout.Button("Apply Portrait Data"))
                Call("ApplyPortraitData");

            if (GUILayout.Button("Apply Landscape Data"))
                Call("ApplyLandscapeData");

            if (GUILayout.Button("Rewrite Portrait Layout Data"))
                Call("RewritePortraitLayoutData");

            if (GUILayout.Button("Rewrite Landscape Layout Data"))
                Call("RewriteLandscapeLayoutData");
        }

        private void Call(string method)
        {
            var t = target.GetType();
            var m = t.GetMethod(method, BindingFlags.Instance | BindingFlags.Public | BindingFlags.NonPublic);
            m?.Invoke(target, null);
        }
    }

}