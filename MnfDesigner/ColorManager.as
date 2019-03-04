class ColorManager
{
	var skin_colorschemes = new Array();
	var red_colorschemes = new Array();
	var hair_colorschemes = new Array();
	var eye_colorschemes = new Array();
	var cloth_colorschemes = new Array();
   function ColorManager()
   {
      this.skin_colorschemes = new Array();
      this.skin_colorschemes.push(new ColorScheme(249,221,200));
      this.skin_colorschemes.push(new ColorScheme(240,205,191));
      this.skin_colorschemes.push(new ColorScheme(249,208,183));
      this.skin_colorschemes.push(new ColorScheme(240,183,162));
      this.skin_colorschemes.push(new ColorScheme(242,199,151));
      this.skin_colorschemes.push(new ColorScheme(226,164,137));
      this.skin_colorschemes.push(new ColorScheme(215,151,104));
      this.skin_colorschemes.push(new ColorScheme(150,91,57));
      this.skin_colorschemes.push(new ColorScheme(151,94,72));
      this.skin_colorschemes.push(new ColorScheme(112,61,48));
      this.skin_colorschemes.push(new ColorScheme(125,79,79));
      this.red_colorschemes = new Array();
      var _loc2_ = 0;
      while(_loc2_ < this.skin_colorschemes.length)
      {
         var _loc4_ = this.skin_colorschemes[_loc2_].fillColorTransform.rb;
         var _loc5_ = this.skin_colorschemes[_loc2_].fillColorTransform.gb;
         var _loc3_ = this.skin_colorschemes[_loc2_].fillColorTransform.bb;
         _loc4_ = _loc4_ - 5;
         if(_loc4_ < 0)
         {
            _loc4_ = 0;
         }
         _loc5_ = _loc5_ - 20;
         if(_loc5_ < 0)
         {
            _loc5_ = 0;
         }
         _loc3_ = _loc3_ - 10;
         if(_loc3_ < 0)
         {
            _loc3_ = 0;
         }
         this.red_colorschemes.push(new ColorScheme(_loc4_,_loc5_,_loc3_));
         _loc2_ = _loc2_ + 1;
      }
      this.hair_colorschemes = new Array();
      this.hair_colorschemes.push(new ColorScheme(241,249,250));
      this.hair_colorschemes.push(new ColorScheme(252,243,202));
      this.hair_colorschemes.push(new ColorScheme(255,233,155));
      this.hair_colorschemes.push(new ColorScheme(204,153,102));
      this.hair_colorschemes.push(new ColorScheme(156,72,48));
      this.hair_colorschemes.push(new ColorScheme(223,112,57));
      this.hair_colorschemes.push(new ColorScheme(133,74,52));
      this.hair_colorschemes.push(new ColorScheme(105,33,37));
      this.hair_colorschemes.push(new ColorScheme(66,36,43));
      this.hair_colorschemes.push(new ColorScheme(63,50,33));
      this.hair_colorschemes.push(new ColorScheme(12,58,60));
      this.hair_colorschemes.push(new ColorScheme(102,51,51));
      this.hair_colorschemes.push(new ColorScheme(182,51,48));
      this.hair_colorschemes.push(new ColorScheme(255,204,204));
      this.hair_colorschemes.push(new ColorScheme(102,76,51));
      this.hair_colorschemes.push(new ColorScheme(178,107,35));
      this.hair_colorschemes.push(new ColorScheme(242,145,48));
      this.hair_colorschemes.push(new ColorScheme(255,229,204));
      this.hair_colorschemes.push(new ColorScheme(102,102,51));
      this.hair_colorschemes.push(new ColorScheme(178,178,35));
      this.hair_colorschemes.push(new ColorScheme(245,235,100));
      this.hair_colorschemes.push(new ColorScheme(255,255,204));
      this.hair_colorschemes.push(new ColorScheme(76,102,51));
      this.hair_colorschemes.push(new ColorScheme(107,178,35));
      this.hair_colorschemes.push(new ColorScheme(229,255,204));
      this.hair_colorschemes.push(new ColorScheme(51,102,51));
      this.hair_colorschemes.push(new ColorScheme(43,146,43));
      this.hair_colorschemes.push(new ColorScheme(204,255,204));
      this.hair_colorschemes.push(new ColorScheme(51,102,76));
      this.hair_colorschemes.push(new ColorScheme(35,178,107));
      this.hair_colorschemes.push(new ColorScheme(204,255,229));
      this.hair_colorschemes.push(new ColorScheme(51,102,102));
      this.hair_colorschemes.push(new ColorScheme(35,178,178));
      this.hair_colorschemes.push(new ColorScheme(204,255,255));
      this.hair_colorschemes.push(new ColorScheme(51,76,102));
      this.hair_colorschemes.push(new ColorScheme(35,107,178));
      this.hair_colorschemes.push(new ColorScheme(204,229,255));
      this.hair_colorschemes.push(new ColorScheme(51,51,102));
      this.hair_colorschemes.push(new ColorScheme(61,61,160));
      this.hair_colorschemes.push(new ColorScheme(204,204,255));
      this.hair_colorschemes.push(new ColorScheme(76,51,102));
      this.hair_colorschemes.push(new ColorScheme(102,53,154));
      this.hair_colorschemes.push(new ColorScheme(229,204,255));
      this.hair_colorschemes.push(new ColorScheme(102,51,102));
      this.hair_colorschemes.push(new ColorScheme(158,63,154));
      this.hair_colorschemes.push(new ColorScheme(255,204,255));
      this.hair_colorschemes.push(new ColorScheme(102,51,76));
      this.hair_colorschemes.push(new ColorScheme(178,35,107));
      this.hair_colorschemes.push(new ColorScheme(255,204,229));
      this.hair_colorschemes.push(new ColorScheme(51,53,55));
      this.hair_colorschemes.push(new ColorScheme(123,123,123));
      this.hair_colorschemes.push(new ColorScheme(255,255,255));
      this.eye_colorschemes = new Array();
      this.eye_colorschemes.push(new ColorScheme(146,165,173));
      this.eye_colorschemes.push(new ColorScheme(43,123,213));
      this.eye_colorschemes.push(new ColorScheme(79,169,63));
      this.eye_colorschemes.push(new ColorScheme(157,102,47));
      this.eye_colorschemes.push(new ColorScheme(102,51,0));
      this.cloth_colorschemes = new Array();
      this.cloth_colorschemes.push(new ColorScheme(102,51,51));
      this.cloth_colorschemes.push(new ColorScheme(182,51,48));
      this.cloth_colorschemes.push(new ColorScheme(255,204,204));
      this.cloth_colorschemes.push(new ColorScheme(102,76,51));
      this.cloth_colorschemes.push(new ColorScheme(178,107,35));
      this.cloth_colorschemes.push(new ColorScheme(242,145,48));
      this.cloth_colorschemes.push(new ColorScheme(255,229,204));
      this.cloth_colorschemes.push(new ColorScheme(102,102,51));
      this.cloth_colorschemes.push(new ColorScheme(178,178,35));
      this.cloth_colorschemes.push(new ColorScheme(245,235,100));
      this.cloth_colorschemes.push(new ColorScheme(255,255,204));
      this.cloth_colorschemes.push(new ColorScheme(76,102,51));
      this.cloth_colorschemes.push(new ColorScheme(107,178,35));
      this.cloth_colorschemes.push(new ColorScheme(229,255,204));
      this.cloth_colorschemes.push(new ColorScheme(51,102,51));
      this.cloth_colorschemes.push(new ColorScheme(43,146,43));
      this.cloth_colorschemes.push(new ColorScheme(204,255,204));
      this.cloth_colorschemes.push(new ColorScheme(51,102,76));
      this.cloth_colorschemes.push(new ColorScheme(35,178,107));
      this.cloth_colorschemes.push(new ColorScheme(204,255,229));
      this.cloth_colorschemes.push(new ColorScheme(51,102,102));
      this.cloth_colorschemes.push(new ColorScheme(35,178,178));
      this.cloth_colorschemes.push(new ColorScheme(204,255,255));
      this.cloth_colorschemes.push(new ColorScheme(51,76,102));
      this.cloth_colorschemes.push(new ColorScheme(35,107,178));
      this.cloth_colorschemes.push(new ColorScheme(204,229,255));
      this.cloth_colorschemes.push(new ColorScheme(51,51,102));
      this.cloth_colorschemes.push(new ColorScheme(61,61,160));
      this.cloth_colorschemes.push(new ColorScheme(204,204,255));
      this.cloth_colorschemes.push(new ColorScheme(76,51,102));
      this.cloth_colorschemes.push(new ColorScheme(102,53,154));
      this.cloth_colorschemes.push(new ColorScheme(229,204,255));
      this.cloth_colorschemes.push(new ColorScheme(102,51,102));
      this.cloth_colorschemes.push(new ColorScheme(158,63,154));
      this.cloth_colorschemes.push(new ColorScheme(255,204,255));
      this.cloth_colorschemes.push(new ColorScheme(102,51,76));
      this.cloth_colorschemes.push(new ColorScheme(178,35,107));
      this.cloth_colorschemes.push(new ColorScheme(255,204,229));
      this.cloth_colorschemes.push(new ColorScheme(51,53,55));
      this.cloth_colorschemes.push(new ColorScheme(123,123,123));
      this.cloth_colorschemes.push(new ColorScheme(255,255,255));
   }
   function setSkinColor(part, color_id)
   {
      this.skin_colorschemes[color_id].PaintSkin(part);
   }
   function setRedColor(part, color_id)
   {
      this.red_colorschemes[color_id].PaintRed(part);
   }
   function setHairColor(part, color_id)
   {
      this.hair_colorschemes[color_id].PaintHair(part);
   }
   function setEyeColor(part, color_id)
   {
      this.eye_colorschemes[color_id].PaintEye(part);
   }
   function setClothColor(part, color_id)
   {
      this.cloth_colorschemes[color_id].PaintCloth(part);
   }
   function setClothColor2(part, color_id)
   {
      this.cloth_colorschemes[color_id].PaintCloth2(part);
   }
   function paintSkinClips(clips, color_id)
   {
      var _loc2_ = 0;
      while(_loc2_ < clips.length)
      {
         this.setSkinColor(clips[_loc2_],color_id);
         _loc2_ = _loc2_ + 1;
      }
   }
   function paintRedClips(clips, color_id)
   {
      var _loc2_ = 0;
      while(_loc2_ < clips.length)
      {
         this.setRedColor(clips[_loc2_],color_id);
         _loc2_ = _loc2_ + 1;
      }
   }
   function paintHairClips(clips, color_id)
   {
      var _loc2_ = 0;
      while(_loc2_ < clips.length)
      {
         this.setHairColor(clips[_loc2_],color_id);
         _loc2_ = _loc2_ + 1;
      }
   }
   function paintEyeClips(clips, color_id)
   {
      var _loc2_ = 0;
      while(_loc2_ < clips.length)
      {
         this.setEyeColor(clips[_loc2_],color_id);
         _loc2_ = _loc2_ + 1;
      }
   }
   function paintClothClips1(clips, color_id)
   {
      var _loc2_ = 0;
      while(_loc2_ < clips.length)
      {
         this.setClothColor(clips[_loc2_],color_id);
         _loc2_ = _loc2_ + 1;
      }
   }
   function paintClothClips2(clips, color_id)
   {
      var _loc2_ = 0;
      while(_loc2_ < clips.length)
      {
         this.setClothColor2(clips[_loc2_],color_id);
         _loc2_ = _loc2_ + 1;
      }
   }
   function getRandomUserCT()
   {
      var _loc10_ = random(256) / 256;
      var _loc8_ = 0.3;
      var _loc1_ = 0.99;
      var _loc6_ = int(_loc10_ * 6);
      var _loc11_ = _loc10_ * 6 - _loc6_;
      var _loc5_ = _loc1_ * (1 - _loc8_);
      var _loc9_ = _loc1_ * (1 - _loc11_ * _loc8_);
      var _loc7_ = _loc1_ * (1 - (1 - _loc11_) * _loc8_);
      var _loc3_ = undefined;
      var _loc4_ = undefined;
      var _loc2_ = undefined;
      if(_loc6_ == 0)
      {
         _loc3_ = _loc1_;
         _loc4_ = _loc7_;
         _loc2_ = _loc5_;
      }
      else if(_loc6_ == 1)
      {
         _loc3_ = _loc9_;
         _loc4_ = _loc1_;
         _loc2_ = _loc5_;
      }
      else if(_loc6_ == 2)
      {
         _loc3_ = _loc5_;
         _loc4_ = _loc1_;
         _loc2_ = _loc7_;
      }
      else if(_loc6_ == 3)
      {
         _loc3_ = _loc5_;
         _loc4_ = _loc9_;
         _loc2_ = _loc1_;
      }
      else if(_loc6_ == 4)
      {
         _loc3_ = _loc7_;
         _loc4_ = _loc5_;
         _loc2_ = _loc1_;
      }
      else
      {
         _loc3_ = _loc1_;
         _loc4_ = _loc5_;
         _loc2_ = _loc9_;
      }
      _loc3_ = int(255 * _loc3_);
      _loc4_ = int(255 * _loc4_);
      _loc2_ = int(255 * _loc2_);
      return {rb:_loc3_,gb:_loc4_,bb:_loc2_,ab:"0",ra:"0",ga:"0",ba:"0",aa:"100"};
   }
   function SetColorsSlave(mc)
   {
      this.setSkinColor(mc,_root.data_slave.skin_color);
      this.setRedColor(mc,_root.data_slave.skin_color);
      this.setHairColor(mc,_root.data_slave.hair_color);
      this.setEyeColor(mc,_root.data_slave.eyes_color);
   }
   function SetColorsSlave2(mc)
   {
      this.setSkinColor(mc,_root.data_slave2.skin_color);
      this.setRedColor(mc,_root.data_slave2.skin_color);
      this.setHairColor(mc,_root.data_slave2.hair_color);
      this.setEyeColor(mc,_root.data_slave2.eyes_color);
   }
   function SetColorsMaster(mc)
   {
      this.setSkinColor(mc,_root.data_master.skin_color);
      this.setRedColor(mc,_root.data_master.skin_color);
      this.setHairColor(mc,_root.data_master.hair_color);
      this.setEyeColor(mc,_root.data_master.eyes_color);
   }
   function SetColorsMaster2(mc)
   {
      this.setSkinColor(mc,_root.data_master2.skin_color);
      this.setRedColor(mc,_root.data_master2.skin_color);
      this.setHairColor(mc,_root.data_master2.hair_color);
      this.setEyeColor(mc,_root.data_master2.eyes_color);
   }
   function SetColorsMasterPlus(mc)
   {
      this.setSkinColor(mc,_root.data_master.skin_color);
      this.setRedColor(mc,_root.data_master.skin_color);
      this.setHairColor(mc,_root.data_master.hair_color);
      this.setEyeColor(mc,_root.data_master.eyes_color);
      this.setClothColor(mc,_root.data_master.cloth_color1);
      this.setClothColor2(mc,_root.data_master.cloth_color2);
   }
   function SetColorsPetnis(mc)
   {
      this.setClothColor(mc,_root.data_master.petnis_color);
   }
   function SetColorsByAvatarData(mc, data)
   {
      this.setSkinColor(mc,data.skin_color);
      this.setRedColor(mc,data.skin_color);
      this.setHairColor(mc,data.hair_color);
      this.setEyeColor(mc,data.eyes_color);
   }
}
