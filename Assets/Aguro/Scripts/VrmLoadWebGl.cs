using System;
using System.Collections;
using System.Runtime.InteropServices;
using UnityEngine;
using UnityEngine.Networking;
using VRM;

public class VrmLoadWebGl : MonoBehaviour
{
    public GameObject ShadowObject;
    public GameObject VrmObject;

    [DllImport("__Internal")]
    private static extern void FileImporterCaptureClick();

    void Update()
    {
        //if (VrmObject.name.Contains("NextVrm") && Input.GetKeyDown(KeyCode.Z))
        if (Input.GetKeyDown(KeyCode.Z))
        {
            //Transform PastVrm = ShadowObject.transform.Find("VRM");
            //Destroy(PastVrm.gameObject);

            VrmObject.name = "VRM";
            VrmObject.transform.parent = ShadowObject.transform;
            VrmObject.transform.localPosition = Vector3.zero;
            VrmObject.transform.localRotation = Quaternion.identity;
            Animator ShadowObjectAnimator = ShadowObject.GetComponent<Animator>();
            Animator VrmObjectAnimator = VrmObject.GetComponent<Animator>();
            ShadowObjectAnimator.avatar = VrmObjectAnimator.avatar;

        }
        if (Input.GetKeyDown(KeyCode.X))
        {
            //VrmObject = GameObject.Find("NextVrm");
            VrmObject.name = "VRM";
            VrmObject.transform.parent = ShadowObject.transform;
            VrmObject.transform.localPosition = Vector3.zero;
            VrmObject.transform.localRotation = Quaternion.identity;
            Animator ShadowObjectAnimator = ShadowObject.GetComponent<Animator>();
            Animator VrmObjectAnimator = VrmObject.GetComponent<Animator>();
            ShadowObjectAnimator.avatar = VrmObjectAnimator.avatar;
            Debug.Log("VrmObject.transform.parent.name=" + VrmObject.transform.parent.name);
            Debug.Log("ShadowObject.name=" + ShadowObject.name);

        }

        if (Input.GetKeyDown(KeyCode.C))
        {
            //VrmObject = GameObject.Find("NextVrm");
            VrmObject.name = "VRM";
            VrmObject.transform.parent = ShadowObject.transform;
            VrmObject.transform.localPosition = Vector3.zero;
            VrmObject.transform.localRotation = Quaternion.identity;
            Animator ShadowObjectAnimator = ShadowObject.GetComponent<Animator>();
            Animator VrmObjectAnimator = VrmObject.GetComponent<Animator>();
            ShadowObjectAnimator.avatar = VrmObjectAnimator.avatar;
            Debug.Log("VrmObject.transform.parent.name=" + VrmObject.transform.parent.name);
            Debug.Log("ShadowObject.name=" + ShadowObject.name);

        }
        if (Input.GetKeyDown(KeyCode.V))
        {
            VrmObject.transform.SetParent(ShadowObject.transform);

        }
    }

    public void OnButtonClicked()
    {
#if UNITY_EDITOR
        //Debug.Log("WebGLビルドで試してください");
        Destroy(VrmObject);
        LoadFromFile();
#elif UNITY_WEBGL
            Destroy(VrmObject);
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
        try
        {
            context.ParseGlb(bytes);
            var meta = context.ReadMeta(true);
            context.Load();
            //var model = context.Root;
            //model.gameObject.name = meta.Title;
            //model.gameObject.name = "VRM";
            //context.ShowMeshes();

            //VrmObject = context.Root;
            context.ShowMeshes();
            VrmObject = context.Root;
            //context.Root.name = "NextVrm";
            //context.Root.transform.SetParent(transform, false);
            //VrmObject = context.Root;
            //Debug.Log("context.Root=" + context.Root);
            //Debug.Log("VrmObject.name=" + VrmObject.name);

            //VrmObject = Instantiate(context.Root, new Vector3(0.0f, 0.0f, 0.0f), Quaternion.identity);
            //VrmObject.transform.parent = ShadowObject.transform;
            //VrmObject.transform.localPosition = Vector3.zero;
            //VrmObject.transform.localRotation = Quaternion.identity;
            //Animator ShadowObjectAnimator = ShadowObject.GetComponent<Animator>();
            //Animator VrmObjectAnimator = VrmObject.GetComponent<Animator>();
            //ShadowObjectAnimator.avatar = VrmObjectAnimator.avatar;
        }
        catch (Exception e)
        {
            Debug.LogError(e);
        }
    }


    void LoadFromFile()
    {
        //VRMファイルのパスを指定します
        //var path = Application.dataPath + "/Aguro/VrmModel/VroidMan.vrm";
        //var path = Application.dataPath + "/Aguro/VrmModel/AliciaSolid.vrm";
        var path = Application.dataPath + "/Aguro/VrmModel/UnityChan(pants).vrm";

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
        //context.Root.name = "NextVrm";
        //Transform PastVrm = ShadowObject.transform.Find("VRM");
        //Destroy(PastVrm.gameObject);
        //context.Root.name = "VRM";
        //context.Root.transform.parent = ShadowObject.transform;
        //context.Root.transform.localPosition = Vector3.zero;
        //context.Root.transform.localRotation = Quaternion.identity;
        //Animator ShadowObjectAnimator = ShadowObject.GetComponent<Animator>();
        //Animator VrmObjectAnimator = context.Root.GetComponent<Animator>();
        //ShadowObjectAnimator.avatar = VrmObjectAnimator.avatar;
        //Debug.Log(VrmObjectAnimator.avatar);

        //メッシュを表示します
        context.ShowMeshes();
        VrmObject = context.Root;
        //VrmObject = Instantiate(context.Root, new Vector3(0.0f, 0.0f, 0.0f), Quaternion.identity);
        //VrmObject.transform.parent = ShadowObject.transform;
        //VrmObject.transform.localPosition = Vector3.zero;
        //VrmObject.transform.localRotation = Quaternion.identity;
        //Animator ShadowObjectAnimator = ShadowObject.GetComponent<Animator>();
        //Animator VrmObjectAnimator = VrmObject.GetComponent<Animator>();
        //ShadowObjectAnimator.avatar = VrmObjectAnimator.avatar;

    }
}
