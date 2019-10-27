using UnityEngine;

public class ApplyColor : MonoBehaviour
{
    public Renderer[] renderersToChange;
    public string colorTag = "ColorBlock";

    //On entering a trigger with the tag "ColorBlock", this will take the renderer attached to said trigger and apply it to any renderer in the array
    private void OnTriggerEnter(Collider collider)
    {
        if (collider.CompareTag(colorTag))
        {
            foreach(Renderer rend in renderersToChange)
            {
                rend.material.color = collider.GetComponent<Renderer>().material.color;
            }
        }
    }
}