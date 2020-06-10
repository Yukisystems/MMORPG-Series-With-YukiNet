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
using Microsoft.Extensions.DependencyInjection;
using UnityEngine;
using YukiNet.Client;
using YukiNet.PackageParser;

public class RaceSelection : MonoBehaviour
{
    private readonly ConfigurationService configService;
    private readonly IPackageParser packageParser;
    private readonly NetworkConnection clientConn;
    private readonly MenuManager menuManager;

    public RaceSelection()
    {
        configService = ConfigurationService.CreateInstance(s =>
        {
            s.AddSingleton<IPackageParser, PackageParser>();
        });
        packageParser = configService.ServiceProvider.GetRequiredService<IPackageParser>();
        menuManager = new MenuManager();
    }

    //public void RaceOne()
    //{
    //    clientConn = new ClientConnection();
    //    Debug.Log("Write Race packages");
    //    int RaceID = 1;

    //    packageParser.ParsePackgeToStream(new RaceRequestPackage
    //    {
    //        RaceID = RaceID,

    //    }, clientConn.Writer);

    //    Debug.Log("Receive Login response packages");
    //    var packgeData = packageParser.ParsePackageFromStream(clientConn.Reader);
    //    //Debug.Log($"Receive Login response packages TYPE: {packgeData.GetType()} RESULT: {(packgeData as RaceRequestPackage)?. IsValid}");

    //    menuManager.CharacterCreationMenu();

    //}

    public void RaceTwo()
    {
    }

    public void RaceThree()
    {
    }

    // Start is called before the first frame update
    private void Start()
    {
    }

    // Update is called once per frame
    private void Update()
    {
    }
}