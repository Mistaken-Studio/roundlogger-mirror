// -----------------------------------------------------------------------
// <copyright file="RoundLogHandler.cs" company="Mistaken">
// Copyright (c) Mistaken. All rights reserved.
// </copyright>
// -----------------------------------------------------------------------

using System;
using System.IO;
using System.Linq;
using System.Reflection.Emit;
using Exiled.API.Features;
using Mistaken.API.Diagnostics;
using UnityEngine;

namespace Mistaken.RoundLogger
{
    internal class RoundLogHandler : Module
    {
        /// <inheritdoc cref="Module.Module(Exiled.API.Interfaces.IPlugin{Exiled.API.Interfaces.IConfig})"/>
        public RoundLogHandler(PluginHandler p)
            : base(p)
        {
            RLogger.IniIfNotAlready();
            RLogger.OnEnd += this.RoundLogHandler_OnEnd;
        }

        public override bool IsBasic => true;

        public override string Name => "RoundLog";

        public override void OnEnable()
        {
            this.CallDelayed(
                1.5f,
                () =>
                {
                    Exiled.Events.Handlers.Player.Banned += this.Handle<Exiled.Events.EventArgs.BannedEventArgs>((ev) => this.Player_Banned(ev));
                    Exiled.Events.Handlers.Player.Kicked += this.Handle<Exiled.Events.EventArgs.KickedEventArgs>((ev) => this.Player_Kicked(ev));
                    Exiled.Events.Handlers.Player.Died += this.Handle<Exiled.Events.EventArgs.DiedEventArgs>((ev) => this.Player_Died(ev));
                    Exiled.Events.Handlers.Player.Hurting += this.Handle<Exiled.Events.EventArgs.HurtingEventArgs>((ev) => this.Player_Hurting(ev));
                    Exiled.Events.Handlers.Player.PreAuthenticating += this.Handle<Exiled.Events.EventArgs.PreAuthenticatingEventArgs>((ev) => this.Player_PreAuthenticating(ev));
                    Exiled.Events.Handlers.Player.Verified += this.Handle<Exiled.Events.EventArgs.VerifiedEventArgs>((ev) => this.Player_Verified(ev));
                    Events.Handlers.CustomEvents.OnFirstTimeJoined += this.Handle<Events.EventArgs.FirstTimeJoinedEventArgs>((ev) => this.CustomEvents_OnFirstTimeJoined(ev));
                    Exiled.Events.Handlers.Player.Destroying += this.Handle<Exiled.Events.EventArgs.DestroyingEventArgs>((ev) => this.Player_Destroying(ev));
                    Exiled.Events.Handlers.Player.Left += this.Handle<Exiled.Events.EventArgs.LeftEventArgs>((ev) => this.Player_Left(ev));
                    Exiled.Events.Handlers.Player.Escaping += this.Handle<Exiled.Events.EventArgs.EscapingEventArgs>((ev) => this.Player_Escaping(ev));
                    Exiled.Events.Handlers.Player.ChangingRole += this.Handle<Exiled.Events.EventArgs.ChangingRoleEventArgs>((ev) => this.Player_ChangingRole(ev));
                    Exiled.Events.Handlers.Player.ChangingGroup += this.Handle<Exiled.Events.EventArgs.ChangingGroupEventArgs>((ev) => this.Player_ChangingGroup(ev));
                    Exiled.Events.Handlers.Player.ThrowingGrenade += this.Handle<Exiled.Events.EventArgs.ThrowingGrenadeEventArgs>((ev) => this.Player_ThrowingGrenade(ev));
                    Exiled.Events.Handlers.Player.Handcuffing += this.Handle<Exiled.Events.EventArgs.HandcuffingEventArgs>((ev) => this.Player_Handcuffing(ev));
                    Exiled.Events.Handlers.Player.RemovingHandcuffs += this.Handle<Exiled.Events.EventArgs.RemovingHandcuffsEventArgs>((ev) => this.Player_RemovingHandcuffs(ev));
                    Exiled.Events.Handlers.Player.IntercomSpeaking += this.Handle<Exiled.Events.EventArgs.IntercomSpeakingEventArgs>((ev) => this.Player_IntercomSpeaking(ev));
                    Exiled.Events.Handlers.Player.ActivatingWarheadPanel += this.Handle<Exiled.Events.EventArgs.ActivatingWarheadPanelEventArgs>((ev) => this.Player_ActivatingWarheadPanel(ev));
                    Exiled.Events.Handlers.Player.ReceivingEffect += this.Handle<Exiled.Events.EventArgs.ReceivingEffectEventArgs>((ev) => this.Player_ReceivingEffect(ev));
                    Exiled.Events.Handlers.Scp914.Activating += this.Handle<Exiled.Events.EventArgs.ActivatingEventArgs>((ev) => this.Scp914_Activating(ev));
                    Exiled.Events.Handlers.Scp079.TriggeringDoor += this.Handle<Exiled.Events.EventArgs.TriggeringDoorEventArgs>((ev) => this.Scp079_TriggeringDoor(ev));
                    Exiled.Events.Handlers.Scp079.InteractingTesla += this.Handle<Exiled.Events.EventArgs.InteractingTeslaEventArgs>((ev) => this.Scp079_InteractingTesla(ev));
                    Exiled.Events.Handlers.Scp096.AddingTarget += this.Handle<Exiled.Events.EventArgs.AddingTargetEventArgs>((ev) => this.Scp096_AddingTarget(ev));
                    Exiled.Events.Handlers.Scp096.Enraging += this.Handle<Exiled.Events.EventArgs.EnragingEventArgs>((ev) => this.Scp096_Enraging(ev));
                    Exiled.Events.Handlers.Warhead.Detonated += this.Handle(() => this.Warhead_Detonated(), "WarheadDetonated");
                    Exiled.Events.Handlers.Warhead.Starting += this.Handle<Exiled.Events.EventArgs.StartingEventArgs>((ev) => this.Warhead_Starting(ev));
                    Exiled.Events.Handlers.Warhead.Stopping += this.Handle<Exiled.Events.EventArgs.StoppingEventArgs>((ev) => this.Warhead_Stopping(ev));
                    Exiled.Events.Handlers.Server.RoundEnded += this.Handle<Exiled.Events.EventArgs.RoundEndedEventArgs>((ev) => this.Server_RoundEnded(ev));
                    Exiled.Events.Handlers.Server.WaitingForPlayers += this.Handle(() => this.Server_WaitingForPlayers(), "WaitingForPlayers");
                    Exiled.Events.Handlers.Server.RoundStarted += this.Handle(() => this.Server_RoundStarted(), "RoundStart");
                    Exiled.Events.Handlers.Server.RespawningTeam += this.Handle<Exiled.Events.EventArgs.RespawningTeamEventArgs>((ev) => this.Server_RespawningTeam(ev));
                    Exiled.Events.Handlers.Server.SendingRemoteAdminCommand += this.Handle<Exiled.Events.EventArgs.SendingRemoteAdminCommandEventArgs>((ev) => this.Server_SendingRemoteAdminCommand(ev));
                    Exiled.Events.Handlers.Server.SendingConsoleCommand += this.Handle<Exiled.Events.EventArgs.SendingConsoleCommandEventArgs>((ev) => this.Server_SendingConsoleCommand(ev));
                    Exiled.Events.Handlers.Map.Decontaminating += this.Handle<Exiled.Events.EventArgs.DecontaminatingEventArgs>((ev) => this.Map_Decontaminating(ev));
                    Exiled.Events.Handlers.Map.GeneratorActivated += this.Handle<Exiled.Events.EventArgs.GeneratorActivatedEventArgs>((ev) => this.Map_GeneratorActivated(ev));
                    Exiled.Events.Handlers.Map.ExplodingGrenade += this.Handle<Exiled.Events.EventArgs.ExplodingGrenadeEventArgs>((ev) => this.Map_ExplodingGrenade(ev));
                    Events.Handlers.CustomEvents.OnBroadcast += this.Handle<Events.EventArgs.BroadcastEventArgs>((ev) => this.CustomEvents_OnBroadcast(ev));
                    Exiled.Events.Handlers.Player.PickingUpItem += this.Handle<Exiled.Events.EventArgs.PickingUpItemEventArgs>((ev) => this.Player_PickingUpItem(ev));
                    Exiled.Events.Handlers.Player.DroppingItem += this.Handle<Exiled.Events.EventArgs.DroppingItemEventArgs>((ev) => this.Player_DroppingItem(ev));
                    Exiled.Events.Handlers.Player.MedicalItemDequipped += this.Handle<Exiled.Events.EventArgs.DequippedMedicalItemEventArgs>((ev) => this.Player_MedicalItemDequipped(ev));

                    API.AnnonymousEvents.Subscribe("VANISH", this.OnVanishUpdate); // (Player player, byte level)
                },
                "LateEnable");
        }

