using System;
using System.Collections.Generic;
using System.Diagnostics;
using Jypeli;
using Jypeli.Assets;
using Jypeli.Controls;
using Jypeli.Widgets;

/// @author Lempi Leinonen
/// @version 22.09.2025
/// <summary>
/// 
/// </summary>
public class FlappyIzmo : PhysicsGame
{

    private double pelaajanNopeus = 200;
    private const int RUUDUN_KOKO = 40;

    private Image pelaajanKuva = LoadImage("norsu.png");
    private Image tahtiKuva = LoadImage("tahti.png");

    private SoundEffect maaliAani = LoadSoundEffect("maali.wav");

    private Pelaaja pelaaja = null;

    private Tausta[] maailmaOliot = new Tausta[0];



    public override void Begin()
    {
        ClearAll();
        Gravity = new Vector(0, -1000);

        LuoKentta();
        LisaaNappaimet();

        /*
        Camera.Follow(pelaaja);
        Camera.ZoomFactor = 1.2;
        Camera.StayInLevel = true;
        */

        MasterVolume = 0.5;

    }

    private void LuoKentta()
    {
        pelaaja = new Pelaaja(100, 100, this);

        LuoMaailma("este", 500);
        LuoMaailma("piste", 2000);        
        

        Level.Background.CreateGradient(Color.White, Color.SkyBlue);
    }

    private void LuoMaailma(string tyyppi, double tiheys)
    {
        double vali = tiheys;
        int pituus = 10000;
        for (int i = 0; i < pituus; i++)
        {
            if (i % (vali * 1.0) == 0)
            {
                double y = RandomGen.NextDouble(Level.Top, Level.Bottom);
                LisaaMaailmaOlio(tyyppi, Level.Right + i, y);
            }
        }
    }

    private void LisaaMaailmaOlio(String tyyppi, double x, double y)
    {
        // Voisi lisata random koon.
        Tausta olio = null;
        switch (tyyppi)
        {
            case "tausta":
                olio = new Tausta(40, 40, pelaajanNopeus, x, y);
                break;

            case "piste":
                olio = new Piste(40, 40, pelaajanNopeus, x, y);
                break;

            case "este":
                olio = new Este(40, 40, pelaajanNopeus, x, y);
                break;

            default:
                olio = new Tausta(40, 40, pelaajanNopeus, x, y);
                break;
        }
        if (olio == null) return;
        int lkm = maailmaOliot.Length;
        Tausta[] uudetOliot = new Tausta[lkm + 1];
        for (int i = 0; i < lkm; i++)
        {
            uudetOliot[i] = maailmaOliot[i];
        }
        uudetOliot[lkm] = olio;
        maailmaOliot = uudetOliot;
    }

    private void PoistaMaailmaOlio(Tausta olio)
    {
        int lkm = maailmaOliot.Length;
        Tausta[] uudetOliot = new Tausta[lkm - 1];
        for (int i = 0; i < lkm; i++)
        {
            if (maailmaOliot[i] == olio) continue;
            uudetOliot[i] = maailmaOliot[i];
        }
        maailmaOliot = uudetOliot;
    }

    private void LisaaNappaimet()
    {
        Keyboard.Listen(Key.F1, ButtonState.Pressed, ShowControlHelp, "Näytä ohjeet");
        Keyboard.Listen(Key.Escape, ButtonState.Pressed, ConfirmExit, "Lopeta peli");
    }


    private void TormaaTahteen(PhysicsObject hahmo, PhysicsObject tahti)
    {
        maaliAani.Play();
        MessageDisplay.Add("Keräsit tähden!");
        tahti.Destroy();
    }
}

