(function (app) {
    'use strict';

    app.controller('bookAddCtrl', bookAddCtrl);

    bookAddCtrl.$inject = ['$scope', '$location', '$routeParams', 'apiService', 'notificationService', 'fileUploadService'];

    function bookAddCtrl($scope, $location, $routeParams, apiService, notificationService, fileUploadService) {

        $scope.pageClass = 'page-books';
        $scope.book = { GenreId: 1, Rating: 1, NumberOfStocks: 1 };

        $scope.genres = [];
        $scope.isReadOnly = false;
        $scope.Addbook = Addbook;
        $scope.prepareFiles = prepareFiles;
        $scope.openDatePicker = openDatePicker;
        $scope.changeNumberOfStocks = changeNumberOfStocks;

        $scope.dateOptions = {
            formatYear: 'yy',
            startingDay: 1
        };
        $scope.datepicker = {};

        var bookImage = null;

        function loadGenres() {
            apiService.get('/api/genres/', null,
            genresLoadCompleted,
            genresLoadFailed);
        }

        function genresLoadCompleted(response) {
            $scope.genres = response.data;
        }

        function genresLoadFailed(response) {
            notificationService.displayError(response.data);
        }

        function Addbook() {
            AddbookModel();
        }

        function AddbookModel() {
            apiService.post('/api/books/add', $scope.book,
            addbookSucceded,
            addbookFailed);
        }

        function prepareFiles($files) {
            bookImage = $files;
        }

        function addbookSucceded(response) {
            notificationService.displaySuccess($scope.book.Title + ' has been submitted to Book Rental');
            $scope.book = response.data;

            if (bookImage) {
                fileUploadService.uploadImage(bookImage, $scope.book.ID, redirectToEdit);
            }
            else
                redirectToEdit();
        }

        function addbookFailed(response) {
            console.log(response);
            notificationService.displayError(response.statusText);
        }

        function openDatePicker($event) {
            $event.preventDefault();
            $event.stopPropagation();

            $scope.datepicker.opened = true;
        };

        function redirectToEdit() {
            $location.url('books/edit/' + $scope.book.ID);
        }

        function changeNumberOfStocks(increase)
        {
            var btn = $('#btnSetStocks'),
            oldValue = $('#inputStocks').val().trim(),
            newVal = 0;

            if (increase) {
                newVal = parseInt(oldValue) + 1;
            } else {
                if (oldValue > 1) {
                    newVal = parseInt(oldValue) - 1;
                } else {
                    newVal = 1;
                }
            }
            $('#inputStocks').val(newVal);
            $scope.book.NumberOfStocks = newVal;
            console.log($scope.book);
        }

        loadGenres();
    }

})(angular.module('bookRental'));