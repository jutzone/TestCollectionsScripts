using UnityEngine;
using UnityEngine.UI;

namespace TestApp
{
    public class CollectionsController : MonoBehaviour
    {
        #region Variables
        [SerializeField] private CollectionsData currentCollection;
        [SerializeField] private CollectionsConfig configuration;
        [SerializeField] private Element element;
        [SerializeField] private GameObject collectionPanel, elementPanel, descriptionPanel, favouritesPanel, elementPref;
        [SerializeField] private Button[] collectionsButtons;
        [SerializeField] private Button backButton, favouriteButton;
        [SerializeField] private Text elementDescription;
        [SerializeField] private Image elementImage;
        [SerializeField] private string currentScreen;
        #endregion

        private void Awake()
        {
            #region Signals
            collectionsButtons[0].onClick.AddListener(() => SetCollection(collectionsButtons[0].name));
            collectionsButtons[1].onClick.AddListener(() => SetCollection(collectionsButtons[1].name));
            collectionsButtons[2].onClick.AddListener(() => SetCollection(collectionsButtons[2].name));
            collectionsButtons[3].onClick.AddListener(() => SetCollection(collectionsButtons[3].name));
            collectionsButtons[4].onClick.AddListener(() => SetCollection(collectionsButtons[4].name));
            backButton.onClick.AddListener(BackToPreviousScreen);
            favouriteButton.onClick.AddListener(OpenFavouriteList);
            #endregion
        }


        private void SetCollection(string name)
        {
            for (int i = 0; i < elementPanel.GetComponentInChildren<ScrollRect>().content.transform.childCount; i++)
            {
                Destroy(elementPanel.GetComponentInChildren<ScrollRect>().content.transform.GetChild(i).gameObject);
            }

            currentScreen = "Elements";
            CheckScreenButtons();

            for (int i = 0; i < configuration.Collections.Count; i++)
            {
                if (name == configuration.Collections[i].CollectionName)
                    currentCollection = configuration.Collections[i];
            }

            collectionPanel.gameObject.SetActive(false);
            elementPanel.gameObject.SetActive(true);
            descriptionPanel.gameObject.SetActive(false);
            favouritesPanel.gameObject.SetActive(false);

            for (int i = 0; i < currentCollection.collectionElements.Count; i++)
            {
                elementPref.GetComponent<ElementController>().InitializeElement(
                    currentCollection.collectionElements[i],
                    PlayerPrefs.GetInt(currentCollection.collectionElements[i].ElementName));
                GameObject GO = Instantiate(elementPref, elementPanel.GetComponentInChildren<ScrollRect>().content.transform);
                GO.gameObject.GetComponent<ElementController>().ElementAdded.AddListener(AddToFavourite);
                GO.gameObject.GetComponent<ElementController>().ElementRemoved.AddListener(RemoveFromFavourite);
                GO.gameObject.GetComponent<ElementController>().ElementDescription.AddListener(ShowDescription);
            }
        }

        private void AddToFavourite(Element el)
        {
            PlayerPrefs.SetInt(el.ElementName, 1);
        }

        private void RemoveFromFavourite(Element el)
        {
            PlayerPrefs.SetInt(el.ElementName, 0);
        }

        private void OnDestroyElement(GameObject obj)
        {
            Destroy(obj);
        }

        public void CheckScreenButtons()
        {
            if (currentScreen == "Collections")
            {
                backButton.gameObject.SetActive(false);
                favouriteButton.gameObject.SetActive(true);
            }

            else
            {
                backButton.gameObject.SetActive(true);
                favouriteButton.gameObject.SetActive(false);
            }
        }

        private void BackToPreviousScreen()
        {

            switch (currentScreen)
            {
                case "Elements":
                    collectionPanel.gameObject.SetActive(true);
                    elementPanel.gameObject.SetActive(false);
                    descriptionPanel.gameObject.SetActive(false);
                    favouritesPanel.gameObject.SetActive(false);
                    currentScreen = "Collections";
                    CheckScreenButtons();
                    break;

                case "Description":
                    collectionPanel.gameObject.SetActive(false);
                    elementPanel.gameObject.SetActive(true);
                    descriptionPanel.gameObject.SetActive(false);
                    favouritesPanel.gameObject.SetActive(false);
                    currentScreen = "Elements";
                    CheckScreenButtons();
                    break;

                case "Favourites":
                    collectionPanel.gameObject.SetActive(true);
                    elementPanel.gameObject.SetActive(false);
                    descriptionPanel.gameObject.SetActive(false);
                    favouritesPanel.gameObject.SetActive(false);
                    currentScreen = "Collections";
                    CheckScreenButtons();
                    break;

            }
        }

        private void OpenFavouriteList()
        {
            configuration.Favourites.Clear();
            for (int i = 0; i < favouritesPanel.GetComponentInChildren<ScrollRect>().content.transform.childCount; i++)
            {
                Destroy(favouritesPanel.GetComponentInChildren<ScrollRect>().content.transform.GetChild(i).gameObject);
            }
            for (int i = 0; i < configuration.Collections.Count; i++)
            {
                for (int k = 0; k < configuration.Collections[i].collectionElements.Count; k++)
                {
                    if (PlayerPrefs.GetInt(configuration.Collections[i].collectionElements[k].ElementName) > 0)
                        configuration.Favourites.Add(configuration.Collections[i].collectionElements[k]);
                }
            }

            collectionPanel.gameObject.SetActive(false);
            elementPanel.gameObject.SetActive(false);
            descriptionPanel.gameObject.SetActive(false);
            favouritesPanel.gameObject.SetActive(true);
            currentScreen = "Favourites";
            CheckScreenButtons();

            for (int i = 0; i < configuration.Favourites.Count; i++)
            {
                elementPref.GetComponent<ElementController>().InitializeElement(
                    configuration.Favourites[i],
                    PlayerPrefs.GetInt(configuration.Favourites[i].ElementName));
                GameObject GO = Instantiate(elementPref, favouritesPanel.GetComponentInChildren<ScrollRect>().content.transform);
                GO.gameObject.GetComponent<ElementController>().ElementRemoved.AddListener(RemoveFromFavourite);
                GO.gameObject.GetComponent<ElementController>().DestroyElement.AddListener(OnDestroyElement);
                GO.gameObject.GetComponent<ElementController>().ElementDescription.AddListener(ShowDescription);
            }
        }

        private void ShowDescription(Element el)
        {
            collectionPanel.gameObject.SetActive(false);
            elementPanel.gameObject.SetActive(false);
            descriptionPanel.gameObject.SetActive(true);
            favouritesPanel.gameObject.SetActive(false);

            elementDescription.text = el.ElementDescription;
            elementImage.sprite = el.ElementImage;
        }
    }
}
