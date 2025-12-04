/// @author Lempi Leinonen
/// @version 3.12.2025
/// <summary>
/// Pelaaja luokka.
/// </summary>
using System;
using Jypeli;

public class Pelaaja : PhysicsObject
{
    /// <summary>
    /// Pelaajan tekstuuri
    /// </summary>
    private Image _pelaajanKuva = Game.LoadImage("ismo.png");

    /// <summary>
    /// Voima, jolla pelaaja hyppää
    /// </summary>
    private const double HyppyNopeus = 5000;

    /// <summary>
    /// Pistelaskuri label
    /// </summary>
    private Label _naytto = new Label();

    /// <summary>
    /// Taulukkko aanista, joita soitetaan pisteitä kerätessä 
    /// </summary>
    private SoundEffect[] _pisteAanet = new SoundEffect[]
    {
        Game.LoadSoundEffect("5000.wav"),
        Game.LoadSoundEffect("noin.wav"),
        Game.LoadSoundEffect("zajaja.wav"),
        Game.LoadSoundEffect("kiitoksia.wav"),
        Game.LoadSoundEffect("kassa.wav")
    };

    /// <summary>
    /// Taulukkko aanista, joita soitetaan kuollessa 
    /// </summary>
    private SoundEffect[] _kuolemaAanet = new SoundEffect[]
    {
        Game.LoadSoundEffect("helvetti.wav"),
        Game.LoadSoundEffect("helvettia.wav"),
        Game.LoadSoundEffect("aijai.wav")
    };

    /// <summary>
    /// Taulukkko aanista, joita soitetaan kuollessa 
    /// </summary>
    private SoundEffect[] _hyppyAanet = new SoundEffect[]
    {
        // Sama ääni 3 kertaa, jotta ärsyttävämpi soi vain 25% todennäkösyydellä
        Game.LoadSoundEffect("bzz.wav"),
        Game.LoadSoundEffect("bzz.wav"),
        Game.LoadSoundEffect("bzz.wav"),
        Game.LoadSoundEffect("szzz.wav")
    };

    /// <summary>
    /// Muuttuja, jonka on oltava true, jotta pelaaja voi hypätä
    /// </summary>
    private bool _voiHypata = true;

    /// <summary>
    /// Aika sekunneissa, joka pitää odottaa hyppyjen välissä
    /// </summary>
    private double _hyppyOdotus = 0.2;

    /// <summary>
    /// Pelaajan rahan/pisteiden määrä
    /// </summary>
    public IntMeter raha;


    /// <summary>
    /// Pelaajan luonti funktio
    /// </summary>
    /// <param name="leveys">Pelaajan leveys</param>
    /// <param name="korkeus">Pelaajan korkeus</param>
    /// <param name="peli">Peli luokka, johon pelaaja luodaan</param>
    public Pelaaja(double leveys, double korkeus, PhysicsGame peli)
        : base(leveys, korkeus)
    {
        IgnoresCollisionResponse = false;
        IgnoresGravity = false;
        Image = _pelaajanKuva;
        // Luo esteen kentän ulkopuolelle oikeaan reunaan
        X = 0;
        Y = 0;
        Restitution = 0.0;
        Game.Add(this);
        LisaaNappaimet();
        raha = LuoPisteLaskuri();
        
        // Tunnistaa osumat pisteisiin ja esteisiin
        peli.AddCollisionHandler(this, "este", EsteOsuma);
        peli.AddCollisionHandler(this, "piste", PisteOsuma);
        // Jos kentän ulkopuolella -> game over
        peli.AddCustomHandler(OnkoUlkona, Kuolema);
        // Estää vaaka suunnan liikkeen
        peli.AddCustomHandler(LiikkuukoX, PysaytaX);

    }


    /// <summary>
    /// Tarkistaa, liikkuuko pelaaja vaaka suunnassa
    /// </summary>
    /// <returns>True, jos pelaaja liikkuu</returns>
    private bool LiikkuukoX()
    {
        if (this.Velocity.X != 0) return true;
        else return false;
    }


    /// <summary>
    /// Pysayttaa pelaajan vaaka suuntaisen liikkeen
    /// </summary>
    private void PysaytaX()
    {
        this.Velocity = new Vector(0, Velocity.Y);
    }


    /// <summary>
    /// Luo piste laskurin
    /// </summary>
    /// <returns>Palauttaa luodun pistelaskurin</returns>
    private IntMeter LuoPisteLaskuri()
    {
        IntMeter laskuri = new IntMeter(0);
        laskuri.MaxValue = 10000;

        // asettaa nayton nayttamaan laskuri muuttujan arvon
        _naytto.BindTo(laskuri);
        // Asettaa parametreina annetut koordinaatit naytolle
        _naytto.X = Game.Level.Left + 50;
        _naytto.Y = Game.Level.Top - 50;
        // asettaa värit naytolle
        _naytto.TextColor = Color.White;
        _naytto.BorderColor = Game.Level.Background.Color;
        _naytto.Color = Game.Level.Background.Color;
        Game.Add(_naytto);

        return laskuri;
    }


