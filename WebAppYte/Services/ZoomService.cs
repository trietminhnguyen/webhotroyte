//using System;
//using System.Threading.Tasks;
//using ZoomNet;
//using ZoomNet.Models;

//public class ZoomService
//{
//    private readonly IZoomClient _zoomClient;

//    public ZoomService(string apiKey, string apiSecret)
//    {
//        _zoomClient = new ZoomClient(apiKey, apiSecret);
//    }

//    public async Task<string> CreateMeetingAsync(string hostEmail, string topic, DateTime startTime, int duration = 30)
//    {
//        var meeting = new Meeting
//        {
//            Topic = topic,
//            Type = MeetingType.Scheduled,
//            StartDate = startTime.ToString("yyyy-MM-ddTHH:mm:ss"),
//            Duration = duration,
//            HostEmail = hostEmail,
//            Settings = new MeetingSettings
//            {
//                HostVideo = true,
//                ParticipantVideo = true,
//                JoinBeforeHost = true,
//                WaitingRoom = false
//            }
//        };

//        var createdMeeting = await _zoomClient.Meetings.CreateAsync(meeting);
//        return createdMeeting.JoinUrl;
//    }
//}
