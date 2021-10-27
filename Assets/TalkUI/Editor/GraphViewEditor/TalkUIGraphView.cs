using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using TalkUI;
using TalkUI.Nodes;
using TalkUIGraphView.Nodes;
using Unity.Plastic.Antlr3.Runtime.Misc;
using UnityEditor;
using UnityEditor.Experimental.GraphView;
using UnityEngine;
using UnityEngine.UIElements;
using GNode = TalkUIGraphView.Nodes;
using TNode = TalkUI.Nodes;

namespace TalkUIGraphView
{
    public class TalkUIGraphView : GraphView
    {
        public GNode.RootNode root;
        public TextField savePathText;
        public TextField saveNameText;
        public TalkUIGraphView() : base()
        {
            // 親のUIにしたがって拡大縮小を行う設定
            style.flexGrow = 1;
            style.flexShrink = 1;

            // ズーム倍率の上限設定
            SetupZoom(ContentZoomer.DefaultMinScale, ContentZoomer.DefaultMaxScale);

            // 背景の設定
            var grid = new GridBackground();
            Insert(0, grid);
            grid.StretchToParentSize();

            // UIElements上でのドラッグ操作などの検知
            this.AddManipulator(new ContentDragger());
            this.AddManipulator(new SelectionDragger());
            this.AddManipulator(new RectangleSelector());

            var searchWindowProvider = new SampleSearchWindowProvider();
            searchWindowProvider.Initialize(this);

            nodeCreationRequest += context =>
            {
                SearchWindow.Open(new SearchWindowContext(context.screenMousePosition), searchWindowProvider);
            };

            // rootnode を始めに作成
            root = new GNode.RootNode();
            AddElement(root);
        }
        public override List<Port> GetCompatiblePorts(Port startAnchor, NodeAdapter nodeAdapter)
        {
            var compatiblePorts = new List<Port>();
            foreach (var port in ports.ToList())
            {
                if (startAnchor.node == port.node ||
                    startAnchor.direction == port.direction ||
                    startAnchor.portType != port.portType)
                {
                    continue;
                }

                compatiblePorts.Add(port);
            }
            return compatiblePorts;
        }
        public void Save()
        {
            Edge rootEdge = root.OutputPort.connections.FirstOrDefault();
            if (rootEdge == null) return;

            Dictionary<GNode.NodeBase, TNode.Node> nodeDict = new Dictionary<GNode.NodeBase, TNode.Node>();
            Queue<GNode.NodeBase> gNodeQue = new Queue<GNode.NodeBase>();
            TNode.Node tRootNode = root.tNode;
            GNode.NodeBase nextGNode = (GNode.NodeBase)rootEdge.input.node;

            // Trootnode の情報
            ((TNode.RootNode)tRootNode).charaNames = ((GNode.RootNode)root).texts;
            nodeDict.Add(root, tRootNode);
            tRootNode.nextNodes.Add(nextGNode.tNode);

            // 次nodeをqueueに追加
            gNodeQue.Enqueue(nextGNode);
            nodeDict.Add(nextGNode, nextGNode.tNode);

            while (gNodeQue.Count > 0)
            {
                GNode.NodeBase currentGNode = gNodeQue.Dequeue();
                TNode.Node currentTNode = currentGNode.tNode;

                // Tnode の情報
                switch (currentTNode.nodeType)
                {
                    case NodeType.text:
                        ((TNode.TextNode)currentTNode).charaID = ((GNode.TextNodeBase)currentGNode).charaID;
                        ((TNode.TextNode)currentTNode).bodyTextStr = ((GNode.TextNodeBase)currentGNode).text;
                        break;
                    case NodeType.button:
                        ((TNode.ButtonNode)currentTNode).buttonTextStrs = ((GNode.ButtonNodeBase)currentGNode).texts;
                        break;
                    case NodeType.end:
                        ((TNode.EndNode)currentTNode).endingID = ((GNode.EndNode)currentGNode).endingID;
                        break;
                }

                // 次nodeをqueueに追加
                foreach (Port _port in currentGNode.outputPorts)
                {
                    Edge _edge = _port.connections.FirstOrDefault();
                    if (_edge == null) continue;

                    GNode.NodeBase _gNode = (GNode.NodeBase)_edge.input.node;
                    currentTNode.nextNodes.Add(_gNode.tNode);
                    if (nodeDict.ContainsKey(_gNode))
                    {
                        continue;
                    }
                    else
                    {
                        nodeDict.Add(_gNode, _gNode.tNode);
                        gNodeQue.Enqueue(_gNode);
                    }
                }
            }

            CreateEventData(nodeDict.Values.ToList<TNode.Node>());
        }
        public void CreateEventData(List<TNode.Node> tNodes)
        {
            // Debug
            TNode.NodeUtility.LogNodes(tNodes);

            TNode.NodeUtility.SaveJson(savePathText.value, saveNameText.value, tNodes);
        }

    }
}