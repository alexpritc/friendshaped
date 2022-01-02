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

    [SerializeField]
    private TextAsset inkJSONAsset = null;
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
	[SerializeField]
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

			// Display the text on screen!
			if (story.currentTags.Count != 0)
			{
				isCommentary = story.currentTags.Contains("Commentary") ? true : false;
				isPlayerTalking = story.currentTags.Contains("Player") ? true : false;
			}

			if (isCommentary)
			{
				DisplayCommentary(text);
			}
			else
			{
				CreateContentView(text);
			}

			if (story.canContinue)
			{
				yield return new WaitForSeconds(waitTime);
			}
			else
			{
				// If there's no more dialogue, don't bother waiting to display the buttons.
				yield return new WaitUntil(() => GetClipName(currentAnim) == "ConstantDialogue" || GetClipName(currentAnim) == "ConstantCommentaryText");
			}
		}

		if (!story.canContinue)
		{
			DisplayNextChoices();
		}
	}

	void DisplayCommentary(string text)
	{
		TextMeshProUGUI storyText = Instantiate(commentaryTextPrefab) as TextMeshProUGUI;
		storyText.text = text;

		storyText.transform.SetParent(commentaryCanvas.transform, false);

		currentAnim = storyText.GetComponent<Animator>();
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
		// TODO: else close the dialogue window
	}

	string GetClipName(Animator m_Animator)
	{
		//Fetch the current Animation clip information for the base layer
		AnimatorClipInfo[] m_CurrentClipInfo = m_Animator.GetCurrentAnimatorClipInfo(0);
		//Access the Animation clip name
		return  m_CurrentClipInfo[0].clip.name;
	}

	// Creates a textbox showing the the line of text
	void CreateContentView (string text) {

		Text storyText = Instantiate (textPrefab) as Text;
		storyText.text = text;

		GameObject textbox = Instantiate(textBoxPrefab) as GameObject;
		textbox.transform.SetParent(dialogueCanvas.transform, false);

		//textbox.GetComponentInChildren<Text>().text = storyText.text;
		storyText.transform.SetParent (textbox.transform, false);
		
		// Add to array
		AddToDialogueArray(ref textbox);

		currentAnim = textbox.GetComponent<Animator>();
	}

	void AddToDialogueArray(ref GameObject go)
	{
		currentDialogueBoxes.Add(go);
		List<GameObject> temp = currentDialogueBoxes;

		// If not already empty
		if (temp.Count > 0)
		{
			// Add dialogue box and 0,0 and move all other boxes up
			for (int i = 0; i < temp.Count; ++i)
			{
				// TODO: Make this a smoother transition i.e swipe up or fade in/fade out
				// Coroutine to change pos 
				/*
				 * IEnumerator Fade()
				{
					Color c = renderer.material.color;
					for (float alpha = 1f; alpha >= 0; alpha -= 0.1f)
					{
						c.a = alpha;
						renderer.material.color = c;
						yield return null;
					}
				}
				 */
				temp[i].transform.localPosition += new Vector3(0, 40f, 0);
			}
		}

		// Only allow x textboxes on screen at once.
		if (temp.Count >= 7)
		{
			// TODO: Play exit animation first
			Destroy(currentDialogueBoxes[0]);
			currentDialogueBoxes.RemoveAt(0);
		}

		float x = isPlayerTalking ? -100f : 100f;
		go.transform.localPosition = new Vector3(x, -50, 0); ;
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
				GameObject.Destroy(canvas.transform.GetChild(i).gameObject);
			}
		}

		currentDialogueBoxes.Clear();
	}

	void SkipDialogue()
	{
		StopAllCoroutines();
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

