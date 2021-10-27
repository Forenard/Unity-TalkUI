using System.Collections;
using System.Collections.Generic;
using System.Linq;
using UnityEditor.Experimental.GraphView;
using UnityEngine.UIElements;
using TNode = TalkUI.Nodes;

namespace TalkUIGraphView.Nodes
{
    public class RootNode : NodeBase
    {
        public Port OutputPort;
        public List<string> charaNames = new List<string>();


        private int maxCharaNum = 3;
        private List<TextField> textFields = new List<TextField>();
        public List<string> texts { get { return textFields.Select(x => x.text).ToList(); } }

        public RootNode() : base()
        {
            //RootNode
            this.tNode = new TNode.RootNode();

            title = "Root";

            capabilities -= Capabilities.Deletable;

            OutputPort = Port.Create<Edge>(Orientation.Horizontal, Direction.Output, Port.Capacity.Single, typeof(Port));
            OutputPort.portName = "Out";
            outputContainer.Add(OutputPort);


            // add explanation
            Label tmpLabel = new Label("CharaNames");
            mainContainer.Add(tmpLabel);
            for (int i = 0; i < maxCharaNum; i++)
            {
                // add explanation
                mainContainer.Add(new Label((i + 1).ToString()));
                // add textfield
                var tmpTextField = new TextField();
                textFields.Add(tmpTextField);
                mainContainer.Add(tmpTextField);
            }
        }
    }
}
