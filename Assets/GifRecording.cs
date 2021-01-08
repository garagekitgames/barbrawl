//using System.Collections;
//using System.Collections.Generic;
//using UnityEngine;
//using EasyMobile;
//using UnityEngine.UI;
//using SO;
//using UnityEngine.Events;

//public class GifRecording : MonoBehaviour
//{
//    [Header("GIF Settings")]
//    // Drag the camera with the Recorder component to this field in the inspector
//    public Recorder recorder;

//    // The recorded clip
//    AnimatedClip myClip;


//    // Suppose you've created a ClipPlayerUI object (ClipPlayer will also work)
//    // Drag the pre-created clip player to this field in the inspector
//    public ClipPlayerUI clipPlayer;

//    public string giphyURL;

//    public string gifFilename = "easy_mobile_demo";
//    public int loop = 0;
//    [Range(1, 100)]
//    public int quality = 90;
//    public System.Threading.ThreadPriority exportThreadPriority;


//    [Header("Giphy Upload Key")]
//    public string giphyUsername;
//    public string giphyApiKey;


//    public string sharePath;

//    //public StringVariable activityText;
//    public StringVariable progressText;
//    // Start is called before the first frame update

//    public UnityEvent OnExportComplete;
//    public void StartGame()
//    {
//        if (myClip != null)
//            myClip.Dispose();

//        Gif.StartRecording(recorder);
//    }

//    public void GameOver()
//    {
//        // Stop recording


//        // Do other stuff...
//        StartCoroutine(stopRecording(0.3f));
        

       

//    }

//    public IEnumerator stopRecording(float sec)
//    {
//        yield return new WaitForSeconds(sec);
//        myClip = Gif.StopRecording(recorder);
//    }

//    public void PlayClip()
//    {
//        PlayMyClip();
//    }

//    public void ExportGif()
//    {
//        // Export GIF image from the resulted clip
//        ExportMyGif();
//    }

//    // This method plays the recorded clip on the created player,
//    // with no delay before playing, and loop indefinitely.
//    void PlayMyClip()
//    {
//        Gif.PlayClip(clipPlayer, myClip);
//    }

    

//    // This method plays the recorded clip on the created player,
//    // with a delay of 1 seconds before playing, and loop indefinitely,
//    // (you can set loop = false to play the clip only once)
//    void PlayMyClipWithDelay()
//    {
//        Gif.PlayClip(clipPlayer, myClip, 1f, true);
//    }

//    // This method pauses the player.
//    void PausePlayer()
//    {
//        Gif.PausePlayer(clipPlayer);
//    }

//    // This method un-pauses the player.
//    void UnPausePlayer()
//    {
//        Gif.ResumePlayer(clipPlayer);
//    }

//    // This method stops the player.
//    void StopPlayer()
//    {
//        Gif.StopPlayer(clipPlayer);
//    }

//    // This method exports a GIF image from the recorded clip.
//    void ExportMyGif()
//    {
//        // Parameter setup
//        string filename = "myGif";    // filename, no need the ".gif" extension
//        int loop = 0;                 // -1: no loop, 0: loop indefinitely, >0: loop a set number of times
//        int quality = 80;             // 80 is a good value in terms of time-quality balance
//        System.Threading.ThreadPriority tPriority = System.Threading.ThreadPriority.Normal; // exporting thread priority

//        Gif.ExportGif(myClip,
//                    filename,
//                    loop,
//                    quality,
//                    tPriority,
//                    OnGifExportProgress,
//                    OnGifExportCompleted);
//    }


//    // This callback is called repeatedly during the GIF exporting process.
//    // It receives a reference to original clip and a progress value ranging from 0 to 1.
//    void OnGifExportProgress(AnimatedClip clip, float progress)
//    {
//        Debug.Log(string.Format("Export progress: {0:P0}", progress));

//        //activityText.value = "GENERATING GIF...";
//        progressText.value = string.Format("{0:P0}", progress);
//    }
//    void OnDestroy()
//    {
//        // Dispose the used clip if needed
//        if (myClip != null)
//            myClip.Dispose();
//    }

