// my jquery lib
// created by Eliran Suisa
$(document).ready(function(){
    $("#logo").hover(function(){
        var div = $("div");
        div.animate({ height: '50px', opacity: '0.4'}, "slow");
        div.animate({ width: '50px', opacity: '0.8'}, "slow");
        div.animate({ height: '20px', opacity: '0.4'}, "slow");
        div.animate({ width: '20px', opacity: '0.8'}, "slow");
    });
});