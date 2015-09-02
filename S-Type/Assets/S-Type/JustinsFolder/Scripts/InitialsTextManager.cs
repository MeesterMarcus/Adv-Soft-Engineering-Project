using UnityEngine;
using UnityEngine.UI;
using System.Collections;

public class InitialsTextManager : MonoBehaviour {

   public AudioClip initialEntered; // Clip for when the player enters an initial.
   public AudioClip inputFinished;  // Clip for when the player finishes typing their initials.
   [HideInInspector] public static bool finalInputValid = false;

   AudioSource audioSource;
   Animator anim;
   InputField inputField;
   GameObject inputCaret;
   void OnEnable() {
      inputField = GetComponent<InputField>();
      audioSource = GetComponent<AudioSource>();
      anim = transform.root.GetComponent<Animator>();
      StartCoroutine(SelectInputField());
   }

   IEnumerator SelectInputField() {
      yield return new WaitForSeconds(.5f);
      inputCaret = transform.FindChild("InitialsInputField Input Caret").gameObject;
      inputCaret.SetActive(false);
      inputField.ActivateInputField();
      inputField.Select();
   }

   /* Can be called to correct caret's location. */
   public void CorrectCaret() {
      //inputCaret = transform.FindChild("InitialsInputField Input Caret").gameObject.GetComponent<RectTransform>();
      //inputCaret.pivot = new Vector2(.5f, .4f);
   }

   public void EnforceCapitalization() {
      if(inputField.text != null) {
        inputField.text = inputField.text.ToUpper(); // Capitalize all text in the input field.
      }
      audioSource.clip = initialEntered;
      audioSource.Play();
   }

   public void VerifyFinishedInput() {
      if(inputField.text.Length == inputField.characterLimit) {
         finalInputValid = true;
         StatisticsManager.SetInitials(inputField.text);
         inputField.DeactivateInputField();
         audioSource.clip = inputFinished;
         audioSource.Play();
         anim.SetTrigger("transitionOut");
      }
   }
}