    /// <summary>
    /// Funktio, jota collision handler kutsuu kun osutaan esteeseen
    /// </summary>
    /// <param name="pelaaja">Pelaaja, jonka osumaa seurataan</param>
    /// <param name="asia">Este johon pelaaja osuu</param>
    private void EsteOsuma(PhysicsObject pelaaja, PhysicsObject asia)
    {
        Kuolema();
    }


    /// <summary>
    ///  Funktio, jota collision handler kutsuu, kun pelaaja osuu pisteeseen
    /// </summary>
    /// <param name="pelaaja">Pelaaja, jonka osumaa seurataan.</param>
    /// <param name="asia">Piste johon pelaaja on osunut</param>
    private void PisteOsuma(PhysicsObject pelaaja, PhysicsObject asia)
    {
        this.Move(new Vector(0, Velocity.Y)); // Estää liikkumisen vaakasuunnassa törmättäessä pisteeseen
        raha.Value += 1;
        SoitaRandomAani(_pisteAanet);
        Pyorita(asia);
        Game.MessageDisplay.Add("Keräsit kolikon!");
        if (asia is Tausta tausta) tausta.Poista();
    }

    /// <summary>
    /// Pyörittää pelaajaa, kun se osuu pisteeseen.
    /// </summary>
    /// <param name="asia"></param>
    private void Pyorita(PhysicsObject asia)
    {
        this.AngularVelocity = AngularVelocity / 2;
        double maxVauhti = 2.5;
        double vauhti = 0 + Random.Shared.NextDouble() * (maxVauhti - 0);
        if (Y < asia.Y) this.AngularVelocity += vauhti;
        else this.AngularVelocity -= vauhti;
    }


    /// <summary>
    /// Valitsee satunnaisen äänen taulukosta, jossa on SoundEffect olioita ja soittaa sen.
    /// </summary>
    /// <param name="aanet">Taulukko äänistä</param>
    private void SoitaRandomAani(SoundEffect[] aanet)
    {
        if (aanet.Length == 0) return;
        int i = RandomGen.NextInt(0, aanet.Length);
        aanet[i].Play();
    }


    /// <summary>
    /// Funktio, joka näyttää lopetus ruudun ja käynnistää pelin resetoinnnin
    /// </summary>
    public void Kuolema()
    {
        // Näyttää viestin, kun pelaaja kuolee
        // Erikois tapaukset 0 ja 1 pisteelle
        Game.MessageDisplay.Clear();
        switch (raha)
        {
            case 0:
                Game.MessageDisplay.Add($"Kuolit! Et keränny yhtään kolikkoa.", Color.Red);
                break;

            case 1:
                Game.MessageDisplay.Add($"Kuolit! Keräsit {raha} kolikon.", Color.Red);
                break;

            default:
                Game.MessageDisplay.Add($"Kuolit! Keräsit {raha} kolikkoa.", Color.Red);
                break;
        }

        Game.MessageDisplay.X = 0;
        Game.MessageDisplay.Y = 0;
        SoitaRandomAani(_kuolemaAanet);
        // Poistaa pelaajan ja aloittaa peli alusta
        this.Destroy();
        _naytto.Destroy();
        Timer.SingleShot(3, Uudestaan);
    }


    /// <summary>
    /// Kutsuu pelin Begin funktioita
    /// </summary>
    private void Uudestaan()
    {
        Game.Begin();
    }


    /// <summary>
    /// Hyppää ja käynnistää nopean ajastimen, joka estää hypyn spammaamisen
    /// </summary>
    /// <param name="nopeus"></param>
    private void Hyppaa(double nopeus)
    {
        if (X != 0) X = 0; // Toinen tapa pitää pelaaja vaaka suunnassa paikoillaan.
        
        // Estää hypyn spämmäämisen
        if (!_voiHypata) return; 
        _voiHypata = false;
        // Ajastin joka resetoi _voiHypata muuttuja takaisin true
        Timer.SingleShot(_hyppyOdotus, ValmisHyppaamaan);

        // Asettaa pelaajan pysty suunnan liikkeen nollaan ennen hyppyä, jotta suuren pudeotuksen jälkeen hyppy ei ole tehoton
        if (Velocity.Y < 0) this.Velocity = new Vector(0, 0);

        // Hyppy
        this.Hit(new Vector(0, nopeus));
        SoitaRandomAani(_hyppyAanet);
    }


    /// <summary>
    /// Resetoi hyppy valmiuden, kun ajastin Hyppaa funktiossa on valmis,
    /// </summary>
    private void ValmisHyppaamaan()
    {
        _voiHypata = true;
    }


    /// <summary>
    /// Lisää hypp nappin peliin
    /// </summary>
    private void LisaaNappaimet()
    {
        Game.Keyboard.Listen(Key.Space, ButtonState.Pressed, Hyppaa, "Pelaaja hyppää", HyppyNopeus);
    }


    /// <summary>
    /// Tarkistaa, onko pelaaja kentän ulkopuolella.
    /// </summary>
    /// <returns>Jos ulkopuolella, niin true</returns>
    private bool OnkoUlkona()
    {
        if (this.Y < Game.Level.Bottom || this.Y > Game.Level.Top)
        {
            this.Y = 0; // Korjaa bugin, joka looppasi pelin aloitusta, kun pelaaja putosi maailmasta.
            return true;
        }
        return false;
    }
}
