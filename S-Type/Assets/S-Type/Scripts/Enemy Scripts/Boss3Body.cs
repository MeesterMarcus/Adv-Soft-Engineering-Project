using UnityEngine;
using System.Collections;

public class Boss3Body : MonoBehaviour {

   [SerializeField] private float switchDelay;
   private SpriteRenderer mySpriteRdr;
   private Boss3 image;

   private void Start() {
      if ((mySpriteRdr = GetComponentInParent<SpriteRenderer> ()) == null) {
         Debug.LogError("No sprite renderer");
         enabled = false;
         return;
      }
      if (transform.parent == null) {
         Debug.LogError("No parent");
         enabled = false;
         return;
      }
      if ((image = GetComponentInParent<Boss3> ()) == null) {
         Debug.LogError("No image");
         enabled = false;
         return;
      }
   }

   public void ApplyDamage(float damage) {
      if (image.asleep == false && mySpriteRdr.isVisible) {
         image.SwitchBodies ();
         StartCoroutine(DamageBlink());
      }
   }

   public virtual IEnumerator DamageBlink() {
      mySpriteRdr.color = new Color (mySpriteRdr.color.r, mySpriteRdr.color.g, mySpriteRdr.color.b, 0.0f);
      yield return new WaitForSeconds (0.05f);
      mySpriteRdr.color = new Color (mySpriteRdr.color.r, mySpriteRdr.color.g, mySpriteRdr.color.b, 1.0f);
   }

   public virtual void OnTriggerEnter2D(Collider2D other) { 
      if (other.CompareTag("Player")) other.SendMessageUpwards ("ApplyDamage", 1, SendMessageOptions.DontRequireReceiver); 
   }
}
