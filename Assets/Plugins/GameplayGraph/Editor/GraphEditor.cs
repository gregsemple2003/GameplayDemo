using UnityEditor;
using GameplayGraph;

namespace GameplayGraph.Editor
{
    [CustomEditor(typeof(GameplayGraph.Graph))]
    public class GraphEditor : UnityEditor.Editor
    {
        public override void OnInspectorGUI()
        {
            DrawDefaultInspector();
        }
    }
}
