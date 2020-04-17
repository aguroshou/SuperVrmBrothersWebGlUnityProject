using System;
using System.Collections;
using System.Runtime.InteropServices;
using SFB;
using UnityEngine;
using VRM;

//
// Get SFB from https://github.com/gkngkc/UnityStandaloneFileBrowser
//

public class VrmRuntimeLoader : MonoBehaviour
{
    public GameObject ShadowObject;

#if UNITY_WEBGL && !UNITY_EDITOR
    //
    // WebGL
    //
    [DllImport("__Internal")]
    private static extern void UploadFile(string gameObjectName, string methodName, string filter, bool multiple);

    public void DoLoadButton() {
        Debug.Log("DoLoadButtonWebGL-WebGL");

        UploadFile(gameObject.name, "OnFileUpload", ".vrm", false);
    }

    // Called from browser
    public void OnFileUpload(string url) {
        StartCoroutine(OutputRoutine(url));
    }
#else

    public void DoLoadButton()
    {
        var paths = StandaloneFileBrowser.OpenFilePanel("Title", "", "vrm", false);
        if (paths.Length > 0)
        {
            StartCoroutine(OutputRoutine(new Uri(paths[0]).AbsoluteUri));
        }
    }
#endif

    private IEnumerator OutputRoutine(string url)
    {
        var loader = new WWW(url);
        yield return loader;
        LoadVrm(loader.bytes);
    }


    //static GameObject LoadVrm(Byte[] bytes)
    GameObject LoadVrm(Byte[] bytes)
    {
        var context = new VRMImporterContext();
        // GLB形式でJSONを取得しParseします
        context.ParseGlb(bytes);

        try
        {
            // ParseしたJSONをシーンオブジェクトに変換していく
            context.Load();

            // バウンディングボックスとカメラの位置関係で見切れるのを防止する
            context.EnableUpdateWhenOffscreen();

            // T-Poseのモデルを表示したくない場合、ShowMeshesする前に準備する
            // ロード後に表示する
            context.ShowMeshes();

            Transform PastVrm = ShadowObject.transform.Find("VRM");
            Destroy(PastVrm.gameObject);
            context.Root.transform.parent = ShadowObject.transform;
            context.Root.transform.localPosition = Vector3.zero;
            context.Root.transform.localRotation = Quaternion.identity;
            Animator ShadowObjectAnimator = ShadowObject.GetComponent<Animator>();
            Animator VrmObjectAnimator = context.Root.GetComponent<Animator>();
            ShadowObjectAnimator.avatar = VrmObjectAnimator.avatar;
            return context.Root;
        }
        catch (Exception ex)
        {
            Debug.LogError(ex);
            // 関連するリソースを破棄する
            context.Dispose();
            throw;
        }
    }
}