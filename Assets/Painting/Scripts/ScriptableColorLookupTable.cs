using UnityEngine;

namespace Tannern.Painting
{
    [CreateAssetMenu(fileName = "ColorLookupTable", menuName = "Scriptable Objects/Color Lookup Table")]
    public class ScriptableColorLookupTable : ScriptableObject
    {
        [System.Serializable]
        public class Item
        {
            public string name;
            public Color color;
        }

        public Item[] items;
    
        public Color? Lookup(string name)
        {
            foreach (Item item in items)
                if (item.name == name)
                    return item.color;
            return null;
        }
    }
}
