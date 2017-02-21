//using System.Collections;
//using Squid;
//using UnityEngine;

//public class TransmissionView : ITransmissionView
//{
//    private Frame _transmissionFrame;
//    private Label _transmissionText;
//    private ImageControl _transmissionImage;

//    public void ShowTransmission(string text, float duration)
//    {
//        SetText(text);
//        _transmissionFrame.Animation.Stop();
//        _transmissionFrame.Animation.Custom(ExpireAnimation(duration));
//    }

//    private IEnumerator ExpireAnimation(float duration)
//    {
//        float startTime = Time.time;

//        while (Time.time - startTime < duration)
//        {
//            yield return null;
//        }

//        _transmissionFrame.Visible = false;
//    }

//    public void SetText(string text)
//    {
//        _transmissionText.Text = text;
//        if (string.IsNullOrEmpty(text))
//        {
//            _transmissionFrame.Visible = false;
//        }
//        else
//        {
//            _transmissionFrame.Visible = true;
//        }
//    }

//    public void SetImage(string imagePath)
//    {
//        _transmissionImage.Texture = imagePath;
//    }
    
//    public TransmissionView(Frame transmissionFrame)
//    {
//        _transmissionFrame = transmissionFrame;

//        _transmissionText = (Label)_transmissionFrame.GetControl("TransmissionText");
//        _transmissionImage = (ImageControl)_transmissionFrame.GetControl("TransmissionImage");

//        _transmissionText.BBCodeEnabled = true;

//    }


//    public void SetImage(Sprite sprite)
//    {
//        throw new System.NotImplementedException();
//    }
//}
