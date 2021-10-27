using System.Collections;
using System.Collections.Generic;
using TalkUI.Nodes;
using UnityEditor.Experimental.GraphView;

namespace TalkUIGraphView.Nodes
{
    public class NodeBase : UnityEditor.Experimental.GraphView.Node
    {
        public Port inputPort;
        public List<Port> outputPorts = new List<Port>();
        public TalkUI.Nodes.Node tNode;
    }
}
