/* jshint -W099 */
/* global jQuery:false */
/* global angular:false */
(function($, productApp) {

	"use strict";

	productApp.factory("productService", ['$http', '$q', function ($http, $q) {
		return {
		    get: function (productData, language, pageNumber, pageSize) {
		        console.log(productData);
		        var deferred = $q.defer();
		        console.log(angular.toJson(productData));
		        $http.post('/' + language + '/api/Shopping/GetProducts?_=' + new Date().getTime().toString(), { productData: productData, page: pageNumber, pageSize: pageSize }).
					success(function (data, status, headers, config) {
						deferred.resolve(data);
					}).
					error(function (data, status, headers, config) {
						deferred.reject(status);
					});
				return deferred.promise;
			}
		};

	}]);

})(jQuery, window.productApp);