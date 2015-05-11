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
                getReviews();

              
                $scope.setRating = function (rating) {
                    if ($scope.rating == rating) {
                        $scope.rating = 0;
                    } else {
                        $scope.rating = rating;
                    }
                    
                };
               function getReviews(){
                   reviewsService.getReviews($scope.contentid, $scope.contentlanguage).then(function (data) {
                        $scope.reviewResult = data;

                    });
                }

                $scope.submitReview = function (formData) {
                    $scope.reviewData.Heading = formData.heading;
                    $scope.reviewData.Text = formData.text;
                    $scope.reviewData.Rating = $scope.rating;
                    reviewsService.postReview($scope.reviewData, $scope.contentlanguage).then(function (data) {
                        getReviews();
                    });
                };
            },
            template: '<div ng-include="reviewUrl"></div>'
        };
    }]);

