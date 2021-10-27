using System.Collections;
using System.Collections.Generic;
using TMPro;
using UnityEngine;
using UnityEngine.Events;
using UnityEngine.UI;
namespace TalkUI
{
    public class ButtonUIManager : MonoBehaviour
    {
        [SerializeField]
        private List<GameObject> buttonObjs = new List<GameObject>();

        private List<List<TextMeshProUGUI>> buttonTexts = new List<List<TextMeshProUGUI>>();
        private Animator animator;
        private string animatorBool = "Active";
        private UnityEvent<int> buttonCallBack;
        private void Start()
        {
            animator = this.GetComponent<Animator>();
            foreach (var buttonObj in buttonObjs)
            {
                List<TextMeshProUGUI> texts = new List<TextMeshProUGUI>();
                for (int i = 0; i < buttonObj.transform.childCount; i++)
                {
                    texts.Add(buttonObj.transform.GetChild(i).GetChild(0).GetComponent<TextMeshProUGUI>());
                }
                buttonTexts.Add(texts);
            }
        }
        public void ButonPushedCallBack(int id)
        {
            animator.SetBool(animatorBool, false);
            foreach (var buttonObj in buttonObjs)
            {
                buttonObj.SetActive(false);
            }
            buttonCallBack.Invoke(id - 1);
        }

        public void ShowButton(int buttonID, List<string> buttonTextStrs, UnityEvent<int> callBack)
        {
            this.buttonCallBack = callBack;
            for (int i = 0; i < buttonID; i++)
            {
                buttonTexts[buttonID - 1][i].text = buttonTextStrs[i];
            }

            for (int i = 0; i < buttonObjs.Count; i++)
            {
                if (i == buttonID - 1)
                {
                    buttonObjs[i].SetActive(true);
                }
                else
                {
                    buttonObjs[i].SetActive(false);
                }
            }
            animator.SetBool(animatorBool, true);
        }

    }
}