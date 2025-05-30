// Copyright (c) Microsoft Corporation.
// Licensed under the MIT license.

namespace SnowflakeTestApp
{
    using System;
    using System.Reflection;
    using System.Web.Http;
    using System.Web.Http.Dependencies;
    using System.Web.OData.Builder;
    using System.Web.OData.Extensions;
    using System.Web.OData.Formatter;
    using System.Web.OData.Formatter.Deserialization;
    using System.Web.OData.Routing.Conventions;
    using Autofac;
    using Autofac.Integration.WebApi;
    using Microsoft.Azure.Connectors.SnowflakeV2Contracts.Constants;
    using Microsoft.Azure.Connectors.SnowflakeV2Contracts.Models;
    using Microsoft.OData.Edm;
    using Newtonsoft.Json.Converters;

    public static class WebApiConfig
    {
        public static void Register(HttpConfiguration config)
        {
            // Web API configuration and services
            const string DefaultODataRouteName = "DefaultODataRoute";
            config.DependencyResolver = InitializeDependencyResolver(config);

            var snowflakeDataModelBuilder = new ODataConventionModelBuilder();
            var model = GetModel(snowflakeDataModelBuilder);

            // Add string enum convertor
            config.Formatters.JsonFormatter.SerializerSettings.TypeNameHandling = Newtonsoft.Json.TypeNameHandling.None;
            config.Formatters.JsonFormatter.SerializerSettings.Converters.Add(new StringEnumConverter());

            // Web API routes
            config.MapHttpAttributeRoutes();

            // create the formatters with the custom serializer provider and use them in the configuration
            var odataFormatters = ODataMediaTypeFormatters.Create(new CustomODataSerializerProvider(), new DefaultODataDeserializerProvider());
            config.Formatters.InsertRange(0, odataFormatters);

            // Add string enum convertor
            config.Formatters.JsonFormatter.SerializerSettings.TypeNameHandling = Newtonsoft.Json.TypeNameHandling.None;
            config.Formatters.JsonFormatter.SerializerSettings.Converters.Add(new StringEnumConverter());

            config.EnableCaseInsensitive(true);
            config.EnableUnqualifiedNameCall(true);
            config.SetSerializeNullDynamicProperty(true);

            config.AddODataQueryFilter();

            config.MapODataServiceRoute(
                routeName: DefaultODataRouteName,
                routePrefix: null,
                model: model,
                pathHandler: new CustomODataPathHandler(),
                routingConventions: ODataRoutingConventions.CreateDefaultWithAttributeRouting(config, model));

            config.IncludeErrorDetailPolicy = IncludeErrorDetailPolicy.Always;

            config.EnsureInitialized();
        }

        internal static IDependencyResolver InitializeDependencyResolver(HttpConfiguration config)
        {
            if (config == null)
            {
                throw new ArgumentNullException("config");
            }

            ContainerBuilder containerBuilder = new ContainerBuilder();

            // Register single instances
            containerBuilder.RegisterInstance(config);
            containerBuilder.RegisterHttpRequestMessage(config);

            containerBuilder.RegisterAssemblyModules(Assembly.GetExecutingAssembly());
            var container = containerBuilder.Build();

            return new AutofacWebApiDependencyResolver(container);
        }

        internal static IEdmModel GetModel(ODataModelBuilder dataModelBuilder)
        {
            if (dataModelBuilder == null)
            {
                throw new ArgumentNullException(nameof(dataModelBuilder));
            }

            // add data service entities
            dataModelBuilder.EntitySet<DataSet>("datasets");
            dataModelBuilder.EntitySet<Table>("tables");
            dataModelBuilder.EntitySet<Item>("items");
            dataModelBuilder.EntityType<DataSet>().Function("tablesfor").ReturnsCollectionFromEntitySet<Table>("tables");
            dataModelBuilder.EntityType<Item>();
            var tableType = dataModelBuilder.EntityType<Table>();
            tableType.Function(StringConstants.NewItemTrigger).ReturnsCollectionFromEntitySet<Item>("items");
            tableType.Function(StringConstants.UpdatedItemTrigger).ReturnsCollectionFromEntitySet<Item>("items");
            tableType.Function(StringConstants.DeletedItemTrigger).ReturnsCollectionFromEntitySet<Item>("items");
            tableType.Function(StringConstants.ChangedItemTrigger).ReturnsCollectionFromEntitySet<Item>("items");

            return dataModelBuilder.GetEdmModel();
        }
    }
}
