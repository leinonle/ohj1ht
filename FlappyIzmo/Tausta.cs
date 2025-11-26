using System;
using Jypeli;
using Silk.NET.OpenGL;

public class Tausta : PhysicsObject
{
    /// <summary>
    /// Nopeus joka lisataan pelaajan nopeuteen
    /// </summary>
    private double _nopeus = 0;

    /// <summary>
    /// Pitää kirjaa, onko tuhottu. Muistaakseni tein tämän bugin korjausta varten
    /// </summary>
    private bool tuhottu = false;

    /// <summary>
    /// Peli olio, jonka siällä tämä on luotu
    /// </summary>
    private FlappyIzmo PELI;


    /// <summary>
    /// Olion luonti funktio
    /// </summary>
    /// <param name="leveys">Olion leveys</param>
    /// <param name="korkeus">Olion korkeus</param>
    /// <param name="pelaajanNopeus">Taustan liike nopeus</param>
    /// <param name="x">X-koordinaatti, johon luodaan</param>
    /// <param name="y">Y-koordinaatti, johon luodaan</param>
    /// <param name="peli">Peli olio, jossa tämä on luotu</param>
    public Tausta(double leveys, double korkeus, double pelaajanNopeus, double x, double y, FlappyIzmo peli)
        : base(leveys, korkeus)
    {
        // Tällä luodaan illuusio, että pelaaj liikku tietyllä nopeudella
        PELI = peli;
        _nopeus += pelaajanNopeus;
        IgnoresCollisionResponse = true;
        IgnoresGravity = true;
        Image texture = Game.LoadImage("pilvi");
        Image = texture;
        // Asettaa esteelle nopeuden
        Velocity = new Vector(-_nopeus, 0);
        // Luo esteen kentän ulkopuolelle oikeaan reunaan
        X = x + leveys * 2;
        Y = y;
        Game.Add(this);
        // Jos on kentän ulkopuolella, niin poistaa itsensä
        Game.AddCustomHandler(OnUlkona, Poista);
    }


    /// <summary>
    /// Tarkistaa, onko kentän ulkopuolella
    /// </summary>
    /// <returns>true jos on ulkona</returns>
    private bool OnUlkona()
    {
        if (this.X < Game.Level.Left)
        {
            return true;
        }
        return false;
    }


    /// <summary>
    /// Kutsuu pelin tausta olion poisto funktiota, joka poistaa tämän taulukosta
    /// </summary>
    public void Poista()
    {
        if (!tuhottu)
        {
            PELI.PoistaMaailmaOlio(this);
            tuhottu = true;
            this.Destroy();
        }
    }

}
