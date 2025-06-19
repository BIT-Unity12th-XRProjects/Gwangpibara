using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.AddressableAssets;
using UnityEngine.ResourceManagement.AsyncOperations;

public class DataLoader : MonoBehaviour
{
    [SerializeField]
    private AssetReferenceT<GameObjectData> ARgameObjectData;
    private List<GameObjectData.GOData> goDatas = new List<GameObjectData.GOData>();

    void Start()
    {
        // StartCoroutine(InitAddressable());
    }

    // IEnumerator InitAddressable()
    // {
    //     var Init = Addressables.InitializeAsync();
    //     yield return Init;
    // }

    public void OnSpawnObjectClicked()
    {
        goDatas.Clear();
        // Addressables.LoadAssetAsync<GameObjectData>("GameObjectData")
        //     .Completed += OnDataLoaded;
            
        ARgameObjectData.LoadAssetAsync()
            .Completed += OnDataLoaded;
    }

    private void OnDataLoaded(AsyncOperationHandle<GameObjectData> handle)
    {
        if (handle.Status == AsyncOperationStatus.Succeeded)
        {
            var data = handle.Result;
            foreach (GameObjectData.GOData d in data.GODatas)
            {
                goDatas.Add(d);

                Debug.Log(d.id + "Position : " + d.position);
            }

        }
        else
        {
            Debug.LogError("로듯 ㅣㄹ패 ㅜ ");
        }
        ARgameObjectData.ReleaseAsset();
    }
    public void OnReleaseClicked()
    {
        if (goDatas.Count == 0) return;

        ARgameObjectData.ReleaseAsset();
    }
}