        public override void OnDisable()
        {
            Exiled.Events.Handlers.Player.Banned -= this.Handle<Exiled.Events.EventArgs.BannedEventArgs>((ev) => this.Player_Banned(ev));
            Exiled.Events.Handlers.Player.Kicked -= this.Handle<Exiled.Events.EventArgs.KickedEventArgs>((ev) => this.Player_Kicked(ev));
            Exiled.Events.Handlers.Player.Died -= this.Handle<Exiled.Events.EventArgs.DiedEventArgs>((ev) => this.Player_Died(ev));
            Exiled.Events.Handlers.Player.Hurting -= this.Handle<Exiled.Events.EventArgs.HurtingEventArgs>((ev) => this.Player_Hurting(ev));
            Exiled.Events.Handlers.Player.PreAuthenticating -= this.Handle<Exiled.Events.EventArgs.PreAuthenticatingEventArgs>((ev) => this.Player_PreAuthenticating(ev));
            Exiled.Events.Handlers.Player.Verified -= this.Handle<Exiled.Events.EventArgs.VerifiedEventArgs>((ev) => this.Player_Verified(ev));
            Events.Handlers.CustomEvents.OnFirstTimeJoined -= this.Handle<Events.EventArgs.FirstTimeJoinedEventArgs>((ev) => this.CustomEvents_OnFirstTimeJoined(ev));
            Exiled.Events.Handlers.Player.Destroying -= this.Handle<Exiled.Events.EventArgs.DestroyingEventArgs>((ev) => this.Player_Destroying(ev));
            Exiled.Events.Handlers.Player.Left -= this.Handle<Exiled.Events.EventArgs.LeftEventArgs>((ev) => this.Player_Left(ev));
            Exiled.Events.Handlers.Player.Escaping -= this.Handle<Exiled.Events.EventArgs.EscapingEventArgs>((ev) => this.Player_Escaping(ev));
            Exiled.Events.Handlers.Player.ChangingRole -= this.Handle<Exiled.Events.EventArgs.ChangingRoleEventArgs>((ev) => this.Player_ChangingRole(ev));
            Exiled.Events.Handlers.Player.ChangingGroup -= this.Handle<Exiled.Events.EventArgs.ChangingGroupEventArgs>((ev) => this.Player_ChangingGroup(ev));
            Exiled.Events.Handlers.Player.ThrowingGrenade -= this.Handle<Exiled.Events.EventArgs.ThrowingGrenadeEventArgs>((ev) => this.Player_ThrowingGrenade(ev));
            Exiled.Events.Handlers.Player.Handcuffing -= this.Handle<Exiled.Events.EventArgs.HandcuffingEventArgs>((ev) => this.Player_Handcuffing(ev));
            Exiled.Events.Handlers.Player.RemovingHandcuffs -= this.Handle<Exiled.Events.EventArgs.RemovingHandcuffsEventArgs>((ev) => this.Player_RemovingHandcuffs(ev));
            Exiled.Events.Handlers.Player.IntercomSpeaking -= this.Handle<Exiled.Events.EventArgs.IntercomSpeakingEventArgs>((ev) => this.Player_IntercomSpeaking(ev));
            Exiled.Events.Handlers.Player.ActivatingWarheadPanel -= this.Handle<Exiled.Events.EventArgs.ActivatingWarheadPanelEventArgs>((ev) => this.Player_ActivatingWarheadPanel(ev));
            Exiled.Events.Handlers.Player.ReceivingEffect -= this.Handle<Exiled.Events.EventArgs.ReceivingEffectEventArgs>((ev) => this.Player_ReceivingEffect(ev));
            Exiled.Events.Handlers.Scp914.Activating -= this.Handle<Exiled.Events.EventArgs.ActivatingEventArgs>((ev) => this.Scp914_Activating(ev));
            Exiled.Events.Handlers.Scp079.TriggeringDoor -= this.Handle<Exiled.Events.EventArgs.TriggeringDoorEventArgs>((ev) => this.Scp079_TriggeringDoor(ev));
            Exiled.Events.Handlers.Scp079.InteractingTesla -= this.Handle<Exiled.Events.EventArgs.InteractingTeslaEventArgs>((ev) => this.Scp079_InteractingTesla(ev));
            Exiled.Events.Handlers.Scp096.AddingTarget -= this.Handle<Exiled.Events.EventArgs.AddingTargetEventArgs>((ev) => this.Scp096_AddingTarget(ev));
            Exiled.Events.Handlers.Scp096.Enraging -= this.Handle<Exiled.Events.EventArgs.EnragingEventArgs>((ev) => this.Scp096_Enraging(ev));
            Exiled.Events.Handlers.Warhead.Detonated -= this.Handle(() => this.Warhead_Detonated(), "WarheadDetonated");
            Exiled.Events.Handlers.Warhead.Starting -= this.Handle<Exiled.Events.EventArgs.StartingEventArgs>((ev) => this.Warhead_Starting(ev));
            Exiled.Events.Handlers.Warhead.Stopping -= this.Handle<Exiled.Events.EventArgs.StoppingEventArgs>((ev) => this.Warhead_Stopping(ev));
            Exiled.Events.Handlers.Server.RoundEnded -= this.Handle<Exiled.Events.EventArgs.RoundEndedEventArgs>((ev) => this.Server_RoundEnded(ev));
            Exiled.Events.Handlers.Server.WaitingForPlayers -= this.Handle(() => this.Server_WaitingForPlayers(), "WaitingForPlayers");
            Exiled.Events.Handlers.Server.RoundStarted -= this.Handle(() => this.Server_RoundStarted(), "RoundStart");
            Exiled.Events.Handlers.Server.RespawningTeam -= this.Handle<Exiled.Events.EventArgs.RespawningTeamEventArgs>((ev) => this.Server_RespawningTeam(ev));
            Exiled.Events.Handlers.Server.SendingRemoteAdminCommand -= this.Handle<Exiled.Events.EventArgs.SendingRemoteAdminCommandEventArgs>((ev) => this.Server_SendingRemoteAdminCommand(ev));
            Exiled.Events.Handlers.Server.SendingConsoleCommand -= this.Handle<Exiled.Events.EventArgs.SendingConsoleCommandEventArgs>((ev) => this.Server_SendingConsoleCommand(ev));
            Exiled.Events.Handlers.Map.Decontaminating -= this.Handle<Exiled.Events.EventArgs.DecontaminatingEventArgs>((ev) => this.Map_Decontaminating(ev));
            Exiled.Events.Handlers.Map.GeneratorActivated -= this.Handle<Exiled.Events.EventArgs.GeneratorActivatedEventArgs>((ev) => this.Map_GeneratorActivated(ev));
            Exiled.Events.Handlers.Map.ExplodingGrenade -= this.Handle<Exiled.Events.EventArgs.ExplodingGrenadeEventArgs>((ev) => this.Map_ExplodingGrenade(ev));
            Exiled.Events.Handlers.Server.SendingRemoteAdminCommand -= this.Handle<Exiled.Events.EventArgs.SendingRemoteAdminCommandEventArgs>((ev) => this.Server_SendingRemoteAdminCommand(ev));
            Exiled.Events.Handlers.Server.SendingConsoleCommand -= this.Handle<Exiled.Events.EventArgs.SendingConsoleCommandEventArgs>((ev) => this.Server_SendingConsoleCommand(ev));
            Events.Handlers.CustomEvents.OnBroadcast -= this.Handle<Events.EventArgs.BroadcastEventArgs>((ev) => this.CustomEvents_OnBroadcast(ev));
            Exiled.Events.Handlers.Player.PickingUpItem -= this.Handle<Exiled.Events.EventArgs.PickingUpItemEventArgs>((ev) => this.Player_PickingUpItem(ev));
            Exiled.Events.Handlers.Player.DroppingItem -= this.Handle<Exiled.Events.EventArgs.DroppingItemEventArgs>((ev) => this.Player_DroppingItem(ev));
            Exiled.Events.Handlers.Player.MedicalItemDequipped -= this.Handle<Exiled.Events.EventArgs.DequippedMedicalItemEventArgs>((ev) => this.Player_MedicalItemDequipped(ev));

            API.AnnonymousEvents.UnSubscribe("VANISH", this.OnVanishUpdate); // (Player player, byte level)
        }

