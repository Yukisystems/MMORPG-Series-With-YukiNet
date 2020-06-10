using Assets.Scripts;
using Microsoft.Extensions.DependencyInjection;
using System;
using System.Collections;
using System.Collections.Generic;
using System.Linq;
using UMA;
using UMA.CharacterSystem;
using UnityEngine;
using UnityEngine.UI;
using static UMA.UMAData;

public class CharacterSelection : MonoBehaviour
{


    [Header("CharacterButtons")]
    public GameObject buttonPrefab;
    public GameObject panelToAttachButtonsTo;
   

    [Header("UMA_STUFF")]
    public DynamicCharacterAvatar characterAvatar;
    private Dictionary<string, DnaSetter> DNA;

    // Start is called before the first frame update
    private readonly ClientManager ClientManager;  
    public CharacterSelection()
    {
        ClientManager = GameContext.ServicesProvider.GetRequiredService<ClientManager>();
    }

    void Start()
    {
       
        ClientManager.CharacterSelection();
         
        var Characters = ClientManager.CharacterSelection().Characters;
        foreach(var chars in Characters)
        {
            //    CharButton.text = chars.CharName;

            var charname = chars.CharName;
            var umdata = chars.UmaData;

            GameObject button = (GameObject)Instantiate(buttonPrefab);
            button.transform.SetParent(panelToAttachButtonsTo.transform);//Setting button parent
            button.GetComponent<Button>().onClick.AddListener(() =>
            {
            //    //  characterAvatar.LoadDNAFromRecipeString(chars.UmaData);

            //    //   characterAvatar.LoadFromRecipeString(chars.UmaData.ToString());
            //    characterAvatar.LoadColorsFromRecipeString(umdata);
            //    //  characterAvatar.ImportSettings(UMATextRecipe.PackedLoadDCS(characterAvatar.context, umdata));
             characterAvatar.LoadFromRecipeString(umdata, DynamicCharacterAvatar.LoadOptions.useDefaults);
            characterAvatar.BuildCharacter();


        });//Setting what button does when clicked
                                                                       //Next line assumes button has child with text as first gameobject like button created from GameObject->UI->Button
            button.transform.GetChild(0).GetComponent<Text>().text = charname;//Changing text
        }
        characterAvatar.CharacterUpdated.AddListener(OnCharacterUpdated);
        characterAvatar.CharacterCreated.AddListener(OnCharacterCreated);
        // ToDO CharButtons  per Script anlegen.. 
        // ToDO UMA Dynamic CharacterAvatar Laden
        // TODO WorldServer Button






        //    var canvas = GameObject.FindObjectsOfType(typeof(ScrollView)).Cast<Canvas>().FirstOrDefault((c) => c.name == "CharacterList");
        //foreach (string str in CharacterList)
        //{
        //    //GameObject go = Instantiate(Button_Template) as GameObject;
        //    //go.SetActive(true);
        //    //Tutorial_Button TB = go.GetComponent<Tutorial_Button>();
        //    //TB.SetName(str);
        //    //go.transform.SetParent(Button_Template.transform.parent);
        //}
        ////GameObject canvas = GameObject.Find("CharacterSelection");
        //charbtn.text = packgeData1.CharName;
        //var characterButton = GameObject.Find("CharacterButton");
        //characterButton.SetActive(true);
        //characterButton.GetComponent<Text>().text = packgeData1.CharName;
        //   var characterButton = GameObject.Find("CharacterButton");
        // GameObject ScrollView = GameObject.FindObjectOfType<ScrollView>()
    }
   
    private void OnCharacterCreated(UMAData data)
    {
        DNA = characterAvatar.GetDNA();
    }

    private void OnCharacterUpdated(UMAData data)
    {
        DNA = characterAvatar.GetDNA();
    }

    //private void OnClick()
    //{
    //    //characterAvatar.LoadFromRecipeString(umadata);
    //}







    // Update is called once per frame
    private static void Update()
    {
        
    }

}