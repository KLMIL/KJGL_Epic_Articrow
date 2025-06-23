using UnityEngine;
using UnityEngine.UI;

namespace CKT
{
    public class Image_Artifact : MonoBehaviour
    {
        Image _img_Artifact;

        private void Start()
        {
            YSJ.Managers.UI.OnUpdateImage_ArtifactActionT1.Register((sprite) => UpdateImage_Artifact(sprite));
        }

        void UpdateImage_Artifact(Sprite sprite)
        {
            _img_Artifact = _img_Artifact ?? GetComponent<Image>();
            _img_Artifact.sprite = sprite;
        }
    }
}