using System;
using System.Collections;
using System.Runtime.InteropServices;
using UnityEngine;
using UnityEngine.Networking;
using VRM;

public class Sample : MonoBehaviour
{
    public GameObject VrmObject;
    public GameObject LoadVrmObject;
    public GameObject ShadowObject;
    [DllImport("__Internal")]
    private static extern void FileImporterCaptureClick();

    public void OnButtonClicked()
    {
        #if UNITY_EDITOR
            LoadFromFile();
            //if (LoadVrmObject!=null)
            //{
            //    DestroyImmediate(VrmObject);
            //}
        #elif UNITY_WEBGL
            FileImporterCaptureClick();
            //if (LoadVrmObject!=null)
            //{
            //    DestroyImmediate(VrmObject);
            //}
        #endif
    }

    public void FileSelected(string url)
    {
        StartCoroutine(LoadJson(url));
    }

    private IEnumerator LoadJson(string url)
    {
        using (UnityWebRequest webRequest = UnityWebRequest.Get(url))
        {
            yield return webRequest.SendWebRequest();

            if (webRequest.isNetworkError)
            {
                Debug.LogError("ネットワークエラー");
            }
            else
            {
                LoadVRMFromBytes(webRequest.downloadHandler.data);
            }
        }
    }

    public void LoadVRMFromBytes(Byte[] bytes)
    {
        var context = new VRMImporterContext();
        try {
            context.ParseGlb(bytes);
            var meta = context.ReadMeta(true);
            context.Load();

            var model = context.Root;
            model.gameObject.name = meta.Title;
            context.ShowMeshes();
            LoadVrmObject = context.Root;
            LoadVrmObject.transform.parent = ShadowObject.transform;
            LoadVrmObject.transform.localPosition = Vector3.zero;
            LoadVrmObject.transform.localRotation = Quaternion.identity;
            Animator ShadowObjectAnimator = ShadowObject.GetComponent<Animator>();
            Animator VrmObjectAnimator = LoadVrmObject.GetComponent<Animator>();
            ShadowObjectAnimator.avatar = VrmObjectAnimator.avatar;
        }
        catch (Exception e) {
            Debug.LogError(e);
        }
    }
    void LoadFromFile()
    {
        //VRMファイルのパスを指定します
        //var path = Application.dataPath + "/Aguro/VrmModel/VroidMan.vrm";
        //var path = Application.dataPath + "/Aguro/VrmModel/AliciaSolid.vrm";
        //var path = Application.dataPath + "/Aguro/VrmModel/UnityChan(pants).vrm";
        var path = Application.dataPath + "/Aguro/VrmModel/VroidMan.vrm";

        //ファイルをByte配列に読み込みます
        var bytes = System.IO.File.ReadAllBytes(path);

        var context = new VRMImporterContext();

        // GLB形式でJSONを取得しParseします
        context.ParseGlb(bytes);

        context.Load();
        OnLoaded(context);
    }
    private void OnLoaded(VRMImporterContext context)
    {
        //メッシュを表示します
        context.ShowMeshes();
        //Destroy(VrmObject);
        LoadVrmObject = context.Root;
        LoadVrmObject.transform.parent = ShadowObject.transform;
        LoadVrmObject.transform.localPosition = Vector3.zero;
        LoadVrmObject.transform.localRotation = Quaternion.identity;
        Animator ShadowObjectAnimator = ShadowObject.GetComponent<Animator>();
        Animator VrmObjectAnimator = LoadVrmObject.GetComponent<Animator>();
        ShadowObjectAnimator.avatar = VrmObjectAnimator.avatar;
    }
}
