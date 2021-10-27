using System.Collections;
using System.Collections.Generic;
using System.Linq;
using UnityEditor.Experimental.GraphView;
using UnityEngine.UIElements;
using TNode = TalkUI.Nodes;

namespace TalkUIGraphView.Nodes
{
    public class EndNode : NodeBase
    {
        private TextField endingIDTextField;
        public int endingID { get { return int.Parse(endingIDTextField.text); } }

        public EndNode()
        {
            this.tNode = new TNode.EndNode();
            SetTitle();
            SetInputPort();
            SetTextField();
        }
        public void SetTitle()
        {
            title = "End";
        }

        public void SetInputPort()
        {
            inputPort = Port.Create<Edge>(Orientation.Horizontal, Direction.Input, Port.Capacity.Multi, typeof(Port));
            inputPort.portName = "In";
            inputContainer.Add(inputPort);
        }

        public void SetTextField()
        {
            endingIDTextField = new TextField("Ending ID");
            mainContainer.Add(endingIDTextField);
        }
    }
}