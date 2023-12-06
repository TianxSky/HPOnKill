using System.Text.Json.Serialization;
using CounterStrikeSharp.API;
using CounterStrikeSharp.API.Core;
using CounterStrikeSharp.API.Core.Attributes.Registration;
using CounterStrikeSharp.API.Modules.Utils;

namespace HPOnKill {
    public partial class HPOnKill : BasePlugin
    {
        public override string ModuleName => "HPOnKill";
        public override string ModuleAuthor => "Tian";
        public override string ModuleDescription => "Simple Gain HP on Kill";
        public override string ModuleVersion => "1.0";

        public HPOnKillConfig Config { get; set; } = new();

        public class HPOnKillConfig : BasePluginConfig
        {
            [JsonPropertyName("Prefix")]
            public string Prefix { get; set; } = "[HPOnKill]";

            [JsonPropertyName("GainHP")]
            public bool GainHP { get; set; } = true;

            [JsonPropertyName("GainHPAmount")]
            public int GainHPAmount { get; set; } = 10;
        }
        public void OnConfigParsed(HPOnKillConfig config)
        {
            Config = config;
        }

        public override void Load(bool hotReload)
        {
            Console.WriteLine("[HPOnKill] -> Plugin loaded");
        }
        public override void Unload(bool hotReload)
        {
            Console.WriteLine("[HPOnKill] -> Plugin  unloaded");
        }

        [GameEventHandler]
        public HookResult OnPlayerDeath(EventPlayerDeath @event, GameEventInfo info)
        {
            CCSPlayerController player = @event.Userid;
            CCSPlayerController attacker = @event.Attacker;
            if (player == null || !player.IsValid || attacker == null || !attacker.IsValid) return HookResult.Continue;
            if (player == attacker) return HookResult.Continue;
            if (Config.GainHP)
            {
                attacker.PlayerPawn.Value.Health = attacker.PlayerPawn.Value.Health + Config.GainHPAmount;
                Server.PrintToChatAll($"{Config.Prefix}{ChatColors.Red}{attacker.PlayerName} gained {ChatColors.Green}+{Config.GainHPAmount} HP{ChatColors.Default} for killing {ChatColors.LightRed}{player.PlayerName}{ChatColors.Default}.");
            }
            return HookResult.Continue;
        }
    }
}