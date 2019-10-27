using Newtonsoft.Json;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Events;
using UnityEngine.Networking;

namespace Tannern.Painting
{
    public class FetchPalette : MonoBehaviour
    {
        [System.Serializable]
        public class PaletteLoadedEvent : UnityEvent<Color[]> { }
        public string paletteUri = "https://codetestvrh.herokuapp.com/view/test";
        public string authHeader = "steve";
        public PaletteLoadedEvent OnPaletteLoaded;
        public bool loadOnAwake = true;
        public ScriptableColorLookupTable colorLookupTable;

        public void Awake()
        {
            if (loadOnAwake)
                Load(paletteUri);
        }

        public void Load() => Load(paletteUri);
        public void Load(string uri) =>  StartCoroutine(DoLoad(uri));

        public IEnumerator DoLoad(string uri)
        {
            UnityWebRequest request = UnityWebRequest.Get(uri);
            request.SetRequestHeader("Authorization", authHeader);
            yield return request.SendWebRequest();
            yield return new WaitForEndOfFrame();
            if (request.isNetworkError || request.isHttpError)
            {
                Debug.LogError("Network error:" + request.error);
            }
            else
            {
                string rawValue = request.downloadHandler.text;
                Debug.Log("Parsing: " + rawValue);
                Color[] parsed = Parse(rawValue);
                OnPaletteLoaded.Invoke(parsed);
            }
        }

        public Color[] Parse(string source)
        {
            List<Color> colors = new List<Color>();
            Dictionary<string, string> data = JsonConvert.DeserializeObject<Dictionary<string, string>>(source);
            string colorName;
            foreach (string key in data.Keys)
            {
                colorName = data[key];
                Color? color = colorLookupTable.Lookup(colorName);
                if (color.HasValue)
                {
                    Debug.Log("adding Color: " + colorName);
                    colors.Add(color.Value);
                }
                else
                {
                    Debug.LogWarning("Color not found: " + colorName);
                }
            }
            return colors.ToArray();
        }
    }
}
