﻿using TMPro;
using UnityEngine;
using Utility;

namespace World.Player.GUI
{
    [RequireComponent(typeof(CanvasRenderer))]
    public class GUIMessageController : PopupBehaviour
    {
        public TMP_Text title;
        public TMP_Text message;

        public void OnToggle(object s, GenEventArgs<(string, string)> e)
        {
            OpenWindow();
            title.text = e.Value.Item1;
            message.text = e.Value.Item2;
        }
    }
}