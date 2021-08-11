// -----------------------------------------------------------------------
// <copyright file="RoundLogger.cs" company="Mistaken">
// Copyright (c) Mistaken. All rights reserved.
// </copyright>
// -----------------------------------------------------------------------

using System;
using System.Collections.Generic;
using System.Threading.Tasks;
using Exiled.API.Features;

namespace Mistaken.RoundLogger
{
    /// <summary>
    /// Round Logger.
    /// </summary>
    public static class RLogger
    {
        #region Public

        /// <summary>
        /// Action event invoked on round saving
        /// </summary>
        public static event Action<LogMessage[], DateTime> OnEnd;

        /// <summary>
        /// Used to log message.
        /// </summary>
        /// <param name="module">Module.</param>
        /// <param name="type">Type.</param>
        /// <param name="message">Message.</param>
        public static void Log(string module, string type, string message)
        {
            if (!(PluginHandler.Instance?.Config.IsEnabled ?? false))
                return;
            Logs.Add(new LogMessage(DateTime.Now, type, module, message.Replace("\n", "\\n")));
            if (module != "LOGGER")
                Exiled.API.Features.Log.SendRaw($"[ROUND LOG] [{module}: {type}] {message}", ConsoleColor.DarkYellow);
        }

        /// <summary>
        /// Converts player to string version.
        /// </summary>
        /// <param name="player">Player.</param>
        /// <returns><paramref name="player"/> string version.</returns>
        public static string PlayerToString(this Player player)
            => player == null ? null : $"{player.Nickname} (ID: {player.Id}|UID: {player.UserId}|Class: {player.Role}|Item: {(player.CurrentItemIndex == -1 ? "None" : player.CurrentItem.id.ToString())}{(player.IsConnected ? string.Empty : "|DISCONNECTED")})";

        /// <summary>
        /// Calls <see cref="Ini"/> if <see cref="initiated"/> is <see langword="false"/>.
        /// </summary>
        public static void IniIfNotAlready()
        {
            if (initiated)
                return;
            Ini();
        }

        /// <summary>
        /// Log object.
        /// </summary>
        public struct LogMessage
        {
            /// <summary>
            /// Log time.
            /// </summary>
            public DateTime Time;

            /// <summary>
            /// Log Type.
            /// </summary>
            public string Type;

            /// <summary>
            /// Log Module.
            /// </summary>
            public string Module;

            /// <summary>
            /// Log Message.
            /// </summary>
            public string Message;

            /// <summary>
            /// Initializes a new instance of the <see cref="LogMessage"/> struct.
            /// </summary>
            public LogMessage(DateTime time, string type, string module, string message)
            {
                this.Time = time;
                this.Type = type;
                this.Module = module;
                this.Message = message;

                if (!Types.Contains(this.Type))
                    RegisterTypes(this.Type);
                if (!Modules.Contains(this.Module))
                    RegisterModules(this.Module);
            }

            /// <inheritdoc/>
            public override string ToString()
            {
                string tmpType = this.Type;
                while (tmpType.Length < typesMaxLength)
                    tmpType += " ";
                string tmpModule = this.Module;
                while (tmpModule.Length < modulesMaxLength)
                    tmpModule += " ";
                return $"{this.Time:HH:mm:ss.fff} | {tmpModule} | {tmpType} | {this.Message}";
            }
        }
        #endregion

        private static readonly List<LogMessage> Logs = new List<LogMessage>();

        private static readonly HashSet<string> Types = new HashSet<string>();
        private static readonly HashSet<string> Modules = new HashSet<string>();
        private static byte typesMaxLength = 0;
        private static byte modulesMaxLength = 0;

        private static bool initiated = false;

        private static DateTime beginLog = DateTime.Now;

        private static void RegisterTypes(string type)
        {
            Types.Add(type);
            typesMaxLength = (byte)Math.Max(typesMaxLength, type.Length);
        }

        private static void RegisterModules(string module)
        {
            Modules.Add(module);
            modulesMaxLength = (byte)Math.Max(modulesMaxLength, module.Length);
        }

        private static void Ini()
        {
            initiated = true;
            Exiled.API.Features.Log.Debug("Initiated RoundLogger");
            Exiled.Events.Handlers.Server.RestartingRound += Server_RestartingRound;
            Log("LOGGER", "INFO", "Start of log");
            beginLog = DateTime.Now;
        }

        private static void Server_RestartingRound() => _ = Server_RestartingRoundTask();

        private static async Task Server_RestartingRoundTask()
        {
            await Task.Delay(10);
            Log("LOGGER", "INFO", "End of log");
            var logsArray = Logs.ToArray();
            Logs.Clear();
            Log("LOGGER", "INFO", "Start of log");
            OnEnd?.Invoke(logsArray, beginLog);
            beginLog = DateTime.Now;
        }
    }
}
