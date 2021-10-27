using System.Collections;
using System.Collections.Generic;
using TalkUI.Nodes;
using UnityEngine;


namespace TalkUI
{
    public class EventData : ScriptableObject
    {
        public List<Node> nodes;

        public void CopyNodes(List<Node> _nodes)
        {
            this.nodes = new List<Node>(_nodes);
        }
    }
}