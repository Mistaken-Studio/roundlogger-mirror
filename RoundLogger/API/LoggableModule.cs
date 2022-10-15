using Exiled.API.Interfaces;
using JetBrains.Annotations;
using Mistaken.API.Diagnostics;

namespace Mistaken.RoundLogger.API
{
    /// <inheritdoc />
    [PublicAPI]
    public abstract class LoggableModule : Module
    {
        /// <summary>
        /// Initializes a new instance of the <see cref="LoggableModule"/> class.
        /// </summary>
        /// <param name="plugin">This plugin.</param>
        private LoggableModule(IPlugin<IConfig> plugin)
            : base(plugin)
        {
        }

        /// <summary>
        /// Logs a message using RoundLogger.
        /// </summary>
        /// <param name="message">Message to log.</param>
        /// <param name="shortName">Short name of logged event.</param>
        protected void RLog(string message, string shortName = "INFO")
        {
            RLogger.Log($"{Plugin.Name}.{Name}", shortName, message);
        }
    }
}