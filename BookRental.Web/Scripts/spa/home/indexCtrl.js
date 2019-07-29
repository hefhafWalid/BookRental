(function (app) {
    'use strict';

    app.controller('indexCtrl', indexCtrl);

    indexCtrl.$inject = ['$scope','apiService', 'notificationService'];

    function indexCtrl($scope, apiService, notificationService) {
        $scope.pageClass = 'page-home';
        $scope.loadingbooks = true;
        $scope.loadingGenres = true;
        $scope.isReadOnly = true;

        $scope.latestbooks = [];
        $scope.loadData = loadData;

        function loadData() {
            apiService.get('/api/books/latest', null,
                        booksLoadCompleted,
                        booksLoadFailed);

            apiService.get("/api/genres/", null,
                genresLoadCompleted,
                genresLoadFailed);
        }

        function booksLoadCompleted(result) {
            $scope.latestbooks = result.data;
            $scope.loadingbooks = false;
        }

        function genresLoadFailed(response) {
            notificationService.displayError(response.data);
        }

        function booksLoadFailed(response) {
            notificationService.displayError(response.data);
        }

        function genresLoadCompleted(result) {
            var genres = result.data;
            Morris.Bar({
                element: "genres-bar",
                data: genres,
                xkey: "Name",
                ykeys: ["NumberOfbooks"],
                labels: ["Number Of books"],
                barRatio: 0.4,
                xLabelAngle: 55,
                hideHover: "auto",
                resize: 'true'
            });
            //.on('click', function (i, row) {
            //    $location.path('/genres/' + row.ID);
            //    $scope.$apply();
            //});

            $scope.loadingGenres = false;
        }

        loadData();
    }

})(angular.module('bookRental'));
