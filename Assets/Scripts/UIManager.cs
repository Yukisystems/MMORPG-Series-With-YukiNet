using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.EventSystems;
using UnityEngine.UI;

public class UIManager : MonoBehaviour
{
    private EventSystem system;
    public Button LoginBtn;
    // Start is called before the first frame update
    void Start()
    {
        system = EventSystem.current;
        LoginBtn = GetComponent<Button>(); 

    }

   

    // Update is called once per frame
    void Update()
    {
        if (Input.GetKeyDown(KeyCode.Tab)
           && system.currentSelectedGameObject != null
           && system.currentSelectedGameObject.GetComponent<Selectable>() != null)
        {
            Selectable next = Input.GetKey(KeyCode.LeftShift) || Input.GetKey(KeyCode.RightShift) ?
            system.currentSelectedGameObject.GetComponent<Selectable>().FindSelectableOnUp() :
            system.currentSelectedGameObject.GetComponent<Selectable>().FindSelectableOnDown();

            if (next != null)
            {
                InputField inputfield = next.GetComponent<InputField>();
                if (inputfield != null)
                {
                    inputfield.OnPointerClick(new PointerEventData(system));
                }

                system.SetSelectedGameObject(next.gameObject);
            }

            //Here is the navigating back part:
            else
            {
                next = Selectable.allSelectables[0];
                system.SetSelectedGameObject(next.gameObject);
            }

        }
        //if (Input.GetKeyDown(KeyCode.Return))
        //{
        //    LoginBtn.onClick.Invoke();
            
        //}

    }
}
