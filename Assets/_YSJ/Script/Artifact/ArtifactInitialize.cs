using UnityEngine;

public class ArtifactInitialize : MonoBehaviour
{
    private void OnEnable()
    {
        if (transform.root.TryGetComponent<MagicHand>(out MagicHand hand)) 
        {
            if (transform.parent.name == "LeftHand")
            {
                hand.leftFirePosition = transform.GetChild(0);
            }
            else 
            {
                hand.rightFirePosition = transform.GetChild(0);
            }
        }
    }
}
