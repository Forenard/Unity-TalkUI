using System.Collections;
using System.Collections.Generic;
using System.Linq;
using UnityEditor.Experimental.GraphView;
using UnityEngine;
using UnityEngine.UIElements;
using TNode = TalkUI.Nodes;

namespace TalkUIGraphView.Nodes
{
    public abstract class ButtonNodeBase : NodeBase
    {
        private List<TextField> buttonTextFields = new List<TextField>();
        public List<string> texts { get { return buttonTextFields.Select(x => x.text).ToList(); } }
        public int buttonNum = 1;

        public ButtonNodeBase()
        {
            this.tNode = new TNode.ButtonNode();
            SetButtonNum();
            SetTitle();
            SetInputPort();
            SetOutputPort(buttonNum);
            SetTextField(buttonNum);
        }

        abstract public void SetButtonNum();

        public void SetTitle()
        {
            title = "Button" + buttonNum.ToString();
        }

        public void SetInputPort()
        {
            inputPort = Port.Create<Edge>(Orientation.Horizontal, Direction.Input, Port.Capacity.Multi, typeof(Port));
            inputPort.portName = "In";
            inputContainer.Add(inputPort);
        }

        public void SetOutputPort(int buttonNum)
        {
            for (int i = 0; i < buttonNum; i++)
            {
                var tmpPort = Port.Create<Edge>(Orientation.Horizontal, Direction.Output, Port.Capacity.Single, typeof(Port));
                tmpPort.portName = (i + 1).ToString();
                outputPorts.Add(tmpPort);
                outputContainer.Add(tmpPort);
            }
        }

        public void SetTextField(int buttonNum)
        {
            for (int i = 0; i < buttonNum; i++)
            {
                // add explanation
                mainContainer.Add(new Label((i + 1).ToString()));
                // add textfield
                var tmpTextField = new TextField();
                tmpTextField.multiline = true;
                buttonTextFields.Add(tmpTextField);
                mainContainer.Add(tmpTextField);
            }
        }
    }
}
