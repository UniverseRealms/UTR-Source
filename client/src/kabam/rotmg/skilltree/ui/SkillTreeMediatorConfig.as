package kabam.rotmg.skilltree.ui
{


import com.company.assembleegameclient.ui.panels.mediators.SkillTreeMediator;

import kabam.rotmg.skilltree.signal.SkillTreeSignal;

import org.swiftsuspenders.Injector;

import robotlegs.bender.extensions.mediatorMap.api.IMediatorMap;
import robotlegs.bender.framework.api.IConfig;

public class SkillTreeMediatorConfig implements IConfig
   {
       
      
      [Inject]
      public var injector:Injector;
      
      [Inject]
      public var mediatorMap:IMediatorMap;
      
      public function SkillTreeMediatorConfig()
      {
         super();
      }
      
      public function configure() : void
      {
         this.injector.map(SkillTreeSignal).asSingleton();
         this.mediatorMap.map(SkillTreeWindow).toMediator(SkillTreeMediator);
      }
   }
}
