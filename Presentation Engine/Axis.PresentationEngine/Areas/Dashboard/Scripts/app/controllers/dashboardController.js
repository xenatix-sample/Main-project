'use strict';

(function () {
    angular.module('xenatixApp').controller('dashboardController', [
        '$scope',
        '$state',
        'formService',
        '$stateParams',
        '$rootScope',
        'dashboardService',
        'alertService',
        'offlineData',
        '$timeout',
        'clientSearchService',
        '$filter',
        '$log',
        function(
            $scope,
            $state,
            formService,
            $stateParams,
            $rootScope,
            dashboardService,
            alertService,
            offlineData,
            $timeout,
            clientSearchService,
            $filter,
            $log) {

            var self = this;
            var CONTACT_TYPE_SEARCH = '1,4';
            var contactsTable = $("#contactsTable");
            Date.prototype.addMinutes = function (m) {
                this.setMinutes(this.getMinutes() + m);
                return this;
            };

            Date.prototype.addHours = function (h) {
                this.setHours(this.getHours() + h);
                return this;
            };

            Date.prototype.addDays = function (d) {
                this.setHours(this.getHours() + (d * 24));
                return this;
            };

            self.init = function() {
                dashboardService.get().then(function(data) {
                    self.data = data;
                    $log.log("this is the init for dashboard", data);
                    //if (data.DataItems.length === 1) {
                    //    //$scope.UserRolePrimary = data.DataItems[0].UserRolePrimary;
                    //} else {
                    //    //$state.go('offlineLogin');
                    //}
                });
                self.playWithData();

                self.showRecentlyAccessed = true;
                self.showLabs = false;
                self.showAlerts = false;
                self.showRegistrations = true;
                self.initializeBootstrapTable();

                self.getRecentlyAdded("i");
            }
            self.sampleData = [
               {
                   "ContactId": 12,
                   "PrimaryPhone": "940.555.1212",
                   "EmailAddress": "Enid@demoxenatix.com",
                   "CurrentAddress1": "4402 Browning Ln. # 204",
                   "CurrentCity": "Dallas",
                   "CurrentState": "TX",
                   "CurrentZip": "75225"
               },
               {
                   "ContactId": 2,
                   "PrimaryPhone": "777.555.7777",
                   "EmailAddress": "Merle@demoxenatix.com",
                   "CurrentAddress1": "55555 Warren G Harding Way",
                   "CurrentCity": "West Chickopee",
                   "CurrentState": "MA",
                   "CurrentZip": "02211"
               },
               {
                   "ContactId": 8,
                   "PrimaryPhone": "555.555.5555",
                   "EmailAddress": "Glenn@demoxenatix.com",
                   "CurrentAddress1": "111 Jefferson Ln. ",
                   "CurrentCity": "Macon",
                   "CurrentState": "GA",
                   "CurrentZip": "95225"
               },
               {
                   "ContactId": 7,
                   "PrimaryPhone": "333.555.3333",
                   "EmailAddress": "Maggie@demoxenatix.com",
                   "CurrentAddress1": "22222 Washington Blvd",
                   "CurrentCity": "Des Moines",
                   "CurrentState": "IA",
                   "CurrentZip": "99399"
               },
               {
                   "ContactId": 3,
                   "PrimaryPhone": "222.555.2222",
                   "EmailAddress": "Carl@demoxenatix.com",
                   "CurrentAddress1": "55302 Lincoln Way #2554",
                   "CurrentCity": "Rio de Janeiro",
                   "CurrentState": "MS",
                   "CurrentZip": "55225"
               }
            ];

            //this is temporary but we need a method to return the same data but just for recently added
            self.getRecentlyAdded = function (searchText) {
                if (searchText != undefined && searchText !== "") {
                    clientSearchService.getClientSummary(searchText, CONTACT_TYPE_SEARCH).then(function (data) {
                        self.recentlyAccessedList = self.bindRecentlyAccessed(data.DataItems);

                        if (self.recentlyAccessedList != null && self.recentlyAccessedList.length > 0) {

                        }
                    }, function (errorStatus) {
                        alertService.error('Unable to connect to server');
                    });
                }
            };

            self.bindRecentlyAccessed = function (contactList) {
                $log.log("contactList", contactList);
                var rtn = {
                        "QueueTypeId": 1,
                        "QueueName": "Recently Accessed",
                        "QueueItems": []
                };
                var i = 1;
                angular.forEach(contactList, function (item) {
                    if (i > 5) {
                        //only 5 records
                        return;
                    }
                    var dateObj = new Date().addDays(-i).addHours(i + 2);
                    var phone = "(not provided)",
                        address1 = "(not provided)",
                        city = "",
                        state = "",
                        zip = "",
                        email = "(not provided)";

                    //todo: this needs to attach to real data
                    if (item["Addresses"].length > 0) {
                        //address1 = item["Addresses"][0].
                    } else {
                        address1 = self.sampleData[i-1].CurrentAddress1;
                        city = self.sampleData[i-1].CurrentCity;
                        state = self.sampleData[i-1].CurrentState;
                        zip = self.sampleData[i-1].CurrentZip;
                    }
                    
                    //todo: this needs to attach to real data
                    if (item["Emails"].length > 0) {
                        //address1 = item["Addresses"][0].
                    } else {
                        email = item["FirstName"] + "." + item["LastName"] + "@demoxenatix.com";
                    }

                    //todo: this needs to attach to real data
                    if (item["Phones"].length > 0) {
                    } else {
                        phone = self.sampleData[i - 1].PrimaryPhone;
                    }

                    var queueItem = {};
                    queueItem["QueueTypeId"] = 1;
                    queueItem["QueueId"] = i;
                    queueItem["Label"] = item["FirstName"] + " " + item["LastName"];
                    //todo: this needs to attach to real data
                    queueItem["Description"] = moment.utc(dateObj).format("dddd, MMMM Do YYYY, h:mm:ss a");
                    queueItem["MetaData"] = {};
                    queueItem["MetaData"]["ContactId"] = item["ContactID"];
                    queueItem["MetaData"]["PrimaryPhone"] = phone;
                    queueItem["MetaData"]["EmailAddress"] = email;
                    queueItem["MetaData"]["CurrentAddress1"] = address1;
                    queueItem["MetaData"]["CurrentCity"] = city;
                    queueItem["MetaData"]["CurrentState"] = state;
                    queueItem["MetaData"]["CurrentZip"] = zip;

                    rtn["QueueItems"].push(queueItem);
                    i += 1;
                });
                self.queueInfo["Queues"].push(rtn);
                $timeout(offCanvasNav.init);
                $log.log("queue", self.queueInfo);
            };

           

            self.getGreeting = function() {
                var today = new Date();
                var msg = "Good ";
                var timeOfDay = "Morning";
                var hourOfTheDay = today.getHours();
                if (hourOfTheDay >= 12 && hourOfTheDay < 17) {
                    timeOfDay = "Afternoon";
                } else if (hourOfTheDay >= 17) {
                    timeOfDay = "Evening";
                }
                return msg + timeOfDay;
            };

            self.productivity = {
                "Registrations": {
                    "Total": 11,
                    "Completed": 2
                },
                "Assesments": {
                    "Total": 21,
                    "Completed": 14
                },
                "ProgressNotes": {
                    "Total": 52,
                    "Completed": 45
                }
            };

            self.queueInfo = {
                "Queues": [
                    //{
                    //    "QueueTypeId": 1,
                    //    "QueueName": "Recently Accessed",
                    //    "QueueItems": [
                    //        {
                    //            "QueueTypeId": 1,
                    //            "QueueId": 1,
                    //            "CreatedDateTime": "1/3/2016 5:12pm",
                    //            "Label": "Enid Walters",
                    //            "Description": "1/06/2016 4:12pm",
                    //            "MetaData": {
                    //                "ContactId": 12,
                    //                "PrimaryPhone": "940.555.1212",
                    //                "EmailAddress": "Enid@demoxenatix.com",
                    //                "CurrentAddress1": "4402 Browning Ln. # 204",
                    //                "CurrentCity": "Dallas",
                    //                "CurrentState": "TX",
                    //                "CurrentZip": "75225"
                    //            }
                    //        },
                    //        {
                    //            "QueueTypeId": 1,
                    //            "QueueId": 2,
                    //            "CreatedDateTime": "1/3/2016 5:12pm",
                    //            "Label": "Merle Dixon",
                    //            "Description": "1/05/2016 7:12pm",
                    //            "MetaData": {
                    //                "ContactId": 2,
                    //                "PrimaryPhone": "777.555.7777",
                    //                "EmailAddress": "Merle@demoxenatix.com",
                    //                "CurrentAddress1": "55555 Warren G Harding Way",
                    //                "CurrentCity": "West Chickopee",
                    //                "CurrentState": "MA",
                    //                "CurrentZip": "02211"
                    //            }
                    //        },
                    //        {
                    //            "QueueTypeId": 1,
                    //            "QueueId": 3,
                    //            "CreatedDateTime": "1/3/2016 5:12pm",
                    //            "Label": "Glenn Rhee",
                    //            "Description": "1/05/2016 11:12am",
                    //            "MetaData": {
                    //                "ContactId": 8,
                    //                "PrimaryPhone": "555.555.5555",
                    //                "EmailAddress": "Glenn@demoxenatix.com",
                    //                "CurrentAddress1": "111 Jefferson Ln. ",
                    //                "CurrentCity": "Macon",
                    //                "CurrentState": "GA",
                    //                "CurrentZip": "95225"
                    //            }
                    //        },
                    //        {
                    //            "QueueTypeId": 1,
                    //            "QueueId": 4,
                    //            "CreatedDateTime": "1/3/2016 5:12pm",
                    //            "Label": "Maggie Greene",
                    //            "Description": "12/30/2015 1:11pm",
                    //            "MetaData": {
                    //                "ContactId": 7,
                    //                "PrimaryPhone": "333.555.3333",
                    //                "EmailAddress": "Maggie@demoxenatix.com",
                    //                "CurrentAddress1": "22222 Washington Blvd",
                    //                "CurrentCity": "Des Moines",
                    //                "CurrentState": "IA",
                    //                "CurrentZip": "99399"
                    //            }
                    //        },
                    //        {
                    //            "QueueTypeId": 1,
                    //            "QueueId": 5,
                    //            "CreatedDateTime": "1/3/2016 5:12pm",
                    //            "Label": "Carl Grimes",
                    //            "Description": "10/3/2015 9:12pm",
                    //            "MetaData": {
                    //                "ContactId": 3,
                    //                "PrimaryPhone": "222.555.2222",
                    //                "EmailAddress": "Carl@demoxenatix.com",
                    //                "CurrentAddress1": "55302 Lincoln Way #2554",
                    //                "CurrentCity": "Rio de Janeiro",
                    //                "CurrentState": "MS",
                    //                "CurrentZip": "55225"
                    //            }
                    //        }
                    //    ]
                    //},
                    {
                        "QueueTypeId": 3,
                        "QueueName": "Registrations",
                        "QueueItems": [
                            {
                                "QueueTypeId": 3,
                                "QueueId": 11,
                                "CreatedBy": 1,
                                "CreatedByName": "Enid Walters",
                                "CreatedDateTime": "1/3/2016 5:12pm",
                                "Label": "Bob Stooky",
                                "Description": "2 Fields need to be completed"
                            },
                            {
                                "QueueTypeId": 3,
                                "QueueId": 12,
                                "CreatedBy": 1,
                                "CreatedByName": "Enid Walters",
                                "CreatedDateTime": "1/3/2016 5:12pm",
                                "Label": "Dale Horvath",
                                "Description": "3 Fields need to be completed"
                            },
                            {
                                "QueueTypeId": 3,
                                "QueueId": 13,
                                "CreatedBy": 1,
                                "CreatedByName": "Enid Walters",
                                "CreatedDateTime": "1/3/2016 5:12pm",
                                "Label": "Shane Walsh",
                                "Description": "8 Fields need to be completed"
                            },
                            {
                                "QueueTypeId": 3,
                                "QueueId": 14,
                                "CreatedBy": 1,
                                "CreatedByName": "Enid Walters",
                                "CreatedDateTime": "1/3/2016 5:12pm",
                                "Label": "Terrance Douglas",
                                "Description": "2 Fields need to be completed"
                            },
                            {
                                "QueueTypeId": 3,
                                "QueueId": 15,
                                "CreatedBy": 1,
                                "CreatedByName": "Enid Walters",
                                "CreatedDateTime": "1/3/2016 5:12pm",
                                "Label": "Deanna Monroe",
                                "Description": "1 Field needs to be completed"
                            }
                        ]
                    },
                    {
                        "QueueTypeId": 2,
                        "QueueName": "Alerts",
                        "QueueItems": [
                            {
                                "QueueTypeId": 2,
                                "QueueId": 6,
                                "CreatedBy": 1,
                                "CreatedByName": "Enid Walters",
                                "CreatedDateTime": "1/3/2016 5:12pm",
                                "SeverityCode": 1,
                                "Label": "Required",
                                "Description": "Primary Insurance Expired"
                            },
                            {
                                "QueueTypeId": 2,
                                "QueueId": 7,
                                "CreatedBy": 1,
                                "CreatedByName": "Enid Walters",
                                "CreatedDateTime": "1/3/2016 5:12pm",
                                "SeverityCode": 2,
                                "Label": "Warning",
                                "Description": "No Emergency Contact Set"
                            },
                            {
                                "QueueTypeId": 2,
                                "QueueId": 8,
                                "CreatedBy": 1,
                                "CreatedByName": "Enid Walters",
                                "CreatedDateTime": "1/3/2016 5:12pm",
                                "SeverityCode": 2,
                                "Label": "Warning",
                                "Description": "Password will expire in 7 days"
                            },
                            {
                                "QueueTypeId": 2,
                                "QueueId": 9,
                                "CreatedBy": 1,
                                "CreatedByName": "Enid Walters",
                                "CreatedDateTime": "1/3/2016 5:12pm",
                                "SeverityCode": 3,
                                "Label": "Informational",
                                "Description": "You have 2 Appointments today"
                            },
                            {
                                "QueueTypeId": 2,
                                "QueueId": 10,
                                "CreatedBy": 1,
                                "CreatedByName": "Enid Walters",
                                "CreatedDateTime": "1/3/2016 5:12pm",
                                "SeverityCode": 3,
                                "Label": "Informational",
                                "Description": "ECI Registrations due in 5 days"
                            }
                        ]
                    },
                    {
                        "QueueTypeId": 4,
                        "QueueName": "Labs",
                        "QueueItems": [
                            {
                                "QueueTypeId": 4,
                                "QueueId": 16,
                                "CreatedBy": 1,
                                "CreatedByName": "Enid Walters",
                                "CreatedDateTime": "1/3/2016 5:12pm",
                                "Label": "TB Lab",
                                "Description": "Results Not Received"
                            },
                            {
                                "QueueTypeId": 4,
                                "QueueId": 17,
                                "CreatedBy": 1,
                                "CreatedByName": "Enid Walters",
                                "CreatedDateTime": "1/3/2016 5:12pm",
                                "Label": "Toxicology Lab",
                                "Description": "Results Received"
                            },
                            {
                                "QueueTypeId": 4,
                                "QueueId": 18,
                                "CreatedBy": 1,
                                "CreatedByName": "Enid Walters",
                                "CreatedDateTime": "1/3/2016 5:12pm",
                                "Label": "LIPO HCL Lab",
                                "Description": "Results Received"
                            },
                            {
                                "QueueTypeId": 4,
                                "QueueId": 19,
                                "CreatedBy": 1,
                                "CreatedByName": "Enid Walters",
                                "CreatedDateTime": "1/3/2016 5:12pm",
                                "Label": "Chemistry Lab",
                                "Description": "Results Not Received"
                            },
                            {
                                "QueueTypeId": 4,
                                "QueueId": 20,
                                "CreatedBy": 1,
                                "CreatedByName": "Enid Walters",
                                "CreatedDateTime": "1/3/2016 5:12pm",
                                "Label": "Physics Lab",
                                "Description": "Results Received"
                            }
                        ]
                    }
                ]
            };


            self.selectedQueueItem = self.queueInfo.Queues[0].QueueItems[0];


            self.Tasks = [
                {
                    TaskId: 1,
                    TypeCodeId: 4,
                    Name: "Maggie Greene",
                    Description: ""
                },
                {
                    TypeId: 2,
                    TypeCodeId: 2,
                    ContactFullName: "Sgt. Abraham Ford"
                },
                {
                    TypeId: 3,
                    TypeCodeId: 3,
                    ContactFullName: "Glenn Rhee"
                },
                {
                    TypeId: 4,
                    TypeCodeId: 1,
                    ContactFullName: "Daryl Dixon"
                },
                {
                    TypeId: 5,
                    TypeCodeId: 4,
                    ContactFullName: "Rick Grimes"
                },
                {
                    TypeId: 6,
                    TypeCodeId: 3,
                    ContactFullName: "Michonne"
                },
                {
                    TypeId: 7,
                    TypeCodeId: 1,
                    ContactFullName: "Beth Greene"
                },
                {
                    TypeId: 8,
                    TypeCodeId: 3,
                    ContactFullName: "Merle Dixon"
                }
            ];

            self.selectedTask = self.Tasks[0];
            //self.currentTypeCode = 2;
            //selectedTask.TypeCodeId
            self.playWithData = function() {
                angular.forEach(self.Tasks, function(item) {

                    switch (item.TypeCodeId) {
                        case 1:
                            item["TypeCode"] = "Recently Accessed";
                            item["DashboardData"] = "Recently Accessed";
                        break;
                    case 2:
                        item["TypeCode"] = "Alerts";
                        break;
                    case 3:
                        item["TypeCode"] = "Registrations";
                        item["ContactFullName"] += " - 3PM";
                        break;
                    case 4:
                        item["TypeCode"] = "Labs";
                        item["ContactFullName"] += " - 12/21/2015 7:12AM";
                        break;
                    }
                });

                self.productivity["AssesmentsCompletedPercent"] = (self.productivity.Assesments.Completed / self.productivity.Assesments.Total) * 100;
                self.productivity["AssesmentsNotCompletedPercent"] = 100 - self.productivity["AssesmentsCompletedPercent"];

                self.productivity["RegistrationsCompletedPercent"] = (self.productivity.Registrations.Completed / self.productivity.Registrations.Total) * 100;
                self.productivity["RegistrationsNotCompletedPercent"] = 100 - self.productivity["RegistrationsCompletedPercent"];

                self.productivity["ProgressNotesCompletedPercent"] = (self.productivity.ProgressNotes.Completed / self.productivity.ProgressNotes.Total) * 100;
                self.productivity["ProgressNotesNotCompletedPercent"] = 100 - self.productivity["ProgressNotesCompletedPercent"];

                //<div class="progress-bar progress-bar-success" style="width: {{vm.productivity.AssesmentsCompletedPercent}}%"><span class="sr-only">&nbsp;</span></div>
                //<div class="progress-bar" style="width: {{vm.productivity.AssesmentsNotCompletedPercent}}%"><

            };

            self.EnableDisableEnterKey = function(searchText, isSearch) {
                if (searchText != undefined && searchText !== "") {
                    self.resetEnterKey();
                    if (isSearch)
                        self.getClientSummary(searchText);
                } else {
                    self.enterKeyStop = false;
                    self.stopNext = false;
                    self.saveOnEnter = true;
                }
            }

            self.stopEnterKey = function() {
                if (!$('#contactListModel').hasClass('in')) {
                    if ($state.current.name.toLowerCase().indexOf('patientprofile') >= 0) {
                        self.enterKeyStop = true;
                        self.stopNext = false;
                        self.saveOnEnter = true;
                    } else {
                        self.enterKeyStop = false;
                        self.stopNext = false;
                        self.saveOnEnter = true;
                    }
                } else
                    self.resetEnterKey();
            }

            self.resetEnterKey = function() {
                self.enterKeyStop = true;
                self.stopNext = false;
                self.saveOnEnter = false;
            }



            //Get the contact detail based on the search text
            self.getClientSummary = function(searchText) {
                if (searchText != undefined && searchText !== "") {

                    clientSearchService.getClientSummary(searchText, CONTACT_TYPE_SEARCH).then(function(data) {
                        self.contactList = self.bindDataModel(data.DataItems, false);

                        if (self.contactList != null && self.contactList.length > 0) {

                            //$scope.collateralList.forEach(function (element, pos) {

                            //    var idx = self.contactList.map(function (contact) {
                            //        return contact.ContactID;
                            //    }).indexOf(element.ContactID);
                            //    if (idx > -1)
                            //        self.contactList.splice(idx, 1);
                            //});

                            $('#contactListModel').on('hidden.bs.modal', function() {
                                self.stopEnterKey();
                                var focus = $('#FirstName').is(":focus");
                                if (!focus) {
                                    $('#txtClientSearch').focus();
                                }
                            });
                            if (self.contactList != null && self.contactList.length > 0) {
                                contactsTable.bootstrapTable('load', self.contactList);
                                $('#contactListModel').modal('show');
                                $('#contactListModel').on('shown.bs.modal', function() {
                                    self.resetEnterKey();
                                    $rootScope.setFocusToGrid('contactsTable');
                                });

                            } else {
                                contactsTable.bootstrapTable('removeAll');
                            }
                        } else {
                            contactsTable.bootstrapTable('removeAll');
                        }
                    }, function(errorStatus) {
                        alertService.error('Unable to connect to server');
                    });
                } else {
                    self.stopEnterKey();
                }
            };

            self.bindDataModel = function(model, showCurrentUser) {
                //$scope.initCollateral();
                var listToBind = model;
                //if ($scope.GenderList == undefined || $scope.GenderList == null)
                //    $scope.GenderList = $scope.getLookupsByType('Gender');
                //if (!showCurrentUser) {
                //    listToBind = removeContactFromList(listToBind, $scope.contactID);
                //}
                //angular.forEach(listToBind, function (collateral) {
                //    collateral.DOB = $filter('toMMDDYYYYDate')(collateral.DOB, 'MM/DD/YYYY');
                //    if (collateral.GenderID > 0)
                //        collateral.GenderText = getGenderText(collateral.GenderID)[0].Name;
                //});
                return listToBind;
            };


            self.initializeBootstrapTable = function () {

                $scope.tableoptions = {
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
                            title: 'MI'
                        },
                        {
                            field: 'DOB',
                            title: 'Date Of Birth',
                            formatter: function (value, row, index) {
                                if (value) {
                                    var formattedDate = $filter('toMMDDYYYYDate')(value, 'MM/DD/YYYY', 'useLocal');
                                    return formattedDate;
                                } else
                                    return '';
                            }
                        },
                        {
                            field: 'GenderText',
                            title: 'Gender'
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
                            title: '',
                            formatter: function (value, row, index) {
                                var hasMRN = false;
                                if (row["MRN"] !== null && row["MRN"] !== undefined) {
                                    hasMRN = true;
                                }

                                //registration/profile
                                return (hasMRN
                                        ? '<a href="javascript:void(0)" data-default-action ui-sref="patientprofile.general({ ContactID: ' + value + ' })" alt="Profile"  space-key-press></a>' 
                                        : '<a href="javascript:void(0)" data-default-action ui-sref="registration.demographics({ ContactID: ' + value + ' })" alt="View Contact"  space-key-press></a>'
                                    );
                            }
                        }
                    ]
                };
            };

            self.closeModal = function () {
                $('#contactListModel').modal('hide');
                self.searchText = '';
            };

            self.toggleTaskFilter = function () {
                $("#taskFilterPopup").toggleClass('in').toggleClass('collapse');
                //inset_menu in collapse
                // el.closest(".tile-flyout").find("[tile-flyout].active").not(el).trigger('click');
                //$(scope.tileFlyout).toggleClass('in').toggleClass('collapse');
                //el.toggleClass('active');
            };

            self.init();
        }
    ]);
}());