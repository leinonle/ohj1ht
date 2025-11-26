/// @author Lempi Leinonen
/// @version 26.11.2025
/// <summary>
/// Pelaaja luokka.
/// </summary>
using Jypeli;

public class Este : Tausta
{
    /// <summary>
    /// Nopeus joka lisataan pelaajan nopeuteen
    /// </summary>
    private double _nopeus = 10;


    /// <summary>
    /// Olion luonti funktio
    /// </summary>
    /// <param name="leveys">Olion leveys</param>
    /// <param name="korkeus">Olion korkeus</param>
    /// <param name="pelaajanNopeus">Taustan liike nopeus</param>
    /// <param name="x">X-koordinaatti, johon luodaan</param>
    /// <param name="y">Y-koordinaatti, johon luodaan</param>
    /// <param name="peli">Peli olio, jossa tämä on luotu</param>
    public Este(double leveys, double korkeus, double pelaajanNopeus, double x, double y, FlappyIzmo peli)
        : base(leveys, korkeus, pelaajanNopeus, x, y, peli)
    {
        // Tällä luodaan illuusio, että pelaaj liikku tietyllä nopeudella
        _nopeus += pelaajanNopeus;
        IgnoresCollisionResponse = false;
        IgnoresGravity = true;
        Image texture = Game.LoadImage("norsu"); // !!!Korvataan paremmalla
        Velocity = new Vector(-_nopeus, 0);
        Image = texture;
        Game.Add(this);
    }

}
