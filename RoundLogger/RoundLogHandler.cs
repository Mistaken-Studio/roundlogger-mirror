// -----------------------------------------------------------------------
// <copyright file="RoundLogHandler.cs" company="Mistaken">
// Copyright (c) Mistaken. All rights reserved.
// </copyright>
// -----------------------------------------------------------------------

using CustomPlayerEffects;
using Exiled.API.Enums;
using Exiled.API.Features;
using Mistaken.API.Diagnostics;
using Mistaken.API.Extensions;

namespace Mistaken.RoundLogger
{
    internal class RoundLogHandler : Module
    {
        /// <inheritdoc cref="Module.Module(Exiled.API.Interfaces.IPlugin{Exiled.API.Interfaces.IConfig})"/>
        public RoundLogHandler(PluginHandler p)
            : base(p)
        {
            RLogger.IniIfNotAlready();
        }

        public override bool IsBasic => true;

        public override string Name => "RoundLog";

        public override void OnEnable()
        {
            this.CallDelayed(
                1.5f,
                () =>
                {
                    Exiled.Events.Handlers.Player.Banned += this.Player_Banned;
                    Exiled.Events.Handlers.Player.Kicked += this.Player_Kicked;
                    Exiled.Events.Handlers.Player.Died += this.Player_Died;
                    Exiled.Events.Handlers.Player.Hurting += this.Player_Hurting;
                    Exiled.Events.Handlers.Player.PreAuthenticating += this.Player_PreAuthenticating;
                    Exiled.Events.Handlers.Player.Verified += this.Player_Verified;
                    Exiled.Events.Handlers.Player.Destroying += this.Player_Destroying;
                    Exiled.Events.Handlers.Player.Left += this.Player_Left;
                    Exiled.Events.Handlers.Player.Escaping += this.Player_Escaping;
                    Exiled.Events.Handlers.Player.ChangingRole += this.Player_ChangingRole;
                    Exiled.Events.Handlers.Player.ChangingGroup += this.Player_ChangingGroup;

                    // Exiled.Events.Handlers.Player.ThrowingGrenade += this.Handle<Exiled.Events.EventArgs.ThrowingGrenadeEventArgs>((ev) => this.Player_ThrowingGrenade(ev));
                    Exiled.Events.Handlers.Player.Handcuffing += this.Player_Handcuffing;
                    Exiled.Events.Handlers.Player.RemovingHandcuffs += this.Player_RemovingHandcuffs;
                    Exiled.Events.Handlers.Player.IntercomSpeaking += this.Player_IntercomSpeaking;
                    Exiled.Events.Handlers.Player.ActivatingWarheadPanel += this.Player_ActivatingWarheadPanel;
                    Exiled.Events.Handlers.Player.ReceivingEffect += this.Player_ReceivingEffect;
                    Exiled.Events.Handlers.Scp914.Activating += this.Scp914_Activating;
                    Exiled.Events.Handlers.Scp079.TriggeringDoor += this.Scp079_TriggeringDoor;
                    Exiled.Events.Handlers.Scp079.InteractingTesla += this.Scp079_InteractingTesla;
                    Exiled.Events.Handlers.Scp096.AddingTarget += this.Scp096_AddingTarget;
                    Exiled.Events.Handlers.Scp096.Enraging += this.Scp096_Enraging;
                    Exiled.Events.Handlers.Warhead.Detonated += this.Warhead_Detonated;
                    Exiled.Events.Handlers.Warhead.Starting += this.Warhead_Starting;
                    Exiled.Events.Handlers.Warhead.Stopping += this.Warhead_Stopping;
                    Exiled.Events.Handlers.Server.RoundEnded += this.Server_RoundEnded;
                    Exiled.Events.Handlers.Server.WaitingForPlayers += this.Server_WaitingForPlayers;
                    Exiled.Events.Handlers.Server.RoundStarted += this.Server_RoundStarted;
                    Exiled.Events.Handlers.Server.RespawningTeam += this.Server_RespawningTeam;
                    Exiled.Events.Handlers.Map.Decontaminating += this.Map_Decontaminating;
                    Exiled.Events.Handlers.Map.GeneratorActivated += this.Map_GeneratorActivated;
                    Exiled.Events.Handlers.Map.ExplodingGrenade += this.Map_ExplodingGrenade;
                    Exiled.Events.Handlers.Player.PickingUpItem += this.Player_PickingUpItem;
                    Exiled.Events.Handlers.Player.DroppingItem += this.Player_DroppingItem;

                    API.AnnonymousEvents.Subscribe("VANISH", this.OnVanishUpdate); // (Player player, byte level)
                },
                "LateEnable");
        }

