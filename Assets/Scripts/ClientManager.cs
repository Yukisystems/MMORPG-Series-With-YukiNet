///////////////////////////////////////////////////////////////////////////////////////
//  _______ _            _______    _             _       _    _____ _           __  //
// |__   __| |          |__   __|  | |           (_)     | |  / ____| |         / _| //
//    | |  | |__   ___     | |_   _| |_ ___  _ __ _  __ _| | | |    | |__   ___| |_  //
//    | |  | '_ \ / _ \    | | | | | __/ _ \| '__| |/ _` | | | |    | '_ \ / _ \  _| //
//    | |  | | | |  __/    | | |_| | || (_) | |  | | (_| | | | |____| | | |  __/ |   //
//    |_|  |_| |_|\___|    |_|\__,_|\__\___/|_|  |_|\__,_|_|  \_____|_| |_|\___|_|   //
//                                                                                   //
//     The Tutorial Chef : https://youtube.com/c/TheTutorialChef                     //
//     Website:            https://thetutorialchef.com                               //
//     Twitter:            https://twitter.com/thetutorialchef                       //
//     Patreon:            https://www.patreon.com/thetutorialchef                   //
//     Discord:            https://discord.gg/kGrRQJ9                                //
//                                                                                   //
//     Company:            https://yukisystems.com                                   //
//                                                                                   //
//         Copyright by Deadlyviruz aka The Tutorialchef                             //
///////////////////////////////////////////////////////////////////////////////////////
using Assets.Scripts;
using Microsoft.Extensions.DependencyInjection;
using System.Collections.Generic;
using System.Linq;
using TMPro;
using UMA.CharacterSystem;
using UnityEngine;
using UnityEngine.SceneManagement;
using UnityEngine.UI;
using UnityEngine.UIElements;
using YukiNet;
using YukiNet.Client;
using YukiNet.PackageParser.PackageImplementaions;

public class ClientManager : MonoBehaviour
{
    public InputField usernameInput;
    public InputField passwordInput;
    private Scene CharacterCreation;

    public Text ErrorMsgText;
    
    
    private readonly NetworkConnection clientConn;

    private readonly ClientConnectionDispatcher connectionDispatcher;
    private readonly MenuManager menuManager;

    public static string Raceidtext { get; set; }

   //[Header("CharCreation")]
   // public DynamicCharacterAvatar characterAvatar;
   // public InputField CharacterNameInput;

    //[Header("CharSelection")]
    //public Button CharacterButton;
    //public Button CharacterButton2;

    // Use this for initialization
    private List<string> CharacterList = new List<string>();

    private void Start()
    {
        Debug.Log("EXECUTE START");

        clientConn.Connect("127.0.0.1", 3456);
        DontDestroyOnLoad(this);
    }

  
    public ClientManager()
    {
        // Log("CLIENT MANAGER CREATED");
        clientConn = GameContext.ServicesProvider.GetRequiredService<NetworkConnection>();
        connectionDispatcher = GameContext.ServicesProvider.GetRequiredService<ClientConnectionDispatcher>();
        menuManager = GameContext.ServicesProvider.GetRequiredService<MenuManager>();
        //packageParser = GameContext.ServicesProvider.GetRequiredService<IPackageParser>();
    }
  
public void Execute()
    {
      
         //  LoginBtn.onClick.Invoke();

       
        Debug.Log("Build test connection");
        //var stream = client.GetStream();
        //Debug.Log("Create Streams");
        //BinaryReader reader = new BinaryReader(stream);
        //BinaryWriter writer = new BinaryWriter(stream);
        // clientConn = new ClientConnection();
        var t = GetComponentInChildren<MenuManager>(true);
       
      

        connectionDispatcher.Start();
        Debug.Log("Write Login packages");
        string username = usernameInput.text;
        string pass = passwordInput.text;

        connectionDispatcher.SendPackage(new LoginRequestPackage
        {
            Username = username,
            Password = pass,
        });

        Debug.Log("Receive Login response packages");
        //var packageData = packageParser.ParsePackageFromStream(clientConn.Reader);
        var packageData = this.connectionDispatcher.WaitForPackage<LoginResponsePackage>().Result;

        if (packageData.ErrorMsg == ErrorMsgTypes.Failed)
        {
            ErrorMsgText.color = Color.red;
            ErrorMsgText.text = "There went Something Wrong with your Creditals";
        }
        else
        {
            ErrorMsgText.color = Color.green;
            ErrorMsgText.text = "Login Successfull";
            Debug.Log($"Receive Login response packages TYPE: {packageData.GetType()} RESULT: {(packageData as LoginResponsePackage)?.IsValid}");

            menuManager.RaceSelectionMenu();
        }
    }

