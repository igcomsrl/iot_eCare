//Concesso in licenza a norma dell'EUPL, versione 1.2. 2019

//Concesso in licenza a norma dell'EUPL, versione 1.2
using System.Configuration;
using System.Web.Configuration;
using System.Web.Optimization;
using MateSharp.Framework.Helpers.Bundle.Angular;

namespace Meti.App.App_Start
{
    public class BundleConfig
    {
        public static void RegisterBundles(BundleCollection bundles)
        {
            var isDebug = ((CompilationSection)ConfigurationManager.GetSection("system.web/compilation")).Debug; //&& !BundleTable.EnableOptimizations;

            BundleTable.EnableOptimizations = !isDebug;

            #region Angular App
            ScriptBundle angularBundleScript = new ScriptBundle("~/Dist/angularApp");
            angularBundleScript.IncludeDirectory("~/App", "*.js");
            angularBundleScript.IncludeDirectory("~/App/services", "*.js");
            angularBundleScript.IncludeDirectory("~/App/directives", "*.js");
            angularBundleScript.IncludeDirectory("~/App/controllers/registry", "*.js");
            angularBundleScript.IncludeDirectory("~/App/controllers/process", "*.js");
            angularBundleScript.IncludeDirectory("~/App/controllers/parameter", "*.js");
            angularBundleScript.IncludeDirectory("~/App/controllers/alarm", "*.js");
            angularBundleScript.IncludeDirectory("~/App/controllers/processMacro", "*.js");
            angularBundleScript.IncludeDirectory("~/App/controllers/processInstance", "*.js");
            angularBundleScript.IncludeDirectory("~/App/controllers/device", "*.js");
            angularBundleScript.IncludeDirectory("~/App/controllers/deviceErrorLog", "*.js");
            angularBundleScript.IncludeDirectory("~/App/controllers/geolocation", "*.js");
            angularBundleScript.IncludeDirectory("~/App/controllers/alarmFired", "*.js");
            angularBundleScript.IncludeDirectory("~/App/controllers/layout", "*.js");
            angularBundleScript.IncludeDirectory("~/App/controllers/alarmMetric", "*.js");
            angularBundleScript.IncludeDirectory("~/App/controllers/user", "*.js");
            angularBundleScript.IncludeDirectory("~/App/controllers/role", "*.js");
            angularBundleScript.IncludeDirectory("~/App/controllers/areaPaziente", "*.js");
            angularBundleScript.IncludeDirectory("~/App/controllers/healthRisk", "*.js");

            bundles.Add(angularBundleScript);
            #endregion

            #region Angular Template cache
            // Add the templates for the app
            var appModuleTemplateCacheBundle = new AngularTemplatesBundle("app", "~/Dist/templates/app").IncludeDirectory("~/App/templates/", "*.html", true);

            bundles.Add(appModuleTemplateCacheBundle);
            #endregion

            #region Stylesheet
            StyleBundle bundleCss = new StyleBundle("~/Dist/css");

            //Custom / Modules css            

            bundleCss.Include("~/Content/node_modules/angular-toastr/dist/angular-toastr.css", new CssRewriteUrlTransformWrapper());
            bundleCss.Include("~/Content/node_modules/animate/animate.css", new CssRewriteUrlTransformWrapper());
            bundleCss.Include("~/Content/node_modules/ng-table/bundles/ng-table.min.css", new CssRewriteUrlTransformWrapper());
            bundleCss.Include("~/Content/node_modules/angular-loading-bar/build/loading-bar.css", new CssRewriteUrlTransformWrapper());

            //Metronic css
            bundleCss.Include("~/Content/metronic/assets/global/plugins/simple-line-icons/simple-line-icons.min.css", new CssRewriteUrlTransformWrapper());
            bundleCss.Include("~/Content/metronic/assets/global/plugins/bootstrap/css/bootstrap.min.css", new CssRewriteUrlTransformWrapper());
            bundleCss.Include("~/Content/metronic/assets/global/plugins/uniform/css/uniform.default.css", new CssRewriteUrlTransformWrapper());
            bundleCss.Include("~/Content/metronic/assets/global/plugins/bootstrap-switch/css/bootstrap-switch.min.css", new CssRewriteUrlTransformWrapper());
            bundleCss.Include("~/Content/metronic/assets/global/css/components-rounded.css", new CssRewriteUrlTransformWrapper());
            bundleCss.Include("~/Content/metronic/assets/global/css/plugins.css", new CssRewriteUrlTransformWrapper());
            bundleCss.Include("~/Content/metronic/assets/admin/layout4/css/layout.css", new CssRewriteUrlTransformWrapper());
            bundleCss.Include("~/Content/metronic/assets/admin/layout4/css/themes/light.css", new CssRewriteUrlTransformWrapper());
            bundleCss.Include("~/Content/metronic/assets/admin/pages/css/login-soft.css", new CssRewriteUrlTransformWrapper());
            bundleCss.Include("~/Content/metronic/assets/admin/pages/css/pricing-table.css", new CssRewriteUrlTransformWrapper());
            bundleCss.Include("~/Content/metronic/assets/admin/pages/css/pricing-tables.css", new CssRewriteUrlTransformWrapper());
            bundleCss.Include("~/Content/metronic/assets/admin/pages/css/profile.css", new CssRewriteUrlTransformWrapper());
            bundleCss.Include("~/Content/metronic/assets/admin/pages/css/tasks.css", new CssRewriteUrlTransformWrapper());
            bundleCss.Include("~/Content/metronic/assets/admin/pages/css/timeline.css", new CssRewriteUrlTransformWrapper());

            //Kendo/Telerik css
            bundleCss.Include("~/Content/kendo/css/kendo.common-bootstrap.min.css", new CssRewriteUrlTransformWrapper());
            bundleCss.Include("~/Content/kendo/css/kendo.bootstrap.min.css", new CssRewriteUrlTransformWrapper());
            bundleCss.Include("~/Content/kendo/css/kendo.dataviz.min.css", new CssRewriteUrlTransformWrapper());
            bundleCss.Include("~/Content/kendo/css/kendo.dataviz.bootstrap.min.css", new CssRewriteUrlTransformWrapper());

            //App css
            bundleCss.Include("~/Content/app/css/app.css", new CssRewriteUrlTransformWrapper());
            bundles.Add(bundleCss);
            #endregion

            #region Javascript
            ScriptBundle bundleScript = new ScriptBundle("~/Dist/js");


            //Custom node_modules
            //Metronic Core plugins
            bundleScript.Include("~/Content/metronic/assets/global/plugins/respond.min.js");
            bundleScript.Include("~/Content/metronic/assets/global/plugins/excanvas.min.js");
            bundleScript.Include("~/Content/metronic/assets/global/plugins/jquery.min.js");
            bundleScript.Include("~/Content/metronic/assets/global/plugins/jquery-migrate.min.js");
            bundleScript.Include("~/Content/metronic/assets/global/plugins/jquery-ui/jquery-ui.min.js");
            bundleScript.Include("~/Content/metronic/assets/global/plugins/bootstrap/js/bootstrap.min.js");
            bundleScript.Include("~/Content/metronic/assets/global/plugins/bootstrap-hover-dropdown/bootstrap-hover-dropdown.min.js");
            bundleScript.Include("~/Content/metronic/assets/global/plugins/jquery-slimscroll/jquery.slimscroll.min.js");
            bundleScript.Include("~/Content/metronic/assets/global/plugins/jquery.blockui.min.js");
            bundleScript.Include("~/Content/metronic/assets/global/plugins/jquery.cokie.min.js");
            bundleScript.Include("~/Content/metronic/assets/global/plugins/uniform/jquery.uniform.min.js");
            bundleScript.Include("~/Content/metronic/assets/global/plugins/bootstrap-switch/js/bootstrap-switch.min.js");

            bundleScript.Include("~/Content/node_modules/angular/angular.js");
            bundleScript.Include("~/Content/node_modules/angular-animate/angular-animate.min.js");
            bundleScript.Include("~/Content/node_modules/angular-ui-router/release/angular-ui-router.js");
            bundleScript.Include("~/Content/node_modules/angular-toastr/dist/angular-toastr.tpls.js");
            bundleScript.Include("~/Content/node_modules/ng-table/bundles/ng-table.min.js");
            bundleScript.Include("~/Content/node_modules/angular-ui-bootstrap/dist/ui-bootstrap-tpls.js");
            bundleScript.Include("~/Content/node_modules/linq/linq.js");
            bundleScript.Include("~/Content/node_modules/angular-loading-bar/build/loading-bar.min.js");
            bundleScript.Include("~/Content/node_modules/moment/moment.js");
            bundleScript.Include("~/Content/node_modules/ng-file-upload/dist/ng-file-upload.min.js");
            bundleScript.Include("~/Content/ngAutocomplete.js");

            bundleScript.Include("~/Content/igcom/js/igcom-framework.js");
            bundleScript.Include("~/Content/igcom/js/igcom-zeus.js");

            bundleScript.Include("~/Content/metronic/assets/global/scripts/metronic.js");
            bundleScript.Include("~/Content/metronic/assets/admin/layout4/scripts/layout.js");
            bundleScript.Include("~/Content/metronic/assets/admin/layout4/scripts/demo.js");
            bundleScript.Include("~/Content/metronic/assets/admin/pages/scripts/index3.js");
            bundleScript.Include("~/Content/metronic/assets/admin/pages/scripts/tasks.js");
            bundleScript.Include("~/Content/metronic/assets/admin/pages/scripts/login-soft.js");

            //Ionic app
            //bundleScript.Include("~/Ionic/www/app/app.js");

            bundles.Add(bundleScript);

            #endregion

            #region KendoUi
            ScriptBundle bundleKendoScript = new ScriptBundle("~/Dist/kendoUi");

            bundleKendoScript.Include("~/Content/kendo/js/kendo.all.min.js");
            bundleKendoScript.Include("~/Content/kendo/js/cultures/kendo.culture.it-IT.min.js");
            //bundleKendoScript.Include("~/Content/telerik/reportViewer/js/telerikReportViewer-10.1.16.504.js");
            //bundleKendoScript.Include("~/Content/kendo/js/messages/kendo.messages.it-IT.min.js");
            //bundleKendoScript.Include("~/Content/kendo/js/cultures/kendo.culture.de-DE.min.js");
            //bundleKendoScript.Include("~/Content/kendo/js/messages/kendo.messages.de-DE.js");
            bundles.Add(bundleKendoScript);
            #endregion

        }
    }
}
