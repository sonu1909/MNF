using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows;

namespace MnfPic
{
    public static class MnfArea
    {
        public enum Typy { TYPE_AREA, TYPE_DIALOG, TYPE_BED, TYPE_CLOTHES_FIT, TYPE_BED_SINGLE, TYPE_MAP, TYPE_BED_TRAINING, TYPE_MINIGAME, TYPE_NPC_SEX, TYPE_POLEDANCE }
        public static List<MnfLocation> Lokace = new List<MnfLocation>()
        {
            new MnfLocation("MnF Alley",new Point(629.55, 238.55),100000000),
            new MnfLocation("Wild Westy",new Point(629.55, 238.55),100000001),
            new MnfLocation("MnF Alley / Mme Olivia`s Private Club",new Point(629.55, 238.55),100000002),
            new MnfLocation("MnF Alley / Hair Salon",new Point(629.55, 238.55),100000003),
            new MnfLocation("Lagoon Beach",new Point(629.55, 238.55),100000004),
            new MnfLocation("Sacred Mountain",new Point(629.55, 238.55),100000005),
            new MnfLocation("Private Club / Room #1",new Point(629.55, 238.55),100000006),
            new MnfLocation("Private Club / Room #2",new Point(629.55, 238.55),100000007),
            new MnfLocation("Private Club / Room #3",new Point(629.55, 238.55),100000008),
            new MnfLocation("Private Club / Room #4",new Point(629.55, 238.55),10000009),
            new MnfLocation("Wild West / Saloon",new Point(629.55, 238.55),100000010),
            new MnfLocation("Wild West / Motel Lobby",new Point(629.55, 238.55),100000011),
            new MnfLocation("Lagoon Beach / Dr.Boobige`s Clinic",new Point(629.55, 238.55),100000012),
            new MnfLocation("Wild West / Motel / Block A",new Point(629.55, 238.55),100000013),
            new MnfLocation("Wild West / Motel / Block B",new Point(629.55, 238.55),100000014),
            new MnfLocation("Wild West / Motel / Block C",new Point(629.55, 238.55),100000015),
            new MnfLocation("Wild West / Motel / Block D",new Point(629.55, 238.55),100000016),
            new MnfLocation("Wild West / Motel / Block E",new Point(629.55, 238.55),100000017),
            new MnfLocation("Wild West / Motel / Block A / Room",new Point(629.55, 238.55),100000018),
            new MnfLocation("Wild West / Motel / Block B / Room",new Point(629.55, 238.55),100000019),
            new MnfLocation("Wild West / Motel / Block C / Room",new Point(629.55, 238.55),100000020),
            new MnfLocation("Wild West / Motel / Block D / Room",new Point(629.55, 238.55),100000021),
            new MnfLocation("Wild West / Motel / Block E / Room",new Point(629.55, 238.55),100000022),
            new MnfLocation("Sacred Mountain / Sexual Wisdom Temple",new Point(629.55, 238.55),100000023),
            new MnfLocation("Wild West / Saloon / Cabaret",new Point(629.55, 238.55),100000024),
            new MnfLocation("Meadows Park",new Point(1055, 707),100000025),
            new MnfLocation("Meadows Park / Museum of Sex Art",new Point(752, 1090),100000026),
            new MnfLocation("Meadows Park / Museum of Sex Art / Recent Exposition 1",new Point(218, 584),100000027),
            new MnfLocation("Meadows Park / Museum of Sex Art / Recent Exposition 2",new Point(208, 600),100000028),
            new MnfLocation("Meadows Park / Museum of Sex Art / Recent Exposition 3",new Point(210, 610),100000029),
            new MnfLocation("Meadows Park / Museum of Sex Art / Recent Exposition 4",new Point(215, 593),100000030),
            new MnfLocation("Meadows Park / Museum of Sex Art / Weekly Best",new Point(230, 515),100000031),
            new MnfLocation("Meadows Park / Museum of Sex Art / Monthly Best",new Point(219, 509),100000032),
            new MnfLocation("Meadows Park / Museum of Sex Art / All Time Best",new Point(222, 570),100000033),
            new MnfLocation("Petnis Forest",new Point(629.55, 238.55),100000034),
            new MnfLocation("Petnis Forest / Petnis Shack",new Point(629.55, 238.55),100000035),
            new MnfLocation("Sacred Mountain / Cave",new Point(215.9, 1011.6),100000036),
            new MnfLocation("Waterfall",new Point(629.55, 238.55),100000037),
            new MnfLocation("MnF Metropolis",new Point(629.55, 238.55),100000038),
            new MnfLocation("MnF Metropolis / Hotel Plaza",new Point(629.55, 238.55),100000039),
            new MnfLocation("MnF Metropolis / Hotel Plaza 2nd floor",new Point(629.55, 238.55),100000040),
            new MnfLocation("MnF Metropolis / Hotel Plaza 3rd floor",new Point(629.55, 238.55),100000041),
            new MnfLocation("MnF Metropolis / Hotel Plaza 4rd floor",new Point(629.55, 238.55),100000042),
            new MnfLocation("MnF Metropolis / Hotel Plaza 5rd floor",new Point(629.55, 238.55),100000043),
            new MnfLocation("MnF Metropolis / Hotel Plaza 6rd floor",new Point(629.55, 238.55),100000044),
            new MnfLocation("MnF Metropolis / Hotel Plaza 7th floor(VIP)",new Point(629.55, 238.55),100000045),
            new MnfLocation("MnF Metropolis / Hotel Plaza 2nd floor / Room",new Point(629.55, 238.55),100000046),
            new MnfLocation("MnF Metropolis / Hotel Plaza 3nd floor / Room",new Point(629.55, 238.55),100000047),
            new MnfLocation("MnF Metropolis / Hotel Plaza 4nd floor / Room",new Point(629.55, 238.55),100000048),
            new MnfLocation("MnF Metropolis / Hotel Plaza 5nd floor / Room",new Point(629.55, 238.55),100000049),
            new MnfLocation("MnF Metropolis / Hotel Plaza 6nd floor / Room",new Point(629.55, 238.55),100000050),
            new MnfLocation("MnF Metropolis / Hotel Plaza 7th floor / President Lux",new Point(629.55, 238.55),100000051),
            new MnfLocation("MnF Metropolis / Hotel Plaza 8th floor / President Lux",new Point(629.55, 238.55),100000052),
            new MnfLocation("MnF Metropolis / Hotel Plaza 8th floor(VIP)",new Point(629.55, 238.55),100000053),
            new MnfLocation("Red Heat",new Point(629.55, 238.55),100000054),
            new MnfLocation("Bar Vodka",new Point(629.55, 238.55),100000055),
            new MnfLocation("Shooting Gallery",new Point(629.55, 238.55),100000056),
            new MnfLocation("Oasis",new Point(629.55, 238.55),100000057),
        };
        
        public static List<int> StartID = new List<int>() { 0, 1, 4, 5, 25, 34, 37, 54 };
    }
}
