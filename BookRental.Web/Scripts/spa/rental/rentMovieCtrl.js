(function (app) {
    'use strict';

    app.controller('rentbookCtrl', rentbookCtrl);

    rentbookCtrl.$inject = ['$scope', '$modalInstance', '$location', 'apiService', 'notificationService'];

    function rentbookCtrl($scope, $modalInstance, $location, apiService, notificationService) {

        $scope.Title = $scope.book.Title;
        $scope.loadStockItems = loadStockItems;
        $scope.selectCustomer = selectCustomer;
        $scope.selectionChanged = selectionChanged;
        $scope.rentbook = rentbook;
        $scope.cancelRental = cancelRental;
        $scope.stockItems = [];
        $scope.selectedCustomer = -1;
        $scope.isEnabled = false;

        function loadStockItems() {
            notificationService.displayInfo('Loading available stock items for ' + $scope.book.Title);

            apiService.get('/api/stocks/book/' + $scope.book.ID, null,
            stockItemsLoadCompleted,
            stockItemsLoadFailed);
        }

        function stockItemsLoadCompleted(response) {
            $scope.stockItems = response.data;
            $scope.selectedStockItem = $scope.stockItems[0].ID;
            console.log(response);
        }

        function stockItemsLoadFailed(response) {
            console.log(response);
            notificationService.displayError(response.data);
        }

        function rentbook() {
            apiService.post('/api/rentals/rent/' + $scope.selectedCustomer + '/' + $scope.selectedStockItem, null,
            rentbookSucceeded,
            rentbookFailed);
        }

        function rentbookSucceeded(response) {
            notificationService.displaySuccess('Rental completed successfully');
            $modalInstance.close();
        }

        function rentbookFailed(response) {
            notificationService.displayError(response.data.Message);
        }

        function cancelRental() {
            $scope.stockItems = [];
            $scope.selectedCustomer = -1;
            $scope.isEnabled = false;
            $modalInstance.dismiss();
        }

        function selectCustomer($item) {
            if ($item) {
                $scope.selectedCustomer = $item.originalObject.ID;
                $scope.isEnabled = true;
            }
            else {
                $scope.selectedCustomer = -1;
                $scope.isEnabled = false;
            }
        }

        function selectionChanged($item) {
        }

        loadStockItems();
    }

})(angular.module('bookRental'));