        private void OnVanishUpdate(object rawEv)
        {
            var ev = ((Player player, byte level))rawEv;
            if (ev.level == 0)
                RLogger.Log("VANISH", "DISABLE", $"Vanish enabled for {ev.player.PlayerToString()}, level {ev.level}");
            else
                RLogger.Log("VANISH", "ENABLE", $"Vanish disabled for {ev.player.PlayerToString()}");
        }

        private void RoundLogHandler_OnEnd(RLogger.LogMessage[] logs, DateTime roundStart)
        {
            string dir = Paths.Plugins + "/RoundLogger/";
            if (!Directory.Exists(dir))
                Directory.CreateDirectory(dir);
            dir += Server.Port + "/";
            if (!Directory.Exists(dir))
                Directory.CreateDirectory(dir);
            File.AppendAllLines(dir + $"{roundStart:yyyy-MM-dd_HH-mm-ss}.log", logs.Select(i => i.ToString()));
        }

        private void Player_MedicalItemDequipped(Exiled.Events.EventArgs.DequippedMedicalItemEventArgs ev)
        {
            RLogger.Log("GAME EVENT", "MEDICAL", $"{ev.Player.PlayerToString()} used {ev.Item}");
        }

        private void Player_DroppingItem(Exiled.Events.EventArgs.DroppingItemEventArgs ev)
        {
            RLogger.Log("GAME EVENT", "ITEM", $"{ev.Player.PlayerToString()} dropped {ev.Item.id} (Durability: {ev.Item.durability}) with result: {(ev.IsAllowed ? "Allowed" : "Disallowed")}");
        }

