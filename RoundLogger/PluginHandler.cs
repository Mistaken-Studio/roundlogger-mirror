// -----------------------------------------------------------------------
// <copyright file="PluginHandler.cs" company="Mistaken">
// Copyright (c) Mistaken. All rights reserved.
// </copyright>
// -----------------------------------------------------------------------

using System;
using Exiled.API.Enums;
using Exiled.API.Features;
using JetBrains.Annotations;
using Mistaken.API.Diagnostics;

namespace Mistaken.RoundLogger
{
    /// <inheritdoc/>
    [UsedImplicitly]
    internal class PluginHandler : Plugin<Config>
    {
        /// <inheritdoc/>
        public override string Author => "Mistaken Devs";

        /// <inheritdoc/>
        public override string Name => "RoundLogger";

        /// <inheritdoc/>
        public override string Prefix => "MRL";

        /// <inheritdoc/>
        public override PluginPriority Priority => PluginPriority.Low;

        /// <inheritdoc/>
        public override Version RequiredExiledVersion => new Version(5, 1, 3);

        /// <inheritdoc/>
        public override void OnEnabled()
        {
            Instance = this;

            Module.RegisterHandler<RoundLogHandler>(this);

            Module.OnEnable(this);

            base.OnEnabled();
        }

        /// <inheritdoc/>
        public override void OnDisabled()
        {
            Module.OnDisable(this);

            base.OnDisabled();
        }

        internal static PluginHandler Instance { get; private set; }
    }
}
