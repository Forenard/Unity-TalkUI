using System;
using System.Collections;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using TMPro;
using UnityEditor;
using UnityEngine;

namespace TalkUI.Nodes
{
    public class Node
    {
        public int id;
        public NodeType nodeType;
        public List<Node> nextNodes = new List<Node>();
    }
    public class RootNode : Node
    {
        public RootNode()
        {
            this.nodeType = NodeType.root;
        }
        public List<string> charaNames;
    }
    public class TextNode : Node
    {
        public TextNode()
        {
            this.nodeType = NodeType.text;
        }
        public int charaID;
        public string bodyTextStr;
    }
    public class ButtonNode : Node
    {
        public ButtonNode()
        {
            this.nodeType = NodeType.button;
        }
        public List<string> buttonTextStrs;
    }
    public class EndNode : Node
    {
        public EndNode()
        {
            this.nodeType = NodeType.end;
        }
        public int endingID;
    }

    public enum NodeType
    {
        root = (int)0,
        text = (int)1,
        button = (int)2,
        end = (int)3
    }

    [System.Serializable]
    public class JsonNode
    {
        //root
        public List<string> charaNames = null;
        //text
        public int charaID;
        public string bodyTextStr;
        //button
        public List<string> buttonTextStrs;
        //end
        public int endingID;
        //共通
        public int id;
        public NodeType nodeType;
        public List<int> nextids = new List<int>();

        public JsonNode(Node node, List<int> nextids)
        {
            this.nodeType = node.nodeType;
            this.nextids = nextids;
            switch (node.nodeType)
            {
                case NodeType.root:
                    this.charaNames = (node as RootNode).charaNames;
                    break;
                case NodeType.text:
                    this.charaID = (node as TextNode).charaID;
                    this.bodyTextStr = (node as TextNode).bodyTextStr;
                    break;
                case NodeType.button:
                    this.buttonTextStrs = (node as ButtonNode).buttonTextStrs;
                    break;
                case NodeType.end:
                    this.endingID = (node as EndNode).endingID;
                    break;
            }
        }
    }


    public class NodeUtility
    {
        [System.Serializable]
        public class JsonWrapper
        {
            public List<JsonNode> jsonNodes;
            public JsonWrapper(List<JsonNode> jsonNodes)
            {
                this.jsonNodes = jsonNodes;
            }
        }

        public static JsonWrapper ToJsonWrapper(List<Node> nodes)
        {
            List<JsonNode> jsonNodes = new List<JsonNode>();

            //set id
            for (int i = 0; i < nodes.Count; i++)
            {
                nodes[i].id = i;
            }

            foreach (var node in nodes)
            {
                JsonNode jsonNode = new JsonNode(node, node.nextNodes.Select(x => x.id).ToList());
                jsonNode.id = jsonNodes.Count;
                jsonNodes.Add(jsonNode);
            }

            return new JsonWrapper(jsonNodes);
        }
        public static void SaveJson(string dataPath, string dataName, List<Node> data)
        {
            string filePath = dataPath + "/" + dataName + ".json";
            string json = JsonUtility.ToJson(ToJsonWrapper(data));
            Debug.Log("SaveJson to " + filePath);
            Debug.Log("Json :\n" + json);

            StreamWriter streamWriter = new StreamWriter(filePath);
            streamWriter.Write(json);
            streamWriter.Flush();
            streamWriter.Close();

            AssetDatabase.Refresh();
        }

        public static List<JsonNode> LoadJsonFromPath(string dataName)
        {
            string filePath = "Assets/Resources/" + dataName + ".json";
            Debug.Log("LoadJson from " + filePath);

            if (File.Exists(filePath))
            {
                StreamReader streamReader;
                streamReader = new StreamReader(filePath);
                string data = streamReader.ReadToEnd();
                streamReader.Close();

                return JsonUtility.FromJson<JsonWrapper>(data).jsonNodes;
            }
            else
            {
                return default(List<JsonNode>);
            }
        }

        public static List<JsonNode> LoadJsonFromTxt(TextAsset textAsset)
        {
            return JsonUtility.FromJson<JsonWrapper>(textAsset.text).jsonNodes;
        }
        public static void LogNodes(List<Node> nodes)
        {
            if (nodes == null)
            {
                Debug.Log("Is Nodes null");
                return;
            }
            Debug.Log("Node num : " + nodes.Count);

            int _i = 0;
            foreach (var node in nodes)
            {
                Debug.Log("[" + _i + "] Node type : " + node.nodeType);
                switch (node.nodeType)
                {
                    case NodeType.text:
                        Debug.Log(((TextNode)node).charaID + " says :\n[" + ((TextNode)node).bodyTextStr + "]");
                        break;
                    case NodeType.button:
                        foreach (var item in ((ButtonNode)node).buttonTextStrs)
                        {
                            Debug.Log("[" + _i + "] Button is :\n" + item);
                        }
                        break;
                }
                _i++;
            }
        }

        public static void LogJsonNodes(List<JsonNode> nodes)
        {
            if (nodes == null)
            {
                Debug.Log("Is Nodes null");
                return;
            }
            Debug.Log("Node num : " + nodes.Count);

            int _i = 0;
            foreach (var node in nodes)
            {
                Debug.Log("[" + _i + "] Node type : " + node.nodeType);
                switch (node.nodeType)
                {
                    case NodeType.text:
                        Debug.Log(node.charaID + " says :\n[" + node.bodyTextStr + "]");
                        break;
                    case NodeType.button:
                        foreach (var item in node.buttonTextStrs)
                        {
                            Debug.Log("[" + _i + "] Button is :\n" + item);
                        }
                        break;
                }
                _i++;
            }
        }
    }
}