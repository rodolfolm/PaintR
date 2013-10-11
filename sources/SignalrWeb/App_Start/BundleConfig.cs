using System;
using System.Web;
using System.Web.Optimization;

namespace SignalrWeb
{
    public class BundleConfig
    {
        // For more information on Bundling, visit http://go.microsoft.com/fwlink/?LinkId=254725
        public static void RegisterBundles(BundleCollection bundles)
        {
            //we can enable by release mode
            AddDefaultIgnorePatterns(bundles.IgnoreList);
#if DEBUG
                BundleTable.EnableOptimizations = false;
#else
            BundleTable.EnableOptimizations = true;
#endif

            bundles.Add(new ScriptBundle("~/bundles/jquery").Include(
                "~/Scripts/jquery-{version}.js", 
                "~/Scripts/jquery.signalR-1.1.3.min.js"));

            bundles.Add(new ScriptBundle("~/bundles/paint").Include(
                 "~/Scripts/Paint/Paint.js"));


            bundles.Add(new ScriptBundle("~/bundles/jqueryval").Include(
                "~/Scripts/jquery.unobtrusive*",
                "~/Scripts/jquery.validate*"));

            // Use the development version of Modernizr to develop with and learn from. Then, when you're
            // ready for production, use the build tool at http://modernizr.com to pick only the tests you need.
            bundles.Add(new ScriptBundle("~/bundles/modernizr").Include(
                "~/Scripts/modernizr-*"));

            bundles.Add(new StyleBundle("~/Content/css").Include("~/Content/Paint.css"));
        }

        public static void AddDefaultIgnorePatterns(IgnoreList ignoreList)
        {
            try
            {
                if (ignoreList == null)
                    throw new ArgumentNullException("ignoreList");

                ignoreList.Clear();

                ignoreList.Ignore("*.intellisense.js");
                ignoreList.Ignore("*-vsdoc.js");
                ignoreList.Ignore("*.debug.js", OptimizationMode.WhenEnabled);
                //ignoreList.Ignore("*.min.js", OptimizationMode.WhenDisabled);
                //ignoreList.Ignore("*.min.css", OptimizationMode.WhenDisabled);
            }
            catch 
            {
            }
        }
    }
}