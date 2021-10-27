# Unity-TalkUI

![image](https://user-images.githubusercontent.com/64544361/139143542-7faa55fb-0b19-4edf-9008-b3b10fc1f57e.png)

![image](https://user-images.githubusercontent.com/64544361/139145208-e188728c-e6ee-445e-8f48-f6cb86450768.png)


A Node-Based Conversation Event Creation System Using Unity-GraphView.

## Features

- You can easily create conversation events by connecting nodes.
- Event information is stored in JSON format and is dynamically loaded by attaching it to a dedicated object.
- The only code you need to implement is `event.RunEvent()` and a callback function for the ID of the ending of the conversation event!

## Usage

- Download the .unitypackage file from Release and import it into your Unity project.
- Open the TalkUIGraphView tab and connect the nodes.

![image](https://user-images.githubusercontent.com/64544361/139144120-9221a15d-87dc-4454-ad8b-b47b9f01fe57.png)

- Create an EventManager object.

![image](https://user-images.githubusercontent.com/64544361/139143982-bea894fb-98bc-43e1-bed7-a3d8a8ec0de7.png)

- Attach the EventData file (JSON) you just saved to the EventManager component.

![image](https://user-images.githubusercontent.com/64544361/139144348-b9e6d52e-8e6a-4726-8f52-e2be2c4a34a0.png)

- The following code is the minimal code to start a conversation event
```csharp
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
    public void TestCallBack(int id)
    {
        Debug.Log("Ending id is " + id);
    }
}
```

- You can also refer to `TalkUI/Scenes/SampleScene`

## TODO

- Loading and re-editing saved EventData
- Adjusting the position of newly created nodes