        private void Player_PickingUpItem(Exiled.Events.EventArgs.PickingUpItemEventArgs ev)
        {
            RLogger.Log("GAME EVENT", "ITEM", $"{ev.Player.PlayerToString()} picked up {ev.Pickup.ItemId} (Durability: {ev.Pickup.durability}) with result: {(ev.IsAllowed ? "Allowed" : "Disallowed")}");
        }

        private void CustomEvents_OnBroadcast(Events.EventArgs.BroadcastEventArgs ev)
        {
            if (ev.Type == Broadcast.BroadcastFlags.AdminChat)
            {
                RLogger.Log("ADMIN EVENT", "ADMIN CHAT", $"{ev.AdminName} sent \"{ev.Content}\"");
            }
            else
            {
                RLogger.Log("GAME EVENT", "BROADCAST", $"Broadcasted \"{ev.Content}\" to {ev.Targets.Length} players");
            }
        }

        private void Map_GeneratorActivated(Exiled.Events.EventArgs.GeneratorActivatedEventArgs ev)
        {
            RLogger.Log("GAME EVENT", "GENERATOR", $"Generator in {ev.Generator.CurRoom} activated");
        }

        private void Map_ExplodingGrenade(Exiled.Events.EventArgs.ExplodingGrenadeEventArgs ev)
        {
            RLogger.Log("GAME EVENT", "GRENADE", $"{this.PTS(ev.Thrower)}'s {(ev.IsFrag ? "Frag" : "Flash")} exploading on {ev.TargetToDamages.Count} target ({(ev.IsAllowed ? "allowed" : "denied")})");
        }

