//using System.Collections;
//using System.Collections.Generic;
//using UnityEngine;
//using EasyMobile;
//using System;

//public class NotificationsManager : UnitySingletonPersistent<NotificationsManager>
//{
//    public bool isInitialized;

//    public int pendingNotificationCount;

//    public override void Awake()
//    {
//        base.Awake();

//        if (!RuntimeManager.IsInitialized())
//            RuntimeManager.Init();


//        Notifications.Init();
//        isInitialized = Notifications.IsInitialized();
//        GetPendingLocalNotifications();
//    }
//    void OnEnable()
//    {
//        Notifications.LocalNotificationOpened += OnLocalNotificationOpened;
        
//    }

//    // Unsubscribes notification events.
//    void OnDisable()
//    {
//        Notifications.LocalNotificationOpened -= OnLocalNotificationOpened;
        
//    }


//    void OnLocalNotificationOpened(EasyMobile.LocalNotification delivered)
//    {
//        // The actionId will be empty if the notification was opened with the default action.
//        // Otherwise it contains the ID of the selected action button.
//        if (!string.IsNullOrEmpty(delivered.actionId))
//        {
//            Debug.Log("Action ID: " + delivered.actionId);
//        }

//        // Whether the notification is delivered when the app is in foreground.
//        Debug.Log("Is app in foreground: " + delivered.isAppInForeground.ToString());

//        // Gets the notification content.
//        NotificationContent content = delivered.content;

//        // Take further actions if needed...

//        // TO DO : 
//        //Give Reward
//    }

//    // Start is called before the first frame update
//    void Start()
//    {
        
//        //if (isInitialized)
//        //{
//            ScheduleRepeatLocalNotification();
//        //}

//    }

//    // Update is called once per frame
//    void Update()
//    {
//        isInitialized = Notifications.IsInitialized();
//    }

//    NotificationContent PrepareNotificationContent()
//    {
//        NotificationContent content = new NotificationContent();

//        // Provide the notification title.
//        content.title = "Please Come Back !";

//        // You can optionally provide the notification subtitle, which is visible on iOS only.
//        content.subtitle = "Demo Subtitle";

//        // Provide the notification message.
//        content.body = "Your enemies are waiting to get punched !";

//        // You can optionally attach custom user information to the notification
//        // in form of a key-value dictionary.
//       /* content.userInfo = new Dictionary<string, object>();
//        content.userInfo.Add("string", "OK");
//        content.userInfo.Add("number", 3);
//        content.userInfo.Add("bool", true);*/

//        // You can optionally assign this notification to a category using the category ID.
//        // If you don't specify any category, the default one will be used.
//        // Note that it's recommended to use the category ID constants from the EM_NotificationsConstants class
//        // if it has been generated before. In this example, UserCategory_notification_category_test is the
//        // generated constant of the category ID "notification.category.test".
//        /*content.categoryId = EM_NotificationsConstants.UserCategory_notification_category_test;*/

//        // If you want to use default small icon and large icon (on Android),
//        // don't set the smallIcon and largeIcon fields of the content.
//        // If you want to use custom icons instead, simply specify their names here (without file extensions).
//       // content.smallIcon = "YOUR_CUSTOM_SMALL_ICON";
//       // content.largeIcon = "YOUR_CUSTOM_LARGE_ICON";

//        return content;
//    }

//    // Schedule a notification to be delivered after 08 hours, 08 minutes and 08 seconds,
//    // then repeat once every day.
//    public void ScheduleRepeatLocalNotification()
//    {
//        // Prepare the notification content (see the above section).
//        //NotificationContent content = PrepareNotificationContent();

//        // Set the delay time as a TimeSpan.
//       // TimeSpan delay = new TimeSpan(00, 00, 30);

//        // Schedule the notification.
//        //Notifications.ScheduleLocalNotification(delay, content, NotificationRepeat.EveryDay);


//        var notif = new NotificationContent();
//        notif.title = "Come Back";
//        notif.body = "Your enemies are waiting for you !";
//        notif.categoryId = "";

//        Notifications.ScheduleLocalNotification(new TimeSpan(10, 00, 00), notif, NotificationRepeat.EveryWeek);
//    }


//    // Gets all pending local notification requests.
//    void GetPendingLocalNotifications()
//    {
//        Notifications.GetPendingLocalNotifications(GetPendingLocalNotificationsCallback);
//    }

//    // Callback.
//    void GetPendingLocalNotificationsCallback(NotificationRequest[] pendingRequests)
//    {
//        pendingNotificationCount = pendingRequests.Length;
//        /*foreach (var request in pendingRequests)
//        {
//            NotificationContent content = request.content;        // notification content

//            Debug.Log("Notification request ID: " + request.id);    // notification request ID
//            Debug.Log("Notification title: " + content.title);
//            Debug.Log("Notification body: " + content.body);
//        }*/
//    }

//    public void RequestRating()
//    {
        
//        if (StoreReview.CanRequestRating())
//        {
//            StoreReview.RequestRating();
//        }
//    }

//}
