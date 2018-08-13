namespace PhaseShift.Managers
{
	using UnityEngine;
	using UnityEngine.UI;
	using UnityEngine.SceneManagement;

	public class GameOver : MonoBehaviour {

		[SerializeField]
		private Image gameOverImage;
		[SerializeField] 
		private string menu;
		[SerializeField]
        private Collider col;
		
		private bool isGameOver;
		private float lerpTimer = 0f;
		private float lerpTime = 2f;

		void Start()
		{
			gameOverImage.enabled = false;
		}

		// Update is called once per frame
		void Update ()
		{
			if (isGameOver)
			{
				gameOverImage.transform.localScale = Vector3.Lerp(Vector3.one, Vector3.one * 3f, lerpTimer / lerpTime);
				lerpTimer += Time.deltaTime;
				if (lerpTimer >= lerpTime + 1f)
				{
					SceneManager.LoadScene(menu);
				}
			}
		}

		private void OnTriggerEnter(Collider other)
        {
            if (other.gameObject.tag == Util.Enums.Tags.Player.ToString())
            {
				gameOverImage.enabled = true;
                this.isGameOver = true;
            }
        }
    }
}
