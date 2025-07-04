using UnityEngine;
using UnityEngine.UI;

namespace CKT
{
    public class Image_Artifact : MonoBehaviour
    {
        Image _img_Artifact;

        private void Start()
        {
            _img_Artifact = GetComponent<Image>();
            //YSJ.Managers.UI.OnUpdateImage_ArtifactActionT1.SingleRegister((sprite) => UpdateImage_Artifact(sprite));

            UpdateImage_Artifact(null);
        }

        void UpdateImage_Artifact(Sprite sprite)
        {
            _img_Artifact = _img_Artifact ?? GetComponent<Image>();
            _img_Artifact.sprite = sprite;

            Color color = _img_Artifact.color;
            color.a = (sprite == null) ? 0 : 1;
            _img_Artifact.color = color;

        }
    }
}