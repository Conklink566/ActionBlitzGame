using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using Game.Interface;

namespace Game.UI
{
    public class StartGameScreenButton : MonoBehaviour
    {
        /// <summary>
        /// On click
        /// </summary>
        public void OnClick()
        {
            Manager.Instance.StartGame = true;
        }
    }
}