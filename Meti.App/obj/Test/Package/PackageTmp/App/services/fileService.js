app.factory('fileService',
    ['utilsService',
        'serverRouteMap',
        'httpService',
        function (utilsService, serverRouteMap, httpService) {
            var factory = {};

            factory.deleteItem = function (request) {
                return httpService.httpRequest(degenzaRouteMap.registry.deleteFile, "POST", request);
            }
            
            factory.fileModel = function (dto) {
                if (!dto) return {};

                var model = {
                    "id": dto.Id || dto.id,
                    "name": dto.Name || dto.name,
                    "fileVirtualpath": dto.FileVirtualpath || dto.fileVirtualpath,
                    "filePhysicalPath": dto.FilePhysicalPath || dto.filePhysicalPath,
                    "fileUrl": dto.FileUrl || dto.fileUrl,
                    "type": dto.Type || dto.type,
                    "size": dto.Size || dto.size,
                    "progress": 100
                };
                
                return model;
            }
            

            return factory;
        }]);