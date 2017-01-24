angular.module('xenatixApp')
    .directive('registrationNavigator', ['$state', '$stateParams', 'lookupService', 'alertService', '$timeout',
        function ($state, $stateParams, lookupService, alertService, $timeout) {
            return {
                restrict: 'E',
                replace: true,
                scope: false,
                template: '<div class="row"><div class="col-lg-3 col-md-3 col-sm-3 nopadding" ng-repeat="programName in clientTypeList track by programName.ID"><div class="card text-center padding background-theme-dark margin-xxsmall" ng-class="{ &quot;selected&quot; : programName.ID == program.ClientTypeID }"><p><abbr title="{{::programName.Name}}">{{::programName.Abbreviation}}</abbr><span class="description">{{::programName.Name}}</span></p><button class="btn btn-default" id="btnSelect{{::programName.ID}}" data-ng-click="handleSelection(programName.ID)" auto-focus event-focus="{{::programName.isFocused}}">{{programName.InnerText}}<span class="sr-only">{{::programName.Name}}</span></button></div></div></div>',
                link: function (scope) {
                    scope.navigate = function () {
                        var programID = scope.program.ClientTypeID || 0;
                        var clientType = scope.clientTypeList;
                        var toStateObject = clientType[clientType.map(function (e) { return e.ID; }).indexOf(programID)];
                        if (toStateObject === undefined || toStateObject === null) {
                            alertService.error('Please select a program');
                        } else {
                            if (scope.referralContactID !== undefined && scope.referralContactID !== null) {
                                $state.go(toStateObject.RegistrationState, { ClientTypeID: programID, OtherContactID: scope.referralContactID });
                            } else {
                                $state.go(toStateObject.RegistrationState, { ClientTypeID: programID });
                            }
                        }
                    };

                    scope.handleSelection = function (id) {
                        scope.selectProgram(id);
                        scope.navigate();
                    };

                    scope.selectProgram = function (id) {
                        scope.program.ClientTypeID = id;
                        angular.forEach(scope.clientTypeList, function (program) {
                            if (program.ID === id) {
                                program.InnerText = scope.selectedText || 'Selected';
                            } else {
                                program.InnerText = scope.unselectedText || 'Select';
                            }
                        });
                    };


                    if ($stateParams.ProgramID != undefined && $stateParams.ProgramID != "") {
                        $timeout(scope.selectProgram(Number($stateParams.ProgramID)));
                    }
                }
            };
        }
    ]);

