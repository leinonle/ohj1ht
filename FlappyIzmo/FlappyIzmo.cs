/// @author Lempi Leinonen
/// @version 22.09.2025
/// <summary>
/// 
/// </summary>
using System;
using Jypeli;

public class FlappyIzmo : PhysicsGame
{

    /// <summary>
    /// Nopeus, jota kaikki tausta oliot kulkevat vähintään
    /// </summary>
    private double _pelaajanNopeus = 400;

    /// <summary>
    /// Pelaaja
    /// </summary>
    private Pelaaja _pelaaja = null;

    /// <summary>
    /// Taulukko kaikista Tausta oliosta joita maailmassa on
    /// Kun taulukko on tyhjä luodaan uusi setti esteitä eli käytännössä uusi taso
    /// Taulukosta poistetaan Olioita sitä mukaa, kun ne poistuvat kentästä
    /// </summary>
    private Tausta[] _maailmaOliot = [];

    /// <summary>
    /// Taso/kierros, johon pelaaja on päässyt.
    /// Korkeampi tasi -> vaikeampaa
    /// </summary>
    public int _taso = 0;

    /// <summary>
    /// Kerroin, jota käytetään tason haastavuuden kasattamiseen tasojen välilä
    /// </summary>
    private const double VaikeusKerroin = 1.45;

    /// <summary>
    /// Esteiden tiheys
    /// </summary>
    private double _esteTiheys;

    /// <summary>
    /// Pisteiden tiheys
    /// </summary>
    private double _pisteTiheys;

    /// <summary>
    /// Jokaisen luodun tason pituus
    /// </summary>
    private double _kentanPituus;


    /// <summary>
    /// Pelin aloitus
    /// </summary>
    public override void Begin()
    {

        // Alustetetaan pelin haastavuuteen vaikuttavat muuttujat
        _taso = 0;
        _esteTiheys = 1000;
        _pisteTiheys = 2000;
        _kentanPituus = 2000;

        for (int i = 0; i < _maailmaOliot.Length; i++)
        {
            PoistaMaailmaOlio(_maailmaOliot[i]);
        }
        Gravity = new Vector(0, -1500);
        LuoKentta(); // Luo maailman
        MasterVolume = 0.3; // Ei ole viela ääniä, mutta varmaan hyödyllinen
    }


    /// <summary>
    /// Luo kentän
    /// </summary>
    private void LuoKentta()
    {
        //luodaan kaikki maailman oliot
        _pelaaja = new Pelaaja(100, 100, this);

        // Aina kun kaikki Tausta oliot on loppunut luo seuraavan tason
        AddCustomHandler(OnkoTyhja, SeuraavaTaso);

        Level.Background.CreateGradient(Color.White, Color.SkyBlue);
    }


    /// <summary>
    /// Tarkistaa onko kaikki Tausta oliot poistettu
    /// </summary>
    /// <returns>true jos kenttä tyhjä</returns>
    private bool OnkoTyhja()
    {
        // Palauttaa true, jos maailmaolioita ei ole olemassa
        if (_maailmaOliot.Length == 0) return true;
        return false;
    }


    /// <summary>
    /// Laskee vaikeuden korotuksen ja luo sen perusteella seuraavan setin esteitä ja pisteitä
    /// </summary>
    private void SeuraavaTaso()
    {
        if (_pelaaja != null) _taso += _pelaaja.raha / 10; //Pisteet lisää haastetta
        _taso += 1;

        double vaikeus = Math.Pow(VaikeusKerroin, _taso);

        _esteTiheys = Math.Max(_esteTiheys - vaikeus * 10, 100);
        _pisteTiheys = Math.Max(_pisteTiheys - vaikeus * 10, 200);   // ei mene negatiiviseksi

        _kentanPituus = _kentanPituus + _taso * 150;   // skaalautuu siististi

        LuoMaailma("este", _esteTiheys, _kentanPituus);
        LuoMaailma("piste", _pisteTiheys, _kentanPituus);
        LuoMaailma("tausta", _pisteTiheys/2, _kentanPituus);
    }


    /// <summary>
    /// Luo tyypin mukaisia olioita tietyn välimatkan välein FOR loopilla
    /// </summary>
    /// <param name="tyyppi">Luotavien olioiden tyyppi</param>
    /// <param name="tiheys">Kuinka tiheästi olioita luodaan</param>
    /// <param name="pituus">Kuinka pitkälle matkalle oloita luodaan kerrallaan</param>
    private void LuoMaailma(string tyyppi, double tiheys, double pituus)
    {
        int vali = Convert.ToInt32(tiheys); // Kuinka suuret välit esteiden välillä on
        Console.WriteLine(vali);
        if (vali < 20) vali = 20; // kaatuu ilman
        // Luodaan silmukalla kenttä
        // Luo FOR loopilla olioita random korkeudelle tietyllä välimatkalla
        for (int i = 0; i < pituus; i++)
        {
            if (i % vali == 0)
            {
                // Asettaa random korkeuden esteelle ylä- ja alareunan välistä
                double y = RandomGen.NextDouble(Level.Top, Level.Bottom);
                LisaaMaailmaOlio(tyyppi, Level.Right + i, y);
            }
        }
    }


    /// <summary>
    /// Luo yksittäisen maailma olion annettuun koordinaattiin
    /// </summary>
    /// <param name="tyyppi">Luotavan olion tyyppi eli luokka</param>
    /// <param name="x">X-koordinaatti</param>
    /// <param name="y">Y-koordinaatti</param>
    private void LisaaMaailmaOlio(String tyyppi, double x, double y)
    {
        // Voisi lisata random koon.
        Tausta olio = null;
        switch (tyyppi)
        {
            case "tausta":
                olio = new Tausta(560, 480, _pelaajanNopeus, x, y, this);
                break;

            case "piste":
                olio = new Piste(140, 120, _pelaajanNopeus, x, y, this);
                break;

            case "este":
                double koko = RandomGen.NextDouble(40, 120);
                olio = new Este(koko, koko, _pelaajanNopeus, x, y, this);
                break;

            default:
                olio = new Tausta(40, 40, _pelaajanNopeus, x, y, this);
                break;
        }
        if (olio == null) return;
        int lkm = _maailmaOliot.Length;
        // Lisää olion _maailmaOliot-taulukoon
        Tausta[] uudetOliot = new Tausta[lkm + 1];
        for (int i = 0; i < lkm; i++)
        {
            uudetOliot[i] = _maailmaOliot[i];
        }
        uudetOliot[lkm] = olio;
        _maailmaOliot = uudetOliot;
    }

    
    /// <summary>
    /// Poistaa yksittäisen maailma olion _maailmaOliot taulukosta
    /// </summary>
    /// <param name="olio">Poistettava olio</param>
    public void PoistaMaailmaOlio(Tausta olio)
    {
        int lkm = _maailmaOliot.Length;
        Tausta[] uudetOliot = new Tausta[lkm - 1];

        int uusiI = 0;
        for (int i = 0; i < lkm; i++)
        {
            if (_maailmaOliot[i] == olio)
                continue;

            uudetOliot[uusiI] = _maailmaOliot[i];
            uusiI++;
        }

        _maailmaOliot = uudetOliot;
        olio.Destroy();
    }
}
