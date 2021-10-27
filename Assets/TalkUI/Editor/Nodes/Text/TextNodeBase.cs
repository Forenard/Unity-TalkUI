using System.Collections;
using System.Collections.Generic;
using System.Linq;
using UnityEditor.Experimental.GraphView;
using UnityEngine.UIElements;
using TNode = TalkUI.Nodes;

namespace TalkUIGraphView.Nodes
{
    public abstract class TextNodeBase : NodeBase
    {
        private TextField charaIDTextField;
        public int charaID { get { return int.Parse(charaIDTextField.text); } }
        private TextField bodyTextField;
        public string text { get { return bodyTextField.text; } }

        public TextNodeBase()
        {
            this.tNode = new TNode.TextNode();
            SetTitle();
            SetInputPort();
            SetOutputPort();
            SetTextField();
        }
        public void SetTitle()
        {
            title = "Text";
        }

        public void SetInputPort()
        {
            inputPort = Port.Create<Edge>(Orientation.Horizontal, Direction.Input, Port.Capacity.Multi, typeof(Port));
            inputPort.portName = "In";
            inputContainer.Add(inputPort);
        }

        public void SetOutputPort()
        {
            var tmpPort = Port.Create<Edge>(Orientation.Horizontal, Direction.Output, Port.Capacity.Single, typeof(Port));
            tmpPort.portName = "Out";
            outputPorts.Add(tmpPort);
            outputContainer.Add(tmpPort);
        }

        public void SetTextField()
        {
            // add Chara ID explanation
            //mainContainer.Add(new Label("Chara ID"));
            // add Chara ID textfield
            charaIDTextField = new TextField("Chara ID");
            mainContainer.Add(charaIDTextField);

            // add Body explanation
            mainContainer.Add(new Label("Body"));
            // add Body textfield
            bodyTextField = new TextField();
            bodyTextField.multiline = true;
            mainContainer.Add(bodyTextField);
        }
    }
}