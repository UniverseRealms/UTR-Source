package kabam.rotmg.application.setup {

public class LocalhostSetup implements ApplicationSetup {

    private const IP:String = "127.0.0.1:80";
    private const BUILD:String = "<font color='#FFEE00'>Debug</font>";

    public function getAppEngineUrl():String {
        return "http://" + this.IP;
    }

    public function getBuildLabel():String {
        return this.BUILD;
    }
}
}
