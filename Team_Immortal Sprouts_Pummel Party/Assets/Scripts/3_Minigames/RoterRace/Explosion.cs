using Cysharp.Threading.Tasks;
using UnityEngine;

public class Explosion : MonoBehaviour
{
    [SerializeField] GameObject asdf;
    [SerializeField] ParticleSystem explosion;
    [SerializeField] Transform spawnPosition;
    // Start is called before the first frame update
    private void Awake()
    {
        explosion = asdf.GetComponent<ParticleSystem>();
    }

    private void OnCollisionEnter(Collision collision)
    {
        
        gameObject.SetActive(false);
        asdf.transform.position = transform.position;
        explosion.Play();

        

        
        Spawn().Forget();
    }

    private async UniTaskVoid Spawn()
    {
        await UniTask.Delay(3000);
        transform.position = spawnPosition.position;
        gameObject.SetActive(true);
    }
}
