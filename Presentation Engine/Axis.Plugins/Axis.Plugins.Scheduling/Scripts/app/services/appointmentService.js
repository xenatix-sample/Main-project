angular.module('xenatixApp')
    .factory('appointmentService', ["$http", "$q", 'settings', 'offlineData', function ($http, $q, settings, offlineData) {
        var CONFIG = {
            controllerAction: "/data/plugins/scheduling/Appointment/",
            offlineApiUrl: '/Appointments/',
            offlineByResApiUrl: '/Appointments/ByResource/',
            offlineDetailUrl: '/Appointment/Detail/',
            offlineResourceUrl: '/Appointment/Resource/',
            offlineContactUrl: '/Appointment/Contact/',
            offlineResourceByContactUrl: '/Appointment/ResourceByContact/',
            offlineApptLengthUrl: '/Appointment/Length/',
            offlineApptTypeUrl: '/Appointment/Type/',
            offlineCalendarApiUrl: '/ApptsByDate/',
            offlineApptNoteUrl: '/Appointment/Note/',
            offlineApptStatusUrl: '/Appointment/Status/',
            offlineApptResourceUrl: '/AppointmentResource/'
        };

        var editStateFunc = function editStateSettings() {
            return {
                description: 'Appointment ',
                state: 'patientprofile.appointments.editAppointment',
                stateParams: { ContactID: this.ContactID, AppointmentID: this.AppointmentID }
            };
        };

        function getResourceAppointmentsByWeek(resourceId, resourceTypeId, startDate) {
            var dfd = $q.defer();
            startDate = new Date(startDate);
            var timeString = startDate.getTime().toString();
            startDate = moment.utc(startDate).format('MM/DD/YYYY');
            $http.get(settings.webApiBaseUrl + CONFIG.controllerAction + 'getResourceAppointmentsByWeek', { data: offlineData.getOfflineSettings(CONFIG.offlineCalendarApiUrl + timeString, 'FacilityID', { childKey: 'AppointmentID' }), params: { resourceId: resourceId, resourceTypeId: resourceTypeId, startDate: startDate } })
            .success(function (data, status, header, config) {
                dfd.resolve(data);
            })
            .error(function (data, status, header, config) {
                dfd.reject(status);
            });

            return dfd.promise;
        };

        function getAppointmentsByDate(scheduleDate) {
            var dfd = $q.defer();
            scheduleDate = new Date(scheduleDate);
            var timeString = scheduleDate.getTime().toString();
            scheduleDate = moment.utc(scheduleDate).format('MM/DD/YYYY');
            $http.get(settings.webApiBaseUrl + CONFIG.controllerAction + 'getAppointmentsByDate', { data: offlineData.getOfflineSettings(CONFIG.offlineCalendarApiUrl + timeString, 'FacilityID', { childKey: 'AppointmentID' }), params: { scheduleDate: scheduleDate } })
            .success(function (data, status, header, config) {
                dfd.resolve(data);
            })
            .error(function (data, status, header, config) {
                dfd.reject(status);
            });

            return dfd.promise;
        };

        function getContactsByAppointment(appointmentId) {
            var dfd = $q.defer();
            $http.get(settings.webApiBaseUrl + CONFIG.controllerAction + 'getContactsByAppointment', { data: offlineData.getOfflineSettings(CONFIG.offlineContactUrl + appointmentId, 'AppointmentID', { childKey: 'AppointmentContactID' }), params: { appointmentId: appointmentId } })
            .success(function (data, status, header, config) {
                dfd.resolve(data);
            })
            .error(function (data, status, header, config) {
                dfd.reject(status);
            });

            return dfd.promise;
        };

        function getAppointments(contactId) {
            var dfd = $q.defer();

            $http.get(settings.webApiBaseUrl + CONFIG.controllerAction + 'getAppointments', {
                data: offlineData.getOfflineSettings(CONFIG.offlineApiUrl + (contactId || 0).toString(), 'ContactID', {  childKey: 'AppointmentID' }), params: { contactId: contactId }
            })
            .success(function (data, status, header, config) {
                dfd.resolve(data);
            })
            .error(function (data, status, header, config) {
                dfd.reject(status);
            });

            return dfd.promise;
        };

        function getAppointment(contactId, appointmentId) {
            var dfd = $q.defer();

            $http.get(settings.webApiBaseUrl + CONFIG.controllerAction + 'getAppointment', { data: offlineData.getOfflineSettings(CONFIG.offlineApiUrl + (contactId || 0).toString() + '/' + (appointmentId || 0).toString(), 'AppointmentID', { childKey: 'AppointmentID' }), params: { appointmentId: appointmentId } })
            .success(function (data, status, header, config) {
                dfd.resolve(data);
            })
            .error(function (data, status, header, config) {
                dfd.reject(status);
            });

            return dfd.promise;
        };

        function getAppointmentByResource(resourceId, resourceTypeId) {
            var dfd = $q.defer();

            $http.get(settings.webApiBaseUrl + CONFIG.controllerAction + 'getAppointmentByResource', { data: offlineData.getOfflineSettings(CONFIG.offlineByResApiUrl + (resourceTypeId || 0).toString() + '/' + (resourceId || 0).toString(), 'AppointmentID', { parentKey: 'ContactID' }), params: { resourceId: resourceId, resourceTypeId: resourceTypeId } })
            .success(function (data, status, header, config) {
                dfd.resolve(data);
            })
            .error(function (data, status, header, config) {
                dfd.reject(status);
            });

            return dfd.promise;
        };

        function getBlockedTimeAppointments(resourceId, resourceTypeId) {
            var dfd = $q.defer();

            $http.get(settings.webApiBaseUrl + CONFIG.controllerAction + 'getBlockedTimeAppointments', { data: offlineData.getOfflineSettings(CONFIG.offlineByResApiUrl + (resourceTypeId || 0).toString() + '/' + (resourceId || 0).toString(), 'AppointmentID', { parentKey: 'ContactID' }), params: { resourceId: resourceId, resourceTypeId: resourceTypeId } })
            .success(function (data, status, header, config) {
                dfd.resolve(data);
            })
            .error(function (data, status, header, config) {
                dfd.reject(status);
            });

            return dfd.promise;
        };

        function getAppointmentResource(appointmentId) {
            var dfd = $q.defer();

            $http.get(settings.webApiBaseUrl + CONFIG.controllerAction + 'getAppointmentResource', { data: offlineData.getOfflineSettings(CONFIG.offlineResourceUrl + (appointmentId || 0).toString(), 'AppointmentID', { childKey: 'AppointmentResourceID' }), params: { appointmentId: appointmentId } })

            .success(function (data, status, header, config) {
                dfd.resolve(data);
            })
            .error(function (data, status, header, config) {
                dfd.reject(status);
            });

            return dfd.promise;
        };

        function getAppointmentResourceByContact(contactId) {
            var dfd = $q.defer();

            $http.get(settings.webApiBaseUrl + CONFIG.controllerAction + 'getAppointmentResourceByContact', { data: offlineData.getOfflineSettings(CONFIG.offlineResourceByContactUrl + (contactId || 0).toString(), 'AppointmentResourceID', { parentKey: 'AppointmentID' }), params: { contactId: contactId } })
            .success(function (data, status, header, config) {
                dfd.resolve(data);
            })
            .error(function (data, status, header, config) {
                dfd.reject(status);
            });

            return dfd.promise;
        };

        function getAppointmentLength(appointmentTypeID) {
            var dfd = $q.defer();

            $http.get(settings.webApiBaseUrl + CONFIG.controllerAction + 'getAppointmentLength', { data: offlineData.getOfflineSettings(CONFIG.offlineApptLengthUrl + (appointmentTypeID || 0).toString(), 'AppointmentTypeID', { childKey: 'AppointmentLengthID' }), params: { appointmentTypeID: appointmentTypeID } })
            .success(function (data, status, header, config) {
                dfd.resolve(data);
            })
            .error(function (data, status, header, config) {
                dfd.reject(status);
            });

            return dfd.promise;
        };

        function getAppointmentType(programId) {
            var dfd = $q.defer();
            $http.get(settings.webApiBaseUrl + CONFIG.controllerAction + 'getAppointmentType', { data: offlineData.getOfflineSettings(CONFIG.offlineApptTypeUrl + (programId || 0).toString(), 'ProgramID', { childKey: 'AppointmentTypeID' }), params: { programId: programId } })
            .success(function (data, status, header, config) {
                dfd.resolve(data);
            })
            .error(function (data, status, header, config) {
                dfd.reject(status);
            });

            return dfd.promise;
        };

        function addAppointment(appointment) {
            var dfd = $q.defer();
            if (!('AppointmentID' in appointment))
                appointment.AppointmentID = 0;
            var data = $.extend(true, {}, appointment, offlineData.getOfflineSettings(CONFIG.offlineApiUrl + (appointment.ContactID || 0).toString() + '/' + (appointment.AppointmentID || 0).toString(), 'AppointmentID', { parentKey: 'ContactID', editState: editStateFunc.toString() }));
            
            $http.post(settings.webApiBaseUrl + CONFIG.controllerAction + 'addAppointment', data)
            .success(function (data, status, header, config) {
                dfd.resolve(data);
            })
            .error(function (data, status, header, config) {
                dfd.reject(status);
            });

            return dfd.promise;
        };

        function addAppointmentContact(appointmentContact) {
            var dfd = $q.defer();
            if (!('AppointmentContactID' in appointmentContact))
                appointmentContact.AppointmentContactID = 0;
            var data = $.extend(true, {}, appointmentContact, offlineData.getOfflineSettings(CONFIG.offlineContactUrl + (appointmentContact.AppointmentID || 0).toString() + '/' + (appointmentContact.AppointmentContactID || 0).toString(), 'AppointmentContactID', { primaryKey: 'AppointmentResourceID', parentKey: 'AppointmentID', referenceKeys: ["ContactID", "AppointmentID"], editState: editStateFunc.toString() }));
            $http.post(settings.webApiBaseUrl + CONFIG.controllerAction + 'addAppointmentContact', data)
            .success(function (data, status, header, config) {
                dfd.resolve(data);
            })
            .error(function (data, status, header, config) {
                dfd.reject(status);
            });

            return dfd.promise;
        };

        function addAppointmentResource(appointmentResource) {
            var dfd = $q.defer();
            if (!('AppointmentResourceID' in appointmentResource))
                appointmentResource.AppointmentResourceID = 0;
            var data = $.extend(true, {}, appointmentResource, offlineData.getOfflineSettings(CONFIG.offlineResourceUrl + (appointmentResource.AppointmentID || 0).toString() + '/' + (appointmentResource.AppointmentResourceID || 0).toString(), 'AppointmentResourceID', { parentKey: 'AppointmentID', referenceKeys: ["AppointmentID", "ContactID"], editState: editStateFunc.toString() }));
            $http.post(settings.webApiBaseUrl + CONFIG.controllerAction + 'addAppointmentResource', data)
            .success(function (data, status, header, config) {
                dfd.resolve(data);
            })
            .error(function (data, status, header, config) {
                dfd.reject(status);
            });

            return dfd.promise;
        };

        function updateAppointment(appointment) {

            var dfd = $q.defer();
            var data = $.extend(true, {}, appointment, offlineData.getOfflineSettings(CONFIG.offlineApiUrl + (appointment.ContactID || 0).toString() + '/' + (appointment.AppointmentID || 0).toString(), 'AppointmentID', {
                parentKey: 'ContactID', editState: editStateFunc.toString()
                }));
            $http.put(settings.webApiBaseUrl + CONFIG.controllerAction + 'updateAppointment', data)
            .success(function(data, status, header, config) {
                dfd.resolve(data);
            })
            .error(function(data, status, header, config) {
                dfd.reject(status);
            });

            return dfd.promise;
        };

        function updateAppointmentContact(appointmentContact) {
            var dfd = $q.defer();
            var data = $.extend(true, {}, appointmentContact, offlineData.getOfflineSettings(CONFIG.offlineContactUrl + (appointmentContact.AppointmentID || 0).toString() + '/' + (appointmentContact.AppointmentContactID || 0).toString(), 'AppointmentContactID', { parentKey: 'AppointmentID', editState: editStateFunc.toString() }));
            $http.put(settings.webApiBaseUrl + CONFIG.controllerAction + 'updateAppointmentContact', data)
            .success(function(data, status, header, config) {
                dfd.resolve(data);
            })
            .error(function(data, status, header, config) {
                dfd.reject(status);
            });
            return dfd.promise;
        };

        function updateAppointmentResource(appointmentResource) {
            var dfd = $q.defer();
             var data = $.extend(true, {}, appointmentResource, offlineData.getOfflineSettings(CONFIG.offlineResourceUrl + (appointmentResource.AppointmentID || 0).toString() + '/' + (appointmentResource.AppointmentResourceID || 0).toString(), 'AppointmentResourceID', {
                 parentKey: 'AppointmentID', referenceKeys: ["AppointmentID"], editState: editStateFunc.toString()
             }));

            $http.put(settings.webApiBaseUrl + CONFIG.controllerAction + 'updateAppointmentResource', data)
            .success(function(data, status, header, config) {
                dfd.resolve(data);
            })
            .error(function(data, status, header, config) {
                dfd.reject(status);
            });

            return dfd.promise;
        };

        function deleteAppointment(contactId, appointmentId) {
            var dfd = $q.defer();
            $http.delete(settings.webApiBaseUrl + CONFIG.controllerAction + 'deleteAppointment', { data: offlineData.getOfflineSettings(CONFIG.offlineApiUrl + (contactId || 0).toString() + '/' + (appointmentId || 0).toString(), 'AppointmentID', { parentKey: 'ContactID' }), params: { id: appointmentId, modifiedOn: moment.utc().format() } })
                .success(function(data, status, header, config) {
                    dfd.resolve(data);
                })
                .error(function(data, status, header, config) {
                    dfd.reject(status);
                });

            return dfd.promise;
        };

        function deleteAppointmentsByRecurrence(id) {
            var dfd = $q.defer();
            $http.delete(settings.webApiBaseUrl + CONFIG.controllerAction + 'deleteAppointmentsByRecurrence',
                {
                    data: offlineData.getOfflineSettings(CONFIG.offlineApiUrl + (id || 0).toString(), 'AppointmentID'), params: { id: id}
                })
                .success(function (data, status, header, config) {
                    dfd.resolve(data);
                })
                .error(function (data, status, header, config) {
                    dfd.reject(status);
                });

            return dfd.promise;
        };

        function addAppointmentNote(appointmentNote) {
            var dfd = $q.defer();
            if (!('AppointmentNoteID' in appointmentNote))
                appointmentNote.AppointmentNoteID = 0;
            var data = $.extend(true, {}, appointmentNote, offlineData.getOfflineSettings(CONFIG.offlineApptNoteUrl + (appointmentNote.AppointmentID || 0).toString() + '/' + (appointmentNote.AppointmentNoteID || 0).toString(), 'AppointmentNoteID', {
                parentKey: 'AppointmentID', referenceKeys: ["AppointmentID", "ContactID"]
            }));

            $http.post(settings.webApiBaseUrl + CONFIG.controllerAction + 'addAppointmentNote', data)
            .success(function (data, status, header, config) {
                dfd.resolve(data);
            })
            .error(function (data, status, header, config) {
                dfd.reject(status);
            });

            return dfd.promise;
        };

        function updateAppointmentNote(appointmentNote) {
            var dfd = $q.defer();
            var data = $.extend(true, {}, appointmentNote, offlineData.getOfflineSettings(CONFIG.offlineApptNoteUrl + (appointmentNote.AppointmentID || 0).toString() + '/' + (appointmentNote.AppointmentNoteID || 0).toString(), 'AppointmentNoteID', {
                parentKey: 'AppointmentID', referenceKeys: ["AppointmentID", "ContactID"]
            }));

            $http.put(settings.webApiBaseUrl + CONFIG.controllerAction + 'updateAppointmentNote', data)
            .success(function (data, status, header, config) {
                dfd.resolve(data);
            })
            .error(function (data, status, header, config) {
                dfd.reject(status);
            });

            return dfd.promise;
        };

        function deleteAppointmentNote(appointmendId, appointmentNoteId) {
            var dfd = $q.defer();
            $http.delete(settings.webApiBaseUrl + CONFIG.controllerAction + 'deleteAppointmentNote',
                {
                    data: offlineData.getOfflineSettings(CONFIG.offlineApptNoteUrl + (appointmendId || 0).toString() + '/' + (appointmentNoteId || 0).toString(), 'AppointmentNoteID',
                      {
                          parentKey: 'AppointmentID', referenceKeys: ["AppointmentID", "ContactID"]
                      }), params: { id: appointmentNoteId, modifiedOn: moment.utc().format() }
                })
                .success(function (data, status, header, config) {
                    dfd.resolve(data);
                })
                .error(function (data, status, header, config) {
                    dfd.reject(status);
                });

            return dfd.promise;
        };

        function getAppointmentNote(appointmentId, contactId, groupId, userId) {
            var dfd = $q.defer();

            $http.get(settings.webApiBaseUrl + CONFIG.controllerAction + 'getAppointmentNote',
                {
                    data: offlineData.getOfflineSettings(CONFIG.offlineApptNoteUrl + (appointmentId || 0).toString(), 'AppointmentID',
                      { childKey: 'AppointmentNoteID' }),
                    params: { appointmentID: appointmentId, contactID: contactId, groupID: groupId, userID: userId }
                })
            .success(function (data, status, header, config) {
                dfd.resolve(data);
            })
            .error(function (data, status, header, config) {
                dfd.reject(status);
            });

            return dfd.promise;
        };

        function updateAppointmentNoShow(appointmentResource) {
            var dfd = $q.defer();
            var data = $.extend(true, {}, appointmentResource, offlineData.getOfflineSettings(CONFIG.offlineResourceUrl + (appointmentResource.AppointmentID || 0).toString() + '/' + (appointmentResource.AppointmentResourceID || 0).toString(),
                'AppointmentResourceID', { parentKey: 'AppointmentID', referenceKeys: ["AppointmentID"] }));
            $http.post(settings.webApiBaseUrl + CONFIG.controllerAction + 'addAppointmentResource', data)
            .success(function (data, status, header, config) {
                dfd.resolve(data);
            })
            .error(function (data, status, header, config) {
                dfd.reject(status);
            });

            return dfd.promise;
        };

        function getAppointmentStatusDetail(appointmentID, appointmentResourceID) {
            var dfd = $q.defer();

            $http.get(settings.webApiBaseUrl + CONFIG.controllerAction + 'getAppointmentStatusDetail',
                {
                    data: offlineData.getOfflineSettings(CONFIG.offlineApptStatusUrl + (appointmentID || 0).toString() + '/' + (appointmentResourceID || 0).toString(), 'AppointmentResourceID',
                      { parentKey: 'AppointmentID' }),
                    params: { appointmentResourceID: appointmentResourceID }
                })
            .success(function (data, status, header, config) {
                dfd.resolve(data);
            })
            .error(function (data, status, header, config) {
                dfd.reject(status);
            });

            return dfd.promise;
        };

        function addAppointmentStatusDetail(appointmentStatus) {
            var dfd = $q.defer();
            if (!('AppointmentStatusDetailID' in appointmentStatus))
                appointmentStatus.AppointmentStatusDetailID = 0;
            var data = $.extend(true, {}, appointmentStatus, offlineData.getOfflineSettings(CONFIG.offlineApptStatusUrl + (appointmentStatus.AppointmentID || 0).toString() + '/' + (appointmentStatus.AppointmentResourceID || 0).toString(), 'AppointmentResourceID', {
                parentKey: 'AppointmentID', referenceKeys: ["AppointmentID"]
            }));

            $http.post(settings.webApiBaseUrl + CONFIG.controllerAction + 'addAppointmentStatusDetail', data)
            .success(function (data, status, header, config) {
                dfd.resolve(data);
            })
            .error(function (data, status, header, config) {
                dfd.reject(status);
            });

            return dfd.promise;
        };

        function updateAppointmentStatusDetail(appointmentStatus) {
            var dfd = $q.defer();
            var data = $.extend(true, {}, appointmentStatus, offlineData.getOfflineSettings(CONFIG.offlineApptStatusUrl + (appointmentStatus.AppointmentID || 0).toString() + '/' + (appointmentStatus.AppointmentResourceID || 0).toString(), 'AppointmentResourceID', {
                parentKey: 'AppointmentID', referenceKeys: ["AppointmentID"]
            }));

            $http.post(settings.webApiBaseUrl + CONFIG.controllerAction + 'updateAppointmentStatusDetail', data)
            .success(function (data, status, header, config) {
                dfd.resolve(data);
            })
            .error(function (data, status, header, config) {
                dfd.reject(status);
            });

            return dfd.promise;
        };

      

        function deleteAppointmentResource(appointmentResourceId) {
            var dfd = $q.defer();
            $http.delete(settings.webApiBaseUrl + CONFIG.controllerAction + 'deleteAppointmentResource',
                {
                    data: offlineData.getOfflineSettings(CONFIG.offlineApptResourceUrl + (appointmentResourceId || 0).toString(), 'AppointmentResourceID',
                      {
                          referenceKeys: ["AppointmentResourceID"]
                      }), params: { id: appointmentResourceId, modifiedOn: moment.utc().format() }
                })
                .success(function (data, status, header, config) {
                    dfd.resolve(data);
                })
                .error(function (data, status, header, config) {
                    dfd.reject(status);
                });

            return dfd.promise;
        };

        function cancelAppointment(appointment) {

            var dfd = $q.defer();
            var data = $.extend(true, {}, appointment, offlineData.getOfflineSettings(CONFIG.offlineApiUrl + (appointment.ContactID || 0).toString() + '/' + (appointment.AppointmentID || 0).toString(), 'AppointmentID', {
                parentKey: 'ContactID', editState: editStateFunc.toString()
            }));
            $http.put(settings.webApiBaseUrl + CONFIG.controllerAction + 'cancelAppointment', data)
            .success(function (data, status, header, config) {
                dfd.resolve(data);
            })
            .error(function (data, status, header, config) {
                dfd.reject(status);
            });

            return dfd.promise;
        };

        return {
            getResourceAppointmentsByWeek: getResourceAppointmentsByWeek,
            getAppointmentsByDate: getAppointmentsByDate,
            getContactsByAppointment: getContactsByAppointment,
            getAppointments: getAppointments,
            getAppointment: getAppointment,
            getAppointmentByResource: getAppointmentByResource,
            getAppointmentResource: getAppointmentResource,
            getAppointmentResourceByContact: getAppointmentResourceByContact,
            getAppointmentLength: getAppointmentLength,
            getAppointmentType: getAppointmentType,
            addAppointment: addAppointment,
            addAppointmentContact: addAppointmentContact,
            addAppointmentResource: addAppointmentResource,
            updateAppointment: updateAppointment,
            updateAppointmentContact: updateAppointmentContact,
            updateAppointmentResource: updateAppointmentResource,
            deleteAppointment: deleteAppointment,
            updateAppointmentNoShow: updateAppointmentNoShow,
            getAppointmentNote: getAppointmentNote,
            deleteAppointmentNote: deleteAppointmentNote,
            updateAppointmentNote: updateAppointmentNote,
            addAppointmentNote: addAppointmentNote,
            updateAppointmentStatusDetail: updateAppointmentStatusDetail,
            addAppointmentStatusDetail: addAppointmentStatusDetail,
            getAppointmentStatusDetail: getAppointmentStatusDetail,
            deleteAppointmentResource: deleteAppointmentResource,
            cancelAppointment: cancelAppointment,
            getBlockedTimeAppointments: getBlockedTimeAppointments,
            deleteAppointmentsByRecurrence: deleteAppointmentsByRecurrence
        };
    }]);
