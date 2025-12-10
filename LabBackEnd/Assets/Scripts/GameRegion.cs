using Newtonsoft.Json;
using NUnit.Framework;
using System.Collections;
using System.Collections.Generic;
using TMPro;
using UnityEngine;
using UnityEngine.Networking;

public class GameRegion : MonoBehaviour
{
    public TMP_Dropdown DropdownRegion;
    private List<Region.RegionData> regions;
    public static int selectedRegionId;

    private IEnumerator GetRegion()
    {
        using (UnityWebRequest www = UnityWebRequest.Get("https://localhost:7041/api/APIGame/GetAllRegion"))
        {
            yield return www.SendWebRequest();

            if (www.isNetworkError || www.isHttpError)
            {
                Debug.Log(www.error);
            }
            else
            {
                string json = www.downloadHandler.text;
                Region.Response response = JsonConvert.DeserializeObject<Region.Response>(json);

                DropdownRegion.options.Clear();
                regions = new List<Region.RegionData>(response.data);
                foreach (Region.RegionData region in response.data)
                {
                    DropdownRegion.options.Add(new TMP_Dropdown.OptionData(region.name));
                }

                if (DropdownRegion.options.Count > 0)
                {
                    DropdownRegion.SetValueWithoutNotify(0);
                    DropdownRegion.RefreshShownValue();
                    DropdownValueChanged(DropdownRegion);
                }
            }
        }
    }
    private void DropdownValueChanged(TMP_Dropdown dropdown)
    {
        int index = dropdown.value;
        if (index < 0 || index >= regions.Count)
        {
            Debug.LogWarning("Invalid dropdown index selected.");
            return;
        }

        selectedRegionId = regions[index].regionId;
        Debug.Log("Selected Region ID: " + selectedRegionId);
    }
    void Start()
    {
        StartCoroutine(GetRegion());
        DropdownRegion.onValueChanged.AddListener(delegate { DropdownValueChanged(DropdownRegion); });
    }
}
