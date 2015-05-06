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
                        $scope.reviewResult.Reviews.splice(0, 0, data);
                    });
                };
            },
            templateUrl: "/scripts/app/reviews/templates/reviews.html"
        };
    }]);

