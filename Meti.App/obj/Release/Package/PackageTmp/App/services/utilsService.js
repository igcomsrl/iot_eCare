app.factory("utilsService",
    [function () {
        var factory = {};

        factory.createItemDtoModel = function (dto) {
            if (!dto) return {};

            var calculateId = function (dto) {
                if (dto.Id === 0 || dto.id === 0)
                    return 0;
                else
                    return dto.Id || dto.id;
            }


            return {
                "id": calculateId(dto),
                "text": dto.Text || dto.text,
                "description": dto.Description || dto.description
            }
        }

        factory.guid = function () {
            function s4() {
                return Math.floor((1 + Math.random()) * 0x10000)
                    .toString(16)
                    .substring(1);
            }
            return s4() + s4() + '-' + s4() + '-' + s4() + '-' + s4() + '-' + s4() + s4() + s4();
        }

        return factory;
    }]);