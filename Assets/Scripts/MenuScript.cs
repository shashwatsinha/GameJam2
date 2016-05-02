using UnityEngine;
using UnityEngine.UI;
using UnityEngine.SceneManagement;
using System.Collections;

public class MenuScript : MonoBehaviour
{
    public Canvas quitMenu;
    public Canvas creditsScreen;
    public Button play;
    public Button credits;
    public Button exit;
    private Canvas mainMenu;


	// Use this for initialization
	void Start ()
    {
        creditsScreen = creditsScreen.GetComponent<Canvas>();
        quitMenu = quitMenu.GetComponent<Canvas>();
        mainMenu = this.GetComponent<Canvas>();
        play = play.GetComponent<Button>();
        credits = credits.GetComponent<Button>();
        exit = exit.GetComponent<Button>();
        quitMenu.enabled = false;
        creditsScreen.enabled = false;
        
	}

	public void playButtonPressed()
    {
        SceneManager.LoadScene("MainLevel");
    }
	
    public void exitButtonPressed()
    {
        quitMenu.enabled = true;
        credits.enabled = false;
        play.enabled = false;
        exit.enabled = false;

    }

    public void noButtonPressed()
    {
        quitMenu.enabled = false ;
        credits.enabled = true ;
        play.enabled = true;
        exit.enabled = true;
    }

    public void yesButtonPressed()
    {
        Application.Quit();
    }

    public void creditsButtonPressed()
    {
        creditsScreen.enabled = true;
        mainMenu.enabled = false;
       

    }

    void Update()
    {
        if (Input.GetKeyDown(KeyCode.Escape) && creditsScreen.enabled )
        {
            creditsScreen.enabled = false;
            mainMenu.enabled = true;
        }
    }



}

