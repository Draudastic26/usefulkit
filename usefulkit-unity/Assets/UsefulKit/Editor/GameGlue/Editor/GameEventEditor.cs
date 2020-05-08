using UnityEngine;
using UnityEditor;

namespace GameGlue
{
    [CustomEditor(typeof(GameEvent), true)]
    public class GameEventEditor : Editor
    {
        public override void OnInspectorGUI()
        {
            base.OnInspectorGUI();

            GUI.enabled = Application.isPlaying;

            var _GE = target as GameEvent;

            if (GUILayout.Button("Raise"))
            {
                _GE.Raise();
            }
        }
    }
}