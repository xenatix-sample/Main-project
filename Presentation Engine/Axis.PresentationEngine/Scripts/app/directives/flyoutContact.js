angular.module('xenatixApp')
.directive('flyoutContact', ['roleSecurityService', '$timeout', function (roleSecurityService, $timeout) {
    return {
        restrict: 'E',
        scope: {
            contactModel: '='
        },
        template: '' +
            ' <div ng-show="hasContact(contactModel)" class="sidebar-offcanvas"> ' +
                '<div class="panel panel-default nomargin-top padding-bottom-small padding-left-small margin-right-small" >' +
                    '<div class="panel-heading">' +
                    '<div class="border-top padding-top-xsmall"></div>' +
                    '<div class="panel-title">' +
                        '<a href="javascript:void(0)" data-toggle="offcanvasoff" alt="Close Panel" ng-click="closeFlyout()"> Contact Information ' +
                            '<div class="btn-group pull-right padding-right">' +
                                '<i class="fa fa-times-circle" data-toggle="offcanvasoff"><span class="sr-only">Close Section and Return</span></i>' +
                            '</div>' +
                        '</a>' +
                    '</div>' +
                    '<div class="border-bottom padding-top-xsmall"></div>' +
                '</div>' +
                '<div class="panel-body nopadding-top" id="panel-last-accessed">' +
                    '<div class="divtable">' +
                        '<div class="row padding-bottom-xsmall divrow">' +
                            '<div class="divcell">' +
                                '<h1>{{contactModel.FirstName}} {{contactModel.MiddleName}} {{contactModel.LastName}}</h1>' +
                            '</div>' +
                            '<div class="divcell">' +
                               // <!-- Standard image display. Male placeholder for males or unknown if no photo is
                               //available. Female placeholder for females if no photo is available. Show photo
                               // if patient has one. -->
                               '<img ng-src="{{ contactModel.ThumbnailBLOB }}" class="margin-top-small margin-right-small"  src="Images/profile_male.svg"/>' +
                            '</div>' +
                        '</div>' +
                    '</div>' +
                    '<div class="row">' +
                        '<div class="col-md-12">' +
                            '<dl>' +
                                '<dt>DIVISION</dt>' +
                                '<dd>{{contactModel.ProgramUnit}}</dd>' +
                            '</dl>' +
                        '</div>' +
                    '</div>' +
                    '<div class="row">' +
                        '<div class="col-md-12">' +
                            '<dl>' +
                                '<dt>MRN</dt>' +
                                '<dd>{{contactModel.MRN}}</dd>' +
                            '</dl>' +
                        '</div>' +
                    '</div>' +
                    '<div class="border-top padding-bottom-xxsmall"></div>' +
                    '<div class="row">' +
                        '<div class="col-md-6 col-sm-6">' +
                            '<dl>' +
                                '<dt>Gender</dt>' +
                                '<dd>{{contactModel.Gender}}</dd>' +
                            '</dl>' +
                        '</div>' +
                        '<div class="col-md-6 col-sm-6">' +
                            '<dl>' +
                                '<dt>Date of Birth</dt>' +
                                '<dd>{{contactModel.DOB}}</dd>' +
                            '</dl>' +
                        '</div>' +
                    '</div>' +
                    '<div class="border-top padding-bottom-xxsmall"></div>' +
                    '<div class="row">' +
                        '<div class="col-md-6 col-sm-6">' +
                            '<dl>' +
                                '<dt>Preferred Contact Method</dt>' +
                                '<dd>{{contactModel.PrimaryContactMethod}}</dd>' +
                            '</dl>' +
                        '</div>' +
                        '<div class="col-md-6 col-sm-6">' +
                            '<dl>' +
                                '<dt>Primary Phone</dt>' +
                                '<dd>{{contactModel.Phone}}</dd>' +
                            '</dl>' +
                        '</div>' +
                    '</div>' +
                    '<div class="border-top padding-bottom-xxsmall"></div>' +
                    '<div class="row">' +
                        '<div class="col-md-12">' +
                            '<dl>' +
                                '<dt>Current Address</dt>' +
                                '<dd class="break-word">{{contactModel.AddressLineFormat}}<br>{{contactModel.AddressCityFormat}}</dd>' +
                            '</dl>' +
                        '</div>' +
                    '</div>' +
                    '<div class="row">' +
                        '<div class="col-md-12">' +
                            '<dl>' +
                                '<dt>Email Address</dt>' +
                                '<dd class="break-word">{{contactModel.Email}}</dd>' +
                            '</dl>' +
                        '</div>' +
                    '</div>' +
                    '<div class="border-top padding-bottom-xxsmall"></div>' +
                '</div>' +
                '<div class="panel-footer" data-spy="affix" data-offset-top="50" data-offset-bottom="0">' +
                '</div>' +
            '</div>' +
        '</div>',
        link: function (scope, element, attrs) {
            var flyoutElement = $('.row-offcanvas');
            scope.hasContact = function (model) {
                return !$.isEmptyObject(model);
            };

            scope.closeFlyout = function () {
                scope.contactModel = {};
                flyoutElement.removeClass('active');
            };
        },
        controller: "baseFlyoutController"
    };
}]);