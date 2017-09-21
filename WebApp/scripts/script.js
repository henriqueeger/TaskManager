
var APP = angular.module('APP', ['ngRoute']);
var hostServico = "http://localhost:60441/";
APP.config(function($httpProvider) {
    $httpProvider.defaults.headers.common = {};
    $httpProvider.defaults.headers.post = {};
    $httpProvider.defaults.headers.put = {};
    $httpProvider.defaults.headers.patch = {};
});

APP.config(function ($routeProvider) {
	$routeProvider
		.when('/', {
			templateUrl: 'pages/home.html',
			controller: 'mainController'
		})
		.when('/tarefas', {
			templateUrl: 'pages/tarefas.html',
			controller: 'tarefaController'
		});
});


APP.controller('mainController', function ($scope, $http) {

	
	$http.get(hostServico + 'api/Tasks?_taskID=0').success(function (data) {
		$scope.tasks = data;
	});


	$scope.actionTask = function (tarefas, acao) {
		var urlPut = hostServico + 'api/Tasks';
		console.log(tarefas);
		if (acao != "Eliminar") {

			switch (acao) {
				case 'Concluida':
					tarefas.IndStatus = "C";
					break;
				case 'EmAndamento':
					tarefas.IndStatus = "E";
					break;
				default:

			}

			$http({
					url: hostServico + 'api/Tasks',
					method: "POST",
					data: $.param(tarefas),
					headers: { 'Content-Type': 'application/x-www-form-urlencoded' }
				}).then(function (response) {
					window.location.href = '/';
				}, function (response) { // optional
					// failed
					console.log('failed');
					console.log(JSON.stringify(response));
				});
		} else {
			$http.delete(urlPut + '?taskID=' + tarefas.TaskID).then(
				function (response) {
					$http.get(hostServico + 'api/Tasks?_taskID=0').success(function (data) {
						$scope.tasks = data;
					});
				},
				function (response) {

				}
			);
		}
	};
	
	$scope.salvaEdicao = function (tarefa) {
		
		$http({
                url: hostServico + 'api/Tasks',
                method: "POST",
                data: $.param(tarefa),
                headers: { 'Content-Type': 'application/x-www-form-urlencoded' }
            }).then(function (response) {
                window.location.href = '/';
            }, function (response) { // optional
                // failed
                console.log('failed');
                console.log(JSON.stringify(response));
            });
	}

	$scope.setTarefaEidtar = function (tarefa) {
		$scope.tarefaEditar = tarefa;
	}

});

APP.controller('tarefaController', function ($scope, $http) {
	$scope.adicionarTarefa = function (tarefa) {
		tarefa.IndStatus =  "N";
		
		$http({
                url: hostServico + 'api/Tasks',
                method: "POST",
                data: $.param(tarefa),
                headers: { 'Content-Type': 'application/x-www-form-urlencoded' }
            }).then(function (response) {
                $http.get(hostServico + 'api/Tasks?_taskID=0').success(function (data) {});
				window.location.href = '/';
            }, function (response) { // optional
                // failed
                console.log('failed');
                console.log(JSON.stringify(response));
            });
		
		
	};
});