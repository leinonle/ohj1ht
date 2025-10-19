using System;
using Jypeli;

public class Este : Tausta
{
    private double nopeus = 200;

    public Este(double leveys, double korkeus, double pelaajanNopeus, double x, double y)
        : base(leveys, korkeus, pelaajanNopeus, x, y)
    {
        // Tällä luodaan illuusio, että pelaaj liikku tietyllä nopeudella
        nopeus += pelaajanNopeus;
        IgnoresCollisionResponse = false;
        IgnoresGravity = true;
        Image texture = Game.LoadImage("norsu"); // !!!Korvataan paremmalla
        Velocity = new Vector(-nopeus, 0);
        Image = texture;
        Game.Add(this);
    }

}
