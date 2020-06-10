using Assets.Scripts;
using Microsoft.Extensions.DependencyInjection;
using System.Collections.Generic;
using UMA;
using UMA.CharacterSystem;
using UnityEngine;
using UnityEngine.UI;
using YukiNet.Client;

public class CharacterCreator : MonoBehaviour
{
    private readonly ClientConnectionDispatcher connectionDispatcher;
    private readonly ClientManager ClientManager;

    private DynamicCharacterAvatar characterAvatar;
    private Dictionary<string, DnaSetter> DNA;

    public List<string> hairMaleModels = new List<string>();
    private int CurrentMaleHair;

    public List<string> hairFemaleModels = new List<string>();
    private int CurrentFemaleHair;

    [SerializeField]
    private Slider HeightSlider, MuscleSlider, WeightSlider;

    //[SerializeField]
    //private UMAWardrobeRecipe[] MaleHair, FemaleHair;
    [Header("Charcreation")]
    public InputField CharacterNameInput;
    [SerializeField]
    //private Color[] SkinColors;
    public CharacterCreator()
    {
        ClientManager = GameContext.ServicesProvider.GetRequiredService<ClientManager>();
    }
    private void Start()
    {
        characterAvatar = GetComponent<DynamicCharacterAvatar>();
        characterAvatar.CharacterUpdated.AddListener(OnCharacterUpdated);
        characterAvatar.CharacterCreated.AddListener(OnCharacterCreated);
        HeightSlider.onValueChanged.AddListener(OnHeightChange);
        WeightSlider.onValueChanged.AddListener(OnWeightChange);
        MuscleSlider.onValueChanged.AddListener(OnMuscleChange);
    }

    private void OnCharacterCreated(UMAData data)
    {
        DNA = characterAvatar.GetDNA();
    }

    private void OnCharacterUpdated(UMAData data)
    {
        DNA = characterAvatar.GetDNA();
    }

    private void OnHeightChange(float height)
    {
        DNA["height"].Set(height);
        characterAvatar.BuildCharacter();
    }

    private void OnMuscleChange(float muscle)
    {
        DNA["lowerMuscle"].Set(muscle);
        DNA["upperMuscle"].Set(muscle);
        characterAvatar.BuildCharacter();
    }

    private void OnWeightChange(float weight)
    {
        DNA["lowerWeight"].Set(weight);
        DNA["upperWeight"].Set(weight);
        characterAvatar.BuildCharacter();
    }

    //public void ChangeHair(int hair)
    //{
    //    if (characterAvatar.activeRace.name == "HumanMaleDCS")
    //    {
    //        characterAvatar.SetSlot(MaleHair[hair]);
    //    }
    //    if (characterAvatar.activeRace.name == "HumanFemaleDCS")
    //    {
    //        characterAvatar.SetSlot(FemaleHair[hair]);
    //    }
    //    characterAvatar.BuildCharacter();
    //}
    public void ChangeHair(bool plus)
    {
        if (characterAvatar.activeRace.name == "HumanMaleDCS")
        {
            if (plus)
            {
                CurrentMaleHair++;
            }
            else
            {
                CurrentMaleHair--;
            }
            CurrentMaleHair = Mathf.Clamp(CurrentMaleHair, 0, hairMaleModels.Count - 1);
            if (hairMaleModels[CurrentMaleHair] == "None")
            {
                characterAvatar.ClearSlot("Hair");
            }
            else
            {
                characterAvatar.SetSlot("Hair", hairMaleModels[CurrentMaleHair]);
            }
        }
        if (characterAvatar.activeRace.name == "HumanFemnetaleDCS")
        {
            //Female Hair
            if (plus)
            {
                CurrentFemaleHair++;
            }
            else
            {
                CurrentFemaleHair--;
            }
            CurrentFemaleHair = Mathf.Clamp(CurrentFemaleHair, 0, hairFemaleModels.Count - 1);

            if (hairFemaleModels[CurrentFemaleHair] == "None")
            {
                characterAvatar.ClearSlot("Hair");
            }
            else
            {
                characterAvatar.SetSlot("Hair", hairFemaleModels[CurrentFemaleHair]);
            }
        }
        characterAvatar.BuildCharacter();
    }

    private void OnDisable()
    {
        characterAvatar.CharacterUpdated.RemoveListener(OnCharacterUpdated);
        characterAvatar.CharacterCreated.RemoveListener(OnCharacterCreated);
        HeightSlider.onValueChanged.RemoveListener(OnHeightChange);
        WeightSlider.onValueChanged.RemoveListener(OnWeightChange);
        MuscleSlider.onValueChanged.RemoveListener(OnMuscleChange);
    }

    public void ChangeSkinColor(Color SkinColor)
    {
        characterAvatar.SetColor("Skin", SkinColor);
        characterAvatar.UpdateColors(true);
        Debug.Log(characterAvatar.GetColor("Skin").color);
    }

    public void SwitchGender(bool male)
    {
        if (male && characterAvatar.activeRace.name != "HumanMaleDCS")
        {
            characterAvatar.ChangeRace("HumanMaleDCS");
        }
        if (!male && characterAvatar.activeRace.name != "HumanFemaleDCS")
        {
            characterAvatar.ChangeRace("HumanFemaleDCS");
        }
    }

    public void SaveCharacters()
    {
        string UmaRecipeData = characterAvatar.GetCurrentRecipe(true); //PlayerPrefs.GetString("CharacterData");
        string CharacterName = CharacterNameInput.text;
        ClientManager.SaveCharacter(UmaRecipeData, CharacterName);

    }
}