        private void Map_Decontaminating(Exiled.Events.EventArgs.DecontaminatingEventArgs ev)
        {
            RLogger.Log("GAME EVENT", "DECONTAMINATION", $"Decontamination finished ({(ev.IsAllowed ? "allowed" : "denied")})");
        }

        private void Server_SendingConsoleCommand(Exiled.Events.EventArgs.SendingConsoleCommandEventArgs ev)
        {
            RLogger.Log("GAME EVENT", "CONSOLE", $"{this.PTS(ev.Player)} run {ev.Name} with args ({string.Join(", ", ev.Arguments)}) with result: {(ev.IsAllowed ? "Allowed" : "Disallowed")}");
        }

        private void Server_SendingRemoteAdminCommand(Exiled.Events.EventArgs.SendingRemoteAdminCommandEventArgs ev)
        {
            RLogger.Log("ADMIN EVENT", "RA", $"{this.PTS(ev.Sender) ?? "Console"} run {ev.Name} with args ({string.Join(", ", ev.Arguments)}) with result: {(ev.Success ? "Success" : "Failure")}");
        }

        private void Server_RespawningTeam(Exiled.Events.EventArgs.RespawningTeamEventArgs ev)
        {
            RLogger.Log("GAME EVENT", "TEAM RESPAWN", $"Spawning {(ev.NextKnownTeam == Respawning.SpawnableTeamType.ChaosInsurgency ? "Chaos" : "MTF")}, {ev.Players.Count} players out of max {ev.MaximumRespawnAmount} ({(ev.IsAllowed ? "allowed" : "denied")})");
        }

