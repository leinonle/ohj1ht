using System;
using Jypeli;

public class Tausta : PhysicsObject
{
    private double nopeus = 0;
    private Timer ajastin;

    public Tausta(double leveys, double korkeus, double pelaajanNopeus, double x, double y)
        : base(leveys, korkeus)
    {
        // Tällä luodaan illuusio, että pelaaj liikku tietyllä nopeudella
        nopeus += pelaajanNopeus;
        IgnoresCollisionResponse = true;
        IgnoresGravity = true;
        Image texture = Game.LoadImage("tahti"); // !!!Korvataan paremmalla
        Image = texture;
        // Asettaa esteelle nopeuden
        Velocity = new Vector(-nopeus, 0);
        // Luo esteen kentän ulkopuolelle oikeaan reunaan
        X = x + leveys * 2;
        Y = y;
        Game.Add(this);
        Paivitus();
    }

    private void Paivitus()
    {
        ajastin = new Timer();
        ajastin.Interval = 1;
        ajastin.Timeout += Paivita;
        ajastin.Start();
    }

    private void Paivita()
    {
        PoistaKunUlkona();
    }

    public void PoistaKunUlkona()
    {
        if (this.X + this.Width / 2 < Game.Level.Left)
        {
            this.Destroy();
            Console.WriteLine("Poistettu");
        }
    }
}
