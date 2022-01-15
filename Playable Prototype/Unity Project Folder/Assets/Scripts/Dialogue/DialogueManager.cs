using UnityEngine;
using UnityEngine.UI;
using System;
using Ink.Runtime;
using System.Collections;
using System.Collections.Generic;
using TMPro;

// This is a super bare bones example of how to play and display a ink story in Unity.
public class DialogueManager : MonoBehaviour
{
	public static event Action<Story> OnCreateStory;

	public TextAsset inkJSONAsset = null;
	public Story story;

	[SerializeField] private Canvas dialogueCanvas = null;
	[SerializeField] private Canvas choiceCanvas = null;
	[SerializeField] private Canvas commentaryCanvas = null;

	// UI Prefabs
	[SerializeField] private Text textPrefab = null;
	[SerializeField] private GameObject textBoxPrefab = null;
	[SerializeField] private Button buttonPrefab = null;

	[SerializeField] private GameObject commentaryTextBoxPrefab = null;
	private TextMeshProUGUI commentaryTextPrefab = null;

	[SerializeField] float waitTime = 2f;

	private Animator currentAnim = null;

	List<GameObject> currentDialogueBoxes;

	private bool isCommentary;
	private bool isPlayerTalking;

	private PlayerControls controls;

	private bool clickedButton;

	[SerializeField] private int maxCharactersPerDialogue = 30;
	[SerializeField] private int maxCharactersPerCommentary = 100;
	[SerializeField] private int maxCharactersPerButton = 80;

	[SerializeField] private int dialogueBoxesLimit = 3;

	public Image windowBackground;
	public Image chatSpriteA;
	public Image chatSpriteB;

	private bool skip;
	private bool coroutineRunning = false;

	void Awake()
	{
		controls = new PlayerControls();
		currentDialogueBoxes = new List<GameObject>();

		// Remove the default message
		RemoveChildren(dialogueCanvas);
		RemoveChildren(choiceCanvas);
		RemoveChildren(commentaryCanvas);

		controls.interactions.click.performed += ctx => SkipDialogue();

		StartStory();
	}

	public void SetImages(Sprite bg, Sprite a, Sprite b)
	{
		windowBackground.sprite = bg;
		chatSpriteA.sprite = a;
		chatSpriteB.sprite = b;
	}

	public void EndAllCoroutinesEarly()
	{
		StopAllCoroutines();
	}

	// Creates a new Story object with the compiled story which we can then play!
	void StartStory()
	{
		story = new Story(inkJSONAsset.text);
		if (OnCreateStory != null) OnCreateStory(story);

		RefreshView();
	}

	// This is the main function called every time the story changes. It does a few things:
	// Destroys all the old content and choices.
	// Continues over all the lines of text, then displays all the choices. If there are no choices, the story is finished!
	public void RefreshView()
	{
		// Remove all the UI on screen
		RemoveChildren(dialogueCanvas);
		RemoveChildren(choiceCanvas);
		RemoveChildren(commentaryCanvas);

		clickedButton = false;

		// Read all the content until we can't continue any more
		StartCoroutine(DisplayNextDialogue());
	}

	// When we click the choice button, tell the story to choose that choice!
	void OnClickChoiceButton(Choice choice)
	{
		GameManager.Instance.Action();

		story.ChooseChoiceIndex(choice.index);

		// TODO: if choice is tagged #end close window
		if (String.Compare(choice.text.ToLower(), "Walk Away".ToLower(), StringComparison.Ordinal) == 0)
		{
			EndChat();
		}
		else
		{
			RefreshView();
		}
	}

