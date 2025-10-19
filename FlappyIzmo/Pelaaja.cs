using System;
using Jypeli;
using Jypeli.Effects;

public class Pelaaja : PhysicsObject
{
    private Image pelaajanKuva = Game.LoadImage("norsu.png");
    private const double HYPPYNOPEUS = 5000;

    private bool voiHypata = true;

    private Timer ajastin;

    private IntMeter raha;

    public Pelaaja(double leveys, double korkeus, PhysicsGame peli)
        : base(leveys, korkeus)
    {
        // Tällä luodaan illuusio, että pelaaj liikku tietyllä nopeudella
        IgnoresCollisionResponse = false;
        IgnoresGravity = false;
        Image = pelaajanKuva;
        // Luo esteen kentän ulkopuolelle oikeaan reunaan
        X = 0;
        Y = 0;
        Restitution = 0.0;
        Game.Add(this);
        LisaaNappaimet();
        raha = LuoPisteLaskuri();
        //    Paivitus();
        peli.AddCollisionHandler<PhysicsObject, Este>(this, EsteOsuma);
        peli.AddCollisionHandler<PhysicsObject, Piste>(this, PisteOsuma);



        Game.AddCustomHandler(OnUlkona, Kuolema);
    }

    private IntMeter LuoPisteLaskuri()
    {
        IntMeter laskuri = new IntMeter(0); 
        laskuri.MaxValue = 100;
        
        // Tekstikentän luonti
        Label naytto = new Label();
        // asettaa nayton nayttamaan laskuri muuttujan arvon
        naytto.BindTo(laskuri);
        // Asettaa parametreina annetut koordinaatit naytolle
        naytto.X = Game.Level.Left + 50;
        naytto.Y = Game.Level.Top - 50;
        // asettaa varit naytolle
        naytto.TextColor = Color.White;
        naytto.BorderColor = Game.Level.Background.Color;
        naytto.Color = Game.Level.Background.Color;
        Game.Add(naytto);
        
        return laskuri;
    }

    private void EsteOsuma(PhysicsObject pelaaja, Tausta asia)
    {
        Kuolema();
    }

    private void PisteOsuma(PhysicsObject pelaaja, Tausta asia)
    {
        raha.Value += 1;
        asia.Destroy();
    }

    private bool OnUlkona()
    {
        // Jos pelaaja on maailman ylä tai ala laidan ulkopuolella palauttaa true
        // Muuten palauttaa false
        if (this.Y > Game.Level.Top) return true;
        else if (this.Y < Game.Level.Bottom) return true;
        return false;
    }

    public void Kuolema()
    {
        this.Destroy();
        Timer.SingleShot(2, Uudestaan);
    }

    private void Uudestaan()
    {
        Game.Begin();
    }

    private void Hyppaa(double nopeus)
    {
        if (!voiHypata) return;
        voiHypata = false;
        Timer.SingleShot(0.33, ValmisHyppaamaan);
        if (Velocity.Y < 0) this.Velocity = new Vector(0, 0);
        this.Hit(new Vector(0, nopeus));
    }

    private void ValmisHyppaamaan()
    {
        voiHypata = true;
    }

    private void LisaaNappaimet()
    {
        Game.Keyboard.Listen(Key.Space, ButtonState.Pressed, Hyppaa, "Pelaaja hyppää", HYPPYNOPEUS);
    }
}
