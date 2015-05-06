angular.module('sharedFactories').
    factory("reviewsService", [
        '$q', '$http', function($q, $http) {
        var reviewsService = {};
        var basicUrl = "/api/Review/";
        reviewsService.getReviews = function(contentid) {
            var df = $q.defer();
            var url = basicUrl + "get?id=" + contentid;
            $http.get(url).success(function (data, status, headers, config) {
                    df.resolve(data);
                })
                .error(function(data, status, headers, config) {
                    df.resolve(headers);
                });
            return df.promise;

        }

        return reviewsService;
    }
]);