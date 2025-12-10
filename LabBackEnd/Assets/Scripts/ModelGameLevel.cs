using UnityEngine;
using System.Collections;
using System.Collections.Generic;

namespace ModelGameLevel
{
    [System.Serializable]
    public class ResponseAPI
    {
        public bool isSuccess;
        public string notification;
        public List<GameLevel> data;
    }

    [System.Serializable]
    public class GameLevel
    {
        public int levelId;
        public string title;
        public string description;
    }
}
