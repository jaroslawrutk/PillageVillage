using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using System.Xml;
using System.IO;
using System;
using UnityEngine.Video;
using Random = System.Random;
using UnityEngine.SceneManagement;

public class GameManager : MonoBehaviour {

    string datapath;
    //Zmienne globalne przechowujące poziom gry oraz środki gracza.
    public static float stage = 1;
    public static int money = 0;
    //Zmienne systemowe służące do pobrania czasu.
    private DateTime datenow = DateTime.Now;
    private DateTime datelastlog;
    //Zmienne pomocnicze
    bool optionsactie = true;
    public int Addstime = 0;
    [Header("Player Statics")] // Statystyki gracza.
    public float HouseHP = stage * 25; //Obecna ilość życia, edytowana.
    public float FullHouseHP = stage * 25;//Cała ilość życia.
    public float TapAtack = 1f;//Wartość zadawanych obrażeń przez pojedyńcze kliknięcie.
    public int MoneyPerDestroy = 50;// Wartość złota dostawanego po zniszczeniu.
    [Header("GUI")] //Zmienne edytowanych obiektów na scenie.
    public Text HpText;
    public Text HpFullHp;
    public Text moneyText;
    public Text Stage;
    public Text Dps;
    public Text UsernameTxt;
    public Image HPimage;
    public GameObject options;
    public GameObject CreditsPanel;
    public GameObject GreenHeroObject;
    public GameObject BlackHeroObject;
    public GameObject AssasinHeroObject;
    public GameObject Tresure;
    public GameObject[] Houses;
    [Header("AddsMenu")]//Zmienne służące do zaimplementowania reklam.
    public GameObject Addplayer;
    public GameObject Video;
    public GameObject AddsPanel;
    public GameObject ExitAddsButton;
    public Text Timer;
    public Text earnedMoney;
    //Zmienn służące do zdefiniowania kolorów pasków życia.
    Color green = new Color(0, 0.6415094f, 0.2539307f);
    Color red = new Color(0.8301887f, 0.1755913f, 0.08223567f);
    //Zmienne używane wczytywania pliku zapisu
    XmlDocument gamedata = new XmlDocument();
    XmlNode Username;
    XmlNode LastloggedTime;
    XmlNode StageXML;
    XmlNode Money;
    XmlNode GreenHeroLvl;
    XmlNode BlackHero;
    XmlNode AssasinHero;
    XmlNode AVGDMG;

    void LoadDataFromXMLFile()
    {
        Username = gamedata.DocumentElement.SelectSingleNode("Username");
        LastloggedTime = gamedata.DocumentElement.SelectSingleNode("LastloggedTime");
        StageXML = gamedata.DocumentElement.SelectSingleNode("Stage");
        Money = gamedata.DocumentElement.SelectSingleNode("Money");
        GreenHeroLvl = gamedata.DocumentElement.SelectSingleNode("GreenHeroLvl");
        BlackHero = gamedata.DocumentElement.SelectSingleNode("BlackHero");
        AssasinHero = gamedata.DocumentElement.SelectSingleNode("AssasinHero");
        AVGDMG = gamedata.DocumentElement.SelectSingleNode("AVGDMG");
        Debug.Log("User last logged was in " + AVGDMG.InnerText);
        UsernameTxt.text = Username.InnerText;
        money = Convert.ToInt32(Money.InnerText);
        stage = Convert.ToInt32(StageXML.InnerText);
        //Zmienne globalne pochodzące z klasy HeroesManager
        HeroesManager.GreenHero.lvl = Convert.ToInt32(GreenHeroLvl.InnerText);
        HeroesManager.BlackHero.lvl = Convert.ToInt32(BlackHero.InnerText);
        HeroesManager.AssasinHero.lvl = Convert.ToInt32(AssasinHero.InnerText);
        HeroesManager.AverageDMGperSecond = Convert.ToInt32(AVGDMG.InnerText);
        datelastlog = Convert.ToDateTime(LastloggedTime.InnerText);
        Debug.Log("User last logged was in " + datelastlog);
    }
    // Use this for initialization
    void Start() {
        datapath = Path.Combine(Application.persistentDataPath, "data.xml");
        gamedata.Load(datapath);
        LoadDataFromXMLFile();
        if (HeroesManager.GreenHero.lvl > 0 )
            GreenHeroObject.SetActive(true);
        if (HeroesManager.BlackHero.lvl > 0)
            BlackHeroObject.SetActive(true);
        if (HeroesManager.AssasinHero.lvl > 0)
            AssasinHeroObject.SetActive(true);

        optionsactie = true;
        Credits = false;
        options.SetActive(false);
        CreditsPanel.SetActive(false);
        HpText.text = HouseHP.ToString("0");
        HpFullHp.text = FullHouseHP.ToString("0");
        Debug.Log(stage+" "+HouseHP.ToString("0"));
        moneyText.text = money.ToString();
        HouseHP = stage * 25;
        FullHouseHP = stage * 25;
        InvokeRepeating("MakeDamage", 0, 1f);
    }
    public void FindTresure()
    {
        AddsPanel.SetActive(true);
        ExitAddsButton.SetActive(false);
        Debug.Log("YOU FIND TRESURE");
        Video.SetActive(true);
        Addplayer.GetComponent<VideoPlayer>().Play();
        InvokeRepeating("Increse", 0f, 1f);
        Tresure.SetActive(false);
    }
    void Increse()
    {
        Addstime++;
        Timer.text = (20-Addstime).ToString() + " s";
        if (Addstime == 20)
        {
            Video.SetActive(false);
            money += 200;
            earnedMoney.gameObject.SetActive(true);
            earnedMoney.text = "You earn a 200 gold";
            ExitAddsButton.SetActive(true);
            CancelInvoke("Increse");
            Addstime = 0;
        }
    }
    public void MakeDamage()
    {
        HouseHP -= HeroesManager.AverageDMGperSecond;
        HpText.text = HouseHP.ToString("0");
        if (HouseHP == 0 || HouseHP < 0 )
        {
            HouseHP = 0;
            HpText.text = HouseHP.ToString("0");
            SetHouse();
            MakeCalculation();
        }
    }
    public void MakeDamageFromClick()
    {
        HouseHP -= TapAtack;
        HpText.text = HouseHP.ToString("0");
        Debug.Log("House hp is :" + HouseHP);
        if (HouseHP == 0 || HouseHP < 0)
        {
            HouseHP = 0;
            HpText.text = HouseHP.ToString("0");
            SetHouse();
            
            MakeCalculation();
        }
    }
    public void SetHouse()
    {
        
        Random random = new Random();
        int index = random.Next(0, Houses.Length);
        int i = 0;
        foreach (GameObject domek in Houses)
        {
            if (i == index)
                Houses[index].SetActive(true);
            else
                Houses[i].SetActive(false);
            i++;
        }
    }
    IEnumerator Enumerator()
    {
        yield return new WaitForSeconds(8);
        Debug.Log(".");
        Tresure.SetActive(false);
    }
    private int counters = 0;

