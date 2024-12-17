using System;
using Sirenix.OdinInspector;
using UnityEngine;


public class ServerComposer : MonoBehaviour
{
    ServerConfig config;
    public void Awake()
    {
    }

    // Start is called once before the first execution of Update after the MonoBehaviour is created
    void Start()
    {
        
    }

    // Update is called once per frame
    void Update()
    {
        
    }
}
using Sirenix.OdinInspector;
using UnityEngine;
using System;

public class ServerConfig : ScriptableObject
{
    [TitleGroup("Network Settings")]
    [HorizontalGroup("Network Settings/Split", Width = 0.5f)]
    [VerticalGroup("Network Settings/Split/Left")]
    [Sirenix.OdinInspector.LabelWidth(120)]
    public string url = "localhost";

    [VerticalGroup("Network Settings/Split/Right")]
    [Sirenix.OdinInspector.LabelWidth(120)]
    [Range(1, 65535)]
    public ushort port = 3000;

    [TitleGroup("WebSocket Settings")]
    [Sirenix.OdinInspector.LabelWidth(200)]
    [Range(1, 10000)]
    public int maxConnections = 100;

    [Sirenix.OdinInspector.LabelWidth(200)]
    [Range(1, 60)]
    [SuffixLabel("seconds", Overlay = true)]
    public float pingInterval = 25f;

    [Sirenix.OdinInspector.LabelWidth(200)]
    [Range(10, 300)]
    [SuffixLabel("seconds", Overlay = true)]
    public float disconnectTimeout = 60f;

    [Sirenix.OdinInspector.LabelWidth(200)]
    [Range(1024, 1048576)]
    [SuffixLabel("bytes", Overlay = true)]
    public int maxMessageSize = 65536;

    [TitleGroup("Event Bus Settings")]
    [Sirenix.OdinInspector.LabelWidth(200)]
    [Range(100, 100000)]
    public int eventQueueSize = 1000;

    [Sirenix.OdinInspector.LabelWidth(200)]
    [Range(0.01f, 1f)]
    [SuffixLabel("seconds", Overlay = true)]
    public float eventProcessingInterval = 0.1f;

    [Sirenix.OdinInspector.LabelWidth(200)]
    [Range(1, 32)]
    public int eventProcessingThreads = 2;

    [TitleGroup("Database Settings")]
    [Sirenix.OdinInspector.LabelWidth(200)]
    [FilePath(Extensions = "db")]
    public string databasePath = "ServerDatabase.db";

    [Sirenix.OdinInspector.LabelWidth(200)]
    [Range(1000, 1000000)]
    public int databaseCacheSize = 5000;

    [Sirenix.OdinInspector.LabelWidth(200)]
    [Sirenix.OdinInspector.ToggleLeft]
    public bool useJournaling = true;

    [Sirenix.OdinInspector.LabelWidth(200)]
    [Sirenix.OdinInspector.MinMaxSlider(1, 60)]
    [Sirenix.OdinInspector.SuffixLabel("minutes", Overlay = true)]
    public int databaseBackupInterval = 30;

    [Sirenix.OdinInspector.TitleGroup("Logging")]
    [Sirenix.OdinInspector.LabelWidth(200)]
    [Sirenix.OdinInspector.ToggleLeft]
    public bool enableDetailedLogging = false;

    [Sirenix.OdinInspector.LabelWidth(200)]
    [Sirenix.OdinInspector.ShowIf("enableDetailedLogging")]
    [Sirenix.OdinInspector.FilePath(Extensions = "txt")]
    public string logFilePath = "ServerLogs.txt";

    [Sirenix.OdinInspector.LabelWidth(200)]
    [Sirenix.OdinInspector.ShowIf("enableDetailedLogging")]
    [Sirenix.OdinInspector.MinMaxSlider(1, 1000)]
    [Sirenix.OdinInspector.SuffixLabel("MB", Overlay = true)]
    public int maxLogFileSize = 100;

    [Sirenix.OdinInspector.LabelWidth(200)]
    [Sirenix.OdinInspector.ShowIf("enableDetailedLogging")]
    [Sirenix.OdinInspector.MinMaxSlider(1, 100)]
    public int maxLogFileBackups = 5;

    [Sirenix.OdinInspector.TitleGroup("Security")]
    [Sirenix.OdinInspector.LabelWidth(200)]
    [Sirenix.OdinInspector.ToggleLeft]
    public bool useSSL = false;

    [Sirenix.OdinInspector.LabelWidth(200)]
    [Sirenix.OdinInspector.ShowIf("useSSL")]
    [Sirenix.OdinInspector.FilePath(Extensions = "crt,pem")]
    public string sslCertPath = "";

