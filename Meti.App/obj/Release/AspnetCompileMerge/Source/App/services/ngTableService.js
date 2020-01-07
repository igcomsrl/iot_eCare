app.factory("ngTableService",
    [function () {
        var factory = {};

        factory.buildPaginatorIndexs = function (ngTableParams) {
            if (!ngTableParams) new Error("ngTableParams is undefined!");

            return {
                "maxRowIndex": ngTableParams.count(),
                "startRowIndex": (ngTableParams.page() - 1) * ngTableParams.count(),
                "orderByProperty": Object.keys(ngTableParams.sorting())[0],
                "orderByType": Object.values(ngTableParams.sorting())[0]
            }
        }

        return factory;
    }]);