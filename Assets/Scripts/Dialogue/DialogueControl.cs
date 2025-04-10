using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class DialogueControl : MonoBehaviour
{
    [System.Serializable]
    public enum idiom
    {
        pt,
        eng,
        spa
    }

    public idiom language;

    [Header("Components")]
    public GameObject dialogueObj; //janela do dialogo
    public GameObject inventoryUI; //UI do Inventario
    public Image profiileSprite; //sprite do perfil
    public Text speechText; //texto da fala
    public Text acorNameText; //nome do npc
    [SerializeField] Player _player;

    [Header("Settings")]
    public float typingSpeed; //velocidade da fala

    //Variáveis de controle
    private bool isShowing; //se a janela está visivel
    private int index; //index das sentenças
    private string[] sentences;

    public static DialogueControl instance;

    public bool IsShowing { get => isShowing; set => isShowing = value; }

    //awake é chamado antes de todos os Start() na hierarquia de execução de scripts
    private void Awake()
    {
        instance = this;
    }
    void Start()
    {
        
    }

    void Update()
    {
        
    }

    IEnumerator TypeSentence()
    {
        foreach (char letter in sentences[index].ToCharArray())
        {
            speechText.text += letter;
            yield return new WaitForSeconds(typingSpeed);
        }
    }

    //pular pra prox fala
    public void NextSentence()
    {
        if(speechText.text == sentences[index])
        {
            if (index < sentences.Length - 1)
            {
                index++;
                speechText.text = "";
                StartCoroutine(TypeSentence());
            }
            else
            {
                TurnOffDialogue();
            }
        }
    }

    //chamar a fala do npc
    public void Sepeech(string[] txt)
    {
        if(!isShowing)
        {
            inventoryUI.SetActive(false);
            dialogueObj.SetActive(true);
            sentences = txt;
            StartCoroutine(TypeSentence());
            isShowing = true;
        }
    }

    public void TurnOffDialogue()
    {
        speechText.text = "";
        index = 0;
        sentences = null;
        dialogueObj.SetActive(false);
        isShowing = false;
        if (!_player.IsInBed) {
            inventoryUI.SetActive(true);
        }
    }
}
