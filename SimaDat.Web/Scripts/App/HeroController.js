(function () {

    var app = angular.module("game");

    var HeroController = function ($scope) {
        $scope.ttl = 24;
        $scope.strength = 1;
        $scope.iq = 2;
        $scope.charm = 3;
    };
    
    app.controller("HeroController", HeroController);

}());
