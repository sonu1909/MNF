using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows;

namespace MnfPic
{
    public class MnfArea
    {
        public enum Typy { TYPE_AREA, TYPE_DIALOG, TYPE_BED, TYPE_CLOTHES_FIT, TYPE_BED_SINGLE, TYPE_MAP, TYPE_BED_TRAINING, TYPE_MINIGAME, TYPE_NPC_SEX, TYPE_POLEDANCE }
        public List<string> Jmena = new List<string>() {"MnF Alley", "Wild West", "MnF Alley / Mme Olivia`s Private Club", "MnF Alley / Hair Salon", "Lagoon Beach", "Sacred Mountain", "Private Club / Room #1", "Private Club / Room #2", "Private Club / Room #3", "Private Club / Room #4", "Wild West / Saloon", "Wild West / Motel Lobby", "Lagoon Beach / Dr.Boobige`s Clinic", "Wild West / Motel / Block A", "Wild West / Motel / Block B", "Wild West / Motel / Block C", "Wild West / Motel / Block D", "Wild West / Motel / Block E", "Wild West / Motel / Block A / Room", "Wild West / Motel / Block B / Room", "Wild West / Motel / Block C / Room", "Wild West / Motel / Block D / Room", "Wild West / Motel / Block E / Room", "Sacred Mountain / Sexual Wisdom Temple", "Wild West / Saloon / Cabaret", "Meadows Park", "Meadows Park / Museum of Sex Art", "Meadows Park / Museum of Sex Art / Recent Exposition 1", "Meadows Park / Museum of Sex Art / Recent Exposition 2", "Meadows Park / Museum of Sex Art / Recent Exposition 3", "Meadows Park / Museum of Sex Art / Recent Exposition 4", "Meadows Park / Museum of Sex Art / Weekly Best", "Meadows Park / Museum of Sex Art / Monthly Best", "Meadows Park / Museum of Sex Art / All Time Best", "Petnis Forest", "Petnis Forest / Petnis Shack", "Sacred Mountain / Cave", "Waterfall", "MnF Metropolis", "MnF Metropolis / Hotel Plaza", "MnF Metropolis / Hotel Plaza 2nd floor", "MnF Metropolis / Hotel Plaza 3rd floor", "MnF Metropolis / Hotel Plaza 4th floor", "MnF Metropolis / Hotel Plaza 5th floor", "MnF Metropolis / Hotel Plaza 6th floor", "MnF Metropolis / Hotel Plaza 7th floor(VIP)", "MnF Metropolis / Hotel Plaza 2nd floor / Room", "MnF Metropolis / Hotel Plaza 3rd floor / Room", "MnF Metropolis / Hotel Plaza 4th floor / Room", "MnF Metropolis / Hotel Plaza 5th floor / Room", "MnF Metropolis / Hotel Plaza 6th floor / Room", "MnF Metropolis / Hotel Plaza 7th floor / President Lux", "MnF Metropolis / Hotel Plaza 8th floor / President Lux", "MnF Metropolis / Hotel Plaza 8th floor(VIP)", "Red Heat", "Bar Vodka", "Shooting Gallery", "Oasis"
            };



        
        public List<Point> Porty = new List<Point>() {
                new Point(629.55, 238.55),//MnF Alley
                new Point(629.55, 238.55),//Wild West
                new Point(629.55, 238.55),//MnF Alley / Mme Olivia`s Private Club
                new Point(629.55, 238.55),//"MnF Alley / Hair Salon
                new Point(629.55, 238.55),//Lagoon Beach
                new Point(629.55, 238.55),//Sacred Mountain
                new Point(629.55, 238.55),//Private Club / Room #1
                new Point(629.55, 238.55),//Private Club / Room #2
                new Point(629.55, 238.55),//Private Club / Room #3
                new Point(629.55, 238.55),//Private Club / Room #4
                new Point(629.55, 238.55),//Wild West / Saloon
                new Point(629.55, 238.55),//Wild West / Motel Lobby
                new Point(629.55, 238.55),//Lagoon Beach / Dr.Boobige`s Clinic
                new Point(629.55, 238.55),//Wild West / Motel / Block A
                new Point(629.55, 238.55),//Wild West / Motel / Block B
                new Point(629.55, 238.55),//Wild West / Motel / Block C
                new Point(629.55, 238.55),//Wild West / Motel / Block D
                new Point(629.55, 238.55),//Wild West / Motel / Block E
                new Point(629.55, 238.55),//Wild West / Motel / Block A / Room
                new Point(629.55, 238.55),//Wild West / Motel / Block B / Room
                new Point(629.55, 238.55),//Wild West / Motel / Block C / Room
                new Point(629.55, 238.55),//Wild West / Motel / Block D / Room
                new Point(629.55, 238.55),//Wild West / Motel / Block E / Room
                new Point(629.55, 238.55),//Sacred Mountain / Sexual Wisdom Temple//
                new Point(629.55, 238.55),//Wild West / Saloon / Cabaret
                new Point(1055, 707),//Meadows Park
                new Point(752, 1090),//Meadows Park / Museum of Sex Art
                new Point(218, 584),//Meadows Park / Museum of Sex Art / Recent Exposition 1
                new Point(208, 600),//Meadows Park / Museum of Sex Art / Recent Exposition 2
                new Point(210, 610),//Meadows Park / Museum of Sex Art / Recent Exposition 3
                new Point(215, 593),//Meadows Park / Museum of Sex Art / Recent Exposition 4
                new Point(230, 515),//Meadows Park / Museum of Sex Art / Weekly Best
                new Point(219, 509),//Meadows Park / Museum of Sex Art / Monthly Best
                new Point(222, 570),//Meadows Park / Museum of Sex Art / All Time Best
                new Point(629.55, 238.55),//Petnis Forest
                new Point(629.55, 238.55),//Petnis Forest / Petnis Shack
                new Point(215.9, 1011.6),//Sacred Mountain / Cave
                new Point(0, 0),//Waterfall//nenasel sem
                new Point(629.55, 238.55),//MnF Metropolis
                new Point(629.55, 238.55),//MnF Metropolis / Hotel Plaza
                new Point(629.55, 238.55),//MnF Metropolis / Hotel Plaza 2nd floor
                new Point(629.55, 238.55),//MnF Metropolis / Hotel Plaza 3nd floor
                new Point(629.55, 238.55),//MnF Metropolis / Hotel Plaza 4nd floor
                new Point(629.55, 238.55),//MnF Metropolis / Hotel Plaza 5nd floor
                new Point(629.55, 238.55),//MnF Metropolis / Hotel Plaza 6nd floor
                new Point(629.55, 238.55),//MnF Metropolis / Hotel Plaza 7th floor(VIP)
                new Point(629.55, 238.55),//MnF Metropolis / Hotel Plaza 2nd floor / Room
                new Point(629.55, 238.55),//MnF Metropolis / Hotel Plaza 3nd floor / Room
                new Point(629.55, 238.55),//MnF Metropolis / Hotel Plaza 4nd floor / Room
                new Point(629.55, 238.55),//MnF Metropolis / Hotel Plaza 5nd floor / Room
                new Point(629.55, 238.55),//MnF Metropolis / Hotel Plaza 6nd floor / Room
                new Point(629.55, 238.55),//MnF Metropolis / Hotel Plaza 7th floor / President Lux
                new Point(629.55, 238.55),//MnF Metropolis / Hotel Plaza 8th floor / President Lux
                new Point(629.55, 238.55),//MnF Metropolis / Hotel Plaza 8th floor(VIP)
                new Point(629.55, 238.55),//Red Heat
                new Point(629.55, 238.55),//Bar Vodka
                new Point(629.55, 238.55),//Shooting Gallery
                new Point(629.55, 238.55)//Oasis
        };
        public List<int> StartID = new List<int>() { 0, 1, 4, 5, 25, 34, 37, 54 };

        public long GetAreaID(string areaName)
        {
            return (long)Jmena.IndexOf(areaName) * 100000000;
        }

        public long GetAreaID(int areaID)
        {
            return (long)areaID * 100000000;
        }
        public string GetAreaName(long AreaID)
        {
            string s = "";//            gotoArea(drop_area_id * 100000000, drop_area_id, WalkPort.TYPE_AREA);
            s = Jmena[(int)(AreaID / 100000000)];

            return s;
        }
    }
}