        private void Server_RoundStarted()
        {
            RLogger.Log("GAME EVENT", "ROUND STARTED", $"Round has been started");
        }

        private void Server_WaitingForPlayers()
        {
            RLogger.Log("GAME EVENT", "ROUND READY", $"Waiting for players to join");
        }

        private void Server_RoundEnded(Exiled.Events.EventArgs.RoundEndedEventArgs ev)
        {
            RLogger.Log("GAME EVENT", "ROUND END", $"Round has ended, {ev.LeadingTeam} won, restarting in {ev.TimeToRestart}s");
        }

        private void Warhead_Stopping(Exiled.Events.EventArgs.StoppingEventArgs ev)
        {
            RLogger.Log("WARHEAD EVENT", "STOP", $"{this.PTS(ev.Player)} stoped warhead ({(ev.IsAllowed ? "allowed" : "denied")})");
        }

        private void Warhead_Starting(Exiled.Events.EventArgs.StartingEventArgs ev)
        {
            RLogger.Log("WARHEAD EVENT", "START", $"{this.PTS(ev.Player)} started warhead ({(ev.IsAllowed ? "allowed" : "denied")})");
        }

        private void Warhead_Detonated()
        {
            RLogger.Log("WARHEAD EVENT", "DETONATED", $"Warhead Detonated");
        }

        private void Scp096_Enraging(Exiled.Events.EventArgs.EnragingEventArgs ev)
        {
            RLogger.Log("SCP096 EVENT", "ENRAGE", $"{this.PTS(ev.Player)} enraged {this.PTS(Player.Get(ev.Scp096.Hub))} ({(ev.IsAllowed ? "allowed" : "denied")})");
        }

