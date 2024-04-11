using UnityEngine;
using UnityEngine.UI;

public class HintController : MonoBehaviour
{
    public Text hintText;

    public void ShowHint()
    {
        hintText.text = "Move the blocks and reach destination";
    }
}
