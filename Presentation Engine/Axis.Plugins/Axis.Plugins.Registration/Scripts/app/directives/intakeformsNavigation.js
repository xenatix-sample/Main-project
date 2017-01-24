angular.module('xenatixApp')
    .directive('intakeformsNavigation', ['$stateParams', '$compile', 'assessmentService', 'roleSecurityService', '$filter', '$q', '$rootScope',
        function ($stateParams, $compile, assessmentService, roleSecurityService, $filter, $q, $rootScope) {
            return {
                restrict: 'E',
                replace: true,
                scope: false,
                template: '<ul class="list-group text-uppercase"><placeholder /></ul>',

                link: function (scope, el, attrs) {
                    var responseID = $stateParams.ResponseID;
                    var arrPromise = [];
                    assessmentService.getAssessmentSections($stateParams.AssessmentID).then(function (data) {
                        if (data.ResultCode === 0) {
                            var assessmentSections = '<workflow-action data-title="Service" data-state-name="formservice" ' +
                                                  'data-state-params="{ AssessmentID:$stateParams.AssessmentID, SectionID: $stateParams.SectionID,ContactID: $stateParams.ContactID, ResponseID: $stateParams.ResponseID, ReadOnly:$stateParams.ReadOnly, ' +
                                                  'ContactFormsID:$stateParams.ContactFormsID}" data-init-state="none" data-init-active="initializeformservice"></workflow-action>';
                            var sections = $filter('orderBy')(data.DataItems, ['SortOrder']);
                            var sectionStateName = "patientprofile.intake.formnavi.forms.formsnavi.formsSection"
                            for (var iIdx = 0; iIdx < sections.length; iIdx++) {
                                if (!sections[iIdx].PermissionKey || roleSecurityService.hasPermission(sections[iIdx].PermissionKey, 'read')) {
                                    assessmentSections += $("<workflow-action>")
                                        .attr('data-title', sections[iIdx].SectionName)
                                        .attr('data-state-name', sectionStateName)
                                        .attr('data-state-key', '-' + sections[iIdx].AssessmentSectionID)
                                        .attr('data-state-params', '{ ContactID: $stateParams.ContactID, ResponseID: $stateParams.ResponseID, SectionID: ' +
                                                                    sections[iIdx].AssessmentSectionID + ',AssessmentID:' + $stateParams.AssessmentID + ', ReadOnly:$stateParams.ReadOnly, ContactFormsID:$stateParams.ContactFormsID, DocumentStatusID:$stateParams.DocumentStatusID}').
                                        attr('data-init-state', 'none').wrap('<div>').parent().html();
                                    if ($stateParams.ContactFormsID) {
                                        arrPromise.push(assessmentService.getAssessmentResponseDetails(responseID, data.DataItems[iIdx].AssessmentSectionID));
                                    }
                                }
                            }
                            el.find('placeholder').replaceWith(assessmentSections);
                            $compile($(el))(scope);
                            $q.all(arrPromise).then(function (responseData) {
                                var totalResponses = responseData.length;
                                for (iIdx = 0; iIdx < totalResponses; iIdx++) {
                                    $rootScope.$broadcast(sectionStateName + "-" + responseData[iIdx].config.params.sectionId, { validationState: hasData(responseData[iIdx].data) ? 'valid' : 'warning' });
                                }
                            }).finally(function () {
                                if ($stateParams.ContactFormsID) {
                                    $rootScope.$broadcast('formservice', { validationState: 'valid' });
                                }
                            });
                        }
                    });
                }
            }
        }
    ]);