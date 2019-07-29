(function (app) {
    'use strict';

    app.controller('booksCtrl', booksCtrl);

    booksCtrl.$inject = ['$scope', 'apiService','notificationService'];

    function booksCtrl($scope, apiService, notificationService) {
        $scope.pageClass = 'page-books';
        $scope.loadingbooks = true;
        $scope.page = 0;
        $scope.pagesCount = 0;
        
        $scope.books = [];

        $scope.search = search;
        $scope.clearSearch = clearSearch;

        function search(page) {
            page = page || 0;

            $scope.loadingbooks = true;

            var config = {
                params: {
                    page: page,
                    pageSize: 6,
                    filter: $scope.filterbooks
                }
            };

            apiService.get('/api/books/', config,
            booksLoadCompleted,
            booksLoadFailed);
        }

        function booksLoadCompleted(result) {
            $scope.books = result.data.Items;
            $scope.page = result.data.Page;
            $scope.pagesCount = result.data.TotalPages;
            $scope.totalCount = result.data.TotalCount;
            $scope.loadingbooks = false;

            if ($scope.filterbooks && $scope.filterbooks.length)
            {
                notificationService.displayInfo(result.data.Items.length + ' books found');
            }
            
        }

        function booksLoadFailed(response) {
            notificationService.displayError(response.data);
        }

        function clearSearch() {
            $scope.filterbooks = '';
            search();
        }

        $scope.search();
    }

})(angular.module('bookRental'));
