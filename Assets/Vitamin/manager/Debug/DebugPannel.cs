using UnityEngine;
using UnityEngine.UI;
using UnityEngine.EventSystems;
using System.Collections;
using System.Collections.Generic;
using System;

namespace vitamin { 
    public class DebugPannel : MonoBehaviour
    {
        public InputField input;
        public Text output;
        public Scrollbar scrollBar;

        public int maxHistory = 60;
        Queue<string> history;

        Action<string> __inputHandler;
        void Start()
        {
            input.text = "";
            output.text = "";
            history = new Queue<string>();
        }

        void Update()
        {
            if (Input.GetKeyDown(KeyCode.KeypadEnter)|| Input.GetKeyDown(KeyCode.Return))
            {
                addToQueue(string.Format("> {0}", input.text));
                if (__inputHandler != null)
                {
                    __inputHandler.Invoke(input.text);
                }
            }
        }

        public void onInput(Action<string> inputHandler)
        {
            __inputHandler = inputHandler;
        }

        public void addToQueue(string content)
        {
            history.Enqueue(content);
            if (history.Count >= maxHistory) history.Dequeue();
            output.text = string.Join("\n", history);
            StartCoroutine(updateScrollBar());
        }

        IEnumerator updateScrollBar()
        {
            yield return new WaitForSeconds(0.2f);
            scrollBar.value = 0;
            //Debug.Log("updateScrollBar");
        }
    }
}