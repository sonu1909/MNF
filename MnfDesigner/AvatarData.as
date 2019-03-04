class AvatarData
{
	var name = "";
	var petnis_name = "";
	var sex = 1;
	var sexual_orientation = 2;
	var skin_color = 1;
	var hair_color = 4;
	var eyes_color = 4;
	var cloth_color1 = 1;
	var cloth_color2 = 10;
	var petnis_color = -1;
	var torso = 2;
	var breast = 2;
	var nipples = 2;
	var chest_hair = 2;
	var pubic_hair = 1;
	var hair_style = 1;
	var jaw = 2;
	var ears = 1;
	var nose = 1;
	var lips = 1;
	var eyes = 1;
	var eye_brows = 1;
	var moustaches = 1;
	var beard = 1;
	var outfit = 2;
	var userCT = undefined;
	var exp_gross = 0;
	var exp_available = 0;
	var deleted = 0;
	var premium = 0;
	var hat = 1;
	var hat_color1 = 1;
	var hat_color2 = 10;
	var glasses = 1;
	var glasses_color1 = 1;
	var glasses_color2 = 10;
	var hide_petnis = 0;
	var icon_id = 0;
	var id = 0;
   function AvatarData(randomize, data_str)
   {
      if(randomize)
      {
         this.name = "";
         this.petnis_name = "";
         this.sex = random(2) + 1;
         this.sexual_orientation = random(3) + 1;
         this.skin_color = random(_root.color_manager.skin_colorschemes.length);
         this.hair_color = random(11);
         this.eyes_color = random(_root.color_manager.eye_colorschemes.length);
         this.cloth_color1 = random(_root.color_manager.cloth_colorschemes.length);
         this.cloth_color2 = random(_root.color_manager.cloth_colorschemes.length);
         this.petnis_color = random(_root.color_manager.cloth_colorschemes.length);
         this.torso = random(3) + 1;
         this.breast = random(3) + 1;
         this.nipples = random(3) + 1;
         this.chest_hair = random(2) + 1;
         this.pubic_hair = random(2) + 1;
         this.hair_style = random(8) + 1;
         this.jaw = random(3) + 1;
         this.ears = random(3) + 1;
         this.nose = random(5) + 1;
         this.lips = random(3) + 1;
         this.eyes = random(6) + 1;
         this.eye_brows = random(3) + 1;
         this.moustaches = random(3) + 1;
         this.beard = random(4) + 1;
         this.outfit = random(2) + 2;
         this.userCT = this.GetColorTransformObjectFromString("255/255/255");
         this.exp_gross = 0;
         this.exp_available = 0;
         this.deleted = 0;
         this.premium = 0;
         this.hat = 1;
         this.hat_color1 = random(_root.color_manager.cloth_colorschemes.length);
         this.hat_color2 = random(_root.color_manager.cloth_colorschemes.length);
         this.glasses = 1;
         this.glasses_color1 = random(_root.color_manager.cloth_colorschemes.length);
         this.glasses_color2 = random(_root.color_manager.cloth_colorschemes.length);
         this.hide_petnis = 0;
         this.icon_id = 0;
      }
      else if(data_str == "" || data_str == undefined)
      {
         this.name = "";
         this.petnis_name = "";
         this.sex = 1;
         this.sexual_orientation = 2;
         this.skin_color = 1;
         this.hair_color = 4;
         this.eyes_color = 4;
         this.cloth_color1 = 1;
         this.cloth_color2 = 10;
         this.petnis_color = -1;
         this.torso = 2;
         this.breast = 2;
         this.nipples = 2;
         this.chest_hair = 2;
         this.pubic_hair = 1;
         this.hair_style = 1;
         this.jaw = 2;
         this.ears = 1;
         this.nose = 1;
         this.lips = 1;
         this.eyes = 1;
         this.eye_brows = 1;
         this.moustaches = 1;
         this.beard = 1;
         this.outfit = 2;
         this.userCT = this.GetColorTransformObjectFromString("255/255/255");
         this.exp_gross = 0;
         this.exp_available = 0;
         this.deleted = 0;
         this.premium = 0;
         this.hat = 1;
         this.hat_color1 = 1;
         this.hat_color2 = 10;
         this.glasses = 1;
         this.glasses_color1 = 1;
         this.glasses_color2 = 10;
         this.hide_petnis = 0;
         this.icon_id = 0;
      }
      else
      {
         this.parseDataStr(data_str);
      }
   }
   function getCopy()
   {
      var _loc2_ = new AvatarData(true);
      _loc2_.id = this.id;
      _loc2_.name = this.name;
      _loc2_.petnis_name = this.petnis_name;
      _loc2_.sex = this.sex;
      _loc2_.sexual_orientation = this.sexual_orientation;
      _loc2_.skin_color = this.skin_color;
      _loc2_.hair_color = this.hair_color;
      _loc2_.eyes_color = this.eyes_color;
      _loc2_.cloth_color1 = this.cloth_color1;
      _loc2_.cloth_color2 = this.cloth_color2;
      _loc2_.torso = this.torso;
      _loc2_.breast = this.breast;
      _loc2_.nipples = this.nipples;
      _loc2_.chest_hair = this.chest_hair;
      _loc2_.pubic_hair = this.pubic_hair;
      _loc2_.hair_style = this.hair_style;
      _loc2_.jaw = this.jaw;
      _loc2_.ears = this.ears;
      _loc2_.nose = this.nose;
      _loc2_.lips = this.lips;
      _loc2_.eyes = this.eyes;
      _loc2_.eye_brows = this.eye_brows;
      _loc2_.moustaches = this.moustaches;
      _loc2_.beard = this.beard;
      _loc2_.outfit = this.outfit;
      _loc2_.userCT = this.userCT;
      _loc2_.exp_gross = this.exp_gross;
      _loc2_.exp_available = this.exp_available;
      _loc2_.petnis_color = this.petnis_color;
      _loc2_.deleted = this.deleted;
      _loc2_.premium = this.premium;
      _loc2_.hat = this.hat;
      _loc2_.hat_color1 = this.hat_color1;
      _loc2_.hat_color2 = this.hat_color2;
      _loc2_.glasses = this.glasses;
      _loc2_.glasses_color1 = this.glasses_color1;
      _loc2_.glasses_color2 = this.glasses_color2;
      _loc2_.hide_petnis = this.hide_petnis;
      _loc2_.icon_id = this.icon_id;
      return _loc2_;
   }
   function parseDataStr(data_str)
   {
      var _loc3_ = 0;
      var _loc4_ = undefined;
      var _loc2_ = "";
      _loc4_ = 0;
      while(_loc4_ <= data_str.length)
      {
         if(data_str.charAt(_loc4_) == "," || _loc4_ == data_str.length)
         {
            if(_loc3_ == 0)
            {
               this.id = Number(_loc2_);
            }
            if(_loc3_ == 1)
            {
               this.name = _loc2_;
            }
            else if(_loc3_ == 2)
            {
               this.sex = Number(_loc2_);
            }
            else if(_loc3_ == 3)
            {
               this.sexual_orientation = Number(_loc2_);
            }
            else if(_loc3_ == 4)
            {
               this.skin_color = Number(_loc2_);
            }
            else if(_loc3_ == 5)
            {
               this.hair_color = Number(_loc2_);
            }
            else if(_loc3_ == 6)
            {
               this.eyes_color = Number(_loc2_);
            }
            else if(_loc3_ == 7)
            {
               this.cloth_color1 = Number(_loc2_);
            }
            else if(_loc3_ == 8)
            {
               this.cloth_color2 = Number(_loc2_);
            }
            else if(_loc3_ == 9)
            {
               this.torso = Number(_loc2_);
            }
            else if(_loc3_ == 10)
            {
               this.breast = Number(_loc2_);
            }
            else if(_loc3_ == 11)
            {
               this.nipples = Number(_loc2_);
            }
            else if(_loc3_ == 12)
            {
               this.chest_hair = Number(_loc2_);
            }
            else if(_loc3_ == 13)
            {
               this.pubic_hair = Number(_loc2_);
            }
            else if(_loc3_ == 14)
            {
               this.hair_style = Number(_loc2_);
            }
            else if(_loc3_ == 15)
            {
               this.jaw = Number(_loc2_);
            }
            else if(_loc3_ == 16)
            {
               this.ears = Number(_loc2_);
            }
            else if(_loc3_ == 17)
            {
               this.nose = Number(_loc2_);
            }
            else if(_loc3_ == 18)
            {
               this.lips = Number(_loc2_);
            }
            else if(_loc3_ == 19)
            {
               this.eyes = Number(_loc2_);
            }
            else if(_loc3_ == 20)
            {
               this.eye_brows = Number(_loc2_);
            }
            else if(_loc3_ == 21)
            {
               this.moustaches = Number(_loc2_);
            }
            else if(_loc3_ == 22)
            {
               this.beard = Number(_loc2_);
            }
            else if(_loc3_ == 23)
            {
               this.outfit = Number(_loc2_);
            }
            else if(_loc3_ == 24)
            {
               this.userCT = this.GetColorTransformObjectFromString(_loc2_);
            }
            else if(_loc3_ == 25)
            {
               this.exp_gross = Number(_loc2_);
            }
            else if(_loc3_ == 26)
            {
               this.exp_available = Number(_loc2_);
            }
            else if(_loc3_ == 27)
            {
               this.petnis_color = Number(_loc2_);
            }
            else if(_loc3_ == 28)
            {
               this.petnis_name = _loc2_;
            }
            else if(_loc3_ == 29)
            {
               this.deleted = Number(_loc2_);
            }
            else if(_loc3_ == 30)
            {
               this.premium = Number(_loc2_);
            }
            else if(_loc3_ == 31)
            {
               this.hat = Number(_loc2_);
            }
            else if(_loc3_ == 32)
            {
               this.hat_color1 = Number(_loc2_);
            }
            else if(_loc3_ == 33)
            {
               this.hat_color2 = Number(_loc2_);
            }
            else if(_loc3_ == 34)
            {
               this.glasses = Number(_loc2_);
            }
            else if(_loc3_ == 35)
            {
               this.glasses_color1 = Number(_loc2_);
            }
            else if(_loc3_ == 36)
            {
               this.glasses_color2 = Number(_loc2_);
            }
            else if(_loc3_ == 37)
            {
               this.hide_petnis = Number(_loc2_);
            }
            else if(_loc3_ == 38)
            {
               this.icon_id = Number(_loc2_);
            }
            _loc3_ = _loc3_ + 1;
            _loc2_ = "";
         }
         else
         {
            _loc2_ = _loc2_ + data_str.charAt(_loc4_);
         }
         _loc4_ = _loc4_ + 1;
      }
   }
   function GetColorTransformObjectFromString(str)
   {
      var _loc2_ = str.indexOf("/");
      var _loc3_ = str.lastIndexOf("/");
      var _loc5_ = str.substr(0,_loc2_);
      var _loc6_ = str.substr(_loc2_ + 1,_loc3_ - _loc2_ - 1);
      var _loc4_ = str.substr(_loc3_ + 1,str.length - 1);
      return {rb:int(_loc5_),gb:int(_loc6_),bb:int(_loc4_),ab:"255",ra:"0",ga:"0",ba:"0",aa:"0"};
   }
   function fillLoadVars(vars)
   {
      vars.petnis_name = this.petnis_name;
      vars.id = this.id;
      vars.sex = this.sex;
      vars.sexual_orientation = this.sexual_orientation;
      vars.skin_color = this.skin_color;
      vars.hair_color = this.hair_color;
      vars.eyes_color = this.eyes_color;
      vars.cloth_color1 = this.cloth_color1;
      vars.cloth_color2 = this.cloth_color2;
      vars.torso = this.torso;
      vars.breast = this.breast;
      vars.nipples = this.nipples;
      vars.chest_hair = this.chest_hair;
      vars.pubic_hair = this.pubic_hair;
      vars.hair_style = this.hair_style;
      vars.jaw = this.jaw;
      vars.ears = this.ears;
      vars.nose = this.nose;
      vars.lips = this.lips;
      vars.eyes = this.eyes;
      vars.eye_brows = this.eye_brows;
      vars.moustaches = this.moustaches;
      vars.beard = this.beard;
      vars.outfit = this.outfit;
      vars.userCT = this.userCT;
      vars.exp_gross = this.exp_gross;
      vars.exp_available = this.exp_available;
      vars.petnis_color = this.petnis_color;
      vars.deleted = this.deleted;
      vars.premium = this.premium;
      vars.hat = this.hat;
      vars.hat_color1 = this.hat_color1;
      vars.hat_color2 = this.hat_color2;
      vars.glasses = this.glasses;
      vars.glasses_color1 = this.glasses_color1;
      vars.glasses_color2 = this.glasses_color2;
      vars.hide_petnis = this.hide_petnis;
      vars.icon_id = this.icon_id;
   }
}
