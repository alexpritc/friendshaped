using UnityEngine;
using UnityEngine.UI;
using System;
using Ink.Runtime;
using System.Collections;
using System.Collections.Generic;
using TMPro;

// This is a super bare bones example of how to play and display a ink story in Unity.
public class DialogueManager : MonoBehaviour {
    public static event Action<Story> OnCreateStory;
    
    public TextAsset inkJSONAsset = null;
    public Story story;

    [SerializeField]
    private Canvas dialogueCanvas = null;
	[SerializeField]
	private Canvas choiceCanvas = null;
	[SerializeField]
	private Canvas commentaryCanvas = null;

	// UI Prefabs
	[SerializeField]
    private Text textPrefab = null;
	[SerializeField]
	private GameObject textBoxPrefab = null;
	[SerializeField]
    private Button buttonPrefab = null;

    [SerializeField] private GameObject commentaryTextBoxPrefab = null;
	private TextMeshProUGUI commentaryTextPrefab = null;

	[SerializeField] float waitTime = 2f;

	private Animator currentAnim = null;

	List<GameObject> currentDialogueBoxes;

	private bool isCommentary;
	private bool isPlayerTalking;

	private PlayerControls controls;

	private bool clickedButton;

	void Awake () {

		controls = new PlayerControls();
		currentDialogueBoxes = new List<GameObject>();

		// Remove the default message
		RemoveChildren(dialogueCanvas);
		RemoveChildren(choiceCanvas);
		RemoveChildren(commentaryCanvas);
		StartStory();

		controls.interactions.click.performed += ctx => SkipDialogue();
	}

	// Creates a new Story object with the compiled story which we can then play!
	void StartStory () {
		story = new Story (inkJSONAsset.text);
        if(OnCreateStory != null) OnCreateStory(story);

		RefreshView();
	}
	
	// This is the main function called every time the story changes. It does a few things:
	// Destroys all the old content and choices.
	// Continues over all the lines of text, then displays all the choices. If there are no choices, the story is finished!
	void RefreshView () {
		// Remove all the UI on screen
		RemoveChildren(dialogueCanvas);
		RemoveChildren(choiceCanvas);
		RemoveChildren(commentaryCanvas);

		clickedButton = false;

		// Read all the content until we can't continue any more
		StartCoroutine(DisplayNextDialogue());
	}

	// When we click the choice button, tell the story to choose that choice!
	void OnClickChoiceButton (Choice choice) {
		story.ChooseChoiceIndex (choice.index);

		// TODO: if choice is tagged #end close window
		if (choice.text.ToLower().CompareTo("Walk Away".ToLower()) == 0)
		{
			Debug.Log("walked away");
			EndChat();
		}
		
		RefreshView();
	}

	IEnumerator DisplayNextDialogue()
	{
		while (story.canContinue)
		{
			// Read all the content until we can't continue any more

			// Continue gets the next line of the story
			string text = story.Continue();
			// This removes any white space from the text.
			text = text.Trim();
			
			if (story.currentTags.Count != 0)
			{
				isCommentary = story.currentTags.Contains("Commentary") ? true : false;
				isPlayerTalking = story.currentTags.Contains("Player") ? true : false;
			}

			// Display the text on screen!
			if (isCommentary)
			{
				RemoveChildren(commentaryCanvas);
				DisplayCommentary(text);
			}
			else
			{
				CreateContentView(text);
			}

			if (story.canContinue)
			{
				if (isCommentary)
				{
					yield return new WaitForSeconds(waitTime*2f);
					RemoveChildren(commentaryCanvas);
				}
				else
				{
					yield return new WaitForSeconds(waitTime);
				}
			}
			else
			{
				// If there's no more dialogue, don't bother waiting to display the buttons.
				yield return new WaitUntil(() => GetClipName(currentAnim) == "ConstantDialogue" || GetClipName(currentAnim) == "ConstantCommentaryText");
			}
		}

		if (!story.canContinue && !clickedButton)
		{
			clickedButton = true;
			DisplayNextChoices();
		}
	}

	void DisplayCommentary(string text)
	{
		GameObject textbox = Instantiate(commentaryTextBoxPrefab) as GameObject;
		TextMeshProUGUI storyText = textbox.GetComponentInChildren<TextMeshProUGUI>();
		storyText.text = text;

		textbox.transform.SetParent(commentaryCanvas.transform, false);

		currentAnim = textbox.GetComponent<Animator>();
	}

