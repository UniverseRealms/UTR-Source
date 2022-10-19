package com.company.assembleegameclient.screens
{
import com.company.assembleegameclient.ui.DeprecatedClickableText;
import com.company.assembleegameclient.ui.Scrollbar;
import flash.display.DisplayObject;
import flash.display.Shape;
import flash.display.Sprite;
import flash.events.Event;
import flash.events.MouseEvent;
import flash.filters.DropShadowFilter;
import flash.geom.Rectangle;
import flash.text.TextFieldAutoSize;
import kabam.rotmg.core.model.PlayerModel;
import kabam.rotmg.game.view.CreditDisplay;
import kabam.rotmg.news.view.NewsView;
import kabam.rotmg.packages.view.PackageButton;
import kabam.rotmg.promotions.view.BeginnersPackageButton;
import kabam.rotmg.text.model.TextKey;
import kabam.rotmg.text.view.TextFieldDisplayConcrete;
import kabam.rotmg.text.view.stringBuilder.LineBuilder;
import kabam.rotmg.text.view.stringBuilder.StaticStringBuilder;
import kabam.rotmg.ui.view.ButtonFactory;
import kabam.rotmg.ui.view.components.MenuOptionsBar;
import kabam.rotmg.ui.view.components.ScreenBase;
import org.osflash.signals.Signal;

public class CharacterSelectionAndNewsScreen extends Sprite
{

    private static const NEWS_X:int = 475;

    private static const TAB_UNSELECTED:uint = 11776947;

    private static const TAB_SELECTED:uint = 16777215;


    private const SCROLLBAR_REQUIREMENT_HEIGHT:Number = 400;

    private const CHARACTER_LIST_Y_POS:int = 70;

    private const CHARACTER_LIST_X_POS:int = 200;

    private const DROP_SHADOW:DropShadowFilter = new DropShadowFilter(0,0,0,1,8,8);

    private const butclicked:Signal = new Signal();

    public var close:Signal;

    public var showClasses:Signal;

    public var newCharacter:Signal;

    public var chooseName:Signal;

    public var playGame:Signal;

    public var beginnersPackageButton:BeginnersPackageButton;

    private var model:PlayerModel;

    private var isInitialized:Boolean;

    private var nameText:TextFieldDisplayConcrete;

    private var nameChooseLink_:DeprecatedClickableText;

    private var creditDisplay:CreditDisplay;

    private var openCharactersText:TextFieldDisplayConcrete;

    private var openGraveyardText:TextFieldDisplayConcrete;

    private var newsText:TextFieldDisplayConcrete;

    private var characterList:CharacterList;

    private var characterListType:int = 1;

    private var characterListHeight:Number;

    private var lines:Shape;

    private var scrollBar:Scrollbar;

    private var packageButton:PackageButton;

    private var playButton:TitleMenuOption;

    private var classesButton:TitleMenuOption;

    private var backButton:TitleMenuOption;

    private var menuOptionsBar:MenuOptionsBar;

    private var BOUNDARY_LINE_ONE_Y:int = 71;

    private var leftarr:Sprite;

    private var rightarr:Sprite;

    public function CharacterSelectionAndNewsScreen()
    {
        this.newCharacter = new Signal();
        this.chooseName = new Signal();
        this.playGame = new Signal();
        this.playButton = ButtonFactory.getPlayButton();
        this.classesButton = ButtonFactory.getClassesButton();
        this.backButton = ButtonFactory.getMainButton();
        super();
        this.close = this.backButton.clicked;
        this.showClasses = this.classesButton.clicked;
        addChild(new ScreenBase());
        addChild(new AccountScreen());
    }

    public function initialize(_arg1:PlayerModel) : void
    {
        if(this.isInitialized)
        {
            return;
        }
        this.isInitialized = true;
        this.model = _arg1;
        this.createDisplayAssets(_arg1);
    }

    private function createDisplayAssets(_arg1:PlayerModel) : void
    {
        this.createNameText();
        this.createCreditDisplay();
        this.createBoundaryLines();
        this.Rightarr();
        this.Leftarr();
        this.createOpenCharactersText();
        var _local2:Graveyard = new Graveyard(_arg1);
        if(_local2.hasCharacters())
        {
            this.openCharactersText.setColor(TAB_SELECTED);
            this.createOpenGraveyardText();
        }
        this.createCharacterListChar();
        this.makeMenuOptionsBar();
        if(!_arg1.isNameChosen())
        {
            this.createChooseNameLink();
        }
    }

    private function makeMenuOptionsBar() : void
    {
        this.playButton.clicked.add(this.onPlayClick);
        this.menuOptionsBar = new MenuOptionsBar();
        this.menuOptionsBar.addButton(this.playButton,MenuOptionsBar.CENTER);
        this.menuOptionsBar.addButton(this.backButton,MenuOptionsBar.LEFT);
        this.menuOptionsBar.addButton(this.classesButton,MenuOptionsBar.RIGHT);
        addChild(this.menuOptionsBar);
    }

    private function createNews() : void
    {
        var _local1:NewsView = null;
        _local1 = new NewsView();
        _local1.x = NEWS_X;
        _local1.y = 112;
        addChild(_local1);
    }

    private function createScrollbar() : void
    {
        this.scrollBar = new Scrollbar(12,399);
        this.scrollBar.x = 626;
        this.scrollBar.y = 93;
        this.scrollBar.setIndicatorSize(399,this.characterList.height);
        this.scrollBar.addEventListener(Event.CHANGE,this.onScrollBarChange);
        addChild(this.scrollBar);
    }

    private function createNewsText() : void
    {
        this.newsText = new TextFieldDisplayConcrete().setSize(18).setColor(TAB_UNSELECTED);
        this.newsText.setBold(true);
        this.newsText.setStringBuilder(new LineBuilder().setParams(TextKey.CHARACTER_SELECTION_NEWS));
        this.newsText.filters = [this.DROP_SHADOW];
        this.newsText.x = NEWS_X;
        this.newsText.y = 79;
        addChild(this.newsText);
    }

    private function createCharacterListChar() : void
    {
        this.characterListType = CharacterList.TYPE_CHAR_SELECT;
        this.addChild(this.rightarr);
        this.removeChild(this.leftarr);
        this.characterList = new CharacterList(this.model,CharacterList.TYPE_CHAR_SELECT);
        this.characterList.x = this.CHARACTER_LIST_X_POS;
        this.characterList.y = this.CHARACTER_LIST_Y_POS;
        this.characterListHeight = this.characterList.height;
        if(this.characterListHeight > this.SCROLLBAR_REQUIREMENT_HEIGHT)
        {
            this.createScrollbar();
        }
        addChild(this.characterList);
    }

    private function createCharacterListGrave() : void
    {
        this.characterListType = CharacterList.TYPE_GRAVE_SELECT;
        this.addChild(this.leftarr);
        this.removeChild(this.rightarr);
        this.characterList = new CharacterList(this.model,CharacterList.TYPE_GRAVE_SELECT);
        this.characterList.x = this.CHARACTER_LIST_X_POS;
        this.characterList.y = this.CHARACTER_LIST_Y_POS;
        this.characterListHeight = this.characterList.height;
        if(this.characterListHeight > this.SCROLLBAR_REQUIREMENT_HEIGHT)
        {
            this.createScrollbar();
        }
        addChild(this.characterList);
    }

    private function removeCharacterList() : void
    {
        if(this.characterList != null)
        {
            removeChild(this.characterList);
            this.characterList = null;
        }
        if(this.scrollBar != null)
        {
            removeChild(this.scrollBar);
            this.scrollBar = null;
        }
    }

    private function createOpenCharactersText() : void
    {
        this.openCharactersText = new TextFieldDisplayConcrete().setSize(18).setColor(TAB_UNSELECTED);
        this.openCharactersText.setBold(true);
        this.openCharactersText.filters = [this.DROP_SHADOW];
        this.openCharactersText.x = this.CHARACTER_LIST_X_POS;
        this.openCharactersText.y = 79;
        this.openCharactersText.addEventListener(MouseEvent.CLICK,this.onOpenCharacters);
        addChild(this.openCharactersText);
    }

    private function onOpenCharacters(_arg1:MouseEvent) : void
    {
        if(this.characterListType != CharacterList.TYPE_CHAR_SELECT)
        {
            this.removeCharacterList();
            this.openCharactersText.setColor(TAB_SELECTED);
            this.openGraveyardText.setColor(TAB_UNSELECTED);
            this.createCharacterListChar();
        }
    }

    private function createOpenGraveyardText() : void
    {
        this.openGraveyardText = new TextFieldDisplayConcrete().setSize(18).setColor(TAB_UNSELECTED);
        this.openGraveyardText.setBold(true);
        this.openGraveyardText.filters = [this.DROP_SHADOW];
        this.openGraveyardText.x = this.CHARACTER_LIST_X_POS + 150;
        this.openGraveyardText.y = 79;
        this.openGraveyardText.addEventListener(MouseEvent.CLICK,this.onOpenGraveyard);
        addChild(this.openGraveyardText);
    }

    private function onOpenGraveyard(_arg1:MouseEvent) : void
    {
        if(this.characterListType != CharacterList.TYPE_GRAVE_SELECT)
        {
            this.removeCharacterList();
            this.openCharactersText.setColor(TAB_UNSELECTED);
            this.openGraveyardText.setColor(TAB_SELECTED);
            this.createCharacterListGrave();
        }
    }

    private function createCreditDisplay() : void
    {
        this.creditDisplay = new CreditDisplay();
        this.creditDisplay.draw(this.model.getCredits(),this.model.getFame(),this.model.getOnrane(),this.model.getKantos());
        this.creditDisplay.x = this.getReferenceRectangle().width;
        this.creditDisplay.y = 20;
        addChild(this.creditDisplay);
    }

    private function createChooseNameLink() : void
    {
        this.nameChooseLink_ = new DeprecatedClickableText(16,false,TextKey.CHARACTER_SELECTION_AND_NEWS_SCREEN_CHOOSE_NAME);
        this.nameChooseLink_.y = 50;
        this.nameChooseLink_.setAutoSize(TextFieldAutoSize.CENTER);
        this.nameChooseLink_.x = this.getReferenceRectangle().width / 2;
        this.nameChooseLink_.addEventListener(MouseEvent.CLICK,this.onChooseName);
        addChild(this.nameChooseLink_);
    }

    private function createNameText() : void
    {
        this.nameText = new TextFieldDisplayConcrete().setSize(22).setColor(11776947);
        this.nameText.setBold(true).setAutoSize(TextFieldAutoSize.CENTER);
        this.nameText.setStringBuilder(new StaticStringBuilder(this.model.getName()));
        this.nameText.filters = [this.DROP_SHADOW];
        this.nameText.y = 24;
        this.nameText.x = (this.getReferenceRectangle().width - this.nameText.width) / 2;
        addChild(this.nameText);
    }

    function getReferenceRectangle() : Rectangle
    {
        var _local1:Rectangle = new Rectangle();
        if(stage)
        {
            _local1 = new Rectangle(0,0,stage.stageWidth,stage.stageHeight);
        }
        return _local1;
    }

    private function createBoundaryLines() : void
    {
        this.lines = new Shape();
        this.lines.graphics.clear();
        this.lines.graphics.lineStyle(2,5526612);
        this.lines.graphics.moveTo(0,this.BOUNDARY_LINE_ONE_Y);
        this.lines.graphics.lineTo(800,this.BOUNDARY_LINE_ONE_Y);
        this.lines.graphics.moveTo(650,71);
        this.lines.graphics.lineTo(650,526);
        this.lines.graphics.moveTo(170,71);
        this.lines.graphics.lineTo(170,526);
        this.lines.graphics.lineStyle();
        addChild(this.lines);
    }

    private function onChooseName(_arg1:MouseEvent) : void
    {
        this.chooseName.dispatch();
    }

    private function onScrollBarChange(_arg1:Event) : void
    {
        if(this.characterList != null)
        {
            this.characterList.setPos(-this.scrollBar.pos() * (this.characterListHeight - 410));
        }
    }

    public function showBeginnersOfferButton() : void
    {
        this.beginnersPackageButton = new BeginnersPackageButton();
        this.beginnersPackageButton.x = 14;
        this.beginnersPackageButton.y = 40;
        addChild(this.beginnersPackageButton);
        this.removeIfAble(this.packageButton);
    }

    public function showPackageButton() : void
    {
        this.packageButton = new PackageButton();
        this.packageButton.init();
        this.packageButton.x = 6;
        this.packageButton.y = 40;
        addChild(this.packageButton);
        this.removeIfAble(this.beginnersPackageButton);
    }

    private function removeIfAble(_arg1:DisplayObject) : void
    {
        if(_arg1 && contains(_arg1))
        {
            removeChild(_arg1);
        }
    }

    private function onPlayClick() : void
    {
        this.removeCharacterList();
        if(this.model.getCharacterCount() == 0)
        {
            this.newCharacter.dispatch();
        }
        else
        {
            this.playGame.dispatch();
        }
    }

    public function setName(_arg1:String) : void
    {
        this.nameText.setStringBuilder(new StaticStringBuilder(_arg1));
        this.nameText.x = (this.getReferenceRectangle().width - this.nameText.width) * 0.5;
        if(this.nameChooseLink_)
        {
            removeChild(this.nameChooseLink_);
            this.nameChooseLink_ = null;
        }
    }

    private function Leftarr() : void
    {
        this.leftarr = new Sprite();
        this.leftarr.graphics.clear();
        this.leftarr.graphics.lineStyle(2,15790320);
        this.leftarr.graphics.beginFill(16777215,1);
        this.leftarr.graphics.moveTo(150,270);
        this.leftarr.graphics.lineTo(150,330);
        this.leftarr.graphics.lineTo(130,300);
        this.leftarr.graphics.endFill();
        this.leftarr.graphics.lineStyle();
        this.leftarr.buttonMode = true;
        this.leftarr.addEventListener(MouseEvent.MOUSE_DOWN,this.onOpenCharacters);
        addChild(this.leftarr);
    }

    private function Rightarr() : void
    {
        this.rightarr = new Sprite();
        this.rightarr.graphics.clear();
        this.rightarr.graphics.lineStyle(2,15790320);
        this.rightarr.graphics.beginFill(16777215,1);
        this.rightarr.graphics.moveTo(670,270);
        this.rightarr.graphics.lineTo(670,330);
        this.rightarr.graphics.lineTo(690,300);
        this.rightarr.graphics.endFill();
        this.rightarr.graphics.lineStyle();
        this.rightarr.buttonMode = true;
        this.rightarr.addEventListener(MouseEvent.MOUSE_DOWN,this.onOpenGraveyard);
        addChild(this.rightarr);
    }

    private function onMouseUp(_arg1:Event) : void
    {
        this.butclicked.dispatch();
    }
}
}
