using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using System.Xml;
using System.IO;
using UnityEngine.UI;
using UnityEngine.SceneManagement;

public class MenuController : MonoBehaviour {
    //Ścieżka do utworzenia pliku xml z danami gracza
    string datapath;
    //Zmienne obiektów na scenie
    GameObject startButton; 
    GameObject continueButton;
    GameObject enterNamePanel;
    GameObject creditsPanel;
    Text errorMessage;
    InputField nameinput;
    //Zmienna przechowywująca nazwe użytkownika
    private string username = "";
	// Use this for initialization
	void Start () {
        datapath = Path.Combine(Application.persistentDataPath, "data.xml");
        if (File.Exists(datapath))
        {
            Application.LoadLevel(1);
            Debug.Log("FILE EXIST");
        }
        else
        {
            Debug.Log("FILE DOESNT EXIST");
        }
        startButton = GameObject.Find("NewGameButton");
        enterNamePanel = GameObject.Find("EnterNamePanel");
        creditsPanel = GameObject.Find("CreditsPanel");
        errorMessage = GameObject.Find("MESSAGETXT").GetComponent<Text>();
        nameinput = GameObject.Find("InputName").GetComponent<InputField>();

        errorMessage.gameObject.SetActive(false);
        creditsPanel.gameObject.SetActive(false);
        enterNamePanel.gameObject.SetActive(false);
	}

    public void NewGame()
    {
        //Włączanie ekranu tworzenia
        enterNamePanel.SetActive(true);
    }
    public void LetPillage()
    {
        if(nameinput.text=="")
        {
            errorMessage.gameObject.SetActive(true);
            errorMessage.text = "Enter the name";
            return;
        }
        else
        {
            errorMessage.text = "";
            username = nameinput.text;
            CreateNewXMLFile(username);
            SceneManager.LoadScene(1);
            Debug.Log("I CREATE FILE FOR USER NAME" + username);
        }
    }
    public void Quit()
    {
        enterNamePanel.SetActive(false);
        creditsPanel.SetActive(false);
    }
    public void ShowCredits()
    {
        creditsPanel.SetActive(true);
    }
    void CreateNewXMLFile(string name)
    {

        XmlDocument gamedata = new XmlDocument();
        XmlNode docNode = gamedata.CreateXmlDeclaration("1.0", "UTF-8", null);
        gamedata.AppendChild(docNode);
        //Username
        XmlNode GameUsersNode = gamedata.CreateElement("GameUser");
        gamedata.AppendChild(GameUsersNode);
        //User
        XmlNode Username = gamedata.CreateElement("Username");
        Username.InnerText = name;
        GameUsersNode.AppendChild(Username);
        //Lastlogged
        XmlNode LastloggedTime = gamedata.CreateElement("LastloggedTime");
        LastloggedTime.InnerText = System.DateTime.Now.ToString();
        GameUsersNode.AppendChild(LastloggedTime);
        //Stage
        XmlNode Stage = gamedata.CreateElement("Stage");
        Stage.InnerText = "1";
        GameUsersNode.AppendChild(Stage);
        //Money
        XmlNode Money = gamedata.CreateElement("Money");
        Money.InnerText = "0";
        GameUsersNode.AppendChild(Money);
        //GreenHeroLvl
        XmlNode GreenHeroLvl = gamedata.CreateElement("GreenHeroLvl");
        GreenHeroLvl.InnerText = "0";
        GameUsersNode.AppendChild(GreenHeroLvl);
        //BlackHero
        XmlNode BlackHero = gamedata.CreateElement("BlackHero");
        BlackHero.InnerText = "0";
        GameUsersNode.AppendChild(BlackHero);
        //AssasinHero
        XmlNode AssasinHero = gamedata.CreateElement("AssasinHero");
        AssasinHero.InnerText = "0";
        GameUsersNode.AppendChild(AssasinHero);

        gamedata.Save(datapath);
    }
    // Update is called once per frame
    void Update()
    {

    }
}
