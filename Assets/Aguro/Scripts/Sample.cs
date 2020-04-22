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

    //public void Update()
    //{
    //    if (LoadVrmObject!=null&& VrmObject != null)
    //    {
    //        Destroy(VrmObject);
    //    }
    //}
    void Update()
    {
        if (Input.GetKeyDown(KeyCode.Z))
        {
            if (VrmObject != null)
            {
                VrmObject.SetActive(false);
            }
        }
        if (Input.GetKeyDown(KeyCode.X))
        {
            if (VrmObject != null)
            {
                Destroy(VrmObject);
            }
        }
        if (Input.GetKeyDown(KeyCode.C))
        {
            if (VrmObject != null)
            {
                VrmObject.transform.position = Vector3.zero;
            }
        }
    }
    public void OnButtonClicked()
    {
        #if UNITY_EDITOR
            Debug.Log("WebGLビルドで試してください");
        #elif UNITY_WEBGL
            if(VrmObject!=null)
            {
                VrmObject.SetActive (false);
            }
            if(LoadVrmObject!=null)
            {
                Destroy(LoadVrmObject);
            }
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
}
