(function (app) {
    'use strict';

    app.controller('bookEditCtrl', bookEditCtrl);

    bookEditCtrl.$inject = ['$scope', '$location', '$routeParams', 'apiService', 'notificationService', 'fileUploadService'];

    function bookEditCtrl($scope, $location, $routeParams, apiService, notificationService, fileUploadService) {
        $scope.pageClass = 'page-books';
        $scope.book = {};
        $scope.genres = [];
        $scope.loadingbook = true;
        $scope.isReadOnly = false;
        $scope.Updatebook = Updatebook;
        $scope.prepareFiles = prepareFiles;
        $scope.openDatePicker = openDatePicker;

        $scope.dateOptions = {
            formatYear: 'yy',
            startingDay: 1
        };
        $scope.datepicker = {};

        var bookImage = null;

        function loadbook() {

            $scope.loadingbook = true;

            apiService.get('/api/books/details/' + $routeParams.id, null,
            bookLoadCompleted,
            bookLoadFailed);
        }

        function bookLoadCompleted(result) {
            $scope.book = result.data;
            $scope.loadingbook = false;

            loadGenres();
        }

        function bookLoadFailed(response) {
            notificationService.displayError(response.data);
        }

        function genresLoadCompleted(response) {
            $scope.genres = response.data;
        }

        function genresLoadFailed(response) {
            notificationService.displayError(response.data);
        }

        function loadGenres() {
            apiService.get('/api/genres/', null,
            genresLoadCompleted,
            genresLoadFailed);
        }

        function Updatebook() {
            if (bookImage) {
                fileUploadService.uploadImage(bookImage, $scope.book.ID, UpdatebookModel);
            }
            else
                UpdatebookModel();
        }

        function UpdatebookModel() {
            apiService.post('/api/books/update', $scope.book,
            updatebookSucceded,
            updatebookFailed);
        }

        function prepareFiles($files) {
            bookImage = $files;
        }

        function updatebookSucceded(response) {
            console.log(response);
            notificationService.displaySuccess($scope.book.Title + ' has been updated');
            $scope.book = response.data;
            bookImage = null;
        }

        function updatebookFailed(response) {
            notificationService.displayError(response);
        }

        function openDatePicker($event) {
            $event.preventDefault();
            $event.stopPropagation();

            $scope.datepicker.opened = true;
        };

        loadbook();
    }

})(angular.module('bookRental'));
