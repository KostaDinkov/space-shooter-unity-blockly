mergeInto(LibraryManager.library, {

  TestBrowser:function(){
    console.log("hello from Unity")
  },
  
  ChallangeLoaded:function(challengeIndex){
    window.GameController.challengeLoaded(challengeIndex);
  },
  ChallangeStarted: function(challengeIndex){
    window.GameController.challengeStarted(challengeIndex);
  },
  ChallangeRestarted:function(challengeIndex){
    window.GameController.challengeRestarted(challengeIndex);
  },
  ChallangeCompleted: function(challengeIndex){
    window.GameController.challangeCompleted(challengeIndex);
  },
  LevelStarted: function(levelIndex){
    window.GameController.levelStarted(levelIndex);
  },
  
  LevelComplete: function(levelIndex){
    window.GameController.levelComplete(levelIndex);
  },
  
  PlayerDied: function(){
    window.GameController.playerDied();
  },
});