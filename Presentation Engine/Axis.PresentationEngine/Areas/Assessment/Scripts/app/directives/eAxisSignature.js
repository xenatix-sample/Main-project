
//lookup-type="user|client|contact|name" 
angular.module("xenatixApp", [])
            .directive("eAxisSignature", function () {
                return {
                    template: function () {
                        return angular.element(
                            document.querySelector("#")).html();
                    },
                    scope: {
                        lookup: "&lookUpProp",
                        signature: "&signatureType",
                        dateType: "&dateType",
                        newExistingRecordCall: "&newExistingRecord"
                    },
                    link: function (scope, element, attrs, ctrl) {

                        var checkLookupData = attrs["lookUp"];
                    }
                }
            })

//controller call

//$scope.defaultAction=function(){
//    newRecord:"NewRecord",
//    oldRecordUpdate:"OldRecordUpdate"
//}

//$scope.getNewOldRecordParam=function(param){
//    return param == "NewRecord" ? $scope.defaultAction.oldRecordUpdate:"SomethingWrong"
//}

//$scope.getLookUpProp=function(){

//}

//$scope.getSignatureType=function(){

//}

//$scope.getDateType=function(){

//}


//HTML Implementation

//<div e-Axis-Signature  newExistingRecord="getNewOldRecordParam(defaultAction.newRecord)" lookUpProp=getLookUpProp()  dateType=getDateType() signatureType=getSignatureType()