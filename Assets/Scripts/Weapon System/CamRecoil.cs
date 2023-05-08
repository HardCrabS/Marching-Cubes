using UnityEngine;

public class CamRecoil : MonoBehaviour
{
	public float rotationSpeed = 6;
	public float returnSpeed = 25;
	public Vector3 RecoilRotation = new Vector3(2f, 2f, 2f);

	private Vector3 currentRotation;
	private Vector3 Rot;

    private void Start()
    {
		EventsDispatcher.Instance.onShoot += Fire;
	}

    private void FixedUpdate()
	{
		currentRotation = Vector3.Lerp(currentRotation, Vector3.zero, returnSpeed * Time.deltaTime);
		Rot = Vector3.Slerp(Rot, currentRotation, rotationSpeed * Time.fixedDeltaTime);
		transform.localRotation = Quaternion.Euler(Rot);
	}

	void Fire()
	{
		currentRotation += new Vector3(
			-RecoilRotation.x,
			Random.Range(-RecoilRotation.y, RecoilRotation.y),
			Random.Range(-RecoilRotation.z, RecoilRotation.z)
		);
	}
}