        private void Scp096_AddingTarget(Exiled.Events.EventArgs.AddingTargetEventArgs ev)
        {
            RLogger.Log("SCP096 EVENT", "ADD TARGET", $"{this.PTS(ev.Target)} is {this.PTS(ev.Scp096)}'s target ({(ev.IsAllowed ? "allowed" : "denied")})");
        }

        private void Scp079_InteractingTesla(Exiled.Events.EventArgs.InteractingTeslaEventArgs ev)
        {
            RLogger.Log("SCP079 EVENT", "TESLA", $"{this.PTS(ev.Player)} trigered tesla ({(ev.IsAllowed ? "allowed" : "denied")})");
        }

        private void Scp079_TriggeringDoor(Exiled.Events.EventArgs.TriggeringDoorEventArgs ev)
        {
            RLogger.Log("SCP079 EVENT", "DOOR", $"{this.PTS(ev.Player)} interacted with door ({(ev.IsAllowed ? "allowed" : "denied")})");
        }

        private void Scp914_Activating(Exiled.Events.EventArgs.ActivatingEventArgs ev)
        {
            RLogger.Log("SCP914 EVENT", "ACTIVATION", $"{this.PTS(ev.Player)} activated 914 ({(ev.IsAllowed ? "allowed" : "denied")})");
        }

        private void Player_ReceivingEffect(Exiled.Events.EventArgs.ReceivingEffectEventArgs ev)
        {
            RLogger.Log("GAME EVENT", "RECIVE EFFECT", $"Updated status of {ev.Effect.GetType().Name} for {this.PTS(ev.Player)} from {ev.CurrentState} to {ev.State}, duration: {ev.Duration}s ({(ev.IsAllowed ? "allowed" : "denied")})");
        }

        private void Player_ActivatingWarheadPanel(Exiled.Events.EventArgs.ActivatingWarheadPanelEventArgs ev)
        {
            RLogger.Log("WARHEAD EVENT", "PANEL", $"{this.PTS(ev.Player)} unlocked button ({(ev.IsAllowed ? "allowed" : "denied")})");
        }

        private void Player_IntercomSpeaking(Exiled.Events.EventArgs.IntercomSpeakingEventArgs ev)
        {
            RLogger.Log("GAME EVENT", "INTERCOM", $"{this.PTS(ev.Player)} is using intercom ({(ev.IsAllowed ? "allowed" : "disallowed")})");
        }

        private void Player_RemovingHandcuffs(Exiled.Events.EventArgs.RemovingHandcuffsEventArgs ev)
        {
            RLogger.Log("GAME EVENT", "HANDCUFF", $"{this.PTS(ev.Target)} was uncuffed, cuffer was {this.PTS(ev.Cuffer)} ({(ev.IsAllowed ? "allowed" : "denied")})");
        }

        private void Player_ThrowingGrenade(Exiled.Events.EventArgs.ThrowingGrenadeEventArgs ev)
        {
            RLogger.Log("GAME EVENT", "GRENADE", $"{this.PTS(ev.Player)} thrown {ev.Type} {(ev.IsSlow ? "slowly " : string.Empty)}with fuse {ev.FuseTime} ({(ev.IsAllowed ? "allowed" : "denied")})");
        }

        private void Player_Handcuffing(Exiled.Events.EventArgs.HandcuffingEventArgs ev)
        {
            RLogger.Log("GAME EVENT", "HANDCUFF", $"{this.PTS(ev.Target)} was cuffed, by {this.PTS(ev.Cuffer)} ({(ev.IsAllowed ? "allowed" : "denied")})");
        }

        private void Player_ChangingGroup(Exiled.Events.EventArgs.ChangingGroupEventArgs ev)
        {
            RLogger.Log("ADMIN EVENT", "CHANGE GROUP", $"{this.PTS(ev.Player)}'s group was changed to {ev.NewGroup?.BadgeText} ({(ev.IsAllowed ? "allowed" : "denied")})");
        }

        private void Player_ChangingRole(Exiled.Events.EventArgs.ChangingRoleEventArgs ev)
        {
            RLogger.Log("GAME EVENT", "CLASS CHANGE", $"{this.PTS(ev.Player)} changed role from {ev.Player.Role} to {ev.NewRole} | Escape: {ev.IsEscaped}");
        }