//    // This callback is called once the GIF exporting has completed.
//    // It receives a reference to the original clip and the filepath of the generated image.
//    void OnGifExportCompleted(AnimatedClip clip, string path)
//    {
//        Debug.Log("A GIF image has been created at " + path);

//        OnExportComplete.Invoke();
//        // We've done using the clip, dispose it to save memory
//        /*if (clip == myClip)
//        {
//            myClip.Dispose();
//            myClip = null;
//        }*/

//        // The GIF image has been created, now we'll upload it to Giphy
//        // First prepare the upload content
//        var content = new GiphyUploadParams();
//        content.localImagePath = path;    // the file path of the generated GIF image
//        content.tags = "easy mobile, sglib games, unity";    // optional image tags, comma-delimited
//        content.sourcePostUrl = "YOUR_WEBSITE_ADDRESS";    // optional image source, e.g. your website
//        content.isHidden = false;    // optional hidden flag, set to true to mark the image as private            

//        sharePath = path;
//        //Native Share
        
//        // Upload the image to Giphy using the public beta key
//        //UploadToGiphyWithBetaKey(content);

//    }

//    public void OnShareClick()
//    {
//        //new NativeShare().AddFile(sharePath).SetSubject("Brutal Beatdown !").SetText("https://play.google.com/store/apps/details?id=com.garagekitgames.brutalbrawl").Share();

//        Sharing.ShareImage(sharePath, "https://play.google.com/store/apps/details?id=com.garagekitgames.brutalbrawl", "Brutal Beatdown !");
//    }


//    // This method uploads a GIF image to Giphy using the public beta key,
//    // no need to specify any username or API key here.
//    void UploadToGiphyWithBetaKey(GiphyUploadParams content)
//    {
//        Giphy.Upload(content, OnGiphyUploadProgress, OnGiphyUploadCompleted, OnGiphyUploadFailed);
//    }

//    // This method uploads a GIF image to your own Giphy channel,
//    // using your channel username and production key.
//    void UploadToGiphyWithProductionKey(GiphyUploadParams content)
//    {
//        Giphy.Upload("YOUR_CHANNEL_USERNAME", "YOUR_PRODUCTION_KEY",
//                        content,
//                        OnGiphyUploadProgress,
//                        OnGiphyUploadCompleted,
//                        OnGiphyUploadFailed);
//    }

//    // This callback is called repeatedly during the uploading process.
//    // It receives a progress value ranging from 0 to 1.
//    void OnGiphyUploadProgress(float progress)
//    {
//        Debug.Log(string.Format("Upload progress: {0:P0}", progress));
//    }

//    // This callback is called once the uploading has completed.
//    // It receives the URL of the uploaded image.
//    void OnGiphyUploadCompleted(string url)
//    {
//        Debug.Log("The GIF image has been uploaded successfully to Giphy at " + url);

//        // Store the URL into our global variable
//        giphyURL = url;

//#if UNITY_EDITOR
//        bool shouldOpen = UnityEditor.EditorUtility.DisplayDialog("Upload Completed", "The GIF image has been uploaded to Giphy at " + url + ". Open it in the browser?", "Yes", "No");
//        if (shouldOpen)
//            Application.OpenURL(giphyURL);
//#else
//            NativeUI.AlertPopup alert = NativeUI.ShowTwoButtonAlert("Upload Completed", "The GIF image has been uploaded to Giphy at " + url + ". Open it in the browser?", "Yes", "No");

//            if (alert != null)
//            {
//                alert.OnComplete += (int buttonId) =>
//                {
//                    if (buttonId == 0)
//                        Application.OpenURL(giphyURL);
//                };
//            }
//#endif

//    }

//    // This callback is called if the upload has failed.
//    // It receives the error message.
//    void OnGiphyUploadFailed(string error)
//    {
//        Debug.Log("Uploading to Giphy has failed with error: " + error);
//    }

//    // This method shares the URL using the native sharing utility on iOS and Android
//    public void ShareGiphyURL()
//    {
//        if (!string.IsNullOrEmpty(giphyURL))
//        {
//            Sharing.ShareURL(giphyURL);
//           // MobileNativeShare.ShareURL(giphyURL);
//        }
//    }

//    // Update is called once per frame
//    void Update()
//    {
        
//    }
//}
