angular.module('xenatixApp')
    .factory('serviceRecordingService', ["$rootScope", "_", "$http", "$q", 'settings', 'offlineData', 'roleSecurityService', 'lookupService', '$filter', 'organizationStructureService', 'helperService',
function ($rootScope, _, $http, $q, settings, offlineData, roleSecurityService, lookupService, $filter, organizationStructureService, helperService) {
		var serviceStatusList;
		var attendanceStatusList;
		var trackingFieldList;
		var recordingRecipientCodeList;
		var recordingDeliveryMethodList;
		var recordingServiceLocationList;
		var serviceStartDateTime;
		var serviceEndDateTime;

		var getServiceRecordingTime = function () {
				return {
						serviceStartDateTime: serviceStartDateTime,
						serviceEndDateTime: serviceEndDateTime
				};
		};

		var setServiceRecordingTime = function (start, end) {
				serviceStartDateTime = start;
				serviceEndDateTime = end;
		};

		var CONFIG = {
				apiControllerRoot: "/data/ServiceRecording/",
				offlineApiUrl: '/ServiceRecording/'
		};

		var editStateFunc = function editStateSettings() {
				return {
						description: 'Service Recording',
						state: 'callcenter.crisisline.services',
						stateParams: { ContactID: this.ContactID, CallCenterHeaderID: this.CallCenterHeaderID, ReadOnly: 'Edit' }
				};
		};

		function getProgramUnits(dataKey) {
				var dfd = $q.defer();
				$http.get(settings.webApiBaseUrl + CONFIG.apiControllerRoot + 'GetProgramUnits', { params: { dataKey: dataKey } })
				.success(function (data, status, header, config) {
						dfd.resolve(data);
				})
				.error(function (data, status, header, config) {
						dfd.reject(status);
				});

				return dfd.promise;
		};

		function getServiceRecordings(ContactID, startDate, endDate) {
				var dfd = $q.defer();
				var serviceRecordings = [];
				//if (roleSecurityService.hasPermission(getPermissioKey(SERVICE_RECORDING_SOURCE.CallCenter), PERMISSION.READ))
				serviceRecordings.push(getServiceList(SERVICE_RECORDING_SOURCE.CallCenter, ContactID, startDate, endDate));
				//if (roleSecurityService.hasPermission(getPermissioKey(SERVICE_RECORDING_SOURCE.BAPN), PERMISSION.READ))
				serviceRecordings.push(getServiceList(SERVICE_RECORDING_SOURCE.BAPN, ContactID, startDate, endDate));
				//if (roleSecurityService.hasPermission(getPermissioKey(SERVICE_RECORDING_SOURCE.IDDForms), PERMISSION.READ))
				serviceRecordings.push(getServiceList(SERVICE_RECORDING_SOURCE.IDDForms, ContactID, startDate, endDate));
				//if (roleSecurityService.hasPermission(getPermissioKey(SERVICE_RECORDING_SOURCE.LawLiaison), PERMISSION.READ))
				serviceRecordings.push(getServiceList(SERVICE_RECORDING_SOURCE.LawLiaison, ContactID, startDate, endDate));

				$q.all(serviceRecordings).then(function (response) {
						var responses = [];
						angular.forEach(response, function (item) {
								if (hasData(item))
										responses = _.union(responses, item.DataItems);
						});
						responses = $filter('orderBy')(responses, ['DocumentStatusID', '-ModifiedOn'], false);
						dfd.resolve(responses);
				}, function (error) {
						dfd.reject(error);
				});
				return dfd.promise;
		};

		function getServiceList(SourceHeaderID, ContactID, startDate, endDate) {

				var dfd = $q.defer();
				$http.get(settings.webApiBaseUrl + CONFIG.apiControllerRoot + 'GetServiceRecordings', { data: offlineData.getOfflineSettings(CONFIG.offlineApiUrl + (SourceHeaderID || 0).toString(), 'CallCenterHeaderID', { childKey: 'ServiceRecordingID' }), params: { ServiceRecordingSourceID: SourceHeaderID, ContactID: ContactID, startDate: startDate, endDate: endDate } })
				.success(function (data, status, header, config) {
						dfd.resolve(data);
				})
				.error(function (data, status, header, config) {
						dfd.reject(status);
				});

				return dfd.promise;
		};


		function getServiceRecording(SourceHeaderID, ServiceRecordingSourceID) {
				var dfd = $q.defer();
				$http.get(settings.webApiBaseUrl + CONFIG.apiControllerRoot + 'GetServiceRecording', { data: offlineData.getOfflineSettings(CONFIG.offlineApiUrl + (SourceHeaderID || 0).toString() + '/' + (ServiceRecordingSourceID || 0), 'CallCenterHeaderID', { childKey: 'ServiceRecordingID' }), params: { SourceHeaderID: SourceHeaderID, ServiceRecordingSourceID: ServiceRecordingSourceID } })
				.success(function (data, status, header, config) {
						dfd.resolve(data);
				})
				.error(function (data, status, header, config) {
						dfd.reject(status);
				});

				return dfd.promise;
		};


		function addServiceRecording(serviceRecording) {
				var dfd = $q.defer();
				var data = $.extend(true, {}, serviceRecording, offlineData.getOfflineSettings(CONFIG.offlineApiUrl + (serviceRecording.CallCenterHeaderID || 0).toString() + '/' + (serviceRecording.ServiceRecordingID || 0).toString(), 'ServiceRecordingID', { parentKey: 'CallCenterHeaderID', editState: editStateFunc.toString() }));
				$http.post(settings.webApiBaseUrl + CONFIG.apiControllerRoot + 'AddServiceRecording', data)
				.success(function (data, status, header, config) {
						dfd.resolve(data);
				})
				.error(function (data, status, header, config) {
						dfd.reject(status);
				});

				return dfd.promise;
		};

		function updateServiceRecording(serviceRecording) {
				var dfd = $q.defer();
				var data = $.extend(true, {}, serviceRecording, offlineData.getOfflineSettings(CONFIG.offlineApiUrl + (serviceRecording.CallCenterHeaderID || 0).toString() + '/' + (serviceRecording.ServiceRecordingID || 0).toString(), 'ServiceRecordingID', { parentKey: 'CallCenterHeaderID', editState: editStateFunc.toString() }));
				$http.post(settings.webApiBaseUrl + CONFIG.apiControllerRoot + 'UpdateServiceRecording', data)
				.success(function (data, status, header, config) {
						dfd.resolve(data);
				})
				.error(function (data, status, header, config) {
						dfd.reject(status);
				});

				return dfd.promise;
		};

		function isOtherUser(SourceHeaderID, ServiceRecordingSourceID, userID) {
				var dfd = $q.defer();
				getServiceRecording(SourceHeaderID, ServiceRecordingSourceID).then(function (data) {
						if (hasData(data)) {
								dfd.resolve(data.DataItems[0].UserID != userID);
						}
						else {
								dfd.reject(null);;
						}
				});

				return dfd.promise;
		};

		var getText = function (lookUpType, value, propName, propID) {
				return value ? lookupService.getText(lookUpType, value, propName, propID) : '';
		};

		var getOrganizationText = function (type, id, serviceRecordingSourceID) {
				if (id && serviceRecordingSourceID) {
						var permissionKey = getPermissioKey(serviceRecordingSourceID);
						var hasCompanyPermission = roleSecurityService.hasPermission(permissionKey, PERMISSION.READ, PERMISSION_LEVEL.Company);
						var organization = $filter('filter')($rootScope.getOrganizationByDataKey(type, undefined, hasCompanyPermission), { ID: id }, true);
						return organization.length > 0 && organization[0].Name ? organization[0].Name : '';
				} else {
						return '';
				}
		};
	var getPermissioKey = function (sourceId) {
				var permissionKey = '';
				switch (sourceId) {
						case SERVICE_RECORDING_SOURCE.CallCenter:
								permissionKey = CallCenterPermissionKey.CallCenter_CrisisLine;
								break;
						case SERVICE_RECORDING_SOURCE.BAPN:
								permissionKey = BenifitsPermissionKey.Benefits_BAPN_BAPN;
								break;
						case SERVICE_RECORDING_SOURCE.IDDForms:
								permissionKey = LettersPermissionKey.Intake_IDDForms_Forms;
								break;
						case SERVICE_RECORDING_SOURCE.LawLiaison:
								permissionKey = CallCenterPermissionKey.CallCenter_LawLiaison;
								break;
				}
				return permissionKey;
		};
	var calcDuration = function (callStartDate, callEndDate, isScreen) {
				//Do all common calculations and logics here
				var duration = '';
				var adjustedTime = "00:00:01";
				var isStartDate = searchString();
		try {
						if (callStartDate == 'Invalid Date' || (!isScreen && searchString(callStartDate, adjustedTime)))
								return duration;
						if (callEndDate == 'Invalid Date' || (!isScreen && searchString(callStartDate, adjustedTime)))
								return duration;
						var elapsed = ((new Date(callEndDate)).setSeconds(0, 0)) - ((new Date(callStartDate)).setSeconds(0, 0)); // time in milliseconds
						if (elapsed >= 0) {
								var data = new DateMeasure(elapsed);
								duration = (data.days > 0) ? data.days + 'day ' : '';
								duration += data.hours + 'hr' + ' ' + data.minutes + 'mins';
						}
						return duration;
				}
				catch (err) {
						return duration;
				};
		};

		var getStatus = function (row) {
				return row["CallStatusID"] ? getText('CallStatus', row["CallStatusID"]) : getDocumentStatus(row["DocumentStatusID"]);
		};

		var getModuleActiveLookups = function (lookupList, moduleComponentID) {
				return $filter('filter')(lookupList, { ModuleComponentID: moduleComponentID, IsActive: true }, true);
		};

		function getAllServiceLookups(moduleComponentID) {
				var resultList = [];
				serviceStatusList = lookupService.getLookupsByTypeAll('ServiceStatusConfigured');
				attendanceStatusList = lookupService.getLookupsByTypeAll('AttendanceStatusConfigured');
				trackingFieldList = lookupService.getLookupsByTypeAll('TrackingFieldConfigured');
				recordingRecipientCodeList = lookupService.getLookupsByTypeAll('RecordingRecipientCode');
				recordingDeliveryMethodList = lookupService.getLookupsByTypeAll('RecordingDeliveryMethod');
				recordingServiceLocationList = lookupService.getLookupsByTypeAll('RecordingServiceLocation');

				resultList.activeServiceStatusList = getModuleActiveLookups(serviceStatusList, moduleComponentID);
				resultList.activeAttendanceStatusList = getModuleActiveLookups(attendanceStatusList, moduleComponentID);
				resultList.activeTrackingFieldList = getModuleActiveLookups(trackingFieldList, moduleComponentID);
				resultList.activeRecordingRecipientCodeList = getModuleActiveLookups(recordingRecipientCodeList, moduleComponentID);
				resultList.activeRecordingDeliveryMethodList = getModuleActiveLookups(recordingDeliveryMethodList, moduleComponentID);
				resultList.activeRecordingServiceLocationList = getModuleActiveLookups(recordingServiceLocationList, moduleComponentID);

				return resultList;
		};

		function getAllServiceLookupsOnGet(serviceData, moduleComponentID) {
				var serviceLookups = getAllServiceLookups(moduleComponentID);
				var commonParams = { ModuleComponentID: moduleComponentID, ServiceID: serviceData.ServiceItemID };
				if (serviceData.ServiceStatusID)
						helperService.updateLookupList(serviceLookups.activeServiceStatusList, serviceStatusList, $.extend(commonParams, { "ID": serviceData.ServiceStatusID }));
				if (serviceData.AttendanceStatusID)
						helperService.updateLookupList(serviceLookups.activeAttendanceStatusList, attendanceStatusList, $.extend(commonParams, { "ID": serviceData.AttendanceStatusID }));
				if (serviceData.TrackingFieldID)
						helperService.updateLookupList(serviceLookups.activeTrackingFieldList, trackingFieldList, $.extend(commonParams, { "ID": serviceData.TrackingFieldID }));
				if (serviceData.RecipientCodeID)
						helperService.updateLookupList(serviceLookups.activeRecordingRecipientCodeList, recordingRecipientCodeList, $.extend(commonParams, { "ID": serviceData.RecipientCodeID }));
				if (serviceData.DeliveryMethodID)
						helperService.updateLookupList(serviceLookups.activeRecordingDeliveryMethodList, recordingDeliveryMethodList, $.extend(commonParams, { "ID": serviceData.DeliveryMethodID }));
				if (serviceData.ServiceLocationID)
						helperService.updateLookupList(serviceLookups.activeRecordingServiceLocationList, recordingServiceLocationList, $.extend(commonParams, { "ID": serviceData.ServiceLocationID }));
				return serviceLookups;
		}

		function getActiveServices(detailID, organizationID, datakey) {
				return organizationStructureService.getServiceOrganizationDetailsByID(detailID).then(function (data) {
						if (hasData(data)) {
								return ($filter('filter')(data.DataItems, { DataKey: datakey, OrganizationID: organizationID }, true));
						}
						else {
								return {};
						}
				});
		};

		function getActiveServicesOnGet(detailID, organizationID, datakey, serviceID) {
				return getActiveServices(detailID, organizationID, datakey).then(function (activeServices) {
						if (serviceID) {
								var recordingServices = lookupService.getLookupsByTypeAll("RecordingServices");
								var filterParams = { ServiceID: serviceID };
							helperService.updateLookupList(activeServices, recordingServices, filterParams);
						}
						return activeServices;
				});
		};

		return {
				setServiceRecordingTime: setServiceRecordingTime,
				getServiceRecordingTime: getServiceRecordingTime,
				getServiceRecordings: getServiceRecordings,
				getServiceRecording: getServiceRecording,
				addServiceRecording: addServiceRecording,
				updateServiceRecording: updateServiceRecording,
				isOtherUser: isOtherUser,
				getOrganizationText: getOrganizationText,
				getText: getText,
				calcDuration: calcDuration,
				getStatus: getStatus,
				getAllServiceLookups: getAllServiceLookups,
				getProgramUnits: getProgramUnits,
				getActiveServices: getActiveServices,
				getActiveServicesOnGet: getActiveServicesOnGet,
				getAllServiceLookupsOnGet: getAllServiceLookupsOnGet
		};


}]);
