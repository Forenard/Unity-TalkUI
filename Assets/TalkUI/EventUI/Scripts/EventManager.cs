using System.Collections;
using System.Collections.Generic;
using System.Linq;
using TalkUI.Nodes;
using TMPro;
using UnityEngine;
using UnityEngine.Events;

namespace TalkUI
{
    public class EventManager : MonoBehaviour
    {
        [SerializeField]
        private TextAsset eventData;
        [SerializeField]
        private GameObject eventUIObject;
        [SerializeField]
        private List<GameObject> charaObjects;
        [SerializeField]
        private TextUIManager textUIManager;
        [SerializeField]
        private ButtonUIManager buttonUIManager;

        // datas
        [HideInInspector]
        private EventSystem eventSystem;

        public void RunEvent(UnityEvent<int> EventCallBack)
        {
            CreateEventSystem(EventCallBack);
            eventSystem.EventStart();
        }


        [ContextMenu("Debug")]
        public void CreateEventSystem(UnityEvent<int> EventCallBack)
        {
            List<CharaUI> charaUIs = charaObjects.Select(x => new CharaUI(x)).ToList();
            EventUI eventUI = new EventUI(eventUIObject, charaUIs, textUIManager, buttonUIManager);
            List<JsonNode> jsonNodes = NodeUtility.LoadJsonFromTxt(eventData);

            eventSystem = new EventSystem(jsonNodes, eventUI, EventCallBack);

            // Debug
            NodeUtility.LogJsonNodes(eventSystem.nodes);
        }

        public void EventStartCallBack() => eventSystem.RunNodeCallBack(0);


        public class EventSystem
        {
            public List<JsonNode> nodes;
            public JsonNode rootNode;
            public int currentNodeID;
            public EventUI eventUI;
            public List<string> charaNames;
            UnityEvent<int> EventCallBack;
            public EventSystem(List<JsonNode> jsonNodes, EventUI eventUI, UnityEvent<int> EventCallBack)
            {
                this.nodes = jsonNodes;
                this.eventUI = eventUI;
                this.EventCallBack = EventCallBack;
                // search root
                foreach (var _node in jsonNodes)
                {
                    if (_node.nodeType == NodeType.root)
                    {
                        SetGeneralData(_node);
                        break;
                    }
                }
            }
            public void EventStart()
            {
                RunCurrentNode();
            }
            public void EventEnd()
            {
                eventUI.EventEnd();
                EventCallBack.Invoke(nodes[currentNodeID].endingID);
            }
            public void SetGeneralData(JsonNode _rootNode)
            {
                this.rootNode = _rootNode;
                this.currentNodeID = _rootNode.id;
                this.charaNames = _rootNode.charaNames;
            }
            public void RunCurrentNode()
            {
                JsonNode currentNode = nodes[currentNodeID];
                UnityEvent<int> unityEventCallBack = new UnityEvent<int>();
                unityEventCallBack.AddListener(RunNodeCallBack);

                switch (currentNode.nodeType)
                {
                    case NodeType.root:
                        eventUI.EventStart();
                        break;
                    case NodeType.text:
                        int charaID = currentNode.charaID;
                        string bodyText = currentNode.bodyTextStr;
                        eventUI.StartTextEvent(charaID, charaNames[charaID - 1], bodyText, unityEventCallBack);
                        break;
                    case NodeType.button:
                        List<string> buttonTexts = currentNode.buttonTextStrs;
                        eventUI.StartButtonEvent(buttonTexts.Count, buttonTexts, unityEventCallBack);
                        break;
                    case NodeType.end:
                        RunNodeCallBack(0);
                        break;
                }
            }

            // is successed
            public bool GoNextNode(int nextIDsInd)
            {
                if (nodes[currentNodeID].nextids.Count == 0)
                {
                    return false;
                }
                else
                {
                    this.currentNodeID = nodes[currentNodeID].nextids[nextIDsInd];
                    return true;
                }
            }

            public void RunNodeCallBack(int nextIDsInd)
            {
                bool isSuccessed = this.GoNextNode(nextIDsInd);

                if (isSuccessed) this.RunCurrentNode();
                else this.EventEnd();
            }
        }

        public class CharaUI
        {
            public Animator animator;
            private string activeBoolStr = "Active";
            public CharaUI(GameObject charaObject)
            {
                this.animator = charaObject.GetComponent<Animator>();
            }
            public void Active()
            {
                this.animator.SetBool(this.activeBoolStr, true);
            }
            public void NonActive()
            {
                this.animator.SetBool(this.activeBoolStr, false);
            }
        }

        public class EventUI
        {
            public Animator animator;
            public TextUIManager textUIManager;
            public ButtonUIManager buttonUIManager;
            public List<CharaUI> charaUIs;
            private string eventStartBoolStr = "EventStart";
            private string eventEndBoolStr = "EventEnd";
            public EventUI(GameObject uiObject, List<CharaUI> charaUIs, TextUIManager textUIManager, ButtonUIManager buttonUIManager)
            {
                this.animator = uiObject.GetComponent<Animator>();
                this.charaUIs = charaUIs;
                this.textUIManager = textUIManager;
                this.buttonUIManager = buttonUIManager;
            }
            public void EventStart()
            {
                this.animator.SetBool(this.eventStartBoolStr, true);
            }

            public void EventEnd()
            {
                this.animator.SetBool(this.eventEndBoolStr, true);
            }

            public void StartButtonEvent(int buttonID, List<string> buttonTexts, UnityEvent<int> callBack)
            {
                this.buttonUIManager.ShowButton(buttonID, buttonTexts, callBack);
            }
            public void StartTextEvent(int charaID, string charaText, string bodyText, UnityEvent<int> callBack)
            {
                for (int i = 0; i < charaUIs.Count; i++)
                {
                    if (i == charaID - 1) charaUIs[i].Active();
                    else charaUIs[i].NonActive();
                }
                this.textUIManager.SetText(charaText, bodyText);
                this.textUIManager.ShowBody(callBack);
            }
        }
    }
}