(function (app) {
    'use strict';

    app.controller('bookDetailsCtrl', bookDetailsCtrl);

    bookDetailsCtrl.$inject = ['$scope', '$location', '$routeParams', '$modal', 'apiService', 'notificationService'];

    function bookDetailsCtrl($scope, $location, $routeParams, $modal, apiService, notificationService) {
        $scope.pageClass = 'page-books';
        $scope.book = {};
        $scope.loadingbook = true;
        $scope.loadingRentals = true;
        $scope.isReadOnly = true;
        $scope.openRentDialog = openRentDialog;
        $scope.returnbook = returnbook;
        $scope.rentalHistory = [];
        $scope.getStatusColor = getStatusColor;
        $scope.clearSearch = clearSearch;
        $scope.isBorrowed = isBorrowed;

        function loadbook() {

            $scope.loadingbook = true;

            apiService.get('/api/books/details/' + $routeParams.id, null,
            bookLoadCompleted,
            bookLoadFailed);
        }

        function loadRentalHistory() {
            $scope.loadingRentals = true;

            apiService.get('/api/rentals/' + $routeParams.id + '/rentalhistory', null,
            rentalHistoryLoadCompleted,
            rentalHistoryLoadFailed);
        }

        function loadbookDetails() {
            loadbook();
            loadRentalHistory();
        }

        function returnbook(rentalID) {
            apiService.post('/api/rentals/return/' + rentalID, null,
            returnbookSucceeded,
            returnbookFailed);
        }

        function isBorrowed(rental)
        {
            return rental.Status == 'Borrowed';
        }

        function getStatusColor(status) {
            if (status == 'Borrowed')
                return 'red'
            else {
                return 'green';
            }
        }

        function clearSearch()
        {
            $scope.filterRentals = '';
        }

        function bookLoadCompleted(result) {
            $scope.book = result.data;
            $scope.loadingbook = false;
        }

        function bookLoadFailed(response) {
            notificationService.displayError(response.data);
        }

        function rentalHistoryLoadCompleted(result) {
            console.log(result);
            $scope.rentalHistory = result.data;
            $scope.loadingRentals = false;
        }

        function rentalHistoryLoadFailed(response) {
            notificationService.displayError(response);
        }

        function returnbookSucceeded(response) {
            notificationService.displaySuccess('book returned to bookRental succeesfully');
            loadbookDetails();
        }

        function returnbookFailed(response) {
            notificationService.displayError(response.data);
        }

        function openRentDialog() {
            $modal.open({
                templateUrl: 'scripts/spa/rental/rentbookModal.html',
                controller: 'rentbookCtrl',
                scope: $scope
            }).result.then(function ($scope) {
                loadbookDetails();
            }, function () {
            });
        }

        loadbookDetails();
    }

})(angular.module('bookRental'));
