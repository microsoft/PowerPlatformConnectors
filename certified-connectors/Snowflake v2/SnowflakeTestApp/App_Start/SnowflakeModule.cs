// Copyright (c) Microsoft Corporation.
// Licensed under the MIT license.

namespace SnowflakeTestApp.App_Start
{
    using System.Net.Http;
    using Autofac;
    using Microsoft.Azure.Connectors.SnowflakeV2Contracts.Interfaces;
    using Microsoft.Azure.Connectors.SnowflakeV2Contracts.Models;
    using Microsoft.Extensions.Logging;
    using SnowflakeTestApp.Mocks;
    using SnowflakeV2CoreLogic.Controllers;
    using SnowflakeV2CoreLogic.Models;
    using SnowflakeV2CoreLogic.Providers;
    using SnowflakeV2CoreLogic.Utilities;

    public class SnowflakeModule : Module
    {
        protected override void Load(ContainerBuilder builder)
        {
            // Setup.
            builder.RegisterType<StandardLogger>().SingleInstance().As<ILogger>();

            builder.RegisterType<ConnectionParametersProviderMock>().SingleInstance().As<IConnectionParametersProvider>();

            // Bootstrap for all providers
            builder.RegisterType<SnowflakeConnectionParameters>();
            builder.RegisterType<SnowflakeClient>().InstancePerRequest().As<ISnowflakeClient>();
            builder.RegisterType<SnowflakeDBOperations>().AsSelf().InstancePerRequest();
            builder.RegisterType<SnowflakeConnectionParametersProvider>().AsSelf().InstancePerRequest();

            builder.RegisterType<HttpClient>().SingleInstance();

            // Test connection.
            builder.RegisterType<SnowflakeTestConnectionProvider>().InstancePerRequest().As<IDiagnosticProvider>();
            builder.RegisterType<SnowflakeTestConnectionController>().AsSelf().InstancePerRequest();

            // Query datasets metadata
            builder.RegisterType<SnowflakeDataSetsMetadataProvider>().InstancePerRequest().As<IDataSetsMetadataProvider>();
            builder.RegisterType<SnowflakeDataSetsMetadataController>().AsSelf().InstancePerRequest();

            // Get datasets.
            builder.RegisterType<SnowflakeDataSetProvider>().InstancePerRequest().As<IDataSetProvider>();
            builder.RegisterType<SnowflakeDataSetController>().AsSelf().InstancePerRequest();

            // Querying datasets.
            builder.RegisterType<SnowflakeTableProvider>().InstancePerRequest().As<ITableProvider>();
            builder.RegisterType<SnowflakeTableController>().AsSelf().InstancePerRequest();

            // Querying metadata.
            builder.RegisterType<SnowflakeTableMetadataProvider>().InstancePerRequest().As<ITableMetadataProvider>();
            builder.RegisterType<SnowflakeTableMetadataController>().AsSelf().InstancePerRequest();

            // CRUD on objects.
            builder.RegisterType<SnowflakeTableDataProvider>().InstancePerRequest().As<ITableDataProvider<Item>>();
            builder.RegisterType<SnowflakeTableDataController>().AsSelf().InstancePerRequest();

            // SQL operations
            builder.RegisterType<SnowflakeSQLOperationsProvider>().InstancePerRequest().As<ISnowflakeSQLOperationsProvider>();
            builder.RegisterType<SnowflakeSQLController>().AsSelf().InstancePerRequest();
        }
    }
}