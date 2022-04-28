using UnityEngine;
using UnityEngine.Events;
using UnityEngine.UI;

namespace TestApp
{
    public class ElementController : MonoBehaviour
    {
        #region Variables
        [SerializeField] private Image elementImage;
        [SerializeField] private Image heartImage;
        [SerializeField] private Sprite[] heartSprite;
        [SerializeField] private Button heartButton;
        [SerializeField] private bool isFavourite;
        [SerializeField] private Text elementName;
        [TextArea]
        [SerializeField] private string elementDescription;
        [SerializeField] private Element thisElement;
        public class GOEvent : UnityEvent<GameObject> { };
        public GOEvent DestroyElement;

        public class ElEvent : UnityEvent<Element> { };
        public ElEvent ElementAdded;
        public ElEvent ElementRemoved;
        public ElEvent ElementDescription;
        #endregion
        private void Awake()
        {
            heartButton.onClick.AddListener(AddToFavourite);
            DestroyElement = new GOEvent();
            ElementAdded = new ElEvent();
            ElementRemoved = new ElEvent();
            ElementDescription = new ElEvent();
            gameObject.GetComponent<Button>().onClick.AddListener(ShowDescription);
        }

        public GameObject InitializeElement(Element el, int favourite)
        {
            thisElement = el;
            elementImage.sprite = thisElement.ElementImage;
            isFavourite = (favourite > 0) ? true : false;
            elementName.text = thisElement.ElementName;
            elementDescription = thisElement.ElementDescription;

            if (isFavourite)
                heartImage.sprite = heartSprite[1];
            else
                heartImage.sprite = heartSprite[0];
            return gameObject;
        }

        private void ShowDescription()
        {
            ElementDescription.Invoke(thisElement);
        }
        public void AddToFavourite()
        {
            isFavourite = !isFavourite;
            if (isFavourite)
            {
                heartImage.sprite = heartSprite[1];
                ElementAdded.Invoke(thisElement);
                thisElement.IsFavourite = true;
            }
            else
            {
                heartImage.sprite = heartSprite[0];
                ElementRemoved.Invoke(thisElement);
                DestroyElement.Invoke(this.gameObject);
            }
        }

    }
}
