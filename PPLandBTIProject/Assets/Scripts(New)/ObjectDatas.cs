using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace ListObject
{
    [CreateAssetMenu(fileName = "ObjectAR", menuName = "ObjectAR/Add New Object")]
    public class ObjectDatas : ScriptableObject
    {

        #region Object_Information

        [Header("Object_Information")]
        public string qrCodeARTrackerName;
        public string objectARName;
        [TextArea(2, 10)]
        public string objectDescInd;
        [TextArea(2, 10)]
        public string objectDescEng;

        #endregion

        #region GameObject_Prefabs

        [Header("GameObject_Prefabs")]
        public GameObject prefabsObj;

        #endregion

        #region Narator_AudioSource

        [Header("Narator_AudioSource")]
        public AudioClip englishNarator;
        public AudioClip naratorIndonesia;

        #endregion

    }

}