    public void MakeCalculation() {

        if (stage % 25 == 0) {
       
                Tresure.SetActive(true);
                StartCoroutine(Enumerator());
                counters = 0;
        }
        money += MoneyPerDestroy;
        moneyText.text = money.ToString();
        stage++;
        HouseHP = stage * 25;
        FullHouseHP = stage * 25;
    }
    public void ShowOptions()
    {
        options.SetActive(optionsactie);
        optionsactie=!optionsactie;
    }
    bool Credits = false;
    public void ShowCredits()
    {
        CreditsPanel.SetActive(true);
        Credits = !Credits;
    }
    public void HideCredits()
    {
        CreditsPanel.SetActive(false);
    }
    public void CloseAddWindow()
    {
        AddsPanel.SetActive(false);
    }
	// Update is called once per frame
	void Update () {
        moneyText.text = money.ToString();
        HpText.text = HouseHP.ToString("0");
        HpFullHp.text = FullHouseHP.ToString("0");
        Stage.text = stage.ToString();
        Dps.text = HeroesManager.AverageDMGperSecond.ToString();
        HPimage.fillAmount = HouseHP / FullHouseHP;
        if (HouseHP / FullHouseHP > 0.50f)
            HPimage.color = green;
        if (HouseHP / FullHouseHP <= 0.50f)
            HPimage.color = Color.yellow;
        if (HouseHP / FullHouseHP <= 0.25f)
            HPimage.color = red;
    }
    public void DeleteAccount()
    {
        File.Delete(datapath);
        Application.LoadLevel(0);
    }
    private void OnApplicationPause(bool pause)
    {
        Username.InnerText = UsernameTxt.text;
        Money.InnerText = money.ToString();
        StageXML.InnerText = stage.ToString();
        AssasinHero.InnerText = HeroesManager.AssasinHero.lvl.ToString();
        BlackHero.InnerText = HeroesManager.BlackHero.lvl.ToString();
        GreenHeroLvl.InnerText = HeroesManager.GreenHero.lvl.ToString();
        LastloggedTime.InnerText = DateTime.Now.ToString();
        Debug.Log("Data Saved " + Username.InnerText + " " + Money.InnerText + " " + StageXML.InnerText + " " + AssasinHero.InnerText + " " + BlackHero.InnerText + " " + GreenHeroLvl.InnerText + " " + LastloggedTime.InnerText);
        gamedata.Save(datapath);
    }
    private void OnApplicationQuit()
    {
        Username.InnerText=UsernameTxt.text;
        Money.InnerText = money.ToString();
        StageXML.InnerText = stage.ToString();
        AssasinHero.InnerText = HeroesManager.AssasinHero.lvl.ToString();
        BlackHero.InnerText = HeroesManager.BlackHero.lvl.ToString();
        GreenHeroLvl.InnerText = HeroesManager.GreenHero.lvl.ToString();
        AVGDMG.InnerText = HeroesManager.AverageDMGperSecond.ToString();
        LastloggedTime.InnerText = DateTime.Now.ToString();
        Debug.Log("Data Saved " + Username.InnerText + " " + Money.InnerText + " " + StageXML.InnerText + " " + AssasinHero.InnerText + " " + BlackHero.InnerText + " " + GreenHeroLvl.InnerText + " " + LastloggedTime.InnerText);
        gamedata.Save(datapath);
    }
}