    [Sirenix.OdinInspector.LabelWidth(200)]
    [Sirenix.OdinInspector.ShowIf("useSSL")]
    [Sirenix.OdinInspector.FilePath(Extensions = "key")]
    public string sslKeyPath = "";

    [Sirenix.OdinInspector.LabelWidth(200)]
    [Sirenix.OdinInspector.MinMaxSlider(1, 1000)]
    [Sirenix.OdinInspector.SuffixLabel("requests/minute", Overlay = true)]
    public int maxRequestsPerMinute = 100;

    [Sirenix.OdinInspector.LabelWidth(200)]
    [Sirenix.OdinInspector.ToggleLeft]
    public bool enableIpBanning = true;

    [Sirenix.OdinInspector.TitleGroup("Performance")]
    [Sirenix.OdinInspector.LabelWidth(200)]
    [Sirenix.OdinInspector.MinMaxSlider(1, 64)]
    public int workerThreads = 4;

    [Sirenix.OdinInspector.LabelWidth(200)]
    [Sirenix.OdinInspector.MinMaxSlider(1, 64)]
    public int ioThreads = 4;

    [Sirenix.OdinInspector.LabelWidth(200)]
    [Sirenix.OdinInspector.MinMaxSlider(1, 1000)]
    [Sirenix.OdinInspector.SuffixLabel("MB", Overlay = true)]
    public int maxMemoryUsage = 512;

    [Sirenix.OdinInspector.LabelWidth(200)]
    [Sirenix.OdinInspector.MinMaxSlider(1, 100)]
    [Sirenix.OdinInspector.SuffixLabel("%", Overlay = true)]
    public int cpuUsageThreshold = 80;

    [Sirenix.OdinInspector.TitleGroup("Game Settings")]
    [Sirenix.OdinInspector.LabelWidth(200)]
    [Sirenix.OdinInspector.MinMaxSlider(1, 1000)]
    public int maxPlayersPerGame = 4;

    [Sirenix.OdinInspector.LabelWidth(200)]
    [Sirenix.OdinInspector.MinMaxSlider(10, 3600)]
    [Sirenix.OdinInspector.SuffixLabel("seconds", Overlay = true)]
    public int gameSessionTimeout = 600;

    [Sirenix.OdinInspector.LabelWidth(200)]
    [Sirenix.OdinInspector.MinMaxSlider(1, 100)]
    public int maxConcurrentGames = 10;

    [Sirenix.OdinInspector.LabelWidth(200)]
    [Sirenix.OdinInspector.ToggleLeft]
    public bool enableMatchmaking = true;

    [Sirenix.OdinInspector.LabelWidth(200)]
    [Sirenix.OdinInspector.ShowIf("enableMatchmaking")]
    [Sirenix.OdinInspector.MinMaxSlider(10, 300)]
    [Sirenix.OdinInspector.SuffixLabel("seconds", Overlay = true)]
    public int matchmakingTimeout = 60;

    [Sirenix.OdinInspector.TitleGroup("Maintenance")]
    [Sirenix.OdinInspector.LabelWidth(200)]
    [Sirenix.OdinInspector.ToggleLeft]
    public bool enableMaintenanceMode = false;

    [Sirenix.OdinInspector.LabelWidth(200)]
    [Sirenix.OdinInspector.ShowIf("enableMaintenanceMode")]
    [Sirenix.OdinInspector.MultiLineProperty(3)]
    public string maintenanceMessage = "Server is under maintenance. Please try again later.";

    [Sirenix.OdinInspector.LabelWidth(200)]
    [Sirenix.OdinInspector.MinMaxSlider(1, 168)]
    [Sirenix.OdinInspector.SuffixLabel("hours", Overlay = true)]
    public int serverRestartInterval = 24;

    [Sirenix.OdinInspector.TitleGroup("Monitoring")]
    [Sirenix.OdinInspector.LabelWidth(200)]
    [Sirenix.OdinInspector.ToggleLeft]
    public bool enableHealthChecks = true;

    [Sirenix.OdinInspector.LabelWidth(200)]
    [Sirenix.OdinInspector.ShowIf("enableHealthChecks")]
    [Sirenix.OdinInspector.MinMaxSlider(1, 60)]
    [Sirenix.OdinInspector.SuffixLabel("minutes", Overlay = true)]
    public int healthCheckInterval = 5;

    [Sirenix.OdinInspector.LabelWidth(200)]
    [Sirenix.OdinInspector.ToggleLeft]
    public bool enableMetricsCollection = true;

    [Sirenix.OdinInspector.LabelWidth(200)]
    [Sirenix.OdinInspector.ShowIf("enableMetricsCollection")]
    [Sirenix.OdinInspector.MinMaxSlider(1, 60)]
    [Sirenix.OdinInspector.SuffixLabel("seconds", Overlay = true)]
    public int metricsCollectionInterval = 30;
}

