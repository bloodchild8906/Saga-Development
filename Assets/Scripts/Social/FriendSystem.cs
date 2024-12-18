using Sirenix.OdinInspector;
using System;
using System.Collections.Generic;
using System.Threading.Tasks;
using UnityEngine;

namespace TcgEngine.Social
{
    public class FriendSystem : SerializedMonoBehaviour
    {
        private static FriendSystem instance;

        [TabGroup("Settings")]
        [SerializeField] private float onlineCheckInterval = 60f;
        [SerializeField] private int maxFriends = 100;
        
        [TabGroup("Events")]
        public UnityEvent<FriendData> onFriendAdded;
        public UnityEvent<FriendData> onFriendRemoved;
        public UnityEvent<FriendData> onFriendRequestReceived;
        public UnityEvent<FriendData> onFriendOnlineStatusChanged;

        [TabGroup("Runtime Data"), ReadOnly]
        [ShowInInspector] private Dictionary<string, FriendData> friends 
            = new Dictionary<string, FriendData>();
        [ShowInInspector] private List<FriendRequest> pendingRequests 
            = new List<FriendRequest>();

        public static FriendSystem Instance => instance;

        private void Awake()
        {
            if (instance == null)
            {
                instance = this;
                DontDestroyOnLoad(gameObject);
                StartCoroutine(OnlineStatusCheckRoutine());
            }
            else
            {
                Destroy(gameObject);
            }
        }

        [Button("Send Friend Request")]
        public async Task<bool> SendFriendRequest(string targetUsername)
        {
            if (friends.Count >= maxFriends)
                return false;

            var request = new FriendRequest
            {
                FromUsername = Authenticator.Get().Username,
                ToUsername = targetUsername,
                RequestTime = DateTime.UtcNow
            };

            bool success = await DatabaseService.Instance
                .GetCollection<FriendRequest>("friend_requests")
                .InsertAsync(request);

            if (success)
            {
                pendingRequests.Add(request);
                await ActivityService.Instance.LogActivity("friend_request_sent", 
                    request.FromUsername, new { target = targetUsername });
            }

            return success;
        }

        [Button("Accept Friend Request")]
        public async Task<bool> AcceptFriendRequest(string requestId)
        {
            var request = pendingRequests.Find(r => r.Id == requestId);
            if (request == null)
                return false;

            var friendData = new FriendData
            {
                Username = request.FromUsername,
                FriendSince = DateTime.UtcNow,
                LastOnline = DateTime.UtcNow,
                Status = FriendStatus.Online
            };

            bool success = await DatabaseService.Instance
                .GetCollection<FriendData>("friends")
                .InsertAsync(friendData);

            if (success)
            {
                friends[friendData.Username] = friendData;
                pendingRequests.Remove(request);
                onFriendAdded?.Invoke(friendData);

                await ActivityService.Instance.LogActivity("friend_request_accepted", 
                    request.ToUsername, new { friend = request.FromUsername });
            }

            return success;
        }
    }

    [System.Serializable]
    public class FriendData
    {
        [HorizontalGroup("Friend")]
        [PreviewField(45)]
        public string AvatarUrl;

        [VerticalGroup("Friend/Details")]
        public string Username;
        
        [VerticalGroup("Friend/Details")]
        public DateTime FriendSince;
        
        [VerticalGroup("Friend/Status")]
        public DateTime LastOnline;
        
        [VerticalGroup("Friend/Status")]
        public FriendStatus Status;
    }

    public enum FriendStatus
    {
        Offline,
        Online,
        InGame,
        Away
    }
} 