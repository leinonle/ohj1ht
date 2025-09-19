# Harjoitustyön suunnitelma

## Tietoja 

Tekijä: Lempi Leinonen

Työ git-varaston osoite: <https://github.com/leinonle/ohj1ht>

Pelin nimi: Izmo Mehiläinen

Pelialusta: Windows

Pelaajien lukumäärä: 1

## Pelin tarina

Ismo Laitela joutuu polttareissaan pukeutumaan mehiläiseksi. Kaverit lähettävät hänet kaupungille tehtävällä: kerää rahaa aviokassaan ja väistele esteitä, muuten häät jää pitämättä. Jokainen kolikko ja seteli vie Ismoa lähemmäs unelmahäitä – mutta pylväät, johdot ja muut kommellukset tekevät matkasta kaikkea muuta kuin helpon.

## Pelin idea ja tavoitteet

Flappy bird klooni, jossa kerätään kolikoita ja seteleitä, joista saa pisteitä.
Peli jatkuu kunnes pelaaja osuu tolppaan/esteeseen.

## Hahmotelma pelistä

(Kun olet lisännyt suunnitelmakuvan tähän hakemistoon, linkitä se tähän alle. Alla on esimerkkikuvan linkitys.)

![Esimerkkikuva](esimerkkikuva.png "Esimerkkikuva")

## Toteutuksen suunnitelma

Lokakuu

- pelaaja hahmo, joka tippuu alas päin ja pomppaa/nostaa korkeutta välilyönnillä
    - Hyppy: tarvitsee cooldownin; Nostaa pelaaja hahmoa ylöspäin
    - Osumien tunnistaminen: Pitää tunnistaa milloin osuu esteisiin ja milloin kolikoihin

Marraskuu

- Este spawneri:
    - Spawnaa erilaisia esteitä randomeilla korkeuksille
- Esteet:
    - Pelaaja ei liiku vaaka suunnassa, vaan esteet luodaan näkymän ulko puolelle oikeaan reunaan ja liikkuvat vasemmalle, kun ne on näkymän ulkona, niin ne poistetaan maailmasta
    - Tolpat/perinteiset flappy bird esteet
        - Spawnaa random korkeuteen ja koostuu ylä ja ala osasta
    - Mahdolliset lentävät esteet, kuten ukkos pilvet yms.

Joulukuu

- Kolikot
    - Kolikot: Tunnistaa, kun osuu pelaajaan ja antaa pelaajalle pisteen
        - Mahdollisesti seteleitä, joista saa enemmän pisteitä
- Tekstuurien lisäys
- Piste ja elämä muuttujat ja "game over" handlaus

Jos aikaa jää

- PowerUpit: 
    - aa zajajajaj koskemattomuus, joka disabloi esteet ja nopeuttaa maailmaa.
    - +1 Elämät ja elämät ylipäätänsä ettei kuole heti

