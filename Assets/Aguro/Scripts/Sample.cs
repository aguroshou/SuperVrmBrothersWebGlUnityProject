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
    public bool IsVrmLoaded;
    public long VrmObjectDestroyCount;
    [DllImport("__Internal")]
    private static extern void FileImporterCaptureClick();
<<<<<<< HEAD

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
=======
    private void Start()
    {
        IsVrmLoaded = false;
        VrmObjectDestroyCount = 0;
    }
    //public void Update()
    //{
    //    if (LoadVrmObject!=null&& VrmObject != null)
    //    {
    //        Destroy(VrmObject);
    //    }
    //}
    void Update()
    {
        //if (IsVrmLoaded == true|| LoadVrmObject != null || Input.GetKeyDown(KeyCode.Z))
        //{
        //    Destroy(VrmObject);
        //}

        //if ((VrmObject != null&&LoadVrmObject!=null)|| Input.GetKeyDown(KeyCode.Z))
        //if ((IsVrmLoaded == true && VrmObjectDestroyCount == 100) || Input.GetKeyDown(KeyCode.Z))
        //VrmObjectDestroyCount = (VrmObjectDestroyCount + 1) % 101;

        //if (Input.GetKeyDown(KeyCode.Z))
        //{
        //    if (VrmObject != null)
        //    {
        //        VrmObject.SetActive(false);
        //    }
        //}
        //if (Input.GetKeyDown(KeyCode.X))
        //{
        //    if (VrmObject != null)
        //    {
        //        Destroy(VrmObject);
        //    }
        //}
        //if (Input.GetKeyDown(KeyCode.C))
        //{
        //    if (VrmObject != null)
        //    {
        //        VrmObject.transform.position = Vector3.zero;
        //    }
        //}
    }
    public void OnButtonClickedDestroyVrmObject()
    {
        Destroy(VrmObject);
    }

    public void OnButtonClicked()
    {
#if UNITY_EDITOR
        Debug.Log("WebGLビルドで試してください");
#elif UNITY_WEBGL
            //if(VrmObject!=null)
            //{
            //    VrmObject.SetActive (false);
            //}
            if(LoadVrmObject!=null)
            {
                Destroy(LoadVrmObject);
            }
            FileImporterCaptureClick();
#endif
>>>>>>> origin/FromWebGlLoadVrmCommitBranch
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

            var model = context.Root;
            model.gameObject.name = meta.Title;
            context.ShowMeshes();
<<<<<<< HEAD
=======

>>>>>>> origin/FromWebGlLoadVrmCommitBranch
            LoadVrmObject = context.Root;
            LoadVrmObject.transform.parent = ShadowObject.transform;
            LoadVrmObject.transform.localPosition = Vector3.zero;
            LoadVrmObject.transform.localRotation = Quaternion.identity;
            Animator ShadowObjectAnimator = ShadowObject.GetComponent<Animator>();
            Animator VrmObjectAnimator = LoadVrmObject.GetComponent<Animator>();
            ShadowObjectAnimator.avatar = VrmObjectAnimator.avatar;
            IsVrmLoaded = true;
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
