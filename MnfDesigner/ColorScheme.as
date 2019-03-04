class ColorScheme
{
	var fillColorTransform = undefined;
	var shadowColorTransform = undefined;
	var lineColorTransform = undefined;
	var blickColorTransform = undefined;
   function ColorScheme(r, g, b)
   {
      this.fillColorTransform = {rb:r,gb:g,bb:b,ab:"255",ra:"0",ga:"0",ba:"0",aa:"0"};
      var _loc3_ = int(r - r / 10);
      var _loc4_ = int(g - g / 6);
      var _loc2_ = int(b - b / 6);
      this.shadowColorTransform = {rb:_loc3_,gb:_loc4_,bb:_loc2_,ab:"255",ra:"0",ga:"0",ba:"0",aa:"0"};
      _loc3_ = int(r - 1.5 * r / 3);
      _loc4_ = int(g - 1.5 * g / 2);
      _loc2_ = int(b - 1.5 * b / 2);
      this.lineColorTransform = {rb:_loc3_,gb:_loc4_,bb:_loc2_,ab:"255",ra:"0",ga:"0",ba:"0",aa:"0"};
      _loc3_ = r + 25;
      if(_loc3_ > 255)
      {
         _loc3_ = 255;
      }
      _loc4_ = g + 37;
      if(_loc4_ > 255)
      {
         _loc4_ = 255;
      }
      _loc2_ = b + 25;
      if(_loc2_ > 255)
      {
         _loc2_ = 255;
      }
      this.blickColorTransform = {rb:_loc3_,gb:_loc4_,bb:_loc2_,ab:"0",ra:"0",ga:"0",ba:"0",aa:"100"};
   }
   function PaintSkin(clip)
   {
      if(clip._name == "skin_f")
      {
         var _loc3_ = new Color(clip);
         _loc3_.setTransform(this.fillColorTransform);
         return undefined;
      }
      if(clip._name == "skin_s")
      {
         _loc3_ = new Color(clip);
         _loc3_.setTransform(this.shadowColorTransform);
         return undefined;
      }
      if(clip._name == "skin_l")
      {
         _loc3_ = new Color(clip);
         _loc3_.setTransform(this.lineColorTransform);
         return undefined;
      }
      if(clip._name == "skin_b")
      {
         _loc3_ = new Color(clip);
         _loc3_.setTransform(this.blickColorTransform);
         return undefined;
      }
      _loc3_ = new Color(clip.skin_f);
      _loc3_.setTransform(this.fillColorTransform);
      _loc3_ = new Color(clip.skin_s);
      _loc3_.setTransform(this.shadowColorTransform);
      _loc3_ = new Color(clip.skin_l);
      _loc3_.setTransform(this.lineColorTransform);
      _loc3_ = new Color(clip.skin_b);
      _loc3_.setTransform(this.blickColorTransform);
   }
   function PaintRed(clip)
   {
      if(clip._name == "red_f")
      {
         var _loc3_ = new Color(clip);
         _loc3_.setTransform(this.fillColorTransform);
         return undefined;
      }
      if(clip._name == "red_s")
      {
         _loc3_ = new Color(clip);
         _loc3_.setTransform(this.shadowColorTransform);
         return undefined;
      }
      if(clip._name == "red_l")
      {
         _loc3_ = new Color(clip);
         _loc3_.setTransform(this.lineColorTransform);
         return undefined;
      }
      if(clip._name == "red_b")
      {
         _loc3_ = new Color(clip);
         _loc3_.setTransform(this.blickColorTransform);
         return undefined;
      }
      _loc3_ = new Color(clip.red_f);
      _loc3_.setTransform(this.fillColorTransform);
      _loc3_ = new Color(clip.red_s);
      _loc3_.setTransform(this.shadowColorTransform);
      _loc3_ = new Color(clip.red_l);
      _loc3_.setTransform(this.lineColorTransform);
      _loc3_ = new Color(clip.red_b);
      _loc3_.setTransform(this.blickColorTransform);
   }
   function PaintHair(clip)
   {
      if(clip._name == "hair_f")
      {
         var _loc3_ = new Color(clip);
         _loc3_.setTransform(this.fillColorTransform);
         return undefined;
      }
      if(clip._name == "hair_s")
      {
         _loc3_ = new Color(clip);
         _loc3_.setTransform(this.shadowColorTransform);
         return undefined;
      }
      if(clip._name == "hair_l")
      {
         _loc3_ = new Color(clip);
         _loc3_.setTransform(this.lineColorTransform);
         return undefined;
      }
      if(clip._name == "hair_b")
      {
         _loc3_ = new Color(clip);
         _loc3_.setTransform(this.blickColorTransform);
         return undefined;
      }
      _loc3_ = new Color(clip.hair_f);
      _loc3_.setTransform(this.fillColorTransform);
      _loc3_ = new Color(clip.hair_s);
      _loc3_.setTransform(this.shadowColorTransform);
      _loc3_ = new Color(clip.hair_l);
      _loc3_.setTransform(this.lineColorTransform);
      _loc3_ = new Color(clip.hair_b);
      _loc3_.setTransform(this.blickColorTransform);
   }
   function PaintEye(clip)
   {
      var _loc2_ = new Color(clip.eye_f);
      _loc2_.setTransform(this.fillColorTransform);
      _loc2_ = new Color(clip.eye_s);
      _loc2_.setTransform(this.shadowColorTransform);
      _loc2_ = new Color(clip.eye_l);
      _loc2_.setTransform(this.lineColorTransform);
      _loc2_ = new Color(clip.eye_b);
      _loc2_.setTransform(this.blickColorTransform);
   }
   function PaintCloth(clip)
   {
      var _loc2_ = new Color(clip.cloth_f);
      _loc2_.setTransform(this.fillColorTransform);
      _loc2_ = new Color(clip.cloth_s);
      _loc2_.setTransform(this.shadowColorTransform);
      _loc2_ = new Color(clip.cloth_l);
      _loc2_.setTransform(this.lineColorTransform);
      _loc2_ = new Color(clip.cloth_b);
      _loc2_.setTransform(this.blickColorTransform);
   }
   function PaintCloth2(clip)
   {
      var _loc2_ = new Color(clip.cloth_f2);
      _loc2_.setTransform(this.fillColorTransform);
      _loc2_ = new Color(clip.cloth_s2);
      _loc2_.setTransform(this.shadowColorTransform);
      _loc2_ = new Color(clip.cloth_l2);
      _loc2_.setTransform(this.lineColorTransform);
      _loc2_ = new Color(clip.cloth_b2);
      _loc2_.setTransform(this.blickColorTransform);
   }
}
