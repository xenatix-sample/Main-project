angular.module('xenatixApp')
    .provider('offlineData', [
        'indexedDbProvider', function (indexedDbProvider) {
            var _httpObject;
            var _qObject;
            var _timeout;
            var _rootScope;
            var _connectionStateService;
            var _indexedDbProvider = indexedDbProvider;
            var _onReadyCallback = null;
            var _requestConfigQueue = null;
            var _offlineWarning = 'Your data request has been deferred until the connection to the server has been re-established.';
            var _hardError = 'An error occurred while attempting to defer the data request while offline!';
            var _offlineResponse =
            {
                Status: 200,
                StatusText: 'OK',
                Code: 0,
                Message: 'OFFLINE'
            };

            // SECTION: Encryption-related
            var sjclPassword = '';
            var sjclProperties = { adata: '', iter: 1000, mode: 'ccm', ts: 64, ks: 256, iv: sjcl.codec.base64.fromBits([294185508, -696763750, 1782465994, 327296730]), v: 1, cipher: 'aes', salt: sjclPassword };
            var useEncryption = true; // false only for DEV!

            function setPasswordHash(passwordHash) {
                passwordHash = passwordHash || '';
                if (sjclPassword !== passwordHash) {
                    sjclPassword = passwordHash;
                    sjclProperties.salt = sjclPassword;
                    if (_onReadyCallback !== null) {
                        _onReadyCallback();
                    }
                    _requestConfigQueue = requestConfigQueuer(_indexedDbProvider);
                }
            }

            // SECTION: State-related
            var _isOnline = true;
            var _eventName_stateChanged = "offlineData.stateChanged";
            var _eventName_syncChanged = "offlineData.syncChanged";
            var _refresh_timeout_online = 15; // 5 minutes = 300 seconds
            var _refresh_timeout_offline = 5; // 1 minute = 60 seconds
            var _refreshTimer = null;
            var _syncErrorTimer = null;
            var _pingUrl = '/xping';
            var _collectionNameIndx = 0;
            var _keyIndex = 1;
            var _objSync = { hasSyncError: false, url: '' };
            var _fakeURL = '/fakeURL';
            function notifystatechange(newState) {
                var event = new CustomEvent(_eventName_stateChanged,
                {
                    detail: { isOnline: newState },
                    bubbles: true,
                    cancelable: false
                });
                document.dispatchEvent(event);
            }

            function notifysyncchange(newSyncStatus) {
                var event = new CustomEvent(_eventName_syncChanged,
                {
                    detail: { isSynching: newSyncStatus },
                    bubbles: true,
                    cancelable: false
                });
                document.dispatchEvent(event);
            }

            function refreshConnectionState() {
                if (_refreshTimer != null)
                    _timeout.cancel(_refreshTimer);
                var prevValue = _isOnline;

                _httpObject.get(_pingUrl).then(function (response) {
                    _isOnline = _connectionStateService.isOnline();
                    resumeRefresh(prevValue);
                }, function (response) {
                    _isOnline = false;
                    resumeRefresh(prevValue);
                });
            }

            function resumeRefresh(prevValue) {
                if (prevValue !== _isOnline)
                    notifystatechange(_isOnline);
                if (_isOnline) {
                    if (!prevValue) {
                        syncNow();
                    }
                }
                _refreshTimer = _timeout(refreshConnectionState, (_isOnline ? _refresh_timeout_online : _refresh_timeout_offline) * 1000);
            }

            function isOnline() {
                return _isOnline && !_objSync.hasSyncError;
            }

            // SECTION: Offline ID generator-related

            function getNextOfflineId() {
                var recordIdUrl = '/OfflineData/NextOfflineId';
                var deferred = _qObject.defer();
                var nextId = -1;
                _indexedDbProvider.get(recordIdUrl).then(function (nextOfflineId) {
                    if (nextOfflineId === undefined) {
                        _indexedDbProvider.add({ entityUrl: recordIdUrl, nextId: nextId - 1 }).then(function () {
                            deferred.resolve(nextId);
                        });
                    } else {
                        nextId = nextOfflineId.nextId;
                        _indexedDbProvider.update({ entityUrl: recordIdUrl, nextId: nextId - 1 }).then(function () {
                            deferred.resolve(nextId);
                        });
                    }
                });
                return deferred.promise;
            }


            // SECTION: Resolve Offline IDs
            
            //Wrapper on resolveOfflineId to handle multiple id's passed in an array. 
            function resolveOfflineIds(offlineId, onlineId, key) {
                var deferred = _qObject.defer();
                if (typeof (offlineId) === 'object') {
                    var promiseArray = [];
                    angular.forEach(offlineId, function (item) {
                        promiseArray.push([resolveOfflineId, [item[key], onlineId]]);
                    });
                    _qObject.serial(promiseArray).then(function (data) {
                        deferred.resolve(data);
                    });
                }
                else {
                    resolveOfflineId(offlineId, onlineId).then(function (retvalue) {
                        deferred.resolve(retvalue);
                    });
                }
                return deferred.promise;
            }

            function resolveOfflineId(offlineId, onlineId) {
                var resolvedIdsUrl = '/OfflineData/ResolvedIds';
                var deferred = _qObject.defer();
                var returnValue = -1;
                _indexedDbProvider.get(resolvedIdsUrl).then(function (resolvedIdsData) {
                    if (resolvedIdsData === undefined) {
                        resolvedIdsData = {};
                        if (offlineId !== undefined) {
                            offlineId = '' + offlineId;
                            if (onlineId !== undefined) {
                                resolvedIdsData[offlineId] = onlineId;
                                returnValue = onlineId;
                            }
                        }
                        _indexedDbProvider.add({ entityUrl: resolvedIdsUrl, resolvedIds: resolvedIdsData }).then(function () {
                            deferred.resolve(returnValue);
                        });
                    } else {
                        if (offlineId !== undefined) {
                            offlineId = '' + offlineId;
                            if (onlineId !== undefined) {
                                resolvedIdsData.resolvedIds[offlineId] = onlineId;
                                returnValue = onlineId;
                            } else if (offlineId in resolvedIdsData.resolvedIds) {
                                returnValue = resolvedIdsData.resolvedIds[offlineId];
                            }
                        }
                        _indexedDbProvider.update(resolvedIdsData).then(function () {
                            deferred.resolve(returnValue);
                        });
                    }
                });
                return deferred.promise;
            }



            // SECTION: Request-related

            var requestConfigQueuer = function (idbProvider) {
                var requestConfigArray = []; // an array of stateful offline data requests, { DataUrl: '/Reg/Demo/-1', Status: 'Pending', LastSync: '01/01/2016 09:30 AM' }
                var requestConfigOfflineUrl = '/Configs';

                var save = function (isNew) {
                    try {
                    var saveObject = { entityUrl: requestConfigOfflineUrl, data: requestConfigArray };
                    if (isNew)
                        return idbProvider.add(saveObject);
                    else
                        return idbProvider.update(saveObject);
                    }
                    catch (err) {
                        console.log(err);
                    }
                };

                var add = function (requestConfigUrl) {
                    requestConfigArray.push({ DataUrl: requestConfigUrl });
                    return save();
                };

                var contains = function (requestConfigUrl) {
                    return requestConfigArray.filter(function (item) {
                        if (item.DataUrl === requestConfigUrl) return item;
                        else return undefined;
                    }).length > 0;
                };

                var remove = function (requestConfigUrl) {
                    requestConfigArray = requestConfigArray.filter(function (item) {
                        if (item.DataUrl !== requestConfigUrl) return item;
                        else return undefined;
                    });
                    return save();
                };

                var errorCurrent = function (errorMessage) {
                    if (requestConfigArray && (requestConfigArray.length > 0)) {
                        requestConfigArray[0].Status = 'Errored';
                        requestConfigArray[0].ErrorMessage = errorMessage;
                        requestConfigArray[0].LastSync = new Date();
                        requestConfigArray[0].AttemptCount = (requestConfigArray[0].AttemptCount || 0) + 1;
                        return save();
                    }
                };

                var peek = function () {
                    return (requestConfigArray.length > 0) ? requestConfigArray[0] : undefined;
                };

                if (requestConfigArray.length === 0) {
                    idbProvider.get(requestConfigOfflineUrl).then(function (data) {
                        if (data && data.data && data.data.length) {
                            requestConfigArray = data.data;
                        } else {
                            requestConfigArray = [];
                            save(true);
                        }
                    });
                }

            return {
                add: add,
                remove: remove,
                peek: peek,
                contains: contains,
                errorCurrent: errorCurrent
                };
            };

            function hasOfflineCapability(config) {
                return (config && config.data && config.data._offlineSettings && config.data._offlineSettings.enabled) || false;
            }

            function isGetRequest(config) {
                return (config && config.method && config.method === 'GET');
            }

            function getOfflineEntityUrl(data) {
                return (data && data._offlineSettings && data._offlineSettings.entityUrl) || '';
            }

            function hasPrimaryKey(config) {
                return getPrimaryKey(config) !== '';
            }

            function hasChildRecords(config) {
                return getChildKey(config) !== '';
            }

            function isChildRecord(config) {
                return getParentKey(config) !== '';
            }

            function getPrimaryKey(config) {
                return (config && config.data && config.data._offlineSettings && config.data._offlineSettings.primaryKey) || '';
            }

            function getParentKey(config) {
                return (config && config.data && config.data._offlineSettings && config.data._offlineSettings.parentKey) || '';
            }

            function getChildKey(config) {
                return (config && config.data && config.data._offlineSettings && config.data._offlineSettings.childKey) || '';
            }

            function getParentCollection(config) {
                return (config && config.data && config.data._offlineSettings && config.data._offlineSettings.parentCollection) || 'DataItems';
            }

            function getReferenceKeys(config) {
                return (config && config.data && config.data._offlineSettings && config.data._offlineSettings.referenceKeys) || [];
            }

            function getCollectionNameAndKey(config) {
                var nameKey = config.split('.');
                if (nameKey.length == 1) nameKey.push('');
                return nameKey;
            }

            function getEditStateSettings(entityUrl) {
                var deferred = _qObject.defer();
                getRequestConfig(entityUrl).then(function (requestConfig) {
                    deferred.resolve(requestConfig && requestConfig.data && requestConfig.data._offlineSettings && requestConfig.data._offlineSettings.editState && eval('(' + requestConfig.data._offlineSettings.editState + ')').bind(requestConfig.data)());
                }, function () { deferred.reject() });
                return deferred.promise;
            }

            function storeChildRecords(offlineSettings, dataItems) {
                for (var i = 0; i < dataItems.length; i++) {
                    saveRequestConfig({ url: '', method: 'GET', data: $.extend(true, {}, dataItems[i], getOfflineSettings(offlineSettings.entityUrl + '/' + dataItems[i][offlineSettings.childKey], offlineSettings.childKey, { parentKey: offlineSettings.primaryKey })) });
                }
            }

            function updateParentRecordCommon(deferred, parentCollection, childKey, data, isDelete, childData) {
                var found = false;
                var i = 0;
                if (!(parentCollection in data))
                    data[parentCollection] = [];
                for (i = 0; !found && (i < data[parentCollection].length) ; i++) {
                    if (data[parentCollection][i][childKey] == childData[childKey])
                        found = true;
                }
                if (found) {
                    if (isDelete)
                        data[parentCollection].splice(i - 1, 1);
                    else
                        data[parentCollection][i - 1] = childData;
                } else if (!isDelete)
                    data[parentCollection].push(childData);
            }

            function updateParentRecord(config, isDelete) {
                var deferred = _qObject.defer();

                isDelete = isDelete || false;
                var entityUrl = getOfflineEntityUrl(config.data);
                var entityUrlParts = entityUrl.split('/');
                entityUrlParts.pop();
                var parentUrl = entityUrlParts.join('/');
                var parentCollection = getParentCollection(config);
                getRequestConfig(parentUrl).then(function (existingConfig) {
                    if (existingConfig === undefined)
                        existingConfig = { url: parentUrl, method: 'GET', data: $.extend(true, {}, getOfflineSettings(parentUrl, getParentKey(config), { childKey: getPrimaryKey(config) })) };
                    var childKey = getChildKey(existingConfig);
                    updateParentRecordCommon(deferred, parentCollection, childKey, existingConfig.data, isDelete, config.data);
                    saveRequestConfig(existingConfig).then(function () {
                        if (isChildRecord(existingConfig))
                            updateParentRecord(existingConfig).then(function () {
                                deferred.resolve(true);
                            });
                        else
                            deferred.resolve(true);
                    });
                });

                return deferred.promise;
            }

            function getRequestConfig(url) {
                var deferred = _qObject.defer();

                _indexedDbProvider.get(url).then(
                    function (existingItem) {
                        var existingConfig = existingItem;
                        if (existingItem && existingItem.data) {
                            if (useEncryption && (sjclPassword.length > 0)) {
                                var tempSjclProps = sjclProperties;
                                tempSjclProps.ct = existingItem.data;
                                try {
                                    existingConfig = JSON.parse(sjcl.decrypt(sjclPassword, JSON.stringify(tempSjclProps), {}, {}));
                                } catch (err) {
                                    existingConfig = undefined;
                                }
                            }
                            else if (useEncryption && (sjclPassword.length === 0)) {
                                existingConfig = undefined;
                            } else
                                existingConfig = JSON.parse(existingItem.data);
                        }
                        deferred.resolve(existingConfig);
                    }
                );

                return deferred.promise;
            }

            function saveRequestConfigSuccess(requestConfig, deferred, entityUrl) {
                // If this is a non-GET request we have never seen before ...
                if (!isGetRequest(requestConfig) && !_requestConfigQueue.contains(entityUrl)) {
                    _requestConfigQueue.add(entityUrl).then(function () {
                        deferred.resolve(_offlineWarning);
                    });
                }
                else
                    deferred.resolve(_offlineWarning);
            }

            function saveRequestConfigError(deferred) {
                deferred.reject(_hardError);
            }

            function saveRequestConfigCommon(requestConfig, entityUrl, existingConfig) {
                var deferred = _qObject.defer();
                //In offline mode, add current date if new record is added.
                if (requestConfig.data != undefined) {
                    requestConfig.data.ModifiedOn = moment.utc();
                }
                var dataItem = { url: requestConfig.url, data: requestConfig.data, method: requestConfig.method, params: requestConfig.params };
                if ((existingConfig !== undefined) && (existingConfig.method === 'POST')) // retain the POST (new record)
                {
                    dataItem.method = 'POST';
                    dataItem.url = existingConfig.url;
                }
                if (useEncryption) {
                    var encryptedData = JSON.parse(sjcl.encrypt(sjclPassword, JSON.stringify(dataItem), sjclProperties, {}));
                    dataItem = { entityUrl: entityUrl, data: encryptedData.ct };
                } else {
                    dataItem = { entityUrl: entityUrl, data: JSON.stringify(dataItem) };
                }

                if (existingConfig === undefined) {
                    _indexedDbProvider.add(dataItem).then(
                        function () {
                            saveRequestConfigSuccess(requestConfig, deferred, entityUrl);
                        },
                        function () {
                            saveRequestConfigError(deferred);
                        });
                } else {
                    _indexedDbProvider.update(dataItem).then(
                        function () {
                            saveRequestConfigSuccess(requestConfig, deferred, entityUrl);
                        },
                        function () {
                            saveRequestConfigError(deferred);
                        });
                }
                return deferred.promise;
            }

            function saveRequestConfig(requestConfig) {
                var deferred = _qObject.defer();

                var entityUrl = getOfflineEntityUrl(requestConfig.data);
                var primaryKey = getPrimaryKey(requestConfig);
                if ((requestConfig.method === 'POST') && ((primaryKey !== '') && (requestConfig.data[primaryKey] === 0))) // New record and primary key is 0
                {
                    getNextOfflineId().then(function (newId) {
                        requestConfig.data[primaryKey] = newId;
                        entityUrl = entityUrl.replace('/0', '/' + newId);
                        requestConfig.data._offlineSettings.entityUrl = entityUrl;
                        requestConfig.url = requestConfig.url.replace(_fakeURL, '')
                        saveRequestConfigCommon(requestConfig, entityUrl, undefined).then(function (message) {
                            deferred.resolve(message);
                        },
                            function (message) {
                                deferred.reject(message);
                            });
                    });
                } else {
                    getRequestConfig(entityUrl).then(
                        function (existingConfig) {
                            if ((requestConfig.method === 'DELETE') && (entityUrl.split('/').pop() < 0)) {
                                _indexedDbProvider.remove(entityUrl).then(function (deleted) {
                                    if (deleted) {
                                        if (isChildRecord(existingConfig)) {
                                            updateParentRecord(existingConfig, true).then(function () {
                                                deferred.resolve('The record you previously created while offline and now removed will simply never be created.');
                                            });
                                        } else
                                            deferred.resolve('The record you previously created while offline and now removed will simply never be created.');
                                    }
                                });
                            } else {
                                saveRequestConfigCommon(requestConfig, entityUrl, existingConfig).then(function (message) {
                                    if ((requestConfig.method === 'DELETE') && isChildRecord(existingConfig)) {
                                        updateParentRecord(existingConfig, true).then(function () {
                                            deferred.resolve(message);
                                        });
                                    } else
                                        deferred.resolve(message);
                                }, function (message) {
                                    deferred.reject(message);
                                });
                            }
                        }
                    ).catch(function (error) {
                        return deferred.reject(_hardError);
                    }
                    ).finally(function () {
                    });
                }

                return deferred.promise;
            }

            function processRequestConfig(requestConfig) {
                var deferred = _qObject.defer();

                delete requestConfig.data._offlineSettings; // No need to send the offline settings along with the request
                _httpObject(requestConfig)
                    .then(function (response) {
                        deferred.resolve(response);
                    }, function (error) {
                        deferred.reject(new Error("An unexpected error has occurred while re-processing an offline data request!"));
                    });
                return deferred.promise;
            }

            function processRequestConfigCommon(requestConfig, offlineId) {
                var deferred = _qObject.defer();
                processRequestConfig(requestConfig).then(function (response) {
                        if (response && response.data && (response.data.ResultCode === 0)) { // SUCCESS
                            if ((response.data.ID > 0) && (offlineId < 0)) {
                                resolveOfflineIds(offlineId, response.data.ID).then(function (value) {
                                    deferred.resolve();
                                });
                            } else
                                deferred.resolve();
                        } else {
                            deferred.reject(response && response.data && response.data.ResultMessage);
                        }
                    },
                    function (error) {
                        deferred.reject(error);
                    });
                return deferred.promise;
            }

            function processRequestConfigs() {
                var nextOfflineEntry = _requestConfigQueue.peek();
                var offlineEntityUrl = nextOfflineEntry && nextOfflineEntry.DataUrl;
                if (_syncErrorTimer != null)
                    _timeout.cancel(_syncErrorTimer);
                if (_isOnline && offlineEntityUrl) {
                    getRequestConfig(offlineEntityUrl).then(function (requestConfig) {
                        if (requestConfig !== undefined) {
                            var offlineEntityUrlParts = offlineEntityUrl.split('/');
                            var offlineId = offlineEntityUrlParts.pop();
                            var referencePromises = [];
                            var referenceKeys = [];
                            var parentKey = getParentKey(requestConfig);
                            if ((parentKey !== '') && (requestConfig.data[parentKey] < 0)) {
                                referenceKeys.push(parentKey);
                            }
                            referenceKeys = referenceKeys.concat(getReferenceKeys(requestConfig));
                            angular.forEach(referenceKeys, function (referenceKey) {
                                var deferred = _qObject.defer();
                                var collNameAndKey = getCollectionNameAndKey(referenceKey);
                                resolveOfflineIds(requestConfig.data[collNameAndKey[_collectionNameIndx]], undefined, collNameAndKey[_keyIndex]).then(function (onlineId) {
                                    if (angular.isArray(onlineId)) {
                                        var configData = requestConfig.data[collNameAndKey[_collectionNameIndx]];
                                        for (var indx = 0; indx < configData.length; indx++) {
                                            configData[indx][collNameAndKey[_keyIndex]] = onlineId[indx];
                                        }
                                    } else if (onlineId > 0) {
                                        requestConfig.data[referenceKey] = onlineId;
                                    }
                                    deferred.resolve();
                                });
                                referencePromises.push(deferred.promise);
                            });
                            _qObject.all(referencePromises).then(function () {
                                processRequestConfigCommon(requestConfig, offlineId).then(function () {
                                    _requestConfigQueue.remove(offlineEntityUrl).then(function () {
                                        processRequestConfigs(); // Next!
                                        _objSync.hasSyncError = false;
                                        _objSync.url = '';
                                    });
                                }, function (error) {
                                    _requestConfigQueue.errorCurrent(error);
                                    requestConfig.url = requestConfig.url.replace(_fakeURL, '');
                                    _objSync.hasSyncError = true;                                    
                                    _objSync.url = requestConfig.url;
                                    _syncErrorTimer = _timeout(function () { processRequestConfigs() }, 2000);
                                });
                            });
                        } else {
                            _requestConfigQueue.remove(offlineEntityUrl).then(function () { // this is not right; the data should be there
                                processRequestConfigs(); // Next!
                            });
                        }
                    });
                }
                return null;
            }

            var interceptors = {
                request: function (config) {
                    if (config.method == 'POST' || config.method == 'PUT') {
                        _rootScope.isRunning = true;
                    }
                    config.url = config.url.replace(_fakeURL, '');
                    if (_objSync.hasSyncError && config.url != _objSync.url && hasOfflineCapability(config)) {
                       config.url = config.url +_fakeURL;
                    }

                    return config || $q.when(config);
                },

                requestError: function (rejection) {
                    if (rejection.config.method == 'POST' || rejection.config.method == 'PUT')
                        _rootScope.isRunning = false;

                    return $q.reject(rejection);
                },

                response: function (response) {
                    if (response.config.method == 'POST' || response.config.method == 'PUT')
                        _rootScope.isRunning = false;

                    if (response.status == 401)
                        toastr.error('You are unauthorized for this opertaion!');

                    // Cache all successful, online responses to offline-capable GET requests
                    if (response.config && isGetRequest(response.config) && hasOfflineCapability(response.config) && isOnline()) {
                        var entityUrl = getOfflineEntityUrl(response.config.data);
                        saveRequestConfig({
                            entityUrl: entityUrl,
                            data: $.extend(true, {}, response.data, { _offlineSettings: response.config.data._offlineSettings }),
                            url: response.config.url,
                            method: response.config.method
                        });
                        if (response.data && response.data.DataItems && (response.data.DataItems.length > 0) && hasChildRecords(response.config)) {
                            storeChildRecords(response.config.data._offlineSettings, response.data.DataItems);
                        }
                    }
                    return response;
                },
                responseError: function (rejection) {
                    if (!rejection.config) {
                        return;
                    console.log(rejection);
                    console.log('rejection');
                    }
                        
                    var isPostPut = (rejection.config.method == 'POST' || rejection.config.method == 'PUT') ? true : false;


                    var deferred = _qObject.defer();
                    // Check if the server is unreachable, a sign that we are offline
                    if (rejection.status <= 0||_objSync.hasSyncError) {
                        //Just-in-time switch to offline
                        var isOfflineCapableRequest = hasOfflineCapability(rejection.config);
                        if (isOnline() && isOfflineCapableRequest) {
                            _isOnline = false;
                            notifystatechange(false);
                            refreshConnectionState();
                        }

                        // Serve cached data to GET requests
                        if (!isOnline() && isOfflineCapableRequest) {
                            if (isGetRequest(rejection.config)) {
                                getRequestConfig(getOfflineEntityUrl(rejection.config.data)).then(function (existingConfig) {
                                    if (existingConfig) {
                                        var cachedData = { config: rejection.config, status: _offlineResponse.Status, statusText: _offlineResponse.StatusText, data: (existingConfig.data && existingConfig.data.DataItems) ? $.extend(true, {}, existingConfig.data, { ResultCode: _offlineResponse.Code, ResultMessage: _offlineResponse.Message }) : { DataItems: [existingConfig.data], ResultCode: _offlineResponse.Code, ResultMessage: _offlineResponse.Message } };
                                        deferred.resolve(cachedData);
                                        if (isPostPut)
                                            _rootScope.isRunning = false;
                                    } else {
                                        saveRequestConfig({
                                            entityUrl: getOfflineEntityUrl(rejection.config.data),
                                            data: $.extend(true, {}, { DataItems: [] }, { _offlineSettings: rejection.config.data._offlineSettings }),
                                            url: rejection.config.url,
                                            method: rejection.config.method
                                        });
                                        deferred.resolve({ config: rejection.config, status: _offlineResponse.Status, statusText: _offlineResponse.StatusText, data: { DataItems: [], ResultCode: _offlineResponse.Code, ResultMessage: _offlineResponse.Message } });
                                        if (isPostPut)
                                            _rootScope.isRunning = false;
                                    }
                                });
                            } else {
                                // Continue caching non-GET requests
                                saveRequestConfig(rejection.config).then(function (message) {
                                    deferred.notify(message);
                                    if (isPostPut)
                                        _rootScope.isRunning = false;
                                    var cachedData = { config: rejection.config, status: _offlineResponse.Status, statusText: _offlineResponse.StatusText, data: { ResultCode: _offlineResponse.Code, ResultMessage: _offlineResponse.Message } };
                                    if ((rejection.config.method !== 'DELETE') && isChildRecord(rejection.config)) {
                                        updateParentRecord(rejection.config).then(function () {
                                            if (rejection.config.method === 'POST') // NEW!
                                                cachedData.data.ID = getOfflineEntityUrl(rejection.config.data).split('/').pop();
                                            deferred.resolve(cachedData);
                                            if (isPostPut)
                                                _rootScope.isRunning = false;
                                        });
                                    } else {
                                        if (rejection.config.method === 'POST') // NEW!
                                            cachedData.data.ID = getOfflineEntityUrl(rejection.config.data).split('/').pop();
                                        deferred.resolve(cachedData);
                                        if (isPostPut)
                                            _rootScope.isRunning = false;
                                    }
                                }, function (error) {
                                    deferred.reject(error);
                                    if (isPostPut)
                                        _rootScope.isRunning = false;
                                });
                            }
                        } else {
                            deferred.reject(rejection);
                            if (isPostPut)
                                _rootScope.isRunning = false;
                        }
                    } else {
                        deferred.reject(rejection);

                        if (isPostPut)
                            _rootScope.isRunning = false;
                    }
                    return deferred.promise;
                }
            };

            function initialize($http) { // $http needs to be supplied here to avoid circular dependency (as it relates to the interceptors)
                _httpObject = $http;

                _connectionStateService.onOnline(function () {
                    refreshConnectionState();
                });

                _connectionStateService.onOffline(function () {
                    refreshConnectionState();
                });

                _refreshTimer = _timeout(refreshConnectionState, 1000);

                return _indexedDbProvider.openDatabase(_qObject, _timeout);
            }

            function syncNow() {
                notifysyncchange(true);
                processRequestConfigs();
                notifysyncchange(false);
            }

            function getOfflineSettings(entityUrl, primaryKey, configuredSettings) {
                var settings = {
                    enabled: true,
                    entityUrl: entityUrl,
                    primaryKey: primaryKey || '',
                    parentKey: '',
                    childKey: '',
                    referenceKeys: [],
                    parentCollection: 'DataItems'
                    //editState: 'function editState() { return { state: 'somestate', stateParams: { some: 'Param' } }; }' // shown here for documentation purposes only; see specific implementations
                };
                var offlineSettings = $.extend({}, settings, configuredSettings || {});
                return {
                    _offlineSettings: offlineSettings
                };
            }

            function onOfflineReady(onReadyCallback) {
                _onReadyCallback = onReadyCallback;
            }

            return {
                $get: ['$q', '$timeout', '$rootScope', 'connectionStateService', function ($q, $timeout, $rootScope, connectionStateService) {
                    _qObject = $q;
                    _timeout = $timeout;
                    _rootScope = $rootScope;
                    _connectionStateService = connectionStateService;
                    return {
                        isOnline: isOnline,
                        stateChangedEventName: _eventName_stateChanged,
                        syncChangedEventName: _eventName_syncChanged,
                        interceptors: interceptors,
                        initialize: initialize,
                        syncNow: syncNow,
                        getOfflineSettings: getOfflineSettings,
                        indexedDbProvider: _indexedDbProvider,
                        setPasswordHash: setPasswordHash,
                        onOfflineReady: onOfflineReady,
                        getEditState: getEditStateSettings
                    };
                }]
            };
        }
    ]).config([
        '$provide', '$httpProvider', function ($provide, $httpProvider) {
            $provide.factory('offlineDataInterceptors', [
                'offlineData', function (offlineData) {
                    return offlineData.interceptors;
                }
            ]);

            //$httpProvider.interceptors.push('offlineDataInterceptors');
        }
    ]);
