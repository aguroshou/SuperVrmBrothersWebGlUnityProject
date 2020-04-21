using System;
using System.Collections;
using System.Runtime.InteropServices;
using UnityEngine;
using UnityEngine.Networking;
using VRM;

public class Sample : MonoBehaviour
{
    public GameObject MoveTestObject;
    public GameObject ShadowObject;
    [DllImport("__Internal")]
    private static extern void FileImporterCaptureClick();

    public void Update()
    {
        if (MoveTestObject!=null&& Input.GetKeyDown(KeyCode.N))
        {
            MoveTestObject.transform.Rotate(5.0f, 5.0f, 5.0f);
        }
        if (Input.GetKeyDown(KeyCode.B))
        {
            //Transform PastVrm = ShadowObject.transform.Find("VRM");
            //Destroy(PastVrm.gameObject);

            //MoveTestObject.name = "VRM";
            MoveTestObject.transform.parent = ShadowObject.transform;
            MoveTestObject.transform.localPosition = Vector3.zero;
            MoveTestObject.transform.localRotation = Quaternion.identity;
            Animator ShadowObjectAnimator = ShadowObject.GetComponent<Animator>();
            Animator VrmObjectAnimator = MoveTestObject.GetComponent<Animator>();
            ShadowObjectAnimator.avatar = VrmObjectAnimator.avatar;

        }
    }

    public void OnButtonClicked()
    {
        #if UNITY_EDITOR
            Debug.Log("WebGLビルドで試してください");
        #elif UNITY_WEBGL
            FileImporterCaptureClick();
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

            MoveTestObject = context.Root;
            MoveTestObject.transform.parent = ShadowObject.transform;
            MoveTestObject.transform.localPosition = Vector3.zero;
            MoveTestObject.transform.localRotation = Quaternion.identity;
            Animator ShadowObjectAnimator = ShadowObject.GetComponent<Animator>();
            Animator VrmObjectAnimator = MoveTestObject.GetComponent<Animator>();
            ShadowObjectAnimator.avatar = VrmObjectAnimator.avatar;
        }
        catch (Exception e) {
            Debug.LogError(e);
        }
    }
}
