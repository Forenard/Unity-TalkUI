using System.Collections;
using UnityEditor;
using UnityEngine;

public class CreateMenu : MonoBehaviour
{
    [MenuItem("GameObject/TalkUI/EventManager-1Chara", false, 1)]
    static void CreateEventManager1Chara(MenuCommand command)
    {
        //　空のゲームオブジェクト作成
        GameObject pref = Resources.Load("Prefabs/EventManager-1Chara", typeof(GameObject)) as GameObject;
        GameObject obj = Instantiate(pref) as GameObject;
        obj.name = "EventManager-1Chara";
        //　ゲームオブジェクトの親の設定
        GameObjectUtility.SetParentAndAlign(obj, command.context as GameObject);
        //　Undo操作を加える(Ctrl+Zキーの操作に加える）
        Undo.RegisterCreatedObjectUndo(obj, "Create " + obj.name);
        //　初期位置を設定
        //obj.transform.position = Vector3.zero;
        //　作成したゲームオブジェクトを選択状態にする
        Selection.activeObject = obj;
    }
    [MenuItem("GameObject/TalkUI/EventManager-2Chara", false, 2)]
    static void CreateEventManager2Chara(MenuCommand command)
    {
        //　空のゲームオブジェクト作成
        GameObject pref = Resources.Load("Prefabs/EventManager-2Chara", typeof(GameObject)) as GameObject;
        GameObject obj = Instantiate(pref) as GameObject;
        obj.name = "EventManager-2Chara";
        //　ゲームオブジェクトの親の設定
        GameObjectUtility.SetParentAndAlign(obj, command.context as GameObject);
        //　Undo操作を加える(Ctrl+Zキーの操作に加える）
        Undo.RegisterCreatedObjectUndo(obj, "Create " + obj.name);
        //　初期位置を設定
        //obj.transform.position = Vector3.zero;
        //　作成したゲームオブジェクトを選択状態にする
        Selection.activeObject = obj;
    }
    [MenuItem("GameObject/TalkUI/EventManager-3Chara", false, 3)]
    static void CreateEventManager3Chara(MenuCommand command)
    {
        //　空のゲームオブジェクト作成
        GameObject pref = Resources.Load("Prefabs/EventManager-3Chara", typeof(GameObject)) as GameObject;
        GameObject obj = Instantiate(pref) as GameObject;
        obj.name = "EventManager-3Chara";
        //　ゲームオブジェクトの親の設定
        GameObjectUtility.SetParentAndAlign(obj, command.context as GameObject);
        //　Undo操作を加える(Ctrl+Zキーの操作に加える）
        Undo.RegisterCreatedObjectUndo(obj, "Create " + obj.name);
        //　初期位置を設定
        //obj.transform.position = Vector3.zero;
        //　作成したゲームオブジェクトを選択状態にする
        Selection.activeObject = obj;
    }
}