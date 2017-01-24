angular.module('xenatixApp')
.directive('keypressEvents', ['$document', 'formService', 'roleSecurityService', 'httpLoaderInterceptor', function ($document, formService, roleSecurityService, httpLoaderInterceptor) {
    return {
        restrict: 'E',
        scope: {
            onSave: "&",
            onCancel: "&",
            onNext: "&",
            onAdd: "&",
            onSign: "&",
            onClear: "&",
            enterKeyStop: "=",
            stopNext: "=",
            saveOnEnter: "=",
            closeModal: "=",
            ignoreEnter: "=",
            stopSave: "=",
            onSaveOtherMethod: "=",
            permissionKey: '@',
            permission: '@'
        },
        link: function ($scope, element, attrs) {
            $document.off('keyup').on('keyup', function (e) {
                if (e.keyCode == 9) {
                    var curPos = $(':focus').offset();
                    if (curPos != undefined) {
                        var curTop = curPos.top;
                        var screenHeight = $(window).height();
                        var scrollTop = $(window).scrollTop();
                        if (scrollTop + screenHeight <= curTop + 50) {
                            $(window).scrollTop(scrollTop + 50);
                        }
                    }
                }
            });

            //Clean attached form's event
            $scope.$on('$destroy', function () {
                $document.off('keydown.xen');
            });

            $document.off('keydown.xen').on('keydown.xen', function (e) {
                var hasPermission = true;

                if (httpLoaderInterceptor.loading()) {
                    e.preventDefault();
                    return;
                }

                if ($scope.permissionKey && $scope.permission)
                    hasPermission = roleSecurityService.hasPermission($scope.permissionKey, $scope.permission);

                //For Save ctrl+s
                if (e.ctrlKey && e.which === 83) {
                    if (!$scope.stopSave && hasPermission) {
                        if ($scope.onSaveOtherMethod != undefined || $scope.onSaveOtherMethod != null) {
                            eval($scope.onSaveOtherMethod());
                        }
                        else {
                            $scope.onSave();
                        }
                        $scope.$apply();
                    }
                    e.preventDefault();
                    return false;
                }
                //For Next ctrl+e
                if (e.ctrlKey && e.which === 69) {
                    if (!hasPermission)
                        formService.reset();
                    if (!$scope.enterKeyStop) {
                        $scope.onNext();
                        $scope.$apply();
                    }
                    else if ($scope.stopNext) {//for save only in case of tiles
                        $scope.onSave();
                        $scope.$apply();
                    }
                    e.preventDefault();
                    return false;
                }
                //For Next Enter
                if (e.which === 13 && !$scope.ignoreEnter) {
                    if (!angular.element('.bootbox-confirm').is(':visible')) {
                        if (!hasPermission)
                            formService.reset();
                        if (!$scope.enterKeyStop) {
                            if ($scope.onNext !== undefined) {
                                $scope.onNext();
                            }
                            $scope.$apply();
                        }
                        else if ($scope.stopNext) {//for save only in case of tiles
                            $scope.onSave();
                            $scope.$apply();
                        }
                        else if ($scope.saveOnEnter) {//for save only in case of tiles
                            $scope.onSave();
                            $scope.$apply();
                        }
                        e.preventDefault();
                        return false;
                    }
                }
                //For Cancel esc
                //uncommented the escape shortcut key b/c the User Manager needs it, to catch the close event of a modal w/ esc, and make sure the user doesn't want to lose data
                if (e.which === 27) {
                    if ($scope.closeModal) {
                        $scope.onCancel();
                    }
                    $scope.$apply();
                    e.preventDefault();
                    return false;
                }
                //Add New ctrl+
                if ((e.which === 187 && e.ctrlKey) || (e.which === 107 && e.ctrlKey) || (e.which === 61 && e.ctrlKey)) {
                    if (!$scope.permissionKey || roleSecurityService.hasPermission($scope.permissionKey, $scope.permission)) {
                        var isDirty = formService.isDirty();

                        var form = element.inheritedData('$formController');
                        if (form != undefined && form.$name.indexOf("financialAssessmentForm") >= 0) {
                            isDirty = formService.isDirty(form.$name);
                        }

                        if (isDirty) {
                            bootbox.confirm("Any unsaved data will be lost. Do you want to continue?", function (result) {
                                if (result == true) {
                                    $scope.onAdd();
                                    $scope.$apply();
                                }
                            });
                        }
                        else {
                            $scope.onAdd();
                            $scope.$apply();
                        }

                        //to remove green color from grid row if button is clicked.
                        ClearGridSelection(attrs);
                        //
                    }
                    e.preventDefault();
                    return false;
                }

                //For Sign ctrl+y
                if (e.ctrlKey && e.which === 89) {
                    $scope.onSign();
                    e.preventDefault();
                    return false;
                }
                //For Reset/Clear ctrl+q
                if (e.ctrlKey && e.which === 81) {
                    $scope.onClear();
                    e.preventDefault();
                    return false;
                }

                // For handling backspace
                //2016-06-19    Kishan. [Bug:11314] added "TEXTAREA" to allow backspace.
                if (e.which === 8 && (e.target.nodeName !== "INPUT" && e.target.nodeName !== "SELECT" && e.target.nodeName !== "TEXTAREA")) {
                    e.preventDefault();
                    return false;
                }
            });
        }
    };
}
])
.directive('spaceKeyPress', function () {
    return {
        restrict: 'A',
        link: function (scope, elem) {
            elem.off('keydown').on('keydown', function (e) {
                if (e.which == 32) {
                    elem.click();
                    e.preventDefault();
                }
            });
        }
    };
})
.directive('gridArrowSelector', function () {
    return {
        restrict: 'A',
        scope: true,
        link: function (scope, elem, attrs, ctrl) {
            var elemFocus = false;
            var isTriggerFocusSorting = false;
            elem.off('mouseenter').on('mouseenter', function () {
                elemFocus = true;
            });
            elem.off('mouseleave').on('mouseleave', function () {
                elemFocus = true;
            });
            elem.off('focus').on('focus', function () {
                elemFocus = true;
                scope.selectedRow = 1;
                elem.find("tr").each(function (index, item) {
                    $(item).removeClass('success');
                });
                elem.find("tr:eq(" + 1 + ")").addClass('success').find("[data-default-action]").focus();;
            });
            elem.on('click-row.bs.table', function (e, row, $element) {
                elemFocus = true;
                scope.selectedRow = $element.index() + 1;
                elem.find("tr").each(function (index, item) {
                    $(item).removeClass('success');
                });
                elem.find("tr:eq(" + scope.selectedRow + ")").addClass('success');

                if ($(':focus').attr('data-default-no-action') == undefined) {
                    elem.find("tr:eq(" + scope.selectedRow + ")").find("[data-default-action]").triggerHandler('click');
                }
            });
            elem.on('editable-init.bs.table', function () {
                if (isTriggerFocusSorting) {
                    elem.triggerHandler('focus');
                    isTriggerFocusSorting = false;
                }
            });

            elem.on('page-change.bs.table', function (e, number, size) {
                elem.triggerHandler('focus');
            });
            elem.on('sort.bs.table', function (e, name, order) {
                isTriggerFocusSorting = true;
            });

            elem.off('keydown').on('keydown', function (e) {
                if (elemFocus) {
                    //Arrow Up Key
                    if (e.keyCode == 38) {
                        if (scope.selectedRow == 0) {
                            scope.selectedRow = 1;
                        }
                        if (scope.selectedRow == 1) {
                            if (elem.bootstrapTable('getOptions').pageNumber > 1) {
                                elem.bootstrapTable('prevPage');
                                scope.selectedRow = elem.bootstrapTable('getOptions').pageSize;
                                elem.find("tr").each(function (index, item) {
                                    $(item).removeClass('success');
                                });
                                elem.find("tr:eq(" + elem.bootstrapTable('getOptions').pageSize + ")").addClass('success').find("[data-default-action]").focus();
                            }
                            else
                                return;
                        }
                        else {
                            scope.selectedRow--;

                            elem.find("tr").each(function (index, item) {
                                $(item).removeClass('success');
                            });

                            elem.find("tr:eq(" + scope.selectedRow + ")").addClass('success').find("[data-default-action]").focus();
                        }
                        scope.$apply();
                        e.preventDefault();
                    }

                    //Arrow Down Key
                    if (e.keyCode == 40) {
                        if (scope.selectedRow == elem.find("tr").length - 1) {
                            if (elem.bootstrapTable('getOptions').pageNumber < elem.bootstrapTable('getOptions').totalPages) {
                                elem.bootstrapTable('nextPage');
                                scope.selectedRow = 1;
                                elem.find("tr").each(function (index, item) {
                                    $(item).removeClass('success');
                                });
                                elem.find("tr:eq(" + 1 + ")").addClass('success').find("[data-default-action]").focus();
                            }
                            else
                                return;
                        }
                        else {
                            scope.selectedRow++;

                            elem.find("tr").each(function (index, item) {
                                $(item).removeClass('success');
                            });

                            elem.find("tr:eq(" + scope.selectedRow + ")").addClass('success').find("[data-default-action]").focus();
                        }
                        scope.$apply();
                        e.preventDefault();
                    }

                    //Space bar
                    if (e.keyCode == 32) {
                        if ($(':focus').attr('data-default-no-action') == undefined) {
                            elem.find("tr:eq(" + scope.selectedRow + ")").find("[data-default-action]").triggerHandler('click');
                        }
                        elemFocus = false;
                        e.preventDefault();
                    }
                }
            });

            elem.off('keyup').on('keyup', function (e) {
                //Tab key
                if (e.keyCode == 9) {
                    var $focused = $(':focus');

                    if ($focused.closest("tr").index() >= 0) {
                        elem.find("tr").each(function (index, item) {
                            $(item).removeClass('success');
                        });
                        $focused.closest("tr").addClass('success');
                        scope.selectedRow = $focused.closest("tr").index() + 1;
                        e.preventDefault();
                    }
                }
            });
        },
        controller: ['$scope', '$element', '$attrs',
            function ($scope, $element, $attrs) {
                $attrs.$observe('gridArrowSelector', function (value) {
                    if (value) {
                        $scope.selectedRow = value;
                    }
                });
            }
        ]
    };
});
