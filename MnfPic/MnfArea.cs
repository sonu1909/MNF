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
        //area_names = new Array("MnF Alley","Wild West","MnF Alley / Mme Olivia`s Private Club","MnF Alley / Hair Salon","Lagoon Beach","Sacred Mountain","Private Club / Room #1","Private Club / Room #2","Private Club / Room #3","Private Club / Room #4","Wild West / Saloon","Wild West / Motel Lobby","Lagoon Beach / Dr.Boobige`s Clinic","Wild West / Motel / Block A","Wild West / Motel / Block B","Wild West / Motel / Block C","Wild West / Motel / Block D","Wild West / Motel / Block E","Wild West / Motel / Block A / Room","Wild West / Motel / Block B / Room","Wild West / Motel / Block C / Room","Wild West / Motel / Block D / Room","Wild West / Motel / Block E / Room","Sacred Mountain / Sexual Wisdom Temple","Wild West / Saloon / Cabaret","Meadows Park","Meadows Park / Museum of Sex Art","Meadows Park / Museum of Sex Art / Recent Exposition 1","Meadows Park / Museum of Sex Art / Recent Exposition 2","Meadows Park / Museum of Sex Art / Recent Exposition 3","Meadows Park / Museum of Sex Art / Recent Exposition 4","Meadows Park / Museum of Sex Art / Weekly Best","Meadows Park / Museum of Sex Art / Monthly Best","Meadows Park / Museum of Sex Art / All Time Best","Petnis Forest","Petnis Forest / Petnis Shack","Sacred Mountain / Cave","Waterfall","MnF Metropolis","MnF Metropolis / Hotel Plaza","MnF Metropolis / Hotel Plaza 2nd floor","MnF Metropolis / Hotel Plaza 3rd floor","MnF Metropolis / Hotel Plaza 4th floor","MnF Metropolis / Hotel Plaza 5th floor","MnF Metropolis / Hotel Plaza 6th floor","MnF Metropolis / Hotel Plaza 7th floor(VIP)","MnF Metropolis / Hotel Plaza 2nd floor / Room","MnF Metropolis / Hotel Plaza 3rd floor / Room","MnF Metropolis / Hotel Plaza 4th floor / Room","MnF Metropolis / Hotel Plaza 5th floor / Room","MnF Metropolis / Hotel Plaza 6th floor / Room","MnF Metropolis / Hotel Plaza 7th floor / President Lux","MnF Metropolis / Hotel Plaza 8th floor / President Lux","MnF Metropolis / Hotel Plaza 8th floor(VIP)","Red Heat","Red Heat / Bar Vodka","Shooting Gallery","Oasis","Oasis / Baths","Oasis / Casino","Southern MnF","Southern MnF / Jail","Southern MnF / MNF National Bank","Shower bed manager - virtual location","Southern MnF / Apartments","END AREA NAMES");
        //var drop_area_ids = new Array(0, 1, 4, 5, 25, 34, 37, 38, 54, 57, 60);

        public enum Typy { TYPE_AREA, TYPE_DIALOG, TYPE_BED, TYPE_CLOTHES_FIT, TYPE_BED_SINGLE, TYPE_MAP, TYPE_BED_TRAINING, TYPE_MINIGAME, TYPE_NPC_SEX, TYPE_POLEDANCE, TYPE_SHOWER_SEX }
        public static List<MnfLocation> Lokace = new List<MnfLocation>()
        {                                                         
            new MnfLocation("MnF Alley",new Point(407.8,247.95),000000000),//0
            new MnfLocation("Wild West",new Point(341.05,297.1),100000000),//1
            new MnfLocation("MnF Alley / Mme Olivia`s Private Club",new Point(317.95,191.35),200000000),//2
            new MnfLocation("Lagoon Beach",new Point(650,480),new Point(300,200),400000000),//3
            new MnfLocation("Sacred Mountain",new Point(356.2,1146.3),500000000),//4
            new MnfLocation("Wild West / Saloon",new Point(787.2,748.2),1000000000),//5
            new MnfLocation("Wild West / Motel Lobby",new Point(436.1,466.1),1100000000),//6
            new MnfLocation("Wild West / Motel / Block A",new Point(1306.3,490.9),1300000000),//7
            new MnfLocation("Wild West / Motel / Block B",new Point(1306.3,490.9),1400000000),//8
            new MnfLocation("Wild West / Motel / Block C",new Point(1306.3,490.9),1500000000),//9
            new MnfLocation("Wild West / Motel / Block D",new Point(1306.3,490.9),1600000000),//10
            new MnfLocation("Wild West / Motel / Block E",new Point(1306.3,490.9),1700000000),//11
            new MnfLocation("Wild West / Motel / Block A / Room",new Point(520.95,361),1800000000),//12
            new MnfLocation("Wild West / Motel / Block B / Room",new Point(520.95,361),1900000000),//13
            new MnfLocation("Wild West / Motel / Block C / Room",new Point(520.95,361),2000000000),//14
            new MnfLocation("Wild West / Motel / Block D / Room",new Point(520.95,361),2100000000),//15
            new MnfLocation("Wild West / Motel / Block E / Room",new Point(520.95,361),2200000000),//16
            new MnfLocation("Sacred Mountain / Sexual Wisdom Temple",new Point(616.15,823.2),2300000000),//17
            new MnfLocation("Wild West / Saloon / Cabaret",new Point(1068,704.2),2400000000),//18
            new MnfLocation("Meadows Park",new Point(736.15,668.7),2500000000),//19
            new MnfLocation("Meadows Park / Museum of Sex Art",new Point(613.9,1063.5),2600000000),//20
            new MnfLocation("Meadows Park / Museum of Sex Art / Recent Exposition 1",new Point(165.25,498.4),2700000000),//21
            new MnfLocation("Meadows Park / Museum of Sex Art / Recent Exposition 2",new Point(165.25,498.4),2800000000),//22
            new MnfLocation("Meadows Park / Museum of Sex Art / Recent Exposition 3",new Point(165.25,498.4),2900000000),//23
            new MnfLocation("Meadows Park / Museum of Sex Art / Recent Exposition 4",new Point(165.25,498.4),3000000000),//24
            new MnfLocation("Meadows Park / Museum of Sex Art / Weekly Best",new Point(165.25,498.4),3100000000),//25
            new MnfLocation("Meadows Park / Museum of Sex Art / Monthly Best",new Point(165.25,498.4),3200000000),//26
            new MnfLocation("Meadows Park / Museum of Sex Art / All Time Best",new Point(165.25,498.4),3300000000),//27
            new MnfLocation("Petnis Forest",new Point(142.15,616.6),3400000000),//28
            new MnfLocation("Sacred Mountain / Cave",new Point(185.55,102.55),3600000000),//29
            new MnfLocation("Waterfall",new Point(1068,704.2),3700000000),//30
            new MnfLocation("MnF Metropolis",new Point(404.7,406.1),3800000000),//31
            new MnfLocation("MnF Metropolis / Hotel Plaza",new Point(410.1,723.15),3900000000),//32
            new MnfLocation("MnF Metropolis / Hotel Plaza 2nd floor",new Point(153.3,520.15),4000000000),//33
            new MnfLocation("MnF Metropolis / Hotel Plaza 3rd floor",new Point(153.3,520.15),4100000000),//34
            new MnfLocation("MnF Metropolis / Hotel Plaza 4rd floor",new Point(153.3,520.15),4200000000),//35
            new MnfLocation("MnF Metropolis / Hotel Plaza 5rd floor",new Point(153.3,520.15),4300000000),//36
            new MnfLocation("MnF Metropolis / Hotel Plaza 6rd floor",new Point(153.3,520.15),4400000000),//37
            new MnfLocation("MnF Metropolis / Hotel Plaza 7th floor(VIP)",new Point(153.3,520.15),4500000000),//38
            new MnfLocation("MnF Metropolis / Hotel Plaza 2nd floor / Room",new Point(793.75,364),4600000000),//39
            new MnfLocation("MnF Metropolis / Hotel Plaza 3nd floor / Room",new Point(793.75,364),4700000000),//40
            new MnfLocation("MnF Metropolis / Hotel Plaza 4nd floor / Room",new Point(793.75,364),4800000000),//41
            new MnfLocation("MnF Metropolis / Hotel Plaza 5nd floor / Room",new Point(793.75,364),4900000000),//42
            new MnfLocation("MnF Metropolis / Hotel Plaza 6nd floor / Room",new Point(793.75,364),5000000000),//43
            new MnfLocation("MnF Metropolis / Hotel Plaza 7th floor / President Lux",new Point(1141.55,460.25),5100000000),//44
            new MnfLocation("MnF Metropolis / Hotel Plaza 8th floor / President Lux",new Point(1141.55,460.25),5200000000),//45
            new MnfLocation("MnF Metropolis / Hotel Plaza 8th floor(VIP)",new Point(153.3,520.15),5300000000),//46
            new MnfLocation("Red Heat",new Point(599.9,1117),5400000000),//47
            new MnfLocation("Red Heat / Bar Vodka",new Point(1059.8,606.05),5500000000),//48
            new MnfLocation("Shooting Gallery",new Point(606.7,980.8),5600000000),//49
            new MnfLocation("Oasis",new Point(648.9,900.35),5700000000),//50
            new MnfLocation("Oasis / Baths",new Point(475.2,784.45),5800000000),//51
            new MnfLocation("Oasis / Casino",new Point(437.35,872.5),5900000000),//52
            new MnfLocation("Southern MnF",new Point(740.8,452.9),6000000000),//53
            new MnfLocation("Southern MnF / Jail",new Point(519.15,953.25),6100000000),//54
            new MnfLocation("Southern MnF / MNF National Bank",new Point(416.2,726),6200000000),//55
            new MnfLocation("Southern MnF / Apartments",new Point(339.1,477.7),6400000000),//56
            new MnfLocation("Suburbs",new Point(126,351.7),6500000000),//57
            new MnfLocation("2 Bedroom House",new Point(402.1,482.7),6600000000),//58
        };
        public static List<MnfLocation> LokaceS = new List<MnfLocation>()
        {
            new MnfLocation("MnF Alley / Hair Salon",new Point(629.55, 238.55),300000000),//portplace?
            new MnfLocation("Private Club / Room #1",new Point(629.55, 238.55),600000000),//portplace?
            new MnfLocation("Private Club / Room #2",new Point(629.55, 238.55),700000000),//portplace?
            new MnfLocation("Private Club / Room #3",new Point(629.55, 238.55),800000000),//portplace?
            new MnfLocation("Private Club / Room #4",new Point(629.55, 238.55),900000000),//portplace?
            new MnfLocation("Lagoon Beach / Dr.Boobige`s Clinic",new Point(629.55, 238.55),1200000000),//portplace?
            new MnfLocation("Petnis Forest / Petnis Shack",new Point(629.55, 238.55),3500000000),//portplace?
            new MnfLocation("Shower bed manager - virtual location",new Point(629.55, 238.55),6300000000),//portplace?
        };
        public static List<int> StartID = new List<int>() { 0, 1, 3, 4, 19, 28, 30, 31, 47, 50, 53};
    }
}
