using System;
using Jypeli;

public class Piste : Tausta
{
    private double nopeus = 400;

    public Piste(double leveys, double korkeus, double pelaajanNopeus, double x, double y)
        : base(leveys, korkeus, pelaajanNopeus, x, y)
    {
        // Tällä luodaan illuusio, että pelaaj liikku tietyllä nopeudella
        nopeus += pelaajanNopeus;
        IgnoresCollisionResponse = false;
        IgnoresGravity = true;
        Image texture = Game.LoadImage("tahti"); // !!!Korvataan paremmalla
        Velocity = new Vector(-nopeus, 0);
        Image = texture;
        Game.Add(this);
    }

}
