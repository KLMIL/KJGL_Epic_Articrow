using UnityEngine;

namespace BMC
{
    public class TutorialTextArea : MonoBehaviour
    {
        [SerializeField] bool _isActived = false;
        [SerializeField] int _id = -1;

        void OnTriggerEnter2D(Collider2D collision)
        {
            if (_isActived || _id == -1)
            {
                return;
            }

            if (collision.CompareTag("Player"))
            {
                if (!_isActived)
                {
                    _isActived = true;
                    UI_TutorialEventBus.OnTutorialText?.Invoke(_id);
                }
            }
        }
    }
}