using System;
using System.Collections.Generic;
using TalkUIGraphView.Nodes;
using UnityEditor.Experimental.GraphView;
using UnityEngine;

namespace TalkUIGraphView
{
    public class SampleSearchWindowProvider : ScriptableObject, ISearchWindowProvider
    {
        private TalkUIGraphView graphView;

        public void Initialize(TalkUIGraphView graphView)
        {
            this.graphView = graphView;
        }

        List<SearchTreeEntry> ISearchWindowProvider.CreateSearchTree(SearchWindowContext context)
        {
            var entries = new List<SearchTreeEntry>();
            entries.Add(new SearchTreeGroupEntry(new GUIContent("Create Node")));

            foreach (var assembly in AppDomain.CurrentDomain.GetAssemblies())
            {
                foreach (var type in assembly.GetTypes())
                {
                    if (type.IsClass && !type.IsAbstract && (type.IsSubclassOf(typeof(NodeBase)))
                        && type != typeof(RootNode))
                    {
                        entries.Add(new SearchTreeEntry(new GUIContent(type.Name)) { level = 1, userData = type });
                    }
                }
            }

            return entries;
        }

        bool ISearchWindowProvider.OnSelectEntry(SearchTreeEntry searchTreeEntry, SearchWindowContext context)
        {
            var type = searchTreeEntry.userData as System.Type;
            var node = Activator.CreateInstance(type) as NodeBase;
            graphView.AddElement(node);
            return true;
        }
    }
}
