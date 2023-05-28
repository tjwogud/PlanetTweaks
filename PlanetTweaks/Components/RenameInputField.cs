using DG.Tweening;
using UnityEngine;
using UnityEngine.Events;
using UnityEngine.UI;

namespace PlanetTweaks.Components
{
    public class RenameInputField : MonoBehaviour
    {
        public static RenameInputField Instance { get; private set; }

        public Canvas Canvas { get; private set; }
        public CanvasGroup CanvasGroup { get; private set; }
        public Text Desc { get; private set; }
        public InputField InputField { get; private set; }
        public Button BackgroundButton { get; private set; }
        public Image Background { get; private set; }

        private UnityAction<string> onHide;

        private void Awake()
        {
            if (Instance)
            {
                DestroyImmediate(this);
                return;
            }

            Instance = this;
            Canvas = GetComponentInChildren<Canvas>();
            CanvasGroup = GetComponentInChildren<CanvasGroup>();
            Desc = Canvas.GetComponentInChildren<Text>();
            InputField = Canvas.GetComponentInChildren<InputField>();
            BackgroundButton = Canvas.GetComponentInChildren<Button>();
            Background = BackgroundButton.GetComponent<Image>();

            CanvasGroup.alpha = 0;
            CanvasGroup.interactable = false;
            CanvasGroup.blocksRaycasts = false;
            Desc.text = RDString.language == SystemLanguage.Korean ? "이름 변경" : "Rename";
            ((Text)InputField.placeholder).text = RDString.language == SystemLanguage.Korean ? "이름을 입력하세요..." : "Enter name...";
            InputField.onEndEdit.AddListener(s => Hide());
            BackgroundButton.onClick.AddListener(Hide);
        }

        public void Show(string text, UnityAction<string> onHide)
        {
            CanvasGroup.interactable = true;
            CanvasGroup.blocksRaycasts = true;
            this.DOKill(false);
            DOTween.To(() => CanvasGroup.alpha, a => CanvasGroup.alpha = a, 1, 0.5f).SetTarget(this);
            this.onHide?.Invoke(InputField.text);
            InputField.text = text;
            this.onHide = onHide;
        }

        public void Hide()
        {
            CanvasGroup.interactable = false;
            CanvasGroup.blocksRaycasts = false;
            this.DOKill(false);
            DOTween.To(() => CanvasGroup.alpha, a => CanvasGroup.alpha = a, 0, 0.5f).SetTarget(this);
            onHide?.Invoke(InputField.text);
            onHide = null;
        }
    }
}
