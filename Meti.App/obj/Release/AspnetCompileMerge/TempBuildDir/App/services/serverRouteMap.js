app.service("serverRouteMap",
    ["apiEndPoint", "zeusEndpoint",
        function (apiEndPoint, zeusEndpoint) {
            var factory = {};

            factory.routeMap =
            {
                process: {
                    get: apiEndPoint + "api/process/get",
                    create: apiEndPoint + "api/process/create",
                    update: apiEndPoint + "api/process/update",
                    fetch: apiEndPoint + "api/process/fetch",
                    deleteItem: apiEndPoint + "api/process/delete",
                    fetchEssentialData: apiEndPoint + "api/process/fetchEssentialData"
                },
                processInstance: {
                    getByRegistry: apiEndPoint + "api/processInstance/getByRegistry",
                    getByRegistryEmail: apiEndPoint + "api/processInstance/getByRegistryEmail",
                    get: apiEndPoint + "api/processInstance/get",
                    create: apiEndPoint + "api/processInstance/create",
                    update: apiEndPoint + "api/processInstance/update",
                    fetch: apiEndPoint + "api/processInstance/fetch",
                    deleteItem: apiEndPoint + "api/processInstance/delete",
                    fetchEssentialData: apiEndPoint + "api/processInstance/fetchEssentialData",
                    fetchProcessInstanceGeo: apiEndPoint + "api/processInstance/fetchProcessInstanceGeo",
                    getProcessInstanceGeo: apiEndPoint + "api/processInstance/getProcessInstanceGeo",
                },
                registry: {
                    get: apiEndPoint + "api/registry/get",
                    create: apiEndPoint + "api/registry/create",
                    update: apiEndPoint + "api/registry/update",
                    fetch: apiEndPoint + "api/registry/fetch",
                    deleteItem: apiEndPoint + "api/registry/delete",
                    fetchEssentialData: apiEndPoint + "api/registry/fetchEssentialData",
                    getGeocoding: "https://nominatim.openstreetmap.org/search"
                },
                parameter: {
                    get: apiEndPoint + "api/parameter/get",
                    create: apiEndPoint + "api/parameter/create",
                    update: apiEndPoint + "api/parameter/update",
                    fetch: apiEndPoint + "api/parameter/fetch",
                    deleteItem: apiEndPoint + "api/parameter/delete",
                    fetchEssentialData: apiEndPoint + "api/parameter/fetchEssentialData",
                    getByDevice: apiEndPoint + "api/parameter/getByDevice"
                },
                device: {
                    get: apiEndPoint + "api/device/get",
                    create: apiEndPoint + "api/device/create",
                    update: apiEndPoint + "api/device/update",
                    fetch: apiEndPoint + "api/device/fetch",
                    deleteItem: apiEndPoint + "api/device/delete",
                    fetchEssentialData: apiEndPoint + "api/device/fetchEssentialData"
                },
                alarm: {
                    fetchEssentialData: apiEndPoint + "api/alarm/fetchEssentialData"
                },
                alarmMetric: {
                    fetchEssentialData: apiEndPoint + "api/alarmMetric/fetchEssentialData"
                },
                alarmFired: {
                    fetch: apiEndPoint + "api/alarmFired/fetch",
                    turnOff: apiEndPoint + "api/alarmFired/turnOff",
                    create: apiEndPoint + "api/alarmFired/create",
                    get: apiEndPoint + "api/alarmFired/get",
                },
                deviceErrorLog: {
                    log: apiEndPoint + "api/deviceErrorLog/log",
                    fetch: apiEndPoint + "api/deviceErrorLog/fetch"
                },
                account: {
                    getProfile: apiEndPoint + "api/account/getProfile"
                },
                zeus: {
                    login: zeusEndpoint + "token",
                    resetPassword: zeusEndpoint + "api/User/ResetPassword",
                    changePassword: zeusEndpoint + "api/User/ChangePassword"
                },
                configuration: {
                    get: apiEndPoint + "api/Configuration/Get"
                },
                user: {
                    get: apiEndPoint + "api/user/get",
                    create: apiEndPoint + "api/user/create",
                    update: apiEndPoint + "api/user/update",
                    fetch: apiEndPoint + "api/user/fetch",
                    deleteItem: apiEndPoint + "api/user/delete",
                    fetchEssentialData: apiEndPoint + "api/user/fetchEssentialData",
                    resetPassword: apiEndPoint + "api/user/resetPassword"
                },
                role: {
                    get: apiEndPoint + "api/role/get",
                    create: apiEndPoint + "api/role/create",
                    update: apiEndPoint + "api/role/update",
                    fetch: apiEndPoint + "api/role/fetch",
                    deleteItem: apiEndPoint + "api/role/delete",
                    fetchEssentialData: apiEndPoint + "api/role/fetchEssentialData"
                }
            };

            return factory;
        }]);