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
                $scope.collapsedWriteReview = false;
                $scope.rating = 0;
                $scope.reviewData = {
                    Heading: "",
                    Text: "",
                    UserName: "",
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

                $scope.toggleReview = function() {
                    $scope.collapsedWriteReview = !$scope.collapsedWriteReview;
                }
                $scope.submitReview = function (form) {
                    $scope.reviewData.Rating = $scope.rating;
                    console.log($scope.collapsedWriteReview);
                    reviewsService.postReview($scope.reviewData, $scope.contentlanguage).then(function (data) {
                        getReviews();
                        $scope.rating = 0;
                        $scope.reviewData.UserName = "";
                        $scope.reviewData.Heading = "";
                        $scope.reviewData.Text = "";
                        form.$setPristine();
                        $scope.toggleReview();

                    });
                    
                   
                };
            },
            template: '<div ng-include="reviewUrl"></div>'
        };
    }]);

