angular.module('xenatixApp')
.directive("duplicateContactDetection", ['$filter', '$rootScope', 'lookupService', function ($filter, $rootScope, lookupService) {
    var duplicateContactDetectionCtrl = function ($scope, registrationService) {
        var duplicate = this;
        duplicate.duplicateContactTableoptions = {
            pagination: true,
            pageSize: 10,
            pageList: [10, 25, 50, 100],
            search: false,
            showColumns: true,
            data: [],
            undefinedText: '',

            columns: [
                {
                    field: 'FirstName',
                    title: 'First Name'
                },
                {
                    field: 'LastName',
                    title: 'Last Name'
                },
                {
                    field: 'Middle',
                    title: 'Middle Name'
                },
                 {
                     field: 'GenderText',
                     title: 'Gender'
                 },
                {
                    field: 'DOB',
                    title: 'DOB',
                    formatter: function (value, row, index) {
                        if (value) {
                            var formattedDate = $filter('formatDate')(value, 'MM/DD/YYYY');
                            return formattedDate;
                        } else
                            return '';
                    }
                },
                {
                    field: 'SSN',
                    title: 'SSN',
                    formatter: function (value, row, index) {
                        var formattedSNN = $filter('toMaskSSN')(value);
                        return formattedSNN;
                    }
                },
                {
                    field: 'ContactID',
                    title: 'Actions',
                    formatter: function (value, row, index) {
                        return '<a href="javascript:void(0)" data-default-action data-ng-click="duplicate.populateContactInformation(' + value + ')" alt="View Contact" security permission-key="Registration-Registration-Demographics" permission="update" title="Edit"><i class="fa fa-plus-circle fa-fw padding-left-small padding-right-small" /></a>';
                    }
                }
            ]
        };
        duplicate.cancelModal = function () {
            $('#duplicateContactModel').modal('hide');
        };
    };
    duplicateContactDetectionCtrl.$inject = ['$scope', 'registrationService'];

    return {
        restrict: 'E',
        scope: {
            callBackNotDuplicate: '&',
            callDuplicateContactList: '=',
            callBackDuplicate: '&',
            callBackCancel: '&',
            setShortcutKey: "&",
        },
        templateUrl: '/Plugins/Axis.Plugins.CallCenter/Scripts/app/template/duplicateContactDetection.html',
        controller: duplicateContactDetectionCtrl,
        controllerAs: "duplicate",
        bindToController: true,
        link: function ($scope) {

            var bindDataModel = function (model, showCurrentUser) {
                var listToBind = model;
                angular.forEach(listToBind, function (contact) {
                    contact.DOB = contact.DOB ? $filter('formatDate')(contact.DOB, 'MM/DD/YYYY') : "";
                    if (contact.GenderID > 0)
                        contact.GenderText = lookupService.getSelectedText('Gender', contact.GenderID)[0].Name;
                });
                return listToBind;
            };

            $scope.duplicate.continueSave = function () {
                $('#duplicateContactModel').modal('hide');
                $scope.duplicate.callBackNotDuplicate();
            };

            $scope.duplicate.populateContactInformation = function (contactID) {
                $('#duplicateContactModel').modal('hide');
                $scope.duplicate.callBackDuplicate({ contactID: contactID });
            };

            $scope.$watch('duplicate.callDuplicateContactList', function (newValue, oldValue) {
                if (newValue && newValue.length > 0) {
                    $('#duplicateContactModel').modal('show');
                    var duplicateContactsTable = $("#duplicateDetectionContactTable");
                    $scope.duplicate.callDuplicateContactList = bindDataModel($scope.duplicate.callDuplicateContactList, false);
                    if ($scope.duplicate.callDuplicateContactList != null && $scope.duplicate.callDuplicateContactList.length > 0) {
                        duplicateContactsTable.bootstrapTable('load', $scope.duplicate.callDuplicateContactList);
                        $('#duplicateContactModel').modal('show');
                        $('#duplicateContactModel').on('shown.bs.modal', function () {
                            $rootScope.setFocusToGrid('duplicateContactModel');
                        });
                    }
                    else {
                        duplicateContactsTable.bootstrapTable('removeAll');
                    }
                }
            }, true);

            $scope.duplicate.cancelModel = function () {
                $scope.duplicate.enableShortcuts();
                $('#duplicateContactModel').modal('hide');
            };
            $scope.duplicate.enableShortcuts = function () {
                $scope.duplicate.setShortcutKey({ enterKeyStop: false, stopNext: false, saveOnEnter: false, stopSave: false });
            };

            $scope.duplicate.callBackCancel({ cancelFunction: $scope.duplicate.cancelModel });
        }
    };
}]);