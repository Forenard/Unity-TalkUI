using System.Collections;
using System.Collections.Generic;
using TMPro;
using UnityEditor.UIElements;
using UnityEngine;
using UnityEngine.Events;
namespace TalkUI
{
    public class TextUIManager : MonoBehaviour
    {
        [SerializeField]
        private RectTransform touchInteractionRect;
        [SerializeField]
        private TextMeshProUGUI bodyText;
        [SerializeField]
        private TextMeshProUGUI nameText;
        [SerializeField]
        private float showSpeed = 4f;

        [HideInInspector]
        public Coroutine runningShowBodyCoroutine;

        private bool isInteractionActive
        {
            get
            {
                return Input.GetMouseButton(0) && touchInteractionRect.rect.Contains(Input.mousePosition);
            }
        }

        public void SetText(string name, string body)
        {
            nameText.text = name;
            bodyText.text = body;
            bodyText.maxVisibleCharacters = 0;
        }

        public void ShowBody(UnityEvent<int> callback)
        {
            if (runningShowBodyCoroutine != null) return;
            // init
            bodyText.maxVisibleCharacters = 0;
            runningShowBodyCoroutine = StartCoroutine(IEShowBody(showSpeed, callback));
        }

        public void KillCoroutine()
        {
            //Debug.Log("Kill");
            StopCoroutine(this.runningShowBodyCoroutine);
            this.runningShowBodyCoroutine = null;
        }

        IEnumerator IEShowBody(float showSpeed, UnityEvent<int> callback)
        {
            int maxCharNum = bodyText.text.Length;
            int defaultMaxCharNum = 99999;
            float tmpShowSpeed = showSpeed;
            int charNum = 0;
            float timeElapsed = 0f;
            float speedMultiply = 2f;

            // showing
            while (true)
            {
                yield return null;
                // interaction
                if (isInteractionActive)
                {
                    // speedMultiply倍
                    tmpShowSpeed = showSpeed * speedMultiply;
                }

                timeElapsed += Time.deltaTime;
                charNum = Mathf.FloorToInt(maxCharNum * timeElapsed * tmpShowSpeed);

                // set max chars
                bodyText.maxVisibleCharacters = charNum;

                // showing end
                if (charNum >= maxCharNum)
                {
                    bodyText.maxVisibleCharacters = defaultMaxCharNum;
                    break;
                }
            }

            // wait interaction
            while (true)
            {
                yield return null;
                if (isInteractionActive)
                {
                    break;
                }
            }

            // end all
            KillCoroutine();
            callback.Invoke(0);
            yield break;
        }
    }
}