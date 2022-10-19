package {
import com.company.assembleegameclient.map.Camera;
import com.company.assembleegameclient.parameters.Parameters;
import com.company.assembleegameclient.ui.panels.mediators.SkillTreeMediator;
import com.company.assembleegameclient.util.AssetLoader;
import com.company.assembleegameclient.util.StageProxy;

import flash.display.LoaderInfo;
import flash.display.Sprite;
import flash.display.Stage;
import flash.display.StageScaleMode;
import flash.events.Event;

import kabam.lib.net.NetConfig;
import kabam.rotmg.account.AccountConfig;
import kabam.rotmg.appengine.AppEngineConfig;
import kabam.rotmg.application.ApplicationConfig;
import kabam.rotmg.assets.AssetsConfig;
import kabam.rotmg.build.BuildConfig;
import kabam.rotmg.build.impl.BuildEnvironments;
import kabam.rotmg.characters.CharactersConfig;
import kabam.rotmg.classes.ClassesConfig;
import kabam.rotmg.core.CoreConfig;
import kabam.rotmg.core.StaticInjectorContext;
import kabam.rotmg.dailyLogin.config.DailyLoginConfig;
import kabam.rotmg.death.DeathConfig;
import kabam.rotmg.dialogs.DialogsConfig;
import kabam.rotmg.editor.EditorConfig;
import kabam.rotmg.errors.ErrorConfig;
import kabam.rotmg.external.ExternalConfig;
import kabam.rotmg.fame.FameConfig;
import kabam.rotmg.game.GameConfig;
import kabam.rotmg.language.LanguageConfig;
import kabam.rotmg.legends.LegendsConfig;
import kabam.rotmg.maploading.MapLoadingConfig;
import kabam.rotmg.market.ui.MarketMediatorConfig;
import kabam.rotmg.minimap.MiniMapConfig;
import kabam.rotmg.news.NewsConfig;
import kabam.rotmg.packages.PackageConfig;
import kabam.rotmg.pets.PetsConfig;
import kabam.rotmg.promotions.PromotionsConfig;
import kabam.rotmg.queue.QueueConfig;
import kabam.rotmg.servers.ServersConfig;
import kabam.rotmg.skilltree.ui.SkillTreeMediatorConfig;
import kabam.rotmg.stage3D.Stage3DConfig;
import kabam.rotmg.startup.StartupConfig;
import kabam.rotmg.startup.control.StartupSignal;
import kabam.rotmg.text.TextConfig;
import kabam.rotmg.tooltips.TooltipsConfig;
import kabam.rotmg.ui.UIConfig;
import kabam.rotmg.ui.UIUtils;

import robotlegs.bender.bundles.mvcs.MVCSBundle;
import robotlegs.bender.extensions.signalCommandMap.SignalCommandMapExtension;
import robotlegs.bender.framework.api.IContext;
import robotlegs.bender.framework.api.LogLevel;

[SWF(frameRate="60", backgroundColor="#000000", width="800", height="600")]
public class WebMain extends Sprite {
    public static var TOKEN:String;
    public static var ENV:String;
    public static var STAGE:Stage;
    public static var sWidth:Number = 800;
    public static var sHeight:Number = 600;

    protected var context:IContext;

    public function WebMain() {
        if (stage) {
            stage.addEventListener(Event.RESIZE, this.onStageResize);
            this.setup();
        } else {
            addEventListener(Event.ADDED_TO_STAGE, this.onAddedToStage);
        }
    }

    public function onStageResize(e:Event) : void {
        if (stage.scaleMode == StageScaleMode.NO_SCALE) {
            this.scaleX = stage.stageWidth / 800;
            this.scaleY = stage.stageHeight / 600;
            this.x = (800 - stage.stageWidth) / 2;
            this.y = (600 - stage.stageHeight) / 2;
        } else {
            this.scaleX = 1;
            this.scaleY = 1;
            this.x = 0;
            this.y = 0;
        }
        sWidth = stage.stageWidth;
        sHeight = stage.stageHeight;
        Camera.resizeCamera();
        Stage3DConfig.Dimensions();
    }

    private function onAddedToStage(_arg1:Event):void {
        removeEventListener(Event.ADDED_TO_STAGE, this.onAddedToStage);
        this.setup();
    }

    private function setup():void {
            this.setEnvironment();
            this.createContext();
            new AssetLoader().load();
            stage.scaleMode = StageScaleMode.EXACT_FIT;
            this.context.injector.getInstance(StartupSignal).dispatch();
            STAGE = stage;
            UIUtils.toggleQuality(Parameters.data_.uiQuality);
            stage.frameRate = int(Parameters.data_.fpsMode);
    }
    
    private function setEnvironment():void {
            ENV = BuildEnvironments.PRODUCTION;
    }

    private function createContext():void {
        this.context = new StaticInjectorContext();
        this.context.injector.map(LoaderInfo).toValue(root.stage.root.loaderInfo);
        var proxy:StageProxy = new StageProxy(this);
        this.context.injector.map(StageProxy).toValue(proxy);
        this.context
                .extend(MVCSBundle)
                .extend(SignalCommandMapExtension)
                .configure(BuildConfig)
                .configure(StartupConfig)
                .configure(NetConfig)
                .configure(DialogsConfig)
                .configure(ApplicationConfig)
                .configure(LanguageConfig)
                .configure(TextConfig)
                .configure(AppEngineConfig)
                .configure(AccountConfig)
                .configure(ErrorConfig)
                .configure(CoreConfig)
                .configure(AssetsConfig)
                .configure(DeathConfig)
                .configure(CharactersConfig)
                .configure(ServersConfig)
                .configure(GameConfig)
                .configure(EditorConfig)
                .configure(UIConfig)
                .configure(MiniMapConfig)
                .configure(LegendsConfig)
                .configure(NewsConfig)
                .configure(FameConfig)
                .configure(TooltipsConfig)
                .configure(PromotionsConfig)
                .configure(MapLoadingConfig)
                .configure(SkillTreeMediatorConfig)
                .configure(MarketMediatorConfig)
                .configure(ClassesConfig)
                .configure(PackageConfig)
                .configure(PetsConfig)
                .configure(DailyLoginConfig)
                .configure(Stage3DConfig)
                .configure(ExternalConfig)
                .configure(QueueConfig)
                .configure(this);
        this.context.logLevel = LogLevel.DEBUG;
    }
}
}
