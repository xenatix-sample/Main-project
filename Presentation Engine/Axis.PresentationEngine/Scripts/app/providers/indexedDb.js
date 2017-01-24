angular.module('xenatixApp')
    .provider('indexedDb', [function () {
        var _default_version = 5;
        var _default_database_name = "AxisDatabase";
        var _default_store_name = "AxisStore";
        var _default_key_path = "entityUrl";

        var setUp = false;
        var db;
        var _qObject;

        function openDatabase(qObject, timeout) {
            _qObject = qObject;
            var deferred = _qObject.defer();

            if (setUp) {
                timeout(function () {
                    deferred.resolve(db);
                });
                return deferred.promise;
            }

            var openRequest = window.indexedDB.open(_default_database_name, _default_version);

            openRequest.onerror = function (e) {
                console.log("Error opening db");
                console.dir(e);
                deferred.reject("An error occurred while opening the offline database!");
            };

            openRequest.onupgradeneeded = function (e) {
                //TODO: Plan database upgrade or refresh, whichever is appropriate
                db = e.target.result;
                createObjectStore(_default_store_name, _default_key_path);
                deferred.resolve(db);
            };

            openRequest.onsuccess = function (e) {
                db = e.target.result;

                db.onerror = function (event) {
                    //errorCode Property doesn't exist -> Changing to error to help log the error in readable manner
                    console.log("Database error: " + event.target.error);
                    deferred.reject("Database error: " + event.target.error);
                };

                setUp = true;
                deferred.resolve(db);
            };

            return deferred.promise;
        }

        function createObjectStore(objectStoreName, keyPath) {
            if (objectStoreName === undefined)
                objectStoreName = _default_store_name;

            if (keyPath === undefined)
                keyPath = _default_key_path;

            if (!db.objectStoreNames.contains(objectStoreName)) {
                db.createObjectStore(objectStoreName, { keyPath: keyPath, autoIncrement: false });
            }
        }

        function get(key, objectStoreName) {
            var deferred = _qObject.defer();

            if (objectStoreName === undefined)
                objectStoreName = _default_store_name;

            try {
                var transaction = db.transaction(objectStoreName);
                var objectStore = transaction.objectStore(objectStoreName);
                var request = objectStore.get(key);

                request.onsuccess = function (event) {
                    var result = request.result;
                    deferred.resolve(result);
                };

                request.onerror = function (event) {
                    deferred.resolve(null);
                };
            } catch (e) {
                console.log('index DB - get :: ' + e);
                deferred.resolve(undefined);
            }

            return deferred.promise;
        }

        function add(objectData, objectStoreName) {
            var deferred = _qObject.defer();

            if (objectStoreName === undefined)
                objectStoreName = _default_store_name;

            var transaction = db.transaction(objectStoreName, "readwrite");
            var objectStore = transaction.objectStore(objectStoreName);
            
            // put() - It will update an object if the key already exists, and it will add a new object otherwise
            // Resolves - Database error: Constraint Error: Key already exists in the object store.
            var request = objectStore.put(objectData);

            transaction.oncomplete = function (event) {
                deferred.resolve();
            };

            transaction.onerror = function (event) {
                deferred.reject(new Error('An error occurred while queueing an offline request!'));
            };

            return deferred.promise;
        }

        function update(objectData, objectStoreName) {
            var transactionSucess = false;
            var deferred = _qObject.defer();

            if (objectStoreName === undefined)
                objectStoreName = _default_store_name;

            var transaction = db.transaction(objectStoreName, "readwrite");
            var objectStore = transaction.objectStore(objectStoreName);
            objectStore.put(objectData);

            transaction.oncomplete = function (event) {
                deferred.resolve();
            };

            transaction.onerror = function (event) {
                deferred.reject(new Error('An error occurred while updating a previously queued offline request!'));
            };
            return deferred.promise;
        };

        function remove(key, objectStoreName) {
            var deferred = _qObject.defer();

            if (objectStoreName === undefined)
                objectStoreName = _default_store_name;

            var transaction = db.transaction(objectStoreName, "readwrite");
            var request = transaction.objectStore(objectStoreName).delete(key);

            transaction.oncomplete = function (event) {
                deferred.resolve(true);
            };

            transaction.onerror = function (event) {
                deferred.resolve(false);
            };

            return deferred.promise;
        }

        function ready() {
            return setUp;
        }

        function isSupported() {
            return ("indexedDB" in window);
        }

        return {
            openDatabase: openDatabase,
            ready: ready,
            get: get,
            add: add,
            update: update,
            remove: remove,
            isSupported: isSupported,
            $get: function () {
                return {
                    ready: ready,
                    get: get,
                    add: add,
                    update: update,
                    remove: remove,
                    isSupported: isSupported
                };
            }
        };
    }]);