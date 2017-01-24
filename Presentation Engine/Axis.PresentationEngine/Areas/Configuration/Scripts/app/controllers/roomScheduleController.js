angular.module('xenatixApp')
    .controller('roomScheduleController', ['$scope', '$q', '$filter', '$injector','formService', 'alertService', '$stateParams', 'lookupService', '$timeout', '$rootScope', '$state',
        function ($scope, $q, $filter, $injector, formService, alertService, $stateParams, lookupService, $timeout, $rootScope, $state) {

            $scope.LocationName = $scope.$parent.LocationName;
            $scope.addedRoomAvailabiliyID = 0;
            $scope.selectedFacilityID = 0;
            $scope.selectedRoomId = 0;
            $scope.selectedRoomName = '';
            $scope.successSaveUpdateCount = 0;
            $scope.resourceAvailabilitiesCount = 0;
            
            $scope.locations = [];
            $scope.rooms = [];
            $scope.resources = [];
            $scope.resourceAvailabilities = [];
            $scope.resourceOverrides = [];
            $scope.facilityRooms = [];
            $scope.roomIdCounter = [];
            $scope.facilitySchedule = [];
            $scope.itemsForDbDelete = [];
            $scope.uniqueids = [];

            $scope.resourceService == null;
           
            $scope.isLoading = true;
            var locationCounter = 0;
            var counter = 0;

            var ROOM_RESOURCE_TYPE = 1;
            var FACILITY_RESOURCE_TYPE = 6;
            var SCHEDULE_TYPE_SELECT = "Select";
            var SCHEDULE_TYPE_FACILITY = "Same as Facility";
            var SCHEDULE_TYPE_SETTIME = "Set Time";
            var SCHEDULE_TYPE_OTHER = "Other";

            $scope.resetForm = function () {
                formService.reset();
                if ($scope.ctrl.roomScheduleForm != undefined)
                    $scope.ctrl.roomScheduleForm.$dirty = false;
            };

            // ******************
            // Initialization
            // ******************
            $scope.init = function (isRefresh) {

               // Get scheduling plugin 'resourceService'
                if ($injector.has('resourceService'))
                    $scope.resourceService = $injector.get('resourceService');
                else {
                    bootbox.alert("Resource Service is not available, please load the Scheduling plugin!");
                    return;
                }
                $scope.resetForm();

                // Get FacilityID from URL
                $scope.selectedFacilityID = $scope.getFacilityIdFromUrl();
                if ($scope.selectedFacilityID == null) {
                    bootbox.alert("FacilityID is missing in the URL!");
                    return;
                }

                // Init
                $scope.getRooms($scope.selectedFacilityID, isRefresh);                
                $scope.roomScheduleTable = $('#roomScheduleTable');
                $scope.initializeBootstrapTable();
                $scope.indexDiff = 0;
                $('.timepicker').timepicker({
                    minuteStep: 1,
                    showInputs: false,
                    showMeridian: false,
                    change: function (time) {
                        $rootScope.validateFutureDate($scope);
                    }
                });

                $scope.applyDropupdownOnRoomScheduleGrid();
            };


           

           // this method is used for setting Dropup class for Columns Selection Popup window
            $scope.applyDropupdownOnRoomScheduleGrid = function () {
                if ($("#roomScheduleTable tr").length > 6)
                {
                    applyDropdownOnGrid();
                }
                else
                {
                    applyDropupOnGrid(true);
                }
                
            }

            $scope.getFacilityIdFromUrl = function () {
                var str = location.href;
                var m = /Locations\/(\d+)\//.exec(str);
                if (m) {
                    // m[1] has the number (as a string)
                    var id = Number(m[1]);
                    if (id != null && !isNaN(id)) {
                        return id;
                    }
                }
                return null;
            }

            $scope.initializeRoomCounter = function(){
                $.each($scope.rooms, function (i, item) {
                    if (item != null) {
                        var ct = new Object();
                        ct.count = 0;
                        ct.rowIdx = i;
                        $scope.roomIdCounter[item.RoomID] = ct;
                        $scope.resourceAvailabilities[item.RoomID] = new Array();
                    }
                })
            }

            $scope.initializeBootstrapTable = function () {
                $scope.tableoptions = {
                    pagination: true,
                    pageSize: 10,
                    pageList: [10, 25, 50, 100],
                    search: false,
                    showColumns: true,
                    data: [],
                    uniqueId: 'uniqueId',
                    undefinedText: '',
                    onClickRow: function (e, row, $element) {
                    },
                    onLoadSuccess: function (data) {
                        $scope.mergeCells();
                    },
                    onColumnSwitch: function (field, checked) {
                        $scope.mergeCells();
                    },
                    onSearch: function (text) {
                        $scope.mergeCells();
                    },
                    onPageChange: function (number, size) {
                        $scope.indexDiff = size * (number - 1);
                        $scope.mergeCells();
                        $scope.applyDropupdownOnRoomScheduleGrid();
                  
                    },
                    onResetView: function () {
                        $scope.mergeCells();
                    },
                    onToggle: function (cardView) {
                        $scope.mergeCells();
                    },
                    onAll: function (name, args) {
                        $('#roomScheduleTable tr').removeClass('success');
                        if (name == 'post-body.bs.table') {

                            angular.forEach($scope.facilityRooms, function (room, key) {
                                angular.forEach(room, function (item, key) {

                                    if (item.scheduleType != SCHEDULE_TYPE_SETTIME) {

                                        // Disable the row
                                        $('.room' + item.rowID + 'ampm > i').addClass('disabled');
                                        $('.room' + item.rowID + 'ampm > label').addClass('disabled');
                                        $('.room' + item.rowID + 'ampm > label').prop('style', 'color:rgb(102,102,102) !important');
                                        $('.room' + item.rowID + 'ampm > i').prop('style', 'color:rgb(102,102,102) !important');
                                        $('.row' + item.rowID + '_' + item.rowIndex + 'daychange').addClass('disabled');

                                    } else if (item.day == SCHEDULE_TYPE_SELECT) {
                                        
                                        // Disable all controls after 'day' select
                                        $('.row' + item.rowID + '_' + item.rowIndex + 'daychange').prop('disabled', 'disabled');
                                        $('.row' + item.rowID + '_' + item.rowIndex + 'daychange').prop('style', 'background-color:rgb(102,102,102) !important');
                                        $('.row' + item.rowID + '_' + item.rowIndex + 'daychange > i').parent().removeAttr('style');
                                        $('.row' + item.rowID + '_' + item.rowIndex + 'daychange').addClass('disabled');
                                        $('.row' + item.rowID + '_' + item.rowIndex + 'daychange > label').prop('style', 'color:rgb(102,102,102) !important');
                                        $('.row' + item.rowID + '_' + item.rowIndex + 'daychange > i').prop('style', 'color:rgb(102,102,102) !important');
                                    }
                                });
                            });
                        }
                    },
                    columns: [
                        {
                            field: 'roomSelect',
                            title: '',
                            titleTooltip: 'Enable or disable this room',
                            formatter: function (value, row, index) {
                                if ($scope.roomIdCounter[row.rowID].count > 0)
                                    $('#roomSelect' + row.rowID + '_' + index).parent().css('vertical-align', 'middle');
                                return '<input id="roomSelect' + row.rowID + '_' + index + '" class="room' + row.rowID + '" type="checkbox"  ng-model="facilityRooms[' + row.rowID + '][' + row.rowIndex + '].roomSelect" ng-change="onRoomSelectChange(' + row.rowID + ',' + index + ')"/>';
                            }
                        },
                        {
                            field: 'roomName',
                            title: 'Room Name',
                            width: 140,
                            formatter: function (value, row, index) {
                                if ($scope.roomIdCounter[row.rowID].count > 0)
                                    $('#roomName' + row.rowID + '_' + index).parent().css('vertical-align', 'middle');
                                return '<div id="roomName' + row.rowID + '_' + index + '" >' + value + '</div>';
                            },
                            cellStyle: function (value, row, index) {
                                return {
                                    css: { "min-width": "140px !important" }
                                };
                            }
                        },
                        {
                            field: 'scheduleType',
                            title: 'Schedule Type',
                            width: 140,
                            formatter: function (value, row, index) {
                                if ($scope.roomIdCounter[row.rowID].count > 0)
                                    $('#scheduleType' + row.rowID + '_' + index).parent().css('vertical-align', 'middle');
                                var classes = ($scope.facilityRooms[row.rowID][row.rowIndex].roomSelect == true) ? '' : 'background-theme-grey';
                                var style = ($scope.facilityRooms[row.rowID][row.rowIndex].roomSelect == true) ? '' : 'background-color: rgb(102,102,102) !important;'
                                var prop = ($scope.facilityRooms[row.rowID][row.rowIndex].roomSelect == true) ? '' : 'disabled';
                                return '<select style="' + style + '" class="form-control room' + row.rowID + ' ' + classes + '" ' + prop + ' autofocus="autofocus" ng-trim="true" ng-model="facilityRooms[' + row.rowID + '][' + row.rowIndex + '].scheduleType" ng-focus="onControlFocus(' + row.rowID + ', \'scheduleType' + row.rowID + '_' + index + '\')" ng-click="onScheduleTypeChange(' + row.rowID + ', \'scheduleType' + row.rowID + '_' + index + '\',' + row.rowIndex + ',' + index + ')" ng-required="false" id="scheduleType' + row.rowID + '_' + index + '">' +
                                            '<option value="Select" >Select</option>' +
                                            '<option value="Same as Facility" >Same as Facility</option>' +
                                            '<option value="Set Time" >Set Time</option>' +
                                        '</select>';
                            },
                            cellStyle: function (value, row, index) {
                                return {
                                    css: { "min-width": "140px !important" }
                                };
                            }
                        },
                        {
                            field: 'day',
                            title: 'Day',
                            width: 120,
                            formatter: function (value, row, index) {
                                var classes = ($scope.facilityRooms[row.rowID][row.rowIndex].scheduleType == SCHEDULE_TYPE_SETTIME) ? '' : 'background-theme-grey';
                                var style = ($scope.facilityRooms[row.rowID][row.rowIndex].scheduleType == SCHEDULE_TYPE_SETTIME) ? '' : 'background-color: rgb(102,102,102) !important;'
                                var prop = ($scope.facilityRooms[row.rowID][row.rowIndex].scheduleType == SCHEDULE_TYPE_SETTIME) ? '' : 'disabled';
                                return '<select style="' + style + '" class="form-control room' + row.rowID + '" autofocus="autofocus" class="' + classes + '" ' + prop + ' ng-model="facilityRooms[' + row.rowID + '][' + row.rowIndex + '].day" ng-trim="true" ng-required="false"  ng-focus="onControlFocus(' + row.rowID + ', \'daySelect' + row.rowID + '_' + index + '\')"  ng-click="onDayChange(' + row.rowID + ', \'daySelect' + row.rowID + '_' + index + '\',' + row.rowIndex + ')"  id="daySelect' + row.rowID + '_' + index + '">' +
                                            '<option value="Select" >Select</option>' +
                                            '<option value="Monday" >MON</option>' +
                                            '<option value="Tuesday" >TUE</option>' +
                                            '<option value="Wednesday" >WED</option>' +
                                            '<option value="Thursday" >THUR</option>' +
                                            '<option value="Friday" >FRI</option>' +
                                            '<option value="Saturday" >SAT</option>' +
                                            '<option value="Sunday" >SUN</option>'
                                        '</select>';
                            },
                            cellStyle: function (value, row, index) {
                                return {
                                    css: { "min-width": "120px !important" }
                                };
                            }
                        },
                        {
                            field: 'startTime',
                            title: 'Start Time',
                            width: 100,
                            formatter: function (value, row, index) {
                                var classes = ($scope.facilityRooms[row.rowID][row.rowIndex].scheduleType == SCHEDULE_TYPE_SETTIME && $scope.facilityRooms[row.rowID][row.rowIndex].day != SCHEDULE_TYPE_SELECT) ? '' : 'background-theme-grey';
                                var style = ($scope.facilityRooms[row.rowID][row.rowIndex].scheduleType == SCHEDULE_TYPE_SETTIME && $scope.facilityRooms[row.rowID][row.rowIndex].day != SCHEDULE_TYPE_SELECT) ? '' : 'background-color: rgb(102,102,102) !important;'
                                var prop = ($scope.facilityRooms[row.rowID][row.rowIndex].scheduleType == SCHEDULE_TYPE_SETTIME && $scope.facilityRooms[row.rowID][row.rowIndex].day != SCHEDULE_TYPE_SELECT) ? '' : 'disabled';
                                return '<div class="bootstrap-timepicker timepicker"><input  style="' + style + '" type="text" ng-model="facilityRooms[' + row.rowID + '][' + row.rowIndex + '].startTime" class="form-control room' + row.rowID + ' ' + classes + ' row' + row.rowID + '_' + row.rowIndex + 'daychange" ' + prop + ' ng-required="true" id="startTime' + row.rowID + '_' + index + '" value="' + value + '" ng-change="validateTime(\'startTime' + row.rowID + '_' + index + '\',' + row.rowID + ',' + row.rowIndex + ',\'start\')" ng-focus="onControlFocus(' + row.rowID + ', \'startTime' + row.rowID + '_' + index + '\')" ng-blur="formatTimeField(\'startTime' + row.rowID + '_' + index + '\',' + row.rowID + ',' + row.rowIndex + ')"/></div>';
                            },
                            cellStyle: function (value, row, index) {
                                return {
                                    css: { "min-width": "100px !important" }
                                };
                            }
                        },
                        {
                            field: 'startAm',
                            title: '',
                            width: 55,
                            formatter: function (value, row, index) {
                                return '<span class="xen-input-small"><xen-radio-button  class="room' + row.rowID + 'ampm row' + row.rowID + '_' + row.rowIndex + 'daychange" data-radio-button-id="startAM' + row.rowID + '_' + index + '"' +
                                      'data-ng-model="facilityRooms[' + row.rowID + '][' + row.rowIndex + '].startAm" data-ng-value=\'"AM"\' data-on-click="onAmPmClick(' + row.rowID + ',' + row.rowIndex + ',\'start\',\'AM\');" data-label="AM"></span>' +
                                      '<span class="xen-input-small"><xen-radio-button  class="room' + row.rowID + 'ampm row' + row.rowID + '_' + row.rowIndex + 'daychange" data-radio-button-id="startPM' + row.rowID + '_' + index + '"' +
                                      'data-ng-model="facilityRooms[' + row.rowID + '][' + row.rowIndex + '].startAm" data-ng-value=\'"PM"\' data-on-click="onAmPmClick(' + row.rowID + ',' + row.rowIndex + ',\'start\',\'PM\');" data-label="PM"></span>';

                            },
                            cellStyle: function (value, row, index) {
                                return {
                                    css: { "min-width": "55px !important" }
                                };
                            }
                        },
                        {
                            field: 'endTime',
                            title: 'End Time',
                            width: 100,
                            formatter: function (value, row, index) {
                                var classes = ($scope.facilityRooms[row.rowID][row.rowIndex].scheduleType == SCHEDULE_TYPE_SETTIME && $scope.facilityRooms[row.rowID][row.rowIndex].day != SCHEDULE_TYPE_SELECT) ? '' : 'background-theme-grey';
                                var style = ($scope.facilityRooms[row.rowID][row.rowIndex].scheduleType == SCHEDULE_TYPE_SETTIME && $scope.facilityRooms[row.rowID][row.rowIndex].day != SCHEDULE_TYPE_SELECT) ? '' : 'background-color: rgb(102,102,102) !important;'
                                var prop = ($scope.facilityRooms[row.rowID][row.rowIndex].scheduleType == SCHEDULE_TYPE_SETTIME && $scope.facilityRooms[row.rowID][row.rowIndex].day != SCHEDULE_TYPE_SELECT) ? '' : 'disabled';
                                return '<div class="bootstrap-timepicker timepicker"><input style="' + style + '" type="text" ng-model="facilityRooms[' + row.rowID + '][' + row.rowIndex + '].endTime" class="form-control room' + row.rowID + ' ' + classes + ' row' + row.rowID + '_' + row.rowIndex + 'daychange" ' + prop + ' ng-required="true" id="endTime' + row.rowID + '_' + index + '" value="' + value + '" ng-focus="onControlFocus(' + row.rowID + ', \'endTime' + row.rowID + '_' + index + '\')" ng-change="validateTime(\'endTime' + row.rowID + '_' + index + '\',' + row.rowID + ',' + row.rowIndex + ',\'end\')" ng-blur="formatTimeField(\'endTime' + row.rowID + '_' + index + '\',' + row.rowID + ',' + row.rowIndex + ')"/>';

                            },
                            cellStyle: function (value, row, index) {
                                return {
                                    css: { "min-width": "100px !important" }
                                };
                            }
                        },
                        {
                            field: 'endAm',
                            title: '',
                            width: 55,
                            formatter: function (value, row, index) {
                                return '<span class="xen-input-small"><xen-radio-button  class="room' + row.rowID + 'ampm row' + row.rowID + '_' + row.rowIndex + 'daychange" data-radio-button-id="endAM' + row.rowID + '_' + index + '"' +
                                       'data-ng-model="facilityRooms[' + row.rowID + '][' + row.rowIndex + '].endAm" data-ng-value=\'"AM"\' data-on-click="onAmPmClick(' + row.rowID + ',' + row.rowIndex + ',\'end\',\'AM\');" data-label="AM"></span>' +
                                       '<span class="xen-input-small"><xen-radio-button  class="room' + row.rowID + 'ampm row' + row.rowID + '_' + row.rowIndex + 'daychange" data-radio-button-id="endPM' + row.rowID + '_' + index + '"' +
                                       'data-ng-model="facilityRooms[' + row.rowID + '][' + row.rowIndex + '].endAm" data-ng-value=\'"PM"\' data-on-click="onAmPmClick(' + row.rowID + ',' + row.rowIndex + ',\'end\',\'PM\');" data-label="PM"></span>';

                            },
                            cellStyle: function (value, row, index) {
                                return {
                                    css: { "min-width": "55px !important" }
                                };
                            }
                        },
                        {
                            field: 'addRemove',
                            title: '',
                            width: 55,
                            formatter: function (value, row, index) {
                                // Don't show anything unless you're in the 'Set Time' schedule type
                                if ($scope.facilityRooms[row.rowID][row.rowIndex].scheduleType != SCHEDULE_TYPE_SETTIME)
                                    return '<i class="fa fa-plus-circle hide addremove' + row.rowID + ' room' + row.rowID + '" id="addlink' + row.rowID + '_' + index + '" ng-click="addRow(' + row.rowID + ',\'' + row.roomName + '\',' + index + ');"></i>';


                                var isnew = $scope.facilityRooms[row.rowID][row.rowIndex].isNew;
                                var removeUniqueId = (row.isAdded != null && row.isAdded) ? row.uniqueId : '';
                                var ct = $scope.roomIdCounter[row.rowID];
                                if (ct.count > 0) {
                                    if (ct.rowIdx + ct.count == index) {
                                        // For the last row, allow adds AND deletes
                                        return '<i class="fa fa-plus-circle room' + row.rowID + '" id="addlink' + row.rowID + '_' + index + '" ng-click="addRow(' + row.rowID + ',\'' + row.roomName + '\',' + index + ');"></i>' +
                                               '<i class="fa fa-minus-circle room' + row.rowID + ' padding-left-xxsmall" id="removelink' + row.rowID + '_' + index + '"  ng-click="deleteResourceAvailability(' + row.rowID + ',\'' + removeUniqueId + '\',' + index + ',' + isnew + ');"></i>';
                                    }
                                    else if (row.rowIndex == 0) {
                                        // For the first one, do not allow deletions OR adds
                                        return '';
                                    }
                                    else {
                                        // Every other row in between the first and last gets a remove link
                                        if (row.isAdded != null && row.isAdded)
                                            return '<i class="fa fa-minus-circle room' + row.rowID + ' padding-left-xxsmall" id="removelink' + row.rowID + '_' + index + '"  ng-click="deleteResourceAvailability(' + row.rowID + ',\'' + removeUniqueId + '\',' + index + ',' + isnew + ');"></i>';
                                    }
                                }
                                else {
                                    // For default, one-row just give an add link
                                    return '<i class="fa fa-plus-circle room' + row.rowID + '" id="addlink' + row.rowID + '_' + index + '" ng-click="addRow(' + row.rowID + ',\'' + row.roomName + '\',' + index + ');"></i>';
                                }
                            },
                            cellStyle: function (value, row, index) {
                                return {
                                    css: { "min-width": "55px !important" }
                                };
                            }
                        }
                    ]
                };
            };

               
            // ******************
            // Validation functions
            // ******************
            $scope.validateTime = function (id, roomID, rowIdx, startOrEnd) {
                $scope.ctrl.roomScheduleForm.$dirty = true;
                var hourcursorpos = $scope.getCursorPosition(id);


                var prev = $('#' + id).data("prev");
                $scope.facilityRooms[roomID][rowIdx].isModified = true;
                var val = $('#' + id).val();
                var idx = val.toString().indexOf(':');
                var newtime = '';
                if (idx < 0) {
                    newtime = $scope.resetTimeField(id, roomID, rowIdx, prev);
                } else {
                    var shour = val.toString().substring(0, (idx < 2) ? idx : 2); //108

                    var smin = val.toString().substring(idx +1, (val.toString().substring(idx +1).length > 2) ? val.toString().length - 1 : val.toString().length);
                    if (idx == hourcursorpos && idx == 3) {
                        var shourrem = val.toString().substring(2, 3);
                        smin = shourrem +smin.substring(1);
                    }
                    var hour =($.isNumeric(shour) && shour.indexOf('o') < 0) ? parseInt(shour.substring(0, 2)): null;
                    var min = ($.isNumeric(smin) && smin.indexOf('o') < 0) ? parseInt(smin.substring(0, 2)) : null;
                    if (hour == null || isNaN(hour) || hour > 12 || hour < 0 || shour == '' || (shour.indexOf('.') > 0))
                        hour = null;
                    if (min == null || isNaN(min) || min > 59 || min < 0 || smin == '' || (smin.indexOf('.') > 0))
                        min = null;
                    if (idx == hourcursorpos  && idx == 3 && min != null)
                            hourcursorpos++;
                    newtime = $scope.resetTimeField(id, roomID, rowIdx, prev, hour == null ? null: $scope.pad(shour, (shour.length > 2) ? hour: null, 2, hourcursorpos), min == null ? null: $scope.pad(smin, (smin.length > 2) ? min: null, 2, hourcursorpos));
                }

                // Set cursor
                $scope.selectRange(id, hourcursorpos);
                $('#' + id).data("prev", newtime);
            }

            $scope.onLeaveTimeField = function (id, roomID, rowIdx) {
                        // Check for start time being before end time
                if (!$scope.validateStartVsEndTime(roomID, rowIdx)) {
                    $scope.facilityRooms[roomID][rowIdx].endTime = '11:59';
                        $scope.facilityRooms[roomID][rowIdx].endAm = 'PM';
                        var newid = id.replace("startTime", "endTime");
                        $('#' +newid).focus();
                        $scope.selectRange(newid, 0);
                        }
                        }

                $scope.getCursorPosition = function (id) {
                var input = $('#' + id).get(0);
                if (!input) return; // No (input) element found
                if ('selectionStart' in input) {
                            // Standard-compliant browsers
                    return input.selectionStart;
                    } else if(document.selection) {
                        // IE
                        input.focus();
                    var sel = document.selection.createRange();
                    var selLen = document.selection.createRange().text.length;
                    sel.moveStart('character', - input.value.length);
                    return sel.text.length -selLen;
                    }
                }

            $scope.selectRange = function (id, start, end) {
                if(end === undefined) {
                    end = start;
                    }
                return $('#' + id).each(function () {
                    if ('selectionStart' in $('#' +id)[0]) {
                        $('#' +id)[0].selectionStart = start;
                        $('#' +id)[0].selectionEnd = end;
                    } else if ($('#' +id)[0].setSelectionRange) {
                        $('#' +id)[0].setSelectionRange(start, end);
                        } else if ($('#' +id)[0].createTextRange) {
                        var range = $('#' +id)[0].createTextRange();
                        range.collapse(true);
                            range.moveEnd('character', end);
                        range.moveStart('character', start);
                        range.select();
                        }
                        });
                        }

            $scope.validateStartVsEndTime = function (roomID, rowIndex) {
                var endtime = $scope.facilityRooms[roomID][rowIndex].endTime;
                var endam = $scope.facilityRooms[roomID][rowIndex].endAm;
                var endtimemilitary = $scope.formatTime(endtime, endam);

                var starttime = $scope.facilityRooms[roomID][rowIndex].startTime;
                var startam = $scope.facilityRooms[roomID][rowIndex].startAm;
                var starttimemilitary = $scope.formatTime(starttime, startam);
                if (starttimemilitary >= endtimemilitary) {
                    return false;
                    }
                return true;
                }

            $scope.pad = function (num, n, size, cursorpos) {

                  var s =(n != null) ? n + "" : num + "";
                    while (s.length < size) s = (cursorpos == 0 || cursorpos == 3) ? "0" +s: s + "0";
                return s;
            }

            $scope.formatTimeField = function (id, roomId, rowIdx) {
                var val = $('#' +id).val();
                var idx = val.toString().indexOf(':');
                var hour = val.toString().substring(0, idx);
                var min = val.toString().substring(idx + 1);
                hour = $scope.pad(hour, null, 2);
                min = $scope.pad(min, null, 2);
                if (hour.length > 2)
                    hour = hour.substring(idx -2, idx);
                if(min.length > 2)
                    min = min.substring(min.length -2);
                $('#' +id).val(hour + ':' +min);

                $scope.onLeaveTimeField(id, roomId, rowIdx);
                }

            $scope.resetTimeField = function (id, roomID, rowIdx, prev, hour, min) {
                var time = hour + ':' +min;
                if (hour == null || min == null || hour == "00")
                    time = prev;
                $('#' +id).val(time);
                if(id.indexOf('startTime') >= 0)
                    $scope.facilityRooms[roomID][rowIdx].startTime = time;
                    else
                    $scope.facilityRooms[roomID][rowIdx].endTime = time;
                return time;
            }

            $scope.isOverlappingSchedule = function () {

                var ret = new Object();
                ret.roomName = '';
                ret.dayName = '';
                angular.forEach($scope.facilityRooms, function (room, key) {

                    // Group availabilities by day
                    var items = {};
                    $.each(room, function (index, availability) {      
                        if (items[availability.day] == null) {
                            var itemslist = [];
                            itemslist.push(availability);
                            items[availability.day] = itemslist;
                        }
                        else {
                            items[availability.day].push(availability);
                        }
                    });                   

                    // For this room, go through each grouping
                    $.each(items, function (day, avails) {

                        if (avails.length > 1) {

                            // Go through each availability at this day
                            for (var j = 0; j < avails.length; j++) {

                                // Use the first one as a reference and check against all other ones
                                var avail = avails[j];
                                var refendtime = $scope.getTime($scope.formatTime(avail.endTime, avail.endAm)).date;
                                var refstarttime = $scope.getTime($scope.formatTime(avail.startTime, avail.startAm)).date;

                                $.each(avails, function (idx, availability) {
                                    // Only compare against OTHER items here
                                    if (availability.uniqueId != avail.uniqueId) {
                                        var cmpendtime = $scope.getTime($scope.formatTime(availability.endTime, availability.endAm)).date;
                                        var cmpstarttime = $scope.getTime($scope.formatTime(availability.startTime, availability.startAm)).date;

                                        // Check all three conditions for overlap
                                        if ((cmpstarttime >= refstarttime && cmpstarttime < refendtime) ||
                                            (cmpendtime > refstarttime && cmpendtime <= refendtime) ||
                                            (cmpstarttime <= refstarttime && cmpendtime >= refendtime)) {
                                            ret.roomName = avail.roomName;
                                            ret.dayName = avail.day;
                                            return ret;
                                        }
                                    }
                                });                                
                            }
                        }
                    })
                });
                return ret;
            }

            // ****************************
            // Save/Delete/Update functions
            // ****************************
            $scope.save = function (isNext, mandatory, hasErrors) {                

                if ($scope.ctrl.roomScheduleForm.$dirty && !hasErrors) {

                    $scope.isLoading = true;

                    // Check for overlapping times
                    var overlapobj = $scope.isOverlappingSchedule();
                    if (overlapobj.roomName != '') {
                        $scope.isLoading = false;
                        bootbox.alert('There are overlapping times for Room: ' + overlapobj.roomName + ', Day: ' + overlapobj.dayName + '. Please correct the errors and try again.', function () { });
                        return;
                    }
                        
                    // Clear the table
                    $scope.roomScheduleTable.bootstrapTable('removeAll');

                    // Remove all pending db deletes
                    $scope.removePendingDbDeletes();                    

                }  else {
                    return $scope.promiseNoOp();
                }
            };

            $scope.removePendingDbDeletes = function () {
                $scope.pendingDbDeleteDone = 0;
                $scope.pendingDbDeleteCt = $scope.itemsForDbDelete.length;
                if ($scope.pendingDbDeleteCt == 0) {
                    $scope.resetForm();
                    $scope.finishSave();
                }
                else {
                    angular.forEach($scope.itemsForDbDelete, function (item, key) {
                        $scope.resourceService.deleteResourceAvailability(item.roomID, item.uniqueId).then(function (response) {
                            if (response.ResultCode === 0) {
                                $scope.pendingDbDeleteDone++
                                if ($scope.pendingDbDeleteDone == $scope.pendingDbDeleteCt)
                                    $scope.finishSave();
                            } else {
                                alertService.error('Error while deleting a resource availability!');
                            }
                        });
                    });
                }
            }

            $scope.finishSave = function () {

                    // Set success counters
                    var modifiedrooms = $filter('filter')($scope.rooms, function (data) {
                        return data.IsModified == true;
                    });
                    $scope.successSaveUpdateCount = 0;
                    $scope.resourceAvailabilitiesCount = modifiedrooms.length;   // Also tracking saving of room schedulability
                    angular.forEach($scope.facilityRooms, function (room, key) {
                        for (var i = 0; i < room.length; i++) {
                            if (room[i].isModified)
                                $scope.resourceAvailabilitiesCount++;
                    }
                    });

                    angular.forEach($scope.facilityRooms, function (room, key) {

                        // For this room, go through and add/update availabilites
                        for (var i = 0; i < room.length; i++) {

                            var avail = room[i];
                            if (avail.isModified == false) {
                                console.log('NOT Saving availability for room: ' +key + ' at [' +avail.rowIndex +']')
                            }
                        else {

                            // Only save the items with valid scheduleType 
                            if (avail.scheduleType == SCHEDULE_TYPE_SELECT || avail.day == SCHEDULE_TYPE_SELECT)
                            continue;

                            console.log('Saving availability for room: ' +key + ' at [' +avail.rowIndex + ']');

                                // Check if this is an update or create
                                if (avail.isNew) {
                                    $scope.resourceService.addResourceAvailability($scope.getAvailModel(avail, false)).then(function (response) {
                                        if (response.ResultCode === 0) {
                                            $scope.successSaveUpdateCount++;
                                            console.log("Add success: OLD ID = " +avail.uniqueId + " NEW ID = " +response.ID + " -- CT = " + $scope.successSaveUpdateCount);
                                            if ($scope.successSaveUpdateCount == $scope.resourceAvailabilitiesCount) {
                                                alertService.success('All resource availabilities saved successfully! Please wait for the data to refresh.');
                                                $scope.init(true);
                                            }
                                                } else {
                                                alertService.error('Error while adding a resource availability! Please reload the page and try again.');
                                                $scope.isLoading = false;
                                            }
                                    });
                                }
                                else {
                                    $scope.resourceService.updateResourceAvailability($scope.getAvailModel(avail, true)).then(function (response) {
                                        if (response.ResultCode === 0) {
                                            $scope.successSaveUpdateCount++;
                                            console.log("Update success: " +avail.uniqueId + " -- CT = " +$scope.successSaveUpdateCount);
                                            if ($scope.successSaveUpdateCount == $scope.resourceAvailabilitiesCount) {
                                                alertService.success('All resource availabilities saved successfully! Please wait for the data to refresh.');
                                                $scope.init(true);
                                            }
                                        } else {
                                                alertService.error('Error while updating a resource availability! Please reload the page and try again.');
                                                $scope.isLoading = false;
                                        }
                                    });
                                }
                              }
                           }

                            // Save room schedulability
                            var foundroom = $filter('filter') ($scope.rooms, function (data) {
                                return data.RoomID == key;
                            });
                            if (foundroom[0].IsModified == false) {
                                console.log('NOT Saving room: ' + key);
                            } else {
                                $scope.resourceService.updateRoom($scope.getRoomModel(foundroom[0])).then(function (response) {
                                    if (response.ResultCode === 0) {
                                        $scope.successSaveUpdateCount++;
                                        console.log("ROOM Update success for RoomID: " +key + " -- CT = " +$scope.successSaveUpdateCount);
                                        if($scope.successSaveUpdateCount == $scope.resourceAvailabilitiesCount) {
                                            alertService.success('All resource availabilities saved successfully! Please wait for the data to refresh.');
                                            $scope.init(true);
                                        }
                                    } else {
                                        alertService.error('Error while updating a resource availability! Please reload the page and try again.');
                                        $scope.isLoading = false;
                                    }
                                });
                            }
                    });

                    if ($scope.successSaveUpdateCount == $scope.resourceAvailabilitiesCount) {
                        alertService.success('All resource availabilities saved successfully! Please wait for the data to refresh.');
                        $scope.init(true);
                    }
            }

            $scope.deleteResourceAvailability = function (resourceId, resourceAvailabilityId, index, isnew) {
                bootbox.confirm("Are you sure that you want to remove this room availability?", function (result) {
                    if (result === true) {
                        $scope.removeRow(resourceId, resourceAvailabilityId, index);
                        if (!isnew) {
                            var item = new Object();
                            item.rowID = resourceId;
                            item.uniqueId = resourceAvailabilityId;
                            $scope.itemsForDbDelete.push(item);
                        }
                    }
                });
            }

            $scope.saveRoomAvailability = function (isUpdate) {
                if (isUpdate) {
                    $scope.resourceService.addResourceAvailability(roomavail).then(function (data) {
                        $scope.addedRoomAvailabiliyID = data.ID;
                    },
                    function (errorStatus) {
                        $scope.isLoading = false;
                        alertService.error('Unable to add a room availability!');
                    });
                }
            }

            $scope.saveRoomOverride = function (isUpdate) {
                if (isUpdate) {
                    $scope.resourceService.addResourceOverrides(roomavail).then(function (data) {
                        $scope.addedRoomAvailabiliyID = data.ID;
                    },
                    function (errorStatus) {
                        $scope.isLoading = false;
                        alertService.error('Unable to add a room availability!');
                    });
                }
            }

            // ******************
            // Helper Functions
            // ******************
            $scope.formatScheduleType = function (schedtype) {
                if (schedtype == SCHEDULE_TYPE_FACILITY)
                    return schedtype;
                else
                    return SCHEDULE_TYPE_SETTIME;
            }

            $scope.formatScheduleTypeSave = function (schedtype) {
                if(schedtype == SCHEDULE_TYPE_SETTIME)
                    return SCHEDULE_TYPE_OTHER;
                else
                    return schedtype;
            }

            $scope.getAmPmValue = function (militarytime) {
                var inttime = parseInt(militarytime);
                if (inttime > 1200)
                    return "PM";
                else
                    return "AM";
            }

            $scope.getRoomModel = function (room) {
                var model = new Object();
                model.RoomID = room.RoomID;
                model.RoomName = room.RoomName;
                model.FacilityID = room.FacilityID;
                model.RoomCapacity = room.RoomCapacity;
                model.IsSchedulable = room.IsSchedulable;
                return model;
            }

            $scope.getAvailModel = function (avail, isUpdate) {
                var model = new Object();
                if (isUpdate)
                    model.ResourceAvailabilityID = avail.uniqueId;
                model.ResourceID = avail.rowID;
                model.ResourceTypeID = ROOM_RESOURCE_TYPE;
                model.FacilityID = $scope.selectedFacilityID;
                model.Days = avail.day;
                model.AvailabilityStartTime = $scope.formatTime(avail.startTime, avail.startAm);
                model.AvailabilityEndTime = $scope.formatTime(avail.endTime, avail.endAm);
                model.ScheduleType = $scope.formatScheduleTypeSave(avail.scheduleType);
                return model;
                        }

            $scope.formatTime = function (time, ampm) {
                var hours = parseInt(time.toString().substring(0, 2));
                var mins = parseInt(time.toString().substring(3, 5));
                if (ampm == "PM" && hours < 12) hours = hours + 12;
                if (ampm == "AM" && hours == 12) hours = hours - 12;
                var sHours = hours.toString();
                var sMinutes = mins.toString();
                if (hours < 10) sHours = "0" + sHours;
                if (mins < 10) sMinutes = "0" + sMinutes;
                return sHours + sMinutes;
            }

            $scope.militartyTimeToDate = function (militarytime) {
                var hours = parseInt(militarytime.toString().substring(0, 2));
                var mins = parseInt(militarytime.toString().substring(2));
                var dt = new Date(99, 1, 1, hours, mins, 0, 0);
                return dt;
            }

            $scope.getCopyAvailability = function (roomID, roomName, index, rowIndex, item) {
                var tblobj = new Object();
                tblobj.roomSelect = item.roomSelect;
                tblobj.roomName = roomName.toString().trim();
                tblobj.day = item.day;
                tblobj.startTime = item.startTime;
                tblobj.startAm = item.startAm;
                tblobj.endTime = item.endTime;
                tblobj.endAm = item.endAm;
                tblobj.rowID = roomID;
                tblobj.uniqueId = $scope.generateUniqueId(roomID, index);
                tblobj.rowIndex = rowIndex;
                tblobj.isAdded = true;
                tblobj.isNew = true;
                tblobj.scheduleType = item.scheduleType;
                return tblobj;
            }

            $scope.getEmptyAvailability = function (roomID, roomName, index, rowIndex) {
                var tblobj = new Object();
                tblobj.roomSelect = true;
                tblobj.roomName = roomName.toString().trim();
                tblobj.day = "Select";
                tblobj.startTime = "";
                tblobj.startAm = "AM";
                tblobj.endTime = "";
                tblobj.endAm = "PM";
                tblobj.rowID = roomID;
                tblobj.uniqueId = $scope.generateUniqueId(roomID, index);
                tblobj.rowIndex = rowIndex;
                tblobj.isAdded = true;
                tblobj.isNew = true;
                tblobj.scheduleType = (rowIndex > 0) ? $scope.facilityRooms[roomID][0].scheduleType : SCHEDULE_TYPE_SELECT;
                return tblobj;
            }

            $scope.setUniqueIdsList = function () {
                var datarows = $('[data-uniqueid');                
                for (var i = 0; i < datarows.length; i++) {
                    $scope.uniqueids.push(parseInt(datarows[i].getAttribute('data-uniqueid')));
                }
            }

            $scope.generateUniqueId = function (roomID, index) {
                return (index > 0) ? index * -1 : index;
            }

            $scope.getTime = function (militarytime) {
                var hours = parseInt(militarytime.toString().substring(0, 2));
                var mins = parseInt(militarytime.toString().substring(2));
                var dt = new Date(99, 1, 1, hours, mins, 0, 0);
                var obj = new Object();
                obj.date = dt;
                obj.hrsmins = $filter('toMMDDYYYYDate')(dt, 'hh:mm', 'useLocal');
                return obj;
            }

            $scope.performSelectBehavior = function (roomID) {

                var foundroom = $filter('filter')($scope.rooms, function (data) {
                    return data.RoomID === roomID;
                });
                var roomName = $scope.facilityRooms[roomID][0].roomName;
                var roomStartIdx = ($scope.roomIdCounter[roomID]).rowIdx;
                var availDelCt = 0;
                var roomAvailCt = $scope.facilityRooms[roomID].length;
                var tmplist =[];
                angular.forEach($scope.facilityRooms[roomID], function (item, key) {
                    tmplist.push(item);
                });
                angular.forEach(tmplist, function (item, key) {

                    // Remove from model/table
                    if (item.isNew) {
                        $scope.removeRow(roomID, item.uniqueId, roomStartIdx + item.rowIndex);
                        availDelCt++;
                    }
                    else {
                        $scope.removeRow(roomID, item.uniqueId, roomStartIdx +item.rowIndex);
                        availDelCt++;
                        if(availDelCt == roomAvailCt) {
                            // Add empty row
                            $scope.addRow(roomID, roomName, roomStartIdx -1);
                            $scope.facilityRooms[roomID][0].roomSelect = foundroom[0].IsSchedulable;
                            availDelCt = 0;
                        }
                        // Push to remove from db on SAVE
                        $scope.itemsForDbDelete.push(item);
                    }
                })

                if (availDelCt == roomAvailCt) {
                    // Add empty row
                    $scope.addRow(roomID, roomName, roomStartIdx - 1);
                    $scope.facilityRooms[roomID][0].roomSelect = foundroom[0].IsSchedulable;
                    availDelCt = 0;
                }
            }

            // ******************
            // OnChange Functions
            // ******************
            $scope.onDayChange = function (roomID, id, rowIndex) {
                $timeout(function () {
                    $scope.ctrl.roomScheduleForm.$dirty = true;
                    var prev = $('#' + id).data("prev");

                    if (prev == $scope.facilityRooms[roomID][rowIndex].day && $scope.facilityRooms[roomID][rowIndex].day == SCHEDULE_TYPE_SELECT)
                        return;

                    if ($scope.facilityRooms[roomID][rowIndex].day == SCHEDULE_TYPE_SELECT) {
                        $scope.facilityRooms[roomID][rowIndex].startTime = '';
                        $scope.facilityRooms[roomID][rowIndex].endTime = '';

                        // Disable all controls after 'day' select
                        $('.row' + roomID + '_' + rowIndex + 'daychange').prop('disabled', 'disabled');
                        $('.row' + roomID + '_' + rowIndex + 'daychange').prop('style', 'background-color:rgb(102,102,102) !important');
                        $('.row' + roomID + '_' + rowIndex + 'daychange > i').parent().removeAttr('style');
                        $('.row' + roomID + '_' + rowIndex + 'daychange').addClass('disabled');
                        $('.row' + roomID + '_' + rowIndex + 'daychange > label').prop('style', 'color:rgb(102,102,102) !important');
                        $('.row' + roomID + '_' + rowIndex + 'daychange > i').prop('style', 'color:rgb(102,102,102) !important');
                        return;

                    }

                    if (prev == SCHEDULE_TYPE_SELECT && $scope.facilityRooms[roomID][rowIndex].day != SCHEDULE_TYPE_SELECT) {

                        $('.row' + roomID + '_' + rowIndex + 'daychange').removeAttr('disabled');
                        $('.row' + roomID + '_' + rowIndex + 'daychange').removeClass('disabled');
                        $('.row' + roomID + '_' + rowIndex + 'daychange').removeAttr('style');
                        $('.row' + roomID + '_' + rowIndex + 'daychange > i').removeAttr('style');
                        $('.row' + roomID + '_' + rowIndex + 'daychange > label').removeAttr('style');
                        $('.row' + roomID + '_' + rowIndex + 'daychange > i').removeClass('disabled');
                        $('.row' + roomID + '_' + rowIndex + 'daychange > label').removeClass('disabled');
                        $('.row' + roomID + '_' + rowIndex + 'daychange').removeClass('background-theme-grey')

                        // Set default values
                        $scope.facilityRooms[roomID][rowIndex].startTime = '12:00';
                        $scope.facilityRooms[roomID][rowIndex].endTime = '11:59';
                    }

                    if (prev != $scope.facilityRooms[roomID][rowIndex].day && $scope.facilityRooms[roomID][rowIndex].day != SCHEDULE_TYPE_SELECT) {
                        $scope.facilityRooms[roomID][rowIndex].isModified = true;
                    }
                })
            }

            $scope.onAmPmClick = function (roomID, rowIndex, startorend, value) {
                $timeout(function () {
                    $scope.ctrl.roomScheduleForm.$dirty = true;
                    $scope.facilityRooms[roomID][rowIndex].isModified = true;

                    // Check for start time being before end time
                    if (!$scope.validateStartVsEndTime(roomID, rowIndex)) {
                        // Put back previous value
                        if (startorend == 'start') {
                            if (value == 'AM')
                                $scope.facilityRooms[roomID][rowIndex].startAm = 'PM';
                            else
                                $scope.facilityRooms[roomID][rowIndex].startAm = 'AM';
                        }
                        else {
                            if (value == 'AM')
                                $scope.facilityRooms[roomID][rowIndex].endAm = 'PM';
                            else
                                $scope.facilityRooms[roomID][rowIndex].endAm = 'AM';
                        }
                    }
                })
            }
            $scope.selectFacility = function (item) {
                $scope.selectedFacilityID = item.ID;
                $('#selectedRoom').find('option').remove().end();
                $scope.getRooms($scope.selectedFacilityID).then(
                    $.each($scope.rooms, function (i, item) {
                        $('#selectedRoom').append($('<option>', {
                            value: item.RoomID,
                            text: item.RoomName
                        }));
                    })
                );
            };

            $scope.roomSelected = function (selectedRoomId) {
                $scope.selectedRoomId = selectedRoomId;
                angular.forEach($scope.rooms, function (item) {
                    if (item.RoomID == selectedRoomId) {
                        $scope.selectedRoomName = item.RoomName;                        
                        alert("I selected " + $scope.selectedRoomName);
                        return;
                }
                });                
            }

            $scope.onRoomSelectChange = function (rowID, index) {

                $scope.ctrl.roomScheduleForm.$dirty = true;

                // Flag this room as modified
                var foundroom = $filter('filter')($scope.rooms, function (data) {
                    return data.RoomID === rowID;
                });
                foundroom[0].IsModified = true;

                // Check for 'checked' behavior
                var counter = $scope.roomIdCounter[rowID];
                var ext = rowID + '_' + index;
                var isChecked = $('#roomSelect' + ext).prop('checked');
                if (isChecked == false) {

                    var roomName = $scope.facilityRooms[rowID][0].roomName;
                    bootbox.confirm('Are you sure that you want to make \'' + roomName + '\' not schedulable?  This will remove any current and pending schedules for this room.', function (result) {
                        if (result === true) {
                            foundroom[0].IsSchedulable = false;

                            // Do all the actions as if this was a change to scheduleType = 'Select':
                            // Remove all rows for this room and add a new, blank one
                            $scope.performSelectBehavior(rowID);

                            // Disable the row
                            $('.room' + rowID).prop('disabled', 'disabled');
                            $('.room' + rowID).addClass('background-theme-grey');
                            $('#scheduleType' + rowID + '_' + index).prop('style', 'background-color: rgb(102,102,102) !important;');

                            $('.room' + rowID + 'ampm > i').addClass('disabled');
                            $('.room' + rowID + 'ampm > label').addClass('disabled');
                            $('.room' + rowID + 'ampm > label').prop('style', 'color:rgb(102,102,102) !important');
                            $('.room' + rowID + 'ampm > i').prop('style', 'color:rgb(102,102,102) !important');

                            // Enable the checkbox regardless of toggle state
                            $('#roomSelect' + ext).removeAttr('disabled');
                    }
                        else {
                            // Otherwise, put back the value                            
                            $scope.facilityRooms[rowID][counter.rowIdx].roomSelect = true;
                            $scope.$apply();
                }
                });
                }
                else {
                    foundroom[0].IsSchedulable = true;

                    // Enable scheduleType selection
                    $('#scheduleType' + rowID + '_' + index).removeAttr('disabled');
                    $('#scheduleType' + rowID + '_' +index).removeClass('background-theme-grey');
                    $('#scheduleType' + rowID + '_' +index).removeAttr('style');
                }

                // Enable the checkbox regardless of toggle state
                $('#roomSelect' + ext).removeAttr('disabled');
            }

            $scope.onControlFocus = function (roomID, id) {
                // Store previous value prior to change
                var val = $('#' + id).val();
                $('#' +id).data("prev", val);
                $scope.selectRange(id, 0);
            }

            $scope.onScheduleTypeChange = function (roomID, id, rowIndex, Index) {

                $scope.ctrl.roomScheduleForm.$dirty = true;
                // Check previous value
                var prev = $('#' + id).data("prev");
                if ($scope.facilityRooms[roomID][rowIndex].scheduleType == prev)
                    return;
                if (SCHEDULE_TYPE_FACILITY == prev && $scope.facilityRooms[roomID][rowIndex].scheduleType == SCHEDULE_TYPE_SETTIME) {
                    // 'Set Time' from 'Set as Facility', enable all but don't set any default values
                    $('.room' + roomID).removeAttr('disabled');
                    $('.room' + roomID).removeClass('background-theme-grey');
                    $('.addremove' + roomID).removeClass('hide');
                    $('.addremove' + roomID).addClass('show');
                    $('.room' + roomID).removeAttr('style');
                    angular.forEach($scope.facilityRooms[roomID], function (item, key) {
                        item.scheduleType = SCHEDULE_TYPE_SETTIME;
                        item.isModified = true;
                    });
                    $('.room' + roomID + 'ampm > i').removeAttr('style');
                    $('.room' + roomID + 'ampm > label').removeAttr('style');
                    $('.room' + roomID + 'ampm > i').removeClass('disabled');
                    $('.room' + roomID + 'ampm > label').removeClass('disabled');
                    $('.row' + roomID + '_' + rowIndex + 'daychange').removeClass('disabled');
                }


                var curct = $scope.roomIdCounter[roomID].count;
                var roomName = $scope.facilityRooms[roomID][rowIndex].roomName;
                if ($scope.facilityRooms[roomID][rowIndex].scheduleType == SCHEDULE_TYPE_FACILITY) {
                    bootbox.confirm('Are you sure that you want to set \'' + roomName + '\' to the facility schedule?', function (result) {
                        if (result === true) {

                            if ($scope.facilitySchedule.length == 0) {
                                bootbox.alert("There are no office hours set for this facility. Please navigate to the Locations page and set the facility hours.", function () { });
                                $scope.facilityRooms[roomID][0].scheduleType = prev;
                                $scope.$apply();
                                return;
                            }

                            // Remove all current rows for this room and load facility schedule, and take -/+ actions away
                            // NOTE: this goes through all facility availability rows and either:
                            // 1. Modifies the row to match facility schedule
                            // 2. Adds a row with facility schedule if one is needed
                            // 3. Removes any unneccessary rows from the model AND table delete --> FLAG THESE FOR REMOVAL ON SAVE FROM THE DB
                            var count = 0;
                            var roomStartIdx = ($scope.roomIdCounter[roomID]).rowIdx
                            var curlen = $scope.facilityRooms[roomID].length;
                            angular.forEach($scope.facilitySchedule, function (item, key) {
                                if (curlen > count) {
                                    // A row already exists at this location - match this to facility schedule
                                    var currentrow = $scope.facilityRooms[roomID][count];
                                    currentrow.startTime = item.startTime;
                                    currentrow.startAm = item.startAm;
                                    currentrow.endTime = item.endTime;
                                    currentrow.endAm = item.endAm;
                                    currentrow.day = item.day;
                                    currentrow.scheduleType = SCHEDULE_TYPE_FACILITY;
                                    currentrow.roomSelect = true;
                                    currentrow.isModified = true;
                                    $scope.$apply();
                                }
                                else {
                                    // Need to add a row here w/the facility schedule
                                    item.isModified = true;
                                    $scope.addRow(roomID, roomName, roomStartIdx + (count - 1), item);
                                }
                                count++;
                            });

                            // Remove any remaining rows (prior to any additions done above)     
                            var tmplist = [];
                            angular.forEach($scope.facilityRooms[roomID], function (item, key) {
                                tmplist.push(item);
                            });
                            angular.forEach(tmplist, function (item, key) {
                                if (key >= count) {

                                    $scope.removeRow(roomID, item.uniqueId, roomStartIdx +item.rowIndex);
                                    if (!item.isNew) {
                                        // Push for db delete on SAVE
                                        $scope.itemsForDbDelete.push(item);
                                    }
                                }
                            });

                            $('.room' + roomID).prop('disabled', 'disabled');
                            $('.room' + roomID).addClass('background-theme-grey');
                            $('.room' + roomID).prop('style', 'background-color: rgb(102,102,102) !important;');
                            $('.room' + roomID + 'ampm > i').addClass('disabled');
                            $('.room' + roomID + 'ampm > label').addClass('disabled');
                            $('.room' + roomID + 'ampm > label').prop('style', 'color:rgb(102,102,102) !important');
                            $('.room' + roomID + 'ampm > i').prop('style', 'color:rgb(102,102,102) !important');
                            $('.row' + roomID + '_' + rowIndex + 'daychange').addClass('disabled');
                            $('.addremove' + roomID).removeClass('show');
                            $('.addremove' + roomID).addClass('hide');

                            // Enable the checkbox regardless of toggle state
                            var ext = roomID + '_' + Index;
                            $('#roomSelect' + ext).removeAttr('disabled');
                            $('#scheduleType' + ext).removeAttr('disabled');
                            $('#scheduleType' + ext).removeAttr('style');
                            $('#scheduleType' + ext).removeClass('background-theme-grey');
                            $scope.isLoading = false;
                        }
                        else {
                            // put selection back to previous value
                            $scope.facilityRooms[roomID][0].scheduleType = prev;
                            $scope.$apply();
                        }
                   });
                }
                else if ($scope.facilityRooms[roomID][rowIndex].scheduleType == SCHEDULE_TYPE_SELECT) {

                    bootbox.confirm('Are you sure that you want to clear the all selections for room \'' + roomName + '\'?', function (result) {
                        if (result === true) {

                            // 1. Remove ALL rows for this room from the model AND table 
                            // 2. Add empty row
                            $scope.performSelectBehavior(roomID);
                            $('.room' + roomID + 'ampm > i').addClass('disabled');
                            $('.room' + roomID + 'ampm > label').addClass('disabled');
                            $('.room' + roomID + 'ampm > label').prop('style', 'color:rgb(102,102,102) !important');
                            $('.room' + roomID + 'ampm > i').prop('style', 'color:rgb(102,102,102) !important');
                            $('.row' + roomID + '_' + rowIndex + 'daychange').addClass('disabled');
                        }
                        else {
                            // put selection back to previous value
                            $scope.facilityRooms[roomID][rowIndex].scheduleType = prev;
                            $scope.$apply();
                        }
                    });                    
                }
                else {

                    // Just enable the daySelect dropdown
                    $('#daySelect' + roomID + '_' + Index).removeAttr('disabled');
                    $('#daySelect' + roomID + '_' + Index).removeAttr('style');
                    $('#daySelect' + roomID + '_' + Index).removeClass('background-theme-grey');
                }
            }

            // **************************
            // Row\Cell manipulation functions
            // **************************
            $scope.reloadTable = function () {
                var ct = 0;
                angular.forEach($scope.facilityRooms, function (item, key) {
                    if (ct == 0)
                        $scope.roomScheduleTable.bootstrapTable('load', item);
                    else
                        $scope.roomScheduleTable.bootstrapTable('append', item);
                    ct++;
                })
                $scope.mergeCells();
                $('.timepicker').timepicker({
                    minuteStep: 1,
                    showInputs: false,
                    showMeridian: false,
                    change: function (time) {
                        $rootScope.validateFutureDate($scope);
                    }
                });
            }

            $scope.mergeCells = function () {
                angular.forEach($scope.roomIdCounter, function (item, key) {
                    // Only do this for the correct page since the row index for 'mergeCells' starts over on each page of the table
                    if (item.count > 0) {
                        if (item.rowIdx >= $scope.indexDiff) {
                            $('#roomScheduleTable').bootstrapTable('mergeCells', { index: item.rowIdx - $scope.indexDiff, field: 'roomSelect', rowspan: (item.count + 1) });
                            $('#roomScheduleTable').bootstrapTable('mergeCells', { index: item.rowIdx - $scope.indexDiff, field: 'roomName', rowspan: (item.count + 1) });
                            $('#roomScheduleTable').bootstrapTable('mergeCells', { index: item.rowIdx - $scope.indexDiff, field: 'scheduleType', rowspan: (item.count + 1) });                            
                        }
                        else if ((item.rowIdx + item.count) > $scope.indexDiff) {
                            // This case covers same room items going over the page break
                            var span = (item.rowIdx + item.count + 1) - $scope.indexDiff;
                            $('#roomScheduleTable').bootstrapTable('mergeCells', { index: 0, field: 'roomSelect', rowspan: span });
                            $('#roomScheduleTable').bootstrapTable('mergeCells', { index: 0, field: 'roomName', rowspan: span });
                            $('#roomScheduleTable').bootstrapTable('mergeCells', { index: 0, field: 'scheduleType', rowspan: span });
                        }
                    }
                });
            }

            $scope.addRow = function (rowID, roomName, index, item) {
                        
                $scope.ctrl.roomScheduleForm.$dirty = true;

                // Get count of rows currently for this room and update count 
                var ct = $scope.roomIdCounter[rowID];
                ct.count++;
                $scope.roomIdCounter[rowID] = ct;

                // Update each rooms start index
                angular.forEach($scope.roomIdCounter, function (item) {
                    if (item.rowIdx > index && ct.count > 0)
                        item.rowIdx = item.rowIdx + 1;
                });

                // Add new row 
                var tblobj = (item == null) ? $scope.getEmptyAvailability(rowID, roomName, (index + 1), ct.count) :
                    $scope.getCopyAvailability(rowID, roomName, (index + 1), ct.count, item);
                $scope.facilityRooms[rowID].push(tblobj);
                $scope.uniqueids.push(tblobj.uniqueId);
                $('#roomScheduleTable').bootstrapTable('insertRow', { index: (index + 1), row: tblobj });
                $scope.mergeCells();
            }         

            $scope.removeRow = function (resourceId, uniqueId, index) {

                $scope.ctrl.roomScheduleForm.$dirty = true;

                // Update counter and row idx tracker
                var ct = $scope.roomIdCounter[resourceId];
                ct.count = ct.count - 1;
                $scope.roomIdCounter[resourceId] = ct;
                angular.forEach($scope.roomIdCounter, function (item) {
                    if (item.rowIdx > index && ct.count >= 0)
                        item.rowIdx = item.rowIdx - 1;
                });

                // Find index by unique id and update all rowIndex values
                var curavailarray = $scope.facilityRooms[resourceId];
                var curindex = 0;
                for (var i = 0; i < curavailarray.length; i++) {
                    if (curavailarray[i].uniqueId == uniqueId) {
                        curindex = i;
                        break;
                    }
                }
                angular.forEach($scope.facilityRooms[resourceId], function (item) {
                    if (item.rowIndex > curindex)
                        item.rowIndex = item.rowIndex - 1;
                    });

                // Remove from model
                $scope.facilityRooms[resourceId].splice(curindex, 1);
                var uididx = $scope.uniqueids.indexOf(uniqueId);
                if (uididx > -1) {
                    $scope.uniqueids.splice(uididx, 1);
                }

                // Remove from table
                $('#roomScheduleTable').bootstrapTable('removeByUniqueId', uniqueId);

                // Update merged cells
                $scope.mergeCells();              
            }

            // ******************
            // Get Data Functions 
            // ******************
            $scope.get = function () {

                // Go through all the rooms and build a list of objects that match the table def...
                var mainct = 0;
                var startIdx = 0;
                angular.forEach($scope.rooms, function (room) {

                    // DEBUG: Only for two rooms for now
                    //if (mainct > 1)
                    //    return;

                    var foundroom = $filter('filter')($scope.rooms, function (data) {
                        return data.RoomID === room.RoomID;
                    });

                    var counter = $scope.roomIdCounter[room.RoomID];
                    counter.rowIdx = startIdx;
                    var ct = 0;
                    $scope.facilityRooms[room.RoomID] = [];
                    angular.forEach($scope.resourceAvailabilities[room.RoomID], function (item) {

                        var tblobj = new Object();
                        tblobj.roomSelect = foundroom[0].IsSchedulable;
                        tblobj.roomName = room.RoomName.trim();
                        tblobj.day = item.Days;
                        tblobj.startTime = $scope.getTime(item.AvailabilityStartTime).hrsmins;
                        tblobj.startAm = $scope.getAmPmValue(item.AvailabilityStartTime);
                        tblobj.endTime = $scope.getTime(item.AvailabilityEndTime).hrsmins;
                        tblobj.endAm = $scope.getAmPmValue(item.AvailabilityEndTime);
                        tblobj.rowID = room.RoomID;
                        tblobj.uniqueId = item.ResourceAvailabilityID;
                        tblobj.rowIndex = ct;
                        tblobj.isAdded = true;
                        tblobj.scheduleType = $scope.formatScheduleType(item.ScheduleType);
                        tblobj.isNew = false;
                        tblobj.isModified = false;
                        $scope.facilityRooms[room.RoomID].push(tblobj);
                        ct++;
                    });
                    startIdx += ct;
                    counter.count = ct - 1;
                    $scope.roomIdCounter[room.RoomID] = counter;

                    // If there are no availabilities for this room, just add an empty one
                    if ($scope.facilityRooms[room.RoomID].length == 0) {
                        $scope.addRow(room.RoomID, room.RoomName, startIdx - 1);
                        $scope.facilityRooms[room.RoomID][0].roomSelect = foundroom[0].IsSchedulable;
                        $scope.facilityRooms[room.RoomID][0].isModified = false;
                        startIdx++;
                    }
                    else {
                        if (mainct == 0)
                            $scope.roomScheduleTable.bootstrapTable('load', $scope.facilityRooms[room.RoomID]);
                        else
                            $scope.roomScheduleTable.bootstrapTable('append', $scope.facilityRooms[room.RoomID]);
                    }                    

                    // Disable the row if this room is not schedulable
                    if (!foundroom[0].IsSchedulable) {
                        
                        $('.room' + room.RoomID).prop('disabled', 'disabled');
                        $('.room' + room.RoomID).addClass('background-theme-grey');

                        $('.room' + room.RoomID + 'ampm > i').addClass('disabled');
                        $('.room' + room.RoomID + 'ampm > label').addClass('disabled');
                        $('.room' + room.RoomID + 'ampm > label').prop('style', 'color:rgb(102,102,102) !important');
                        $('.room' + room.RoomID + 'ampm > i').prop('style', 'color:rgb(102,102,102) !important');
                        $('.row' + room.RoomID + '_0daychange').addClass('disabled');

                        // Enable the checkbox regardless of toggle state
                        var ext = room.RoomID + '_' + counter.rowIdx;
                        $('#roomSelect' + ext).removeAttr('disabled');
                    }
                    foundroom[0].IsModified = false;
                    mainct++;
                });
                $scope.mergeCells();
                $scope.isLoading = false;
                $scope.getFacilitySchedule($scope.selectedFacilityID);
                //$scope.setUniqueIdsList();
                $('.timepicker').timepicker({
                    minuteStep: 1,
                    showInputs: false,
                    showMeridian: false,
                    change: function (time) {
                        $rootScope.validateFutureDate($scope);
                    }
                });
            }

            $scope.getFacility = function (typeName) {
                $scope.facilityData = lookupService.getLookupsByType('Facility');
                angular.forEach($scope.facilityData, function (param) {
                    $scope.locations.push({
                        ID: locationCounter++,
                        LocationID: param.ID,
                        Name: param.Name,
                        type: 1
                    });
                })
            };

            $scope.getLocations = function () {
                var q = $q.defer();
                $scope.getFacility();
                q.resolve();
                resetForm();
                return q.promise;
            }

            $scope.getRooms = function (facilityID, isRefresh) {
                return $scope.resourceService.getRooms(facilityID, isRefresh).then(function (data) {
                    $scope.rooms = data.DataItems;
                    $scope.initializeRoomCounter();
                    $scope.lastRoomID = $scope.rooms[$scope.rooms.length - 1].RoomID;

                    // Now get room availabilities for each room
                    angular.forEach($scope.rooms, function (room) {
                        $scope.getRoomAvailability(room.RoomID, isRefresh);
                    });
                },
                function (errorStatus) {
                    $scope.isLoading = false;
                    alertService.error('Unable to connect to server');
                });
            };

            $scope.getRoomAvailability = function (resourceId, isRefresh) {
                return $scope.resourceService.getResourceAvailability(resourceId, ROOM_RESOURCE_TYPE, isRefresh).then(function (data) {
                    Array.prototype.push.apply($scope.resourceAvailabilities[resourceId], data.DataItems);
                    if (resourceId == $scope.lastRoomID)
                        $scope.get();
                });
            }

            $scope.getRoomOverrides = function (resourceId) {
                return $scope.resourceService.getResourceOverrides(resourceId, ROOM_RESOURCE_TYPE).then(function (data) {
                    Array.prototype.push.apply($scope.resourceOverrides[resourceId], data.DataItems);
                });
            }

            $scope.getRoomDetails = function (resourceId) {
                return $scope.resourceService.getResourceDetails(resourceId, ROOM_RESOURCE_TYPE).then(function (data) {
                    Array.prototype.push.apply($scope.resources, data.DataItems);
                });
            }

            $scope.getLookupsByType = function (typeName) {
                return lookupService.getLookupsByType(typeName);
            };

            $scope.getFacilitySchedule = function (facilityID) {

                return $scope.resourceService.getResourceAvailability(facilityID, FACILITY_RESOURCE_TYPE, false).then(function (data) {
                    $scope.loadFacilityData(data.DataItems);
                    //Array.prototype.push.apply($scope.facilitySchedule, data.DataItems);
                });

                // DEBUG: just fake for now with resource 3 availability schedule
                //if ($scope.facilityRooms[2].length > 0) {
                //    $scope.facilitySchedule = [];
                //    for (var i = 0; i < 3; i++) {
                //        if ($scope.facilityRooms[2][i] == null || $scope.facilityRooms[2][i].day == SCHEDULE_TYPE_SELECT)
                //            continue;
                //        var tblobj = new Object();
                //        tblobj.day = $scope.facilityRooms[2][i].day;
                //        tblobj.startTime = $scope.facilityRooms[2][i].startTime;
                //        tblobj.startAm = $scope.facilityRooms[2][i].startAm;
                //        tblobj.endTime = $scope.facilityRooms[2][i].endTime;
                //        tblobj.endAm = $scope.facilityRooms[2][i].endAm;
                //        tblobj.scheduleType = SCHEDULE_TYPE_FACILITY;
                //        tblobj.roomSelect = true;
                //        $scope.facilitySchedule.push(tblobj);
                //    }
                //}
            }

            $scope.loadFacilityData = function (data) {

                    $scope.facilitySchedule = [];
                for (var i = 0; i < data.length; i++) {
                    if (data[i] == null || data[i].day == SCHEDULE_TYPE_SELECT)
                            continue;
                        var tblobj = new Object();
                    var item = data[i];
                    tblobj.day = item.Days;
                    tblobj.startTime = $scope.getTime(item.AvailabilityStartTime).hrsmins;
                    tblobj.startAm = $scope.getAmPmValue(item.AvailabilityStartTime);
                    tblobj.endTime = $scope.getTime(item.AvailabilityEndTime).hrsmins;
                    tblobj.endAm = $scope.getAmPmValue(item.AvailabilityEndTime);
                    tblobj.scheduleType = SCHEDULE_TYPE_FACILITY;
                    tblobj.roomSelect = true;
                        $scope.facilitySchedule.push(tblobj);
                    }
                }

            // ******************
            // Tmp/Test Functions
            // ******************
            $scope.AddRoomAvailability = function () {

                var roomavail = new Object();
                roomavail.ResourceID = 20;
                roomavail.ResourceTypeID = 1;
                roomavail.FacilityID = 1;
                roomavail.Days = "Friday";
                roomavail.AvailabilityStartTime = "800";
                roomavail.AvailabilityEndTime = "900";
                return $scope.resourceService.addResourceAvailability(roomavail).then(function (data) {
                    $scope.addedRoomAvailabiliyID = data.ID;
                },
                function (errorStatus) {
                    $scope.isLoading = false;
                    alertService.error('Unable to add a room availability!');
                });
            };
            $scope.init(true);
        }]);