        private void Player_Escaping(Exiled.Events.EventArgs.EscapingEventArgs ev)
        {
            RLogger.Log("GAME EVENT", "ESCAPE", $"{this.PTS(ev.Player)} escaped ({(ev.IsAllowed ? "allowed" : "denied")})");
        }

        private void CustomEvents_OnFirstTimeJoined(Events.EventArgs.FirstTimeJoinedEventArgs ev)
        {
            RLogger.Log("NETWORK EVENT", "PLAYER JOINED", $"{this.PTS(ev.Player)} joined first time");
        }

        private void Player_Destroying(Exiled.Events.EventArgs.DestroyingEventArgs ev)
        {
            RLogger.Log("GAME EVENT", "PLAYER DESTROYED", $"{this.PTS(ev.Player)} was destroyed");
        }

        private void Player_Left(Exiled.Events.EventArgs.LeftEventArgs ev)
        {
            RLogger.Log("NETWORK EVENT", "PLAYER LEFT", $"{this.PTS(ev.Player)} left");
        }

        private void Player_Verified(Exiled.Events.EventArgs.VerifiedEventArgs ev)
        {
            RLogger.Log("NETWORK EVENT", "PLAYER VERIFIED", $"{this.PTS(ev.Player)} was verified");
        }

        private void Player_PreAuthenticating(Exiled.Events.EventArgs.PreAuthenticatingEventArgs ev)
        {
            RLogger.Log("NETWORK EVENT", "PLAYER PREAUTHED", $"Preauthing {ev.UserId} from {ev.Request?.RemoteEndPoint?.Address?.ToString() ?? "NULL"} ({ev.Country}) with flags {ev.Flags}, {(ev.IsAllowed ? "allowed" : "denied")}");
        }

        private void Player_Hurting(Exiled.Events.EventArgs.HurtingEventArgs ev)
        {
            if (ev.Target.IsDead)
                return;
            if (ev.DamageType == DamageTypes.Scp207)
                RLogger.Log("GAME EVENT", "DAMAGE", $"{this.PTS(ev.Target)} was damaged by SCP-207 ({ev.Target.GetEffectIntensity<CustomPlayerEffects.Scp207>()})");
            else if (ev.Target.Id == ev.Attacker?.Id)
                RLogger.Log("GAME EVENT", "DAMAGE", $"{this.PTS(ev.Target)} hurt himself using {ev.DamageType.name}, done {ev.Amount} damage");
            else
                RLogger.Log("GAME EVENT", "DAMAGE", $"{this.PTS(ev.Target)} was hurt by {this.PTS(ev.Attacker) ?? "WORLD"} using {ev.DamageType.name}, done {ev.Amount} damage");
        }

        private void Player_Died(Exiled.Events.EventArgs.DiedEventArgs ev)
        {
            if (ev.Target.Id == ev.Killer?.Id)
                RLogger.Log("GAME EVENT", "SUICIDE", $"{this.PTS(ev.Target)} killed himself using {ev.HitInformations.GetDamageName()}");
            else
                RLogger.Log("GAME EVENT", "KILL", $"{this.PTS(ev.Target)} was killed by {this.PTS(ev.Killer) ?? "WORLD"} using {ev.HitInformations.GetDamageName()}");
        }

        private void Player_Kicked(Exiled.Events.EventArgs.KickedEventArgs ev)
        {
            RLogger.Log("ADMIN EVENT", "KICK", $"{this.PTS(ev.Target)} was kicked with reason {ev.Reason}");
        }

        private void Player_Banned(Exiled.Events.EventArgs.BannedEventArgs ev)
        {
            RLogger.Log("ADMIN EVENT", "BAN", $"{this.PTS(ev.Target)} was banned by {this.PTS(ev.Issuer)} with reason {ev.Details.Reason}");
        }

        private string PTS(Player player) => player.PlayerToString();
    }
}
