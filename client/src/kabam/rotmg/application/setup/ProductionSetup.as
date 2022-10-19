package kabam.rotmg.application.setup {

public class ProductionSetup implements ApplicationSetup {

    private const IP:String = "127.0.0.1:80";//"86.28.154.85:8080";
    private const BUILD_LABEL:String = "<font color='#A24DC1'>UT Reborn</font>";

    public function getAppEngineUrl():String {
        return "http://" + this.IP;
    }

    public function getBuildLabel():String {
        return this.BUILD_LABEL;
    }
}
}
