angular.module('OsDirectives').
    directive('osReviews', ['reviewsService', function (reviewsService) {
  
        return {
            restrict: 'A',
            scope: {
                contentid: "=",
                contentlanguage: "@"
            },
            link: function (scope) {
                scope.reviewUrl = "/scripts/app/reviews/templates/reviews?lang=" + scope.contentlanguage;
            },
            controller: function ($scope) {
               
                $scope.rating = 0;
                $scope.reviewData = {
                    Heading: "",
                    Text: "",
                    Rating: $scope.rating,
                    ContentId : $scope.contentid
                };


                reviewsService.getReviews($scope.contentid).then(function (data) {
                    $scope.reviewResult = data;

                });
                $scope.setRating = function (rating) {
                    if ($scope.rating == rating) {
                        $scope.rating = 0;
                    } else {
                        $scope.rating = rating;
                    }
                    
                };

                $scope.submitReview = function () {
                    $scope.reviewData.Heading = $scope.form.heading;
                    $scope.reviewData.Text = $scope.form.text;
                    $scope.reviewData.Rating = $scope.rating;
                    reviewsService.postReview($scope.reviewData).then(function (data) {
                        if ($scope.reviewResult.Reviews.length > 0) {
                            $scope.reviewResult.Reviews.splice(0, 0, data);
                        } else {
                            $scope.reviewResult.Reviews.push(data);
                        }
                    });
                };
            },
            template: '<div ng-include="reviewUrl"></div>'
        };
    }]);

