using App;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace MnfPic
{
    public class Postava:ANotify
    {
        public int AvatarID { get; set; }
        public string Name { get; set; }
        public string petnis_name { get; set; }
        public int sex { get; set; }
        public int sexual_orientation { get; set; }
        public int skin_color { get; set; }
        public int hair_color { get; set; }
        public int eyes_color { get; set; }
        public int cloth_color1 { get; set; }
        public int cloth_color2 { get; set; }
        public int petnis_color { get; set; }
        public int torso { get; set; }
        public int breast { get; set; }
        public int nipples { get; set; }
        public int chest_hair { get; set; }
        public int pubic_hair { get; set; }
        public int hair_style { get; set; }
        public int jaw { get; set; }
        public int ears { get; set; }
        public int nose { get; set; }
        public int lips { get; set; }
        public int eyes { get; set; }
        public int eye_brows { get; set; }
        public int moustaches { get; set; }
        public int beard { get; set; }
        public int outfit { get; set; }
        public string userCT { get; set; }
        public int exp_gross { get; set; }
        public int exp_available { get; set; }
        public int deleted { get; set; }
        public int premium { get; set; }
        public int hat { get; set; }
        public int hat_color1 { get; set; }
        public int hat_color2 { get; set; }
        public int glasses { get; set; }
        public int glasses_color1 { get; set; }
        public int glasses_color2 { get; set; }
        public int hide_petnis { get; set; }
        public int novaHodnota { get; set; }
    }
   public class ParserMnfAvatar: Postava
    {

        public int PoziceX { get; set; }
        public int PoziceY { get; set; }

        /// <summary>
        /// avatar data="8781987,edgomez,1,2,10,8,4,40,1,3,2,3,1,1,5,3,1,1,1,4,3,2,4,2,218/252/176,0,0,-1,,0,0,1,0,0,1,0,0,0,1" points="1126,743" />
        /// </summary>
        /// <param name="s"></param>
        public ParserMnfAvatar(string s)
        {
            try
            {
                string[] ss = s.Split(' ');
                int i = 0;
                string[] data = ss[1].Replace("\"", "").Split(',');
                AvatarID = int.Parse(data[i++].Split('=')[1]);
                Name = data[i++];
                sex = int.Parse(data[i++]);
                sexual_orientation = int.Parse(data[i++]);
                skin_color = int.Parse(data[i++]);
                hair_color = int.Parse(data[i++]);
                eyes_color = int.Parse(data[i++]);
                cloth_color1 = int.Parse(data[i++]);
                cloth_color2 = int.Parse(data[i++]);
                torso = int.Parse(data[i++]);
                breast = int.Parse(data[i++]);
                nipples = int.Parse(data[i++]);
                chest_hair = int.Parse(data[i++]);
                pubic_hair = int.Parse(data[i++]);
                hair_style = int.Parse(data[i++]);
                jaw = int.Parse(data[i++]);
                ears = int.Parse(data[i++]);
                nose = int.Parse(data[i++]);
                lips = int.Parse(data[i++]);
                eyes = int.Parse(data[i++]);
                eye_brows = int.Parse(data[i++]);
                moustaches = int.Parse(data[i++]);
                beard = int.Parse(data[i++]);
                outfit = int.Parse(data[i++]);
                userCT = data[i++];
                exp_gross = int.Parse(data[i++]);
                exp_available = int.Parse(data[i++]);
                petnis_color = int.Parse(data[i++]);
                petnis_name = data[i++];
                deleted = int.Parse(data[i++]);
                premium = int.Parse(data[i++]);
                hat = int.Parse(data[i++]);
                hat_color1 = int.Parse(data[i++]);
                hat_color2 = int.Parse(data[i++]);
                glasses = int.Parse(data[i++]);
                glasses_color1 = int.Parse(data[i++]);
                glasses_color2 = int.Parse(data[i++]);
                hide_petnis = int.Parse(data[i++]);
                novaHodnota = int.Parse(data[i++]);

                string[] pozice = ss[2].Replace("\"", "").Split(',');
                PoziceX = int.Parse(pozice[0].Split('=')[1]);
                PoziceY = int.Parse(pozice[1]);
            }
            catch (Exception e)
            {
                Console.WriteLine(e);
            }
    }
    }
}
