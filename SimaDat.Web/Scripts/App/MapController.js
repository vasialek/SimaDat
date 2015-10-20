(function () {

    var app = angular.module("game");

    var MapController = function ($scope) {
        $scope.location = {
            n: "North",
            s: "South",
            w: "West",
            e: "East"
        };
    };

    app.controller("MapController", MapController);

}());