using System.Collections;
using System.Collections.Generic;
using TalkUI;
using UnityEngine;
using UnityEngine.Events;
public class TalkUISampleScript : MonoBehaviour
{
    [SerializeField]
    EventManager eventManager;
    private void Start()
    {
        UnityEvent<int> uEventCallBack = new UnityEvent<int>();
        uEventCallBack.AddListener(TestCallBack);
        eventManager.RunEvent(uEventCallBack);
    }

    //test callback
    public void TestCallBack(int id)
    {
        Debug.Log("Ending id is " + id);
    }
}
