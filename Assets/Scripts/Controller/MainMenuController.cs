using System.Collections;
using UnityEngine;
using UnityEngine.UI;
using UnityEngine.SceneManagement;
using TMPro;

public class MainMenuController : MonoBehaviour
{
    [Header("Fade Transition")]
    public GameObject transitionPanel;
    [SerializeField] private float waitTime = 4f;

    [Space]

    [Header("UI interface")]
    public Image playButton;
    public Image quitButton;
    public Image gameTitle;
    [SerializeField] private TMP_Text gameName;
    [SerializeField] private float waitToTransition = 3f;
    [SerializeField] private float lerpTime = 3f;
    private Color currentColor;
    private Color targetColor;

    private void Start()
    {
        currentColor = playButton.color;
        targetColor = currentColor;
    }

    //Change to scene Level 1
    public void Play()
    {
        StartCoroutine(LerpAndTrasitionToPlay());
    }

    IEnumerator LerpAndTrasitionToPlay()
    {
        targetColor.a = 0.0f;

        while (currentColor.a > 0.01f)
        {
            currentColor = Color.Lerp(currentColor, targetColor, lerpTime * Time.deltaTime);
            playButton.color = currentColor;
            quitButton.color = currentColor;
            gameTitle.color = currentColor;
            gameName.color = currentColor;

            yield return null;
        }

        //Make sure all button alpha to 0
        currentColor = targetColor;
        playButton.color = targetColor;
        quitButton.color = targetColor;
        gameTitle.color = targetColor;
        gameName.color = targetColor;

        yield return new WaitForSeconds(waitToTransition);

        if(transitionPanel != null)
        {
            Animator transiAnim = transitionPanel.GetComponent<Animator>();
            transiAnim.SetTrigger("Start");
            yield return new WaitForSeconds(waitTime);
            SceneManager.LoadSceneAsync(SceneManager.GetActiveScene().buildIndex + 1);
            transiAnim.SetTrigger("End");
            transiAnim.SetBool("Completed", true);
        }
    }

    //Quit game
    public void Quit()
    {
        Application.Quit();
    }
}