        public override void OnDisable()
        {
            Exiled.Events.Handlers.Player.Banned -= this.Player_Banned;
            Exiled.Events.Handlers.Player.Kicked -= this.Player_Kicked;
            Exiled.Events.Handlers.Player.Died -= this.Player_Died;
            Exiled.Events.Handlers.Player.Hurting -= this.Player_Hurting;
            Exiled.Events.Handlers.Player.PreAuthenticating -= this.Player_PreAuthenticating;
            Exiled.Events.Handlers.Player.Verified -= this.Player_Verified;
            Exiled.Events.Handlers.Player.Destroying -= this.Player_Destroying;
            Exiled.Events.Handlers.Player.Left -= this.Player_Left;
            Exiled.Events.Handlers.Player.Escaping -= this.Player_Escaping;
            Exiled.Events.Handlers.Player.ChangingRole -= this.Player_ChangingRole;
            Exiled.Events.Handlers.Player.ChangingGroup -= this.Player_ChangingGroup;

            // Exiled.Events.Handlers.Player.ThrowingGrenade -= this.Handle<Exiled.Events.EventArgs.ThrowingGrenadeEventArgs>((ev) => this.Player_ThrowingGrenade(ev));
            Exiled.Events.Handlers.Player.Handcuffing -= this.Player_Handcuffing;
            Exiled.Events.Handlers.Player.RemovingHandcuffs -= this.Player_RemovingHandcuffs;
            Exiled.Events.Handlers.Player.IntercomSpeaking -= this.Player_IntercomSpeaking;
            Exiled.Events.Handlers.Player.ActivatingWarheadPanel -= this.Player_ActivatingWarheadPanel;
            Exiled.Events.Handlers.Player.ReceivingEffect -= this.Player_ReceivingEffect;
            Exiled.Events.Handlers.Scp914.Activating -= this.Scp914_Activating;
            Exiled.Events.Handlers.Scp079.TriggeringDoor -= this.Scp079_TriggeringDoor;
            Exiled.Events.Handlers.Scp079.InteractingTesla -= this.Scp079_InteractingTesla;
            Exiled.Events.Handlers.Scp096.AddingTarget -= this.Scp096_AddingTarget;
            Exiled.Events.Handlers.Scp096.Enraging -= this.Scp096_Enraging;
            Exiled.Events.Handlers.Warhead.Detonated -= this.Warhead_Detonated;
            Exiled.Events.Handlers.Warhead.Starting -= this.Warhead_Starting;
            Exiled.Events.Handlers.Warhead.Stopping -= this.Warhead_Stopping;
            Exiled.Events.Handlers.Server.RoundEnded -= this.Server_RoundEnded;
            Exiled.Events.Handlers.Server.WaitingForPlayers -= this.Server_WaitingForPlayers;
            Exiled.Events.Handlers.Server.RoundStarted -= this.Server_RoundStarted;
            Exiled.Events.Handlers.Server.RespawningTeam -= this.Server_RespawningTeam;
            Exiled.Events.Handlers.Map.Decontaminating -= this.Map_Decontaminating;
            Exiled.Events.Handlers.Map.GeneratorActivated -= this.Map_GeneratorActivated;
            Exiled.Events.Handlers.Map.ExplodingGrenade -= this.Map_ExplodingGrenade;
            Exiled.Events.Handlers.Player.PickingUpItem -= this.Player_PickingUpItem;
            Exiled.Events.Handlers.Player.DroppingItem -= this.Player_DroppingItem;
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

        private void Player_DroppingItem(Exiled.Events.EventArgs.DroppingItemEventArgs ev)
        {
            RLogger.Log("GAME EVENT", "ITEM", $"{ev.Player.PlayerToString()} dropped {ev.Item.Type} with result: {(ev.IsAllowed ? "Allowed" : "Disallowed")}");
        }

        private void Player_PickingUpItem(Exiled.Events.EventArgs.PickingUpItemEventArgs ev)
        {
            RLogger.Log("GAME EVENT", "ITEM", $"{ev.Player.PlayerToString()} picked up {ev.Pickup.Type} with result: {(ev.IsAllowed ? "Allowed" : "Disallowed")}");
        }

        private void Map_GeneratorActivated(Exiled.Events.EventArgs.GeneratorActivatedEventArgs ev)
        {
            RLogger.Log("GAME EVENT", "GENERATOR", $"Generator activated");
        }

        private void Map_ExplodingGrenade(Exiled.Events.EventArgs.ExplodingGrenadeEventArgs ev)
        {
            RLogger.Log("GAME EVENT", "GRENADE", $"{this.PTS(ev.Thrower)}'s {(ev.GrenadeType == GrenadeType.FragGrenade ? "Frag" : "Flash")} exploading on {ev.TargetsToAffect.Count} target ({(ev.IsAllowed ? "allowed" : "denied")})");
        }

        private void Map_Decontaminating(Exiled.Events.EventArgs.DecontaminatingEventArgs ev)
        {
            RLogger.Log("GAME EVENT", "DECONTAMINATION", $"Decontamination finished ({(ev.IsAllowed ? "allowed" : "denied")})");
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
            if (ev.Effect.GetType() == typeof(Stained))
                return;

            // if (ev.Effect.GetType() == typeof(Hypothermia))
            //     return;
            if (ev.CurrentState == 0 && ev.State == 0)
                return;
            RLogger.Log("GAME EVENT", "RECIVE EFFECT", $"Updated status of {ev.Effect.GetType().Name} for {this.PTS(ev.Player)} from {ev.CurrentState} to {ev.State}, duration: {ev.Duration}s ({(ev.IsAllowed ? "allowed" : "denied")})");
        }

        private void Player_ActivatingWarheadPanel(Exiled.Events.EventArgs.ActivatingWarheadPanelEventArgs ev)
        {
            RLogger.Log("WARHEAD EVENT", "PANEL", $"{this.PTS(ev.Player)} unlocked button ({(ev.IsAllowed ? "allowed" : "denied")})");
        }

        private void Player_IntercomSpeaking(Exiled.Events.EventArgs.IntercomSpeakingEventArgs ev)
        {
            if (Round.IsStarted)
                RLogger.Log("GAME EVENT", "INTERCOM", $"{this.PTS(ev.Player)} is using intercom ({(ev.IsAllowed ? "allowed" : "disallowed")})");
        }

        private void Player_RemovingHandcuffs(Exiled.Events.EventArgs.RemovingHandcuffsEventArgs ev)
        {
            RLogger.Log("GAME EVENT", "HANDCUFF", $"{this.PTS(ev.Target)} was uncuffed, cuffer was {this.PTS(ev.Cuffer)} ({(ev.IsAllowed ? "allowed" : "denied")})");
        }

        /*private void Player_ThrowingGrenade(Exiled.Events.EventArgs.ThrowingGrenadeEventArgs ev)
        {
            RLogger.Log("GAME EVENT", "GRENADE", $"{this.PTS(ev.Player)} thrown {ev.Type} {(ev.IsSlow ? "slowly " : string.Empty)}with fuse {ev.FuseTime} ({(ev.IsAllowed ? "allowed" : "denied")})");
        }*/

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
            RLogger.Log("GAME EVENT", "CLASS CHANGE", $"{this.PTS(ev.Player)} changed role from {ev.Player.Role} to {ev.NewRole} | Reason: {ev.Reason}");
        }