	void DisplayNextChoices()
	{
		if (story.currentChoices.Count > 0)
		{
			for (int i = 0; i < story.currentChoices.Count; i++)
			{
				Choice choice = story.currentChoices[i];
				Button button = CreateChoiceView(choice.text.Trim());
				// Tell the button what to do when we press it
				button.onClick.AddListener(delegate {
					OnClickChoiceButton(choice);
				});
			}
		}
	}

	void EndChat()
	{
		GameManager.Instance.StopTalkingToNPC(gameObject);
	}
	
	string GetClipName(Animator m_Animator)
	{
		//Fetch the current Animation clip information for the base layer
		AnimatorClipInfo[] m_CurrentClipInfo = m_Animator.GetCurrentAnimatorClipInfo(0);
		//Access the Animation clip name

		if (m_CurrentClipInfo.Length >= 1)
		{
			return m_CurrentClipInfo[0].clip.name;
		}
		return "";
	}

	// Creates a textbox showing the the line of text
	void CreateContentView (string text) {

		Text storyText = Instantiate (textPrefab) as Text;
		storyText.text = text;

		GameObject textbox = Instantiate(textBoxPrefab) as GameObject;
		textbox.transform.SetParent(dialogueCanvas.transform, false);
		
		storyText.transform.SetParent (textbox.transform, false);
		
		// Add to array
		AddToDialogueArray(ref textbox);

		currentAnim = textbox.GetComponent<Animator>();
	}

	void AddToDialogueArray(ref GameObject go)
	{
		currentDialogueBoxes.Add(go);
		
		float x = isPlayerTalking ? -100f : 100f;
		go.transform.localPosition = new Vector3(x, -100, 0); ;
		
		// If not already empty
		if (currentDialogueBoxes.Count > 0)
		{
			// Add dialogue box and 0,0 and move all other boxes up
			for (int i = 0; i < currentDialogueBoxes.Count; ++i)
			{
				StartCoroutine(MoveTextBox(currentDialogueBoxes[i]));
			}
		}

		// Only allow x textboxes on screen at once.
		if (currentDialogueBoxes.Count >= 6)
		{
			StartCoroutine(FadeTextBox(currentDialogueBoxes[0]));
		}
	}

	IEnumerator FadeTextBox(GameObject textbox)
	{
		Animator parentAnim = textbox.GetComponent<Animator>();
		parentAnim.Play("FadeOut");

		bool hasChildren = false;

		Animator childAnim = null;
		foreach (Transform child in textbox.transform)
		{
			childAnim = child.GetComponent<Animator>();
			hasChildren = true;
			childAnim.Play("FadeOut");
		}

		if (hasChildren)
		{
			yield return new WaitUntil(() => GetClipName(parentAnim) == "FadeOutConstant" && GetClipName(childAnim) == "FadeOutConstant");
		}
		else
		{
			yield return new WaitUntil(() => GetClipName(parentAnim) == "FadeOutConstant");	
		}

		Destroy(currentDialogueBoxes[0]);
		currentDialogueBoxes.RemoveAt(0);
	}
	IEnumerator MoveTextBox(GameObject go)
	{
		for (int i = 0; i < 10; i++)
		{
			if (go != null)
			{
				go.transform.localPosition += new Vector3(0, 4f, 0);
				yield return null;	
			}
			else
			{
				StopCoroutine(MoveTextBox(go));
			}
		}
	}

	// Creates a button showing the choice text
	Button CreateChoiceView (string text) {
		// Creates the button from a prefab
		Button choice = Instantiate (buttonPrefab) as Button;
		choice.transform.SetParent (choiceCanvas.transform, false);
		
		// Gets the text from the button prefab
		Text choiceText = choice.GetComponentInChildren<Text> ();
		choiceText.text = text;

		// Make the button expand to fit the text
		HorizontalLayoutGroup layoutGroup = choice.GetComponent <HorizontalLayoutGroup> ();
		layoutGroup.childForceExpandHeight = false;

		return choice;
	}

	// Destroys all the children of this gameobject (all the UI)
	void RemoveChildren(Canvas canvas)
	{
		int childCount = canvas.transform.childCount;
		
		for (int i = childCount - 1; i >= 0; --i)
		{
			if (!canvas.transform.GetChild(i).name.Contains("Canvas"))
			{
				Destroy(canvas.transform.GetChild(i).gameObject);
			}
		}

		if (canvas.name.Contains("Dialogue"))
		{
			currentDialogueBoxes.Clear();	
		}
	}
	
	void SkipDialogue()
	{
		StopAllCoroutines();
		//StopCoroutine(DisplayNextDialogue());
		StartCoroutine(DisplayNextDialogue());
	}

	// Required for the input system.
	void OnEnable()
	{
		controls.Enable();
	}

	void OnDisable()
	{
		controls.Disable();
	}
}

