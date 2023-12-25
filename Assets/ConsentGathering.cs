
using GoogleMobileAds.Ump.Api;
using UnityEngine;
using UnityEngine.UI;

public class ConsentGathering : MonoBehaviour
{
    [SerializeField] private Text canRequestAds;
    [SerializeField] private Text isFormNull;
   ConsentForm _consentForm;
    // Start is called before the first frame update
    void Start()
    {
        // var debugSettings = new ConsentDebugSettings
        // {
        //     // Geography appears as in EEA for debug devices.
        //     DebugGeography = DebugGeography.NotEEA,
        //     TestDeviceHashedIds = new List<string>
        //     {
        //         "C2F913EE6A8C2DC8371A2A32F4CDF27C"
        //     }
        // };

        // Here false means users are not under age.
        ConsentRequestParameters request = new ConsentRequestParameters
        {
            TagForUnderAgeOfConsent = false,
            // ConsentDebugSettings = debugSettings,
        };
// Debug.LogError($"debug setting {debugSettings.TestDeviceHashedIds[0]}-- request TagForUnderAgeOfConsent {request.TagForUnderAgeOfConsent}");
        // Check the current consent information status.
        ConsentInformation.Update(request, OnConsentInfoUpdated);
    }

    void OnConsentInfoUpdated(FormError error)
    {
        if (error != null)
        {
            // Handle the error.
            UnityEngine.Debug.LogError(error);
            SetCanRequestAdsText($"can request ads {ConsentInformation.CanRequestAds()}");
            Debug.Log($"load consent form get this error {error}");
            return;
        }

        error =null;
        Debug.LogError($"IsConsentFormAvailable {ConsentInformation.IsConsentFormAvailable()}");

        if (ConsentInformation.IsConsentFormAvailable())
        {
            SetCanRequestAdsText($"can request ads {ConsentInformation.CanRequestAds()}");
            LoadConsentForm();
        }
        // If the error is null, the consent information state was updated.
        // You are now ready to check if a form is available.
    }

    private void SetCanRequestAdsText(string set)
    {
        canRequestAds.text = set;
    }
    private void SetIsFormNullText(string set)
    {
        isFormNull.text = set;
    }
   public void LoadConsentForm()
    {
        // Loads a consent form.
        ConsentForm.Load(OnLoadConsentForm);
    }

    void OnLoadConsentForm(ConsentForm consentForm, FormError error)
    {
        if (error != null)
        {
            // Handle the error.
            UnityEngine.Debug.LogError(error);
            return;
        }

        if (consentForm == null)
        {
            SetIsFormNullText($"consenform null");
        }
        else
        {
            SetIsFormNullText($"consenform valid");
        }


        // The consent form was loaded.
        // Save the consent form for future requests.
        _consentForm = consentForm;
        Debug.LogError($"is consent form null {_consentForm==null}");

        // You are now ready to show the form.
        if (ConsentInformation.ConsentStatus == ConsentStatus.Required)
        {
            _consentForm.Show(OnShowForm);
        }
    }


    void OnShowForm(FormError error)
    {
        if (error != null)
        {
            // Handle the error.
            UnityEngine.Debug.LogError(error);
            return;
        }

        // Handle dismissal by reloading form.
        LoadConsentForm();
    }

    // Update is called once per frame
    public void Reset()
    {
        ConsentInformation.Reset();
    }
}
