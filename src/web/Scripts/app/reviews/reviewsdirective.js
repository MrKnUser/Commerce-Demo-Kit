angular.module('OsDirectives').
    directive('osReviews', ['reviewsService', function (reviewsService) {
        return {
            restrict: 'A',
            scope: {
                contentid: "="
            },
            link: function (scope) {
              
               
            },
            controller: function ($scope) {
                console.log($scope.contentid);
                reviewsService.getReviews($scope.contentid).then(function (data) {
                    $scope.reviewData = data;

                });
            },
            templateUrl: "/scripts/app/reviews/templates/reviews.html"
        };
    }]);

