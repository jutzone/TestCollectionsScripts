using System;
using System.Collections.Generic;
using UnityEngine;

namespace TestApp
{
    public class CollectionsConfig : MonoBehaviour
    {
        [Header("Collections")]
        public List<CollectionsData> Collections;
        [Header("FavouriteList")]
        public List<Element> Favourites;
    }

    [Serializable]
    public struct CollectionsData
    {
        public string CollectionName;
        public List<Element> collectionElements;
    }

    [Serializable]
    public struct Element
    {
        public Sprite ElementImage;
        public bool IsFavourite { get; set; }
        public string ElementName;
        [TextArea]
        public string ElementDescription;
    } 
}