        private void Player_Escaping(Exiled.Events.EventArgs.EscapingEventArgs ev)
        {
            RLogger.Log("GAME EVENT", "ESCAPE", $"{this.PTS(ev.Player)} escaped ({(ev.IsAllowed ? "allowed" : "denied")})");
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
            if (ev.Target.IsDead || ev.Target.IsGodModeEnabled)
                return;
            if (ev.Handler.Type == DamageType.Scp207)
                RLogger.Log("GAME EVENT", "DAMAGE", $"{this.PTS(ev.Target)} was damaged by SCP-207 ({ev.Target.GetEffectIntensity<CustomPlayerEffects.Scp207>()}) ({(ev.IsAllowed ? "allowed" : "disallowed")})");
            else if (ev.Target.Id == ev.Attacker?.Id)
            {
                var rd = ev.Target.GetRealDamageAmount((PlayerStatsSystem.StandardDamageHandler)ev.Handler.Base, out var hd, out var ahpd);
                RLogger.Log("GAME EVENT", "DAMAGE", $"{this.PTS(ev.Target)} hurt himself using {ev.Handler.Type}, done damage: {ev.Amount}  real damage: {rd}  HP damage: {hd}  AHP damage: {ahpd} ({(ev.IsAllowed ? "allowed" : "disallowed")})");
            }
            else
            {
                var rd = ev.Target.GetRealDamageAmount((PlayerStatsSystem.StandardDamageHandler)ev.Handler.Base, out var hd, out var ahpd);
                RLogger.Log("GAME EVENT", "DAMAGE", $"{this.PTS(ev.Target)} was hurt by {this.PTS(ev.Attacker) ?? "WORLD"} using {ev.Handler.Type}, done damage: {ev.Amount}  real damage: {rd}  HP damage: {hd}  AHP damage: {ahpd} ({(ev.IsAllowed ? "allowed" : "disallowed")})");
            }
        }

        private void Player_Died(Exiled.Events.EventArgs.DiedEventArgs ev)
        {
            if (ev.Target.Id == ev.Killer?.Id)
                RLogger.Log("GAME EVENT", "SUICIDE", $"{this.PTS(ev.Target)} killed himself using {ev.Handler.Type}");
            else
                RLogger.Log("GAME EVENT", "KILL", $"{this.PTS(ev.Target)} was killed by {this.PTS(ev.Killer) ?? "WORLD"} using {ev.Handler.Type}");
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
