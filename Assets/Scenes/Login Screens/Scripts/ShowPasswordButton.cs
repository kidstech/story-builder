using UnityEngine;
using UnityEngine.UI;

public class ShowPasswordButton : MonoBehaviour
{
    public GameObject PasswordInputField;
    private bool passwordHidden = true;
    public void TogglePasswordView()
    {
        // if we are already hiding password, reveal it
        if (passwordHidden)
        {
            PasswordInputField.GetComponent<InputField>().contentType = InputField.ContentType.Standard;
            // force the field to notice the changed content type (otherwise it won't update until the field is clicked on)
            PasswordInputField.GetComponent<InputField>().ForceLabelUpdate();
            passwordHidden = false;
            // change button description for next press
            this.gameObject.GetComponentInChildren<Text>().text = "Hide Password";
        }
        // otherwise hide it
        else
        {
            PasswordInputField.GetComponent<InputField>().contentType = InputField.ContentType.Password;
            PasswordInputField.GetComponent<InputField>().ForceLabelUpdate();
            passwordHidden = true;
            this.gameObject.GetComponentInChildren<Text>().text = "Show Password";
        }
    }

}
