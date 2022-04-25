using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using UnityEngine.Networking;

public class login : MonoBehaviour
{
    public InputField usernameField;
    public InputField passwordField;

    public Button submitButton;

    public void CallLogin()
    {
        StartCoroutine(Login());
    }

    public void GoToRegister()
    {
        UnityEngine.SceneManagement.SceneManager.LoadScene(1);
    }

    IEnumerator Login()
    {
        WWWForm form = new WWWForm();
        form.AddField("username", usernameField.text);
        form.AddField("password", passwordField.text);
        WWW www = new WWW("http://" + DBManager.ip + "/sqlconnect/login.php", form);
        yield return www;
        if ((www.text).Length < 8)
        {
            DBManager.username = usernameField.text;
            DBManager.userId = www.text;
            Debug.Log("User created successfully " + DBManager.userId);
            //Debug.Log((www.text).GetType() == typeof(System.String));
            UnityEngine.SceneManagement.SceneManager.LoadScene(2);
        }
        else
        {
            Debug.Log("User login failed. error #" + www.text);
        }

        /*using (UnityWebRequest www = UnityWebRequest.Post("http://localhost/sqlconnect/login.php", form))
        {
            yield return www.SendWebRequest();

            if (www.isNetworkError || www.isHttpError)
            {
                Debug.Log("User login failed. error #" + www.error);
            }
            else
            {
                DBManager.username = usernameField.text;
                Debug.Log("User login successfully");
                UnityEngine.SceneManagement.SceneManager.LoadScene(2);
            }
        }*/
    }

    public void VerifyInputs()
    {
        submitButton.interactable = (usernameField.text.Length >= 8 && passwordField.text.Length >= 8);
    }
}
