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

public class ServerConfig : ScriptableObject
{
    [TitleGroup("Network Settings")]
    [HorizontalGroup("Network Settings/Split", Width = 0.5f)]
    [VerticalGroup("Network Settings/Split/Left")]
    [LabelWidth(120)]
    public string url = "localhost";

    [VerticalGroup("Network Settings/Split/Right")]
    [LabelWidth(120)]
    [Range(1, 65535)]
    public ushort port = 3000;

    [TitleGroup("WebSocket Settings")]
    [LabelWidth(200)]
    [Range(1, 10000)]
    public int maxConnections = 100;

    [LabelWidth(200)]
    [Range(1, 60)]
    [SuffixLabel("seconds", Overlay = true)]
    public float pingInterval = 25f;

    [LabelWidth(200)]
    [Range(10, 300)]
    [SuffixLabel("seconds", Overlay = true)]
    public float disconnectTimeout = 60f;

    [LabelWidth(200)]
    [Range(1024, 1048576)]
    [SuffixLabel("bytes", Overlay = true)]
    public int maxMessageSize = 65536;

    [TitleGroup("Event Bus Settings")]
    [LabelWidth(200)]
    [Range(100, 100000)]
    public int eventQueueSize = 1000;

    [LabelWidth(200)]
    [Range(0.01f, 1f)]
    [SuffixLabel("seconds", Overlay = true)]
    public float eventProcessingInterval = 0.1f;

    [LabelWidth(200)]
    [Range(1, 32)]
    public int eventProcessingThreads = 2;

    [TitleGroup("Database Settings")]
    [LabelWidth(200)]
    [FilePath(Extensions = "db")]
    public string databasePath = "ServerDatabase.db";

    [LabelWidth(200)]
    [Range(1000, 1000000)]
    public int databaseCacheSize = 5000;

    [LabelWidth(200)]
    [ToggleLeft]
    public bool useJournaling = true;

    [LabelWidth(200)]
    [MinMaxSlider(1, 60)]
    [SuffixLabel("minutes", Overlay = true)]
    public int databaseBackupInterval = 30;

    [TitleGroup("Logging")]
    [LabelWidth(200)]
    [ToggleLeft]
    public bool enableDetailedLogging = false;

    [LabelWidth(200)]
    [ShowIf("enableDetailedLogging")]
    [FilePath(Extensions = "txt")]
    public string logFilePath = "ServerLogs.txt";

    [LabelWidth(200)]
    [ShowIf("enableDetailedLogging")]
    [MinMaxSlider(1, 1000)]
    [SuffixLabel("MB", Overlay = true)]
    public int maxLogFileSize = 100;

    [LabelWidth(200)]
    [ShowIf("enableDetailedLogging")]
    [MinMaxSlider(1, 100)]
    public int maxLogFileBackups = 5;

    [TitleGroup("Security")]
    [LabelWidth(200)]
    [ToggleLeft]
    public bool useSSL = false;

    [LabelWidth(200)]
    [ShowIf("useSSL")]
    [FilePath(Extensions = "crt,pem")]
    public string sslCertPath = "";

    [LabelWidth(200)]
    [ShowIf("useSSL")]
    [FilePath(Extensions = "key")]
    public string sslKeyPath = "";

    [LabelWidth(200)]
    [MinMaxSlider(1, 1000)]
    [SuffixLabel("requests/minute", Overlay = true)]
    public int maxRequestsPerMinute = 100;

    [LabelWidth(200)]
    [ToggleLeft]
    public bool enableIpBanning = true;

    [TitleGroup("Performance")]
    [LabelWidth(200)]
    [MinMaxSlider(1, 64)]
    public int workerThreads = 4;

    [LabelWidth(200)]
    [MinMaxSlider(1, 64)]
    public int ioThreads = 4;

    [LabelWidth(200)]
    [MinMaxSlider(1, 1000)]
    [SuffixLabel("MB", Overlay = true)]
    public int maxMemoryUsage = 512;

    [LabelWidth(200)]
    [MinMaxSlider(1, 100)]
    [SuffixLabel("%", Overlay = true)]
    public int cpuUsageThreshold = 80;

    [TitleGroup("Game Settings")]
    [LabelWidth(200)]
    [MinMaxSlider(1, 1000)]
    public int maxPlayersPerGame = 4;

    [LabelWidth(200)]
    [MinMaxSlider(10, 3600)]
    [SuffixLabel("seconds", Overlay = true)]
    public int gameSessionTimeout = 600;

    [LabelWidth(200)]
    [MinMaxSlider(1, 100)]
    public int maxConcurrentGames = 10;

    [LabelWidth(200)]
    [ToggleLeft]
    public bool enableMatchmaking = true;

    [LabelWidth(200)]
    [ShowIf("enableMatchmaking")]
    [MinMaxSlider(10, 300)]
    [SuffixLabel("seconds", Overlay = true)]
    public int matchmakingTimeout = 60;

    [TitleGroup("Maintenance")]
    [LabelWidth(200)]
    [ToggleLeft]
    public bool enableMaintenanceMode = false;

    [LabelWidth(200)]
    [ShowIf("enableMaintenanceMode")]
    [MultiLineProperty(3)]
    public string maintenanceMessage = "Server is under maintenance. Please try again later.";

    [LabelWidth(200)]
    [MinMaxSlider(1, 168)]
    [SuffixLabel("hours", Overlay = true)]
    public int serverRestartInterval = 24;

    [TitleGroup("Monitoring")]
    [LabelWidth(200)]
    [ToggleLeft]
    public bool enableHealthChecks = true;

    [LabelWidth(200)]
    [ShowIf("enableHealthChecks")]
    [MinMaxSlider(1, 60)]
    [SuffixLabel("minutes", Overlay = true)]
    public int healthCheckInterval = 5;

    [LabelWidth(200)]
    [ToggleLeft]
    public bool enableMetricsCollection = true;

    [LabelWidth(200)]
    [ShowIf("enableMetricsCollection")]
    [MinMaxSlider(1, 60)]
    [SuffixLabel("seconds", Overlay = true)]
    public int metricsCollectionInterval = 30;
}