    public void RaceOne()
    {
        //clientConn.Connect("127.0.0.1", 3456);
        //clientConn = new ClientConnection();
        //clientConn.Connect("127.0.0.1", 3456);
        Debug.Log("Write Race packages");
        const int realmID = 1;

        //packageParser.ParsePackgeToStream(new RaceRequestPackage
        //{
        //},clientConn.Writer);
        connectionDispatcher.SendPackage(new RealmRequestPackage
        {
            RealmID = realmID,
        });

        Debug.Log("Receive Race response packages");

        var packgeData = this.connectionDispatcher.WaitForPackage<RealmResponsePackage>().Result;

        

        Debug.Log($"Receive Race response packages TYPE: {packgeData.GetType()} RESULT: {packgeData?.RealmID}");

        Raceidtext = packgeData?.RealmID.ToString();
        // public string Raceidtext = { get; set(packgeData as RaceResponsePackage)?.RaceID.ToString();}

        /// Muss noch abgefragt werden
        const bool charExists = true;

        var t = GetComponentInChildren<MenuManager>(true);
        if (!charExists)
        {
            menuManager.CharacterCreationMenu();
        }
        else
        {
            menuManager.CharacterSelectionMenu();

            Debug.Log($"Receive Race response packages TYPE: {packgeData.GetType()} RESULT: {packgeData?.ID}");


          //  var packgeData1 = this.connectionDispatcher.WaitForPackage<CharacterSelectionResponsePackage>().Result;

            //   CharacterList.Add(packgeData1.CharName);
         //   t.CharacterSelectionMenu();
        }

        //GameObject canvas = GameObject.Find("Canvas");
        //var name = canvas.transform.GetChild(0).transform.GetChild(0).name.ToString(); //GetComponent<Text>;
        // .GetComponentInChildren<Text>().text = raceidtext.ToString();
        // canvas.GetComponentInChildren<Text>().text = raceidtext;
        //var race = raceidtext;

        //GameObject Racetext = GameObject.FindWithTag("ID");
        //Racetext.GetComponent<Text>().text = raceidtext;
        //Racetext.GetComponent("RaceIDInput")
        //RaceIDText.text = raceidtext;

        //var text = GameObject.Find("Canvas").transform.Find("Text").GetComponentInChildren<Text>().text;
        //Canvas canvas = GameObject.FindObjectOfType<Canvas>();
        //canvas.GetComponentInChildren<Text>().GetComponent<Text>().text = (packgeData as RaceResponsePackage)?.RaceID.ToString() ?? throw new InvalidOperationException("WRONG PACKAGE");
        ////var text = GameObject.Find("Image").GetComponentInChildren<Text>();
        //////gameObject. FindObjectOfType<Image>().GetComponentInChildren<Text>().text;
        //text.text = (packgeData as RaceResponsePackage)?.RaceID.ToString() ?? throw new InvalidOperationException("WRONG PACKAGE");
        //text = (packgeData as RaceResponsePackage)?.RaceID.ToString()
        //    ?? throw new InvalidOperationException("WRONG PACKAGE");
    }

    public CharacterSelectionResponsePackage CharacterSelection()
    {
        connectionDispatcher.SendPackage(new CharacterSelectionRequestPackage
        {
        });

        var packgeData = this.connectionDispatcher.WaitForPackage<CharacterSelectionResponsePackage>().Result;

        Debug.Log($"Receive CharacterSelection response packages TYPE: {packgeData.GetType()} RESULT: {packgeData?.ID}");

        return packgeData;

    }

    //   public int userId { get; set; }
    public string UmaRecipeData { get; set; }

    public void SaveCharacter(string UmaRecipeData, string CharacterName)
    {
      //  PlayerPrefs.SetString("CharacterData", characterAvatar.GetCurrentRecipe(true));
        
        //string UmaRecipeData = characterAvatar.GetCurrentRecipe(true); //PlayerPrefs.GetString("CharacterData");
        //string CharacterName = CharacterNameInput.text;
        connectionDispatcher.SendPackage(new CharacterCreationRequestPackage
        {
            CharName = CharacterName,
            UmaRecipe = UmaRecipeData,
        });


        menuManager.CharacterSelectionMenu();
        //var t = GetComponentInChildren<MenuManager>(true);
        //t.CharacterSelectionMenu();


    }
}