	void CheckObjectives(string text)
	{
		if (text == "...Is that a cat?")
		{
			GameManager.foundCat = true;
		}

		if (text == "I'm the stowaway.")
		{
			GameManager.foundVictim = true;
		}

		if (text == "I TAKE NAPS IN THE STAFF ROOM OKAY!" || text == "Had a shameful nap")
		{
			GameManager.foundNapper = true;
		}

		if (text == "Why, just last night the staff forgot to bring me my night-time tea!" ||
		    text == "Ladies do like tea.")
		{
			GameManager.foundTeaDrinker = true;
		}
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

			Debug.Log("text.trim():" + text);

			if (story.currentTags.Count != 0)
			{
				isCommentary = story.currentTags.Contains("Commentary") ? true : false;
				isPlayerTalking = story.currentTags.Contains("Player") ? true : false;
			}

			if (text != "" && text != null)
			{
				CheckObjectives(text);

				// Display the text on screen!
				if (isCommentary)
				{
					RemoveChildren(commentaryCanvas);
					yield return new WaitUntil(() => commentaryCanvas.transform.childCount == 0);
					CreateCommentaryView(text);
				}
				else
				{
					CreateContentView(text);
				}

				if (story.canContinue)
				{
					if (isCommentary)
					{
						yield return new WaitForSeconds(waitTime * 2f);
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
					yield return new WaitUntil(() =>
						GetClipName(currentAnim) == "ConstantDialogue" ||
						GetClipName(currentAnim) == "ConstantCommentaryText");

				}
			}
		}

		if (!story.canContinue && !clickedButton)
		{
			clickedButton = true;
			DisplayNextChoices();
		}
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
				button.onClick.AddListener(delegate { OnClickChoiceButton(choice); });
			}
		}
	}

	void EndChat()
	{
		EndAllCoroutinesEarly();
		GameManager.Instance.StopTalkingToNPC(gameObject);
	}

	string GetClipName(Animator m_Animator)
	{
		// Fetch the current Animation clip information for the base layer
		AnimatorClipInfo[] m_CurrentClipInfo = m_Animator.GetCurrentAnimatorClipInfo(0);
		// Access the Animation clip name

		if (m_CurrentClipInfo.Length >= 1)
		{
			return m_CurrentClipInfo[0].clip.name;
		}

		return "";
	}

	// Creates a textbox showing the the line of text
	void CreateContentView(string text)
	{

		Text storyText = Instantiate(textPrefab) as Text;
		storyText.text = ManageTextFormatting(text, maxCharactersPerDialogue);

		GameObject textbox = Instantiate(textBoxPrefab) as GameObject;
		textbox.transform.SetParent(dialogueCanvas.transform, false);

		storyText.transform.SetParent(textbox.transform, false);

		// Add to array
		AddToDialogueArray(ref textbox);

		currentAnim = textbox.GetComponent<Animator>();
	}

	// Creates a button showing the choice text
	Button CreateChoiceView(string text)
	{
		// Creates the button from a prefab
		Button choice = Instantiate(buttonPrefab) as Button;
		choice.transform.SetParent(choiceCanvas.transform, false);

		// Gets the text from the button prefab
		Text choiceText = choice.GetComponentInChildren<Text>();
		choiceText.text = ManageTextFormatting(text, maxCharactersPerButton);

		// Make the button expand to fit the text
		HorizontalLayoutGroup layoutGroup = choice.GetComponent<HorizontalLayoutGroup>();
		layoutGroup.childForceExpandHeight = false;

		return choice;
	}

	void CreateCommentaryView(string text)
	{

		RemoveChildren(commentaryCanvas);

		GameObject textbox = Instantiate(commentaryTextBoxPrefab) as GameObject;
		TextMeshProUGUI storyText = textbox.GetComponentInChildren<TextMeshProUGUI>();
		storyText.text = ManageTextFormatting(text, maxCharactersPerCommentary);

		textbox.transform.SetParent(commentaryCanvas.transform, false);

		currentAnim = textbox.GetComponent<Animator>();
	}

	// needs to be recursive
	string ManageTextFormatting(string input, int charLimit)
	{
		if (DoesStringFitCriteria(input, charLimit))
		{
			return input;
		}

		string output = "";
		string temp = "";

		// Get individual lines
		List<string> lines = new List<string>();
		lines = GetLines(input);

		// Empty line - shouldn't happen in real script
		if (lines == null || lines.Count == 0)
		{
			return "";
		}

		string unformattedString = lines[lines.Count - 1];

		for (int i = 0; i < unformattedString.Length; i++)
		{
			if (i >= charLimit)
			{
				temp = "";
				for (int j = i; j > 0; j--)
				{
					if (input[j] == ' ')
					{
						temp += unformattedString.Substring(0, j);
						if (j < unformattedString.Length - 1)
						{
							int t = unformattedString.Length - j;
							temp += '\n' + unformattedString.Substring(j, t);
							break;
						}
					}
				}

				if (temp == "")
				{
					temp = unformattedString;
				}

				break;
			}

			if (temp == "")
			{
				temp = unformattedString;
			}

		}

		lines[lines.Count - 1] = temp;

		foreach (var line in lines)
		{
			output += line + '\n';
		}

		// get rid of the last \n
		output = output.Substring(0, output.Length - 1);

		return output;
	}

	List<string> GetLines(string input)
	{
		List<string> lines = new List<string>();

		string line = "";

		for (int i = 0; i < input.Length; i++)
		{
			if (input[i] != '\n')
			{
				line += input[i];

				// If end of line
				if (i >= input.Length - 1)
				{
					lines.Add(line);
					line = "";
				}
			}
			else
			{
				lines.Add(line);
				line = "";
			}
		}

		return lines;
	}

	bool DoesStringFitCriteria(string input, int charLimit)
	{
		bool isFormatted = true;

		List<string> lines = GetLines(input);

		foreach (var line in lines)
		{
			if (line.Length >= charLimit)
			{
				isFormatted = false;
			}
		}

		return isFormatted;
	}

	void AddToDialogueArray(ref GameObject go)
	{
		currentDialogueBoxes.Add(go);

		float x = isPlayerTalking ? -110f : 110f;
		go.transform.localPosition = new Vector3(x, -150, 0);
		;

		// If not already empty
		if (currentDialogueBoxes.Count > 0)
		{
			// Add dialogue box and 0,0 and move all other boxes up
			for (int i = 0; i < currentDialogueBoxes.Count; ++i)
			{
				if (skip)
				{
					currentDialogueBoxes[i].transform.localPosition += new Vector3(0, 60f, 0);
				}
				else
				{
					StartCoroutine(MoveTextBox(currentDialogueBoxes[i]));
				}
			}
		}

		// Only allow x textboxes on screen at once.
		if (currentDialogueBoxes.Count >= dialogueBoxesLimit)
		{
			StartCoroutine(FadeTextBox(currentDialogueBoxes[0]));
		}
	}

	IEnumerator FadeTextBox(GameObject textbox)
	{
		Animator parentAnim = textbox.GetComponent<Animator>();

		bool hasChildren = false;

		Animator childAnim = null;
		foreach (Transform child in textbox.transform)
		{
			childAnim = child.GetComponent<Animator>();
			hasChildren = true;
			if (skip)
			{
				childAnim.Play("FadeOutConstant");
			}
			else
			{
				childAnim.Play("FadeOut");
			}
		}

		if (skip)
		{
			parentAnim.Play("FadeOutConstant");
			StopCoroutine(FadeTextBox(textbox));
		}
		else
		{
			parentAnim.Play("FadeOut");
		}
		
		if (hasChildren)
		{
			yield return new WaitUntil(() =>
				GetClipName(parentAnim) == "FadeOutConstant" && GetClipName(childAnim) == "FadeOutConstant");
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
		for (int i = 0; i < 15; i++)
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
		if (!skip)
		{
			skip = true;
			
			// Stop all except moving ones
			StopAllCoroutines();

			// Clear commentary
			if (story.canContinue)
			{
				RemoveChildren(commentaryCanvas);
			}

			if (currentDialogueBoxes != null && currentDialogueBoxes.Count > 0)
			{
				if (currentDialogueBoxes.Count >= dialogueBoxesLimit)
				{
					// Only allow x textboxes on screen at once.
					Destroy(currentDialogueBoxes[0]);
					currentDialogueBoxes.RemoveAt(0);
				}
			}

			StartCoroutine(DisplayNextDialogue());
			StartCoroutine(ResetSkip());
		}
	}

	IEnumerator ResetSkip()
	{
		yield return new WaitForSeconds(0.1f);
		skip = false;
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