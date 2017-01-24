angular.module('xenatixApp')
    .controller('eligibilityCalculationController', [
        '$scope', '$state', '$stateParams', '$modal', 'eligibilityCalculationService', 'registrationService', 'alertService', 'lookupService', '$filter', '$rootScope', 'formService', '$q',
        function ($scope, $state, $stateParams, $modal, eligibilityCalculationService, registrationService, alertService, lookupService, $filter, $rootScope, formService, $q) {
            $scope.eligibilityCalculationID = null;
            $scope.init = function () {
                $scope.noDelayMessage = "No Delay";
                $scope.nextState = "patientprofile.chart.eligibilities.eligibility.notes";
                $scope.calculation = {};
                $scope.get();
            };

            $scope.handleAgeFieldsByDOB = function (dob, gestationalAge) {
                var dob = moment(dob);
                var today = moment();
                var ageInMonths = today.diff(dob, 'months');
                $scope.calculation.DeterminedAge = ageInMonths;
                $scope.calculation.ChronologicalAge = ageInMonths;
                $scope.calculation.AdjustedAge = ageInMonths;
                $scope.calculation.GestationalAge = parseInt(gestationalAge);
            };

            $scope.calculateGestationalAge = function () {
                var dob = moment(dob);
                var today = moment();
                var ageInMonths = today.diff(dob, 'months');

                if ($scope.calculation.ChronologicalAge <= 18) {
                    if ($scope.calculation.GestationalAge !== null && $scope.calculation.GestationalAge !== undefined && $scope.calculation.GestationalAge > 0) {
                        if ($scope.calculation.GestationalAge < 37) {
                            var prematureWeeks = 37 - $scope.calculation.GestationalAge;
                            var adjustedWeeks = ($scope.calculation.ChronologicalAge * 4) - prematureWeeks;
                            $scope.calculation.AdjustedAge = (adjustedWeeks / 4).toFixed();
                        }
                        else {
                            $scope.calculation.AdjustedAge = ageInMonths;
                        }
                    }
                    else {
                        $scope.calculation.AdjustedAge = ageInMonths;
                    }
                }
            };

            //watchers for non-green fields that are used to complete other fields
            $scope.$watch('calculation.GestationalAge', function () {
                $scope.calculateGestationalAge();
            });

            $scope.$watch('calculation.UseAdjustedAge', function () {
                if ($scope.calculation.UseAdjustedAge !== undefined) {
                    if ($scope.calculation.UseAdjustedAge === true) {
                        $scope.calculation.DeterminedAge = $scope.calculation.AdjustedAge;
                    }
                    else {
                        $scope.calculation.DeterminedAge = $scope.calculation.ChronologicalAge;
                    }
                }
            });

            //Helpers
            function isChronologicalAgeOver24() {
                return $scope.calculation.ChronologicalAge >= 24;
            }

            $scope.get = function () {
                var tmpData = {};
                var dob = new moment();
                var gestationalAge = null;
                var promises = [];

                promises.push(eligibilityCalculationService.get($stateParams.EligibilityID).then(function (data) {
                    if (data.ResultCode !== 0) {
                        alertService.error('Error while loading eligibility data');
                    } else if (data.DataItems.length > 0) {
                        tmpData = data.DataItems[0];
                    }
                },
                    function (errorStatus) {
                        alertService.error('Error while loading eligibility data: ' + errorStatus);
                    }
                ));

                promises.push(registrationService.get($stateParams.ContactID).then(function (regdata) {
                    if ((regdata.DataItems && regdata.DataItems.length === 1)) {
                        dob = moment(regdata.DataItems[0].DOB);
                        gestationalAge = regdata.DataItems[0].GestationalAge;
                    }
                    else {
                        dob = $scope.header.DOB;
                    }

                    $scope.handleAgeFieldsByDOB(dob, gestationalAge);
                })
                );

                $q.all(promises).finally(function () {
                    //use an extend so that the age fields are not overridden
                    var data = $.extend(true, {}, $scope.calculation, tmpData);
                    if (data.EligibilityCalculationID == null) {
                        $scope.eligibilityCalculationID = 0;
                    }
                    else {
                        $scope.eligibilityCalculationID = data.EligibilityCalculationID;
                    }
                    $scope.calculation = data;
                    $scope.ctrl.calculationForm.$setPristine();
                });
            };

            $scope.saveCalculations = function (isNext) {
                if ($scope.calculation.EligibilityCalculationID === 0 || $scope.calculation.EligibilityCalculationID === undefined) {
                    eligibilityCalculationService.add($scope.calculation).then(function (data) {
                        if (data.data.ResultCode !== 0) {
                            alertService.error('Error while saving eligibility data');
                        } else {
                            alertService.success('Eligibility calculation saved successfully');
                            if (isNext) {
                                $state.go($scope.nextState, { EligibilityID: $stateParams.EligibilityID });
                            }
                        }
                    },
                        function (errorStatus) {
                            alertService.error('Error while saving eligibility data: ' + errorStatus);
                        });
                } else {
                    eligibilityCalculationService.update($scope.calculation).then(function (data) {
                        if (data.data.ResultCode !== 0) {
                            alertService.error('Error while saving eligibility data');
                        } else {
                            alertService.success('Eligibility calculation saved successfully');
                            if (isNext) {
                                $state.go($scope.nextState, { EligibilityID: $stateParams.EligibilityID });
                            }
                        }
                    },
                    function (errorStatus) {
                        alertService.error('Error while saving eligibility data: ' + errorStatus);
                    });
                }
            };

            $scope.save = function (isNext, mandatory, hasErrors) {
                if (!hasErrors) {
                    $scope.calculation.EligibilityID = $stateParams.EligibilityID;
                    $scope.saveCalculations(isNext);
                }
            };

            //Adaptive
            $scope.$watch('calculation.SCAE', function () {
                $scope.calculateAAE();
            });

            $scope.$watch('calculation.PRAE', function () {
                $scope.calculateAAE();
            });

            $scope.$watch('calculation.AAE', function () {
                $scope.calculateAMD();
            });

            $scope.$watch('calculation.AMD', function () {
                var diffCalc = determinedAgeAAEDiffLessThanOne();
                if (diffCalc.LessThanOne) {
                    $scope.calculation.APD = $scope.noDelayMessage;
                }
                else {
                    $scope.calculation.APD = Math.floor((($scope.calculation.AMD / $scope.calculation.DeterminedAge) * 100));
                    if (isNaN($scope.calculation.APD))
                        $scope.calculation.APD = '';
                }
            });
            //Adaptive End
            //Personal-Social
            $scope.$watch('calculation.AIAE', function () {
                $scope.calculatePersonalSocialAE();
            });

            $scope.$watch('calculation.PIAE', function () {
                $scope.calculatePersonalSocialAE();
            });

            $scope.$watch('calculation.SRAE', function () {
                $scope.calculatePersonalSocialAE();
            });

            $scope.$watch('calculation.PersonalSocialAE', function () {
                var diffCalc = determinedAgePSAEDiffLessThanOne();
                if (diffCalc.LessThanOne) {
                    $scope.calculation.PersonalSocialMD = $scope.noDelayMessage;//Can this stay 0?
                }
                else {
                    $scope.calculation.PersonalSocialMD = Math.floor(diffCalc.Difference);
                }
            });

            $scope.$watch('calculation.PersonalSocialMD', function () {
                var diffCalc = determinedAgePSAEDiffLessThanOne();
                if (diffCalc.LessThanOne) {
                    $scope.calculation.PersonalSocialPD = $scope.noDelayMessage;
                }
                else {
                    $scope.calculation.PersonalSocialPD = (($scope.calculation.PersonalSocialMD / $scope.calculation.DeterminedAge) * 100).toFixed();

                    if (isNaN($scope.calculation.PersonalSocialPD))
                        $scope.calculation.PersonalSocialPD = '';
                }
            });
            //Personal-Social End
            //Communication
            $scope.$watch('calculation.RCAE', function () {
                $scope.calculation.CommAE = ($scope.calculation.RCAE + $scope.calculation.ECAE) / 2;
            });

            $scope.$watch('calculation.ECAE', function () {
                var diffCalc = determinedAgeECAEDiffLessThanOne();
                if (diffCalc.LessThanOne) {
                    $scope.calculation.ECMD = $scope.noDelayMessage;
                }
                else {
                    $scope.calculation.ECMD = Math.floor(diffCalc.Difference);
                }
            });

            $scope.$watch('calculation.ECMD', function () {
                var diffCalc = determinedAgeECAEDiffLessThanOne();
                if (diffCalc.LessThanOne) {
                    $scope.calculation.ECPD = $scope.noDelayMessage;
                }
                else {
                    $scope.calculation.ECPD = (($scope.calculation.ECMD / $scope.calculation.DeterminedAge) * 100).toFixed();
                    if (isNaN($scope.calculation.ECPD))
                        $scope.calculation.ECPD = '';
                }

                $scope.calculation.CommAE = ($scope.calculation.RCAE + $scope.calculation.ECAE) / 2;
            });

            $scope.$watch('calculation.CommAE', function () {
                var diffCalc = determinedAgeCommAEDiffLessThanOne();
                if (diffCalc.LessThanOne) {
                    $scope.calculation.CommMD = $scope.noDelayMessage;
                }
                else {
                    $scope.calculation.CommMD = Math.floor(diffCalc.Difference);
                }
            });

            $scope.$watch('calculation.CommMD', function () {
                var diffCalc = determinedAgeCommAEDiffLessThanOne();
                if (diffCalc.LessThanOne) {
                    $scope.calculation.CommPD = $scope.noDelayMessage;
                }
                else {
                    $scope.calculation.CommPD = Math.floor(($scope.calculation.CommMD / $scope.calculation.DeterminedAge) * 100).toFixed();
                    if (isNaN($scope.calculation.CommPD))
                        $scope.calculation.CommPD = '';
                }
            });
            //Communication End
            //Motor
            $scope.$watch('calculation.GMAE', function () {
                var diffCalc = determinedAgeGMAEDiffLessThanOne();
                if (diffCalc.LessThanOne) {
                    $scope.calculation.GMD = $scope.noDelayMessage;
                }
                else {
                    $scope.calculation.GMD = Math.floor(diffCalc.Difference);
                    if (isNaN($scope.calculation.GMD))
                        $scope.calculation.GMD = '';
                }
            });

            $scope.$watch('calculation.GMD', function () {
                var diffCalc = determinedAgeGMAEDiffLessThanOne();
                if (diffCalc.LessThanOne) {
                    $scope.calculation.GMPD = $scope.noDelayMessage;
                }
                else {
                    $scope.calculation.GMPD = (($scope.calculation.GMD / $scope.calculation.DeterminedAge) * 100).toFixed();
                    if (isNaN($scope.calculation.GMPD))
                        $scope.calculation.GMPD = '';
                }
            });

            $scope.$watch('calculation.FMAE', function () {
                if (isChronologicalAgeOver24()) {
                    $scope.calculation.FPMAE = $scope.calculation.FMAE;
                } else {
                    $scope.calculation.FPMAE = (($scope.calculation.FMAE + $scope.calculation.PMAE) / 2).toFixed();
                }
            });

            $scope.$watch('calculation.PMAE', function () {
                if (isChronologicalAgeOver24()) {
                    $scope.calculation.FPMAE = $scope.calculation.FMAE;
                } else {
                    $scope.calculation.FPMAE = (($scope.calculation.FMAE + $scope.calculation.PMAE) / 2).toFixed();
                }
                if (isNaN($scope.calculation.FPMAE))
                    $scope.calculation.FPMAE = '';
            });

            $scope.$watch('calculation.FPMAE', function () {
                var diffCalc = determinedAgeFPMAEDiffLessThanOne();
                if (diffCalc.LessThanOne === true) {
                    $scope.calculation.FPMD = $scope.noDelayMessage;
                }
                else {
                    $scope.calculation.FPMD = Math.floor($scope.calculation.DeterminedAge - $scope.calculation.FPMAE).toFixed();
                }
                if (isNaN($scope.calculation.FPMD))
                    $scope.calculation.FPMD = '';
            });

            $scope.$watch('calculation.FPMD', function () {
                var diffCalc = determinedAgeFPMAEDiffLessThanOne();
                if (diffCalc.LessThanOne) {
                    $scope.calculation.FPMPD = $scope.noDelayMessage;
                }
                else {
                    $scope.calculation.FPMPD = Math.floor(($scope.calculation.FPMD / $scope.calculation.DeterminedAge) * 100).toFixed();
                    if (isNaN($scope.calculation.FPMPD))
                        $scope.calculation.FPMPD = '';
                }
            });
            //Motor End
            //Cognitive
            $scope.$watch('calculation.AMAE', function () {
                if (isChronologicalAgeOver24()) {
                    $scope.calculation.CognitiveAE = (($scope.calculation.AMAE + $scope.calculation.RAAE + $scope.calculation.PCAE) / 3).toFixed();
                } else {
                    $scope.calculation.CognitiveAE = (($scope.calculation.AMAE + $scope.calculation.PCAE) / 2).toFixed();
                }
                if (isNaN($scope.calculation.CognitiveAE))
                    $scope.calculation.CognitiveAE = '';
            });

            $scope.$watch('calculation.RAAE', function () {
                if (isChronologicalAgeOver24()) {
                    $scope.calculation.CognitiveAE = (($scope.calculation.AMAE + $scope.calculation.RAAE + $scope.calculation.PCAE) / 3).toFixed();
                } else {
                    $scope.calculation.CognitiveAE = (($scope.calculation.AMAE + $scope.calculation.PCAE) / 2).toFixed();
                }
                if (isNaN($scope.calculation.CognitiveAE))
                    $scope.calculation.CognitiveAE = '';
            });

            $scope.$watch('calculation.PCAE', function () {
                if (isChronologicalAgeOver24()) {
                    $scope.calculation.CognitiveAE = (($scope.calculation.AMAE + $scope.calculation.RAAE + $scope.calculation.PCAE) / 3).toFixed();
                } else {
                    $scope.calculation.CognitiveAE = (($scope.calculation.AMAE + $scope.calculation.PCAE) / 2).toFixed();
                }
                if (isNaN($scope.calculation.CognitiveAE))
                    $scope.calculation.CognitiveAE = '';
            });

            $scope.$watch('calculation.CognitiveAE', function () {
                var diffCalc = determinedAgeCognitiveAEDiffLessThanOne();
                if (diffCalc.LessThanOne) {
                    $scope.calculation.CD = $scope.noDelayMessage;
                }
                else {
                    $scope.calculation.CD = Math.floor($scope.calculation.DeterminedAge - $scope.calculation.CognitiveAE);
                    if (isNaN($scope.calculation.CD))
                        $scope.calculation.CD = '';
                }
            });

            $scope.$watch('calculation.CD', function () {
                var diffCalc = determinedAgeCognitiveAEDiffLessThanOne();
                if (diffCalc.LessThanOne) {
                    $scope.calculation.CPD = $scope.noDelayMessage;
                }
                else {
                    $scope.calculation.CPD = Math.floor(($scope.calculation.CD / $scope.calculation.DeterminedAge) * 100).toFixed();
                    if (isNaN($scope.calculation.CPD))
                        $scope.calculation.CPD = '';
                }
            });
            //Cognitive End
            //End of watchers

            //Helpers
            function determinedAgeAAEDiffLessThanOne() {
                var returnObject = {};
                var tmpAMD = ($scope.calculation.DeterminedAge - $scope.calculation.AAE);
                if (tmpAMD < 1) {
                    returnObject.LessThanOne = true;
                }
                else {
                    returnObject.LessThanOne = false;
                    returnObject.Difference = tmpAMD;
                }

                return returnObject;
            }

            function determinedAgePSAEDiffLessThanOne() {
                var returnObject = {};
                var tmpAMD = ($scope.calculation.DeterminedAge - $scope.calculation.PersonalSocialAE);
                if (tmpAMD < 1) {
                    returnObject.LessThanOne = true;
                }
                else {
                    returnObject.LessThanOne = false;
                    returnObject.Difference = tmpAMD;
                }

                return returnObject;
            }

            function determinedAgeECAEDiffLessThanOne() {
                var returnObject = {};
                var tmpAMD = ($scope.calculation.DeterminedAge - $scope.calculation.ECAE);
                if (tmpAMD < 1) {
                    returnObject.LessThanOne = true;
                }
                else {
                    returnObject.LessThanOne = false;
                    returnObject.Difference = tmpAMD;
                }

                return returnObject;
            }

            function determinedAgeCommAEDiffLessThanOne() {
                var returnObject = {};
                var tmpAMD = ($scope.calculation.DeterminedAge - $scope.calculation.CommAE);
                if (tmpAMD < 1) {
                    returnObject.LessThanOne = true;
                }
                else {
                    returnObject.LessThanOne = false;
                    returnObject.Difference = tmpAMD;
                }

                return returnObject;
            }

            function determinedAgeGMAEDiffLessThanOne() {
                var returnObject = {};
                var tmpMD = ($scope.calculation.DeterminedAge - $scope.calculation.GMAE);
                if (tmpMD < 1) {
                    returnObject.LessThanOne = true;
                }
                else {
                    returnObject.LessThanOne = false;
                    returnObject.Difference = tmpMD;
                }

                return returnObject;
            }

            function determinedAgeFPMAEDiffLessThanOne() {
                var returnObject = {};
                var tmpMD = ($scope.calculation.DeterminedAge - $scope.calculation.FPMAE);
                if (tmpMD < 1) {
                    returnObject.LessThanOne = true;
                }
                else {
                    returnObject.LessThanOne = false;
                    returnObject.Difference = tmpMD;
                }

                return returnObject;
            }

            function determinedAgeCognitiveAEDiffLessThanOne() {
                var returnObject = {};
                var tmpMD = ($scope.calculation.DeterminedAge - $scope.calculation.CognitiveAE);
                if (tmpMD < 1) {
                    returnObject.LessThanOne = true;
                }
                else {
                    returnObject.LessThanOne = false;
                    returnObject.Difference = tmpMD;
                }

                return returnObject;
            }

            $scope.calculateAMD = function () {
                var diffCalc = determinedAgeAAEDiffLessThanOne();
                if (diffCalc.LessThanOne) {
                    $scope.calculation.AMD = $scope.noDelayMessage;//Can this stay 0?
                }
                else {
                    $scope.calculation.AMD = Math.floor(diffCalc.Difference);
                }
            };

            $scope.calculateAAE = function () {
                if (isChronologicalAgeOver24()) {
                    $scope.calculation.AAE = ($scope.calculation.SCAE + $scope.calculation.PRAE) / 2;
                }
                else {
                    $scope.calculation.AAE = $scope.calculation.SCAE;
                }
            };

            $scope.calculatePersonalSocialAE = function () {
                if (isChronologicalAgeOver24() === true) {
                    $scope.calculation.PersonalSocialAE = (($scope.calculation.AIAE + $scope.calculation.PIAE + $scope.calculation.SRAE) / 3).toFixed();
                }
                else {
                    $scope.calculation.PersonalSocialAE = (($scope.calculation.AIAE + $scope.calculation.SRAE) / 2).toFixed();
                }

                if (isNaN($scope.calculation.PersonalSocialAE))
                    $scope.calculation.PersonalSocialAE = '';
            };
            //End Helpers

            //call on load
            $scope.init();
        }
    ]);