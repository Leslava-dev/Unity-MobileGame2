namespace Dreamteck.Forever
{
    using UnityEngine;

    public class Gem : MonoBehaviour
    {
        private void OnTriggerEnter(Collider other)
        {
            HoverPlayer player = other.GetComponentInParent<HoverPlayer>();
            if (player == null) return;
            Destroy(gameObject);
            //Add score
        }
    }
}
