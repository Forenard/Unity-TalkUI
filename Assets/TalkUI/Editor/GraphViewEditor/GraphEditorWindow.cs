using TalkUIGraphView.Nodes;
using UnityEditor;
using UnityEngine.UIElements;

namespace TalkUIGraphView
{
    public class GraphEditorWindow : EditorWindow
    {
        [MenuItem("Window/Open TalkUIGraphView")]
        public static void Open()
        {
            GetWindow<GraphEditorWindow>("TalkUIGraphView");
        }
        void OnEnable()
        {
            var graphView = new TalkUIGraphView()
            {
                style = { flexGrow = 1 }
            };
            rootVisualElement.Add(graphView);
            rootVisualElement.Add(new Button(graphView.Save) { text = "Save" });

            TextField savePathText = new TextField("Save Path");
            savePathText.value = "Assets/TalkUI/Resources";
            rootVisualElement.Add(savePathText);
            TextField saveNameText = new TextField("Save Name");
            saveNameText.value = "TestEventData";
            rootVisualElement.Add(saveNameText);

            graphView.saveNameText = saveNameText;
            graphView.savePathText = savePathText;
        }
    }
}
