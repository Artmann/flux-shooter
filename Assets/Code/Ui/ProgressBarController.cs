using UnityEngine;

namespace Code.Ui
{
    public class ProgressBarController : MonoBehaviour
    {
        [SerializeField] private RectTransform fillTransform;

        private float maxWidth;
        
        private void Start()
        {
            maxWidth = GetComponent<RectTransform>().rect.width;
        }

        public void SetProgress(float value)
        {
            var width = Mathf.Clamp(value * maxWidth, 0, maxWidth);
            
            fillTransform.sizeDelta = new Vector2(width, fillTransform.sizeDelta.y);
        }
    }
}