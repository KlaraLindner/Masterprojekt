﻿using TMPro;
using Utility;

namespace World.Player.GUI
{
    public class GUIErrorHandlingController : PopupBehaviour
    {
        public TMP_InputField content;

        public void PlaceError(string err)
        {
            content.text = err;
            gameObject.SetActive(true);
        }
    }
}