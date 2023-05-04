﻿// --------------------------------------------------------------------------------------------------------------------
// <copyright file="SerializationConfigurationTypes.cs" company="OBeautifulCode">
//   Copyright (c) OBeautifulCode 2018. All rights reserved.
// </copyright>
// <auto-generated>
//   Sourced from NuGet package OBeautifulCode.Build.Conventions.VisualStudioProjectTemplates.Domain.Test (1.1.139)
// </auto-generated>
// --------------------------------------------------------------------------------------------------------------------

namespace OBeautifulCode.IO.Test
{
    using System;
    using System.CodeDom.Compiler;
    using System.Diagnostics.CodeAnalysis;

    using OBeautifulCode.Serialization.Bson;
    using OBeautifulCode.Serialization.Json;

    using OBeautifulCode.IO.Serialization.Bson;
    using OBeautifulCode.IO.Serialization.Json;

    [ExcludeFromCodeCoverage]
    [GeneratedCode("OBeautifulCode.Build.Conventions.VisualStudioProjectTemplates.Domain.Test", "1.1.139")]
    public static class SerializationConfigurationTypes
    {
        public static BsonSerializationConfigurationType BsonSerializationConfigurationType => typeof(IOBsonSerializationConfiguration).ToBsonSerializationConfigurationType();

        public static JsonSerializationConfigurationType JsonSerializationConfigurationType => typeof(IOJsonSerializationConfiguration).ToJsonSerializationConfigurationType();
    }
}